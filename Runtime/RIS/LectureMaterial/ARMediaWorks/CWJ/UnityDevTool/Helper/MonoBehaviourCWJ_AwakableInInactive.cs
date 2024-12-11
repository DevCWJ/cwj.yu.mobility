using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CWJ
{

    public abstract class MonoBehaviourCWJ_AwakableInInactive : MonoBehaviour
    {
        public MonoBehaviourCWJ_AwakableInInactive() : base()
        {
            SingletonHelper.AddAwakableInInactive(this);
        }

        ~MonoBehaviourCWJ_AwakableInInactive()
        {
            SingletonHelper.RemoveAwakableInInactive(this);
        }

        public void AwakeInInactive()
        {
            _AwakeInInactive();
        }

        protected abstract void _AwakeInInactive();
    }

}