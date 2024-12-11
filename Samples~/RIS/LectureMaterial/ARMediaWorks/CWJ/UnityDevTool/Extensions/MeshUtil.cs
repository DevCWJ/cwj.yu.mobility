using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

using Random = UnityEngine.Random;

namespace CWJ
{
    /// <summary>
    /// local space인지 world space 기준인지 잘 확인하기
    /// <br/>renderer.bounds와 mesh.bounds는 다르다
    /// <para/>meshFilter든 meshCollider든 mesh 사용할때 특별한 경우 아니면 sharedMesh 이용하기
    /// </summary>
    public static class MeshUtil
    {
        public static Bounds GetNearPlaneBounds(this Camera camera, float zSize = 0.001f)
        {
            Rect rect = new Rect();
            float angle = camera.fieldOfView * 0.5f; //get angle 
            float height = Mathf.Tan(angle * Mathf.Deg2Rad) * camera.nearClipPlane; //calc height
            float width = (height / camera.pixelHeight) * camera.pixelWidth; //deduct width

            rect.xMin = -width;
            rect.xMax = width;
            rect.yMin = -height;
            rect.yMax = height;

            return new Bounds(center: new Vector3(0, 0, camera.nearClipPlane + (zSize * .5f)), size: new Vector3(rect.width, rect.height, zSize));
        }

        public static Bounds TransformBounds(this Transform transform, Bounds bounds)
        {
            Vector3 center = transform.TransformPoint(bounds.center);

            // transform the local extents' axes
            Vector3 extents = bounds.extents;
            var axisX = transform.TransformVector(extents.x, 0, 0);
            var axisY = transform.TransformVector(0, extents.y, 0);
            var axisZ = transform.TransformVector(0, 0, extents.z);

            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            return new Bounds { center = center, extents = extents };
        }

        public static bool ContainBounds(this Bounds bounds, Bounds target)
        {
            return bounds.Contains(target.min) && bounds.Contains(target.max);
        }

        public static (Vector3 bottomPoint, Vector3 topPoint)[] GetColumnLines(this Bounds bounds, Vector3 position, Quaternion rotation)
        {
            Vector3[] edges = bounds.GetEdgeFromBounds(position, rotation);

            (Vector3 bottom, Vector3 top)[] columnLines = new (Vector3, Vector3)[4]
                                                            {
                                                                (edges[0], edges[4]),
                                                                (edges[1], edges[5]),
                                                                (edges[2], edges[6]),
                                                                (edges[3], edges[7]),
                                                            };// column (0-4, 1-5, 2-6, 3-7)


            //#if UNITY_EDITOR && CWJ_EDITOR_DEBUG_ENABLED
            //            Color lineColor = Color.magenta;
            //            // bottom (0-1-2-3)
            //            Debug.DrawLine(edges[0], edges[1], lineColor, 5);
            //            Debug.DrawLine(edges[1], edges[2], lineColor, 5);
            //            Debug.DrawLine(edges[2], edges[3], lineColor, 5);
            //            Debug.DrawLine(edges[3], edges[0], lineColor, 5);

            //            // column (0-4, 1-5, 2-6, 3-7)
            //            Debug.DrawLine(edges[0], edges[4], lineColor, 5);
            //            Debug.DrawLine(edges[1], edges[5], lineColor, 5);
            //            Debug.DrawLine(edges[2], edges[6], lineColor, 5);
            //            Debug.DrawLine(edges[3], edges[7], lineColor, 5);

            //            // top (4-5-6-7)
            //            Debug.DrawLine(edges[4], edges[5], lineColor, 5);
            //            Debug.DrawLine(edges[5], edges[6], lineColor, 5);
            //            Debug.DrawLine(edges[6], edges[7], lineColor, 5);
            //            Debug.DrawLine(edges[7], edges[4], lineColor, 5);
            //#endif
            return columnLines;
        }

