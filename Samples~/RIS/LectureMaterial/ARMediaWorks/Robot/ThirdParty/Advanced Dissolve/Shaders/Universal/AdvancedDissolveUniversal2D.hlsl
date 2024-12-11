#ifndef UNIVERSAL_FALLBACK_2D_INCLUDED
#define UNIVERSAL_FALLBACK_2D_INCLUDED

struct Attributes
{
    float4 positionOS       : POSITION;
    float2 uv               : TEXCOORD0;
    float3 normalOS         : NORMAL;
    float4 tangentOS        : TANGENT;
};

struct Varyings
{
    float2 uv        : TEXCOORD0;
    float4 vertex : SV_POSITION;

    float3 positionOS   : TEXCOORD1;
    float3 normalOS     : TEXCOORD2;
    //Advanced Dissolve
    ADVANCED_DISSOLVE_UV(3)
};

Varyings vert(Attributes input)
{
    Varyings output = (Varyings)0;


#if defined(CURVEDWORLD_IS_INSTALLED) && !defined(CURVEDWORLD_DISABLED_ON)
   #ifdef CURVEDWORLD_NORMAL_TRANSFORMATION_ON
      CURVEDWORLD_TRANSFORM_VERTEX_AND_NORMAL(input.positionOS, input.normalOS, input.tangentOS)
   #else
      CURVEDWORLD_TRANSFORM_VERTEX(input.positionOS)
   #endif
#endif


    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    output.vertex = vertexInput.positionCS;
    output.uv = TRANSFORM_TEX(input.uv, _BaseMap);


    output.positionOS = input.positionOS.xyz;
    output.normalOS = input.normalOS;
    //Advanced Dissolve
    ADVANCED_DISSOLVE_INIT_UV(output, input.uv, vertexInput.positionCS)

    return output;
}

half4 frag(Varyings input) : SV_Target
{

//Advanced Dissolve////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if defined(_AD_STATE_ENABLED)

    float4 dissolveBase = 0;
    #if defined(_AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA) || defined(_AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR)
        dissolveBase = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
        dissolveBase.rgb *= _BaseColor.rgb;
    #endif

	ADVANCED_DISSOLVE_SETUP_CUTOUT_SOURCE_USING_OS(input, dissolveBase, input.positionOS.xyz, input.normalOS.xyz)

    #if !defined(_ALPHATEST_ON)
        AdvancedDissolveClip(cutoutSource);
    #endif
    	
    float3 dissolveAlbedo = 0; 
    float3 dissolveEmission = 0;
	float dissolveBlend = AdvancedDissolveAlbedoEmission(cutoutSource, dissolveBase, dissolveAlbedo, dissolveEmission, input.uv);

#endif
//Advanced Dissolve/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    half2 uv = input.uv;
    half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
    half3 color = texColor.rgb * _BaseColor.rgb;
    half alpha = texColor.a * _BaseColor.a;


    float cutout = _Cutoff;
    //Advanced Dissolve/////////////////////////////////////////
    #if defined(_AD_STATE_ENABLED) && defined(_ALPHATEST_ON)
        AdvancedDissolveCalculateAlphaAndClip(cutoutSource, alpha, cutout);
    #endif


    AlphaDiscard(alpha, cutout);


//Advanced Dissolve/////////////////////////////////////////
#if defined(_AD_STATE_ENABLED)
    color.rgb = lerp(color.rgb, dissolveAlbedo, dissolveBlend);
    color += dissolveEmission * dissolveBlend;
#endif


#ifdef _ALPHAPREMULTIPLY_ON
    color *= alpha;
#endif
    return half4(color, alpha);
}

#endif
