Shader "Tutorial2D/shader2d_blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // 采样等级 等级越高，模糊采样次数越多
        _SampleLevel("BlurRadius",float) = 1
        // 采样距离 该数值越大 采样距离越近
        _SampleSize("BlurSize",float) = 50
        
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
        Tags { "RenderType"="Transparent" }
        Cull Off
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
            float _SampleLevel;
            float _SampleSize;
            float4 _MainTex_ST;
            
            // 关于模糊其实实现的思路主要只有一种，就是使得一个像素其旁边的像素影响该像素，使像素之间的颜色趋近，就会变得模糊
            // 好比一张本来 600*600的图片 现在我们把它压缩到100*100 也就意味着原来 6*6 的像素区间描述的颜色，现在用 1*1 来描述
            // 本质上就是使一个像素与周围的像素趋同化
            // 实现以上思路的方式，主要依靠该方法
            fixed4 SampleBlur(float2 uv)
            {
                float4 col = float4(0,0,0,0);
                // SampleLevel 采样等级，负责控制采样的次数，采样等级越高，采样次数越多，模糊的效果也越好
                // 这里循环遍历当前像素的周围的点进行采养，相当于一个方形采样的效果
                // 当 SampleLevel = 1的时候相当于采样 包括这个点及其周围8个点的像素
                for (int x = - _SampleLevel; x <= _SampleLevel; ++x)
                {
                    for (int y = - _SampleLevel; y <= _SampleLevel; ++y)
                    {
                        // SampleSize也就是采集长度，负责控制每次采集像素时，uv的偏移距离，与SampleLevel叠加共同完成采样累积
                        float4 color = tex2D(_MainTex, uv + float2(x / _SampleSize, y / _SampleSize));
                        // 将颜色叠加
                        col += color;
                    }
                }

                // 颜色求平均，pow这里是计算了一个平方，当SampleLevel为1是 也就是3的2次幂 总共采集了9个点
                col = col / pow(_SampleLevel * 2 + 1, 2.0f);
                return col;
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
                // 将颜色赋返回到片面函数
                fixed4 col = SampleBlur(i.uv);
                return col;
            }
            ENDCG
        }
    }
}