        public static Vector3[] GetEdgeFromBounds(this Bounds bounds, Vector3 pivot, Quaternion rotation/*, bool isLocal= true*/)
        {
            Vector3[] edges = bounds.GetEdgeFromBounds();

            for (int i = 0; i < 8; i++)
            {
                edges[i] = VectorUtil.RotatePointAroundPivot(pivot + edges[i], pivot, rotation);
            }

            return edges;
        }

        public static Vector3[] GetEdgeFromBounds(this Bounds bounds, Transform transform)
        {
            Vector3[] edges = bounds.GetEdgeFromBounds();
            for (int i = 0; i < 8; i++)
            {
                edges[i] = transform.TransformPoint(edges[i]);
            }
            return edges;
        }

        /// <summary>
        /// bounds가 가지는 8개의 point를 vector3배열로 반환
        /// <para>반환되는 Vector3는 local 좌표계 위치값</para>
        /// </summary>
        public static Vector3[] GetEdgeFromBounds(this Bounds bounds)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            Vector3[] edges = new Vector3[8];

            //아래쪽 꼭지점 (좌측후방부터 시계방향)
            edges[0] = min;
            edges[1] = new Vector3(min.x, min.y, max.z);
            edges[2] = new Vector3(max.x, min.y, max.z);
            edges[3] = new Vector3(max.x, min.y, min.z);

            //위쪽 꼭지점 (좌측후방부터 시계방향)
            edges[4] = new Vector3(min.x, max.y, min.z);
            edges[5] = new Vector3(min.x, max.y, max.z);
            edges[6] = max;
            edges[7] = new Vector3(max.x, max.y, min.z);
            return edges;
        }

        public static Vector3 GetRandomPosOnNonConvexMesh(this Mesh mesh)
        {
            var meshDataForRandom = new MeshCacheForRandom(mesh);
            return meshDataForRandom.GetRandomLocalPoint();
        }

        public static Vector3 GetRandomPosOnNonConvexMesh(this Mesh mesh, ref List<MeshCacheForRandom> meshDataForRandoms)
        {
            //int instanceID = mesh.GetInstanceID();
            //int findIndex = meshDataForRandoms.FindIndex((m) => m.instanceID == instanceID);
            int findIndex = meshDataForRandoms.FindIndex((m) => m.mesh == mesh);

            if (findIndex < 0)
            {
                meshDataForRandoms.Add(new MeshCacheForRandom(mesh));
                findIndex = meshDataForRandoms.Count - 1;
            }

            return meshDataForRandoms[findIndex].GetRandomLocalPoint();
        }
#if UNITY_EDITOR
        private static HashSet<string> duplicatePath { get; set; } = new HashSet<string>();

        public static void SetReadableMesh(this Mesh[] sharedMesh, bool isReadable = true)
        {
            for (int i = 0; i < sharedMesh.Length; i++)
            {
                _SetReadableMesh(sharedMesh[i], isReadable);
            }
            AssetDatabase.Refresh();
            for (int i = 0; i < sharedMesh.Length; i++)
            {
                Debug.LogError(sharedMesh[i].isReadable);
            }
        }

        public static void SetReadableMesh(this Mesh sharedMesh, bool isReadable = true)
        {
            _SetReadableMesh(sharedMesh, isReadable);
            AssetDatabase.Refresh();
        }

