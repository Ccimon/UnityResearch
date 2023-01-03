Shader "shader2d/alphacode3"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("MainColor",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
		{ 
			"Queue"="Transparent" 
            "RenderType"="Transparent" 
        }

		Blend SrcAlpha DstAlpha   

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
            float4 _MainCol;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color;
                // apply fog
                return col;
            }
            ENDCG
        }
    }
}
