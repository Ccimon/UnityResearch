// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "shader3d/cartoon" {

Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NormalTex("NormalMap", 2D) = "white" {}
        _MetalicTex("MetalicTexture",2D) = "white"{}
        _Outline("Outline",Range(0.001,0.1)) = 0.1
        _OutlineColor("OutlineColor",Color) = (0,0,0,1)
        _SpecularColor("Specular",Color) = (1,1,1,1)
        _LambertValue("LamberValue",Range(0,1)) = 0.5
        _Glossiness("Gloss",Range(1,10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        //描边阶段，法线外扩，渲染背面
        Pass
        {
            //只需要边缘外扩
            Cull Front
            
            CGPROGRAM
            #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                // UNITY_FOG_COORDS(1)
            };
            fixed _Outline;
            float4 _OutlineColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
                normal.x *= UNITY_MATRIX_P[0][0];
                normal.y *= UNITY_MATRIX_P[1][1];
                o.vertex.xy += normal.xy * _Outline;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
        //正常阶段
        Pass
        {
            blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZTest LEqual

            CGPROGRAM
            // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members pos)
            #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"


            
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
            float _Metallic;
            float _Glossiness;
            float _LightBorder;
            sampler2D _MainTex;
            sampler2D _MetalicTex;
            sampler2D _NormalTex;
            float _LambertValue;
            float4 _MainTex_ST;
            float4 _SpecularColor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                // 法线从模型空间转换到世界空间
                o.normal = UnityObjectToWorldNormal(v.normal);
                // 坐标也从模型空间转到世界空间
                o.pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex,i.uv);
                fixed4 metalic = tex2D(_MetalicTex,i.uv);
                float3 normal = tex2D(_NormalTex,i.uv);
                float3 light = normalize(_WorldSpaceLightPos0);
                float3 view = normalize(_WorldSpaceCameraPos.xyz - i.pos);
                float highLight = saturate(dot(light,normal));
                float3 diffuse = highLight * color.rgb * _LightColor0.rgb;

                float3 reflectDir = normalize(reflect(light,normal));
                float3 specular = _LightColor0.rgb * _SpecularColor.rgb * pow(max(0,dot(reflectDir,view)),_Glossiness);
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                float4 result = float4(ambient + specular + diffuse,1) * color.rgba;
                return result;
            }
            ENDCG
        }
    }
}