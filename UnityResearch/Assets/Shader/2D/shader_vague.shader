Shader "Application2D/shader_vague"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Bold ("Sampler Bold",Range(0.1,0)) = 0.01
        _SamplerRatio("Sampler Ratio",Range(1.0,0)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="TransParent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            float4 _MainTex_ST;
            float _Bold;
            float _SamplerRatio;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 vagueSampler(sampler2D tex,float2 uv)
            {
                fixed color = fixed4(0,0,0,0);
                color += (1 - _SamplerRatio) * tex2D(tex,uv);
                color += _SamplerRatio / 4 * tex2D(tex,uv + float2(_Bold,-_Bold));
                color += _SamplerRatio / 4 * tex2D(tex,uv + float2(-_Bold,_Bold));
                color += _SamplerRatio / 4 * tex2D(tex,uv + float2(-_Bold,-_Bold));
                color += _SamplerRatio / 4 * tex2D(tex,uv + float2(_Bold,_Bold));
                
                return color;
            }
                
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = vagueSampler(_MainTex,i.uv);

                return col;
            }
            ENDCG
        }
    }
}
