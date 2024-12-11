#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;

namespace CWJ.EditorOnly
{
    public class EditObjectScope : GUI.Scope
    {
        private SerializedObject _serializedObject;

        public EditObjectScope(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
            serializedObject.ApplyModifiedProperties();
        }

        protected override void CloseScope()
        {
            _serializedObject.Update();
        }
    }
}

#endif