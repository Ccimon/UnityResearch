Shader "shader3d/borderline" {

Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Outline("Outline",float) = 0.1
        _OutlineColor("OutlineColor",Color) = (0,0,0,1)
        _Metallic("Metallic",float) = 0.5
        _Glossiness("Glossiness",float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        //描边阶段，法线外扩，渲染背面
        Pass
        {
            //只需要边缘外扩
            Cull Front
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            float _Outline;
            float4 _OutlineColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //把法线转换到视图空间
                float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV,v.normal);
                //把法线转换到投影空间
                float2 pnormal_xy = mul((float2x2)UNITY_MATRIX_P,vnormal.xy);
                //朝法线方向外扩
                o.vertex.xy = o.vertex.xy + pnormal_xy * _Outline;
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

            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members pos)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            float _Metallic;
            float _Glossiness;
            half2 MetallicGloss(float2 uv)
            {
               half2 mg;
               #ifdef _METALLICGLOSSMAP
                    mg = tex2D(_MetallicGlossMap,uv.xy).ra;
               #else
                   mg = half2(_Metallic,_Glossiness);  
               #endif
                  return mg;

            }
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv :TEXCOORD0;
                // float2 normal;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv:TEXCOORD0;
                // float2 pos;
                // float2 shphereUV;
            };
            
            sampler2D _MainTex;     
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                // half3 viewNormal = mul((float3x3)UNITY_MATRIX_MV,v.normal);
                // half4 viewPos = mul(UNITY_MATRIX_MV,v.vertex); 
                // half3 projPos = normalize(viewPos.xyz/viewPos.w);
                // half3 reflectVar = reflect(projPos,viewNormal);
                // half m = 2.0 * sqrt(reflectVar.x * reflectVar.x + reflectVar.y*reflectVar.y + (reflectVar.x+1)*(reflectVar.z + 1));
                // o.shphereUV = fixed2(reflectVar.x/m + 0.5, reflectVar.y/m + 0.5);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                
                return tex2D(_MainTex,i.uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"  
}