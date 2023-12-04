Shader "Application3D/shader_Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //卡通渲染 主颜色
        _Color("Color",Color) = (1,1,1,1)
        //卡通渲染 次要颜色
        _SubColor("SubColor",Color) = (1,1,1,1)
        //深度值换算比例
        _DepthMaxLen("DepthMaxLength",float) = 1
        //噪点纹理，柏林噪点
        _NoiseTex("NoiseTexture",2D) = "white" {}
        //噪点裁剪，取消一定范围内的噪点影响
        _NoiseCull("Noise",Range(1,0)) = 0.5
        //气泡颜色
        _FoamColor("FoamColor",Color) = (1,1,1,1)
        //气泡所覆盖的范围
        _FoamDistance("FoamDistance",float) = 0.4
        //溶解纹理，与噪点混合，使其更分散
        _DistortTex("DistortTexture",2D) = "white" {}
        //噪点纹理的移动方向
        _NoiseDirection("NoiseFlowDirection",Vector) = (0.03,0.03,0.03,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
      
        Pass
        {
            ZWrite On
//            ZTest Greater
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                //世界顶点坐标
                float4 worldVertex : TEXCOORD1;
                //屏幕坐标位置
                float4 screenPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            //摄像机深度纹理
            sampler2D _CameraDepthTexture;
            sampler2D _CameraDepthNormalsTexture;
            sampler2D _DistortTex;
            float _DepthMaxLen;
            fixed _NoiseCull;
            float _FoamDistance;
            float4 _NoiseDirection;
            float4 _NoiseTex_ST;
            float4 _Color;
            float4 _SubColor;
            float4 _FoamColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.color = v.color;
                o.normal = UnityObjectToWorldNormal(v.normal);
                //获取顶点的世界坐标
                o.worldVertex = mul(unity_ObjectToWorld,v.vertex);
                //获取顶点的屏幕位置，齐次坐标系下
                o.screenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //采集溶解纹理
                fixed4 disort = tex2D(_DistortTex,i.uv);
                //偏移噪点纹理
                float2 noiseUV = i.uv + _Time.y * _NoiseDirection.xy + disort.rg;
                fixed4 col = tex2D(_NoiseTex,noiseUV);
                //获取深度信息
                float depthInfo = 1;
                float3 normalInfo = 1;
                depthInfo = tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPosition)).r;
                // DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture,UNITY_PROJ_COORD(i.screenPosition.xy/i.screenPosition.w)),depthInfo,normalInfo);
                //转换到视图空间下深度
                float depthLinear = LinearEyeDepth(depthInfo);
                //其次坐标空间下的3维坐标，w类似于深度
                float waterDepth = i.screenPosition.w;
                //获取水面与地面之间的距离
                float depthDiff = depthLinear - waterDepth;
                //使用深度限制归一化
                float depthRatio = saturate(depthDiff / _DepthMaxLen);
                //跑泡沫也使用同样的思路归一化
                float foamRatio = saturate(depthDiff / _FoamDistance);
                //对噪点图进行裁剪，越浅的地方fomRatio越大
                //使用这种方式对噪点的裁剪限制进行缩放，可以控制泡沫的范围
                float noise = step(_NoiseCull * foamRatio,col.r);
                //使用Lerp让两个水颜色之间过渡
                float4 waterColor = lerp(_Color,_SubColor,depthRatio);
                //叠加泡沫，noise是个裁剪结果
                waterColor = lerp(waterColor,_FoamColor,noise);
                return waterColor;
            }
            ENDCG
        }
    }
}
