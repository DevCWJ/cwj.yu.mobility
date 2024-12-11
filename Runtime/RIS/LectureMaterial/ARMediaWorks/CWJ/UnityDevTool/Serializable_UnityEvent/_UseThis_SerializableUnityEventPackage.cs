using UnityEngine;
using UnityEngine.Events;

namespace CWJ
{
    [System.Serializable] public class UnityEvent_Void : UnityEvent { }

    [System.Serializable] public class UnityEvent_Bool : UnityEvent<bool> { }

    [System.Serializable] public class UnityEvent_Byte : UnityEvent<byte> { }

    [System.Serializable] public class UnityEvent_Int : UnityEvent<int> { }

    [System.Serializable] public class UnityEvent_Float : UnityEvent<float> { }

    [System.Serializable] public class UnityEvent_String : UnityEvent<string> { }

    [System.Serializable] public class UnityEvent_GameObject : UnityEvent<GameObject> { }

    [System.Serializable] public class UnityEvent_Transform : UnityEvent<Transform> { }

}