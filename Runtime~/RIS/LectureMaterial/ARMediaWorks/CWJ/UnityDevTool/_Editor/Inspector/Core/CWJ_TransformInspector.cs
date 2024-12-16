using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using CWJ.AccessibleEditor;

namespace CWJ.EditorOnly.Inspector
{
    [CustomEditor(typeof(Transform)), CanEditMultipleObjects]
    public class CWJ_TransformInspector : Editor
    {
        #region ReflectionKeyword

        private const string TypeName_TransformInspector = "TransformInspector";
        private const string FieldName_m_RotationGUI = "m_RotationGUI";
        private const string MethodName_RotationField = "RotationField";

        private const string FieldName_localPosition = "m_LocalPosition";
        private const string FieldName_localScale = "m_LocalScale";

        #endregion ReflectionKeyword

        public enum SpaceType
        {
            Local,
            World
        }

        private Transform targetTrf;

        private SpaceType spaceType;

        //position
        SerializedProperty prop_localPosition;
        //rotation
        Editor builtInEditor;
        private object rotationDrawer;
        private MethodInfo rotationFieldMethod;
        private readonly object[] rotationFieldParam = new object[0];
        //scale
        SerializedProperty prop_localScale;

        private void OnEnable()
        {
            builtInEditor = CreateEditor(targets, ReflectionUtil.GetUnityEditorClassType(TypeName_TransformInspector));
            if (builtInEditor == null) return;
            rotationDrawer = builtInEditor.GetType().GetField(FieldName_m_RotationGUI, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(builtInEditor);
            rotationFieldMethod = rotationDrawer.GetType().GetMethod(MethodName_RotationField, new Type[0]);

            prop_localPosition = serializedObject.FindProperty(FieldName_localPosition);
            prop_localScale = serializedObject.FindProperty(FieldName_localScale);
            
        }

        private void OnDisable()
        {
            if (builtInEditor == null) return;
            DestroyImmediate(builtInEditor);
        }

        public override void OnInspectorGUI()
        {
            if (builtInEditor == null) return;
            targetTrf = target as Transform;

            if (!EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = true;
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
            }

            serializedObject.Update();

            EditorGUI_CWJ_TransformExtension.DrawSibilingIndex(targets, targetTrf, ref lastSiblingTxt);

            DrawInspector3D();

            Vector3 pos = targetTrf.position;
            if (Mathf.Abs(pos.x) > 100000 || Mathf.Abs(pos.y) > 100000 || Mathf.Abs(pos.z) > 100000)
                EditorGUILayout.HelpBox(WarningOfFloatingPoint, MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
            
            // ================================================================
            // EzPIVOT
            var selectedTransforms = EzPivot.EditorAPI.GetSelectedTransforms();
            EditorGUILayout.Separator();
            EzPivot.EditorAPI.Instance.DrawGUI(selectedTransforms);
            // ================================================================
        }

        string lastSiblingTxt = null;

        private void DrawInspector3D()
        {
            EditorGUILayout.BeginHorizontal(EditorGUICustomStyle.Box);

            using (var changeScope= new EditorGUI.ChangeCheckScope())
            {
                if (GUILayout.Button("<", GUILayout.MaxWidth(20), GUILayout.MinWidth(0)))
                {
                    spaceType = EnumUtil.PreviousEnum(spaceType);
                }
                GUILayout.Label(spaceType.ToString() + " Space", EditorGUICustomStyle.LargeBoldLabelStyles, GUILayout.ExpandHeight(false));
                if (GUILayout.Button(">", GUILayout.MaxWidth(20), GUILayout.MinWidth(0)))
                {
                    spaceType = EnumUtil.NextEnum(spaceType);
                }
                if (changeScope.changed)
                {
                    EditorGUI_CWJ.RemoveFocusFromText();
                    return;
                }
            }

            EditorGUILayout.EndHorizontal();

            if (spaceType == SpaceType.Local)
                DrawLocalSpace();
            else if (spaceType == SpaceType.World)
                DrawWorldSpace();
        }

        private void DrawLocalSpace()
        {
            EditorGUILayout.PropertyField(prop_localPosition, LocalContent.positionContent);
            rotationFieldMethod.Invoke(rotationDrawer, rotationFieldParam); //DrawLocalRotationField
#if UNITY_2019_3_OR_NEWER
            EditorGUILayout.Space(0.25f);
#endif
            EditorGUILayout.PropertyField(prop_localScale, LocalContent.scaleContent);
        }

        private void DrawWorldSpace()
        {
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                var worldPos = EditorGUILayout.Vector3Field(WorldContent.positionContent, GetFloatingSafeVector(targetTrf.position));
                if (changeScope.changed)
                {
                    Undo.RecordObjects(targets, "Inspector " + nameof(worldPos));
                    foreach (Transform tr in targets)
                    {
                        tr.position = worldPos;
                    }
                }
            }

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.Vector3Field(WorldContent.rotationContent, GetFloatingSafeVector(GetWorldSpaceRot(targetTrf)));
            }

            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                Vector3 lossyScale = EditorGUILayout.Vector3Field(WorldContent.scaleContent, GetFloatingSafeVector(targetTrf.lossyScale));
                if (changeScope.changed)
                {
                    Undo.RecordObjects(targets, "Inspector " + nameof(lossyScale));
                    foreach (Transform tr in targets)
                    {
                        tr.localScale = TransformUtil.LossyToLocalScale(tr, lossyScale);
                    }
                }
            }

#if UNITY_2019_3_OR_NEWER
            EditorGUILayout.Space(0.25f);
#endif
        }

