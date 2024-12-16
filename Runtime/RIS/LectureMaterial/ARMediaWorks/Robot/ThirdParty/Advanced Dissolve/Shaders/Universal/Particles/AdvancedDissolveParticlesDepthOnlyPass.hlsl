#ifndef UNIVERSAL_PARTICLES_LIT_DEPTH_ONLY_PASS_INCLUDED
#define UNIVERSAL_PARTICLES_LIT_DEPTH_ONLY_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

VaryingsDepthOnlyParticle DepthOnlyVertex(AttributesDepthOnlyParticle input)
{
    VaryingsDepthOnlyParticle output = (VaryingsDepthOnlyParticle)0;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);


#if defined(CURVEDWORLD_IS_INSTALLED) && !defined(CURVEDWORLD_DISABLED_ON)
      CURVEDWORLD_TRANSFORM_VERTEX(input.vertex)
#endif


    VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz);
    output.clipPos = vertexInput.positionCS;

    #if defined(_ALPHATEST_ON)
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
    #endif


    //Advanced Dissolve
    ADVANCED_DISSOLVE_INIT_UV(output, input.texcoords.xy, vertexInput.positionCS)

    return output;
}

half4 DepthOnlyFragment(VaryingsDepthOnlyParticle input) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);


//Advanced Dissolve////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if defined(_AD_STATE_ENABLED)

    float4 dissolveBase = 0;
    #if defined(_AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA) || defined(_AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR)
        dissolveBase = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.texcoord);
        dissolveBase.rgb *= _BaseColor.rgb;
    #endif

	ADVANCED_DISSOLVE_SETUP_CUTOUT_SOURCE_USING_OS(input, dissolveBase, input.positionOS, input.normalOS)

    #if !defined(_ALPHATEST_ON)
        AdvancedDissolveClip(cutoutSource);
    #endif

#endif
//Advanced Dissolve/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    // Check if we need to discard...
    #if defined(_ALPHATEST_ON)
        float2 uv = input.texcoord;
        half4 vertexColor = input.color;
        half4 baseColor = _BaseColor;

        #if defined(_FLIPBOOKBLENDING_ON)
            float3 blendUv = input.texcoord2AndBlend;
        #else
            float3 blendUv = float3(0,0,0);
        #endif

        half4 albedo = BlendTexture(TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), uv, blendUv) * baseColor;
        half4 colorAddSubDiff = half4(0, 0, 0, 0);
        #if defined (_COLORADDSUBDIFF_ON)
            colorAddSubDiff = _BaseColorAddSubDiff;
        #endif

        albedo = MixParticleColor(albedo, vertexColor, colorAddSubDiff);


        //Advanced Dissolve
        float cutout = _Cutoff;
        #if defined(_AD_STATE_ENABLED)
            AdvancedDissolveCalculateAlphaAndClip(cutoutSource, albedo.a, cutout);
        #endif


        AlphaDiscard(albedo.a, cutout);
    #endif

    return 0;
}

#endif // UNIVERSAL_PARTICLES_LIT_DEPTH_ONLY_PASS_INCLUDED
