#if UNITY_EDITOR
#if UNITY_2019_1_OR_NEWER
#define EZPIVOT_SUPPORT_ROTATION
#endif

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EzPivot
{
    public class EditorAPI
    {
        /// <summary>
        /// Called when the user starts to change the pivot in Editor
        /// Simple function: no access to Transform being modified
        /// </summary>
        public static event System.Action onChangeBeginSimple;

        /// <summary>
        /// Called when the user starts to change the pivot in Editor
        /// Complex function: full access of all Transforms being modified
        /// </summary>
        public static event System.Action<Transform[]> onChangeBeginArray;

        /// <summary>
        /// Called when the user stops to change the pivot in Editor
        /// </summary>
        public static event System.Action onChangeEnd;

        TargetManager targetManager = new TargetManager();
        Tool m_prevTool;
        bool m_SnapActive = false;
        bool m_SnapIndependant = false;
        Vector3 m_SnapGridSize = Vector3.one;
        const string m_EditorPrefsPrefix = "EzPivot_";

        static GUIContent ms_SnapActiveContent = new GUIContent("Snapping Enabled", "Snap position to a virtual grid while moving the pivot with gizmos handlers in the scene view.");
        static GUIContent ms_SnapGridSizeContent = new GUIContent("Grid Size", "Size of the grid");
        static GUIContent ms_SnapIndependentContent = new GUIContent("Per Axis", "Control the size of the grid either using one single value, or independent values per axis");

        public event System.Action askToRepaintWindowDelegate;

        static public Transform[] GetSelectedTransforms()
        {
            return Selection.GetTransforms(SelectionMode.ExcludePrefab);
        }

        static public void SetSelectedTransform(Transform selected)
        {
            Selection.activeTransform = selected;
        }

        void AskToRepaintWindow()
        {
            askToRepaintWindowDelegate?.Invoke();
        }

        void SetTargets(Transform[] targets)
        {
            var wasEmpty = targetManager.isEmpty;
            targetManager.SetTargets(targets);

            if (targetManager.isEmpty)
            {
                if (!wasEmpty)
                    if (onChangeEnd != null) onChangeEnd();
            }
            else
            {
                onChangeBeginSimple?.Invoke();
                onChangeBeginArray?.Invoke(targetManager.GetTransformTargets());
            }
        }

        void ResetTargets()
        {
            SetTargets(null);
        }

        public static Transform WrapTransform(Transform towrap)
        {
            var undoName = "Wrap GameObject";

            var wrapper = new GameObject(towrap.name + " (wrapper)").transform;
            UnityEditor.Undo.RegisterCreatedObjectUndo(wrapper.gameObject, undoName);

            wrapper.position = towrap.position; // the new pivot of the wrapper has the same position
            wrapper.rotation = towrap.rotation; // and rotation than the wrapped object

            var siblingIndex = towrap.GetSiblingIndex();
            UnityEditor.Undo.SetTransformParent(wrapper, towrap.parent, undoName);

            wrapper.localScale = Vector3.one; // we want the wrapper to be unscaled so we can change its pivot rotation after wrapping 

            UnityEditor.Undo.SetTransformParent(towrap, wrapper, undoName);
            wrapper.SetSiblingIndex(siblingIndex);

            return wrapper;
        }

        void WrapSelectedTransforms()
        {
            var selected = GetSelectedTransforms();

            for (int i = 0; i < selected.Length; ++i)
            {
                selected[i] = WrapTransform(selected[i]);
            }

            SetSelectedTransform(selected[0]);
        }

#if EZPIVOT_SUPPORT_ROTATION
        // Quaternions used to properly handle local and global rotation handles
        Quaternion m_GlobalGizmoQuat = Quaternion.identity;
        Quaternion m_GlobalGizmoQuatOnGizmoInteract = Quaternion.identity;
        Quaternion m_TargetQuatOnGizmoInteract = Quaternion.identity;

        void InitCachedQuaternions()
        {
            m_GlobalGizmoQuat = Quaternion.identity;
            m_GlobalGizmoQuatOnGizmoInteract = Quaternion.identity;
            m_TargetQuatOnGizmoInteract = Quaternion.identity;
        }
#endif

        public bool StartMove()
        {
            StopMove();
            StartListeningSceneGUI();

#if EZPIVOT_SUPPORT_ROTATION
            InitCachedQuaternions();
#endif

            SetTargets(GetSelectedTransforms());
            if (!targetManager.isEmpty)
            {
                m_prevTool = Tools.current;
                Tools.current = Tool.None;
                SceneView.RepaintAll();
            }
            AskToRepaintWindow();
            return !targetManager.isEmpty;
        }

        public void StopMove()
        {
            StopListeningSceneGUI();
            if (!targetManager.isEmpty)
                Tools.current = m_prevTool;

            ResetTargets();
            AskToRepaintWindow();
        }

        void OnSelectionChanged()
        {
            StopMove();
        }

        public void DrawGUI(Transform[] selectedTransforms)
        {
            if (selectedTransforms.Length == 0)
                return;

            if (targetManager.isEmpty)
            {
                bool showMoveButton = API.HasAtLeastOneTransformWithResult(selectedTransforms, API.CanChangePivotResult.Yes, API.CanChangePivotResult.NoButShowMoveButton);

                if (showMoveButton)
                {
                    if (GUILayout.Button("Move/Rotate Pivot"))
                        StartMove();
                }
                else
                {
                    if (GUILayout.Button("Wrap into empty GameObject"))
                        WrapSelectedTransforms();
                }

                if (!string.IsNullOrEmpty(targetManager.errorMsg))
                    EditorGUILayout.HelpBox(targetManager.errorMsg, MessageType.Error);
            }
            else
            {
                if (GUILayout.Button("Stop Changing Pivot"))
                {
                    Tools.current = m_prevTool;
                    ResetTargets();
                    SceneView.RepaintAll();
                }

                if (Tools.pivotMode == PivotMode.Center)
                {
                    EditorGUILayout.Separator();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.HelpBox("Be careful, the Unity tool handle is in 'Center' mode, which may be confusing.\nWe recommend you to work in 'Pivot' mode instead.", MessageType.Warning, true);
                        if (GUILayout.Button("Switch to\n'Pivot' mode"))
                            Tools.pivotMode = PivotMode.Pivot;
                    }
                    EditorGUILayout.Separator();
                }

                if (!targetManager.isEmpty)
                {
                    GUISelection();
                    GUIPrefabWarnings();

                    if (targetManager.isUnique)
                    {
                        var uniqueTarget = targetManager.uniqueTarget;
                        uniqueTarget.GUIPositions();
                        uniqueTarget.GUIRotations();
                        uniqueTarget.GUIChildren();
                    }

                    targetManager.GUIBounds();

                    if (targetManager.isUnique)
                    {
                        GUISceneView();
                    }
                }
            }
        }

        void GUISelection()
        {
            string header = "";
            if (targetManager.isUnique)
                header = string.Format("Currently changing '{0}\' pivot", targetManager.uniqueTarget.name);
            else
                header = string.Format("Currently changing pivot of {0} objects: ", targetManager.count);
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);

            if (!targetManager.isUnique)
            {
                const int kMax = 20;
                GUI.enabled = false;
                string names = "";
                int count = 0;
                foreach (var target in targetManager.EveryTarget())
                {
                    if (count >= kMax)
                    {
                        names += "...";
                        break;
                    }

                    names += target.name;
                    if (count < targetManager.count - 1) names += ", ";
                    count++;
                }

                var oldWordWrap = EditorStyles.textField.wordWrap;
                EditorStyles.textField.wordWrap = true;
                EditorGUILayout.TextArea(names);
                EditorStyles.textField.wordWrap = oldWordWrap;
                GUI.enabled = true;
            }

            EditorGUILayout.Separator();
        }

        void GUIPrefabWarnings()
        {
            foreach (var target in targetManager.EveryTarget())
            {
                var transf = target.transform;

#if UNITY_2020_1_OR_NEWER
                bool isInPrefabIsolation = UnityEditor.SceneManagement.StageUtility.GetStage(transf.gameObject) != UnityEditor.SceneManagement.StageUtility.GetMainStage();
#elif UNITY_2018_3_OR_NEWER
                bool isInPrefabIsolation = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null;
#else
                bool isInPrefabIsolation = PrefabUtility.GetPrefabType(transf) != PrefabType.None;
#endif
                bool isRoot = transf.parent == null;

                if (isInPrefabIsolation && isRoot)
                {
                    EditorGUILayout.HelpBox("Modifying the pivot of the root GameObject of a prefab in isolation mode will modify the position/rotation of the instances.\nYou should probably modify the pivot of one of its children instead.", MessageType.Warning, true);
                }
            }
        }

        static Vector3 GetEditorPrefsVector3(string key, Vector3 def)
        {
            return new Vector3(
                EditorPrefs.GetFloat(key + "X", def.x),
                EditorPrefs.GetFloat(key + "Y", def.y),
                EditorPrefs.GetFloat(key + "Z", def.z));
        }

        static void SetEditorPrefsVector3(string key, Vector3 value)
        {
            EditorPrefs.SetFloat(key + "X", value.x);
            EditorPrefs.SetFloat(key + "Y", value.y);
            EditorPrefs.SetFloat(key + "Z", value.z);
        }

        void GUISceneView()
        {
            if (FoldableHeader.Begin("Scene view"))
            {
                EditorGUIUtility.labelWidth = 120f;

                {
                    const string kKey = m_EditorPrefsPrefix + "SnapToggle";
                    var currentValue = EditorPrefs.GetBool(kKey, m_SnapActive);
                    m_SnapActive = EditorGUILayout.Toggle(ms_SnapActiveContent, currentValue);
                    if (m_SnapActive != currentValue)
                        EditorPrefs.SetBool(kKey, m_SnapActive);
                }

                if (m_SnapActive)
                {
                    {
                        const string kKey = m_EditorPrefsPrefix + "SnapIndependent";
                        var currentValue = EditorPrefs.GetBool(kKey, m_SnapActive);
                        m_SnapIndependant = EditorGUILayout.Toggle(ms_SnapIndependentContent, currentValue);
                        if (m_SnapIndependant != currentValue)
                            EditorPrefs.SetBool(kKey, m_SnapIndependant);
                    }

                    {
                        const string kKey = m_EditorPrefsPrefix + "SnapSize";
                        var currentValue = GetEditorPrefsVector3(kKey, m_SnapGridSize);

                        if (!m_SnapIndependant)
                        {
                            var vx = EditorGUILayout.FloatField(ms_SnapGridSizeContent, currentValue.x);
                            m_SnapGridSize = Vector3.one * vx;
                        }
                        else
                        {
                            m_SnapGridSize = EditorGUILayout.Vector3Field(ms_SnapGridSizeContent, currentValue);
                        }

                        if (m_SnapGridSize != currentValue)
                            SetEditorPrefsVector3(kKey, m_SnapGridSize);
                    }
                }
            }
            FoldableHeader.End();
        }

        Vector3 GetSnappedPosition(Vector3 pivotPos)
        {
            if (m_SnapActive)
                pivotPos = new Vector3(
                    Mathf.Round(pivotPos.x / m_SnapGridSize.x) * m_SnapGridSize.x,
                    Mathf.Round(pivotPos.y / m_SnapGridSize.y) * m_SnapGridSize.y,
                    Mathf.Round(pivotPos.z / m_SnapGridSize.z) * m_SnapGridSize.z);
            return pivotPos;
        }

        public void OnWindowUpdate()
        {
            if (!targetManager.isEmpty && Tools.current != Tool.None)
            {
                StopMove();
            }
        }

        void StartListeningSceneGUI()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
