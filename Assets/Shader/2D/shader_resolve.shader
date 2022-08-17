Shader "shader2d/resolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ResolveTex ("Texture", 2D) = "white" {}
        _ResolvePer ("ResplvePer", Range(0,1)) = 0
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
            #include "UnityCG.cginc"
            
            sampler2D _ResolveTex;
            float4 _MainTex_ST;
            float _ResolvePer;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 rso = tex2D(_ResolveTex, i.uv);
                clip(rso.r * 0.1 + rso.g * 0.5 + rso.b * 0.4 - _ResolvePer);

                // if(rso.r * 0.1 + rso.g * 0.5 + rso.b * 0.4 < _ResolvePer)
                // {
                //     discard;
                // }
                
                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
