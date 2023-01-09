Shader "Flow"
{

    Properties{
        //Properties
  
        _MainTex ("MainTex", 2D) = "white" {}
        _XSpeed ("Flow Speed",float) = -0.2
        _Color ("MainCol",Color) = (1,1,1,1)

        //MASK SUPPORT ADD
        _StencilComp ("Stencil Comparison", Float)=8
        _Stencil ("Stencil ID", Float)=0
        _StencilOp ("Stencil Operation", Float)=0
        _StencilWriteMask ("Stencil Write Mask", Float)=255
        _StencilReadMask ("Stencil Read Mask", Float)=255
        _ColorMask ("Color Mask", Float)=15
        //MASK SUPPORT END
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        //MASK SUPPORT ADD
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        ColorMask [_ColorMask]
        //MASK SUPPORT END

        Pass
        {   
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                fixed2 uv = i.uv;

                [unroll(100)]
                for (fixed i = 1.0; i < 5.0; i++)
                {   
                    uv.x += 0.6 / i * cos(i * 2.5 * uv.y + _Time.y * _XSpeed);
                    uv.y += 0.6 / i * cos(i * 1.5 * uv.x + _Time.y * _XSpeed);
                }
                
                fixed4 col = fixed4(mainTex.xyz / abs(sin(_Time.y * _XSpeed - uv.y - uv.x )), _Color.a);


                col.xyz = _Color.a;
                col.xyz *= _Color.xyz;
                col.a *= _Color.a;

                return col; 
            }
            ENDCG
        }
    }
}