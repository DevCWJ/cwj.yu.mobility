#if UNITY_EDITOR

using CWJ.AccessibleEditor;

using UnityEditor;

using UnityEngine;

namespace CWJ.UI
{
    [CanEditMultipleObjects, CustomEditor(typeof(PolygonChart))]
    public class PolygonChart_Inspector : InspectorBehaviour<PolygonChart>
    {
        private bool prev_isLineDistMaxValue;
        private bool prev_isLineDistSyncValue;
        private bool isCenterLineSettingEnabled = false;
        private bool isVisualizeSettingEnabled = false;

        protected override void DrawInspector()
        {
            var backup_ident = 0;
            EditorGUI.indentLevel = backup_ident;

            EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
            WriteSubTitle("-Graph Setting-");

            Draw(nameof(Target.axisLength), "각 갯수");
            Draw(nameof(Target.defaultValue), "valueList 배열에 값이 없을시 정해지는 초기화 값");
            Draw(nameof(Target._raycastTarget));
            Target.raycastTarget = Target._raycastTarget;

            Draw(nameof(Target.values));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
            WriteSubTitle("-Visualize Setting-");
            Draw(nameof(Target.isValueVisualized), "값 위치에 오브젝트 생성");
            isVisualizeSettingEnabled = Target.isValueVisualized;
            if (isVisualizeSettingEnabled = EditorGUILayout.Foldout(isVisualizeSettingEnabled, "setting", true))
            {
                Draw(nameof(Target.valueOffset));

                GUI.enabled = false;
                Draw(nameof(Target.valueTrfList));
                GUI.enabled = true;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
            WriteSubTitle("-Fill Setting-");
            Draw(nameof(Target.fillAmount), "표면을 얼마나 채워넣을것인지/ 1이 최대");

            BeginError(Any(t => t.fillTexture == null));
            Draw(nameof(Target.fillTexture), "표면 텍스쳐");
            EndError();
            BeginError(Any(t => t._material == null));
            Draw(nameof(Target._material), "표면 머티리얼");
            EndError();
            Target.material = Target._material;

            Draw(nameof(Target._color), "표면 색상");
            Target.color = Target._color;
            Draw(nameof(Target.turnOffset), "회전 각도 수치");
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            //변수값에따라 숨겨지는거 다른방법. 근데 if문으로 걍 처리하는거랑 뭐가다른지 잘모르겠음
            //Draw(nameof(Target.onDrawCenterLine));
            //using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(Target.onDrawCenterLine)))
            //{
            //    if (group.visible)
            //    {
            //      //내용
            //    }
            //}
            EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
            WriteSubTitle("-Line Setting-");

            Draw(nameof(Target.onDrawCenterLine));
            isCenterLineSettingEnabled = Target.onDrawCenterLine;
            if (isCenterLineSettingEnabled = EditorGUILayout.Foldout(isCenterLineSettingEnabled, "setting", true))
            {
                Draw(nameof(Target.lineWidth));
                Draw(nameof(Target.lineColor));
                Draw(nameof(Target.lineDist));

                Draw(nameof(Target.isLineDistMaxValue));
                Draw(nameof(Target.isLineDistSyncValue));
                SwitchingToggle(ref prev_isLineDistMaxValue, ref Target.isLineDistMaxValue,
                                ref prev_isLineDistSyncValue, ref Target.isLineDistSyncValue);
            }
            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorGUICustomStyle.InspectorBox);
            WriteSubTitle("-Event-");
            Draw(nameof(Target.onValueChanged));
            Draw(nameof(Target._onCullStateChanged));
            Target.onCullStateChanged = Target._onCullStateChanged;
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel = backup_ident;
        }

        //private void InspectorSetDirty()
        //{
        //    if (GUI.changed)
        //    {
        //        Undo.RegisterCompleteObjectUndo(Target, $"PolygonChart view {Target.gameObject.name}");

        //        Target.SetAllDirty();
        //        serializedObject.ApplyModifiedProperties();
        //        //Undo.RecordObject(target, $"PolygonChart Edit ({polygonChart.gameObject.name}");

        //        if (!Application.isPlaying)
        //        {
        //            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        //        }

        //    }
        //}
    }
}

#endif