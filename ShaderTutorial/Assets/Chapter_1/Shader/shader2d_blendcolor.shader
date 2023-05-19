Shader "Tutorial2D/shader2d_blendcolor"
{
    Properties
    {
        // 主要颜色
        _MainTex ("MainTexture",2D) = "White"{}
        // 次要颜色
        _SubColor ("SubColor",Color) = (0,1,0,0.5)
        
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

            sampler2D _MainTex;
            float4 _SubColor;
            
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
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex,i.uv);
                // 主纹理颜色与副颜色根据alpha值进行混合
                // Blend的混合逻辑与次相似
                // 不过Blend混合含义，指的是当前像素颜色如何与已经渲染完成的背景色进行
                // 这里的混合操作对应的是 Blend SrcAlpha OneMinusSrcAlpha 即源色值与目标色值以各自的透明度进行混合
                col.rgb = _SubColor.rgb * _SubColor.a + col.rgb * (1 - _SubColor.a);
                
                return col;
            }
            ENDCG
        }
    }
}
