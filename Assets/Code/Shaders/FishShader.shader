// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/FishShader"
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
                UNITY_FOG_COORDS(1)
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

            v2f vert (appdata v)
            {
                v2f o;
                //get object direction
                float3 waveDir = normalize(mul((float3x3)unity_ObjectToWorld, float3(0,1,0)));

                o.vertex = UnityObjectToClipPos(transformVertex(v.vertex, waveDir));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o; 
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDCG
        }
    }
}
