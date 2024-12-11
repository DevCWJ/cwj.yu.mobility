#if UNITY_EDITOR

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

//[MenuItem(MissingObjectFinder_Function.WindowTag_First_Path)]
//public static void Open()
//MenuItem attribute는 상속받은 스크립트에서 직접 작성해야함
namespace CWJ.AccessibleEditor
{
    public abstract class WindowBehaviour<T, TS> : WindowBehaviourRoot where TS : CWJScriptableObject where T : WindowBehaviour<T, TS>
    {
        public static WindowBehaviour<T, TS> Window = null;

        //씬전환 시 scriptableObj 만 초기화됨.
        //매번 ScriptableObject를 생성하지 않으려면 asset파일로 만들어서 저장되어있게하는게 나음

        protected static TS _ScriptableObjs = null;

        public static TS ScriptableObj
        {
            get
            {
                if (_ScriptableObjs == null)
                {
                    _ScriptableObjs = ScriptableObjectStore.Instanced.GetScriptableObj<TS>();
                }

                return _ScriptableObjs;
            }
        }

        public static void SaveScriptableObj()
        {
            EditorUtility.SetDirty(ScriptableObj);
        }

        protected GUIStyle headerStyle = null;

        protected virtual GUIStyle HeaderStyle
        {
            get
            {
                if (headerStyle == null)
                {
                    headerStyle = new GUIStyle(EditorStyles.largeLabel);
                    headerStyle.alignment = TextAnchor.MiddleLeft;
                    headerStyle.fontSize = 15;
                    headerStyle.fontStyle = FontStyle.Bold;
                    headerStyle.wordWrap = true;
                }
                return headerStyle;
            }
        }

        protected GUIStyle bodyStyle = null;

        protected virtual GUIStyle BodyStyle
        {
            get
            {
                if (bodyStyle == null)
                {
                    bodyStyle = new GUIStyle(EditorStyles.label);
                    bodyStyle.alignment = TextAnchor.MiddleLeft;
                    bodyStyle.fontSize = 13;
                    bodyStyle.fontStyle = FontStyle.Normal;
                    bodyStyle.wordWrap = true;
                }
                return bodyStyle;
            }
        }

        public static WindowBehaviour<T, TS>[] GetThisWindows() => Resources.FindObjectsOfTypeAll<WindowBehaviour<T, TS>>();

        public static WindowBehaviourRoot[] GetCWJWindows() => Resources.FindObjectsOfTypeAll<WindowBehaviourRoot>();

        protected static SerializedObject SerializedObj;

        public static bool IsOpened => GetThisWindows()?.Length > 0;

        public abstract string GetScriptableFirstName { get; }

        private static List<Color> colors = new List<Color>();

        public static void Open()
        {
        } //need 'public new static void Open()'

        public static void Reopen()
        {
            CloseAllThisWindow();
            ReflectionUtil.InvokeMethodForcibly(null, true, false, typeof(T), nameof(Open));
        }

        private const float Size_minX = 250;
        private const float Size_minY = 225;

        public static void OnlyOpen(Vector2? minSize = null, Vector2? maxSize = null, bool isUtility = true)
        {
            Window = GetWindowInstance(isUtility: isUtility);

            Vector2 min = new Vector2(Size_minX, Size_minY);
            if (minSize != null) min = new Vector2((minSize.Value.x > Size_minX ? minSize.Value.x: Size_minX), (minSize.Value.y > Size_minY ? minSize.Value.y : Size_minY));
            Window.minSize = min;
            if (maxSize != null && maxSize.Value.x > min.x && maxSize.Value.y > min.y) Window.maxSize = maxSize.Value;
        }

        public static void CloseAllThisWindow()
        {
            foreach (var window in GetThisWindows())
            {
                window.Close();
            }
        }

        public static void CloseAllCWJWindow()
        {
            foreach (var window in GetCWJWindows())
            {
                window.Close();
            }
        }

