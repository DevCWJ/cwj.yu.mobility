using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace CWJ.Serializable
{
    public class DictionaryExample_Unity2020 : MonoBehaviour
    {
        public enum TestEnum
        {
            a,
            b,
            c,
        }
#if UNITY_2020_1_OR_NEWER
        [System.Serializable]
        public class Container
        {
            public string str;
            public int @int;
        }

        [System.Serializable]
        public class FileDataContainer
        {
            public TestEnum enumType;
            public Sprite[] iconSprites;
        }

        public DictionaryVisualized <TestEnum, GameObject[]> enumDict; 
        public DictionaryVisualized <string, GameObject[]> strGoArrayDict; 
        public DictionaryVisualized <string, GameObject> strGoDict; 
        public DictionaryVisualized <string, FileDataContainer> classTest; 

        public Dictionary<string, GameObject[]> dict;

        void Start()
        {
            string keyToCheck = "abc";
            bool contains = strGoArrayDict.TryGetValue(keyToCheck, out GameObject[] gameObjects);
            Debug.LogFormat("myGenericDict contains '{0}': {1}\n{2}", keyToCheck, contains, string.Join(", ", gameObjects.Select(g => g.name)));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                string newKey = "runtime example";
                Debug.LogFormat("Added '{0}' to myGenericDict.", newKey);
            }
        }
#endif
    }
}