Shader "shader2d/Flow"
{

    Properties{
        //Properties
  
        _MainTex ("MainTex", 2D) = "white" {}
        _XSpeed ("Flow Speed",float) = -0.2
        _Color ("Color",Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Pass
        {   
	        Cull Off         //关闭背面剔除
	        Lighting Off     //关闭灯光
	        ZWrite Off       //关闭Z缓冲
	        Blend One OneMinusSrcAlpha   

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
                fixed4 tangent : TANGENT;
                fixed3 normal : NORMAL;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float _XSpeed;
            fixed4 _Color;

            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            //Variables

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(VertexOutput i) : SV_Target
            {
                fixed4 mainTex = tex2D(_MainTex, i.uv);
                fixed2 uv = (4.0 * i.uv - 3) / min(1, 1);

                [unroll(100)]
                for (fixed i = 1.0; i < 10.0; i++)
                {   
                    uv.x += 0.6 / i * cos(i * 2.5 * uv.y + _Time.y * _XSpeed);
                    uv.y += 0.6 / i * cos(i * 1.5 * uv.x + _Time.y * _XSpeed);
                }
                
                fixed4 col = fixed4(mainTex.xyz / abs(sin(_Time.y - uv.y - uv.x)), _Color.a);
                
                col.rbg *= _Color.a;
                
                col.a = _Color.a;
                
                return col; 
            }
            ENDCG
        }
    }
}