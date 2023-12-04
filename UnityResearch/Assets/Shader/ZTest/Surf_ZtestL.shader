Shader "Custom/Surf_ZtestL"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Outline("Outline",float) = 0.1
        _OutlineColor("OutlineColor",Color) = (0,0,0,1)
        _Specluar("Specular",Range(1,32)) = 1
        _Metallic("Metallic",float) = 0.5
        _Glossiness("Glossiness",float) = 0.5
        _FrontCoe("FrontCoefficient",float) = 1
        _BackCoe("BackCoefficient",Range(0,1)) = 0.75
        _AmbientFactor("AmbientFactor",Range(0,1)) = 0.5
        _LightBorder("LightBorder",float) = 0.0
        _Color("MainColor",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //正常阶段
        Pass
        {
            Zwrite On
            ZTest Greater
            
            Cull back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            int _Specluar;
            float _Metallic;
            float _Glossiness;
            float _LightBorder;
            float _FrontCoe;
            float _BackCoe;
            float _AmbientFactor;
            float _Color;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv :TEXCOORD0;
                fixed3 normal:NORMAL;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv:TEXCOORD0;
                fixed3 normal:NORMAL;
                float3 pos:TEXCOORD1;
            };
            
            sampler2D _MainTex;     
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // 法线从模型空间转换到世界空间
                o.normal = UnityObjectToWorldNormal(v.normal);
                // 坐标也从模型空间转到世界空间
                o.pos = mul(unity_ObjectToWorld, v.vertex).xyz;

                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
               
                return float4(1,0,0,1);
            }
            ENDCG
        }
        Pass
        {
            Zwrite On
            ZTest Less
            
            Cull back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            int _Specluar;
            float _Metallic;
            float _Glossiness;
            float _LightBorder;
            float _FrontCoe;
            float _BackCoe;
            float _AmbientFactor;
            float _Color;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv :TEXCOORD0;
                fixed3 normal:NORMAL;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv:TEXCOORD0;
                fixed3 normal:NORMAL;
                float3 pos:TEXCOORD1;
            };
            
            sampler2D _MainTex;     
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // 法线从模型空间转换到世界空间
                o.normal = UnityObjectToWorldNormal(v.normal);
                // 坐标也从模型空间转到世界空间
                o.pos = mul(unity_ObjectToWorld, v.vertex).xyz;

                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
               
                return _Color;
            }
            ENDCG
        }
    }
}
