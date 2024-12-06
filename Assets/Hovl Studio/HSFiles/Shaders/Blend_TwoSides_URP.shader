
Shader "URP/Particles/Blend_TwoSides"
{
    Properties
    {
        _Cutoff ("Mask Clip Value", Range(0,1)) = 0.5
        _MainTex ("Main Texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _SpeedMainTexUVNoiseZW ("Speed MainTex U/V + Noise Z/W", Vector) = (0, 0, 0, 0)
        _Emission ("Emission", Float) = 2
        _UseFresnel ("Use Fresnel?", Float) = 1
        _Usesmoothcorners ("Use Smooth Corners?", Float) = 0
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" "IgnoreProjector"="True" }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_Mask);
            SAMPLER(sampler_Mask);
            TEXTURE2D(_Noise);
            SAMPLER(sampler_Noise);

            float _Cutoff;
            float _Emission;
            float _UseFresnel;
            float _Usesmoothcorners;
            float4 _SpeedMainTexUVNoiseZW;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 mask = SAMPLE_TEXTURE2D(_Mask, sampler_Mask, i.uv);
                float4 noise = SAMPLE_TEXTURE2D(_Noise, sampler_Noise, i.uv);
                
                float alpha = mainTex.a * mask.r;
                if (alpha < _Cutoff)
                    discard;

                float emission = _Emission * noise.r;
                return float4(mainTex.rgb * emission, alpha);
            }
            ENDHLSL
        }
    }
}
