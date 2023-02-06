Shader "UIShader/Shader_BgWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color",Color) = (1,1,1,1)
        _CompParam("Complication",Range(0,1)) = 0.7
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _CompParam;
            float4 _Color;
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
                float2 uv = float2(i.uv.x + sin(_Time.x * 1.5),i.uv.y + sin(_Time.x * 1.5));
                fixed4 col = tex2D(_MainTex, uv);
                uv = float2(i.uv.x + cos(_Time.x),i.uv.y + cos(_Time.x))/2;
                fixed4 par = tex2D(_MainTex, uv);
                float len = saturate(length(i.uv));
                col.rgb = col.rgb * _CompParam + par.rgb * (1 - _CompParam);
                col.rgb *= len;
                return col;
            }
            ENDCG
        }
    }
}
