using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GesturesControls : MonoBehaviour
{
	public new Camera camera;
	private LineRenderer lineRenderer;
	private List<Vector3> lines = new List<Vector3>();
	private int lineCount = 0;
	private bool isDrawing = false;

	public int nbPoints = 10;
	public float distanceAccuracy = .8f;
	public float angleAccuracy = 30.0f;
	private bool hasReference = false;
	private List<Vector2> reference = new List<Vector2>();
	private List<Vector2> sample = new List<Vector2>();

	public static GesturesControls instance = null;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}


	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.startWidth = 0.1f;
		lineRenderer.endWidth = 0.1f;
	}


	// TODO touch gestures
	void Update()
	{
		if (!isDrawing && !Input.GetMouseButtonDown(0))
			return;

		// Keep left clic pressed to draw
		else if (Input.GetMouseButtonDown(0))
		{
			Clear();

			lineRenderer.startColor = Color.white;
			lineRenderer.endColor = Color.white;
			isDrawing = true;
		}

		// Release left clic to end
		else if (Input.GetMouseButtonUp(0))
		{
			lineRenderer.startColor=Color.yellow;
			lineRenderer.endColor = Color.yellow;
			isDrawing = false;

			// First is reference, second is sample to compare
			if (hasReference)
			{
				if (Compare(reference, Reduce(sample)))
				{
					lineRenderer.startColor=Color.green;
					lineRenderer.endColor = Color.green;
				}
				else
				{
					lineRenderer.startColor=Color.red;
					lineRenderer.endColor = Color.red;
				}
				reference.Clear();
				hasReference = false;
			}
			else
			{
				reference = Reduce(sample);
				hasReference = true;
			}
		}

		// Draw
		else Draw(Input.mousePosition);
	}


	// Only keep some points
	List<Vector2> Reduce(List<Vector2> input)
	{
		// TODO fix nbPoints sometimes fails
		List<Vector2> output = new List<Vector2>();
		int step = Mathf.FloorToInt(input.Count / (nbPoints + 1));
		float scale = 0;
		for (int i = 0; i < input.Count && output.Count < nbPoints; i += step)
		{
			Vector2 position = input[i] - input[0];
			scale = Mathf.Max(position.magnitude, scale);
			output.Add(position);
		}
		for (int i = 0; i < output.Count; i++)
			output[i] = Vector2.ClampMagnitude(output[i], output[i].magnitude / scale);
		return output;
	}


	// Compare two lines
	bool Compare(List<Vector2> lineA, List<Vector2> lineB)
	{
		// TODO remove distances (not accurate at all), and adjust angles comparisons
		//		float distances = 0;
		float angles = 0;
		float rotate = Vector2.Angle(lineA[lineA.Count - 1], lineB[lineB.Count - 1]);
		if (Mathf.Abs(rotate) > 60)
			return false;
		for (int i = 1; i < nbPoints; i++)
		{
			//			distances += Vector2.Distance (lineA[i], lineB[i]) / lineA[i].magnitude;
			angles += Mathf.Abs(Vector2.Angle(lineA[i], lineB[i]) - rotate);
		}
		return angles / nbPoints <= angleAccuracy;
	}


	// Draw line (add point)
	void Draw(Vector2 screenPos)
	{
		// Store point for comparison
		Vector2 pos = camera.ScreenToWorldPoint(screenPos);
		if (sample.Contains(pos))
			return;
		sample.Add(pos);

		// Draw point
		Vector3 point = new Vector3(pos.x, pos.y, 0);
		lineRenderer.positionCount = lineCount + 1;
		lineRenderer.SetPosition(lineCount, point);
		lines.Add(point);
		lineCount++;
	}


	// Clear canvas
	void Clear()
	{
		sample.Clear();
		lineRenderer.positionCount = 0;
		lines.Clear();
		lineCount = 0;
	}
}