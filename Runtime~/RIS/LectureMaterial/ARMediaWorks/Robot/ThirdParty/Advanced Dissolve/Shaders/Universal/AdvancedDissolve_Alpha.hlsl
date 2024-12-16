#ifndef ADVANCED_DISSOLVE_ALPHA
#define ADVANCED_DISSOLVE_ALPHA

half AdvancedDissolve_Alpha(half albedoAlpha, half4 color, half cutoff, float4 cutoutSource)
{
    #if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
        half alpha = albedoAlpha * color.a;
    #else
        half alpha = color.a;
    #endif


    //Advanced Dissolve/////////////////////////////////////////
    AdvancedDissolveCalculateAlphaAndClip(cutoutSource, alpha, cutoff);


    #if defined(_ALPHATEST_ON)
        clip(alpha - cutoff);
    #endif

    return alpha;
}


#endif