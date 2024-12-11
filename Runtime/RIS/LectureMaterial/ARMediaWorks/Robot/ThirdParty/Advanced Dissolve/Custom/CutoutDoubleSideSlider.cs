
using UnityEngine;
using AmazingAssets.AdvancedDissolve;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class CutoutDoubleSideSlider : MonoBehaviour
{
    [SerializeField] Renderer outRenderer;
    [SerializeField] Renderer inRenderer;

    Material intMat, outMat;

    [Range(0, 1)] public float cutoutClip;

    private void Awake()
    {
        if (!outRenderer)
        {
            this.enabled = false;
        }
        intMat = inRenderer.sharedMaterial;        
        outMat = outRenderer.sharedMaterial;
        lastCoutout = -1;
    }

    private float lastCoutout = -1;

    private void Update()
    {
#if UNITY_EDITOR
        if (lastCoutout != cutoutClip)
        {
#endif
        lastCoutout = cutoutClip;
        AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(outMat,
            AdvancedDissolveProperties.Cutout.Standard.Property.Clip, cutoutClip);
        AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(intMat,
            AdvancedDissolveProperties.Cutout.Standard.Property.Clip, cutoutClip);
#if UNITY_EDITOR
        }
#endif
    }
}
