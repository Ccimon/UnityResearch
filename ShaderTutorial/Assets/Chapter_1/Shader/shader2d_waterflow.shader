Shader "Tutorial2D/shader2d_waterflow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // 频率
        _Freq ("Frequence",float) = 1
        // 振幅
        _Ampl ("Amplitude",float) = 1
        // 函数水平线
        _Horizon ("Horizontal Line",float) = 0
        
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
            float _Freq;
            float _Ampl;
            float _Horizon;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 对y值进行采样修改
                // 因为要对y方向的点进行扭曲，而且随着x从0到1要做波浪效果，也就是sin/cos函数效果，所以uv.x必须是其中的影响参数
                // 因为要随着时间进行波动所以_Time也是其中的影响参数
                // uv.x负责让图片y方向根据x方向坐标产生扭曲，_Time负责让图片根据时间推移产生变化
                // _Ampl为振幅影响函数的最高点和最低点
                // _Freq为频率影响sin/cos函数的周期长短
                // _Horizon为水平线负责调整整个动画在y轴上的位置
                float2 uv = float2(i.uv.x,i.uv.y + _Ampl * cos(i.uv.x * _Freq + _Time.y) + _Horizon);
                fixed4 col = tex2D(_MainTex,uv);
                
                return col;
            }
            ENDCG
        }
    }
}
