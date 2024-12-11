using UnityEngine;

namespace CWJ
{
    public static class TransformUtil
    {


        public static Vector3 NormalizeEulerAngle(Vector3 eulerAngle)
        {
            return new Vector3(NormalizeAngle(eulerAngle.x), NormalizeAngle(eulerAngle.y), NormalizeAngle(eulerAngle.z));
        }

        /// <summary>
        /// eulerAngle to 0' ~ 360'
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float NormalizeAngle(float angle)
        {
            // 각도를 0에서 360도 사이로 정규화
            angle = angle % 360; // 0에서 360도를 초과하는 값을 제거
            if (angle < 0) angle += 360; // 음수인 경우 360도를 더해줌

            return angle;
        }

        /// <summary>
        /// -90 ~ 270
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float WrapAngle(float angle)
        {
            angle %= 360;
            if (angle > 180)
                return angle - 360;

            return angle;
        }

        /// <summary>
        /// -180 ~ 180
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public  static float UnwrapAngle(float angle)
        {
            if (angle >= 0)
                return angle;

            angle = -angle % 360;

            return 360 - angle;
        }

        public static void Reset(this Transform trf)
        {
            trf.localPosition = Vector3.zero;
            trf.localRotation = Quaternion.identity;
            trf.localScale = Vector3.one;
        }

        //public static Transform CopyTo(this Transform trf, Transform res)
        //{
        //    trf.SetParent(res, false);
        //    trf.Reset();
        //    trf.SetParent(res.parent);
        //    trf.gameObject.name = res.gameObject.name;
        //    return trf;
        //}

        public static void SetParentAndReset(this Transform trf, Transform parent, bool isFirstSibling = true)
        {
            trf.SetParent(parent, true);
            trf.Reset();
            if (isFirstSibling)
            {
                trf.SetAsFirstSibling();
            }
        }

        /// <summary>
        /// 월드좌표값으로 이동시키기위한 로컬좌표값을 줌
        /// </summary>
        /// <param name="transform">오브젝트</param>
        /// <param name="worldPosition">원하는위치</param>
        /// <returns></returns>
        public static Vector3 WorldToLocalPosition(this Transform transform, Vector3 worldPosition)
        {
            return (transform.parent == null) ? worldPosition : transform.parent.InverseTransformPoint(worldPosition);

            //return (transform.parent == null) ? worldPosition : transform.InverseTransformPoint(worldPosition);
        }

        /// <summary>
        /// 원하는 Lossy스케일값 설정을 위한 로컬스케일 값을 알아냄
        /// <br/>(위험)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="lossySize"></param>
        /// <returns></returns>
        public static Vector3 LossyToLocalScale(this Transform transform, Vector3 lossyScale)
        {
            Vector3 lastLocalScale = transform.localScale;
            transform.localScale = Vector3.one;
            Matrix4x4 m = transform.worldToLocalMatrix;
            m.SetColumn(0, new Vector4(m.GetColumn(0).magnitude, 0f));
            m.SetColumn(1, new Vector4(0f, m.GetColumn(1).magnitude));
            m.SetColumn(2, new Vector4(0f, 0f, m.GetColumn(2).magnitude));
            m.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            Vector3 returnLocalScale = m.MultiplyPoint(lossyScale);
            transform.localScale = lastLocalScale;
            return returnLocalScale;
        }

        public static Transform Lerp(Transform start, Transform end, float t)
        {
            start.position = Vector3.LerpUnclamped(start.position, end.position, t);
            start.rotation = Quaternion.SlerpUnclamped(start.rotation, end.rotation, t);
            start.localScale = Vector3.Lerp(start.localScale, end.localScale, t);
            return start;
        }

        public static Transform Slerp(Transform start, Transform end, float t)
        {
            start.position = Vector3.SlerpUnclamped(start.position, end.position, t);
            start.rotation = Quaternion.SlerpUnclamped(start.rotation, end.rotation, t);
            start.localScale = Vector3.LerpUnclamped(start.localScale, end.localScale, t);
            return start;
        }

        #region Convert Methods

        public static Matrix4x4 GetMatrix(this Transform transform)
        {
            return transform.localToWorldMatrix;
        }

