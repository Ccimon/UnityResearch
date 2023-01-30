Shader "Uncomplete/Shader_Stencil"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineWidth("OutlineWidth", Range(0,0.1)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Stencil
        {
            ref 1
            Comp Greater
        }
        
         Pass
        {
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(0,0,1,1);
            }
            ENDCG
        }
        
//        Pass
//        {
//            
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            // make fog work
//            #pragma multi_compile_fog
//
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float4 normal : NORMAL;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                UNITY_FOG_COORDS(1)
//                float4 vertex : SV_POSITION;
//            };
//
//            sampler2D _MainTex;
//            float _OutlineWidth;
//            float4 _MainTex_ST;
//
//            v2f vert(appdata v)
//            {
//                v2f o;
//                float4 vertex = v.vertex + normalize(v.normal) * _OutlineWidth;
//                o.vertex = UnityObjectToClipPos(vertex);
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                UNITY_TRANSFER_FOG(o,o.vertex);
//                return o;
//            }
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//                fixed4 col = tex2D(_MainTex,i.uv);
//                UNITY_APPLY_FOG(i.fogCoord,col)
//                return float4(0.5,0,0,1);
//            }
//            ENDCG
//        }
    }
}
