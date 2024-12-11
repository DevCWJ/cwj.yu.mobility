using UnityEditor;
using UnityEngine;
using CWJ.AccessibleEditor;
using UnityEngine.SceneManagement;

#if UNITY_2021_3_OR_NEWER
using UnityEditor.SceneManagement;
#else
using UnityEditor.Experimental.SceneManagement;
#endif

namespace CWJ.EditorOnly.Hierarchy
{
    public enum HierarchyItemType
    {
        Scene,

        /// <summary>
        /// gameObject in the scene. OR child of root prefab in the PrefabStage 
        /// </summary>
        GameObj,
        /// <summary>
        /// Prefab instance object in the scene
        /// </summary>
        PrefabObjInScene,
    }

    public class CWJ_Hierarchy_Core
    {
        //Disabled 211117
        //[InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            //EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        public delegate void HierachyItemGUIHandler(HierarchyItemInfo itemData, Rect selectionRect, Event guiEvent);
        public static event HierachyItemGUIHandler hierarchyItemGUIEvent;


        public class HierarchyItemInfo
        {
            public readonly int instanceID;

            public HierarchyItemType itemType;
            public Scene scene;
            public readonly GameObject gameObj;

            /// <summary>
            /// 현재 PrefabStage 편집기 모드로 Scene과 Hierarchy에 사용중 인지
            /// </summary>
            public readonly bool isInPrefabStage;
            public readonly PrefabStage prefabStage;

            public HierarchyItemInfo(int instanceID)
            {
                this.instanceID = instanceID;
                gameObj = (EditorUtility.InstanceIDToObject(instanceID) as GameObject);

                if (gameObj != null)
                {
                    prefabStage= PrefabStageUtility.GetCurrentPrefabStage();
                    isInPrefabStage = prefabStage != null && prefabStage.IsPartOfPrefabContents(gameObj);
                    itemType = !isInPrefabStage && PrefabUtility.IsPartOfPrefabInstance(gameObj) ? HierarchyItemType.PrefabObjInScene : HierarchyItemType.GameObj;
                    return;
                }

                int sceneCnt = SceneManager.sceneCount;
                for (int i = 0; i < sceneCnt; i++)
                {   
                    var scene = SceneManager.GetSceneAt(i);
                    if (scene.GetHashCode() == instanceID)
                    {
                        this.scene = scene;
                        itemType = HierarchyItemType.Scene;
                        gameObj = null;
                        isInPrefabStage = false;
                        prefabStage = null;
                        return;
                    }
                }
            }
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var guiEvent = Event.current;
            if (!AccessibleEditorUtil.IsAppFocused || guiEvent == null) return;

            hierarchyItemGUIEvent?.Invoke(new HierarchyItemInfo(instanceID), selectionRect, guiEvent);
        }
    }
}