using System;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

using UnityEditorInternal;

#endif

using CWJ.AccessibleEditor;

namespace CWJ
{
    //LayerMask 매개변수에는 항상 LayerMask를 넣거나 LayerMask.GetMask.
    //gameObject.layer와 NameToLayer가 같고 LayerMask와 GetMask가 같음 헷갈리지 말것
    //gameObject.layer = LayerMask.NameToLayer("a")
    //LayerMask = (LayerMask)LayerMask.GetMask("a")
    public static class LayerUtil
    {
        /// <summary>
        /// 특정 Layer들과 충돌을 ignore하고싶을때
        /// </summary>
        /// <param name="myLayer"></param>
        /// <param name="ignoreLayer"></param>
        /// <param name="isIgnore"></param>
        public static void SetIgnoreLayerCollision(this int myLayer, LayerMask ignoreLayer, bool isIgnore = true)
        {
            int[] ignoreLayerList = ignoreLayer.ConvertToIntList();

            for (int i = 0; i < ignoreLayerList.Length; i++)
            {
                Physics.IgnoreLayerCollision(myLayer, ignoreLayerList[i], isIgnore);
            }
        }

        public static void SetIgnoreLayerMaskCollision(LayerMask layerMaskOne, LayerMask layerMaskTwo, bool isIgnore)
        {
            int[] ignoreLayer1List = layerMaskOne.ConvertToIntList();
            int[] ignoreLayer2List = layerMaskTwo.ConvertToIntList();

            for (int i = 0; i < ignoreLayer1List.Length; i++)
            {
                for (int j = 0; j < ignoreLayer2List.Length; j++)
                {
                    Physics.IgnoreLayerCollision(ignoreLayer1List[i], ignoreLayer2List[j], isIgnore);
                }
            }
        }

        /// <summary>
        /// SetIgnoreLayerCollision와는 다르게 해당레이어만 충돌되게하거나 해당레이어만 충돌안되게 하는 극단적인 함수
        /// 반대로 해당 레이어만 충돌시키게 하고싶으면 그냥 '~' 쓰기
        /// </summary>
        /// <param name="myLayer"></param>
        /// <param name="ignoreLayer"></param>
        public static void SetInverseIgnoreLayerCollision(this int myLayer, LayerMask ignoreLayer)
        {
            int[] ignoreLayerList = ignoreLayer.ConvertToIntList();

            for (int i = 0; i < ignoreLayerList.Length; i++)
            {
                Physics.IgnoreLayerCollision(myLayer, ignoreLayerList[i], true);
            }

            int[] notIgnoreLayerList = ignoreLayer.ConvertToReverseIntList();

            for (int i = 0; i < notIgnoreLayerList.Length; i++)
            {
                Physics.IgnoreLayerCollision(myLayer, notIgnoreLayerList[i], false);
            }
        }

        public static string[] GetIgnoreLayerNames(this int myLayer)
        {
            List<string> ignoreLayerNames = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                int layer = LayerMask.NameToLayer(layerName);
                if (Physics.GetIgnoreLayerCollision(myLayer, layer))
                {
                    ignoreLayerNames.Add(layerName);
                }
            }

            return ignoreLayerNames.ToArray();
        }

