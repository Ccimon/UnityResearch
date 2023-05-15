Shader "Tutorial2D/shader2d_gradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

         // UnityMask所需参数
         _StencilComp ("Stencil Comparison", Float)=8
        _Stencil ("Stencil ID", Float)=0
        _StencilOp ("Stencil Operation", Float)=0
        _StencilWriteMask ("Stencil Write Mask", Float)=255
        _StencilReadMask ("Stencil Read Mask", Float)=255
        _ColorMask ("Color Mask", Float)=15
    }
    SubShader
    {
        Tags { "RenderType"="TransParent" }
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
                // 图片的uv的x值映射到图片颜色的r值上
                // 图片的uv的y值映射到图片颜色的g值上
                // 一张纹理的uv坐标的x,y值，都是从0～1变化的，色值也是如此
                // 当我们将x值映射到r值上，整个图片就会呈现出，从左到右逐渐变红的效果
                // y值，同理，将其映射到g值上，整个纹理就会自下而上的变绿
                col.r = i.uv.x;
                col.g = i.uv.y;
                return col;
            }
            ENDCG
        }
    }
}