        // TODO : 테스트필요 특정 프로젝트에선 잘됐었는데 갑자기 무한루프걸림
        // https://issuetracker.unity3d.com/issues/adb-v2-importer-stuck-in-infinite-loop-on-importing-ogg-files-from-the-asset-store?_ga=2.247105796.1269132251.1597729041-1156714004.1591600585
        // 이 이슈일지도 모르니 여러 플젝에서 테스트필요
        private static void _SetReadableMesh(this Mesh sharedMesh, bool isReadable)
        {
            if (sharedMesh == null || sharedMesh.isReadable == isReadable) return;

            string path = UnityEditor.AssetDatabase.GetAssetPath(sharedMesh);

            if (string.IsNullOrEmpty(path) || !duplicatePath.Add(path)) return;

            var assetImporter = UnityEditor.AssetImporter.GetAtPath(path);

            if (assetImporter is UnityEditor.ModelImporter)
            {
                UnityEditor.AssetDatabase.WriteImportSettingsIfDirty(path);

                var modelImporter = (UnityEditor.ModelImporter)assetImporter;
                try
                {
                    AssetDatabase.StartAssetEditing();
                    modelImporter.isReadable = true;
                    UnityEditor.AssetDatabase.ImportAsset(modelImporter.assetPath, ImportAssetOptions.ImportRecursive); // 이거안되면 그냥 직접 수정하고 apply눌러줘야함
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                }
                // ^1차 시도
            }
        }
#endif
        

        [Serializable]
        public struct MeshCacheForRandom
        {
            //public int instanceID;
            public Mesh mesh;
            [SerializeField] private Vector3[] vertices;
            [SerializeField] private int[] triangles;
            [SerializeField] private float[] sizes;
            [SerializeField] private float[] cumulativeSizes;
            [SerializeField] private float total;

            /// <summary>
            /// 메쉬 변형하는 특수한 경우아니면 sharedMesh를 사용할것
            /// </summary>
            /// <param name="sharedMesh"></param>
            public MeshCacheForRandom(Mesh sharedMesh)
            {
                //instanceID = mesh.GetInstanceID();
                mesh = sharedMesh;
                triangles = sharedMesh.triangles;
                vertices = sharedMesh.vertices;
                int triCount = triangles.Length / 3;
                sizes = new float[triCount];
                for (int i = 0; i < triCount; i++)
                {
                    sizes[i] = .5f * Vector3.Cross(vertices[triangles[i * 3 + 1]] - vertices[triangles[i * 3]], vertices[triangles[i * 3 + 2]] - vertices[triangles[i * 3]]).magnitude;
                }

                this.cumulativeSizes = new float[sizes.Length];
                this.total = 0;

                for (int i = 0; i < sizes.Length; i++)
                {
                    total += sizes[i];
                    cumulativeSizes[i] = total;
                }
            }

            public Vector3 GetRandomLocalPoint()
            {
                float randomSample = UnityEngine.Random.value * total;

                int triIndex = -1;

                for (int i = 0; i < sizes.Length; i++)
                {
                    if (randomSample <= cumulativeSizes[i])
                    {
                        triIndex = i;
                        break;
                    }
                }

                if (triIndex == -1)
                {
                    throw new System.IndexOutOfRangeException("triIndex should never be -1");
                }

                triIndex *= 3;

                Vector3 a = vertices[triangles[triIndex]];
                Vector3 b = vertices[triangles[triIndex + 1]];
                Vector3 c = vertices[triangles[triIndex + 2]];

                float r = Random.value;
                float s = Random.value;

                if (r + s >= 1)
                {
                    r = 1 - r;
                    s = 1 - s;
                }

                return a + r * (b - a) + s * (c - a);
            }
        }

        public static Vector3 GetCenterPoint(this Mesh mesh)
        {
            Vector3 center = Vector3.zero;

            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                center += vertices[i];
            }

