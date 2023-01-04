// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "shader3d/simpleborder" {

Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Outline("Outline",Range(0.1,0.0001)) = 0.1
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
        LOD 100
        Pass
        {
            Cull Front
            offset 1,1
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
            };
            fixed _Outline;
            float4 _OutlineColor;
            
            v2f vert (appdata_base v)
            {
                v2f o;
                float3 dir = normalize(v.vertex.xyz);
                float4 newPos = v.vertex;
                newPos.xyz += dir * _Outline;
                o.vertex = UnityObjectToClipPos(newPos);
                
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
                fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT;
                ambient.xyz *= ambient.a * 0.1;
                color.rgb = color.rgb * diff + specColor + ambient; 
                
                return color;

                
                // 漫反射实现
                // diff是由顶点的法线世界向量与光照向量点乘获得
                // 如果小于0，说明与光照的夹角大于180度，及该顶点面朝方向与光照相背，及该点为阴影处
                // 相反则处于光照下
                // float diff = dot(nor,light);
                // float para = diff > _LightBorder ? _FrontCoe : _BackCoe;
                // // 获取环境光
                // color.rgb += UNITY_LIGHTMODEL_AMBIENT.xyz * color.rgb;
                // // rgb值强制归一 
                // color.rgb = saturate(color.rgb);
                // // 乘以前后渲染的颜色系数，实现卡通渲染
                // color.rgb *= para;
                // return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"  
}