#endif
        }

        void StopListeningSceneGUI()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= this.OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
#endif
        }

        void OnSceneGUI(SceneView sceneView)
        {
            Handles.color = Color.yellow;
            Handles.matrix = Matrix4x4.identity;

            foreach (var target in targetManager.EveryTarget())
            {
#if UNITY_5_6_OR_NEWER
                Handles.SphereHandleCap(0, target.transform.position, Quaternion.identity, HandleUtility.GetHandleSize(target.transform.position) * 0.3f, Event.current.type);
#else
                Handles.SphereCap(0, target.transform.position, Quaternion.identity, HandleUtility.GetHandleSize(target.transform.position) * 0.3f);
#endif

                Handles_DrawWireCube(target.cachedBounds);
            }

            if (targetManager.isUnique)
            {
                var uniqueTarget = targetManager.uniqueTarget;
                if (uniqueTarget.TargetTransformHasChanged())
                    uniqueTarget.UpdateTargetCachedData();

#if EZPIVOT_SUPPORT_ROTATION
                if (Event.current.type == EventType.MouseDown) // on gizmo interaction start
                {
                    m_TargetQuatOnGizmoInteract = uniqueTarget.transform.rotation;
                    m_GlobalGizmoQuatOnGizmoInteract = m_GlobalGizmoQuat;
                }
#endif

                EditorGUI.BeginChangeCheck();

#if EZPIVOT_SUPPORT_ROTATION
                var newPos = uniqueTarget.transform.position;
                var newRot = uniqueTarget.transform.rotation;
                if (uniqueTarget.CanBeRotated())
                {
                    if (Tools.pivotRotation == PivotRotation.Global)
                    {
                        Handles.TransformHandle(ref newPos, ref m_GlobalGizmoQuat);
                        var rotDiff = m_GlobalGizmoQuat * Quaternion.Inverse(m_GlobalGizmoQuatOnGizmoInteract);
                        newRot = rotDiff * m_TargetQuatOnGizmoInteract;
                    }
                    else
                        Handles.TransformHandle(ref newPos, ref newRot);
                }
                else
                {
                    Quaternion quat = Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : newRot;
                    newPos = Handles.PositionHandle(newPos, quat);
                }
#else
                Quaternion quat = Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : uniqueTarget.transform.rotation;
                var newPos = Handles.PositionHandle(uniqueTarget.transform.position, quat);
#endif

                if (EditorGUI.EndChangeCheck())
                {
                    if (newPos != uniqueTarget.transform.position)
                        uniqueTarget.SetPivotPosition(GetSnappedPosition(newPos), API.Space.Global);

#if EZPIVOT_SUPPORT_ROTATION
                    if (newRot != uniqueTarget.transform.rotation)
                        uniqueTarget.SetPivotRotation(newRot, API.Space.Global);
#endif

                    AskToRepaintWindow(); // ask to repaint window
                }
            }
        }

        void Handles_DrawWireCube(Bounds bounds)
        {
#if UNITY_5_4_OR_NEWER
            Handles.DrawWireCube(bounds.center, bounds.size);
#else
            Vector3[] corners =
            {
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(1, 1, 1)),
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(-1, 1, 1)),
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(-1, 1, -1)),
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(1, 1, -1)),
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(1, -1, 1)),
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(-1, -1, 1)),
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(-1, -1, -1)),
                bounds.center + Vector3.Scale(bounds.extents, new Vector3(1, -1, -1))
            };

            for (int i = 0; i < 4; i++)
            {
                Handles.DrawLine(corners[i], corners[(i + 1) % 4]);
                Handles.DrawLine(corners[i], corners[i + 4]);
                Handles.DrawLine(corners[i + 4], corners[4 + (i + 1) % 4]);
            }
#endif
        }

        // Singleton
        private static readonly EditorAPI instance = new EditorAPI();
        public static EditorAPI Instance { get { return instance; } }

        EditorAPI()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            Selection.selectionChanged += OnSelectionChanged;

            EditorPrefs.SetFloat(m_EditorPrefsPrefix + "Version", API.Version);
        }
    }

    internal static class Extensions
    {
        public static void AddIfNotNull<T>(this List<T> list, T item) where T : UnityEngine.Object
        {
            if (item)
                list.Add(item);
        }
    }
}

#endif