Shader "Tutorial2D/shader2d_outline"
{
    Properties
    {
        // 主纹理
        _MainTex ("Texture", 2D) = "white" {}
        // 采样级别
        _SampleLevel ("SampleLevel",int) = 4
        // 描边颜色
        _OutlineColor ("OutlineColor",Color) = (1,1,1,1)
        // 描边宽度
        _OutlineBold ("OutlineBold",float) = 0.1
        
        // 是否启用Outline
        [Toggle]
        _UseOutline ("UseOutline",int) = 1
        
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

        // Unity Mask实现
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
            // _MainTex_TexeSize 是一个内置参数，其四个值分别为(1/width,1/height,width,height)
            float4 _MainTex_TexelSize;
            fixed4 _OutlineColor;
            float _OutlineBold;
            int _UseOutline;
            int _SampleLevel;

            // 边缘采样，返回采样之后的透明度系数
            // 边缘采样的核心逻辑是，当我们采集一个像素的时候，同时对其周围的像素进行采样
            // 当该像素周围存在透明像素的时候，说明该像素处于边界
            // 也就意味着该像素点处在边界需要与描边颜色进行混合
            float edge_sample(float2 uv)
            {
                // 透明度叠加结果值
                float alpha = 1;
                // 边缘采样的采样偏移弧度
                float rad = 0;
                // 采样等级负责采样的精细程度
                for(int i = 0; i < _SampleLevel ; i++)
                {
                    // 计算为弧度
                    rad = radians(i * 360 / _SampleLevel);
                    // 对原有的uv进行偏移，并根据纹理的像素进行换算
                    float2 offset_uv = uv + float2(sin(rad),cos(rad)) * _OutlineBold * _MainTex_TexelSize.xy;
                    // 以乘法叠加透明度，如果alpha最终结果为0，则说明有一个像素是透明的，则该像素点处在边界
                    alpha *= tex2D(_MainTex,offset_uv).a;
                }

                // 返回透明度叠加值
                return alpha;
            }
            
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
                // 根据是否启用描边来选择结果
                // 通过边缘采样返回的透明值结果，来让描边颜色与主纹理颜色进行混合
                // 使用lerp来使颜色叠加变得光滑
                col.rgb = _UseOutline * lerp(_OutlineColor.rgb,col.rgb,edge_sample(i.uv)) +
                    (1 - _UseOutline) * col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
