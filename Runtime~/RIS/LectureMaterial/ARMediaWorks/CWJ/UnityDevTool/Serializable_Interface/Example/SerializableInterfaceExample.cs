using System;

using CWJ;
using CWJ.Serializable;

using UnityEngine;

public interface ITestSerialize
{
    void PrintLog(string message);
}

[Serializable] public class SI_Test : InterfaceSerializable<ITestSerialize> { }

public class SerializableInterfaceExample : MonoBehaviour, ITestSerialize
{
    public SI_Test[] sI_Test2Array;
    public SI_Test sI_Test2;

    public void PrintLog(string message)
    {
        Debug.LogError(message);
    }

    [InvokeButton]
    private void FindField()
    {
        ITestSerialize[] testSerialize = GetComponents<ITestSerialize>();
        sI_Test2Array = testSerialize.ToSerializableInterfaces<ITestSerialize, SI_Test>();
        sI_Test2 = SerializableInterfaceUtil.FindSerializableInterface<ITestSerialize, SI_Test>(includeInactive: false, includeDontDestroyOnLoadObjs: true);
        //sI_Test2 = FindExtension.FindInterface<ITestSerialize>(includeInactive: true).ConvertInterfaceSerializable<ITestSerialize, SI_Test>();

        sI_Test2Array = SerializableInterfaceUtil.FindSerializableInterfaces<ITestSerialize, SI_Test>(includeInactive: true, includeDontDestroyOnLoadObjs: true);
        //sI_Test2Array = FindExtension.FindInterfaces<ITestSerialize>(includeInactive: false).ConvertInterfacesSerializable<ITestSerialize, SI_Test>();
    }

    [InvokeButton]
    private void TestInterface()
    {
        sI_Test2.Interface.PrintLog("Test Serialize Interface.");
        foreach (var item in sI_Test2Array)
        {
            item.Interface.PrintLog("Test Serialize Interface Array");
        }
    }
}