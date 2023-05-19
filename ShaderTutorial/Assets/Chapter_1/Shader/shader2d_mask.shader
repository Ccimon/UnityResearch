Shader "Tutorial2D/shader2d_mask"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
		// 反向遮罩开关，开启之后，遮罩将被渲染，主纹理将成为遮罩
		[Toggle]
		_MaskRevert("Mask Revert",int) = 0
	} 
 
	SubShader
	{
		Tags
		{ 
			"IgnoreProjector"="True" 
			"RenderType"="TransParent" 
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
			fixed4 _Color;
			int _MaskRevert;

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
				o.mask_uv = i.uv;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				// 主纹理采样
				fixed4 col = tex2D(_MainTex,i.uv);
				// 遮罩纹理采样
				fixed4 mask = tex2D(_MaskTex,i.mask_uv);
				// 不开启反向遮罩的时候，渲染主纹理的颜色，并与遮罩纹理的alpha值相减，进行遮罩计算
				// 开启反向遮罩的时候，渲染主纹理的颜色，因为既要保存主纹理的alpha值又要根据遮罩纹理的alpha值进行蒙版，所以使用两个alpha相乘
				col = float4(col.rgb,col.a - mask.a) * (1 - _MaskRevert) + float4(col.rgb,mask.a * col.a) * _MaskRevert; 
				return col;
			}
		ENDCG
		}

	}
}