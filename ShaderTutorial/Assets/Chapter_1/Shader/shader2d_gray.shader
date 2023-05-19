Shader "Tutorial2D/shader2d_gray"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // 灰度比例值
        _Gray ("GrayValue",Range(0,1)) = 0

        // UnityMask所需参数
        [HideInInspector]
        _StencilComp ("Stencil Comparison", Float)=8
        [HideInInspector]
        _Stencil ("Stencil ID", Float)=0
        [HideInInspector]
        _StencilOp ("Stencil Operation", Float)=0
        [HideInInspector]
        _StencilWriteMask ("Stencil Write Mask", Float)=255
        [HideInInspector]
        _StencilReadMask ("Stencil Read Mask", Float)=255
        [HideInInspector]
        _ColorMask ("Color Mask", Float)=15
    }
    SubShader
    {
        Tags {
            "RenderType"="TransParent"
            "CanUseSpriteAtlas"="True"
        }

		Cull Off         //关闭剔除
		Lighting Off     //关闭灯光
		ZWrite Off       //关闭Z缓冲
        Blend SrcAlpha OneMinusSrcAlpha

        // UnityMask实现
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

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
            float _Gray;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // 灰色的色值一般都是rgb趋向于统一并且降低的，比如说一张纯白的图（1，1，1，1），置灰的话就应该是（0.5，0.5，0.5，1）
                // 总体上的视觉逻辑是，rgb值同时下降图片会变暗，rgb值之间的差值缩小图片会泛白，当同时执行这两个规律的时候，图片就会泛黑
                // g变量在此代表一个灰度值，灰度值由当前像素的rgb共同参与影响，这样既可以保持纹理各个色块之间的差异（以黑白区分），又可以使得同一色块儿下的颜色趋近于灰色
                fixed g = col.r * 0.299 + col.g * 0.518 + col.b * 0.183;
                // float3（g,g,g）使rgb值统一为同一值，进而呈现灰色
                fixed4 mix = float4(col.rgb * ( 1 - _Gray ) + float3( g, g, g) * _Gray,col.a);
                return mix;
            }
            ENDCG
        }
    }
}