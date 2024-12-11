//Addon - InGameGizmos 패키지사용할것

//using UnityEngine;

//namespace CWJ
//{
//    public class GizmosExtension
//    {
//        public static void DrawLine(Vector3 a, Vector3 b, Color color)
//        {
//            if (!GizmoManager.IsEnabled) return;

//            GizmoManager.lines.Add(new GizmoManager.GizmoLine(a, b, color));
//        }

//        public static void DrawBox(Vector3 position, Vector3 size, Color color)
//        {
//            if (!GizmoManager.IsEnabled) return;

//            Vector3 point1 = new Vector3(position.x - size.x / 2f, position.y - size.y / 2f, position.z - size.z / 2f);
//            Vector3 point2 = new Vector3(position.x + size.x / 2f, position.y - size.y / 2f, position.z - size.z / 2f);
//            Vector3 point3 = new Vector3(position.x + size.x / 2f, position.y + size.y / 2f, position.z - size.z / 2f);
//            Vector3 point4 = new Vector3(position.x - size.x / 2f, position.y + size.y / 2f, position.z - size.z / 2f);

//            Vector3 point5 = new Vector3(position.x - size.x / 2f, position.y - size.y / 2f, position.z + size.z / 2f);
//            Vector3 point6 = new Vector3(position.x + size.x / 2f, position.y - size.y / 2f, position.z + size.z / 2f);
//            Vector3 point7 = new Vector3(position.x + size.x / 2f, position.y + size.y / 2f, position.z + size.z / 2f);
//            Vector3 point8 = new Vector3(position.x - size.x / 2f, position.y + size.y / 2f, position.z + size.z / 2f);

//            DrawLine(point1, point2, color);
//            DrawLine(point2, point3, color);
//            DrawLine(point3, point4, color);
//            DrawLine(point4, point1, color);

//            DrawLine(point5, point6, color);
//            DrawLine(point6, point7, color);
//            DrawLine(point7, point8, color);
//            DrawLine(point8, point5, color);

//            DrawLine(point1, point5, color);
//            DrawLine(point2, point6, color);
//            DrawLine(point3, point7, color);
//            DrawLine(point4, point8, color);
//        }

//        public static void DrawSquare(Vector3 position, Vector3 size, Color color)
//        {
//            if (!GizmoManager.IsEnabled) return;

//            Vector3 point1 = new Vector3(position.x - size.x / 2f, position.y - size.y / 2f, position.z);
//            Vector3 point2 = new Vector3(position.x + size.x / 2f, position.y - size.y / 2f, position.z);
//            Vector3 point3 = new Vector3(position.x + size.x / 2f, position.y + size.y / 2f, position.z);
//            Vector3 point4 = new Vector3(position.x - size.x / 2f, position.y + size.y / 2f, position.z);

//            DrawLine(point1, point2, color);
//            DrawLine(point2, point3, color);
//            DrawLine(point3, point4, color);
//            DrawLine(point4, point1, color);
//        }

//        public static void DrawCircle(Vector3 position, float radius, Color color, float zOffset = 0)
//        {
//            if (!GizmoManager.IsEnabled) return;

//            DrawPolygon(position, radius, 18, color, zOffset);
//        }

//        public static void DrawPolygon(Vector3 position, float radius, int points, Color color, float zOffset)
//        {
//            if (!GizmoManager.IsEnabled) return;

//            float angle = 360f / points;

//            for (int i = 0; i < points; ++i)
//            {
//                float sx = Mathf.Cos(Mathf.Deg2Rad * angle * i) * radius / 2;
//                float sy = Mathf.Sin(Mathf.Deg2Rad * angle * i) * radius / 2;

//                float nx = Mathf.Cos(Mathf.Deg2Rad * angle * (i + 1)) * radius / 2;
//                float ny = Mathf.Sin(Mathf.Deg2Rad * angle * (i + 1)) * radius / 2;

//                Vector3 a = new Vector3(sx, sy, zOffset);
//                Vector3 b = new Vector3(nx, ny, zOffset);

//                DrawLine(position + a, position + b, color);
//            }
//        }
//    }
//}