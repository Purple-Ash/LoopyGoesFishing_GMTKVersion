Shader "Custom/TEST"
{
    Properties
    {
        _OCTAVE("Octave", Int) = 6
        _MulScale("Mul Scale", Float) = 5.0
        _Height("Height", Float) = 0.6
        _Tide("Tide", Float) = 0.1
        _FoamThickness("Foam Thickness", Float) = 0.1
        _TimeScale("Time Scale", Float) = 1.0
        _WaterDeep("Water Deep", Float) = 0.3
        _WaterColor("Water Color", Color) = (0.04, 0.38, 0.88, 1.0)
        _Water2Color("Water 2 Color", Color) = (0.04, 0.35, 0.78, 1.0)
        _FoamColor("Foam Color", Color) = (0.8125, 0.9609, 0.9648, 1.0)
        [HideInInspector]_MainTex("Sprite Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            int _OCTAVE;
            float _MulScale;
            float _Height;
            float _Tide;
            float _FoamThickness;
            float _TimeScale;
            float _WaterDeep;
            float4 _WaterColor;
            float4 _Water2Color;
            float4 _FoamColor;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float rand(float2 input)
            {
                return frac(sin(dot(input, float2(23.53, 44.0))) * 42350.45);
            }

            float perlin(float2 input)
            {
                float2 i = floor(input);
                float2 j = frac(input);
                float2 coord = smoothstep(0.0, 1.0, j);

                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));

                return lerp(lerp(a, b, coord.x), lerp(c, d, coord.x), coord.y);
            }

            float fbm(float2 input)
            {
                float value = 0.0;
                float scale = 0.5;
                for (int i = 0; i < _OCTAVE; i++)
                {
                    value += perlin(input) * scale;
                    input *= 2.0;
                    scale *= 0.5;
                }
                return value;
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.y * _TimeScale;
                float2 uv = i.uv;

                float fbmval = fbm(float2(uv.x * _MulScale + 0.2 * sin(0.3 * t) + 0.15 * t, -0.05 * t + uv.y * _MulScale + 0.1 * cos(0.68 * t)));
                float fbmvalshadow = fbm(float2(uv.x * _MulScale + 0.2 * sin(-0.6 * t + 25.0 * uv.y) + 0.15 * t + 3.0,
                                         -0.05 * t + uv.y * _MulScale + 0.13 * cos(-0.68 * t)) - 7.0 + 0.1 * sin(0.43 * t));

                float myheight = _Height + _Tide * sin(t + 5.0 * uv.x - 8.0 * uv.y);
                float shadowheight = _Height + _Tide * 1.3 * cos(t + 2.0 * uv.x - 2.0 * uv.y);
                float withinFoam = step(myheight, fbmval) * step(fbmval, myheight + _FoamThickness);
                float shadow = (1.0 - withinFoam) * step(shadowheight, fbmvalshadow) * step(fbmvalshadow, shadowheight + _FoamThickness * 0.7);

                float4 col = withinFoam * _FoamColor + shadow * _Water2Color + ((1.0 - withinFoam) * (1.0 - shadow)) * _WaterColor;
                return col;
            }
            ENDCG
        }
    }
}