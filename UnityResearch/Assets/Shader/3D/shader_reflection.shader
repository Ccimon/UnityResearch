Shader "shader3d/shader_reflection"
{
    Properties
    {
        _Color ("Color Tint",Color) = (1,1,1,1)
        _ReflectColor ("Reflection Color",Color) = (1,1,1,1)
        _ReflectAmount ("Reflect Amount",Range(0,1)) = 1
        _Cubemap ("Reflection Cubemap",Cube) = "_Skybox"{}
    }
    SubShader
    {
        Tags { "RenderType"="TransParent" "Queue"="Geometry"}
        
        Pass
        {
            Tags{"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include  "AutoLight.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                
            };

            struct v2f
            {
                float3 worldPos : TEXCOORD0;
                fixed3 worldNormal : TEXCOORD1;
                fixed3 worldViewDir : TEXCOORD2;
                fixed3 worldReflect : TEXCOORD3;
                float4 vertex : SV_POSITION;
                SHADOW_COORDS(4)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _ReflectColor;
            fixed _ReflectAmount;
            samplerCUBE _Cubemap;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld , v.vertex).xyz;
                o.worldViewDir = UnityWorldSpaceViewDir(o.worldPos);
                o.worldReflect = reflect(-o.worldViewDir,o.worldNormal);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 no = normalize(i.worldNormal);
                fixed3 ld = normalize(UnityWorldSpaceLightDir(i.worldPos));
                fixed3 vd = normalize(i.worldViewDir);

                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                fixed3 diffuse = _LightColor0.rgb * _Color.rgb * max(0,dot(no,ld));
                fixed3 reflect = texCUBE(_Cubemap,i.worldReflect).rgb * _ReflectColor.rgb;

                UNITY_LIGHT_ATTENUATION(atten,i,i.worldPos);

                fixed3 color = ambient + lerp(diffuse,reflect,_ReflectAmount) * atten;
                
                return fixed4(color,1.0);
            }
            ENDCG
        }
    }
}
