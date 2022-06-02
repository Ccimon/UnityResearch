Shader "shader2d/ztest" {
 Properties {
        _MainTex ("Main Tex", 2D) = "white" {}
        _OcclusionColor("OcclusionColor",Color)=(0.5,1,0.5,1)
    }
 SubShader 
    {
 Tags { "RenderType"="TransParent" }
 LOD 200
 
 //先判断Greater，因此此Pass必须放在第一个
 Pass
        {
            ZTest LEqual
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert 
            #pragma fragment frag
             fixed4 _OcclusionColor;
            
            float4 vert(float4 v : POSITION) : SV_POSITION {
                 return UnityObjectToClipPos(v);
            }

            fixed4 frag() : SV_Target
            {
                 return _OcclusionColor;
            }
            ENDCG
        }
 
 
 Pass
        {
            ZTest Greater
            Tags { "LightMode"="ForwardBase" }
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"
            sampler2D _MainTex;
            float4 _MainTex_ST;
            struct a2v {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            v2f vert(a2v v) {
                v2f o; 
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); 
                return o;
            }
 
             fixed4 frag(v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.uv);
                c.rgb *= c.a;
                if(c.a < 0.05){
                    discard;
                }
                return fixed4(c.rgb, 0.0);
            }
 
            ENDCG
        }
    }
 FallBack "Diffuse"
}