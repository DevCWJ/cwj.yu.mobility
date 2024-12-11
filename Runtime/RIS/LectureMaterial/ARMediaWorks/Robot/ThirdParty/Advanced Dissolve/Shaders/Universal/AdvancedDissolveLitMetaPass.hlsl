#ifndef UNIVERSAL_LIT_META_PASS_INCLUDED
#define UNIVERSAL_LIT_META_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 uv0          : TEXCOORD0;
    float2 uv1          : TEXCOORD1;
    float2 uv2          : TEXCOORD2;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4 positionCS   : SV_POSITION;
    float2 uv           : TEXCOORD0;
	#ifdef EDITOR_VISUALIZATION
	    float2 VizUV        : TEXCOORD1;
	    float4 LightCoord   : TEXCOORD2;
	#endif

    //Advanced Dissolve
	float3 positionOS    : TEXCOORD3;
    float3 normalOS      : TEXCOORD4;

	ADVANCED_DISSOLVE_UV(5)
};

Varyings UniversalVertexMeta(Attributes input)
{
    Varyings output = (Varyings)0;
    output.positionCS = UnityMetaVertexPosition(input.positionOS.xyz, input.uv1, input.uv2);
    output.uv = TRANSFORM_TEX(input.uv0, _BaseMap);
	#ifdef EDITOR_VISUALIZATION
	    UnityEditorVizData(input.positionOS.xyz, input.uv0, input.uv1, input.uv2, output.VizUV, output.LightCoord);
	#endif


    //Advanced Dissolve 
	output.positionOS = input.positionOS.xyz;
	output.normalOS = input.normalOS;

	ADVANCED_DISSOLVE_INIT_UV(output, input.uv0, output.positionCS)


    return output;
}

half4 UniversalFragmentMeta(Varyings fragIn, MetaInput metaInput)
{
#ifdef EDITOR_VISUALIZATION
    metaInput.VizUV = fragIn.VizUV;
    metaInput.LightCoord = fragIn.LightCoord;
#endif

    return UnityMetaFragment(metaInput);
}

half4 UniversalFragmentMetaLit(Varyings input) : SV_Target
{

//Advanced Dissolve////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if defined(_AD_STATE_ENABLED)

    float4 dissolveBase = 0;
    #if defined(_AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA) || defined(_AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR)
        dissolveBase = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
        dissolveBase.rgb *= _BaseColor.rgb;
    #endif 

	ADVANCED_DISSOLVE_SETUP_CUTOUT_SOURCE_USING_OS(input, dissolveBase, input.positionOS, input.normalOS)
    	
    float3 dissolveAlbedo = 0; 
    float3 dissolveEmission = 0;
	float dissolveBlend = AdvancedDissolveAlbedoEmission(cutoutSource, dissolveBase, dissolveAlbedo, dissolveEmission, input.uv);

#endif
//Advanced Dissolve/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.Emission = surfaceData.emission;


//Advanced Dissolve/////////////////////////////////////////
#if defined(_AD_STATE_ENABLED)
    metaInput.Albedo = lerp(metaInput.Albedo, dissolveAlbedo, dissolveBlend);
    metaInput.Emission = lerp(metaInput.Emission, dissolveEmission, dissolveBlend);
#endif
 

    return UniversalFragmentMeta(input, metaInput);
}
#endif
