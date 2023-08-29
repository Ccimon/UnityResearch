//参考 https://mp.weixin.qq.com/s/-ukjq_pJqCCAYHQEcmzO3w 
Shader "AFei/MatcapGlass"
{
	Properties
	{
		_MainCol ("Main Color", Color) = (1,1,1,1)
		_NormalTex ("NormalTex", 2D) = "bump" {}
		[HDR]_SpecularColor("SpecularColor",Color) = (1,1,1,1)
		_MatCapTex("MapCapTex",2D) = "white"{}
		_RefMatCapTex("RefMapCapTex",2D) = "white"{}
		_ThicknessTex ("Thickness", 2D) = "white" {}
		_ReflectionIntensity("_ReflectionIntensity",Range(0,1.0))=0.1
		_EdgeThickness("Edge Thickness",Range(0,1.0)) = 0.2
	}
	SubShader
	{

		Pass
		{
			Tags { "RenderType"="Transparent" "Queue" ="Transparent"    "LightMode" = "ForwardBase"}
			LOD 100
			Blend SrcAlpha OneMinusSrcAlpha
			Zwrite off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#include "AutoLight.cginc"
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal:NORMAL0;
				float4 tangent:TANGENT0;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 vertex :POSITION;
				float3 worldNormal:NORMAL0;
				float3 viewPos:TEXCOORD1;
				float3 viewDir:TEXCOORD2;
				float3 worldTangent:TEXCOORD3;
				float3 worldBinormal:TEXCOORD4;
				SHADOW_COORDS(5)
			};

			sampler2D _NormalTex;
			float4 _NormalTex_ST;
			float4 _MainCol;
			float4 _SpecularColor;
			float _ReflectionIntensity;
			float _EdgeThickness;
			sampler2D _MatCapTex;
			sampler2D _RefMatCapTex;
			sampler2D _ThicknessTex;
			float4 _ThicknessTex_ST;

			 float2 MatCapUV (in float3 N,in float3 viewPos)
			 {
					float3 viewNorm = mul((float3x3)UNITY_MATRIX_V, N);
					float3 viewDir = normalize(viewPos);
					float3 viewCross = cross(viewDir, viewNorm);
					viewNorm = float3(-viewCross.y, viewCross.x, 0.0);
					float2 matCapUV = viewNorm.xy * 0.5 + 0.5;
					return matCapUV; 
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _NormalTex);
				o.uv.zw = TRANSFORM_TEX(v.uv, _ThicknessTex);
				TRANSFER_SHADOW(o);
				o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
				o.viewPos = UnityObjectToViewPos(v.vertex);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz-mul(unity_ObjectToWorld,v.vertex).xyz);

				o.worldTangent = normalize(UnityObjectToWorldNormal(v.tangent));
				o.worldBinormal = cross(o.worldNormal,o.worldTangent)*v.tangent.w;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col;

				fixed4 normal = tex2D(_NormalTex,i.uv.xy);
				fixed3 normalUnpack = normalize(UnpackNormal(normal));
				float3x3  tbn = float3x3 (i.worldTangent,i.worldBinormal,i.worldNormal);
				i.worldNormal = normalize(mul(normalUnpack,tbn));
				
				fixed thickness = tex2D(_ThicknessTex,i.uv.zw).r;
				thickness +=pow(1.0-dot(i.viewDir,i.worldNormal),(1.0-_EdgeThickness)*11);

				float2 matcapUV = MatCapUV(i.worldNormal,i.viewPos);
				//高光颜色
				fixed4 matcapCol =tex2D(_MatCapTex,matcapUV);
				//折射颜色
				fixed3 refCol =tex2D(_RefMatCapTex,matcapUV+thickness*_ReflectionIntensity);

				col.rgb = refCol*_MainCol + matcapCol*_SpecularColor;

				col.a = matcapCol.r+thickness;

				 float shadow = SHADOW_ATTENUATION(i);

				return col*shadow ;
			}
			ENDCG
		}

		Pass 
		{

		Name "ShadowCaster"
		Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
			#include "UnityCG.cginc"

			struct v2f {
				V2F_SHADOW_CASTER;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
	//Fallback"Specular"
}
