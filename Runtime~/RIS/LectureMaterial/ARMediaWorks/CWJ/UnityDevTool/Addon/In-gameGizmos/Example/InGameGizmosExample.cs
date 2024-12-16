using UnityEngine;

using Gizmos = Popcron.Gizmos;

[ExecuteAlways]
public class InGameGizmosExample : MonoBehaviour
{
    public Material material = null;

    private void Reset()
    {
        material = new Material(Shader.Find("Sprites/Default"));
    }

    private void Awake()
    {
        Gizmos.Enabled = true;

        Gizmos.CameraFilter += cam =>
        {
            return true;
        };
    }

    private void OnRenderObject()
    {
        Gizmos.Line(transform.position, Vector3.one, Color.green, true);

        Gizmos.Cube(transform.position, transform.rotation, transform.lossyScale);
    }

    private void Update()
    {
        Gizmos.Material = material;

        //can also draw from update
        Gizmos.Cone(transform.position, transform.rotation, 15f, 45f, Color.green);
    }
}