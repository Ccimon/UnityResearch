Shader "shader2d/blink"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("BlinkColor",Color) = (1,1,1,1)
        _Process ("BlinkPorcess",Range(0,1)) = 1.0
        [MaterialToggle] _IsBlink ("IsBlink",float) = 1.0
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

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;

                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            half _Process;
            float _IsBlink;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= col.a;
                if(_IsBlink)
                {
                    col.rgb = col.rgb * (1 - min(_Process,1)) + _Color.rgb * _Process;
                    return col;
                }

                return col;
            }
            ENDCG
        }
    }
}
