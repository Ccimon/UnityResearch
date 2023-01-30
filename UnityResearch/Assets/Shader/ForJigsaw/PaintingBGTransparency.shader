Shader "MyShader/TestPaintingBGTransparency" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
//		_Light("Light", Float) = 3
//		_Dis("Dis", Float) = 3
//		_Alpha("Alpha", Float) = 3
//		_ReplaceColor("Replace (RGB)",Color) = (1,1,1,1)
//		_FinalColor("Final (RGB)",Color) = (1,1,1,1)
		_HighLitghtFilter("HighLight FilterColor",Range(0,1.73)) = 0.5
		_FilterfRedPass("Shadow FilterRed",Range(0,1)) = 1
		_LightAlpha("LightAlpha",Range(0,1)) = 1
		_ShadowAlpha("ShadowAlpha",Range(0,3)) = 1

	}
		SubShader{
			Tags {"Queue" = "Transparent"}
			Blend SrcAlpha OneMinusSrcAlpha

			pass
			{
				CGPROGRAM
				#pragma vertex vertext_convert
				#pragma fragment fragment_convert
				#include "UnityCG.cginc" 
 
				sampler2D  _MainTex;
				sampler2D  _MainTex1;
				// float _Light;
				// float _Dis;
				// float _Alpha;
				float _HighLitghtFilter;
				float  _FilterfRedPass;
				float _LightAlpha;
				float _ShadowAlpha;
				// float4  _ReplaceColor;
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
 
				// float ColorLerp(float3 tmp_nowcolor,float3 tmp_FilterfColor)
				// {
				// 	float3 dis = float3(abs(tmp_nowcolor.x - tmp_FilterfColor.x),abs(tmp_nowcolor.y - tmp_FilterfColor.y),abs(tmp_nowcolor.z - tmp_FilterfColor.z));
				// 	float dis0 = length(dis);
				// 	float maxdis = sqrt(_Dis);
				// 	float dis1 = lerp(0,maxdis,dis0);
				// 	return dis1;
				// }

				// fixed4 第一位返回是否执行颜色替换，并执行透明处理
				// 第二位表示是否执行纯白色高光处理，否则执行阴影处理
				fixed2 ClipAndRelpalce(float4 localColor,float4 filterColor)
				{
					float len = length(localColor.gb);
					if(len > _HighLitghtFilter)
					{
						// 高光处理
						return fixed2(0,0);
					}
					
					if(localColor.r >= _FilterfRedPass)
					{
						// 纯红色替换
						return fixed2(1,0);
					}
					
					// 返回原色
					return fixed2(0,1);
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
					clip(c.a < 0);
					float2 flag = ClipAndRelpalce(c, _FilterfRedPass);
					// flag的第一位代表是否是纯红色，是的话这里就走重新设置Alpha值为0，高光和阴影Alpha值默认为一并且受_Alpha参数影响
					if(c.r > 0.01)
					{
						c.a *= flag.x >= 1 ? (1 - flag.x) * (1 - c.r) * c.a : (1 - flag.x);
					}else
					{
						c.a = 1 * c.r;
					}
					// flag的第二位用以区分高光和阴影，为0时时高光，这里rgb = (1,1,1)，为1时阴影，rgb = (0,0,0)
					c.rgb = float3(1-flag.y,1-flag.y,1-flag.y);

					c.a *= flag.y >= 1 ?  _ShadowAlpha : _LightAlpha;
					
					return c;
				}
 
 
			ENDCG
			}
		}
			FallBack "Diffuse"
}