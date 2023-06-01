Shader "Tutorial2D/shader2d_default"
// Shader命名以及路径索引
{
    // Shader面向Inspector的可视化编辑属性
    Properties
    {
        // Inspector面板可显示属性，在Inspector修改可以同步给Shader
        _Color("MainColor",Color) = (1,1,1,1)

    }
    
    // 一个Shader脚本可以有多个Shader
    SubShader
    {
        // 每一个SubShader可以有很多的可配置选项，放在Pass之前
        Tags { "RenderType"="TransParent" }

        Pass
        {
            // CGPROGRAM 意味着接下来的内容都是由CG语法提供的代码环境，其符合C语言规范
            CGPROGRAM
            // #pragma是宏编译 是UnityShader提供给我们的接口 以方便接入到Unity内置的各种流程中
            // #pragma vertex *** 向Unity指定了我们的顶点渲染函数
            // #pragma frag *** 向Unity指定了我们的片面渲染函数
            #pragma vertex vert
            #pragma fragment frag

            // 代码所依赖的库
            #include "UnityCG.cginc"

            // 顶点函数的输入结构体，包含顶点函数所需要的信息
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            // 片面函数的输入结构体，同时也是顶点函数的输出结构体，包含片面函数所需要的信息
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // 这里引入Inspector的_Color信息，要注意同名
            float4 _Color;

            // 顶点渲染函数，对每一个顶点执行该函数
            v2f vert (appdata v)
            {
                v2f o;
                // 坐标的空间转换，顶点的坐标所处的空间最开始处在模型空间，可以直观的理解为模型的本地坐标系
                // UnityObjectToClipPos，也就是Unity物体转换到裁剪空间位置
                // 裁剪空间，顾名思义，渲染管线的裁剪流程，也就是在这里处理我们的物体遮盖，将不可见的部分裁去
                // 裁剪空间之后，就是几何空间，也就是光栅化，将3D的模型转换为屏幕上的像素点
                // 在此之后就要进入到我们的片面渲染函数，就是一个个像素进行处理
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // 片面渲染函数，对每个像素点执行该函数，比如手机屏幕是1920*1080，那就会对2073600个像素都执行这个函数
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = float4(0,0,1,1);
                // 在这里返回颜色，fixed其实是比float精度更低的浮点数
                return col;
            }
            ENDCG
        }
    }
}
