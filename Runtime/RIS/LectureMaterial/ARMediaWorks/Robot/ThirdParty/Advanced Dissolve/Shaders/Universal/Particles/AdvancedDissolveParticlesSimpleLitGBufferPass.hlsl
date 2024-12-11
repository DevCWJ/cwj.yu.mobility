#ifndef UNIVERSAL_PARTICLES_GBUFFER_SIMPLE_LIT_PASS_INCLUDED
#define UNIVERSAL_PARTICLES_GBUFFER_SIMPLE_LIT_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Particles.hlsl"

void InitializeInputData(VaryingsParticle input, half3 normalTS, out InputData inputData)
{
    inputData = (InputData)0;

    inputData.positionWS = input.positionWS.xyz;
    inputData.positionCS = input.clipPos;

#ifdef _NORMALMAP
    half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
    inputData.normalWS = TransformTangentToWorld(normalTS,
        half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));
#else
    half3 viewDirWS = input.viewDirWS;
    inputData.normalWS = input.normalWS;
#endif

    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);

#if SHADER_HINT_NICE_QUALITY
    viewDirWS = SafeNormalize(viewDirWS);
#endif

    inputData.viewDirectionWS = viewDirWS;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    inputData.shadowCoord = input.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
    inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
#else
    inputData.shadowCoord = float4(0, 0, 0, 0);
#endif

    inputData.fogCoord = 0; // not used for deferred shading
    inputData.vertexLighting = half3(0.0h, 0.0h, 0.0h);
    inputData.bakedGI = SampleSHPixel(input.vertexSH, inputData.normalWS);
    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.clipPos);
    inputData.shadowMask = half4(1, 1, 1, 1);

    #if defined(DEBUG_DISPLAY) && !defined(PARTICLES_EDITOR_META_PASS)
    inputData.vertexSH = input.vertexSH;
    #endif
}

inline void InitializeParticleSimpleLitSurfaceData(VaryingsParticle input, out SurfaceData outSurfaceData, float4 cutoutSource)
{
    outSurfaceData = (SurfaceData)0;

    ParticleParams particleParams;
    InitParticleParams(input, particleParams);

    outSurfaceData.normalTS = SampleNormalTS(particleParams.uv, particleParams.blendUv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
    half4 albedo = SampleAlbedo(TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), particleParams, cutoutSource);
    outSurfaceData.albedo = AlphaModulate(albedo.rgb, albedo.a);
    outSurfaceData.alpha = albedo.a;
#if defined(_EMISSION)
    outSurfaceData.emission = BlendTexture(TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap), particleParams.uv, particleParams.blendUv) * _EmissionColor.rgb;
#else
    outSurfaceData.emission = half3(0, 0, 0);
#endif
    half4 specularGloss = SampleSpecularSmoothness(particleParams.uv, particleParams.blendUv, albedo.a, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
    outSurfaceData.specular = specularGloss.rgb;
    outSurfaceData.smoothness = specularGloss.a;

#if defined(_DISTORTION_ON)
    outSurfaceData.albedo = Distortion(half4(outSurfaceData.albedo, outSurfaceData.alpha), outSurfaceData.normalTS, _DistortionStrengthScaled, _DistortionBlend, projectedPosition);
#endif

    outSurfaceData.metallic = 0.0; // unused
    outSurfaceData.occlusion = 1.0;
}

///////////////////////////////////////////////////////////////////////////////
//                  Vertex and Fragment functions                            //
///////////////////////////////////////////////////////////////////////////////

VaryingsParticle ParticlesLitGBufferVertex(AttributesParticle input)
{
    VaryingsParticle output;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);


#if defined(CURVEDWORLD_IS_INSTALLED) && !defined(CURVEDWORLD_DISABLED_ON)
   #ifdef CURVEDWORLD_NORMAL_TRANSFORMATION_ON
      CURVEDWORLD_TRANSFORM_VERTEX_AND_NORMAL(input.positionOS, input.normalOS, input.tangentOS)
   #else
      CURVEDWORLD_TRANSFORM_VERTEX(input.positionOS)
   #endif
#endif


    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
    half3 viewDirWS = GetWorldSpaceNormalizeViewDir(vertexInput.positionWS);

#ifdef _NORMALMAP
    output.normalWS = half4(normalInput.normalWS, viewDirWS.x);
    output.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
    output.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
#else
    output.normalWS = half3(normalInput.normalWS);
    output.viewDirWS = viewDirWS;
#endif

    OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

    output.positionWS.xyz = vertexInput.positionWS.xyz;
    output.positionWS.w = 0;
    output.clipPos = vertexInput.positionCS;
    output.color = GetParticleColor(input.color);

#if defined(_FLIPBOOKBLENDING_ON)
    #if defined(UNITY_PARTICLE_INSTANCING_ENABLED)
        GetParticleTexcoords(output.texcoord, output.texcoord2AndBlend, input.texcoords.xyxy, 0.0);
    #else
        GetParticleTexcoords(output.texcoord, output.texcoord2AndBlend, input.texcoords, input.texcoordBlend);
    #endif
#else
    GetParticleTexcoords(output.texcoord, input.texcoords.xy);
#endif

#if defined(_SOFTPARTICLES_ON) || defined(_FADING_ON) || defined(_DISTORTION_ON)
    output.projectedPosition = vertexInput.positionNDC;
#endif

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    output.shadowCoord = GetShadowCoord(vertexInput);
#endif


    //Advanced Dissolve
    ADVANCED_DISSOLVE_INIT_UV(output, input.texcoords.xy, vertexInput.positionCS)


    return output;
}


FragmentOutput ParticlesLitGBufferFragment(VaryingsParticle input)
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);


//Advanced Dissolve////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
float4 AD_CutoutSource = 0;

#if defined(_AD_STATE_ENABLED)

    float4 dissolveBase = 0;
    #if defined(_AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA) || defined(_AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR)
        dissolveBase = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.texcoord.xy);
        dissolveBase.rgb *= _BaseColor.rgb;
    #endif

	ADVANCED_DISSOLVE_SETUP_CUTOUT_SOURCE_USING_WS(input, dissolveBase, input.positionWS.xyz, input.normalWS.xyz)

    #if !defined(_ALPHATEST_ON)
        AdvancedDissolveClip(cutoutSource);
    #endif
    	
    float3 dissolveAlbedo = 0; 
    float3 dissolveEmission = 0;
	float dissolveBlend = AdvancedDissolveAlbedoEmission(cutoutSource, dissolveBase, dissolveAlbedo, dissolveEmission, input.texcoord.xy);


    AD_CutoutSource = cutoutSource;
#endif
//Advanced Dissolve/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    SurfaceData surfaceData;
    InitializeParticleSimpleLitSurfaceData(input, surfaceData, AD_CutoutSource);


//Advanced Dissolve/////////////////////////////////////////
#if defined(_AD_STATE_ENABLED)
    surfaceData.albedo = lerp(surfaceData.albedo, dissolveAlbedo, dissolveBlend);
    surfaceData.emission = lerp(surfaceData.emission, dissolveEmission, dissolveBlend);
#endif


    InputData inputData;
    InitializeInputData(input, surfaceData.normalTS, inputData);
    SETUP_DEBUG_TEXTURE_DATA(inputData, input.texcoord, _BaseMap);

    half4 color = half4(inputData.bakedGI * surfaceData.albedo + surfaceData.emission, surfaceData.alpha);

    return SurfaceDataToGbuffer(surfaceData, inputData, color.rgb, kLightingSimpleLit);

}

#endif // UNIVERSAL_PARTICLES_GBUFFER_SIMPLE_LIT_PASS_INCLUDED
