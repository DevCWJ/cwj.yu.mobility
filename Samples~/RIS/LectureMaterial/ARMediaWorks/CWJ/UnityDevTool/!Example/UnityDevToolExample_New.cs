using System;
using System.Collections;
using System.Collections.Generic;

using CWJ;
using CWJ.AccessibleEditor;

using UnityEngine;

public struct StructForTest
{
    public int @int;
    public string str;
    public Vector3 vector3;
}

public interface IVisualizedInterface
{
    StructForTest StructTest(StructForTest @struct);
}

[CWJ.VisualizeField_All, CWJ.VisualizeProperty_All]
public class UnityDevToolExample_New : MonoBehaviour, 
                                     InspectorHandler.ISelectHandler,
                                    InspectorHandler.IOnGUIHandler,
                                    InspectorHandler.IDeselectHandler,
                                    InspectorHandler.IDestroyHandler,
                                    InspectorHandler.ICompiledHandler,
                                    IVisualizedInterface
{
    //interface
    [CWJ.VisualizeField] public IVisualizedInterface[] visualizedInterface;

    //static field
    [CWJ.VisualizeField] public static UnityDevToolExample_New StaticField;
    [CWJ.VisualizeProperty]
    public static UnityDevToolExample_New StaticProperty
    {
        get => StaticField;
        set => StaticField = value;
    }

    //private field
    [CWJ.VisualizeField] private GameObject[] privateFields;

    //property
    private List<Collider> getSetPropertyList { get; set; }
    private bool getProperty => true;
    private string setProperty { set => Debug.LogError(value); }

    [InvokeButton]
    void SwapTest()
    {
        int a, b;
        void init()
        {
            a = 3; b = 5;
        }

        init();
        a ^= b ^= a ^= b;
        Debug.Log($"0>>  a:{a}  b:{b}");//안됨

        init();
        a = a ^ b;
        b = b ^ a;
        a = a ^ b;
        Debug.Log($"1>>  a:{a}  b:{b}");//됨

        init();
        (a, b) = (b, a);
        Debug.Log($"2>>  a:{a}  b:{b}");//됨
    }

    [InvokeButton]
    string Boob(string s)
    {
        return s;
    }

    [InvokeButton]
    private void ToDetailedStringTest(List<Transform> trfList = null)
    {
        //List<T>
        Debug.LogError(trfList.ToStringByDetailed());

        //T[]
        Transform[] trfArray = trfList.ToArray();
        Debug.LogError(trfArray.ToStringByDetailed());

        //Array
        System.Array arr = Array.CreateInstance(typeof(Transform), trfList.Count);
        int i = 0;
        foreach (var item in trfList)
            arr.SetValue(item, i++);
        Debug.LogError(arr.ToStringByDetailed());

        //Array (Tuple)
        string str = "cwj"; int @int = 94; Transform[] trfs = trfArray;
        var tuple = (str, @int, trfs);
        System.Array tupleArr = Array.CreateInstance(tuple.GetType(), 1);
        tupleArr.SetValue(tuple, 0);
        Debug.LogError(tupleArr.ToStringByDetailed());

        //Dictionary (string, ValueTuple)
        Dictionary<string, ValueTuple<string, int, Transform[]>> dictionary = new Dictionary<string, ValueTuple<string, int, Transform[]>>();
        dictionary.Add("CWJ_0", tuple);
        dictionary.Add("CWJ_1", tuple);
        Debug.LogError(dictionary.ToStringByDetailed());
    }

    [InvokeButton]
    public IEnumerator CountCoroutine(int max)
    {
        if (max == 0) max = 6;
        int current = 0;
        while (current < max)
        {
            Debug.LogWarning(ReflectionUtil.GetPrevMethodName() + " : " + current++);
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }


    [CWJ.InvokeButton]
    public StructForTest StructTest(StructForTest @struct)
    {
        return @struct;
    }

    [InvokeButton]
    private Transform[] ArrayTest(Transform[] trfs = null)
    {
        return trfs;
    }

    [InvokeButton]
    private (string, int, List<GameObject>) TupleTest(string str = "cwj", int @int = 94, List<GameObject> objs = null)
    {
        return (str, @int, objs);
    }

    [InvokeButton]
    private ValueTuple<string, int, List<GameObject>> ValueTupleTest(ValueTuple<string, int, List<GameObject>> valueTuple)
    {
        return valueTuple;
    }

    public void CWJEditor_OnSelect(MonoBehaviour target)
    {
        Debug.Log($"[CWJ Editor Event] {"OnSelect".SetStyle(new Color().GetCommentsColor(), isBold: true, isViewOneLine: true, size: 23)} ! (this: {gameObject.name} // Inspector target: {target.name})", gameObject);
    }

    public void CWJEditor_OnDeselect(MonoBehaviour target)
    {
        Debug.Log($"[CWJ Editor Event] {"OnDeselect".SetStyle(new Color().GetCommentsColor(), isBold: true, isViewOneLine: true, size: 23)} ! (this: {gameObject.name} // Inspector target: {target.name})", gameObject);
    }

    public void CWJEditor_OnGUI()
    {
        Debug.Log($"[CWJ Editor Event] {"OnGUI".SetStyle(new Color().GetCommentsColor(), isBold: true, isViewOneLine: true, size: 23)} by inspector ! ({gameObject.name})", gameObject);
    }

    public void CWJEditor_OnDestroy()
    {
        Debug.Log($"[CWJ Editor Event] {"OnDestroy".SetStyle(new Color().GetCommentsColor(), isBold: true, isViewOneLine: true, size: 23)} by user ! "); //gameObject is Null
    }

    public void CWJEditor_OnCompile()
    {
        Debug.Log($"[CWJ Editor Event] {"OnCompile".SetStyle(new Color().GetCommentsColor(), isBold: true, isViewOneLine: true, size: 23)} ({gameObject.name})", gameObject); //gameObject is Null
    }
}
