using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using UnityObject = UnityEngine.Object;

namespace CWJ.EditorOnly
{
    public class AssetReferenceData_ScriptableObject : CWJScriptableObject
    {
        public UnityObject targetObj;
        public List<UnityObject> references = new List<UnityObject>();

        public override void OnConstruct()
        {
            targetObj = null;
            references = new List<UnityObject>();
        }
    }
}