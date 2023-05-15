Shader "Tutorial2D/shader2d_mask"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
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
 
		Cull Off         //关闭剔除
		Lighting Off     //关闭灯光
		ZWrite Off       //关闭Z缓冲
		Blend SrcAlpha OneMinusSrcAlpha
 
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
		
			sampler2D _MainTex;
            sampler2D _MaskTex;
			float4 _MaskTex_ST;
			fixed4 _Color;

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 uv : TEXCOORD0;
			};
 
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 uv  : TEXCOORD0;
				float2 mask_uv: TEXCOORD1;
			};
 
			v2f vert(appdata_t i)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(i.vertex);
				o.uv = i.uv;
				o.mask_uv = TRANSFORM_TEX(i.uv,_MaskTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				// 主纹理采样
				fixed4 col = tex2D(_MainTex,i.uv);
				// 遮罩纹理采样
				fixed4 mask = tex2D(_MaskTex,i.mask_uv);
				// 两者采集到的透明度相减，也就是遮罩不透明的地方，会挖空主纹理，这样就实现了遮罩/镂空的效果
				col.a = col.a - mask.a;
				return col;
			}
		ENDCG
		}

	}
}