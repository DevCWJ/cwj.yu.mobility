#if UNITY_EDITOR
using UnityEngine;

namespace CWJ.EditorOnly
{
    public abstract class StaticContent
    {
        private GUIContent _content;

        protected abstract GUIContent Create();

        public GUIContent Content
        {
            get
            {
                if (_content == null)
                    _content = Create();

                return _content;
            }
        }
    }
}

#endif