        protected void OnReloadedWhileOpened()
        {
            GetSerializedObjAndInit();
            _OnReloadedWhileOpened();
            Repaint();
        }

        protected virtual void _OnReloadedWhileOpened() { }

        protected void OnSceneOpenedWhileOpened(UnityEngine.SceneManagement.Scene scene)
        {
            OnReloadedWhileOpened();
            _OnSceneOpenedWhileOpened();
            Repaint();
        }
        protected virtual void _OnSceneOpenedWhileOpened() { }

        protected void OnEnable()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent += OnReloadedWhileOpened;
            CWJ_EditorEventHelper.EditorSceneOpenedEvent += OnSceneOpenedWhileOpened;
            _OnEnable();
        }

        protected virtual void _OnEnable() { }

        protected virtual void _OnDisable() { }

        protected void OnDisable()
        {
            CWJ_EditorEventHelper.ReloadedScriptEvent -= OnReloadedWhileOpened;
            CWJ_EditorEventHelper.EditorSceneOpenedEvent -= OnSceneOpenedWhileOpened;
            _OnDisable();
        }

        protected static string _FamilyName = "";

        protected static string _FirstName = "";
        protected static void GetSerializedObjAndInit()
        {
            SerializedObj = new SerializedObject(ScriptableObj);

            if (string.IsNullOrEmpty(_FamilyName))
            {
                _FamilyName = typeof(TS).FullName;
                int lastIndex = _FamilyName.LastIndexOf('.') + 1;

                var windows = GetThisWindows();
                if (windows?.Length == 0) return;

                string customFirstName = windows[0].GetScriptableFirstName;
                if (string.IsNullOrEmpty(customFirstName))
                {
                    _FirstName = _FamilyName.Substring(lastIndex, (_FamilyName.Contains('_') ? _FamilyName.LastIndexOf('_') : _FamilyName.Length) - lastIndex);
                }
                else
                {
                    _FirstName = customFirstName;
                }
                _FamilyName = _FamilyName.Substring(0, _FamilyName.IndexOf('.'));
            }
        }


