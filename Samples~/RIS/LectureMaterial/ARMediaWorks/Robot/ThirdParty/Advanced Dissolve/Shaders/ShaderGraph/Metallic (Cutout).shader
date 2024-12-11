Shader "Amazing Assets/Advanced Dissolve/Shader Graph/Metallic (Cutout)"
{
    Properties
    {
//Advanced Dissolve Properties Start////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Cutout
[HideInInspector]                                                   _AdvancedDissolveCutoutStandardClip("", Range(0,1)) = 0.5

[HideInInspector]											        _AdvancedDissolveCutoutStandardMap1("", 2D) = "white" { }
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap1Tiling("", Vector) = (1, 1, 1, 0)
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap1Offset("", Vector) = (0, 0, 0, 0)
[HideInInspector]					                                _AdvancedDissolveCutoutStandardMap1Scroll("", Vector) = (0, 0, 0, 0)
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap1Intensity("", Range(0, 1)) = 1
[HideInInspector][Enum(Red, 0, Green, 1, Blue, 2, Alpha, 3)]        _AdvancedDissolveCutoutStandardMap1Channel("", INT) = 3
[HideInInspector][AdvancedDissolveToggleFloat]				        _AdvancedDissolveCutoutStandardMap1Invert("", INT) = 0
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap2("", 2D) = "white" { }
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap2Tiling("", Vector) = (1, 1, 1, 0)
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap2Offset("", Vector) = (0, 0, 0, 0)
[HideInInspector]					                                _AdvancedDissolveCutoutStandardMap2Scroll("", Vector) = (0, 0, 0, 0)
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap2Intensity("", Range(0, 1)) = 1
[HideInInspector][Enum(Red, 0, Green, 1, Blue, 2, Alpha, 3)]        _AdvancedDissolveCutoutStandardMap2Channel("", INT) = 3
[HideInInspector][AdvancedDissolveToggleFloat]				        _AdvancedDissolveCutoutStandardMap2Invert("", INT) = 0
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap3("", 2D) = "white" { }
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap3Tiling("", Vector) = (1, 1, 1, 0)
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap3Offset("", Vector) = (0, 0, 0, 0)
[HideInInspector]					                                _AdvancedDissolveCutoutStandardMap3Scroll("", Vector) = (0, 0, 0, 0)
[HideInInspector]											        _AdvancedDissolveCutoutStandardMap3Intensity("", Range(0, 1)) = 1
[HideInInspector][Enum(Red, 0, Green, 1, Blue, 2, Alpha, 3)]        _AdvancedDissolveCutoutStandardMap3Channel("", INT) = 3
[HideInInspector][AdvancedDissolveToggleFloat]				        _AdvancedDissolveCutoutStandardMap3Invert("", INT) = 0

[HideInInspector][Enum(Multiply, 0, Add, 1)]				        _AdvancedDissolveCutoutStandardMapsBlendType("", Float) = 0
[HideInInspector][Enum(World, 0, Local, 1)]					        _AdvancedDissolveCutoutStandardMapsTriplanarMappingSpace("", Float) = 0	
[HideInInspector][Enum(Constant, 0, Camera Relative, 1)]            _AdvancedDissolveCutoutStandardMapsScreenSpaceUVScale("", Float) = 0
[HideInInspector][AdvancedDissolveToggleFloat]				        _AdvancedDissolveCutoutStandardBaseInvert("", INT) = 0

//Geometric
[HideInInspector][AdvancedDissolveToggleFloat]			    	    _AdvancedDissolveCutoutGeometricInvert("", Float) = 0
[HideInInspector]										    	    _AdvancedDissolveCutoutGeometricNoise("", Float) = 0.1	

[HideInInspector][Enum(X, 0, Y, 1, Z, 2)]                           _AdvancedDissolveCutoutGeometricXYZAxis("", Float) = 0
[HideInInspector][Enum(Linear, 0, Symmetrical, 1)]                  _AdvancedDissolveCutoutGeometricXYZStyle("", Float) = 0 
[HideInInspector][Enum(World, 0, Local, 1)]                         _AdvancedDissolveCutoutGeometricXYZSpace("", Float) = 0	 
[HideInInspector]											        _AdvancedDissolveCutoutGeometricXYZRollout("", Float) = 0
[HideInInspector]											        _AdvancedDissolveCutoutGeometricXYZPosition("", Vector) = (0, 0, 0, 0)

[HideInInspector]										    	    _AdvancedDissolveCutoutGeometric1Position("", Vector) = (0,0,0,0)
[HideInInspector]										    	    _AdvancedDissolveCutoutGeometric1Normal("", Vector) = (1,0,0,0)
[HideInInspector]										    	    _AdvancedDissolveCutoutGeometric1Radius("", Float) = 1
[HideInInspector]										    	    _AdvancedDissolveCutoutGeometric1Height("", Float) = 1

[HideInInspector]										    	    _AdvancedDissolveCutoutGeometric2Position("", Vector) = (0,0,0,0)
[HideInInspector]									    		    _AdvancedDissolveCutoutGeometric2Normal("", Vector) = (1,0,0,0)
[HideInInspector]									    		    _AdvancedDissolveCutoutGeometric2Radius("", Float) = 1
[HideInInspector]									    		    _AdvancedDissolveCutoutGeometric2Height("", Float) = 1
 
[HideInInspector]									    		    _AdvancedDissolveCutoutGeometric3Position("", Vector) = (0,0,0,0)
[HideInInspector]									    		    _AdvancedDissolveCutoutGeometric3Normal("", Vector) = (1,0,0,0)
[HideInInspector]									    		    _AdvancedDissolveCutoutGeometric3Radius("", Float) = 1
[HideInInspector]										    	    _AdvancedDissolveCutoutGeometric3Height("", Float) = 1

[HideInInspector]										    	    _AdvancedDissolveCutoutGeometric4Position("", Vector) = (0,0,0,0)
[HideInInspector]											        _AdvancedDissolveCutoutGeometric4Normal("", Vector) = (1,0,0,0)
[HideInInspector]											        _AdvancedDissolveCutoutGeometric4Radius("", Float) = 1
[HideInInspector]											        _AdvancedDissolveCutoutGeometric4Height("", Float) = 1

//Edge
[HideInInspector]										    	    _AdvancedDissolveEdgeBaseWidthStandard("", Range(0,1)) = 0.1 
[HideInInspector]										    	    _AdvancedDissolveEdgeBaseWidthGeometric("", Range(0,1)) = 0.1 
[HideInInspector][Enum(Solid, 0, Smooth, 1, Smoother, 2)]           _AdvancedDissolveEdgeBaseShape("", INT) = 0
[HideInInspector][AdvancedDissolveColorRGB]  				        _AdvancedDissolveEdgeBaseColor("", Color) = (0,1,0,1)
[HideInInspector]											        _AdvancedDissolveEdgeBaseColorTransparency("", Range(0, 1)) = 1
[HideInInspector][AdvancedDissolveExponental]                       _AdvancedDissolveEdgeBaseColorIntensity("", Vector) = (0, 0, 0, 0)		

[HideInInspector][AdvancedDissolveColorRGB]					        _AdvancedDissolveEdgeAdditionalColor("", color) = (1, 0, 0, 1)
[HideInInspector]											        _AdvancedDissolveEdgeAdditionalColorTransparency("", Range(0, 1)) = 1
[HideInInspector][AdvancedDissolveExponental]			            _AdvancedDissolveEdgeAdditionalColorIntensity("", Vector) = (0, 0, 0, 0)
[HideInInspector]								                    _AdvancedDissolveEdgeAdditionalColorMap("", 2D) = "white" { }
[HideInInspector]					                                _AdvancedDissolveEdgeAdditionalColorMapTiling("", Vector) = (1, 1, 1, 0)
[HideInInspector]					                                _AdvancedDissolveEdgeAdditionalColorMapOffset("", Vector) = (0, 0, 0, 0)
[HideInInspector]					                                _AdvancedDissolveEdgeAdditionalColorMapScroll("", Vector) = (0, 0, 0, 0)
[HideInInspector][AdvancedDissolveToggleFloat]				        _AdvancedDissolveEdgeAdditionalColorMapReverse("", FLOAT) = 0
[HideInInspector]											        _AdvancedDissolveEdgeAdditionalColorMapMipmap("", Range(0, 10)) = 1	
[HideInInspector]											        _AdvancedDissolveEdgeAdditionalColorPhaseOffset("", FLOAT) = 0
[HideInInspector]											        _AdvancedDissolveEdgeAdditionalColorAlphaOffset("", Range(-1, 1)) = 0	
[HideInInspector][AdvancedDissolveToggleFloat]				        _AdvancedDissolveEdgeAdditionalColorClipInterpolation("", Float) = 0


[HideInInspector]								                    _AdvancedDissolveEdgeUVDistortionMap("", 2D) = "black" { }
[HideInInspector]					                                _AdvancedDissolveEdgeUVDistortionMapTiling("", Vector) = (1, 1, 1, 0)
[HideInInspector]					                                _AdvancedDissolveEdgeUVDistortionMapOffset("", Vector) = (0, 0, 0, 0)
[HideInInspector]					                                _AdvancedDissolveEdgeUVDistortionMapScroll("", Vector) = (0, 0, 0, 0)
[HideInInspector]				                                    _AdvancedDissolveEdgeUVDistortionStrength("", Float) = 0

[HideInInspector][AdvancedDissolvePositiveFloat]			        _AdvancedDissolveEdgeGIMetaPassMultiplier("", Float) = 1

//Keywords
[HideInInspector][AdvancedDissolveKeywordState]                     _AdvancedDissolveKeywordState("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordCutoutStandardSource]      _AdvancedDissolveKeywordCutoutStandardSource("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordCutoutStandardMappingType] _AdvancedDissolveKeywordCutoutStandardSourceMapsMappingType("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordCutoutGeometricType]       _AdvancedDissolveKeywordCutoutGeometricType("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordCutoutGeometricCount]      _AdvancedDissolveKeywordCutoutGeometricCount("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordEdgeBaseSource]            _AdvancedDissolveKeywordEdgeBaseSource("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordEdgeAdditionalColorSource] _AdvancedDissolveKeywordEdgeAdditionalColorSource("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordEdgeUVDistortionSource]    _AdvancedDissolveKeywordEdgeUVDistortionSource("", INT) = 0
[HideInInspector][AdvancedDissolveKeywordGlobalControlID]           _AdvancedDissolveKeywordGlobalControlID("", INT) = 0

//BakedKeywords
[HideInInspector]                                                   _AdvancedDissolveBakedKeywords("", Vector) = (0,0,0,0)	

//Advanced Dissolve Properties End////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        _Color("Color", Color) = (0, 0, 0, 0)
        _Cutout("Alpha Cutoff", Range(0, 1)) = 0.5
        [NoScaleOffset]_MainTex("Albedo", 2D) = "white" {}
        _AlbedoTiling("Tiling", Vector) = (1, 1, 0, 0)
        _Metallic("Metallic", Range(0, 1)) = 0.5
        [NoScaleOffset]_MetallicGlossMap("Metallic Map", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0, 1)) = 0.5
        [Normal][NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_DetailMask("Detail Mask", 2D) = "white" {}
        _DetailTiling("Detail Tiling", Vector) = (1, 1, 0, 0)
        [NoScaleOffset]_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
        _DetailNormalMapScale("Scale", Float) = 1
        [Normal][NoScaleOffset]_DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue"="AlphaTest"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalLitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Off
        Blend One Zero
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _LIGHT_COOKIES
        #pragma multi_compile _ _CLUSTERED_RENDERING
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_FORWARD
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 TangentSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV : INTERP0;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV : INTERP1;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh : INTERP2;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord : INTERP3;
            #endif
             float4 tangentWS : INTERP4;
             float4 texCoord0 : INTERP5;
             float4 fogFactorAndVertexLight : INTERP6;
             float3 positionWS : INTERP7;
             float3 normalWS : INTERP8;
             float3 viewDirectionWS : INTERP9;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.fogFactorAndVertexLight.xyzw = input.fogFactorAndVertexLight;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            output.viewDirectionWS.xyz = input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.fogFactorAndVertexLight = input.fogFactorAndVertexLight.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            output.viewDirectionWS = input.viewDirectionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void LerpWhiteTo_float(float3 b, float t, out float3 _out){
            half oneMinusT = 1 - t;
            _out = half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0 = _Color;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float4 _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2;
            Unity_Multiply_float4_float4(_Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0, _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0, _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2);
            UnityTexture2D _Property_8989c0284a1046d69bd798a916ddef61_Out_0 = UnityBuildTexture2DStructNoScale(_DetailAlbedoMap);
            float2 _Property_0733227372e0459db0761a6b7c5789c5_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_0733227372e0459db0761a6b7c5789c5_Out_0, float2 (0, 0), _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3);
            float4 _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8989c0284a1046d69bd798a916ddef61_Out_0.tex, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.samplerstate, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.GetTransformedUV(_TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3));
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_R_4 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.r;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_G_5 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.g;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_B_6 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.b;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_A_7 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.a;
            UnityTexture2D _Property_7caac4574f344834b84b0bee767e2ee4_Out_0 = UnityBuildTexture2DStructNoScale(_DetailMask);
            float2 _Property_e60862a15bee495aa98a2c32e7082a19_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_e60862a15bee495aa98a2c32e7082a19_Out_0, float2 (0, 0), _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3);
            float4 _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7caac4574f344834b84b0bee767e2ee4_Out_0.tex, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.samplerstate, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.GetTransformedUV(_TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3));
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_R_4 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.r;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_G_5 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.g;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_B_6 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.b;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.a;
            float3 _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2;
            LerpWhiteTo_float((_SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.xyz), _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7, _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2);
            float3 _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            Unity_Multiply_float3_float3((_Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2.xyz), _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2, _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2);
            UnityTexture2D _Property_9120b0d7949f4988abd0796affa542a4_Out_0 = UnityBuildTexture2DStructNoScale(_BumpMap);
            float2 _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0, float2 (0, 0), _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3);
            float4 _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9120b0d7949f4988abd0796affa542a4_Out_0.tex, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.samplerstate, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.GetTransformedUV(_TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3));
            _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0);
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_R_4 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.r;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_G_5 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.g;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_B_6 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.b;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_A_7 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.a;
            UnityTexture2D _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0 = UnityBuildTexture2DStructNoScale(_DetailNormalMap);
            float2 _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0, float2 (0, 0), _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3);
            float4 _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.tex, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.samplerstate, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.GetTransformedUV(_TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3));
            _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0);
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_R_4 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.r;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_G_5 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.g;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_B_6 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.b;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_A_7 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.a;
            float _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0 = _DetailNormalMapScale;
            float3 _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2;
            Unity_NormalStrength_float((_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.xyz), _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0, _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2);
            float3 _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            Unity_NormalBlend_float((_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.xyz), _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2, _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2);
            UnityTexture2D _Property_e6e5d425f963466dbb073b5ebed88524_Out_0 = UnityBuildTexture2DStructNoScale(_MetallicGlossMap);
            float2 _Property_139a6620cc4d432387db56ea48a30754_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_da978c144900420697b563b2da61024c_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_139a6620cc4d432387db56ea48a30754_Out_0, float2 (0, 0), _TilingAndOffset_da978c144900420697b563b2da61024c_Out_3);
            float4 _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0 = SAMPLE_TEXTURE2D(_Property_e6e5d425f963466dbb073b5ebed88524_Out_0.tex, _Property_e6e5d425f963466dbb073b5ebed88524_Out_0.samplerstate, _Property_e6e5d425f963466dbb073b5ebed88524_Out_0.GetTransformedUV(_TilingAndOffset_da978c144900420697b563b2da61024c_Out_3));
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_R_4 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.r;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_G_5 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.g;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_B_6 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.b;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_A_7 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.a;
            float _Property_5a43ed32f9844383a3fc2e9c22de45a6_Out_0 = _Metallic;
            float _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_R_4, _Property_5a43ed32f9844383a3fc2e9c22de45a6_Out_0, _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2);
            float _Property_4a309e00f2a0447e823f4ede6802e827_Out_0 = _Glossiness;
            float _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_A_7, _Property_4a309e00f2a0447e823f4ede6802e827_Out_0, _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2);
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.BaseColor = _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            surface.NormalTS = _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2;
            surface.Smoothness = _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2;
            surface.Occlusion = 1;
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//UniversalForward
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.BaseColor, surface.Emission, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }
        
        // Render State
        Cull Off
        Blend One Zero
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_GBUFFER
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 TangentSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV : INTERP0;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV : INTERP1;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh : INTERP2;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord : INTERP3;
            #endif
             float4 tangentWS : INTERP4;
             float4 texCoord0 : INTERP5;
             float4 fogFactorAndVertexLight : INTERP6;
             float3 positionWS : INTERP7;
             float3 normalWS : INTERP8;
             float3 viewDirectionWS : INTERP9;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.fogFactorAndVertexLight.xyzw = input.fogFactorAndVertexLight;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            output.viewDirectionWS.xyz = input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.fogFactorAndVertexLight = input.fogFactorAndVertexLight.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            output.viewDirectionWS = input.viewDirectionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void LerpWhiteTo_float(float3 b, float t, out float3 _out){
            half oneMinusT = 1 - t;
            _out = half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0 = _Color;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float4 _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2;
            Unity_Multiply_float4_float4(_Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0, _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0, _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2);
            UnityTexture2D _Property_8989c0284a1046d69bd798a916ddef61_Out_0 = UnityBuildTexture2DStructNoScale(_DetailAlbedoMap);
            float2 _Property_0733227372e0459db0761a6b7c5789c5_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_0733227372e0459db0761a6b7c5789c5_Out_0, float2 (0, 0), _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3);
            float4 _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8989c0284a1046d69bd798a916ddef61_Out_0.tex, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.samplerstate, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.GetTransformedUV(_TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3));
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_R_4 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.r;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_G_5 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.g;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_B_6 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.b;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_A_7 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.a;
            UnityTexture2D _Property_7caac4574f344834b84b0bee767e2ee4_Out_0 = UnityBuildTexture2DStructNoScale(_DetailMask);
            float2 _Property_e60862a15bee495aa98a2c32e7082a19_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_e60862a15bee495aa98a2c32e7082a19_Out_0, float2 (0, 0), _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3);
            float4 _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7caac4574f344834b84b0bee767e2ee4_Out_0.tex, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.samplerstate, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.GetTransformedUV(_TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3));
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_R_4 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.r;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_G_5 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.g;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_B_6 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.b;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.a;
            float3 _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2;
            LerpWhiteTo_float((_SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.xyz), _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7, _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2);
            float3 _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            Unity_Multiply_float3_float3((_Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2.xyz), _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2, _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2);
            UnityTexture2D _Property_9120b0d7949f4988abd0796affa542a4_Out_0 = UnityBuildTexture2DStructNoScale(_BumpMap);
            float2 _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0, float2 (0, 0), _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3);
            float4 _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9120b0d7949f4988abd0796affa542a4_Out_0.tex, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.samplerstate, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.GetTransformedUV(_TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3));
            _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0);
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_R_4 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.r;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_G_5 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.g;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_B_6 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.b;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_A_7 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.a;
            UnityTexture2D _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0 = UnityBuildTexture2DStructNoScale(_DetailNormalMap);
            float2 _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0, float2 (0, 0), _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3);
            float4 _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.tex, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.samplerstate, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.GetTransformedUV(_TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3));
            _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0);
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_R_4 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.r;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_G_5 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.g;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_B_6 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.b;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_A_7 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.a;
            float _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0 = _DetailNormalMapScale;
            float3 _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2;
            Unity_NormalStrength_float((_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.xyz), _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0, _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2);
            float3 _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            Unity_NormalBlend_float((_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.xyz), _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2, _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2);
            UnityTexture2D _Property_e6e5d425f963466dbb073b5ebed88524_Out_0 = UnityBuildTexture2DStructNoScale(_MetallicGlossMap);
            float2 _Property_139a6620cc4d432387db56ea48a30754_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_da978c144900420697b563b2da61024c_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_139a6620cc4d432387db56ea48a30754_Out_0, float2 (0, 0), _TilingAndOffset_da978c144900420697b563b2da61024c_Out_3);
            float4 _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0 = SAMPLE_TEXTURE2D(_Property_e6e5d425f963466dbb073b5ebed88524_Out_0.tex, _Property_e6e5d425f963466dbb073b5ebed88524_Out_0.samplerstate, _Property_e6e5d425f963466dbb073b5ebed88524_Out_0.GetTransformedUV(_TilingAndOffset_da978c144900420697b563b2da61024c_Out_3));
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_R_4 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.r;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_G_5 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.g;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_B_6 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.b;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_A_7 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.a;
            float _Property_5a43ed32f9844383a3fc2e9c22de45a6_Out_0 = _Metallic;
            float _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_R_4, _Property_5a43ed32f9844383a3fc2e9c22de45a6_Out_0, _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2);
            float _Property_4a309e00f2a0447e823f4ede6802e827_Out_0 = _Glossiness;
            float _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_A_7, _Property_4a309e00f2a0447e823f4ede6802e827_Out_0, _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2);
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.BaseColor = _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            surface.NormalTS = _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2;
            surface.Smoothness = _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2;
            surface.Occlusion = 1;
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//GBuffer
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.BaseColor, surface.Emission, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//ShadowCaster
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//DepthOnly
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALS
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 TangentSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float4 texCoord0 : INTERP1;
             float3 positionWS : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 NormalTS;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9120b0d7949f4988abd0796affa542a4_Out_0 = UnityBuildTexture2DStructNoScale(_BumpMap);
            float2 _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0, float2 (0, 0), _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3);
            float4 _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9120b0d7949f4988abd0796affa542a4_Out_0.tex, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.samplerstate, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.GetTransformedUV(_TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3));
            _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0);
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_R_4 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.r;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_G_5 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.g;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_B_6 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.b;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_A_7 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.a;
            UnityTexture2D _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0 = UnityBuildTexture2DStructNoScale(_DetailNormalMap);
            float2 _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0, float2 (0, 0), _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3);
            float4 _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.tex, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.samplerstate, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.GetTransformedUV(_TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3));
            _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0);
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_R_4 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.r;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_G_5 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.g;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_B_6 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.b;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_A_7 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.a;
            float _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0 = _DetailNormalMapScale;
            float3 _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2;
            Unity_NormalStrength_float((_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.xyz), _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0, _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2);
            float3 _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            Unity_NormalBlend_float((_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.xyz), _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2, _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2);
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.NormalTS = _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//DepthNormals
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma shader_feature _ EDITOR_VISUALIZATION
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        #define VARYINGS_NEED_TEXCOORD2
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_META
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 texCoord2 : INTERP2;
             float3 positionWS : INTERP3;
             float3 normalWS : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.texCoord2.xyzw = input.texCoord2;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.texCoord2 = input.texCoord2.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_META_PASS
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void LerpWhiteTo_float(float3 b, float t, out float3 _out){
            half oneMinusT = 1 - t;
            _out = half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0 = _Color;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float4 _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2;
            Unity_Multiply_float4_float4(_Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0, _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0, _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2);
            UnityTexture2D _Property_8989c0284a1046d69bd798a916ddef61_Out_0 = UnityBuildTexture2DStructNoScale(_DetailAlbedoMap);
            float2 _Property_0733227372e0459db0761a6b7c5789c5_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_0733227372e0459db0761a6b7c5789c5_Out_0, float2 (0, 0), _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3);
            float4 _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8989c0284a1046d69bd798a916ddef61_Out_0.tex, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.samplerstate, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.GetTransformedUV(_TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3));
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_R_4 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.r;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_G_5 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.g;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_B_6 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.b;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_A_7 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.a;
            UnityTexture2D _Property_7caac4574f344834b84b0bee767e2ee4_Out_0 = UnityBuildTexture2DStructNoScale(_DetailMask);
            float2 _Property_e60862a15bee495aa98a2c32e7082a19_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_e60862a15bee495aa98a2c32e7082a19_Out_0, float2 (0, 0), _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3);
            float4 _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7caac4574f344834b84b0bee767e2ee4_Out_0.tex, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.samplerstate, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.GetTransformedUV(_TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3));
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_R_4 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.r;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_G_5 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.g;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_B_6 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.b;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.a;
            float3 _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2;
            LerpWhiteTo_float((_SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.xyz), _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7, _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2);
            float3 _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            Unity_Multiply_float3_float3((_Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2.xyz), _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2, _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2);
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.BaseColor = _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//Unknown
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.BaseColor, surface.Emission, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//SceneSelectionPass
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//ScenePickingPass
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            // Name: <None>
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Off
        Blend One Zero
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_2D
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void LerpWhiteTo_float(float3 b, float t, out float3 _out){
            half oneMinusT = 1 - t;
            _out = half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0 = _Color;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float4 _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2;
            Unity_Multiply_float4_float4(_Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0, _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0, _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2);
            UnityTexture2D _Property_8989c0284a1046d69bd798a916ddef61_Out_0 = UnityBuildTexture2DStructNoScale(_DetailAlbedoMap);
            float2 _Property_0733227372e0459db0761a6b7c5789c5_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_0733227372e0459db0761a6b7c5789c5_Out_0, float2 (0, 0), _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3);
            float4 _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8989c0284a1046d69bd798a916ddef61_Out_0.tex, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.samplerstate, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.GetTransformedUV(_TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3));
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_R_4 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.r;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_G_5 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.g;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_B_6 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.b;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_A_7 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.a;
            UnityTexture2D _Property_7caac4574f344834b84b0bee767e2ee4_Out_0 = UnityBuildTexture2DStructNoScale(_DetailMask);
            float2 _Property_e60862a15bee495aa98a2c32e7082a19_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_e60862a15bee495aa98a2c32e7082a19_Out_0, float2 (0, 0), _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3);
            float4 _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7caac4574f344834b84b0bee767e2ee4_Out_0.tex, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.samplerstate, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.GetTransformedUV(_TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3));
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_R_4 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.r;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_G_5 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.g;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_B_6 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.b;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.a;
            float3 _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2;
            LerpWhiteTo_float((_SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.xyz), _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7, _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2);
            float3 _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            Unity_Multiply_float3_float3((_Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2.xyz), _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2, _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2);
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.BaseColor = _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//ScenePickingPass
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.BaseColor, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue"="AlphaTest"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalLitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Off
        Blend One Zero
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _LIGHT_COOKIES
        #pragma multi_compile _ _CLUSTERED_RENDERING
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_FORWARD
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 TangentSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV : INTERP0;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV : INTERP1;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh : INTERP2;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord : INTERP3;
            #endif
             float4 tangentWS : INTERP4;
             float4 texCoord0 : INTERP5;
             float4 fogFactorAndVertexLight : INTERP6;
             float3 positionWS : INTERP7;
             float3 normalWS : INTERP8;
             float3 viewDirectionWS : INTERP9;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.fogFactorAndVertexLight.xyzw = input.fogFactorAndVertexLight;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            output.viewDirectionWS.xyz = input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.shadowCoord;
            #endif
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.fogFactorAndVertexLight = input.fogFactorAndVertexLight.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            output.viewDirectionWS = input.viewDirectionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void LerpWhiteTo_float(float3 b, float t, out float3 _out){
            half oneMinusT = 1 - t;
            _out = half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0 = _Color;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float4 _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2;
            Unity_Multiply_float4_float4(_Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0, _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0, _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2);
            UnityTexture2D _Property_8989c0284a1046d69bd798a916ddef61_Out_0 = UnityBuildTexture2DStructNoScale(_DetailAlbedoMap);
            float2 _Property_0733227372e0459db0761a6b7c5789c5_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_0733227372e0459db0761a6b7c5789c5_Out_0, float2 (0, 0), _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3);
            float4 _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8989c0284a1046d69bd798a916ddef61_Out_0.tex, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.samplerstate, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.GetTransformedUV(_TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3));
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_R_4 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.r;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_G_5 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.g;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_B_6 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.b;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_A_7 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.a;
            UnityTexture2D _Property_7caac4574f344834b84b0bee767e2ee4_Out_0 = UnityBuildTexture2DStructNoScale(_DetailMask);
            float2 _Property_e60862a15bee495aa98a2c32e7082a19_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_e60862a15bee495aa98a2c32e7082a19_Out_0, float2 (0, 0), _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3);
            float4 _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7caac4574f344834b84b0bee767e2ee4_Out_0.tex, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.samplerstate, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.GetTransformedUV(_TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3));
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_R_4 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.r;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_G_5 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.g;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_B_6 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.b;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.a;
            float3 _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2;
            LerpWhiteTo_float((_SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.xyz), _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7, _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2);
            float3 _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            Unity_Multiply_float3_float3((_Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2.xyz), _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2, _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2);
            UnityTexture2D _Property_9120b0d7949f4988abd0796affa542a4_Out_0 = UnityBuildTexture2DStructNoScale(_BumpMap);
            float2 _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0, float2 (0, 0), _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3);
            float4 _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9120b0d7949f4988abd0796affa542a4_Out_0.tex, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.samplerstate, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.GetTransformedUV(_TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3));
            _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0);
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_R_4 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.r;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_G_5 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.g;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_B_6 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.b;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_A_7 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.a;
            UnityTexture2D _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0 = UnityBuildTexture2DStructNoScale(_DetailNormalMap);
            float2 _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0, float2 (0, 0), _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3);
            float4 _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.tex, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.samplerstate, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.GetTransformedUV(_TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3));
            _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0);
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_R_4 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.r;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_G_5 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.g;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_B_6 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.b;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_A_7 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.a;
            float _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0 = _DetailNormalMapScale;
            float3 _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2;
            Unity_NormalStrength_float((_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.xyz), _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0, _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2);
            float3 _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            Unity_NormalBlend_float((_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.xyz), _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2, _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2);
            UnityTexture2D _Property_e6e5d425f963466dbb073b5ebed88524_Out_0 = UnityBuildTexture2DStructNoScale(_MetallicGlossMap);
            float2 _Property_139a6620cc4d432387db56ea48a30754_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_da978c144900420697b563b2da61024c_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_139a6620cc4d432387db56ea48a30754_Out_0, float2 (0, 0), _TilingAndOffset_da978c144900420697b563b2da61024c_Out_3);
            float4 _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0 = SAMPLE_TEXTURE2D(_Property_e6e5d425f963466dbb073b5ebed88524_Out_0.tex, _Property_e6e5d425f963466dbb073b5ebed88524_Out_0.samplerstate, _Property_e6e5d425f963466dbb073b5ebed88524_Out_0.GetTransformedUV(_TilingAndOffset_da978c144900420697b563b2da61024c_Out_3));
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_R_4 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.r;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_G_5 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.g;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_B_6 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.b;
            float _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_A_7 = _SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_RGBA_0.a;
            float _Property_5a43ed32f9844383a3fc2e9c22de45a6_Out_0 = _Metallic;
            float _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_R_4, _Property_5a43ed32f9844383a3fc2e9c22de45a6_Out_0, _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2);
            float _Property_4a309e00f2a0447e823f4ede6802e827_Out_0 = _Glossiness;
            float _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_08cb6b5fdd544d51b8980045ee2c3353_A_7, _Property_4a309e00f2a0447e823f4ede6802e827_Out_0, _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2);
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.BaseColor = _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            surface.NormalTS = _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = _Multiply_daa0a62def004e22ab4ea8dc200966c9_Out_2;
            surface.Smoothness = _Multiply_1fb7857d38544464a128cbe8ade86a54_Out_2;
            surface.Occlusion = 1;
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//UniversalForward
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.BaseColor, surface.Emission, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//ShadowCaster
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//DepthOnly
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALS
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 TangentSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float4 texCoord0 : INTERP1;
             float3 positionWS : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
        {
            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
        }
        
        void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
        {
            Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 NormalTS;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_9120b0d7949f4988abd0796affa542a4_Out_0 = UnityBuildTexture2DStructNoScale(_BumpMap);
            float2 _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_7a4366114c4e4bcf85ca41714779cfe6_Out_0, float2 (0, 0), _TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3);
            float4 _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0 = SAMPLE_TEXTURE2D(_Property_9120b0d7949f4988abd0796affa542a4_Out_0.tex, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.samplerstate, _Property_9120b0d7949f4988abd0796affa542a4_Out_0.GetTransformedUV(_TilingAndOffset_3fbbd059d68a40698d8e3f0af48be787_Out_3));
            _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0);
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_R_4 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.r;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_G_5 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.g;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_B_6 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.b;
            float _SampleTexture2D_14a69f54c73442c59702d5eea9016460_A_7 = _SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.a;
            UnityTexture2D _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0 = UnityBuildTexture2DStructNoScale(_DetailNormalMap);
            float2 _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_a0f365b5b1ae40778312f7fb0f469fff_Out_0, float2 (0, 0), _TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3);
            float4 _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0 = SAMPLE_TEXTURE2D(_Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.tex, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.samplerstate, _Property_bd38e220fcf54391a1b907a1fbfdd6ed_Out_0.GetTransformedUV(_TilingAndOffset_18713259b8734932823bf35d518f24b8_Out_3));
            _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0);
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_R_4 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.r;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_G_5 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.g;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_B_6 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.b;
            float _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_A_7 = _SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.a;
            float _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0 = _DetailNormalMapScale;
            float3 _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2;
            Unity_NormalStrength_float((_SampleTexture2D_f3a387f9970c48d1beea89f9eae1564e_RGBA_0.xyz), _Property_1c5f4766ff9242908aecb9a7e58b05f3_Out_0, _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2);
            float3 _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            Unity_NormalBlend_float((_SampleTexture2D_14a69f54c73442c59702d5eea9016460_RGBA_0.xyz), _NormalStrength_a393d5a6fbc948b9b9ba7d4decd5f2e9_Out_2, _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2);
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.NormalTS = _NormalBlend_b19589c525c040a4b691de6ea369905f_Out_2;
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//DepthNormals
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma shader_feature _ EDITOR_VISUALIZATION
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        #define VARYINGS_NEED_TEXCOORD2
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_META
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 texCoord2 : INTERP2;
             float3 positionWS : INTERP3;
             float3 normalWS : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.texCoord2.xyzw = input.texCoord2;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.texCoord2 = input.texCoord2.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_META_PASS
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void LerpWhiteTo_float(float3 b, float t, out float3 _out){
            half oneMinusT = 1 - t;
            _out = half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0 = _Color;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float4 _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2;
            Unity_Multiply_float4_float4(_Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0, _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0, _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2);
            UnityTexture2D _Property_8989c0284a1046d69bd798a916ddef61_Out_0 = UnityBuildTexture2DStructNoScale(_DetailAlbedoMap);
            float2 _Property_0733227372e0459db0761a6b7c5789c5_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_0733227372e0459db0761a6b7c5789c5_Out_0, float2 (0, 0), _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3);
            float4 _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8989c0284a1046d69bd798a916ddef61_Out_0.tex, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.samplerstate, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.GetTransformedUV(_TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3));
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_R_4 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.r;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_G_5 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.g;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_B_6 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.b;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_A_7 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.a;
            UnityTexture2D _Property_7caac4574f344834b84b0bee767e2ee4_Out_0 = UnityBuildTexture2DStructNoScale(_DetailMask);
            float2 _Property_e60862a15bee495aa98a2c32e7082a19_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_e60862a15bee495aa98a2c32e7082a19_Out_0, float2 (0, 0), _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3);
            float4 _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7caac4574f344834b84b0bee767e2ee4_Out_0.tex, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.samplerstate, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.GetTransformedUV(_TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3));
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_R_4 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.r;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_G_5 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.g;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_B_6 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.b;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.a;
            float3 _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2;
            LerpWhiteTo_float((_SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.xyz), _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7, _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2);
            float3 _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            Unity_Multiply_float3_float3((_Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2.xyz), _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2, _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2);
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.BaseColor = _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//Unknown
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.BaseColor, surface.Emission, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//SceneSelectionPass
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//ScenePickingPass
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            // Name: <None>
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Off
        Blend One Zero
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_2D
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 WorldSpaceNormal;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties


