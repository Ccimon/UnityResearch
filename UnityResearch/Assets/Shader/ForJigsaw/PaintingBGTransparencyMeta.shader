Shader "MyShader/PaintingBGTransparency" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ContentTex("ContentTexture", 2D) = "white" {}
		_Light("Light", Float) = 3
		_Dis("Dis", Float) = 3
		_Alpha("Alpha", Float) = 3
		// uvinfo前两位是UV采集的起点，后两位是UV采集的百分比
		_UvInfo("UVStartPoint",Vector) = (1,1,1,1)
		_FilterfColor("Ridof (RGB)",Color) = (1,1,1,1)
		_ReplaceColor("Replace (RGB)",Color) = (1,1,1,1)
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			Blend SrcAlpha OneMinusSrcAlpha
			pass
			{
				CGPROGRAM
 
				#pragma vertex vertext_convert
				#pragma fragment fragment_convert
				#include "UnityCG.cginc" 
 
				sampler2D  _MainTex;
				sampler2D  _ContentTex;
				float _Light;
				float _Dis;
				float _Alpha;
				float4 _UvInfo;
				float4  _FilterfColor;
				float4  _ReplaceColor;
				
				struct Inputvrite
				{
					float4 vertex : POSITION;
					float4 texcoord : TEXCOORD0;
				};
				struct Inputfragment
				{
					float4 pos : SV_POSITION;
					float4 uv : TEXCOORD0;
				};
 
				float ColorLerp(float3 tmp_nowcolor,float3 tmp_FilterfColor)
				{
					float3 dis = float3(abs(tmp_nowcolor.x - tmp_FilterfColor.x),abs(tmp_nowcolor.y - tmp_FilterfColor.y),abs(tmp_nowcolor.z - tmp_FilterfColor.z));
					float dis0 = sqrt(pow(dis.x,_Light) + pow(dis.y,_Light) + pow(dis.z,_Light));
					float maxdis = sqrt(_Dis);
					float dis1 = lerp(0,maxdis,dis0);
					return dis1;
				}
 
				Inputfragment vertext_convert(Inputvrite i)
				{
					Inputfragment o;
					o.pos = UnityObjectToClipPos(i.vertex);
					o.uv = float4(i.texcoord.xy,1,1);
					return o;
				}
 
				float4 fragment_convert(Inputfragment o) : COLOR
				{
					float4 c = tex2D(_MainTex,o.uv);
					float2 contentUV = float2(_UvInfo.x + o.uv.x * _UvInfo.z,_UvInfo.y + o.uv.y * _UvInfo.w);
					float4 content = tex2D(_ContentTex,contentUV);
					float4 col = c;
					c.a *= ColorLerp(c.rgb,_FilterfColor.rgb);
					
					c.rgb = c.rgb * (1 - col.r) + content.rgb * col.r;
					c.rgb *= length(col.rgb);
					c.a *= _Alpha;
					return c;
				}
 
			ENDCG
			}
		}
			FallBack "Diffuse"
}