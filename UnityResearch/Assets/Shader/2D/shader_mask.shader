Shader "shader2d/mask"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1) //图像回合颜色
        _Location ("Location", Vector) = (0.5,0.5,1,1)
        _MainTexSize ("MainTexSize", Vector) = (1,1,1)
        _MaskTexSize ("MaskTexSize", Vector) = (1,1,1)
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
 
		Cull Off         //关闭背面剔除
		Lighting Off     //关闭灯光
		ZWrite Off       //关闭Z缓冲
		Blend One OneMinusSrcAlpha     //混合源系数one(1)  目标系数OneMinusSrcAlpha(1-one=0)
 
		Pass
		{
		ZTest Greater
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON       //告诉Unity编译不同版本的Shader,这里和后面vert中的PIXELSNAP_ON对应
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			
            sampler2D _MaskTex;
			fixed4 _Color;
            float4 _Location;
            float4 _MainTexSize;
            float4 _MaskTexSize;

			struct appdata_t                           //vert输入
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
 
			struct v2f                                 //vert输出数据结构
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
 
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
 
				return OUT;
			}
 
			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				return color;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                float2 border = float2(_MaskTexSize.x/_MainTexSize.x/2,_MaskTexSize.y/_MainTexSize.y/2);

                float right = _Location.x + border.x;
                float left = _Location.x - border.x;
                float top = _Location.y + border.y;
                float bottom = _Location.y - border.y;
                float2 pos = IN.texcoord;

                if(pos.x < right && pos.x > left && pos.y > bottom && pos.y < top){
                    
                    pos.y -= bottom;
                    pos.x -= left;
                    pos.x = pos.x / border.x /2;
                    pos.y = pos.y / border.y /2;
                    fixed4 co = tex2D(_MaskTex,pos);
                    // return fixed4(pos.x * pos.y,0,0,1);

                    if(co.a >= 0.02){
                        return fixed4(0,0,0,0);
                    }else{
                        return c;
                    }
                }
				return c;
			}
		ENDCG
		}

		Pass{
		ZTest LEqual
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON       //告诉Unity编译不同版本的Shader,这里和后面vert中的PIXELSNAP_ON对应
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			
            sampler2D _MaskTex;
			fixed4 _Color;
            float4 _Location;
            float4 _MainTexSize;
            float4 _MaskTexSize;

			struct appdata_t                           //vert输入
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
 
			struct v2f                                 //vert输出数据结构
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
 
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
 
				return OUT;
			}
 
			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				return color;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                float2 border = float2(_MaskTexSize.x/_MainTexSize.x/2,_MaskTexSize.y/_MainTexSize.y/2);

                float right = _Location.x + border.x;
                float left = _Location.x - border.x;
                float top = _Location.y + border.y;
                float bottom = _Location.y - border.y;
                float2 pos = IN.texcoord;

                if(pos.x < right && pos.x > left && pos.y > bottom && pos.y < top){
                    
                    pos.y -= bottom;
                    pos.x -= left;
                    pos.x = pos.x / border.x /2;
                    pos.y = pos.y / border.y /2;
                    fixed4 co = tex2D(_MaskTex,pos);
                    // return fixed4(pos.x * pos.y,0,0,1);

                    if(co.a >= 0.02){
                        return fixed4(0,0,0,0);
                    }else{
                        return c;
                    }
                }
				return c;
			}
		ENDCG
		}
	}
}