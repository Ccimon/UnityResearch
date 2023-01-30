﻿Shader "shader3d/unlite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="TransParent" }
        LOD 210

        //这个是遮盖物体的Shader，最开始尝试的时候，用的是3D物体的StandardShader
        //因为被遮盖物体的Shader的实现基于的是3D的Shader
        //但是StandardShader用到UI上会导致UI有高亮以及光照渲染
        //所以在经过尝试之后发现UnliteShader也有和StandardShader也有一样的ZTest渲染逻辑
        //这里就用UnliteShader的模版作为遮盖物的Shader
        Pass
        {
            ZTest LEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

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
                col.rgb *= col.a;
                if((col.r + col.g + col.b)*col.a< 0.95){
                    discard;
                }
                return fixed4(0,0,0,0);
            }
            ENDCG
        }
    }
}