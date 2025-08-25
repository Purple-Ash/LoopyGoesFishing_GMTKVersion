// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/RainbowFishShader"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Source Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Destination Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Oop("Operation", Float) = 0

        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Wave("Wave", Float) = 1.0 
        _Density("Density", Float) = 1.0
        _Frequency("Frequency", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } 
        LOD 100
        
        BlendOp [_Oop]
        Blend [_SrcFactor] [_DstFactor]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Wave;
            float _Density;
            float _Frequency;

            float4 transformVertex(float4 vertex, float3 waveDir){
                vertex.y += waveDir * sin(vertex.x*_Density + _Time.y*_Frequency )*_Wave;
                return vertex;
            }

            //rainbow cycle, found on the internet
            float3 hsv2rgb(float3 c) {
                float4 K = float4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            //the same for this
            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0.0, -1.0/3.0, 2.0/3.0, -1.0);
                float4 p = (c.g < c.b) ? float4(c.bg, K.wz) : float4(c.gb, K.xy);
                float4 q = (c.r < p.x) ? float4(p.xyw, c.r) : float4(c.r, p.yzx);

                float d = q.x - min(q.w, q.y);
                float e = 1e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            v2f vert (appdata v)
            {
                v2f o;
                //get object direction
                float3 waveDir = normalize(mul((float3x3)unity_ObjectToWorld, float3(0,1,0)));

                o.vertex = UnityObjectToClipPos(transformVertex(v.vertex, waveDir));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o; 
            } 

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                float t = frac(_Time.y * 0.1 + i.worldPos.x + i.worldPos.y); // speed
                float3 rainbow = rgb2hsv(col.rgb);
                rainbow.x = t; // hue from time
                float3 rainbowRGB = hsv2rgb(rainbow);

                col.rgb = lerp(col.rgb, rainbowRGB, 0.8);

                return saturate(col);
            } 
            ENDCG
        }
    } 
}
