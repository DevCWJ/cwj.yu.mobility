using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmazingAssets.AdvancedDissolve;
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class CutoutSlider : MonoBehaviour
{
    Material material;

    [Range(0, 1)] public float cutoutClip;

    private void Start()
    {
        //Instantiate material
        material = GetComponent<Renderer>().sharedMaterial;
    }

    private void Update()
    {
        AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, cutoutClip);
    }
}
