Shader "Unlit/Shader_Nothing"
{
    Properties
	{
        _MainTex ("贴图", 2D) = "white" {}
		_BackgroundColor ("背景颜色", Color) = (1, 1, 1, 1)
        _alphaValue("alpha", range(0.00,1.00)) = 0.5
	}
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        //关闭裁剪这样就嫩看到镂空的背面
        Cull Off 
        //关闭深度写入
        ZWrite Off 
        
//        ColorMask GB
        
        Blend SrcColor OneMinusSrcAlpha
        
        //透明测试  开启透明测试，当前限速透明度 > _alphaValue 时，显示当前限速
//        AlphaTest Greater [_alphaValue]
        Pass
        {
            //输出颜色
            SetTexture[_MainTex]
        }
    }  
//    Properties
//    {
//        _MainTex ("Texture", 2D) = "white" {}
//        _Color ("MainColor", Color) = (1, 1, 1, 1)
//        _AlphaClip("AlphaClip",Range(0.00,1.00)) = 0.5
//    }
//    SubShader
//    {
//        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
//        Cull Off 
//        AlphaTest Greater [_AlphaClip]
//        Zwrite off
//        Blend off
//        Pass
//        {
//
//            
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                float4 vertex : SV_POSITION;
//            };
//
//            sampler2D _MainTex;
//            float4 _MainTex_ST;
//
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                return o;
//            }
//
//            fixed4 frag (v2f i) : SV_Target
//            {
//                // sample the texture
//                fixed4 col = tex2D(_MainTex, i.uv);
//                return col;
//            }
//            ENDCG
//        }
//    }
}