        public static Matrix4x4 GetRelativeMatrix(this Transform transform, Transform relativeTo)
        {
            var m = transform.localToWorldMatrix;
            return relativeTo.worldToLocalMatrix * m;
        }

        public static Matrix4x4 GetLocalMatrix(this Transform transform)
        {
            return Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        }

        #endregion Convert Methods

        #region Matrix Methods

        public static Vector3 GetTranslation(this Matrix4x4 m)
        {
            var col = m.GetColumn(3);
            return new Vector3(col.x, col.y, col.z);
        }

        public static Quaternion GetRotation(this Matrix4x4 m)
        {
            //http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
            Quaternion q = new Quaternion();
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
            q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
            q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
            q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
            return q;
        }

        public static Vector3 GetScale(this Matrix4x4 m)
        {
            return new Vector3(m.GetColumn(0).magnitude, m.GetColumn(1).magnitude, m.GetColumn(2).magnitude);
        }

        #endregion Matrix Methods

        #region Parent

        public static Vector3 ParentTransformPoint(this Transform t, Vector3 pnt)
        {
            if (t.parent == null) return pnt;
            return t.parent.TransformPoint(pnt);
        }

        public static Vector3 ParentInverseTransformPoint(this Transform t, Vector3 pnt)
        {
            if (t.parent == null) return pnt;
            return t.parent.InverseTransformPoint(pnt);
        }

        #endregion Parent

        #region Transform Methods

        public static void ZeroOut(this GameObject go, bool isIgnoreScale, bool isGlobal = false)
        {
            if (isGlobal)
            {
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                if (!isIgnoreScale) go.transform.localScale = Vector3.one;
            }
            else
            {
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                if (!isIgnoreScale) go.transform.localScale = Vector3.one;
            }

            var rb = go.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public static void ZeroOut(this Transform transform, bool isIgnoreScale, bool isGlobal = false)
        {
            if (isGlobal)
            {
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
                if (!isIgnoreScale) transform.localScale = Vector3.one;
            }
            else
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                if (!isIgnoreScale) transform.localScale = Vector3.one;
            }
        }

        public static void ZeroOut(this Rigidbody body)
        {
            if (body.isKinematic) return;

            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }

        public static Vector3 GetRelativePosition(this Transform transform, Transform relativeTo)
        {
            return relativeTo.InverseTransformPoint(transform.position);
        }

        public static Quaternion GetRelativeRotation(this Transform transform, Transform relativeTo)
        {
            //return transform.rotation * Quaternion.Inverse(relativeTo.rotation);
            return Quaternion.Inverse(relativeTo.rotation) * transform.rotation;
            //return Quaternion.Inverse(transform.rotation) * relativeTo.rotation;
        }

        /// <summary>
        /// Multiply a vector by only the scale part of a transformation
        /// </summary>
        /// <param name="m"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 ScaleVector(this Matrix4x4 m, Vector3 v)
        {
            var sc = m.GetScale();
            return Matrix4x4.Scale(sc).MultiplyPoint(v);
        }

        public static Vector3 ScaleVector(this Transform t, Vector3 v)
        {
            var sc = t.localToWorldMatrix.GetScale();
            return Matrix4x4.Scale(sc).MultiplyPoint(v);
        }

        /// <summary>
        /// Inverse multiply a vector by on the scale part of a transformation
        /// </summary>
        /// <param name="m"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 InverseScaleVector(this Matrix4x4 m, Vector3 v)
        {
            var sc = m.inverse.GetScale();
            return Matrix4x4.Scale(sc).MultiplyPoint(v);
        }

        public static Vector3 InverseScaleVector(this Transform t, Vector3 v)
        {
            var sc = t.worldToLocalMatrix.GetScale();
            return Matrix4x4.Scale(sc).MultiplyPoint(v);
        }

        public static Quaternion TranformRotation(this Matrix4x4 m, Quaternion rot)
        {
            return m.GetRotation() * rot;
        }

        public static Quaternion TransformRotation(this Transform t, Quaternion rot)
        {
            return t.rotation * rot;
        }

