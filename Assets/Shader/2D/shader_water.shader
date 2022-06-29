Shader "shader2d/water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Texture", 2D) = "white" {}
        _MaskBottom ("Bottom", float) = 0
        _MaskTop ("Top", float) = 0.1
        _MultiX ("MultiX", float) = 8
        _MultiY ("MultiY", float) = 0.2
        _AddY ("AddY", Range(0,1)) = 0.15
        _MultiTime ("MultiTime",float) = 1
        [Toggle(WaterAnimate)]_IsOpen ("WaterAnimate", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                // UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;
            float _MultiX;
            float _MultiY;
            float _AddY;
            bool _IsOpen;
            float _MultiTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mcol = tex2D(_MaskTex, i.uv);
                
                if(mcol.a < 0.1 || i.uv.y > _MaskTop || i.uv.y < _MaskBottom){
                    discard;
                }

                if(_IsOpen > 0 && _AddY < 1){
                    float minLerp = 0.3;
                    float x = i.uv.x + _Time.y * _MultiTime;
                    float lerp = min((1 - _AddY),minLerp) * _MultiY;
                    float cory = max(_AddY , lerp);
                    if(i.uv.y > (sin(x * _MultiX) * lerp) + cory){
                        discard;
                    }
                }
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
