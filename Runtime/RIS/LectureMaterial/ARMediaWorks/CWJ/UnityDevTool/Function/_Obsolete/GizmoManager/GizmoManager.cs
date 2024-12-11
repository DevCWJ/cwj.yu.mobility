//Addon - InGameGizmos 패키지사용할것

//using System.Collections.Generic;

//using UnityEngine;

//namespace CWJ
//{
//    /// <summary>
//    /// camera에 넣어야함
//    /// </summary>
//    [RequireComponent(typeof(Camera))]
//    public class GizmoManager : MonoBehaviour
//    {
//        public struct GizmoLine
//        {
//            public Vector3 a;
//            public Vector3 b;
//            public Color color;

//            public GizmoLine(Vector3 a, Vector3 b, Color color)
//            {
//                this.a = a;
//                this.b = b;
//                this.color = color;
//            }
//        }
//        public static bool IsEnabled { get; private set; }

//        public Material material;
//        internal static List<GizmoLine> lines = new List<GizmoLine>();

//        private void Reset()
//        {
//            material = new Material(Shader.Find("Sprites/Default"));
//        }

//        private void OnValidate()
//        {
//            IsEnabled = this.enabled;
//        }

//        private void OnEnable()
//        {
//            IsEnabled = true;
//            Debug.LogError("OnEnable");
//        }

//        private void OnDisable()
//        {
//            IsEnabled = false;
//            Debug.LogError("OnDisable");
//        }

//        private void OnPostRender()
//        {
//            material.SetPass(0);
//            GL.PushMatrix();

//            GL.MultMatrix(gameObject.transform.transform.localToWorldMatrix);
//            GL.Begin(GL.LINES);

//            for (int i = 0; i < lines.Count; i++)
//            {
//                GL.Color(lines[i].color);
//                GL.Vertex(lines[i].a);
//                GL.Vertex(lines[i].b);
//            }

//            GL.End();
//            GL.PopMatrix();
//            lines.Clear();
//        }

//        //private void OnDrawGizmos()
//        //{
//        //    for (int i = 0; i < lines.Count; i++)
//        //    {
//        //        UnityEngine.Gizmos.color = lines[i].color;
//        //        UnityEngine.Gizmos.DrawLine(lines[i].a, lines[i].b);
//        //    }
//        //}
//    }
//}