            return center / mesh.vertexCount;
        }

        public static Vector3 GetRandomPosOnConvexMesh(this Mesh mesh)
        {
            Vector3 surfacePosA = GetRandomPosOnSurface(mesh);
            Vector3 surfacePosB = GetRandomPosOnSurface(mesh);

            return Vector3.Lerp(surfacePosA, surfacePosB, Random.Range(0f, 1f));
        }

        /// <summary>
        /// 눈속임. 가상 중심위치를 기준으로 위치반환
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="debugTransform"></param>
        /// <returns></returns>
        //public static Vector3 GetRandomPosInNonConvexFake(this Mesh mesh, Vector3 virtualCenterPos)
        //{
        //    Vector3 randomPointOnSurface = mesh.GetRandomPosOnSurface();

        //    return Vector3.Lerp(virtualCenterPos, randomPointOnSurface, Random.Range(0f, 1f));
        //}


        private static Vector3 GetRandomPosOnSurface(this Mesh mesh, Transform debugTransform = null)
        {
            int triangleOrigin = Mathf.FloorToInt(UnityEngine.Random.Range(0f, mesh.triangles.Length) / 3f) * 3;

            Vector3 vertexA = mesh.vertices[mesh.triangles[triangleOrigin]];
            Vector3 vertexB = mesh.vertices[mesh.triangles[triangleOrigin + 1]];
            Vector3 vertexC = mesh.vertices[mesh.triangles[triangleOrigin + 2]];

            Vector3 dAB = vertexB - vertexA;
            Vector3 dBC = vertexC - vertexB;

            float rAB = UnityEngine.Random.Range(0f, 1f);
            float rBC = UnityEngine.Random.Range(0f, 1f);

            Vector3 randPoint = vertexA + rAB * dAB + rBC * dBC;

            Vector3 dirPC = (vertexC - randPoint).normalized;

            Vector3 dirAB = (vertexB - vertexA).normalized;
            Vector3 dirAC = (vertexC - vertexA).normalized;

            Vector3 triangleNormal = Vector3.Cross(dirAC, dirAB).normalized;

            Vector3 dirH_AC = Vector3.Cross(triangleNormal, dirAC).normalized;

            float dot = Vector3.Dot(dirPC, dirH_AC);

            if (dot >= 0)
            {
                Vector3 centralPoint = (vertexA + vertexC) / 2;
                Vector3 symmetricRandPoint = 2 * centralPoint - randPoint;
#if CWJ_EDITOR_DEBUG_ENABLED
                if (debugTransform)
                    Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(symmetricRandPoint), Color.red, 10);
#endif
                randPoint = symmetricRandPoint;
            }
#if CWJ_EDITOR_DEBUG_ENABLED
            if (debugTransform)
            {
                Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexA), Color.cyan, 10);
                Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexB), Color.green, 10);
                Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexC), Color.blue, 10);
            }
