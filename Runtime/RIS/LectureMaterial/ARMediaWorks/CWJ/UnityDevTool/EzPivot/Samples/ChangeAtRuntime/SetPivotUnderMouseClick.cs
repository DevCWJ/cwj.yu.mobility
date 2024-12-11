using UnityEngine;

namespace EzPivot
{
    namespace Samples
    {
        [RequireComponent(typeof(Collider))]
        public class SetPivotUnderMouseClick : MonoBehaviour
        {
            public Transform target;

            void Update()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (target && hit.transform == transform)
                        {
                            Debug.Log("Change pivot position to " + hit.point);
                            EzPivot.API.SetPivotPosition(target.transform, hit.point);
                        }
                    }
                }
            }
        }
    }
}
