using UnityEngine;

namespace EzPivot
{
    namespace Samples
    {
        [ExecuteInEditMode]
        public class MoveToParentPivotPosition : MonoBehaviour
        {
            public bool ShowRotation = false;

            void Update()
            {
                if (transform.parent)
                {
                    transform.position = transform.parent.position;

                    if(ShowRotation)
                        transform.rotation = transform.parent.rotation;
                }
            }
        }
    }
}
