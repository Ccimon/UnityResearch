// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shadow"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
		_ShadowValue("_ShadowValue",Range(0,1)) = 0.5
		_SoftVaule("Soft",Range(0,2)) = 0.1
    }
        SubShader
    {
        Tags
    {
        "Queue" = "Transparent+100"
    }
    zwrite off
        Pass
    {
        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
 
		#include "UnityCG.cginc"
        uniform float4x4 _WorldToCameraMatrix;
        uniform float4x4 _ProjectionMatrix;
 
        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };
 
        struct v2f
        {
            float2 uv : TEXCOORD0;
			float2 uv1:TEXCOORD1;
			float2 uv2:TEXCOORD2;
			float2 uv3:TEXCOORD3;
			float2 uv4:TEXCOORD4;
            float4 vertex : SV_POSITION;
        };
        float _ShadowValue;
		float _SoftVaule;
		float4 _MainTex_TexelSize;

        v2f vert(appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);

            //将该模型投影到影子生成的纹理的照相机的空间中，便可以将纹理一一对应上
            float4 worldCoord = mul(unity_ObjectToWorld, v.vertex);
            float4 cameraCoord = mul(_WorldToCameraMatrix, worldCoord);
            float4 projectionCoord = mul(_ProjectionMatrix, cameraCoord);
            o.uv = projectionCoord / projectionCoord.w;
            o.uv = 0.5f*o.uv + float2(0.5f, 0.5f);

		
		    o.uv1 = o.uv + _MainTex_TexelSize*_SoftVaule*float2(1,1);
			o.uv2 = o.uv + _MainTex_TexelSize*_SoftVaule*float2(-1,1);
			o.uv3 = o.uv + _MainTex_TexelSize*_SoftVaule*float2(1,-1);
            o.uv4 = o.uv + _MainTex_TexelSize*_SoftVaule*float2(-1,-1);

            return o;
        }
 
        sampler2D _MainTex;

        fixed4 frag(v2f i) : SV_Target
        {
		   //计算离中心的距离
            //float dis = (i.uv.x - 0.5f)*(i.uv.x - 0.5f) + (i.uv.y - 0.5f)*(i.uv.y - 0.5f);
			
            fixed4 col =  tex2D(_MainTex, i.uv);

		
			 col +=  tex2D(_MainTex, i.uv1);
			 col +=  tex2D(_MainTex, i.uv2);
			 col +=  tex2D(_MainTex, i.uv3);
			 col +=  tex2D(_MainTex, i.uv4);

			 col = col*0.1;

		     if(col.a >0)
			 {
			   col.rgb = float4(0.0f,0.0f,0.0f,col.a*_ShadowValue);
			 }

            //这里将纹理沿着边缘虚幻处理
             //col.a = (col.a - dis*4.0f)*_ShadowValue;
			 		
             return col;
        }
            ENDCG
        }
    }
}