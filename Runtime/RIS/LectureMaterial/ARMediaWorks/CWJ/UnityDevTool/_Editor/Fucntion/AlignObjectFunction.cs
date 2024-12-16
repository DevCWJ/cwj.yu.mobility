#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshFilter))]
public class AlignObjectFunction : Editor
{
    private bool RaycastToMousePos(Vector2 mousePos, out RaycastHit hit)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
        return (Physics.Raycast(ray, out hit));
    }

    void OnSceneGUI()
    {
        Event e = Event.current;

        if (e == null) return;
        if (e.type != EventType.KeyDown) return;

        if (e.shift)
        {
            if (e.keyCode == KeyCode.R)
            {
                RaycastHit hit;
                if (RaycastToMousePos(e.mousePosition, out hit))
                {
                    Undo.RecordObject(Selection.gameObjects[0].transform, "Aligned GameObject");

                    Selection.gameObjects[0].transform.position = hit.transform.gameObject.transform.position;
                    Selection.gameObjects[0].transform.rotation = hit.transform.gameObject.transform.rotation;
                    Debug.Log("Aligned " + Selection.gameObjects[0].name + " to " + hit.transform.gameObject.name);
                }
                else
                    Debug.LogError("Ray cast didn't hit. Make sure objects you want to align to has a collider.");
            }
            else if (e.keyCode == KeyCode.T)
            {
                RaycastHit hit;
                if (RaycastToMousePos(e.mousePosition, out hit))
                {
                    Undo.RecordObject(Selection.activeGameObject, "Teleported GameObject");
                    Selection.activeGameObject.transform.position = hit.point;
                    //Selection.gameObjects[0].transform.position = hit.point;
                    Debug.Log("Teleported " + Selection.gameObjects[0].name + " to " + hit.point);
                }
                else
                    Debug.LogError("Ray cast didn't hit. Check to see if the surface you're teleporting to has a collider.");
            }
        }
    }
}

#endif