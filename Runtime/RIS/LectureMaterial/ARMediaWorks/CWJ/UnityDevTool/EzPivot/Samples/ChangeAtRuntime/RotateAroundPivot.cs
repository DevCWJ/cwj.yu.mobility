using UnityEngine;

namespace EzPivot
{
    namespace Samples
    {
        public class RotateAroundPivot : MonoBehaviour
        {
            public float speed = 100f;

            void Update()
            {
                transform.rotation *= Quaternion.Euler(0f, speed * Time.deltaTime, 0f);
            }
        }
    }
}