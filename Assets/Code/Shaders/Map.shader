Shader "Unlit/Glitch"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Source Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Destination Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Oop("Operation", Float) = 0

        _MainTex ("Sprite Texture", 2D) = "white" {}
        _GlitchStrength ("Glitch Strength", Float) = 0.05
        _Speed ("Speed", Float) = 1.0
        _Transparency ("Transparency", Range(0, 1)) = 1.0
        _Outlines("Outlines", Range(0,10)) = 1.0
        _Color("Color", Color) = (1,1,1,1)
        _Pixelise("Pixelise" , float) = 36
        _Strength("Strength", Range(0, 1)) = 0.95
        _PixelSpeed("Speed", float) = 200
        [Toggle(Grayscale)] 
        _Grayscale("Grayscale", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        BlendOp [_Oop]
        Blend [_SrcFactor] [_DstFactor]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GlitchStrength;
            float _Speed;
            float _Transparency;
            float _Outlines;
            fixed4 _Color;
            float _Pixelise;
            float _Strength;
            float _PixelSpeed;
            float _Grayscale;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float rand(in float2 uv)
            {
                float2 noise = (frac(sin(dot(uv ,float2(12.9898,78.233)*2.0)) * 43758.5453));
                return abs(noise.x + noise.y) * 0.5;
            }

            float2 glitchOffset(float2 uv)
            {
                float timeShift = floor(_Time.y * _Speed); 
                float yline = floor(uv.y * 40.0);
                float shift = rand(float2(yline, timeShift)) * 2.0 - 1.0;
                uv.x += shift * _GlitchStrength;
                return uv;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = glitchOffset(i.uv);
                fixed4 col = tex2D(_MainTex, uv);
                
                if(col.a < 0) col.a = 0;
                if(fmod(uv.y*10+rand(floor((uv+floor(_Time.x*_PixelSpeed))*_Pixelise)/_Pixelise),1) - 0.5 < 0){
                    col.rgb *= _Strength;
                }

                if(_Grayscale > 0){
                    float3 grayscale = dot(col.rgb, float3(0.299, 0.587, 0.114));
                    col.rgb = grayscale.xxx;
                }


                return col * _Color;
            }
            ENDCG
        }
    }
}