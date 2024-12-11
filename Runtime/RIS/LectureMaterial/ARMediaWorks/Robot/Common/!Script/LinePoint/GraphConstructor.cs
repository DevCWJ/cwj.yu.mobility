using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using CWJ;

public class GraphConstructor : MonoBehaviour
{
    public RectTransform panelAxes;
    public RectTransform axesPivot;
    public RectTransform axesCenter;
    public string LabelX, LabelY, LabelZ;
    [FoldoutGroup("Object", true)]
    [SerializeField] LineRenderer X, Y, Z;
    [SerializeField] LineRenderer Xf, Xb, Xd;
    [SerializeField] LineRenderer Yl, Yf, Yr;
    [SerializeField] LineRenderer Zf, Zb, Zd;
    [SerializeField] TextMeshPro labelX, labelY, labelZ;
    [SerializeField] Transform XScaleL, XScaleH;
    [SerializeField] Transform YScaleL, YScaleH, ZScaleL;
    [FoldoutGroup("Object", false)]
    [SerializeField] Transform ZScaleH;

    [SerializeField]
    private float _sizeX = 500, _sizeY = 500, _sizeZ = 500;

    private void Init()
    {
        if (X != null && ZScaleH != null)
        {
            return;
        }
        X = axesCenter.Find("X").GetComponent<LineRenderer>();
        Xf = X.transform.Find("Xf").GetComponent<LineRenderer>();
        Xb = X.transform.Find("Xb").GetComponent<LineRenderer>();
        Xd = X.transform.Find("Xd").GetComponent<LineRenderer>();

        Y = axesCenter.Find("Y").GetComponent<LineRenderer>();
        Yl = Y.transform.Find("Yl").GetComponent<LineRenderer>();
        Yf = Y.transform.Find("Yf").GetComponent<LineRenderer>();
        Yr = Y.transform.Find("Yr").GetComponent<LineRenderer>();

        Z = axesCenter.Find("Z").GetComponent<LineRenderer>();
        Zf = Z.transform.Find("Zf").GetComponent<LineRenderer>();
        Zb = Z.transform.Find("Zb").GetComponent<LineRenderer>();
        Zd = Z.transform.Find("Zd").GetComponent<LineRenderer>();

        labelX = X.transform.Find("Label").GetComponent<TextMeshPro>();
        labelY = Y.transform.Find("Label").GetComponent<TextMeshPro>();
        labelZ = Z.transform.Find("Label").GetComponent<TextMeshPro>();

        XScaleL = X.transform.Find("ScaleL");
        XScaleH = X.transform.Find("ScaleH");
        YScaleL = Y.transform.Find("ScaleL");
        YScaleH = Y.transform.Find("ScaleH");
        ZScaleL = Z.transform.Find("ScaleL");
        ZScaleH = Z.transform.Find("ScaleH");
    }


    public float sizeX 
    {
        set {
            _sizeX = value;
            if (value < 1) _sizeX = 1;
            DataChanged();
        }
        get { return _sizeX; }
    }   

    public float sizeY
    {
        set {
            _sizeY = value;
            if (value < 1) _sizeY = 1;
            DataChanged();
        }
        get { return _sizeY; }
    }   
    public float sizeZ
    {
        set {
            _sizeZ = value;
            if (value < 1) _sizeZ = 1;
            DataChanged();
        }
        get { return _sizeZ; }
    }   

