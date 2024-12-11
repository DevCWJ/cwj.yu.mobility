using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWJ.YU.Mobility
{
    public class PositionChaser : MonoBehaviour
    {
        public Transform target;
        public Transform chaser;
        [SerializeField] float unitDistance = CoordinateAutoCreator.UnitDistance;
        //Vector3 UsersidePosConvertToUnityPos(Vector3 usersidePos)
        //{
        //    float unitySpaceX = usersidePos.y * -UnitDistance;
        //    float unitySpaceY = usersidePos.z * UnitDistance;
        //    float unitySpaceZ = usersidePos.x * UnitDistance;

        //    return new Vector3(unitySpaceX, unitySpaceY, unitySpaceZ);
        //}
        //Vector3 UnityPosConvertToUsersidePos(Vector3 localPos)
        //{
        //    float usersideY = localPos.x / -UnitDistance;
        //    float usersideZ = localPos.y / UnitDistance;
        //    float usersideX = localPos.z / UnitDistance;

        //    return new Vector3(usersideX, usersideY, usersideZ);
        //}
        public Vector3 GetChaserUsersidePosition()
        {
            return Extension.UnityPosConvertToUsersidePos(chaser.localPosition, unitDistance);
        }

        private void Update()
        {
            chaser.position = target.position;
        }
    }
}
