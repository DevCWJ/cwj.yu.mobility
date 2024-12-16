using UnityEngine;

namespace CWJ.AccessibleEditor.Function
{
    public class MissingObjectFinder_ScriptableObject : CWJScriptableObject
    {
        public Component copyComp_Prefab;

        public GameObject[] missingObjs_Prefabs = new GameObject[0];
    }
}