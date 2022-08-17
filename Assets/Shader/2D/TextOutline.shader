Shader "TSF Shaders/UI/TextOutline" 
{
Properties
    {
        [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth ("Outline Width", Int) = 1
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    
    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        // Blend Off
        ColorMask [_ColorMask]
        
        Pass
        {
            Name "OUTLINE"
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            //Add for RectMask2D  
            #include <UnityShaderUtilities.cginc>

            #include "UnityUI.cginc"
            //End for RectMask2D  
            
            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _MainTex_TexelSize;
            
            float4 _OutlineColor;
            int _OutlineWidth;
            
            //Add for RectMask2D  
            float4 _ClipRect;
            //End for RectMask2D
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                fixed4 color : COLOR;
            };
            
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 tangent : TANGENT;
                float4 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                //Add for RectMask2D  
                float4 worldPosition : TEXCOORD4;
                //End for RectMask2D
                fixed4 color : COLOR;
            };
            
            //顶点渲染过程其实没有太多的操作
            v2f vert(appdata IN)
            {
                v2f o;
                
                //Add for RectMask2D  
                o.worldPosition = IN.vertex;
                //End for RectMask2D 
                
                o.vertex = UnityObjectToClipPos(IN.vertex);
                o.tangent = IN.tangent;
                o.texcoord = IN.texcoord;
                o.color = IN.color;
                o.uv1 = IN.uv1;
                o.uv2 = IN.uv2;
                o.uv3 = IN.uv3;
                o.normal = IN.normal;
                
                return o;
            }
            /*
            fixed IsInRect(float2 pPos, float4 pClipRect)
            {
                pPos = step(pClipRect.xy, pPos) * step(pPos, pClipRect.zw);
                return pPos.x * pPos.y;
            }
            */
            
            fixed IsInRect(float2 pPos, float2 pClipRectMin, float2 pClipRectMax)
            {
                //Step(x,y)函数的含义是y>x,返回1,y<x,返回0
                pPos = step(pClipRectMin, pPos) * step(pPos, pClipRectMax);
                //如果这里返回0,则证明超出边界
                return pPos.x * pPos.y;
            }
            
            fixed SampleAlpha(int pIndex, v2f IN)
            {

                //这12个值的含义实际上是围绕当前的纹理点绕一个圆形，每隔30度取一个点，360/30 于是就有了这12个值的数组
                //两个分别对应sin，cos实际上是直接转换了坐标，方便直接采样
                const fixed sinArray[12] = { 0, 0.5, 0.866, 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5 };
                const fixed cosArray[12] = { 1, 0.866, 0.5, 0, -0.5, -0.866, -1, -0.866, -0.5, 0, 0.5, 0.866 };
                //IN.texcoord是当前的uv坐标
                //_MainTex_TexelSize，这个变量的从字面意思是主贴图 _MainTex 的像素尺寸大小，是一个四元数，是 unity 内置的变量，它的值为 Vector4(1 / width, 1 / height, width, height)
                //这个Pos变量实际上是对原来的纹理坐标进行了一个偏移计算
                float2 pos = IN.texcoord + _MainTex_TexelSize.xy * float2(cosArray[pIndex], sinArray[pIndex]) * _OutlineWidth;	//normal.z 存放 _OutlineWidth
                return IsInRect(pos, IN.uv1, IN.uv2) * tex2D(_MainTex, pos).a * _OutlineColor.a;    //如果这个点在边界内 就返回纹理与描边颜色的透明度乘积
            }


            
            fixed4 frag(v2f IN) : SV_Target
            {
                //默认的文字颜色
                fixed4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                //normal.z 存放 _OutlineWidth
                if (_OutlineWidth > 0)	
                {
                    //uv1 uv2 存着原始字的uv长方形区域大小
                    color.w *= IsInRect(IN.texcoord, IN.uv1, IN.uv2);	
                    //uv3.xy tangent.z 分别存放着 _OutlineColor的rgb
                    half4 val = half4(_OutlineColor.rgb, 0);		
                    //val 是 _OutlineColor的rgb，a是后面计算的
                    //在shader里一个fixed4,half4等。其中的数值可以通过xyzw,rgba这样的方式去引用,都是一样的。
                    //实际上这里val的w,即a也就是alpha是参与了后面的计算的
                    val.w += SampleAlpha(0, IN);
                    val.w += SampleAlpha(1, IN);
                    val.w += SampleAlpha(2, IN);
                    val.w += SampleAlpha(3, IN);
                    val.w += SampleAlpha(4, IN);
                    val.w += SampleAlpha(5, IN);
                    val.w += SampleAlpha(6, IN);
                    val.w += SampleAlpha(7, IN);
                    val.w += SampleAlpha(8, IN);
                    val.w += SampleAlpha(9, IN);
                    val.w += SampleAlpha(10, IN);
                    val.w += SampleAlpha(11, IN);
                    
                    //在这里val.rgb都是来自上面_OutlineColor也就是描边颜色
                    //所以这里就是将字体的颜色与描边的颜色进行混合
                    color = (val * (1.0 - color.a)) + (color * color.a);
                    //saturate是归一化函数,x>1就取1,0<x<1就取x本身,x<0则取0
                    color.a = saturate(color.a);
                    //字逐渐隐藏时，描边也要隐藏
                    color.a *= IN.color.a;	
                }
                
                //Add for RectMask2D 
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                #endif
                //End for RectMask2D 
                
                return color;
            }
            
            ENDCG
        }
    }
}