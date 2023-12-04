// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Surf_ZtestG"
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
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        //正常阶段
        Pass
        {
            Zwrite On
            Cull off
            CGPROGRAM
            // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members pos)
            #pragma exclude_renderers d3d11
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
                fixed4 color = tex2D(_MainTex,i.uv);
                fixed3 nor = i.normal;
                fixed3 light = normalize(_WorldSpaceLightPos0.xyz);
                
                fixed3 cameraPos = _WorldSpaceCameraPos.xyz;
                fixed3 faceDir = normalize(cameraPos - i.pos);
                fixed3 hightLight = normalize(faceDir + light);
                
                float specular = max(0,dot(hightLight,nor));
                fixed3 specColor = saturate(pow(specular,_Specluar)) * fixed3(1,1,1);
                
                float diff = saturate(dot(nor,light));
                fixed3 ambient = _AmbientFactor * UNITY_LIGHTMODEL_AMBIENT.xyz * color;
                color.rgb = color.rgb * diff + specColor + ambient; 

                return color;

            }
            ENDCG
        }
    }
}