        public static void SetLayer(this GameObject obj, string layerName, bool isRecursively = false)
        {
            LayerEditorUtil.AddNewLayer(layerName);
            int layer = LayerMask.NameToLayer(layerName);
            if (!isRecursively)
            {
                obj.layer = layer;
            }
            else
            {
                SetLayerRecursive(obj.transform, layer);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }

        public static void SetLayerRecursive(Transform rootTrf, int layer)
        {
            rootTrf.gameObject.layer = layer;
            foreach (Transform child in rootTrf)
            {
                SetLayerRecursive(child, layer);
            }
        }

        public static string[] GetAllLayerName(params string[] ignoreNames)
        {
            List<string> layerNames = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName) && (ignoreNames != null && !ignoreNames.IsExists(layerName)))
                {
                    layerNames.Add(layerName);
                }
            }

            return layerNames.ToArray();
        }

        public static int[] ConvertToIntList(this LayerMask mask)
        {
            List<int> layerList = new List<int>();
            int maskValue = mask.value;

            for (int i = 0; i < 32; i++)
            {
                if (maskValue == (1 << i | maskValue))
                {
                    layerList.Add(i);
                }
            }

            return layerList.ToArray();
        }

        public static int[] ConvertToReverseIntList(this LayerMask mask)
        {
            List<int> layerList = new List<int>();
            int maskValue = mask.value;

            for (int i = 0; i < 32; i++)
            {
                if (maskValue != (1 << i | maskValue))
                {
                    layerList.Add(i);
                }
            }

            return layerList.ToArray();
        }

        #region 레이어비교 메소드

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectLayer">gameObject.layer</param>
        /// <param name="checkLayerName"></param>
        /// <returns></returns>
        public static bool LayerEquals(this int gameObjectLayer, string checkLayerName)
        {
            return gameObjectLayer == LayerMask.NameToLayer(checkLayerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectLayer">gameObject.layer</param>
        /// <param name="checkLayerName"></param>
        /// <returns></returns>
        public static bool LayerEquals(this int gameObjectLayer, LayerMask checkLayerMask)
        {
            return (1 << gameObjectLayer) == checkLayerMask;
        }

        public static bool LayerContains(this LayerMask layerMask, params string[] names)
        {
            int layerMaskValue = layerMask.value;
            return layerMaskValue == (layerMaskValue | LayerMask.GetMask(names));
        }

        /// <summary>
        /// 이름으로 레이어를 가져오고 싶다면 위에꺼 쓰기
        /// <para/>paralayerMask안에 checkLayer(gameObject.layer)가 있는지?
        /// </summary>
        /// <param name="layerMaskValue"></param>
        /// <param name="checkLayer"></param>
        /// <returns></returns>
        public static bool LayerContains(this LayerMask layerMask, int checkLayer)
        {
            int layerMaskValue = layerMask.value;
            return layerMaskValue == (layerMaskValue | (1 << checkLayer));
        }

        public static bool LayerContains(string[] layerMaskNames, int checkLayer)
        {
            return LayerContains((LayerMask)LayerMask.GetMask(layerMaskNames), checkLayer);
        }

        #endregion 레이어비교 메소드

        #region 레이어 포함 반환

        public static LayerMask Include(this LayerMask layerMask, LayerMask addLayerMask)
        {
            int[] layers = addLayerMask.ConvertToIntList();
            for (int i = 0; i < layers.Length; i++)
            {
                layerMask |= (1 << layers[i]);
            }
            return layerMask;
        }

        public static LayerMask Include(this LayerMask layerMask, GameObject gameObject)
        {
            return layerMask |= (1 << gameObject.layer);
        }

        //public static LayerMask Include(this int layerMaskValue, params int[] addLayerMaskArray)
        //{
        //    int length = addLayerMaskArray.Length;

        //    for (int i = 0; i < addLayerMaskArray.Length; i++)
        //    {
        //        layerMaskValue |= addLayerMaskArray[i];
        //    }
        //    return layerMaskValue;
        //}

        public static LayerMask Include(this int layerMaskValue, params string[] addLayerMaskNameArray)
        {
            int paramLength = addLayerMaskNameArray?.Length ?? 0;

            for (int i = 0; i < paramLength; i++)
            {
                layerMaskValue |= (1 << LayerMask.NameToLayer(addLayerMaskNameArray[i]));
            }

            return layerMaskValue;
        }

        //public static LayerMask Include(this LayerMask layerMask, params int[] addLayerMaskArray)
        //{
        //    int length = addLayerMaskArray.Length;

        //    for (int i = 0; i < length; i++)
        //    {
        //        layerMask |= addLayerMaskArray[i];
        //    }

        //    return layerMask;
        //}

        public static LayerMask Include(this LayerMask layerMask, params string[] addLayerMaskNameArray)
        {
            int paramLength = addLayerMaskNameArray?.Length ?? 0;

            for (int i = 0; i < paramLength; i++)
            {
                layerMask |= (1 << LayerMask.NameToLayer(addLayerMaskNameArray[i]));
            }

            return layerMask;
        }

        #endregion 레이어 포함 반환

        #region 레이어 제외 반환

        public static LayerMask Exclude(this LayerMask layerMask, LayerMask removeLayerMask)
        {
            if (layerMask.value == 0)
            {
                return layerMask;
            }
            int[] removeLayers = removeLayerMask.ConvertToIntList();
            for (int i = 0; i < removeLayers.Length; i++)
            {
                if (LayerContains(layerMask, removeLayers[i]))
                {
                    layerMask ^= (1 << removeLayers[i]);
                }
            }
            return layerMask;
        }

        public static LayerMask Exclude(this LayerMask layerMask, int gameObjectLayer)
        {
            return layerMask.LayerContains(gameObjectLayer) ? (layerMask ^= (1 << gameObjectLayer)) : (layerMask);
        }

        public static LayerMask Exclude(this LayerMask layerMask, params string[] removeLayerMaskNameArray)
        {
            return Exclude(layerMask, (LayerMask)LayerMask.GetMask(removeLayerMaskNameArray));
        }

        #endregion 레이어 제외 반환
    }



    public static class LayerEditorUtil
    {
        #region <Layer> ContainsLayerList, AddLayerList
        private const int MaxLayerLength = 32;
        public static bool IsExists(string layerName, bool printLog = false)
        {
#if UNITY_EDITOR
            for (int i = 0; i < InternalEditorUtility.layers.Length; i++)
            {
                if (InternalEditorUtility.layers[i].Equals(layerName))
                {
                    if (printLog)
                    {
                        typeof(LayerMask).PrintLogWithClassName("레이어리스트에 '" + layerName + "' 가 존재합니다", LogType.Log);
                    }

                    return true;
                }
            }
            if (printLog)
            {
                typeof(LayerMask).PrintLogWithClassName("레이어리스트에 '" + layerName + "' 가 존재하지 않습니다", LogType.Warning);
            }
            return false;
#else
            return true;
#endif
        }

        public static void AddNewLayers(string[] layerNames, bool isPrintLog = true, bool isPrintExistsLog = false)
        {
            foreach (var layerName in layerNames)
            {
                AddNewLayer(layerName, isPrintLog);
            }
        }

        /// <summary>
        /// 에디터&&실행중이 아닐때여야지 레이어가 추가된것이 저장이됨(런타임중일때 추가되면 저장이 안되므로 주의)
        /// Reset에서 해주면 딱좋음
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static bool AddNewLayer(string layerName, bool isPrintLog = true, bool isPrintExistsLog = false)
        {
#if UNITY_EDITOR
            if (IsExists(layerName, false))
            {
                if (isPrintLog && isPrintExistsLog) typeof(LayerMask).PrintLogWithClassName("Layer list에 '" + layerName + "' 가 이미 존재해서 추가하지 않았습니다", LogType.Log);

                return true;
            }
            SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

            SerializedProperty layersProp = manager.FindProperty("layers");

            SerializedProperty prop;
            //레이어는 인덱스8 부터 사용자사용가능한 레이어임
            for (int i = 8; i < MaxLayerLength; i++)
            {
                prop = layersProp.GetArrayElementAtIndex(i);
                if (string.IsNullOrEmpty(prop.stringValue))
                {
                    prop.stringValue = layerName;
                    if (Application.isPlaying)
                    {
                        if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("Layer list에 '" + layerName + "' 를 일시적으로 추가했습니다.\n(저장되지는 않았습니다 실행중이 아닌 에디터모드에서만 저장됩니다)", isError: true);
                    }
                    else
                    {
                        if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("Layer list에 '" + layerName + "' 추가를 성공했습니다.");
                    }

                    manager.ApplyModifiedProperties();
                    return true;
                }
                if (i == MaxLayerLength - 1)
                {
                    if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("Layer list가 가득차서 '" + layerName + "' 를 추가하지 못했습니다.", isError: true);
                }
            }
            return false;

#else
            return false;
#endif
        }



        #endregion <Layer> ContainsLayerList, AddLayerList
    }

    public static class NavMeshAreaUtil
    {
        private const int MaxAreaLength = 32;

        public static bool IsExists(string areaName, bool printLog = false)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(areaName)) return false;

            foreach (var name in GameObjectUtility.GetNavMeshAreaNames())
            {
                if (name.Equals(areaName))
                {
                    if (printLog)
                    {
                        typeof(NavMeshAreaUtil).PrintLogWithClassName("NavMeshArea 리스트에 '" + areaName + "' 가 존재합니다", LogType.Log);
                    }
                    return true;
                }
            }

            if (printLog)
            {
                typeof(NavMeshAreaUtil).PrintLogWithClassName("NavMeshArea 리스트에 '" + areaName + "' 가 존재하지 않습니다", LogType.Warning);
            }
            return false;
#else
            return true;
#endif
        }
        public static bool AddNewArea(string areaName, float areaCost = 1f, bool isPrintLog = true, bool isPrintExistsLog = false)
        {
#if UNITY_EDITOR
            if (IsExists(areaName, false))
            {
                if (isPrintLog && isPrintExistsLog) typeof(LayerMask).PrintLogWithClassName("NavMeshArea 리스트에 '" + areaName + "' 가 이미 존재해서 추가하지 않았습니다", LogType.Log);

                return true;
            }
            SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/NavMeshAreas.asset")[0]);
            if (manager == null) return false;
            SerializedProperty areasProp = manager.FindProperty("areas");

            SerializedProperty nameProp, costProp;

            for (int i = 0; i < MaxAreaLength; i++)
            {
                var strcut = areasProp.GetArrayElementAtIndex(i);
                nameProp = strcut.FindPropertyRelative("name");
                
                if (string.IsNullOrEmpty(nameProp.stringValue))
                {
                    nameProp.stringValue = areaName;
                    costProp = strcut.FindPropertyRelative("cost");
                    costProp.floatValue = areaCost;
                    if (Application.isPlaying)
                    {
                        if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("NavMeshArea 리스트에 '" + areaName + "' 를 일시적으로 추가했습니다.\n(저장되지는 않았습니다 실행중이 아닌 에디터모드에서만 저장됩니다)", isError: true);
                    }
                    else
                    {
                        if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("NavMeshArea 리스트에 '" + areaName + "' 추가를 성공했습니다.");
                    }

                    manager.ApplyModifiedProperties();
                    return true;
                }
                if (i == MaxAreaLength - 1)
                {
                    if (isPrintLog) DisplayDialogUtil.DisplayDialogReflection("NavMeshArea 리스트가 가득차서 '" + areaName + "' 를 추가하지 못했습니다.", isError: true);
                }
            }
            return false;

#else
            return false;
#endif
        }
    }
}