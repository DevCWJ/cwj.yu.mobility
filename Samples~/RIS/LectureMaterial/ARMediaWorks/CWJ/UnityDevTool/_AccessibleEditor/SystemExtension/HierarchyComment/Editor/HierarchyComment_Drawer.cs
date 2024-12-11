using System;

using UnityEditor;
using UnityEngine;
using CWJ.AccessibleEditor;
using UnityEngine.SceneManagement;

namespace CWJ.EditorOnly.Hierarchy.Comment
{
    using static HierarchyCommentExtension;
    public interface IHideInHierarchy
    {
        void Editor_HideFlagsDirt();
    }

    public class HierarchyComment_Drawer
    {
        //Disabled 210317
        //[InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CWJ_Hierarchy_Core.hierarchyItemGUIEvent += OnHierarchyItemGUI;

            //EditorEventSystem.EditorSceneOpenedEvent += OnEditorSceneLoaded;
            //UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnRuntimeSceneLoaded;
        }

        //private static void OnRuntimeSceneLoaded(Scene arg0, LoadSceneMode arg1)
        //{
        //    OnEditorSceneLoaded(arg0);
        //}

        //private static void OnEditorSceneLoaded(Scene obj)
        //{
        //    foreach (var item in FindUtil.FindInterfaces<IHideInHierarchy>(includeInactive: true, includeDontDestroyOnLoadObjs: true))
        //    {
        //        item.Editor_HideFlagsDirt();
        //    }
        //}

        const float ButtonSize = 18;
        const float XOffset = 30; //(15 *2)

        static GUIContent _findChildCommentBtnContent = null;
        protected static GUIContent findChildCommentBtnContent
        {
            get
            {
                if (_findChildCommentBtnContent == null)
                {
                    _findChildCommentBtnContent = new GUIContent(text: "0", tooltip: "Comment가 기록되어있는 자식의 수\n클릭하면 expand하여 보여줌");
                }

                return _findChildCommentBtnContent;
            }
        }


        static GUIContent _commentBtnContent = null;
        protected static GUIContent commentBtnContent
        {
            get
            {
                if (_commentBtnContent == null)
                {
                    _commentBtnContent = new GUIContent(EditorGUIUtility.IconContent("CollabEdit Icon"));
                    _commentBtnContent.tooltip = "오브젝트 주석 편집";
                }

                return _commentBtnContent;
            }
        }

        static GUIContent _commentTooltipContent = null;
        protected static GUIContent commentTooltipContent
        {
            get
            {
                if (_commentTooltipContent == null) _commentTooltipContent = new GUIContent(text: string.Empty, tooltip: string.Empty);
                return _commentTooltipContent;
            }
        }

        private static HierarchyCommentCache curSceneCache = null;
        private static HierarchyCommentCache curPrefabStageCache = null;

        private static bool IsSerializedObj(GameObject obj) => (obj != null
            && !obj.hideFlags.Flags_Contains(HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor)
                                                    && obj.scene.isLoaded);

        private static void UpdateCachesBySceneObj(ref HierarchyCommentCache sceneCahce, ref HierarchyCommentCache prefabCache, CWJ_Hierarchy_Core.HierarchyItemInfo itemInfo)
        {
            prefabCache = null;

            if (!IsSerializedObj(itemInfo.gameObj))
            {
                return;
            }

            if (sceneCahce == null)
            {
                sceneCahce = GetCommentCache(itemInfo.gameObj.scene);
            }

            if (itemInfo.itemType == HierarchyItemType.PrefabObjInScene)
            {
                prefabCache = GetPrefabInsCommentCache(itemInfo.gameObj);

                //prefabCache= 
            }
        }

        private static void UpdateCommentCacheInPrefabStage(ref HierarchyCommentCache prefabCache, CWJ_Hierarchy_Core.HierarchyItemInfo itemInfo)
        {
            if (!IsSerializedObj(itemInfo.gameObj))
            {
                return;
            }

            if (prefabCache == null)
            {
                prefabCache = GetCommentCache(itemInfo.gameObj.scene, prefabStage: itemInfo.prefabStage);
            }
        }

        private static Rect GetFullWidthRect(Rect selectionRect)
        {
            Rect fullWidthRect = new Rect(selectionRect);
            fullWidthRect.width += fullWidthRect.x - XOffset;
            fullWidthRect.x = XOffset;
            return fullWidthRect;
        }

