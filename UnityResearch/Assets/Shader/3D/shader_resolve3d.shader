// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "shader3d/resolve3d" {

Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ResolveTex ("ResolveTexture", 2D) = "white" {}
        _ResolveFactor ("ResolveProgress",Range(1,0)) = 0.0
        _AmbientFactor ("AmbientFactor",Range(1,0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" "IgnoreProject"="True"}
        Pass
        {
            Tags{"LightMode"="ForwardBase"}
            Zwrite off
            blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            
            CGPROGRAM
            // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members pos)
            #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            fixed _AmbientFactor;
            fixed _ResolveFactor;
            float4 _MainTex_ST;
            sampler2D _MainTex;     
            sampler2D _ResolveTex;
            
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
                SHADOW_COORDS(2)
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // 法线从模型空间转换到世界空间
                o.normal = UnityObjectToWorldNormal(v.normal);
                // 坐标也从模型空间转到世界空间
                o.pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                TRANSFER_SHADOW(o)

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex,i.uv);
                fixed4 resoColor = tex2D(_ResolveTex,i.uv);
                fixed3 nor = i.normal;
                fixed3 light = normalize(_WorldSpaceLightPos0.xyz);
                
                fixed alphaFactor = resoColor.r * 0.33 + resoColor.g * 0.34 + resoColor.b * 0.33;
                clip(alphaFactor - _ResolveFactor);
                
                float diff = saturate(dot(nor,light));
                fixed3 ambient = _AmbientFactor * UNITY_LIGHTMODEL_AMBIENT.xyz * color.a;
                
                UNITY_LIGHT_ATTENUATION(atten,i,i.pos)
                color.rgb = color.rgb * diff * atten + ambient;
                return color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/VertexLit"  
}