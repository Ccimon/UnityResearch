Shader "Tutorial2D/shader2d_gradient"
{
    Properties
    {
        // 主颜色
        _MainColor ("MainColor", Color) = (0,1,0,1)
        // 副颜色
        _SubColor("SubColor",Color) = (1,0,0,1)
        // 渐变混合范围
        _GradientRange("GradientRange",Range(0,0.5)) = 0.2
        // 渐变中心
        _GradientBorder("GradientBorder",Range(0,1)) = 0.5

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
        Blend off

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

            float4 _MainColor;
            float4 _SubColor;
            float _GradientRange;
            float _GradientBorder;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {
                // 计算主颜色，副颜色的渲染范围
                // mainRange是从左到右，主颜色最多渲染到哪里，可以视作两个颜色的渲染长度
                // subRange是从左到右，副颜色从哪里开始渲染，视作副颜色的渲染起点
                float mianRange = _GradientBorder + _GradientRange;
                float subRange = _GradientBorder - _GradientRange;

                // 计算当前像素是否渲染主颜色以及副颜色，step（x,y）如果x<y返回0，否则返回1
                // step函数是优化Shader编程中的一个重要函数
                // GPU与CPU的架构不同，GPU并不善于对各种资源的管理以及索引
                // 也因此if语句在GPU看来并不是一个性能较好的语句
                // GPU的架构是一个力大砖飞的结构，GPU中会有数千个小的渲染计算单元对数十万，百万的像素执行我们语句
                // 所以我们可以去想象，一个if语句，将代码分开，导致不同的像素会去执行不同的语句
                // 但渲染最终画面的时候，却都是同一帧，所以本质上还是以执行时间较长的那块代码为准
                // 更重要的是，我们无法确认哪个单元在执行if分支中较少的那一个，哪个在执行较长的那一个
                // 所以绝大多数时候我们的渲染单元会同时加载两个分支的代码
                // 反而，GPU会针对我们的基础运算，浮点数计算，乘除法计算，甚至一些专门的函数，比如step(),saturate()进行优化
                // 结果上，我们做分支，不如使用step将两个可能的结果相加
                float mianParam = step(i.uv.x,mianRange);
                float subParam = step(subRange,i.uv.x);

                // 主颜色*主颜色参考值 计算当前像素是否渲染主颜色
                // mainRange - i.uv.x在渲染范围内，从左到右逐渐变浅
                float4 mainColor = _MainColor * mianParam * (mianRange - i.uv.x) / mianRange;
                // 副颜色同上
                // 从渲染起点开始，从左到右逐渐变深
                float4 subColor = _SubColor * subParam * (i.uv.x - subRange) / mianRange;
                float4 col = mainColor + subColor;
                return col;
            }
            ENDCG
        }
    }
}