        private static Rect GetButtonRect(Rect fullWidthRect, int offsetCnt = 1)
        {
            Rect btnRect = new Rect(fullWidthRect);
            btnRect.x = fullWidthRect.x + fullWidthRect.width - (ButtonSize * offsetCnt);
            btnRect.width = ButtonSize;
            return btnRect;
            //float xOffset = EditorStyles.label.CalcSize(new GUIContent(itemInfo.gameObj.name)).x + ButtonSize + 2;
            //if (xOffset > btnRect.width) xOffset = btnRect.width;
        }

        private static void ItemIsSceneType(CWJ_Hierarchy_Core.HierarchyItemInfo itemInfo, Rect selectionRect)
        {
            curSceneCache = GetCommentCache(itemInfo.scene); // procedural loading by hierarchy order
            if (curSceneCache == null) return;
            Rect btnRect = GetButtonRect(GetFullWidthRect(selectionRect), 2);

            if (GUI.Button(btnRect, findChildCommentBtnContent, EditorGUICustomStyle.NonPaddingButton))
            {
//#error 여기부터 작업해야함
            }
        }

        private static void ItemIsObjectType(CWJ_Hierarchy_Core.HierarchyItemInfo itemInfo, Rect selectionRect, Event guiEvent)
        {
            //Other types except Scene(GameObj, PrefabIns)
            if (!IsSerializedObj(itemInfo.gameObj)) return;

            if (itemInfo.isInPrefabStage)
            {
                UpdateCommentCacheInPrefabStage(ref curPrefabStageCache, itemInfo);
                if (curPrefabStageCache == null) return;
            }
            else
            {
                UpdateCachesBySceneObj(ref curSceneCache, ref curPrefabStageCache, itemInfo);
                if (curSceneCache == null) return;
            }

            Rect fullWidthRect = GetFullWidthRect(selectionRect);

            string sceneComment = string.Empty;
            string prefabComment = string.Empty;

            bool hasComment = (curSceneCache != null && curSceneCache.TryGetComment(itemInfo.gameObj, out sceneComment))
            | (curPrefabStageCache != null && curPrefabStageCache.TryGetComment(itemInfo.gameObj, out prefabComment));

            if (!hasComment)
            {
                if (!fullWidthRect.Contains(guiEvent.mousePosition) || (guiEvent.type != EventType.Repaint && !(guiEvent.isMouse && guiEvent.button == 0)))
                {
                    return;
                }
            }
            else
            {
                ChangeColorAndDrawGUI(Color.clear, () =>
                {
                    commentTooltipContent.tooltip = sceneComment;
                    if ((itemInfo.itemType == HierarchyItemType.PrefabObjInScene || itemInfo.isInPrefabStage) && !string.IsNullOrEmpty(prefabComment))
                    {
                        if (!string.IsNullOrEmpty(sceneComment)) commentTooltipContent.tooltip = "[Scene]\n" + commentTooltipContent.tooltip + "\n\n";
                        commentTooltipContent.tooltip += "[PrefabStage]\n" + prefabComment;
                    }
                    Rect tooltipRect = new Rect(fullWidthRect);
                    tooltipRect.width -= ButtonSize;
                    GUI.Box(tooltipRect, commentTooltipContent);
                });
            }

            Rect btnRect = GetButtonRect(fullWidthRect);

            Action drawButton = () =>
            {
                if (GUI.Button(btnRect, commentBtnContent, EditorGUICustomStyle.NonPaddingButton))
                {
                    HierarchyComment_Window.Open(itemInfo.gameObj, hasComment, itemInfo.isInPrefabStage ? prefabComment : sceneComment, itemInfo.prefabStage);
                }
            };

            if (hasComment)
            {
                if (string.IsNullOrEmpty(sceneComment) && itemInfo.prefabStage == null)
                { //has only prefabComment
                    Color color = Color.gray;
                    color.a = .5f;
                    ChangeColorAndDrawGUI(color, drawButton);
                }
                else
                {
                    drawButton.Invoke();
                }
            }
            else
            {
                Color color = GUI.color;
                color.a = .3f;
                ChangeColorAndDrawGUI(color, drawButton);
            }

        }


        private static void OnHierarchyItemGUI(CWJ_Hierarchy_Core.HierarchyItemInfo itemInfo, Rect selectionRect, Event guiEvent)
        {
            if (itemInfo.itemType == HierarchyItemType.Scene)
            {
                ItemIsSceneType(itemInfo, selectionRect);
            }
            else
            {
                ItemIsObjectType(itemInfo, selectionRect, guiEvent);
            }
        }

        public static void ChangeColorAndDrawGUI(Color color, Action drawAction)
        {
            Color lastColor = GUI.color;
            GUI.color = color;

            drawAction();

            GUI.color = lastColor;
        }
    }
}