        protected void OnGUI()
        {
            if (SerializedObj == null || SerializedObj.targetObject == null) return;

            Event e = Event.current;
            if (e != null)
            {
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
                {
                    if (!e.shift)
                    {
                        CloseAllThisWindow();
                        return;
                    }
                    else
                    {
                        CloseAllCWJWindow();
                        return;
                    }
                }
            }

            SerializedObj.Update();
            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.HelpBox(new GUIContent(typeof(T).GetMyInfo(), WindowContent.image), true);
                _OnGUI();
            }
            SerializedObj.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                GUI.changed = true;
                Repaint();
            }

            if (GUI.changed)
            {
                isChanged = true;
                OnGUIChanged();
            }
        }

        protected bool isChanged;

        /// <summary>
        /// OnGUI() 대체
        /// </summary>
        protected abstract void _OnGUI();

        protected virtual void OnGUIChanged() { }

        private static GUIContent windowContent = null;
        protected static GUIContent WindowContent
        {
            get
            {
                if (windowContent == null)
                {
                    windowContent = new GUIContent(_FirstName, AccessibleEditorUtil.EditorHelperObj.IconTexture);
                }
                return windowContent;
            }
        }

        /// <summary>
        /// Window open 할 함수에서 해줘야할것.
        /// Init 포함되어있음
        /// ex) window = GetWindowInit<MissingObjectFinder_Window>();
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        protected static T GetWindowInstance(bool isUtility, bool isShow = true)
        {
            T window = GetWindow<T>(isUtility);

            window.OnReloadedWhileOpened();

            window.titleContent = WindowContent;

            if (isShow)
            {
                window.Show();
            }

            return window;
        }

        protected void FoldoutCustomize(ref bool isFold, bool isOuter, string title, System.Action contentMethod, string openChar = "{", string closeChar = "}")
        {
            if (isOuter)
            {
                BeginVerticalBox_Outer(true);
            }
            else
            {
                BeginVerticalBox_Inner(true);
            }
            if (!isFold) title += "  " + openChar + closeChar;
            if (isFold = EditorGUILayout.Foldout(isFold, title, true, EditorGUICustomStyle.Foldout))
            {
                EditorGUILayout.LabelField(openChar);
                contentMethod();
                EditorGUILayout.LabelField(closeChar);
            }
            EndVerticalBox();
        }

        public static void BeginError(bool error)
        {
            BeginError(error, new Color().GetDarkRed());
        }

        public static void BeginError(bool error, Color color)
        {
            colors.Add(GUI.color);

            GUI.color = (error ? color : colors[0]);
        }

        public static void EndError()
        {
            int index = colors.Count - 1;

            GUI.color = colors[index];

            colors.RemoveAt(index);
        }

        public static Rect Reserve()
        {
            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(GUIContent.none);
            EditorGUILayout.EndVertical();

            return rect;
        }

        protected void SwitchingToggle(ref bool a_prevValue, ref bool a_curValue,
                                       ref bool b_PrevValue, ref bool b_CurValue)
        {
            if (a_prevValue != a_curValue)
            {
                if (a_curValue)
                {
                    b_PrevValue = b_CurValue = false;
                }
                a_prevValue = a_curValue;
            }

            if (b_PrevValue != b_CurValue)
            {
                if (b_CurValue)
                {
                    a_prevValue = a_curValue = false;
                }
                b_PrevValue = b_CurValue;
            }
        }

        /// <summary>
        /// Default guiStyle is headerStyle.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="guiStyle"></param>
        /// <param name="isExpandWidth"></param>
        /// <param name="alignment"></param>
        protected void DrawLabelField(string content, bool isHeader = true, bool isExpandWidth = true, TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            GUIStyle guiStyle = new GUIStyle(isHeader ? EditorGUICustomStyle.LargeBoldLabelStyles : EditorStyles.label);
            guiStyle.alignment = alignment;
            if (alignment == TextAnchor.MiddleLeft || alignment == TextAnchor.LowerLeft || alignment == TextAnchor.UpperLeft)
            {
                content = " " + content;
            }
            EditorGUILayout.LabelField(content, guiStyle, GUILayout.ExpandWidth(isExpandWidth), GUILayout.ExpandHeight(false));
        }

        protected bool IsCompiling()
        {
            if (EditorApplication.isCompiling)
            {
                GUILayout.FlexibleSpace();
                DrawLabelField("Please wait until the compilation of the script has finished.", alignment: TextAnchor.MiddleCenter);

                GUILayout.FlexibleSpace();
                DrawLabelField("※ When editor gets stuck compiling※\nClick on a CWJ's window to give it focus and\npress 'Shift + ESC' key to close all open CWJ's window", false, alignment: TextAnchor.MiddleCenter);
                GUILayout.Space(10);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool IsAppPlaying(bool isWriteLabel = true)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (isWriteLabel) DrawLabelField("Cannot be used during play mode.", alignment: TextAnchor.MiddleCenter);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void DirectoryPathButton(string label, string path)
        {
            if (GUILayout.Button(label + " " + path))
            {
                path.OpenFolder();
            }
        }

        protected void BeginVerticalBox_Inner(bool isBold)
        {
            bool prevEnabled = GUI.enabled;
            GUI.enabled = isBold;
            EditorGUILayout.BeginVertical(EditorGUICustomStyle.Box);
            GUI.enabled = prevEnabled;
        }

        protected void BeginVerticalBox_Outer(bool isBold)
        {
            bool prevEnabled = GUI.enabled;
            GUI.enabled = isBold;
            EditorGUILayout.BeginVertical(EditorGUICustomStyle.OuterBox);
            GUI.enabled = prevEnabled;
        }

        protected void EndVerticalBox()
        {
            EditorGUILayout.EndVertical();
        }
    }
}

#endif