    private void UpdateAxes()
    {
        float sizeXZ = Mathf.Sqrt(sizeX * sizeX + sizeZ * sizeZ);
        float sizeXYZ = Mathf.Sqrt(sizeX * sizeX + sizeZ * sizeZ + sizeY * sizeY);
        panelAxes.sizeDelta = new Vector2(sizeXZ, sizeXYZ);
        panelAxes.anchoredPosition = new Vector3(0, 0, 0);

        axesPivot.localPosition = new Vector3(0, 0, (-sizeXYZ * 0.5f));
        //CenterAxe
        axesCenter.localPosition = new Vector3(-sizeX * 0.5f, -sizeY * 0.5f, sizeZ * 0.5f);
        //X
        X.transform.position = axesCenter.position;
        X.SetPosition(0, new Vector3(0, 0, 0));
        X.SetPosition(1, new Vector3(0, 0, -sizeZ));
        //Xf
        Xf.transform.position = X.transform.position;
        Xf.transform.localPosition += new Vector3(0, sizeY, 0);
        Xf.SetPosition(0, X.GetPosition(0));
        Xf.SetPosition(1, X.GetPosition(1));
        //Xb
        Xb.transform.position = X.transform.position;
        Xb.transform.localPosition += new Vector3(sizeX, sizeY, 0);
        Xb.SetPosition(0, X.GetPosition(0));
        Xb.SetPosition(1, X.GetPosition(1));
        //Xd
        Xd.transform.position = X.transform.position;
        Xd.transform.localPosition += new Vector3(sizeX, 0, 0);
        Xd.SetPosition(0, X.GetPosition(0));
        Xd.SetPosition(1, X.GetPosition(1));
        //Y
        Y.transform.position = axesCenter.position;
        Y.SetPosition(0, new Vector3(0, 0, 0));
        Y.SetPosition(1, new Vector3(sizeX, 0, 0));
        //Yl
        Yl.transform.position = Y.transform.position;
        Yl.transform.localPosition += new Vector3(0, 0, -sizeZ);
        Yl.SetPosition(0, Y.GetPosition(0));
        Yl.SetPosition(1, Y.GetPosition(1));
        //Yr
        Yr.transform.position = Y.transform.position;
        Yr.transform.localPosition += new Vector3(0, sizeY, 0);
        Yr.SetPosition(0, Y.GetPosition(0));
        Yr.SetPosition(1, Y.GetPosition(1));
        //Yf
        Yf.transform.position = Y.transform.position;
        Yf.transform.localPosition += new Vector3(0, sizeY, -sizeZ);
        Yf.SetPosition(0, Y.GetPosition(0));
        Yf.SetPosition(1, Y.GetPosition(1));
        //Z
        Z.transform.position = axesCenter.position;
        Z.SetPosition(0, new Vector3(0, 0, 0));
        Z.SetPosition(1, new Vector3(0, sizeY, 0));
        //Zf
        Zf.transform.position = Z.transform.position;
        Zf.transform.localPosition += new Vector3(sizeX, 0, -sizeZ);
        Zf.SetPosition(0, Z.GetPosition(0));
        Zf.SetPosition(1, Z.GetPosition(1));
        //Zb
        Zb.transform.position = Z.transform.position;
        Zb.transform.localPosition += new Vector3(0, 0, -sizeZ);
        Zb.SetPosition(0, Z.GetPosition(0));
        Zb.SetPosition(1, Z.GetPosition(1));
        //Zd
        Zd.transform.position = Z.transform.position;
        Zd.transform.localPosition += new Vector3(sizeX, 0, 0);
        Zd.SetPosition(0, Z.GetPosition(0));
        Zd.SetPosition(1, Z.GetPosition(1));
    }    

    private void DataChanged()
    {            
        Init();

        UpdateAxes();

        void SetPlaneSize(LineRenderer xyz, Vector3 pos, Vector2 scale)
        {
            var xPlaneRectTrf = xyz.GetComponentInChildren<MeshCollider>(true).GetComponent<RectTransform>();
            xPlaneRectTrf.anchoredPosition3D = pos * 0.5f;
            xPlaneRectTrf.localScale = new Vector3(scale.x * 0.1f, 1, scale.y * 0.1f);
        }
        SetPlaneSize(X, new Vector3(sizeX, sizeY, 0), new Vector2(sizeX, sizeY));
        SetPlaneSize(Y, new Vector3(0, sizeY, -sizeZ), new Vector2(sizeZ, sizeY));
        SetPlaneSize(Z, new Vector3(sizeX, 0, -sizeZ), new Vector2(sizeZ, sizeX));

        var boxCol = axesPivot.GetComponent<BoxCollider>();
        boxCol.center = Vector3.zero;
        boxCol.size = new Vector3(sizeX, sizeY, sizeZ);
    }

    private void Reset()
    {
        Init();
    }

    void OnValidate()
    {
        //Debug.Log("OnValidate ( )");
        _sizeX = _sizeX < 1 ? 1 : _sizeX;
        _sizeY = _sizeY < 1 ? 1 : _sizeY;
        _sizeZ = _sizeZ < 1 ? 1 : _sizeZ;
        DataChanged();

        labelX.SetText(LabelX);
        labelY.SetText(LabelY);
        labelZ.SetText(LabelZ);

        labelX.transform.localPosition = new Vector3(0, /*Y.GetPosition(1).y*/0, -sizeZ - 25);
        labelY.transform.localPosition = new Vector3(sizeX + 25, 0, 0);
        labelZ.transform.localPosition = new Vector3(0, sizeY+25, 0);

        if (XScaleL)
            XScaleL.localPosition = new Vector3(0, 0, X.GetPosition(0).z + 25);
        if (XScaleH)
            XScaleH.localPosition = new Vector3(0, 0, X.GetPosition(1).z - 25);
        if (YScaleL)
            YScaleL.localPosition = new Vector3(Y.GetPosition(0).x - 25, 0, 0);
        if (YScaleH)
            YScaleH.localPosition = new Vector3(Y.GetPosition(1).x + 25, 0, 0);
        if (ZScaleL)
            ZScaleL.localPosition = new Vector3(0, Z.GetPosition(0).y - 25, 0);
        if (ZScaleH)
            ZScaleH.localPosition = new Vector3(0, Z.GetPosition(1).y + 25, 0);
    }

}


