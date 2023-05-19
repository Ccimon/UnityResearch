Shader "Tutorial2D/shader2d_light"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // 光照程度，低于0.5为削弱，高于0.5为增强
        _Light ("Light",Range(0,1)) = 0.5
        
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
            float _Light;
            
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
                // 计算是削弱还是增强
                float param = step(_Light,0.5);
                // 处于削弱区间时，[0,0.5]，随着光照系数减少，削弱图片的rgb值，趋向于黑色
                // 处于增强区间时，[0.5,1],随着光照系数增强，图片的rgb值趋近于1
                col.rgb = col.rgb - param * col.rgb * (0.5 - _Light) * 2 + (1 - param) * float3(1,1,1) * (_Light - 0.5) * 2;
                return col;
            }
            ENDCG
        }
    }
}
