using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Linq;

namespace CWJ.AccessibleEditor.Function
{
    [System.Serializable]
    public struct StaticFlagStruct
    {
        public string name;
        public List<GameObject> objects;
        public bool isVisible;

        public StaticFlagStruct(string name)
        {
            this.name = name;
            this.objects = new List<GameObject>();
            //isVisible = objects.Length > 0;
            isVisible = false;
        }
    }

    public class FindStaticEditorFlagObject_ScriptableObject : CWJScriptableObject
    {
        public bool isVisibleAll, isVisibleActivate, isVisibleDeactivate;

        public StaticFlagStruct[] staticFlagStructs = null;

        public StaticFlagStruct[] activateStructs;
        public StaticFlagStruct[] deactivateStructs;

        (string[] Names, StaticEditorFlags[] Enums) GetStaticEditorFlagNames()
        {
#if UNITY_2019_2_OR_NEWER //19.2버전부터 생긴 StaticEditorFlags.ContributeGI 으로 인한 버그(같은 인덱스가 두개) 덕분에 이딴짓을 해야함(새로운 Enum이름과 Obsolete된 Enum이름까지 둘다나와버림
            List<string> enumNames = new List<string>();
            List<StaticEditorFlags> enums = new List<StaticEditorFlags>();

            Type type = typeof(StaticEditorFlags);
            var names = Enum.GetNames(type);
            for (int i = 0; i < names.Length; i++)
            {
                var attributes = (ObsoleteAttribute[])type.GetField(names[i]).GetCustomAttributes(typeof(ObsoleteAttribute), false);
                if (attributes == null || attributes.Length == 0)
                {
                    enumNames.Add(names[i]);
                    enums.Add(EnumUtil.ToEnum<StaticEditorFlags>(names[i]));
                }
            }

            return (enumNames.ToArray(), enums.ToArray()); //LightmapStatic이 Obsolete되버림
#else
            StaticEditorFlags[] staticEditorFlags = EnumUtil.GetEnumArray<StaticEditorFlags>();
            return (staticEditorFlags.ConvertAll(e => e.ToString()), staticEditorFlags);
#endif
        }

        public void UpdateStaticFlagObjs()
        {
            var flagArrays = GetStaticEditorFlagNames();

            string[] flagEnumNames = flagArrays.Names;

            StaticEditorFlags[] flagEnums = flagArrays.Enums;

            staticFlagStructs = new StaticFlagStruct[flagEnumNames.Length];
            activateStructs = new StaticFlagStruct[flagEnumNames.Length];
            deactivateStructs = new StaticFlagStruct[flagEnumNames.Length];

            //GameObject[] allGameObjs = FindUtil.FindGameObjects(true, true);
            //var objsByFlags = allGameObjs
            //                            .Select(go => new { flags = AccessibleEditorUtil.GetEditorStaticFlags(go), obj = go })
            //                            .SelectMany(group => group.flags.Select(f => new { Flag = f, Item = group.obj }))
            //                            .GroupBy(k => k.Flag, v => v.Item);

            //int i = 0;
            //foreach (var group in objsByFlags)
            //{
            //    string flagName = group.Key.ToString();
            //    Debug.LogError(flagName);
            //    List<GameObject> allObjs = new List<GameObject>();

            //    foreach (var obj in group)
            //    {
            //        allObjs.Add(obj);
            //    }

            //    staticFlagStructs[i] = new StaticFlagStruct(flagName, allObjs.ToArray());
            //    GameObject[] activates, deactivates;
            //    activates = ArrayUtil.FindAll(allObjs, s => s.activeInHierarchy, out deactivates);
            //    activateStructs[i] = new StaticFlagStruct(flagName, activates);
            //    deactivateStructs[i] = new StaticFlagStruct(flagName, deactivates);
            //    i++;
            //} 이건 왜 잘되다가 중간부터 안되는지 의문

            for (int i = 0; i < flagEnumNames.Length; i++)
            {
                staticFlagStructs[i] = new StaticFlagStruct(flagEnumNames[i]);
                activateStructs[i] = new StaticFlagStruct(flagEnumNames[i]);
                deactivateStructs[i] = new StaticFlagStruct(flagEnumNames[i]);
            }
  
            GameObject[] allObjs = FindUtil.FindGameObjects(true, true);

            for (int i = 0; i < allObjs.Length; i++)
            {
                for (int j = 0; j < flagEnumNames.Length; j++)
                {
                    if (GameObjectUtility.AreStaticEditorFlagsSet(allObjs[i], flagEnums[j]))
                    {
                        staticFlagStructs[j].objects.Add(allObjs[i]);
                        if (allObjs[i].activeInHierarchy)
                        {
                            activateStructs[j].objects.Add(allObjs[i]);
                        }
                        else
                        {
                            deactivateStructs[j].objects.Add(allObjs[i]);
                        }
                    }
                }
            }

            EditorUtility.SetDirty(this);
        }
    }
}