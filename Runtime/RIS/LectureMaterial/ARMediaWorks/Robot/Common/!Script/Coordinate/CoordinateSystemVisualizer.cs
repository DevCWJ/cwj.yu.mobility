
//namespace CWJ.YU.Mobility
//{
//    using UnityEngine;
//    using TMPro;
//    using System.Collections.Generic;

//    public class CoordinateSystemVisualizer : MonoBehaviour
//    {
//        public Vector3 blueCoords = new Vector3(3, 2, 0);
//        public Vector3 redCoords = new Vector3(2, 8, 4);

//        public LineRenderer blueLineRenderer;
//        public LineRenderer redLineRenderer;

//        public TMP_Text blueCoordsLabel;
//        public TMP_Text redCoordsLabel;
//        public TMP_Text blueXLabel;
//        public TMP_Text blueYLabel;
//        public TMP_Text blueZLabel;
//        public TMP_Text redXLabel;
//        public TMP_Text redYLabel;
//        public TMP_Text redZLabel;

//        [InvokeButton]
//        void Start()
//        {
//            // Set up LineRenderers
//            DrawCoordinateSystem(blueLineRenderer, Vector3.zero, blueCoords, Color.blue);
//            SetCoordinateLabels(blueCoordsLabel, blueCoords, Vector3.zero);
//            SetAxisLabels(blueCoords, blueXLabel, blueYLabel, blueZLabel);

//            DrawCoordinateSystem(redLineRenderer, Vector3.zero, redCoords, Color.red);
//            SetCoordinateLabels(redCoordsLabel, redCoords, Vector3.zero);
//            SetAxisLabels(redCoords, redXLabel, redYLabel, redZLabel);
//        }

//        void DrawCoordinateSystem(LineRenderer lineRenderer, Vector3 origin, Vector3 coords, Color color)
//        {
//            lineRenderer.startColor = color;
//            lineRenderer.endColor = color;

//            List<Vector3> positions = new List<Vector3>();

//            // Draw X axis with arrow
//            positions.Add(origin);
//            positions.Add(origin + Vector3.right * coords.x);
//            DrawArrow(positions, origin + Vector3.right * coords.x, Vector3.right);

//            // Draw Y axis with arrow
//            positions.Add(origin);
//            positions.Add(origin + Vector3.up * coords.y);
//            DrawArrow(positions, origin + Vector3.up * coords.y, Vector3.up);

//            // Draw Z axis with arrow
//            positions.Add(origin);
//            positions.Add(origin + Vector3.forward * coords.z);
//            DrawArrow(positions, origin + Vector3.forward * coords.z, Vector3.forward);

//            lineRenderer.positionCount = positions.Count;
//            lineRenderer.SetPositions(positions.ToArray());
//        }

//        void DrawArrow(List<Vector3> positions, Vector3 arrowHead, Vector3 direction)
//        {
//            Vector3 arrowLeft = arrowHead + Quaternion.Euler(0, 45, 0) * -direction * 0.5f;
//            Vector3 arrowRight = arrowHead + Quaternion.Euler(0, -45, 0) * -direction * 0.5f;

//            positions.Add(arrowLeft);
//            positions.Add(arrowHead);
//            positions.Add(arrowRight);
//        }

//        void SetCoordinateLabels(TMP_Text label, Vector3 coords, Vector3 offset)
//        {
//            label.text = $"({coords.x + offset.x}, {coords.y + offset.y}, {coords.z + offset.z})";
//            label.transform.position = Camera.main.WorldToScreenPoint(coords + offset + Vector3.up * 0.5f);
//        }

//        void SetAxisLabels(Vector3 origin, TMP_Text xLabel, TMP_Text yLabel, TMP_Text zLabel)
//        {
//            xLabel.text = "X";
//            xLabel.transform.position = Camera.main.WorldToScreenPoint(origin + Vector3.right * 2);

//            yLabel.text = "Y";
//            yLabel.transform.position = Camera.main.WorldToScreenPoint(origin + Vector3.up * 2);

//            zLabel.text = "Z";
//            zLabel.transform.position = Camera.main.WorldToScreenPoint(origin + Vector3.forward * 2);
//        }

//        // Update blueCoords and redCoords with input fields
//        public void SetBlueCoords(float x, float y, float z)
//        {
//            blueCoords = new Vector3(x, y, z);
//            Start();
//        }

//        public void SetRedCoords(float x, float y, float z)
//        {
//            redCoords = new Vector3(x, y, z);
//            Start();
//        }
//    }



//}
