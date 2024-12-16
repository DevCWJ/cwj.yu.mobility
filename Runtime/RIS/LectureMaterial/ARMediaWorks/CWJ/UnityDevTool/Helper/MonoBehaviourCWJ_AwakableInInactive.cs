// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// namespace CWJ
// {
//     /// <summary>
//     ///     GameObject가 비활성화상태여도 불리게하는 MonoBehaviour <<실패
//     ///     <br />
//     ///     OnAwakeInInactive가 호출되는데
//     ///     <br />
//     ///     오브젝트가 활성화상태이면 유니티Awake를 통해 호출되고 비활성화상태이면 Helper에 의해 호출됨.
//     /// </summary>
//     public abstract class MonoBehaviourCWJ_AwakableInInactive : MonoBehaviour
//     {
//         public MonoBehaviourCWJ_AwakableInInactive() : base()
//         {
//
//         }
//
//         ~MonoBehaviourCWJ_AwakableInInactive()
//         {
//
//         }
//
//         public void AwakeInInactive()
//         {
//             _AwakeInInactive();
//         }
//
//         protected abstract void _AwakeInInactive();
//     }
//
// }