        public static Quaternion InverseTranformRotation(this Matrix4x4 m, Quaternion rot)
        {
            //return rot * Quaternion.Inverse(m.GetRotation());
            return Quaternion.Inverse(m.GetRotation()) * rot;
        }

        public static Quaternion InverseTransformRotation(this Transform t, Quaternion rot)
        {
            //return rot * Quaternion.Inverse(t.rotation);
            return Quaternion.Inverse(t.rotation) * rot;
        }

        /// <summary>
        /// Transform a ray by a transformation
        /// </summary>
        /// <param name="m"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Ray TransformRay(this Matrix4x4 m, Ray r)
        {
            return new Ray(m.MultiplyPoint(r.origin), m.MultiplyVector(r.direction));
        }

        public static Ray TransformRay(this Transform t, Ray r)
        {
            return new Ray(t.TransformPoint(r.origin), t.TransformDirection(r.direction));
        }

        /// <summary>
        /// Inverse transform a ray by a transformation
        /// </summary>
        /// <param name="m"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Ray InverseTransformRay(this Matrix4x4 m, Ray r)
        {
            m = m.inverse;
            return new Ray(m.MultiplyPoint(r.origin), m.MultiplyVector(r.direction));
        }

        public static Ray InverseTransformRay(this Transform t, Ray r)
        {
            return new Ray(t.InverseTransformPoint(r.origin), t.InverseTransformDirection(r.direction));
        }

        #endregion Transform Methods

        #region Transpose Methods

        /// <summary>
        /// Set the position and rotation of a Transform as if its origin were that of 'anchor'.
        /// Anchor should be in world space.
        /// </summary>
        /// <param name=nameof(transform)>The transform to transpose.</param>
        /// <param name="anchor">The point around which to transpose in world space.</param>
        /// <param name="position">The new position in world space.</param>
        /// <param name="rotation">The new rotation in world space.</param>
        public static void TransposeAroundGlobalAnchor(this Transform transform, Vector3 anchor, Vector3 position, Quaternion rotation)
        {
            anchor = transform.InverseTransformPoint(anchor);
            if (transform.parent != null)
            {
                position = transform.parent.InverseTransformPoint(position);
                rotation = transform.parent.InverseTransformRotation(rotation);
            }

            LocalTransposeAroundAnchor(transform, anchor, position, rotation);
        }

        /// <summary>
        /// Set the position and rotation of a Transform as if its origin were that of 'anchor'.
        /// Anchor should be local to the Transform where <0,0,0> would be the same as its true origin.
        /// </summary>
        /// <param name=nameof(transform)>The transform to transpose.</param>
        /// <param name="anchor">The point around which to transpose in local space.</param>
        /// <param name="position">The new position in world space.</param>
        /// <param name="rotation">The new rotation in world space.</param>
        public static void TransposeAroundAnchor(this Transform transform, Vector3 anchor, Vector3 position, Quaternion rotation)
        {
            if (transform.parent != null)
            {
                position = transform.parent.InverseTransformPoint(position);
                rotation = transform.parent.InverseTransformRotation(rotation);
            }

            LocalTransposeAroundAnchor(transform, anchor, position, rotation);
        }

        /// <summary>
        /// Set the localPosition and localRotation of a Transform as if its origin were that of 'anchor'.
        /// Anchor should be local to the Transform where <0,0,0> would be the same as its true origin.
        /// </summary>
        /// <param name=nameof(transform)>The transform to transpose.</param>
        /// <param name="anchor">The point around which to transpose relative to the transform.</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public static void LocalTransposeAroundAnchor(this Transform transform, Vector3 anchor, Vector3 position, Quaternion rotation)
        {
            anchor = rotation * Vector3.Scale(anchor, transform.localScale);
            transform.localPosition = position - anchor;
            transform.localRotation = rotation;
        }

        public static void LocalTransposeAroundAnchor(this Transform transform, Transform anchor, Vector3 position, Quaternion rotation)
        {
            var m = anchor.GetRelativeMatrix(transform);

            var anchorPos = rotation * Vector3.Scale(m.GetTranslation(), transform.localScale);
            transform.localPosition = position - anchorPos;
            transform.localRotation = m.GetRotation() * rotation;
        }

        #endregion Transpose Methods
    }
}