        private float GetFloatingSafeNumber(float num)
        {
            int intNum = Mathf.RoundToInt(num);
            if (Mathf.Approximately(intNum, num))
            {
                return intNum;
            }

            return num;
        }

        private Vector3 GetFloatingSafeVector(Vector3 v)
        {
            return new Vector3(GetFloatingSafeNumber(v.x), GetFloatingSafeNumber(v.y), GetFloatingSafeNumber(v.z));
        }

        private Vector3 GetWorldSpaceRot(Transform transform)
        {
            Vector3 angle = transform.eulerAngles;
            bool isUp = Vector3.Dot(transform.up, Vector3.up) >= 0f;
            float x = angle.x;
            if (x >= 0f && x <= 90f)
            {
                if (!isUp) x = 180 - x;
            }
            else if (x >= 270f && x <= 360f)
            {
                x = !isUp ? (180 - x) : (x - 360f);
            }

            float y = angle.y;
            if (y > 180)
            {
                y -= 360f;
            }

            float z = angle.z;
            if (z > 180)
            {
                z -= 360f;
            }

            return new Vector3(x, y, z);
        }


        //private void DrawRotField()
        //{
        //    EditorGUI.BeginChangeCheck();
        //    var rot = EditorGUILayout.Vector3Field("Rotation", TransformUtils.GetInspectorRotation(targetTrf));
        //    if (EditorGUI.EndChangeCheck())
        //    {
        //        Undo.RecordObjects(targets, "Inspector");
        //        foreach (Transform tr in targets)
        //        {
        //            TransformUtils.SetInspectorRotation(tr, rot);
        //            if (tr.parent != null)
        //            {
        //                tr.localScale = tr.localScale;
        //            }
        //        }
        //    }
        //}



        #region Content


        class LocationGUIContents
        {
            public GUIContent positionContent;
            public GUIContent rotationContent;
            public GUIContent scaleContent;

            public LocationGUIContents(bool isLocal = true)
            {
                this.positionContent = new GUIContent(EditorGUIUtility.TrTextContent("Position"));
                this.rotationContent = new GUIContent(EditorGUIUtility.TrTextContent("Rotation"));
                this.scaleContent = new GUIContent(EditorGUIUtility.TrTextContent("Scale"));
                if (isLocal)
                {
                    this.positionContent.tooltip = "The local position of this GameObject relative to the parent.";
                    this.rotationContent.tooltip = "The local rotation of this Game Object relative to the parent.";
                    this.scaleContent.tooltip = "The local scaling of this GameObject relative to the parent.";
                }
                else
                {
                    this.positionContent.tooltip = "The world position of this GameObject.";
                    this.rotationContent.tooltip = "The world rotation of this Game Object.";
                    this.scaleContent.tooltip = "The world scaling of this GameObject. \n(same of transform.lossyscale.)\nBe careful to change the lossyscale value";
                }
            }
        }

        static LocationGUIContents _LocalContents = null;
        static LocationGUIContents LocalContent
        {
            get
            {
                if (_LocalContents == null)
                {
                    _LocalContents = new LocationGUIContents(true);
                }
                return _LocalContents;
            }
        }
        static LocationGUIContents _WorldContents = null;
        static LocationGUIContents WorldContent
        {
            get
            {
                if (_WorldContents == null)
                {
                    _WorldContents = new LocationGUIContents(false);
                }
                return _WorldContents;
            }
        }
        const string WarningOfFloatingPoint = "Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.";

        #endregion
    }
}