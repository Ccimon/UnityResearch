Shader "Tutorial2D/shader2d_Pro"
{
    Properties
    {
        // 主纹理
        _MainTex ("MainTexture",2D) = "white"{}
        // 主颜色
        _MainColor ("MainColor", Color) = (0,1,0,1)
        // 副颜色
        _SubColor("SubColor",Color) = (1,0,0,1)  
        // 渐变混合范围   
        _Range("Range",Range(0,1)) = 0.1
        // 渐变中心
        _Border("Border",Range(0,1)) = 0.5
        // 渐变方向角度
        _Angle("Angle", Range(0, 180)) = 0

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
            float4 _MainColor;
            float4 _SubColor;
            float _Range;
            float _Border;
            float _Angle;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // 采集主纹理的颜色
                float4 col = tex2D(_MainTex,i.uv);
                // 计算渐变方向的角度（以弧度为单位）
                float angle = radians(_Angle);
            
                // 确定旋转中心为纹理中心点
                float2 centerUV = float2(0.5, 0.5);
            
                // 根据角度旋转纹理坐标
                float2 rotatedUV = float2(
                    (i.uv.x - centerUV.x) * cos(angle) - (i.uv.y - centerUV.y) * sin(angle) + centerUV.x,
                    (i.uv.x - centerUV.x) * sin(angle) + (i.uv.y - centerUV.y) * cos(angle) + centerUV.y
                );
            
                // 计算渐变位置(这里我们只要考虑垂直分量即可，因为本质上我们只是做了一个垂直分量上的渐变，然后旋转成不同方向的渐变而已，所以只计算垂直分量就够用) 
                float position = rotatedUV.y - (_Border - _Range / 2);
            
                // 得出此处渐变程度，给lerp函数用
                float progress = saturate(position / _Range);
            
                // 根据位置在主颜色和副颜色之间进行插值，叠加上纹理的颜色，获得最终颜色
                // Lerp(x,y,w) x,y分别指定起始值和目标值，w为进度权重
                // Lerp函数会让输出逐渐由x趋紧y，并且越是趋进y，其增量就越小
                // 从而达到一个缓和的渐变效果
                col = col * lerp(_SubColor, _MainColor, progress);

                // 返回最终颜色
                return col;
            }
            ENDCG
        }
    }
}
