using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CWJ;
using CWJ.AccessibleEditor;

public class AddonExample : MonoBehaviour
{
    [DrawHeaderAndLine("CWJ Attribute 2", AttributeUtil.EParameterColor.Indigo)]
    [IntPopup(new int[] { 0, 5, 10 }, new string[] { "Zero", "Five", "Ten" })] public int IntPopup;
    [StringPopup(new string[] { "Yes", "No" })] public string StringPopup;

    [AssetPopup] public ScriptableObject Asset;

    [EnumFlag] public _TestFlagsEnum PopupFlags;
    [EnumButtons] public _TestFlagsEnum ButtonFlags;
    [EnumButtons] public _TestEnum Buttons;

    [ListDisplay] public StringList StringList = new StringList();
    [ListDisplay] public IntArray IntArray = new IntArray(5);

    [InvokeButton]
    public void SetHideFlag(GameObject obj,HideFlags hideFlags)
    {
        obj.hideFlags = hideFlags;
    }

    [Header("Inline Children")]
    [InlineDisplay] public Vector2 InlineVector;
    [Space]

    [Maximum(100.0f)] public float MaximumFloat;
    [Maximum(100)] public int MaximumInt;
    [Minimum(0.0f)] public float MinimumFloat;
    [Minimum(0)] public int MinimumInt;

    [Slider(0, 100, 25)] public int IntSlider;
    [Slider(0.0f, 10.0f, 1.125f)] public float FloatSlider;

    [MinMaxSlider(0, 10, 2)] public int IntMinMaxSlider;
    [HideInInspector] public int IntMinMaxSliderMax;

    [MinMaxSlider(0.0f, 10.0f, 0.5f)] public float FloatMinMaxSlider;
    [HideInInspector] public float FloatMinMaxSliderMax;

    [Snap(5)] public int SnapInt;
    [Snap(0.5f)] public float SnapFloat;
}
