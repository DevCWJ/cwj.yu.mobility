//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

//fixed cwj 2020.07.06
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using CWJ;

[DisallowMultipleComponent]
public class Outline_Mesh : MonoBehaviour
{
    //#if UNITY_EDITOR

    //[UnityEditor.InitializeOnLoadMethod]
    //public static void InitializeOnLoad()
    //{
    //    CWJ.AccessibleEditor.EditorEventSystem.EditorWillSaveEvent += EditorEventSystem_EditorWillSaveEvent;
    //}

    //private static void EditorEventSystem_EditorWillSaveEvent(CWJ.AccessibleEditor.EditorEventSystem.SaveTarget saveTarget, bool isModified)
    //{
    //    if (!isModified) return;

    //    var outlines = CWJ.FindUtil.FindObjectsOfType_New<Outline_Mesh>(true, false);

    //    for (int i = 0; i < outlines.Length; i++)
    //    {
    //        outlines[i].Editor_Bake();
    //    }
    //}

    //#endif

    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    public enum Mode
    {
        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly
    }

    public void SetOutlineSetting(Mode outlineMode, Color outlineColor, float outlineWidth)
    {
        this.outlineMode = outlineMode;
        this.outlineColor = outlineColor;
        this.outlineWidth = outlineWidth;
        UpdateMaterialProperties();
    }

    public Mode OutlineMode
    {
        get { return outlineMode; }
        set
        {
            if (outlineMode == value) return;
            outlineMode = value;
            UpdateMaterialProperties();
        }
    }

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            if (outlineColor == value) return;
            outlineColor = value;
            UpdateMaterialProperties();
        }
    }

    public float OutlineWidth
    {
        get { return outlineWidth; }
        set
        {
            if (outlineWidth == value) return;
            outlineWidth = value;
            UpdateMaterialProperties();
        }
    }
#pragma warning disable

    [SerializeField, CWJ.GetComponentInChildren(isIncludeInactive: true, isFindOnlyWhenNull: true), ReadonlyConditional(EPlayMode.PlayMode)]
    private Renderer[] renderers;

    [SerializeField]
    private Mode outlineMode;

    [SerializeField]
    private Color outlineColor = Color.white;

    [SerializeField, Range(0f, 10f)]
    private float outlineWidth = 2f;

    [Header("Optional")]

    [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
    + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")
        , ReadonlyConditional(EPlayMode.PlayMode)]
    private bool isPrecomputeOutline = true;

#if UNITY_EDITOR
    public void SetPrecomputeOutline(bool value)
    {
        isPrecomputeOutline = value;
    }
#endif

    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }
    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();
    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();

    private Material outlineMaskMaterial = null;
    private Material outlineFillMaterial = null;

    private bool isInit = false;
#pragma warning restore

    private void Init()
    {
        // Instantiate outline materials
        if (outlineMaskMaterial == null)
        {
            outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
            outlineMaskMaterial.name = gameObject.name + "_OutlineMask (Instance)";
        }
        if (outlineFillMaterial == null)
        {
            outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));
            outlineFillMaterial.name = gameObject.name + "_OutlineFill (Instance)";
        }

        // Retrieve or generate smooth normals
        LoadSmoothNormals();

        isInit = true;
    }

    void OnEnable()
    {
        if (!isInit)
        {
            Init();
        }

        UpdateMaterialProperties();

        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }


    void OnDisable()
    {
        if (!isInit) return;

        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();

            materials.Remove(outlineMaskMaterial);
            materials.Remove(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    void OnDestroy()
    {
        if (outlineMaskMaterial != null)
        {
            Destroy(outlineMaskMaterial);
            outlineMaskMaterial = null;
        }

        if (outlineFillMaterial != null)
        {
            Destroy(outlineFillMaterial);
            outlineFillMaterial = null;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (isInit)
        {
            UpdateMaterialProperties();
        }

        if (!isPrecomputeOutline && bakeKeys != null && bakeKeys.Count > 0)
        {
            isEditorBaked = false;
            bakeKeys.Clear();
            bakeValues.Clear();
        }

        if (isPrecomputeOutline && (!isEditorBaked || bakeKeys.Count != bakeValues.Count))
        {
            Editor_Bake();
        }
    }

    [SerializeField, HideInInspector] bool isEditorBaked = false;
    void Editor_Bake()
    {
        bakeKeys.Clear();
        bakeValues.Clear();

        var bakedMeshHash = new HashSet<Mesh>();
        try
        {
            // Generate smooth normals for each mesh
            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {
                if (meshFilter.sharedMesh == null)
                {
                    Debug.LogError(meshFilter.gameObject.name, meshFilter);
                    continue;
                }

                if (!bakedMeshHash.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                bakeKeys.Add(meshFilter.sharedMesh);

                var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

                bakeValues.Add(new ListVector3() { data = smoothNormals });
            }
            isEditorBaked = true;
        }
        catch(Exception e)
        {
            Debug.LogError($"문제 확인후 {nameof(isPrecomputeOutline)}를 다시 켜주세요\n" + e.ToString());
            isEditorBaked = false;
        }
    }

    [InvokeButton]
    void Editor_BakeAllOutlineInScene()
    {
        if (!CWJ.AccessibleEditor.DisplayDialogUtil.DisplayDialog<Outline_Mesh>
            ("정말로 모든 Outline_Mesh의 Mesh데이터의 smooth normal값을 PreCompute하시겠습니까?", ok: "Yes", cancel: "No"))
        {
            return;
        }

        var outlines = CWJ.FindUtil.FindObjectsOfType_New<Outline_Mesh>(true, false);

        for (int i = 0; i < outlines.Length; i++)
        {
            outlines[i].Editor_Bake();
        }
    }
#endif

    void LoadSmoothNormals()
    {

        // Retrieve or generate smooth normals
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            // Skip if smooth normals have already been adopted
            if (meshFilter.sharedMesh == null || !registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            //if (!meshFilter.sharedMesh.isReadable)
            //{
            //    Debug.LogError("'" + meshFilter.sharedMesh.name + "' 's fbx file (Model_Importer) Need -> 'Read/Write Enabled' [√] (true)", meshFilter.sharedMesh);
            //    continue;
            //}

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (skinnedMeshRenderer.sharedMesh == null || !registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                continue;
            }

            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
        }
    }

    List<Vector3> SmoothNormals(Mesh mesh)
    {

        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += mesh.normals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    void UpdateMaterialProperties()
    {
        // Apply properties according to mode
        outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

        switch (outlineMode)
        {
            case Mode.OutlineAll:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.OutlineVisible:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.OutlineHidden:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.OutlineAndSilhouette:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.SilhouetteOnly:
                outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineFillMaterial.SetFloat("_OutlineWidth", 0);
                break;
        }
    }
}