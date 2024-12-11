#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace CWJ.EditorOnly
{
    public abstract class ListProxy : IList
    {
        public abstract int Count { get; }
        public abstract object this[int index] { get; set; }

        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => null;

        public int Add(object value) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public bool Contains(object value) => throw new NotImplementedException();
        public void CopyTo(Array array, int index) => throw new NotImplementedException();
        public IEnumerator GetEnumerator() => throw new NotImplementedException();
        public int IndexOf(object value) => throw new NotImplementedException();
        public void Insert(int index, object value) => throw new NotImplementedException();
        public void Remove(object value) => throw new NotImplementedException();
        public void RemoveAt(int index) => throw new NotImplementedException();
    }

    public abstract class ListControl
    {
        public static float HeaderHeight = EditorGUIUtility.singleLineHeight;
        public static float CollapsedHeight = EditorGUIUtility.singleLineHeight * 0.5f;
        public static float ItemDefaultHeight = EditorGUIUtility.singleLineHeight;
        public const float ItemPadding = 5.0f;

        private static IconButton expandButton = new IconButton(IconButton.Collapsed, "Expand the contents of this list");
        private static IconButton collapseButton = new IconButton(IconButton.Expanded, "Collapse the contents of this list");

        public ReorderableList List { get; private set; } // this is Unity's undocumented list class that does the bulk of the drawing and layout

        private string collapsablePreference;
        private GUIContent emptyLabel;
        private ReorderableList.ElementHeightCallbackDelegate getElementHeight;
        public bool isReadonly;
        private bool canReorder = false;
        private Action<int, int> onReorder;
        private List<HeaderButton> headerButtons = new List<HeaderButton>();
        private List<ItemButton> itemButtons = new List<ItemButton>();

        private GUIContent label;
        private bool isVisible = true;
        private int clickedItem = -1;
        private int clickedButton = -1;
        private Rect clickedRect;
        protected abstract void Draw(Rect rect, int index);

        #region Setup Methods

        protected void Setup(ReorderableList list)
        {
            List = list;

            List.drawHeaderCallback += DrawHeader;
            List.drawElementBackgroundCallback = DrawBackground;
            List.drawElementCallback = DrawElement;
            List.elementHeightCallback = GetElementHeight;
            List.drawNoneElementCallback = DrawEmpty;
            List.onReorderCallbackWithDetails = ElementsMoved;
            List.footerHeight = 0.0f;

            Visible = isVisible;
        }

        public ListControl MakeCollapsable(string preferenceName)
        {
            collapsablePreference = preferenceName;
            Visible = EditorPrefs.GetBool(collapsablePreference, true);
            return this;
        }

        public ListControl MakeEmptyLabel(GUIContent label)
        {
            emptyLabel = label;
            return this;
        }

        public ListControl MakeReorderable(Action<int, int> callback = null)
        {
            canReorder = true;
            onReorder = callback;
            List.draggable = Visible;
            return this;
        }

        public ListControl MakeHeaderButton(IconButton icon, Action<Rect> callback)
        {
            headerButtons.Add(new HeaderButton { Icon = icon, Callback = callback });
            return this;
        }

        public ListControl MakeHeaderButton(IconButton icon, GenericMenu menu)
        {
            headerButtons.Add(new HeaderButton { Icon = icon, Menu = menu });
            return this;
        }

        public ListControl MakeHeaderButton(IconButton icon, PopupWindowContent popup)
        {
            headerButtons.Add(new HeaderButton { Icon = icon, Popup = popup });
            return this;
        }

        public ListControl MakeItemButton(IconButton icon, Action<Rect, int> callback)
        {
            itemButtons.Add(new ItemButton { Icon = icon, Callback = callback });
            return this;
        }

        public ListControl MakeCustomHeight(ReorderableList.ElementHeightCallbackDelegate callback)
        {
            getElementHeight = callback;
            return this;
        }

        #endregion

        #region Collapsing

        public bool Visible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;

                if (List != null)
                {
                    List.elementHeight = isVisible ? ItemDefaultHeight + ItemPadding : 0;
                    List.draggable = isVisible && canReorder;
                }
            }
        }

        #endregion

        #region Moving

        private void ElementsMoved(ReorderableList list, int oldIndex, int newIndex)
        {
            onReorder?.Invoke(oldIndex, newIndex);
        }

        #endregion

        #region Sizing

        public float GetHeight()
        {
            return Visible ? List.GetHeight() : HeaderHeight + CollapsedHeight;
        }

        private float GetElementHeight(int index)
        {
            return Visible ? (getElementHeight == null ? ItemDefaultHeight + ItemPadding : getElementHeight(index) + ItemPadding) : 0;
        }

        #endregion

        #region Buttons

        private class HeaderButton
        {
            public IconButton Icon;
            public Action<Rect> Callback;
            public PopupWindowContent Popup;
            public GenericMenu Menu;

            public void Press(Rect rect)
            {
                if (Popup != null)
                    PopupWindow.Show(rect, Popup);
                else if (Menu != null)
                    Menu.ShowAsContext();
                else
                    Callback(rect);
            }
        }

        private class ItemButton
        {
            public IconButton Icon;
            public Action<Rect, int> Callback;

            public void Press(Rect rect, int index)
            {
                Callback(rect, index);
            }
        }

        private void ClearClickedButton()
        {
            clickedItem = -1;
            clickedButton = -1;
        }

        private void SetClickedButton(int item, int index, Rect rect)
        {
            clickedItem = item;
            clickedButton = index;
            clickedRect = rect;
        }

        private void ProcessClickedButton()
        {
            if (clickedButton >= 0)
            {
                if (clickedItem == -1)
                {
                    var button = headerButtons[clickedButton];
                    button.Press(clickedRect);
                }
                else
                {


                    var button = itemButtons[clickedButton];
                    button.Press(clickedRect, clickedItem);
                }
            }

            ClearClickedButton();
        }

        #endregion

        #region Drawing

        public void Draw(GUIContent label)
        {
            var height = GetHeight();
            var rect = EditorGUILayout.GetControlRect(false, height);

            Draw(rect, label);
        }

        public void Draw(Rect position, GUIContent label)
        {
            // DoList respects the current indent level when drawing labels and it shouldn't - the workaround is to
            // temporarily set the indent back to 0

            RectHelper.TakeIndent(ref position);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            using (new EditorGUI.DisabledScope(isReadonly))
            {
                PreDraw(label);
                List.DoList(position);
                PostDraw();
            }

            EditorGUI.indentLevel = indent;
        }

        private void PreDraw(GUIContent label)
        {
            ClearClickedButton();
            this.label = label;
        }

        private void PostDraw()
        {
            if (collapsablePreference != null)
                EditorPrefs.SetBool(collapsablePreference, Visible);

            ProcessClickedButton();
        }

        private void DrawHeader(Rect rect)
        {
            var buttonsWidth = (RectHelper.IconWidth + RectHelper.HorizontalSpace) * headerButtons.Count;
            var buttonsRect = RectHelper.TakeTrailingWidth(ref rect, buttonsWidth);
            var labelRect = new Rect(rect);

            if (collapsablePreference != null)
            {
                var arrowRect = RectHelper.TakeLeadingIcon(ref labelRect);

                EditorGUI.LabelField(arrowRect, Visible ? collapseButton.Content : expandButton.Content);

                if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
                    Visible = !Visible;
            }

            EditorGUI.LabelField(labelRect, label);

            if (Visible)
            {
                for (var i = 0; i < headerButtons.Count; i++)
                {
                    var buttonRect = RectHelper.TakeLeadingIcon(ref buttonsRect);

                    if (GUI.Button(buttonRect, headerButtons[i].Icon.Content, GUIStyle.none))
                        SetClickedButton(-1, i, buttonRect);
                }
            }
        }

        private void DrawBackground(Rect rect, int index, bool isActive, bool isFocused)
        {
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (Visible && index < List.count)
            {
                var elementRect = RectHelper.AdjustHeight(rect, rect.height - ItemPadding, RectVerticalAlignment.Middle);
                var buttonsWidth = (RectHelper.IconWidth + RectHelper.HorizontalSpace) * itemButtons.Count;
                var buttonsRect = RectHelper.TakeTrailingWidth(ref elementRect, buttonsWidth);

                for (var i = 0; i < itemButtons.Count; i++)
                {
                    var buttonRect = RectHelper.AdjustHeight(RectHelper.TakeLeadingIcon(ref buttonsRect), ItemDefaultHeight, RectVerticalAlignment.Top);

                    if (GUI.Button(buttonRect, itemButtons[i].Icon.Content, GUIStyle.none))
                        SetClickedButton(index, i, buttonRect);
                }

                Draw(elementRect, index);
            }
        }

        private void DrawEmpty(Rect rect)
        {
            if (Visible && emptyLabel != null)
                EditorGUI.LabelField(rect, emptyLabel);
            else
                ReorderableList.defaultBehaviours.DrawNoneElement(rect, false);
        }

        #endregion
    }
}

#endif