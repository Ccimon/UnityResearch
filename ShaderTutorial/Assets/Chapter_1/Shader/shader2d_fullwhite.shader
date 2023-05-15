Shader "Tutorial2D/shader2d_fullwhite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MixColor("MixColor", Range(0,1)) = 0.5

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
        Tags
        {
            "RenderType"="TransParent"
            "CanUseSpriteAtlas"="True"
        }

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
            float4 _MainTex_ST;
            float _MixColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // 在变灰的时候我们知道，一个图片变黑，然后色值趋向统一，两种变化同时实现就会呈现置灰效果
                // 而一张纹理的亮度调整，就是整体泛白，也就对应着rgb值统一上升
                // 让rgb值统一加上一个变量，纹理就会整体趋向于白色，也就会变得更亮
                col.rgb += _MixColor;
                return col;
            }
            ENDCG
        }
    }
}
