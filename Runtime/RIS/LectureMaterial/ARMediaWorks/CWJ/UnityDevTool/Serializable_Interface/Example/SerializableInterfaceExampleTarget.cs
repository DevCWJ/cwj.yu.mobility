using UnityEngine;

public class SerializableInterfaceExampleTarget : MonoBehaviour, ITestSerialize
{
    public void PrintLog(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}