//Advanced Dissolve Keywords Start///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#pragma shader_feature_local   _AD_STATE_ENABLED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_BASE_ALPHA				  _AD_CUTOUT_STANDARD_SOURCE_CUSTOM_MAP                     _AD_CUTOUT_STANDARD_SOURCE_TWO_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_THREE_CUSTOM_MAPS _AD_CUTOUT_STANDARD_SOURCE_USER_DEFINED
#pragma shader_feature_local _ _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_TRIPLANAR _AD_CUTOUT_STANDARD_SOURCE_MAPS_MAPPING_TYPE_SCREEN_SPACE
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_TYPE_XYZ						  _AD_CUTOUT_GEOMETRIC_TYPE_PLANE                           _AD_CUTOUT_GEOMETRIC_TYPE_SPHERE           _AD_CUTOUT_GEOMETRIC_TYPE_CUBE               _AD_CUTOUT_GEOMETRIC_TYPE_CAPSULE       _AD_CUTOUT_GEOMETRIC_TYPE_CONE_SMOOTH
#pragma shader_feature_local _ _AD_CUTOUT_GEOMETRIC_COUNT_TWO					      _AD_CUTOUT_GEOMETRIC_COUNT_THREE                          _AD_CUTOUT_GEOMETRIC_COUNT_FOUR
#pragma shader_feature_local _ _AD_EDGE_BASE_SOURCE_CUTOUT_STANDARD                   _AD_EDGE_BASE_SOURCE_CUTOUT_GEOMETRIC                     _AD_EDGE_BASE_SOURCE_ALL
#pragma shader_feature_local _ _AD_EDGE_ADDITIONAL_COLOR_BASE_COLOR                   _AD_EDGE_ADDITIONAL_COLOR_CUSTOM_MAP                      _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_MAP     _AD_EDGE_ADDITIONAL_COLOR_GRADIENT_COLOR     _AD_EDGE_ADDITIONAL_COLOR_USER_DEFINED
#pragma shader_feature_local _ _AD_GLOBAL_CONTROL_ID_ONE                              _AD_GLOBAL_CONTROL_ID_TWO                                 _AD_GLOBAL_CONTROL_ID_THREE                _AD_GLOBAL_CONTROL_ID_FOUR
//Advanced Dissolve Keywords End/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


