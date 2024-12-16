#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace CWJ.AccessibleEditor
{
    public static class EditorGUI_CWJ_TransformExtension
    {
        public static void DrawSibilingIndex(UnityEngine.Object[] targets, Transform targetTrf, ref string lastSiblingTxt)
        {
            int targetLength = targets.Length;

            if (targetLength == 0) return;

            Action<Transform> btnCallback = null;
            Func<Transform, bool> condition = null;
            Func<Transform[], IEnumerable<Transform>> getTargetsTrfs = null;

            EditorGUILayout.BeginHorizontal();

            //Sibling Index
            if ((targetLength > 1 || GetMaxSiblingIndex(targetTrf) > 1))
            {
                EditorGUILayout.PrefixLabel("Sibling Index");

                if ((targetLength == 1 && targetTrf.parent == null))
                    GUILayout.FlexibleSpace();

                //Up
                if (targetLength > 1)
                {
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        DrawSiblingArrowBtn_Max(true, ref condition, ref btnCallback);
                        DrawSiblingArrowBtn(true, ref condition, ref btnCallback);
                        if (changeScope.changed)
                        {
                            getTargetsTrfs = (trfs) => trfs.OrderBy(t => t.GetSiblingIndex());
                        }
                    }
                }
                else
                {
                    DrawSiblingArrowBtn_Max(true, ref condition, ref btnCallback);
                    DrawSiblingArrowBtn(true, ref condition, ref btnCallback);
                }

                int curSiblingIndex = targetTrf.GetSiblingIndex();
                int txtLength = (lastSiblingTxt?.Length ?? 1) + 1;
                string setSiblingTxt = EditorGUILayout.DelayedTextField(targetLength == 1 ? curSiblingIndex.ToString() : "─", EditorGUICustomStyle.Label_Centered, GUILayout.MaxWidth(txtLength * 8));

                if (setSiblingTxt != lastSiblingTxt)
                {
                    int setSiblingIndex;
                    if (int.TryParse(setSiblingTxt, out setSiblingIndex))
                    {
                        if (setSiblingIndex != curSiblingIndex)
                        {
                            condition = (t) => IsAbleToChangeSiblingIndex(curSiblingIndex, setSiblingIndex, GetMaxSiblingIndex(t));
                            btnCallback = (t) => t.SetSiblingIndex(setSiblingIndex);
                        }
                    }
                    lastSiblingTxt = setSiblingTxt;
                }

                //Down
                if (targetLength > 1)
                {
                    using (var changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        DrawSiblingArrowBtn(false, ref condition, ref btnCallback);
                        DrawSiblingArrowBtn_Max(false, ref condition, ref btnCallback);
                        if (changeScope.changed)
                        {
                            getTargetsTrfs = (trfs) => trfs.OrderByDescending(t => t.GetSiblingIndex());
                        }
                    }
                }
                else
                {
                    DrawSiblingArrowBtn(false, ref condition, ref btnCallback);
                    DrawSiblingArrowBtn_Max(false, ref condition, ref btnCallback);
                }
            }

            GUILayout.FlexibleSpace();

            //Remove Parent
            if ((targetLength > 1 || targetTrf.parent != null))
            {
                if (GUILayout.Button(RemoveParentBtnContent, GUILayout.ExpandWidth(false)))
                {
                    condition = (t) => t.parent != null;
                    btnCallback = (t) =>
                    {
                        int parentSiblingIndex = t.parent.GetSiblingIndex();
                        t.SetParent(t.parent.parent, true);
                        t.SetSiblingIndex(parentSiblingIndex + 1);
                    };
                }
            }

            EditorGUILayout.EndHorizontal();

            if (btnCallback != null)
            {
                Func<Transform, bool> @do = (t) =>
                {
                    if (!condition.Invoke(t)) return false;

                    Undo.SetTransformParent(t, t.parent, "Undo Changed 'Parent Or SibilingIndex' " + t.name);
                    btnCallback.Invoke(t);
                    return true;
                };

                if (targetLength == 1)
                {
                    if (@do.Invoke(targetTrf))
                    {
                        EditorGUIUtility.PingObject(targetTrf);
                    }
                }
                else
                {
                    Transform[] trfs = targets.ConvertAll(o => o as Transform);
                    foreach (Transform t in getTargetsTrfs == null ? trfs : getTargetsTrfs.Invoke(trfs))
                    {
                        @do.Invoke(t);
                    }
                }
            }
        }

        private static int GetMaxSiblingIndex(Transform t)
        {
            return (t.parent != null) ? t.parent.childCount : t.gameObject.scene.GetRootGameObjsOnlyValidScene().Length;
        }

        private static bool IsAbleToChangeSiblingIndex(int curIndex, int setIndex, int maxIndex)
        {
            return curIndex != setIndex && 0 <= setIndex && setIndex < maxIndex;
        }

        private static bool IsAbleToChangeSiblingIndex(Transform t, bool isUp, bool isMaxMove)
        {
            int maxIndex = GetMaxSiblingIndex(t);
            int curIndex = t.GetSiblingIndex();
            int setIndex = isMaxMove ? (isUp ? 0 : maxIndex - 1) : (curIndex + (isUp ? -1 : 1));
            return IsAbleToChangeSiblingIndex(curIndex, setIndex, maxIndex);
        }

        private static void DrawSiblingArrowBtn_Max(bool isUp, ref Func<Transform, bool> condition, ref Action<Transform> btnCallback)
        {
            Color c = GUI.color;
            GUI.color = Color.red;

            _DrawSiblingArrowBtn(isUp, true, ref condition, ref btnCallback);

            GUI.color = c;
        }

        private static void DrawSiblingArrowBtn(bool isUp, ref Func<Transform, bool> condition, ref Action<Transform> btnCallback)
        {
            _DrawSiblingArrowBtn(isUp, false, ref condition, ref btnCallback);
        }


        private static void _DrawSiblingArrowBtn(bool isUp, bool isMaxMove,
            ref Func<Transform, bool> condition, ref Action<Transform> btnCallback)
        {
            if (GUILayout.Button(isUp ? UpBtnContent : DownBtnContent, UpDownBtnStyle, GetUpDownBtnOptions))
            {
                condition = (t) => IsAbleToChangeSiblingIndex(t, isUp, isMaxMove);
                btnCallback = (t) => ChangeSiblingIndex(t, isUp, isMaxMove);
            }
        }

        private static void ChangeSiblingIndex(Transform t, bool isUp, bool isMaxMove)
        {
            if (isMaxMove)
            {
                if (isUp) t.SetAsFirstSibling();
                else t.SetAsLastSibling();
            }
            else
            {
                t.SetSiblingIndex(t.GetSiblingIndex() + (isUp ? -1 : +1));
            }
        }

        #region Content

        static GUIContent _UpBtnContent = null;
        static GUIContent UpBtnContent
        {
            get
            {
                if (_UpBtnContent == null)
                {
                    _UpBtnContent = new GUIContent(EditorGUIUtility.IconContent("d_scrollup"));
                    _UpBtnContent.tooltip = "Set the sibling index up";
                }
                return _UpBtnContent;
            }
        }

        static GUIContent _DownBtnContent = null;
        static GUIContent DownBtnContent
        {
            get
            {
                if (_DownBtnContent == null)
                {
                    _DownBtnContent = new GUIContent(EditorGUIUtility.IconContent("d_scrolldown"));
                    _DownBtnContent.tooltip = "Set the sibling index down";
                }
                return _DownBtnContent;
            }
        }

        static GUIStyle _UpDownBtnStyle = null;
        static GUIStyle UpDownBtnStyle
        {
            get
            {
                if (_UpDownBtnStyle == null)
                {
                    _UpDownBtnStyle = new GUIStyle(EditorGUICustomStyle.NonPaddingButton);
                    _UpDownBtnStyle.margin = new RectOffset(0, 0, 5, 0);
                }
                return _UpDownBtnStyle;
            }
        }


        static GUILayoutOption[] _GetUpDownBtnOptions = null;
        static GUILayoutOption[] GetUpDownBtnOptions
        {
            get
            {
                if (_GetUpDownBtnOptions == null)
                {
                    float min, max;
                    UpDownBtnStyle.CalcMinMaxWidth(DownBtnContent, out min, out max);
                    _GetUpDownBtnOptions = new GUILayoutOption[] { GUILayout.MaxWidth(max), GUILayout.MinWidth(min) };
                }
                return _GetUpDownBtnOptions;
            }
        }

        static GUIContent _RemoveParentBtnContent = null;
        static GUIContent RemoveParentBtnContent
        {
            get
            {
                if (_RemoveParentBtnContent == null)
                {
                    _RemoveParentBtnContent = new GUIContent("Remove Parent", "Remove parents one by one");
                }
                return _RemoveParentBtnContent;
            }
        }

        #endregion
    }
}

#endif