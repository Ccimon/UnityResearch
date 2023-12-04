Shader "ApplicationTest/Shader_depthTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraDepthNormalsTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //采样法线与深度
                fixed4 info = tex2D(_CameraDepthNormalsTexture,i.uv);
                fixed depth = 0;
                fixed3 normal;
                DecodeDepthNormal(info,depth,normal);

                // 渲染法线
                normalize(normal);
                
                normal.rgb *= saturate(depth * 100);
                return float4(normal,1);

                // 渲染深度
                // col.rgb = col.w * 50;
                return float4(normal,1);
            }
            ENDCG
        }
    }
}
