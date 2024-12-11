using System;


using UnityEngine;
using CWJ.Singleton.OnlyUseNew;

namespace CWJ.Singleton.SwapSingleton
{
    [CWJInfoBox]
    public abstract class SingletonBehaviour_Swap<T> : SingletonBehaviour_OnlyUseNew<T> where T : MonoBehaviour
    {
        protected override void _Start()
        {
            //Swap
            if (this.GetType().Equals(TargetType)) //root type일 경우에만 실행
            {
                var swapType = GetSwapType();
                if (swapType == null || !swapType.BaseType.Equals(TargetType))
                {
                    Debug.LogError("이게 아부지도 없는 게 까불어!");
                    return;
                }

                T child = gameObject.AddComponent(swapType) as T;

                gameObject.name = swapType.Name;
                SwapSetting(child);
            }
        }

        protected abstract Type GetSwapType();

        /// <summary>
        /// Reflection으로 하면 느릴까봐 그냥 수작업으로 넣게 유도함.
        /// <para/>변수가 너무많은경우엔 그냥 reflection이 나으려나..
        /// </summary>
        protected abstract void SwapSetting(T swapComp);

    }
}