Shader "shader3d/ShadowCast"
{
Properties
	{
		_Specular("Specular", Color) = (1,1,1,1)
		//控制高光区域的大小
		_Gloss("Gloss", Range(8.0,256)) = 20
		//控制高光反射的颜色
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		//
		_Alpha("Alpha",Range(0,1)) = 1
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
	}
	
	SubShader
	{
		//BasePass,渲染环境光和最重要的平行光
	Pass
	{
		//指明光照模式为前向渲染模式
		Tags{ "LightMode" = "ForwardBase" }
		blend srcalpha oneminussrcalpha
		
		CGPROGRAM
#pragma vertex vert  
#pragma fragment frag  
		//确保光照衰减等光照变量可以被正确赋值
#pragma multi_compile_fwdbase

		//包含引用的内置文件  
#include "Lighting.cginc"  
#include "AutoLight.cginc"
		
		//声明properties中定义的属性  
		fixed4 _Diffuse;
	    fixed4 _Specular;
	    float _Gloss;
		float _Alpha;
		fixed _Cutoff;

	//定义输入与输出的结构体  
	struct a2v
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		//存储世界坐标下的法线方向和顶点坐标  
		float3 worldNormal : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		//添加内置宏，声明一个用于阴影纹理采样的坐标，参数是下一个可用的插值寄存器的索引值
		SHADOW_COORDS(2)
	};

	//在顶点着色器中，计算世界坐标下的法线方向和顶点坐标，并传递给片元着色器  
	v2f vert(a2v v)
	{
		v2f o;
		//转换顶点坐标到裁剪空间  
		o.pos = UnityObjectToClipPos(v.vertex);
		//转换法线坐标到世界空间，直接使用_Object2World转换法线，不能保证转换后法线依然与模型垂直  
		o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
		//转换顶点坐标到世界空间  
		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

		//添加另一个内置宏，用于在顶点着色器中计算上一步中声明的阴影纹理坐标
		TRANSFER_SHADOW(o);
		return o;
	}

	//在片元着色器中计算光照模型  
	fixed4 frag(v2f i) : SV_Target
	{
		//获取环境光 ,只计算一次，在之后的Additional Pass中不会再计算这个部分
		fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

		fixed3 worldNormal = normalize(i.worldNormal);
		fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

		//计算漫反射光照  
		fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal,worldLightDir));

		//获取视角方向 = 摄像机的世界坐标 - 顶点的世界坐标  
		fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
		//计算新矢量h  
		fixed3 halfDir = normalize(viewDir + worldLightDir);
		//计算高光光照  
		fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(worldNormal,halfDir)),_Gloss);

		//平行光的衰减值为1
		fixed atten = 1.0;

		//使用内置宏计算阴影值
		fixed shadow = SHADOW_ATTENUATION(i);

		return fixed4(ambient +(diffuse+specular) * atten * shadow,_Alpha);

	}
		ENDCG
	}

//Additional Pass,渲染其他光源
//	Pass
//	{
//		//指明光照模式为前向渲染模式
//		Tags{ "LightMode" = "ForwardAdd" }
//
//		//开启混合模式，将计算结果与之前的光照结果进行叠加
//		Blend srcalpha oneminussrcalpha
//
//		CGPROGRAM
//#pragma vertex vert  
//#pragma fragment frag  
//		//确保光照衰减等光照变量可以被正确赋值
//#pragma multi_compile_fwdadd
//
//		//包含引用的内置文件  
//#include "Lighting.cginc"  
//
//		//声明properties中定义的属性  
//		fixed4 _Diffuse;
//	    fixed4 _Specular;
//	    float _Gloss;
//
//	//定义输入与输出的结构体  
//	struct a2v
//	{
//		float4 vertex : POSITION;
//		float3 normal : NORMAL;
//	};
//
//	struct v2f
//	{
//		float4 pos : SV_POSITION;
//		//存储世界坐标下的法线方向和顶点坐标  
//		float3 worldNormal : TEXCOORD0;
//		float3 worldPos : TEXCOORD1;
//	};
//
//	//在顶点着色器中，计算世界坐标下的法线方向和顶点坐标，并传递给片元着色器  
//	v2f vert(a2v v)
//	{
//		v2f o;
//		//转换顶点坐标到裁剪空间  
//		o.pos = UnityObjectToClipPos(v.vertex);
//		//转换法线坐标到世界空间，直接使用_Object2World转换法线，不能保证转换后法线依然与模型垂直  
//		o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
//		//转换顶点坐标到世界空间  
//		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
//		return o;
//	}
//
//	//在片元着色器中计算光照模型  
//	fixed4 frag(v2f i) : SV_Target
//	{
//	fixed3 worldNormal = normalize(i.worldNormal);
//
//	//计算不同的光源方向
//#ifdef USING_DIRECTIONAL_LIGHT
//	fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
//#else
//	fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
//#endif
//	
//
//	//计算漫反射光照  
//	fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal,worldLightDir));
//
//	//获取视角方向 = 摄像机的世界坐标 - 顶点的世界坐标  
//	fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
//	//计算新矢量h  
//	fixed3 halfDir = normalize(viewDir + worldLightDir);
//	//计算高光光照  
//	fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(saturate(dot(worldNormal,halfDir)),_Gloss);
//
//	//处理不同的光源衰减
//#ifdef USING_DIRECTIONAL_LIGHT
//	fixed atten = 1.0;
//#else
//	/*float3 lightCoord = mul(unity_WorldToLight, float4(i.worldPos, 1)).xyz;
//	fixed atten = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).UNITY_ATTEN_CHANNEL;*/
//	float distance = length(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
//	//线性衰减
//	fixed atten = 1.0 / distance;
//#endif
//
//	return fixed4((diffuse + specular) * atten,1.0);
//
//	}
//		ENDCG
//	}
	}

		Fallback"VertexLit"
}