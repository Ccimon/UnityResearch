// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "shader3d/cartoon" {

Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Outline("Outline",float) = 0.1
        _OutlineColor("OutlineColor",Color) = (0,0,0,1)
        _Specluar("Specular",Range(1,32)) = 1
        _Metallic("Metallic",float) = 0.5
        _Glossiness("Glossiness",float) = 0.5
        _FrontCoe("FrontCoefficient",float) = 1
        _BackCoe("BackCoefficient",Range(0,1)) = 0.75
        _AmbientFactor("AmbientFactor",Range(0,1)) = 0.5
        _LightBorder("LightBorder",float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        //描边阶段，法线外扩，渲染背面
        Pass
        {
            //只需要边缘外扩
            Cull Front
            ZWrite Off
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members pos)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            float _Outline;
            float4 _OutlineColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
                normal.x *= UNITY_MATRIX_P[0][0];
                normal.y *= UNITY_MATRIX_P[1][1];
                o.vertex.xy += normal.xy * _Outline;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
        //正常阶段
        Pass
        {
            Cull Back
            CGPROGRAM
            // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members pos)
            #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            int _Specluar;
            float _Metallic;
            float _Glossiness;
            float _LightBorder;
            float _FrontCoe;
            float _BackCoe;
            float _AmbientFactor;
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv :TEXCOORD0;
                fixed3 normal:NORMAL;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv:TEXCOORD0;
                fixed3 normal:NORMAL;
                float3 pos:TEXCOORD1;
            };
            
            sampler2D _MainTex;     
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                // 法线从模型空间转换到世界空间
                o.normal = UnityObjectToWorldNormal(v.normal);
                // 坐标也从模型空间转到世界空间
                o.pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex,i.uv);
                fixed3 nor = i.normal;
                fixed3 light = normalize(_WorldSpaceLightPos0.xyz);
                
                // fixed3 cameraPos = _WorldSpaceCameraPos.xyz;
                // fixed3 faceDir = normalize(cameraPos - i.pos);
                // fixed3 hightLight = normalize(faceDir + light);
                //
                // float specular = max(0,dot(hightLight,nor));
                // fixed3 specColor = saturate(pow(specular,_Specluar)) * fixed3(1,1,1);
                //
                // float diff = saturate(dot(nor,light));
                // fixed3 ambient = _AmbientFactor * UNITY_LIGHTMODEL_AMBIENT.xyz * color;
                // color.rgb = color.rgb * diff + specColor + ambient; 
                //
                // return color;

                
                // 漫反射实现
                // diff是由顶点的法线世界向量与光照向量点乘获得
                // 如果小于0，说明与光照的夹角大于180度，及该顶点面朝方向与光照相背，及该点为阴影处
                // 相反则处于光照下
                float diff = dot(nor,light);
                float para = diff > _LightBorder ? _FrontCoe : _BackCoe;
                // 获取环境光
                color.rgb += UNITY_LIGHTMODEL_AMBIENT.xyz * color.rgb;
                // rgb值强制归一 
                color.rgb = saturate(color.rgb);
                // 乘以前后渲染的颜色系数，实现卡通渲染
                color.rgb *= para;
                return color;
            }

            //    //高斯模糊 vert shader
	        // v2f_blur vert_blur(appdata_img v)
	        // ｛
		       //  v2f_blur o;
		       //  _offsets *= _MainTex_TexelSize.xyxy;
		       //  o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		       //  o.uv = v.texcoord.xy;
		       //  o.uv01 = v.texcoord.xyxy + _offsets.xyxy * float4(1, 1, -1, -1);
		       //  o.uv23 = v.texcoord.xyxy + _offsets.xyxy * float4(1, 1, -1, -1) * 2.0;
		       //  o.uv45 = v.texcoord.xyxy + _offsets.xyxy * float4(1, 1, -1, -1) * 3.0;
		       //  return o;
	        // ｝
	        // //高斯模糊 pixel shader
	        // fixed4 frag_blur(v2f_blur i) : SV_Target
	        // ｛
		       //  fixed4 color = fixed4(0,0,0,0);
		       //  color += 0.40 * tex2D(_MainTex, i.uv);
		       //  color += 0.15 * tex2D(_MainTex, i.uv01.xy);
		       //  color += 0.15 * tex2D(_MainTex, i.uv01.zw);
		       //  color += 0.10 * tex2D(_MainTex, i.uv23.xy);
		       //  color += 0.10 * tex2D(_MainTex, i.uv23.zw);
		       //  color += 0.05 * tex2D(_MainTex, i.uv45.xy);
		       //  color += 0.05 * tex2D(_MainTex, i.uv45.zw);
		       //  return color;
	        // ｝
            ENDCG
        }
    }
    FallBack "Diffuse"  
}