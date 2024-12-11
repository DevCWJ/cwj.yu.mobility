#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;

namespace CWJ.EditorOnly
{
    public abstract class AddNamedItemContent : IAddContent
    {
        private string name = "";
        private bool isNameValid = true;
        private bool isFocusName = true;

        protected abstract void Add_(string name);
        protected abstract bool IsNameInUse(string name);

        protected virtual float GetHeight_() => 0.0f;
        protected virtual bool Draw_(bool clean) => false;
        protected virtual bool Validate_() => true;
        protected virtual void Reset_() { }

        public float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + GetHeight_();
        }

        public bool Draw(bool clean)
        {
            var create = false;

            using (new InvalidScope(clean || isNameValid))
            {
                create |= EnterFieldDrawer.DrawString("NewItemName", GUIContent.none, ref name);

                if (isFocusName)
                {
                    GUI.FocusControl("NewItemName");
                    isFocusName = false;
                }
            }

            create = Draw_(clean) || create;

            return create;
        }

        public bool Validate()
        {
            isNameValid = !string.IsNullOrEmpty(name) && !IsNameInUse(name);
            return Validate_() && isNameValid;
        }

        public void Add()
        {
            Add_(name);
        }

        public void Reset()
        {
            name = "";
            Reset_();
        }
    }
}

#endif