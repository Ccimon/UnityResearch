Shader "Tutorial2D/shader2d_fullwhite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MixColor("MixColor", Range(0,1)) = 0.5

        
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
        // Tag可以全部不设置，因为这些及时不设置，Unity也会配置默认值
        // 这些都是一些有关于Unity引擎与Shader之间的配置
        // RenderType 渲染类型，当我们指定该变量时，Unity引擎会准备好相关的渲染环境
        // CanUseSpriteAtlas 是否使用引擎输入的Sprite，该变量为true时，Shader会接受渲染组件中的纹理为_MainTex进行渲染
        Tags
        {
            "RenderType"="TransParent"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        // UnityMask实现
        // Unity的Mask是基于模版测试，Stencil实现的
        // 模版测试是一个较为复杂的过程，Stencil有着相当丰富的参数供我们调整其中流程
        // 虽然复杂，但是它负责的事情却不复杂，他负责当两个物体叠加的时候，两个颜色如何处理，里面大量的可配置参数都是对两个像素的比较以及结果处理
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
            float _MixColor;

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
