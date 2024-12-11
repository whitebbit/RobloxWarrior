// Converted Shader for Unity URP
Shader "Hovl/Particles/Blend_TwoSides"
{
    Properties
    {
        _Cutoff("Mask Clip Value", Float) = 0.5
        _MainTex("Main Texture", 2D) = "white" {}
        _Mask("Mask", 2D) = "white" {}
        _Noise("Noise", 2D) = "white" {}
        _SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)
        _Emission("Emission", Float) = 2
        [Toggle]_UseFresnel("Use Fresnel?", Float) = 1
        [Toggle]_FlipBackside("Flip Backside?", Float) = 0
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _USEFRESNEL_ON
            #pragma multi_compile _ _FLIPBACKSIDE_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_Mask);
            SAMPLER(sampler_Mask);
            TEXTURE2D(_Noise);
            SAMPLER(sampler_Noise);
            float4 _MainTex_ST;
            float4 _SpeedMainTexUVNoiseZW;
            float _Emission;
            float _UseFresnel;
            float _FlipBackside;

            Varyings vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                float3 positionWS = TransformObjectToWorld(v.positionOS);
                o.positionHCS = TransformWorldToHClip(positionWS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.viewDirWS = GetCameraPositionWS() - positionWS;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                // Sample textures
                half4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                half mask = SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv).r;
                half noise = SAMPLE_TEXTURE2D(_Noise, sampler_Noise, i.uv).r;

                // Calculate Fresnel effect if enabled
                half fresnel = _UseFresnel ? pow(1.0 - saturate(dot(normalize(i.viewDirWS), normalize(i.normalWS))), 5.0) : 1.0;

                // Blend based on mask and noise
                half alpha = saturate(mainTex.a * mask * noise * fresnel);
                if (_FlipBackside && dot(i.normalWS, normalize(i.viewDirWS)) < 0.0)
                {
                    alpha = 1.0 - alpha;
                }

                // Final color with emission
                half4 color = half4(mainTex.rgb * _Emission, alpha);
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Unlit/Texture"
}