#define ADVANCED_DISSOLVE_SHADER_GRAPH
#define ADVANCED_DISSOLVE_UNIVERSAL_RENDER_PIPELINE
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Defines.cginc"
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _Cutout;
        float4 _MainTex_TexelSize;
        float2 _AlbedoTiling;
        float _Metallic;
        float4 _MetallicGlossMap_TexelSize;
        float _Glossiness;
        float4 _BumpMap_TexelSize;
        float4 _DetailMask_TexelSize;
        float2 _DetailTiling;
        float4 _DetailAlbedoMap_TexelSize;
        float _DetailNormalMapScale;
        float4 _DetailNormalMap_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MetallicGlossMap);
        SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_BumpMap);
        SAMPLER(sampler_BumpMap);
        TEXTURE2D(_DetailMask);
        SAMPLER(sampler_DetailMask);
        TEXTURE2D(_DetailAlbedoMap);
        SAMPLER(sampler_DetailAlbedoMap);
        TEXTURE2D(_DetailNormalMap);
        SAMPLER(sampler_DetailNormalMap);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void LerpWhiteTo_float(float3 b, float t, out float3 _out){
            half oneMinusT = 1 - t;
            _out = half3(oneMinusT, oneMinusT, oneMinusT) + b * t;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void AdvancedDissolveShaderGraphFunction_float(float2 UV, float3 PositionOS, float3 PositionWS, float3 PositionWS_Absolut, float3 NormalOS, float3 NormalWS, float Custom_Cutout, float4 Custom_Color, out float Value){
        Value = 0;
        }
        
        struct Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float
        {
        float3 ObjectSpaceNormal;
        float3 WorldSpaceNormal;
        float3 ObjectSpacePosition;
        float3 WorldSpacePosition;
        float3 AbsoluteWorldSpacePosition;
        half4 uv0;
        };
        
        void SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(float Vector1_9E44E7D0, float4 Color_d37717e22d9845eeb5507ed0b661e197, Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float IN, out float Out_3)
        {
        float4 _UV_0af11090dff4968fbefbff780ab3f959_Out_0 = IN.uv0;
        float _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0 = Vector1_9E44E7D0;
        float4 _Property_6d35f866e3e7457cb788755ca206532e_Out_0 = Color_d37717e22d9845eeb5507ed0b661e197;
        float _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        AdvancedDissolveShaderGraphFunction_float((_UV_0af11090dff4968fbefbff780ab3f959_Out_0.xy), IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, _Property_2254a3efc4fcf082bc34b2ce5b131975_Out_0, _Property_6d35f866e3e7457cb788755ca206532e_Out_0, _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7);
        Out_3 = _AdvancedDissolveShaderGraphFunctionCustomFunction_18f0160f9996fe8f938c567e2ad92b60_Value_7;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        


//Advanced Dissolve
#include "Assets/RIS/LectureMaterial/ARMediaWorks/Robot/ThirdParty/Advanced Dissolve/Shaders/cginc/Core.cginc"


        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0 = _Color;
            UnityTexture2D _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_99f0e7fdf1fa442f99fc1499a93dee62_Out_0, float2 (0, 0), _TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3);
            float4 _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0 = SAMPLE_TEXTURE2D(_Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.tex, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.samplerstate, _Property_cd87f479e89d4e5eb0759a5e69e06d73_Out_0.GetTransformedUV(_TilingAndOffset_1c385608a51d48e79fe602b6be109c09_Out_3));
            float _SampleTexture2D_672f3876748c4100994d418733456305_R_4 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.r;
            float _SampleTexture2D_672f3876748c4100994d418733456305_G_5 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.g;
            float _SampleTexture2D_672f3876748c4100994d418733456305_B_6 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.b;
            float _SampleTexture2D_672f3876748c4100994d418733456305_A_7 = _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0.a;
            float4 _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2;
            Unity_Multiply_float4_float4(_Property_4e1128bf0e0e47ddbc76f01375e98cdd_Out_0, _SampleTexture2D_672f3876748c4100994d418733456305_RGBA_0, _Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2);
            UnityTexture2D _Property_8989c0284a1046d69bd798a916ddef61_Out_0 = UnityBuildTexture2DStructNoScale(_DetailAlbedoMap);
            float2 _Property_0733227372e0459db0761a6b7c5789c5_Out_0 = _DetailTiling;
            float2 _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_0733227372e0459db0761a6b7c5789c5_Out_0, float2 (0, 0), _TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3);
            float4 _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8989c0284a1046d69bd798a916ddef61_Out_0.tex, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.samplerstate, _Property_8989c0284a1046d69bd798a916ddef61_Out_0.GetTransformedUV(_TilingAndOffset_a20573f5cd484f1393e6dafe58cf2771_Out_3));
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_R_4 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.r;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_G_5 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.g;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_B_6 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.b;
            float _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_A_7 = _SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.a;
            UnityTexture2D _Property_7caac4574f344834b84b0bee767e2ee4_Out_0 = UnityBuildTexture2DStructNoScale(_DetailMask);
            float2 _Property_e60862a15bee495aa98a2c32e7082a19_Out_0 = _AlbedoTiling;
            float2 _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_e60862a15bee495aa98a2c32e7082a19_Out_0, float2 (0, 0), _TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3);
            float4 _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7caac4574f344834b84b0bee767e2ee4_Out_0.tex, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.samplerstate, _Property_7caac4574f344834b84b0bee767e2ee4_Out_0.GetTransformedUV(_TilingAndOffset_e57b27aa684e420aa89271f42530f433_Out_3));
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_R_4 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.r;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_G_5 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.g;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_B_6 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.b;
            float _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7 = _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_RGBA_0.a;
            float3 _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2;
            LerpWhiteTo_float((_SampleTexture2D_864f40cb0cb74eff85fccb1a0066eb93_RGBA_0.xyz), _SampleTexture2D_54fc1536ce0d487b94143e3a0ee447a8_A_7, _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2);
            float3 _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            Unity_Multiply_float3_float3((_Multiply_934ec43e5128405aafaccb1b6b7d3c0f_Out_2.xyz), _LerpWhiteToCustomFunction_c04c9e26af0646fbbdf7a407d55aaa2c_out_2, _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2);
            float _Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0 = _Cutout;
            Bindings_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpaceNormal = IN.ObjectSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpaceNormal = IN.WorldSpaceNormal;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.ObjectSpacePosition = IN.ObjectSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.WorldSpacePosition = IN.WorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.AbsoluteWorldSpacePosition = IN.AbsoluteWorldSpacePosition;
            _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44.uv0 = IN.uv0;
            float _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3;
            SG_AdvancedDissolve_58cc1ed7edc36664e85cbe55fd29d527_float(0, float4 (0, 0, 0, 1), _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3);
            float _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;
            Unity_Add_float(_Property_926ddd6aacc8433a9d75b38c6f2fc21e_Out_0, _AdvancedDissolve_a4c9700cc3ca4c8fada2a16fd9aa7b44_Out_3, _Add_a76b0466580542a6b2afbb447ac438e9_Out_2);
            surface.BaseColor = _Multiply_eb45cfc72fa04bdc81210b4f561d9ab0_Out_2;
            surface.Alpha = _SampleTexture2D_672f3876748c4100994d418733456305_A_7;
            surface.AlphaClipThreshold = _Add_a76b0466580542a6b2afbb447ac438e9_Out_2;


//ScenePickingPass
AdvancedDissolveShaderGraph(IN.uv0.xy, IN.ObjectSpacePosition, IN.WorldSpacePosition, IN.AbsoluteWorldSpacePosition, IN.ObjectSpaceNormal, IN.WorldSpaceNormal, 0, 1, surface.BaseColor, surface.Alpha, surface.AlphaClipThreshold);


return surface;

        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
            output.ObjectSpaceNormal = normalize(mul(output.WorldSpaceNormal, (float3x3) UNITY_MATRIX_M));           // transposed multiplication by inverse matrix to handle normal scale
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ObjectSpacePosition = TransformWorldToObject(input.positionWS);
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
//    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphLitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
CustomEditorForRenderPipeline "UnityEditor.AdvancedDissolve_ShaderGraphLitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}
