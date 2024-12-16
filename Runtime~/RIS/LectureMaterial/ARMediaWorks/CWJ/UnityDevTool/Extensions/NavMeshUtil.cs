using UnityEngine;
using UnityEngine.AI;

namespace CWJ
{
    public static class NavMeshUtil
    {
        public static Vector3 ConvertNavPosition(this Vector3 toPos, float sampleDist, string navMeshAreaName)
        {
            return toPos.ConvertNavPosition(sampleDist, 1 << NavMesh.GetAreaFromName(navMeshAreaName));
        }

        public static Vector3 ConvertNavPosition(this Vector3 toPos, float sampleDist, int navAreaMask)
        {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(toPos, out navHit, sampleDist, navAreaMask) && !navHit.position.Equals(new Vector3(0, 0, 0)))
            {
                return navHit.position;
            }
            else
            {
                return toPos;
            }
        }

        public static Vector3 GetNavRandomPosition(this Vector3 originPos, float dist, int navAreaMask)
        {
            var randDirection = Random.insideUnitSphere * dist;
            randDirection += originPos;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randDirection, out navHit, dist, navAreaMask);
            return navHit.position;
        }

        public static float GetDistBetweenNavPoint(this Vector3 startPoint, Vector3 targetPoint, int navAreaMask, ref NavMeshPath navMeshPath)
        {
            navMeshPath.ClearCorners();
            if (NavMesh.CalculatePath(startPoint, targetPoint, NavMesh.AllAreas, navMeshPath))
            {
                if (navMeshPath.status != NavMeshPathStatus.PathInvalid && navMeshPath.corners.Length > 1)
                    return navMeshPath.corners.GetAllDist();
            }
            return -1;
        }

        public static Vector3 Vector3ArrayLerp(this Vector3[] vector3s, float t)
        {
            if (t <= 0f) return vector3s[0];
            if (t >= 1f) return vector3s[vector3s.Length - 1];

            float len = 0f;
            for (int i = 1; i < vector3s.Length; i++) len += Vector3.Distance(vector3s[i], vector3s[i - 1]);

            float dt = len * t;
            float d = 0f;
            for (int i = 1; i < vector3s.Length; i++)
            {
                float distanceBetweenPoints = d + Vector3.Distance(vector3s[i], vector3s[i - 1]);
                if (dt < d + distanceBetweenPoints)
                {
                    t = (dt - d) / distanceBetweenPoints;
                    return Vector3.Lerp(vector3s[i - 1], vector3s[i], t);
                }
                d += distanceBetweenPoints;
            }

            return vector3s[vector3s.Length - 1];
        }
    }
}