#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace CWJ.AccessibleEditor
{
    public static partial class EditorGUI_CWJ
    {
        private const int ArrayMaxSize = 4443; //4444-1

        private delegate (bool isArray, Type elemType, T list) ArrayConstructorHandler<T>(Type type, object value, ref bool isValueChangedViaCode) where T : IList;
        private delegate T ArrayUpdateAndDrawHandler<T>(Type elemType, string name, T list, int newLength, ref bool isValueChangedViaCode, DrawVariousTypeHandler drawElemVariousType, Action undoRecord) where T : IList;
        private delegate T ArrayDragAndDropHandler<T>(Rect rect, Type elemType, string name, T list, ref bool isValueChangedViaCode, Action undoRecord) where T : IList;

        private static Array DrawArrayType(Type type, string name, object value, ref bool isValueChangedViaCode, int reflectObjInstanceID, DrawVariousTypeHandler drawElemVariousType = null, Action undoRecord = null)
        {
            return DrawArrangementCore<Array>(type, name, value, ref isValueChangedViaCode, drawElemVariousType, reflectObjInstanceID,
                                                OnArrayConstructor,
                                                undoRecord, OnArrayUpdateAndDraw,
                                                OnArrayDragAndDrop);
        }


        private static IList DrawListType(Type type, string name, object value, ref bool isValueChangedViaCode, int reflectObjInstanceID, DrawVariousTypeHandler drawElemVariousType = null, Action undoRecord = null)
        {
            return DrawArrangementCore<IList>(type, name, value, ref isValueChangedViaCode, drawElemVariousType, reflectObjInstanceID,
                                                OnListConstructor,
                                                undoRecord, OnListUpdateAndDraw,
                                                OnListDragAndDrop);
        }


        /// <summary>
        /// Array, List를 그려주는 역할
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="isValueChangedViaCode">코드를 통해 값이 바뀌었는지</param>
        /// <param name="constructor">생성자</param>
        /// <param name="updateAndDraw">배열을 최신화 후 그려줌</param>
        /// <param name="dragAndDrop">드래그로 옮긴 오브젝트들을 배열에 추가해줌</param>
        /// <returns></returns>
        private static T DrawArrangementCore<T>(Type type, string name, object value, ref bool isValueChangedViaCode, DrawVariousTypeHandler drawElemVariousType, int reflectObjInstanceID,
                                                ArrayConstructorHandler<T> constructor,
                                                Action undoRecord, ArrayUpdateAndDrawHandler<T> updateAndDraw,
                                                ArrayDragAndDropHandler<T> dragAndDrop) where T : IList
        {
            var listInfo = constructor(type, value, ref isValueChangedViaCode);
            bool isArray = listInfo.isArray;
            Type elemType = listInfo.elemType;

            if (drawElemVariousType == null) drawElemVariousType = GetDrawVariousTypeDelegate(elemType);
            if (drawElemVariousType == null) return default(T);

            T list = listInfo.list;

            var cacheKey = new FoldoutCacheKey(reflectObjInstanceID, name);

            if (!FoldoutCacheDict.TryGetValue(cacheKey, out bool foldoutBackup))
            {
                FoldoutCacheDict.Add(cacheKey, false);
            }
            bool isFoldout = foldoutBackup;

            bool prevGuiEnabled = GUI.enabled;
            GUI.enabled = true;
            bool isHierarchyMode = EditorGUIUtility.hierarchyMode;
            EditorGUIUtility.hierarchyMode = false;
            
            var rect = EditorGUILayout.BeginVertical(EditorGUICustomStyle.Box);

            EditorGUILayout.BeginHorizontal();
            
            string foldoutName = name + "  ";

            int listCnt = list.Count;
            int newListCnt = listCnt;
            if (!isFoldout)
            {
                foldoutName += "( " + (isArray ? $"{elemType.Name}[{listCnt}]" : $"List<{elemType.Name}>({listCnt})") + " )";
            }

            if (isFoldout = EditorGUILayout.Foldout(isFoldout, foldoutName, true, EditorStyles.foldout))
            {
                using (new EditorGUI.DisabledScope(!prevGuiEnabled))
                {
                    newListCnt = EditorGUILayout.DelayedIntField(listCnt, GUILayout.MaxWidth(60), GUILayout.MinWidth(0));

                    if (newListCnt < 0 || newListCnt >= ArrayMaxSize)
                    {
                        RemoveFocusFromText();
                        newListCnt = listCnt;
                    }
                    using (new EditorGUI.DisabledScope(newListCnt <= 0))
                    {
                        if (GUILayout.Button("-", GUILayout.MaxWidth(20), GUILayout.MinWidth(0)))
                        {
                            RemoveFocusFromText();
                            --newListCnt;
                        }
                    }
                    using (new EditorGUI.DisabledScope(newListCnt >= ArrayMaxSize))
                    {
                        if (GUILayout.Button("+", GUILayout.MaxWidth(20), GUILayout.MinWidth(0)))
                        {
                            RemoveFocusFromText();
                            ++newListCnt;
                        }
                    }
                }
            }

            if(foldoutBackup != isFoldout)
            {
                FoldoutCacheDict[cacheKey] = isFoldout;
            }

            //EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndHorizontal();

            if (isFoldout)
            {
                DrawLine();
                if (newListCnt == 0)
                {
                    EditorGUILayout.PrefixLabel((isArray ? $"Length : {list.Count}" : $"Count : {list.Count}"));
                }
                using (new EditorGUI.DisabledScope(!prevGuiEnabled))
                {
                    list = updateAndDraw(elemType, name, list, newListCnt, ref isValueChangedViaCode, drawElemVariousType, undoRecord);
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUIUtility.hierarchyMode = isHierarchyMode;
            GUI.enabled = prevGuiEnabled;
            return dragAndDrop(rect, elemType, name, list, ref isValueChangedViaCode, undoRecord);
        }

        #region Array/List Event Material
        /////////////////////
        /// Constructor /////
        /////////////////////
        private static (bool isArray, Type elemType, Array array) OnArrayConstructor(Type fieldType, object value, ref bool isValueChangedViaCode)
        {
            Type elemType = fieldType.GetElementType();
            Array array = (value as Array);
            if (array == null)
            {
                array = _ArrayConstructor(elemType);
                isValueChangedViaCode = true;
            }

            return (true, elemType, array);
        }

        private static Array _ArrayConstructor(Type elemType)
        { //Required verification : array == null
            return Array.CreateInstance(elemType, 0);
        }

        private static (bool isArray, Type elemType, IList list) OnListConstructor(Type fieldType, object value, ref bool isValueChangedViaCode)
        {

            IList list;
            if (value == null || value.Equals(null))
            {
                list = _ListConstructor(fieldType);
                isValueChangedViaCode = true;
            }
            else
            {
                list = value as IList;
            }
            return (false, fieldType.GetGenericArguments()[0], list);
        }

        private static IList _ListConstructor(Type fieldType)
        { //Required verification : (value == null || value.Equals(null))
            return Activator.CreateInstance(fieldType) as IList;
        }

        /////////////////////
        /// UpdateAndDraw ///
        /////////////////////
        private static Array OnArrayUpdateAndDraw(Type elemType, string name, Array array, int newLength, ref bool isValueChangedViaCode, 
                                                    DrawVariousTypeHandler drawElemVariousType, Action undoRecord)
        {
            //Add or Remove
            if (newLength != array.Length)
            {
                undoRecord?.Invoke();
                array = _UpdateArrayLength(elemType, newLength, array);
                isValueChangedViaCode = true;
                //FoldoutCacheDict.ChangeHashKey(cacheKey, new FoldoutCacheKey(array.GetHashCode(), name)); // array가 Array.CreateInstance로 재할당 되면서 HashCode값이 바뀔 수 있음. //cacheKey의 구조가 reflectObj의 (instanceID, field이름)으로 바뀌면서 필요없어짐
            }

            //Draw
            int drawLength = array.Length <= ArrayMaxSize ? array.Length : ArrayMaxSize;
            for (int i = 0; i < drawLength; ++i)
            {
                bool isElemValueChangedViaCode = false;
                EditorGUI.BeginChangeCheck();
                var newElemValue = drawElemVariousType(elemType, (/*name +*/ "Element " + i), array.GetValue(i), ref isElemValueChangedViaCode);
                bool isGUIChanged = EditorGUI.EndChangeCheck();
                if (isGUIChanged || isElemValueChangedViaCode)
                {
                    if (isGUIChanged) undoRecord?.Invoke();
                    array.SetValue(newElemValue, i);
                    if(isElemValueChangedViaCode) isValueChangedViaCode = true;
                }
            }
            return array;
        }
        private static Array _UpdateArrayLength(Type elemType, int newLength, Array array)
        { //Required verification : newLength != array.Length

            Array insArray = Array.CreateInstance(elemType, newLength);
            Array.Copy(array, insArray, Math.Min(array.Length, newLength));
            return insArray;
        }

        private static IList OnListUpdateAndDraw(Type elemType, string name, IList list, int newCount, ref bool isValueChangedViaCode, 
                                                DrawVariousTypeHandler drawElemVariousType, Action undoRecord)
        {
            if (newCount != list.Count)
            {
                undoRecord?.Invoke();
                list = _UpdateListCount(elemType, newCount, list);
                isValueChangedViaCode = true;
            }

            //Draw
            int drawCount = list.Count <= ArrayMaxSize ? list.Count : ArrayMaxSize;
            for (int i = 0; i < drawCount; ++i)
            {
                bool isElemValueChangedViaCode = false;
                EditorGUI.BeginChangeCheck();
                var newElemValue = drawElemVariousType(elemType, ("Element " + i), list[i], ref isElemValueChangedViaCode);
                bool isGUIChanged = EditorGUI.EndChangeCheck();
                if (isGUIChanged || isElemValueChangedViaCode)
                {
                    if (isGUIChanged) undoRecord?.Invoke();
                    list[i] = newElemValue;
                    if (isElemValueChangedViaCode) isValueChangedViaCode = true;
                }
            }
            return list;
        }

        private static IList _UpdateListCount(Type elemType, int newCount, IList list)
        { // Required verification : newCount != list.Count
            if (list.Count > newCount) // Remove
            {
                for (int i = list.Count - 1; i >= newCount; --i)
                {
                    list.RemoveAt(i);
                }
            }
            else if (list.Count < newCount) // Add
            {
                if (list.Count == 0)
                {
                    if (typeof(UnityObject).IsAssignableFrom(elemType)) list.Add(null);
                    else if (elemType.IsGenericType && elemType.GetGenericTypeDefinition() == typeof(List<>) /*IsList*/) list.Add(null); // recursive call에서 init할거임.
                    else if (elemType.IsClass) list.Add(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(elemType));
                    else list.Add(Activator.CreateInstance(elemType));
                }

                Func<int, object> getNewListElement;

                if (typeof(UnityObject).IsAssignableFrom(elemType)) getNewListElement = (index) => list[index - 1];
                else if (elemType.IsGenericType && elemType.GetGenericTypeDefinition() == typeof(List<>) /*IsList*/) getNewListElement = (index) => null;
                else if (elemType.IsClass) getNewListElement = (index) => System.Runtime.Serialization.FormatterServices.GetUninitializedObject(elemType);
                else getNewListElement = (index) => list[index - 1];

                for (int i = list.Count; i < newCount; ++i)
                {
                    list.Add(getNewListElement(i));
                }
            }
            return list;
        }

        /////////////////////
        /// DragAndDrop /////
        /////////////////////
        private static Array OnArrayDragAndDrop(Rect rect, Type elemType, string name, Array array, ref bool isValueChangedViaCode, Action undoRecord)
        {
            Func<UnityObject, UnityObject> getCompFunc = null;
            UnityObject[] dragObjs = null;

            if(! DragAndDropAddCondition(rect, elemType, ref getCompFunc, ref dragObjs))
            {
                return array;
            }

            List<UnityObject> compList = new List<UnityObject>();
            for (int i = 0; i < dragObjs.Length; i++)
            {
                var component = getCompFunc(dragObjs[i]);
                if (component != null)
                {
                    compList.Add(component);
                }
            }

            if (compList.Count == 0) return array;

            undoRecord?.Invoke();

            int prevArrayLength = array.Length;
            int compCnt = compList.Count;

            array = _UpdateArrayLength(elemType, prevArrayLength + compCnt, array);

            for (int i = 0; i < compCnt; i++)
            {
                array.SetValue(compList[i], prevArrayLength + i);
            }

            //FoldoutCacheDict.ChangeHashKey(hashKey, (array.GetHashCode(), name));

            Event.current.Use();
            isValueChangedViaCode = true;
            GUI.changed = true;

            return array;
        }

        private static IList OnListDragAndDrop(Rect rect, Type elemType, string name, IList list, ref bool isValueChangedViaCode, Action undoRecord)
        {
            Func<UnityObject, UnityObject> getCompFunc = null;
            UnityObject[] dragObjs = null;

            if (! DragAndDropAddCondition(rect, elemType, ref getCompFunc, ref dragObjs))
            {
                return list;
            }

            List<UnityObject> compList = new List<UnityObject>();
            for (int i = 0; i < dragObjs.Length; i++)
            {
                var component = getCompFunc(dragObjs[i]);
                if (component != null)
                {
                    compList.Add(component);
                }
            }
            if (compList.Count == 0) return list;

            undoRecord?.Invoke();

            int compCnt = compList.Count;

            for (int i = 0; i < compCnt; i++)
            {
                list.Add(compList[i]);
            }

            Event.current.Use();
            isValueChangedViaCode = true;
            GUI.changed = true;

            return list;
        }

        private static bool DragAndDropAddCondition(Rect rect, Type elemType, ref Func<UnityObject, UnityObject> getCompFunc, ref UnityObject[] dragObjs)
        {
            if (!GUI.enabled) return false;
            if (!typeof(UnityObject).IsAssignableFrom(elemType) && !elemType.IsInterface) return false;
            if (!rect.Contains(Event.current.mousePosition)) return false;

            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
                return false;
            }
            else if (Event.current.type == EventType.DragPerform)
            {
                dragObjs = DragAndDrop.objectReferences;

                if (dragObjs.Length == 0 || dragObjs[0] == null) return false;

                Type dragObjType = dragObjs[0].GetType();

                if (elemType.IsAssignableFrom(dragObjType)) getCompFunc = (o) => o;
                else if (dragObjType == typeof(GameObject)) getCompFunc = (o) => (o as GameObject)?.GetComponent(elemType);
                else return false;

                return true;
            }
            else return false;
        }
        #endregion Array/List Event Material

        public static void DrawObjectsField<T>(string arrayName, ref T[] objs, ref bool isFoldout, ref Vector2 scrollViewPos, bool isReadonlyLength = false, Action updateCallback = null) where T : UnityObject
        {
            if (updateCallback != null)
            {
                EditorGUILayout.BeginHorizontal();
            }

            isFoldout = EditorGUILayout.Foldout(isFoldout, arrayName, true);

            if (updateCallback != null)
            {
                if (GUILayout.Button("Update")) updateCallback.Invoke();
                EditorGUILayout.EndHorizontal();
            }

            if (isFoldout)
            {
                scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);
                EditorGUI.indentLevel++;
                using (new EditorGUI.DisabledScope(isReadonlyLength))
                {
                    int length = EditorGUILayout.IntField("size", objs.Length);
                    if (!isReadonlyLength && length != objs.Length)
                    {
                        Array.Resize(ref objs, length);
                    }
                }
                GUIContent content = new GUIContent();
                for (int i = 0; i < objs.Length; ++i)
                {
                    content.text = "element " + i;
                    EditorGUILayout.ObjectField(content, objs[i], typeof(Transform), allowSceneObjects: true);
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndScrollView();
            }
        }
    }
}
#endif