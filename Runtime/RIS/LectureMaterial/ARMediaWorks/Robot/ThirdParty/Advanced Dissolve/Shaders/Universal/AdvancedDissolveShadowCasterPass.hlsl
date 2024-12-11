#ifndef UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED
#define UNIVERSAL_SHADOW_CASTER_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"


//Advnaced Dissolve
#include "AdvancedDissolve_Alpha.hlsl"


// Shadow Casting Light geometric parameters. These variables are used when applying the shadow Normal Bias and are set by UnityEngine.Rendering.Universal.ShadowUtils.SetupShadowCasterConstantBuffer in com.unity.render-pipelines.universal/Runtime/ShadowUtils.cs
// For Directional lights, _LightDirection is used when applying shadow Normal Bias.
// For Spot lights and Point lights, _LightPosition is used to compute the actual light direction because it is different at each shadow caster geometry vertex.
float3 _LightDirection;
float3 _LightPosition;

struct Attributes
{
    float4 positionOS   : POSITION;
    float3 normalOS     : NORMAL;
    float2 texcoord     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float2 uv           : TEXCOORD0;
    float4 positionCS   : SV_POSITION;

    //Advanced Dissolve
	float3 positionOS    : TEXCOORD1;
    float3 normalOS      : TEXCOORD2;

	ADVANCED_DISSOLVE_UV(3)
};

float4 GetShadowPositionHClip(Attributes input)
{
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

#if _CASTING_PUNCTUAL_LIGHT_SHADOW
    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
#else
    float3 lightDirectionWS = _LightDirection;
#endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif

    return positionCS;
}

Varyings ShadowPassVertex(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);


#if defined(CURVEDWORLD_IS_INSTALLED) && !defined(CURVEDWORLD_DISABLED_ON)  
      CURVEDWORLD_TRANSFORM_VERTEX(input.positionOS)
#endif


    output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.positionCS = GetShadowPositionHClip(input);


    //Advanced Dissolve 
    output.positionOS = input.positionOS.xyz;
	output.normalOS = input.normalOS;

	ADVANCED_DISSOLVE_INIT_UV(output, input.texcoord, output.positionCS)


    return output;
}

half4 ShadowPassFragment(Varyings input) : SV_TARGET
{

//Advanced Dissolve////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if defined(_AD_STATE_ENABLED)

    float4 dissolveBase = 0;
    #if defined(_AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA) || defined(_AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR)
        dissolveBase = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
        dissolveBase.rgb *= _BaseColor.rgb;
    #endif

	ADVANCED_DISSOLVE_SETUP_CUTOUT_SOURCE_USING_OS(input, dissolveBase, input.positionOS, input.normalOS)

    #if !defined(_ALPHATEST_ON)
        AdvancedDissolveClip(cutoutSource);
    #endif

#endif
//Advanced Dissolve/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	#if defined(_AD_STATE_ENABLED)
    	AdvancedDissolve_Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff, cutoutSource);
    #else
        Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff);
    #endif
    
    return 0;
}

#endif