#endif

            return randPoint;
        }

        /// <summary>
        /// <para>외곽 vertices 를 구할때 쓰려고 만듬</para>
        /// 평면 mesh일때는 원하는대로 잘구해지는듯 꼭 처음에 캐싱해놓고 쓰기
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static Vector3[] GetOutlineVertices(this Mesh mesh)
        {
            int[] triangles = mesh.triangles;
            List<(int v1, int v2)> edges = new List<(int v1, int v2)>();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int v1 = triangles[i];
                int v2 = triangles[i + 1];
                int v3 = triangles[i + 2];
                edges.Add((v1, v2));
                edges.Add((v2, v3));
                edges.Add((v3, v1));
            }

            for (int i = edges.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (edges[i].v1 == edges[j].v2 && edges[i].v2 == edges[j].v1)
                    {
                        edges.RemoveAt(i);
                        edges.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            }

            int edgeCnt = edges.Count;

            //정렬
            for (int i = 0; i < edgeCnt - 2; i++)
            {
                var edge = edges[i];
                for (int j = i + 1; j < edgeCnt; j++)
                {
                    var nextEdge = edges[j];
                    if (edge.v2 == nextEdge.v1)
                    {
                        if (j == i + 1)
                            break;
                        edges[j] = edges[i + 1];
                        edges[i + 1] = nextEdge;
                        break;
                    }
                }
            }

            Vector3[] verts = mesh.vertices;
            Vector3[] outlineVerts = new Vector3[edgeCnt];
            for (int i = 0; i < edgeCnt; i++)
            {
                outlineVerts[i] = verts[edges[i].v1];
            }

            return outlineVerts;
        }

        public static Mesh CreateCubeMesh(Vector3 size, string name)
        {
            return _CreateCubeMesh(size, center: Vector3.zero, name: name + " [CWJ_CubeMesh]");
        }

        public static Mesh CreateCubeMesh(Vector3 size, Vector3 center, string name)
        {
            return _CreateCubeMesh(size, center: center, name: name + " [CWJ_CubeMesh]");
        }

        private static Mesh _CreateCubeMesh(Vector3 size, Vector3 center, string name)
        {
            float x = size.x;
            float y = size.y;
            float z = size.z;

            Vector3 p0 = new Vector3(-x * .5f, -y * .5f, z * .5f);
            Vector3 p1 = new Vector3(x * .5f, -y * .5f, z * .5f);
            Vector3 p2 = new Vector3(x * .5f, -y * .5f, -z * .5f);
            Vector3 p3 = new Vector3(-x * .5f, -y * .5f, -z * .5f);
            Vector3 p4 = new Vector3(-x * .5f, y * .5f, z * .5f);
            Vector3 p5 = new Vector3(x * .5f, y * .5f, z * .5f);
            Vector3 p6 = new Vector3(x * .5f, y * .5f, -z * .5f);
            Vector3 p7 = new Vector3(-x * .5f, y * .5f, -z * .5f);

            //큐브가 구성된 16개의 vertices를 정의 (각면의 꼭짓점이 별도의 법선을 갖기를 원하기 때문)
            Vector3[] vertices = new Vector3[]
            {
            p0, p1, p2, p3, // Bottom
	        p7, p4, p0, p3, // Left
	        p4, p5, p1, p0, // Front
	        p6, p7, p3, p2, // Back
	        p5, p6, p2, p1, // Right
	        p7, p6, p5, p4  // Top
            };


            //5) 각 vertex의 normal을 정의
            Vector3 up = Vector3.up;
            Vector3 down = Vector3.down;
            Vector3 forward = Vector3.forward;
            Vector3 back = Vector3.back;
            Vector3 left = Vector3.left;
            Vector3 right = Vector3.right;


            Vector3[] normals = new Vector3[]
            {
            down, down, down, down,             // Bottom
	        left, left, left, left,             // Left
	        forward, forward, forward, forward,	// Front
	        back, back, back, back,             // Back
	        right, right, right, right,         // Right
	        up, up, up, up                      // Top
            };


            Vector2 uv00 = new Vector2(0f, 0f);
            Vector2 uv10 = new Vector2(1f, 0f);
            Vector2 uv01 = new Vector2(0f, 1f);
            Vector2 uv11 = new Vector2(1f, 1f);

            Vector2[] uvs = new Vector2[]
            {
            uv11, uv01, uv00, uv10, // Bottom
	        uv11, uv01, uv00, uv10, // Left
	        uv11, uv01, uv00, uv10, // Front
	        uv11, uv01, uv00, uv10, // Back	        
	        uv11, uv01, uv00, uv10, // Right 
	        uv11, uv01, uv00, uv10  // Top
            };


            //메쉬를 구성하는 삼각형을 정의
            //Unity는 폴리곤을 Clockwise winding(카메라 기준)으로 결정함.
            int[] triangles = new int[]
            {
            3, 1, 0,        3, 2, 1,        // Bottom	
	        7, 5, 4,        7, 6, 5,        // Left
	        11, 9, 8,       11, 10, 9,      // Front
	        15, 13, 12,     15, 14, 13,     // Back
	        19, 17, 16,     19, 18, 17,	    // Right
	        23, 21, 20,     23, 22, 21,     // Top
            };

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.name = name;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uvs;
            if (!center.Equals(Vector3.zero))
            {
                mesh.bounds = new Bounds(center: center, size: mesh.bounds.size);
            }
            //mesh.RecalculateNormals();
            mesh.Optimize();
            return mesh;
        }

        public static Mesh CreateSimpleCubeMesh()
        {
            Vector3 p0 = new Vector3(-.5f, -.5f, .5f);
            Vector3 p1 = new Vector3(.5f, -.5f, .5f);
            Vector3 p2 = new Vector3(.5f, -.5f, -.5f);
            Vector3 p3 = new Vector3(-.5f, -.5f, -.5f);
            Vector3 p4 = new Vector3(-.5f, .5f, .5f);
            Vector3 p5 = new Vector3(.5f, .5f, .5f);
            Vector3 p6 = new Vector3(.5f, .5f, -.5f);
            Vector3 p7 = new Vector3(-.5f, .5f, -.5f);

            Vector3[] vertices = new Vector3[]
            {
            p0, p1, p2, p3,
            p7, p4, p0, p3,
            p4, p5, p1, p0,
            p6, p7, p3, p2,
            p5, p6, p2, p1,
            p7, p6, p5, p4
            };

            int[] triangles = new int[]
            {
            3, 1, 0,
            3, 2, 1,
            3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
            3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
            3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
            3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
            3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
            };

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.name = "CWJ_SimpleCubeMesh";
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.Optimize();
            return mesh;
        }

        public static Mesh CreatePlaneMesh(float width, float height, bool hasUv = false)
        {
            Mesh m = new Mesh();
            m.name = "CWJ_PlaneMesh";
            m.vertices = new Vector3[] {
                         new Vector3(-width, -height, 0.01f),
                         new Vector3(width, -height, 0.01f),
                         new Vector3(width, height, 0.01f),
                         new Vector3(-width, height, 0.01f)
                        };
            m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            if (hasUv)
            {
                m.uv = new Vector2[] {
                        new Vector2 (0, 0),
                        new Vector2 (0, 1),
                        new Vector2 (1, 1),
                        new Vector2 (1, 0)
                        };
            }
            m.RecalculateBounds();
            m.RecalculateNormals();

            return m;
        }

        public static Mesh CanvasToPlaneMesh(this Canvas canvas)
        {
            var rectTrf = canvas.GetComponent<RectTransform>();
            float width = rectTrf.rect.width * canvas.transform.lossyScale.x * .5f;
            float height = rectTrf.rect.height * canvas.transform.lossyScale.y * .5f;

            return CreatePlaneMesh(width, height);
        }

        public static void DebugDrawPlane(Vector3 normal, Vector3 position, float radius)
        {
            Vector3 v3;

            if (normal.normalized != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude * radius;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude * radius;

            Vector3 corner0 = position + v3;
            Vector3 corner2 = position - v3;

            Quaternion rot = Quaternion.AngleAxis(90.0f, normal);
            v3 = rot * v3;
            Vector3 corner1 = position + v3;
            Vector3 corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, Color.green);
            Debug.DrawLine(corner1, corner3, Color.green);
            Debug.DrawLine(corner0, corner1, Color.green);
            Debug.DrawLine(corner1, corner2, Color.green);
            Debug.DrawLine(corner2, corner3, Color.green);
            Debug.DrawLine(corner3, corner0, Color.green);
            Debug.DrawRay(position, normal, Color.blue);
        }



        public static Mesh MergeVertices(this Mesh mesh, float threshold)
        {
            Vector3[] verts = mesh.vertices;
            List<Vector3> newVerts = new List<Vector3>();
            for (int i = 0; i < verts.Length; ++i)
            {
                foreach (Vector3 newVert in newVerts)
                {
                    if (Vector3.Distance(newVert, verts[i]) <= threshold)
                        goto skipToNext;
                }
                newVerts.Add(verts[i]);
            skipToNext:;
            }
            int[] tris = mesh.triangles;
            for (int i = 0; i < tris.Length; ++i)
            {
                for (int j = 0; j < newVerts.Count; ++j)
                {
                    if (Vector3.Distance(newVerts[j], verts[tris[i]]) <= threshold)
                    {
                        tris[i] = j;
                        break;
                    }
                }
            }
            mesh.Clear();
            mesh.vertices = newVerts.ToArray();
            mesh.triangles = tris;
            mesh.uv = null;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            return mesh;
        }

        #region Mesh Random Position

        public static Vector3 GenerateRandomPoint(Mesh mesh, Transform transform)
        {
            // 1 - Calculate Surface Areas
            float[] triangleSurfaceAreas = CalculateSurfaceAreas(mesh);

            // 2 - Normalize area weights
            float[] normalizedAreaWeights = NormalizeAreaWeights(triangleSurfaceAreas);

            // 3 - Generate 'triangle selection' random #
            float triangleSelectionValue = Random.value;

            // 4 - Walk through the list of weights to select the proper triangle
            int triangleIndex = SelectRandomTriangle(normalizedAreaWeights, triangleSelectionValue);

            // 5 - Generate a random barycentric coordinate
            Vector3 randomBarycentricCoordinates = GenerateRandomBarycentricCoordinates();

            // 6 - Using the selected barycentric coordinate and the selected mesh triangle, convert this point to world space.
            return transform.TransformPoint(ConvertToLocalSpace(randomBarycentricCoordinates, triangleIndex, mesh));
        }

        public static float[] CalculateSurfaceAreas(Mesh mesh)
        {
            int triangleCount = mesh.triangles.Length / 3;

            float[] surfaceAreas = new float[triangleCount];


            for (int triangleIndex = 0; triangleIndex < triangleCount; triangleIndex++)
            {
                Vector3[] points = new Vector3[3];
                points[0] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 0]];
                points[1] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 1]];
                points[2] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 2]];

                // calculate the three sidelengths and use those to determine the area of the triangle
                // http://www.wikihow.com/Sample/Area-of-a-Triangle-Side-Length
                float a = (points[0] - points[1]).magnitude;
                float b = (points[0] - points[2]).magnitude;
                float c = (points[1] - points[2]).magnitude;

                float s = (a + b + c) / 2;

                surfaceAreas[triangleIndex] = Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));
            }

            return surfaceAreas;
        }

        public static float[] NormalizeAreaWeights(float[] surfaceAreas)
        {
            float[] normalizedAreaWeights = new float[surfaceAreas.Length];

            float totalSurfaceArea = 0;
            foreach (float surfaceArea in surfaceAreas)
            {
                totalSurfaceArea += surfaceArea;
            }

            for (int i = 0; i < normalizedAreaWeights.Length; i++)
            {
                normalizedAreaWeights[i] = surfaceAreas[i] / totalSurfaceArea;
            }

            return normalizedAreaWeights;
        }

        public static int SelectRandomTriangle(float[] normalizedAreaWeights, float triangleSelectionValue)
        {
            float accumulated = 0;

            for (int i = 0; i < normalizedAreaWeights.Length; i++)
            {
                accumulated += normalizedAreaWeights[i];

                if (accumulated >= triangleSelectionValue)
                {
                    return i;
                }
            }

            // unless we were handed malformed normalizedAreaWeights, we should have returned from this already.
            throw new System.ArgumentException("Normalized Area Weights were not normalized properly, or triangle selection value was not [0, 1]");
        }

        public static Vector3 GenerateRandomBarycentricCoordinates()
        {
            Vector3 barycentric = new Vector3(Random.value, Random.value, Random.value);

            while (barycentric == Vector3.zero)
            {
                // seems unlikely, but just in case...
                barycentric = new Vector3(Random.value, Random.value, Random.value);
            }

            // normalize the barycentric coordinates. These are normalized such that x + y + z = 1, as opposed to
            // normal vectors which are normalized such that Sqrt(x^2 + y^2 + z^2) = 1. See:
            // http://en.wikipedia.org/wiki/Barycentric_coordinate_system
            float sum = barycentric.x + barycentric.y + barycentric.z;

            return barycentric / sum;
        }

        public static Vector3 ConvertToLocalSpace(Vector3 barycentric, int triangleIndex, Mesh mesh)
        {
            Vector3[] points = new Vector3[3];
            points[0] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 0]];
            points[1] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 1]];
            points[2] = mesh.vertices[mesh.triangles[triangleIndex * 3 + 2]];

            return (points[0] * barycentric.x + points[1] * barycentric.y + points[2] * barycentric.z);
        }

        #endregion

        #region Bounds GetScreenRect
        public static Bounds GetWorldSpaceBounds(params Renderer[] renderers)
        {
            if (renderers == null || renderers.Length == 0) throw new NullReferenceException();

            Bounds bounds = renderers[0].bounds;
            if (renderers.Length > 1)
            {
                renderers.ForEach(rend => bounds.Encapsulate(rend.bounds), startIndex: 1);
            }

            return bounds;
        }

        /// <summary>
        /// worldspace bounds가 필요할땐 MeshFilter.bounds 보단 Renderer.bounds 사용할것을 권장
        /// </summary>
        /// <param name="meshFilters"></param>
        /// <returns></returns>
        public static Bounds GetWorldSpaceBounds(params MeshFilter[] meshFilters)
        {
            meshFilters = meshFilters.FindAll(m => m.sharedMesh != null);

            if (meshFilters == null || meshFilters.Length == 0) throw new NullReferenceException();

            Func<MeshFilter, Bounds> getBounds = mf =>
            {
                Bounds newBounds = mf.sharedMesh.bounds;
                newBounds.center = mf.transform.TransformPoint(newBounds.center);
                newBounds.extents = Vector3.Scale(newBounds.extents, mf.transform.lossyScale);
                return newBounds;
            };

            Bounds bounds = getBounds(meshFilters[0]);
            if (meshFilters.Length > 1)
            {
                meshFilters.ForEach(meshFilter => bounds.Encapsulate(getBounds(meshFilter)), startIndex: 1);
            }

            return bounds;
        }

        public static Rect GetSizeOfBoundsThroughCamera(Camera camera = null, params Renderer[] renderers)
        {
            if (renderers == null || renderers.Length == 0)
            {
                return Rect.zero;
            }

            if (camera == null) camera = Camera.main;

            return GetSizeOfBoundsThroughCamera(GetWorldSpaceBounds(renderers), camera);
        }

        public static Rect GetSizeOfBoundsThroughCamera(Camera camera = null, params MeshFilter[] meshFilters)
        {
            if (meshFilters == null || meshFilters.Length == 0)
            {
                return Rect.zero;
            }

            if (camera == null) camera = Camera.main;

            return GetSizeOfBoundsThroughCamera(GetWorldSpaceBounds(meshFilters), camera);
        }

        /// <summary>
        /// bounds : worldspace bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Rect GetSizeOfBoundsThroughCamera(Bounds bounds, Camera camera)
        {
            Vector3 ext = bounds.extents;
            var screenPositions = new List<Vector2>();
            for (int x = -1; x < 2; x += 2)
            {
                for (int y = -1; y < 2; y += 2)
                {
                    for (int z = -1; z < 2; z += 2)
                    {
                        Vector3 pos = bounds.center + new Vector3(ext.x * x, ext.y * y, ext.z * z);
                        Vector3 screenPos = camera.WorldToScreenPoint(pos);
                        screenPos.y = Screen.height - screenPos.y;
                        screenPositions.Add(screenPos);
                    }
                }
            }
            var rect = new Rect(screenPositions[0].x, screenPositions[0].y, 1, 1);
            Action<Vector2> FindMinMax = vec =>
            {
                if (vec.x < rect.xMin) rect.xMin = vec.x;
                if (vec.x > rect.xMax) rect.xMax = vec.x;
                if (vec.y < rect.yMin) rect.yMin = vec.y;
                if (vec.y > rect.yMax) rect.yMax = vec.y;
            };
            screenPositions.ForEach(FindMinMax);
            return rect;
        }
        #endregion
    }
}