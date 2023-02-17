Shader "Applications/outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutLine("OutLineBold",Range(0.2,0)) = 0.05
        _OutLineColor("OutLineColor",Color) = (0,0,0,1)
        _ModelColor("ModelColor",Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            Tags {"RenderType" = "TransParent"}
            Blend SrcAlpha OneMinusSrcAlpha
            cull front
            Zwrite off
            
            Stencil
            {
                ref 2
                comp GEqual
                pass Replace
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include  "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 pos: TEXCOORD1;
            };

            float4 _OutLineColor;
            float _OutLine;

            v2f vert (appdata_full v)
            {
                v2f o;
                v.vertex.xyz += _OutLine * normalize(v.tangent);
                o.uv = v.texcoord;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = _OutLineColor;
                return col;
            }
            ENDCG
        }
        
        Pass
        {
            Tags { "RenderType"="TransParent" }
            Blend SrcAlpha OneMinusSrcAlpha
            
            Stencil
            {
                ref 1
                comp Less
                pass replace
            }
            
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
                float4 pos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ModelColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.pos = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = _ModelColor;
                return col;
            }
            ENDCG
        }
    }
}
