Shader "Tutorial2D/shader2d_shadow"
{
    Properties
    {
        // 主纹理
        _MainTex ("Texture", 2D) = "white" {}
        // 阴影颜色，Alpha通道生效
        _ShadowColor ("ShadowColor",Color) = (0,0,0,1)
        // 光照方向
        _LightDirection ("LightAngle",Range(0,360)) = 0
        // 阴影距离
        _ShadowDistance ("ShadowDistance",float) = 0
        
        // 阴影开关
        [Toggle]
        _ShadowRender ("MainTex Render",int) = 1
        
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
        
        // 制作阴影Shader需要我们创建两个pass
        // 两个pass粗浅的理解，就是渲染两边，但是做不同的处理
        // 我们需要一个pass渲染物体本身
        // 另一个pass渲染阴影
        // 要注意两个pass的顺序，被遮盖的pass总是最先渲染，要放在最上面
        
        Pass
        {
            // Name pass的名称
            Name "Shadow"
            // 这些tags既可以放在subshader中，也可以放在pass里
            // 当放在pass里时，就是对单一通道的配置
            Tags { "RenderType"="Opaque" "CanUseSpriteAtlas" = "True"}
            // 这些渲染参数也是同样的，可以单独配置一个pass
            Blend SrcAlpha OneMinusSrcAlpha
            
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
            fixed4 _ShadowColor;
            float _LightDirection;
            float _ShadowDistance;
            int _ShadowRender;

            v2f vert (appdata v)
            {
                v2f o;
                // 计算光照角度
                float angle = radians(_LightDirection);
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 使用角度使阴影相较于正常渲染偏移一段距离
                o.vertex.xy -= float2(sin(angle),cos(angle)) * _ShadowDistance;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 阴影开关
                clip(_ShadowRender - 1);
                // 采集主纹理，我们需要获取主纹理的形状，它的颜色不重要
                fixed4 col = tex2D(_MainTex, i.uv);
                // 修改rgb，统一阴影颜色
                col.rgb = _ShadowColor;
                // 叠加alpha值，使得相较于正常渲染获得更加透明的效果
                col.a *= _ShadowColor.a;
                return col;
            }
            ENDCG  
        }
        
        Pass
        {
            // 正常渲染的pass
            Name "Image"
            Tags { "RenderType"="TransParent" "CanUseSpriteAtlas" = "True"}
            Blend SrcAlpha OneMinusSrcAlpha
            
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
            int _MainRender;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 采集渲染主纹理
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
        
    }
}
