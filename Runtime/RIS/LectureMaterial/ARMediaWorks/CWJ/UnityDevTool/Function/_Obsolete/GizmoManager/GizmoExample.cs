//Addon - InGameGizmos 패키지사용할것

//using CWJ;

//using UnityEngine;

//[ExecuteInEditMode]
//public class GizmoExample : MonoBehaviour
//{
//    [GetComponent] public new Transform transform;
//    [GetComponent] public RectTransform rectTransform;

//    public bool isUI;
//    public float size = 1;
//    public Color color = Color.black;

//    private void OnEnable() { }

//    //private void OnDrawGizmos()
//    //{
//    //    if (!this.enabled) return;

//    //    if (isUI)
//    //    {
//    //        if (rectTransform == null) return;
//    //        Vector3 pos = Camera.main.ScreenToWorldPoint(rectTransform.position);

//    //        CWJ.GizmosExtension.DrawCircle(pos, size, color);
//    //    }
//    //    else
//    //    {
//    //        if (transform == null) return;

//    //        CWJ.GizmosExtension.DrawCircle(transform.position+transform.forward, size, color);
//    //    }
//    //}

//    private void Update()
//    {
//        if (!this.enabled) return;

//        if (isUI)
//        {
//            if (rectTransform == null) return;
//            Vector3 pos = Camera.main.ScreenToWorldPoint(rectTransform.position);

//            CWJ.GizmosExtension.DrawCircle(pos, size, color);
//        }
//        else
//        {
//            if (transform == null) return;

//            CWJ.GizmosExtension.DrawCircle(transform.position + transform.forward, size, color);
//        }
//    }
//}