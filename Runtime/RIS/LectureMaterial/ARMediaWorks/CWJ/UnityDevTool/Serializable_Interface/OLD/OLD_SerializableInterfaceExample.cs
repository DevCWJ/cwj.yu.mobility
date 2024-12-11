//using UnityEngine;
//using CWJ.Serializable;

//public interface IOldTestInterface
//{
//    void PrintLog(string message);
//}

//[System.Serializable] public class SI_OldTest : OLD_InterfaceSerializable<IOldTestInterface> { public SI_OldTest(Component c) : base(c) { } }
//public class OLD_SerializableInterfaceExample : MonoBehaviour
//{
//    public SI_OldTest si_test;

//    [ContextMenu("Get")]
//    void GetInterfaceInit()
//    {
//        si_test = transform.GetSerializeInterface<IOldTestInterface, SI_OldTest>((c) => new SI_OldTest(c));
//    }

//    [ContextMenu("Test")]
//    void TestInterface()
//    {
//        si_test.@interface.PrintLog("Test Serialize Interface");
//    }

//}