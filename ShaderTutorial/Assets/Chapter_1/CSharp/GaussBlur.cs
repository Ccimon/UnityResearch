using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 后处理实现全屏模糊
/// </summary>
public class GaussBlur : MonoBehaviour
{

    private Material _curMat;
    [SerializeField()]
    private Shader _curShader;

    public int DownSampleValue = 2;
    public float BlurSpreadSize = 2;
    public float BlurIterations = 3;
    
    const string SHADER_NAME = "Tutorial2D_AfterEffect/shader_gaussblur";
    private const string SHADER_PARAM_DOWNSAMPLE = "_DownSampleNum";
    
    void OnEnable()
    {
        // 创建屏幕渲染用的材质球
        _curMat = new Material(_curShader);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_curMat != null)
        {
            // 设置材质球属性
            float width = 1.0f / (1.0f * (1 << DownSampleValue));
            _curMat.SetFloat(SHADER_PARAM_DOWNSAMPLE,BlurSpreadSize * width);
            src.filterMode = FilterMode.Bilinear;
            
            // 对纹理的宽高进行降采样计算 这里相当于进行2的n次幂除法运算 降低纹理精度
            int renderWidth = src.width >> DownSampleValue;
            int renderHeight = src.height >> DownSampleValue;
            
            // 对屏幕纹理进行第一遍渲染
            RenderTexture renderBuffer = BlitTargetTexture(src,renderWidth,renderHeight,src.format,0);

            for (int i = 0; i < BlurIterations; i++)
            {
                // 对纹理进行 Y轴的渲染模糊
                RenderTexture tempBuffer = BlitTargetTexture(renderBuffer,renderWidth,renderHeight,src.format, 1);
                // 对纹理进行 X轴的渲染模糊
                tempBuffer = BlitTargetTexture(renderBuffer,renderWidth,renderHeight,src.format, 2);
                // 释放renderBuffer的像素缓存
                RenderTexture.ReleaseTemporary(renderBuffer);
                // 将经过材质球渲染后的结果给renderBuffer
                renderBuffer = tempBuffer;
            }
            
            // 混合模糊后的纹理与背景纹理
            Graphics.Blit(renderBuffer,dest);
            // 释放纹理
            RenderTexture.ReleaseTemporary(renderBuffer);
        }
        else
        {
            Graphics.Blit(src,dest);
        }
    }

    RenderTexture BlitTargetTexture(RenderTexture src,int renderWidth,int renderHeight,RenderTextureFormat format,int passIndex)
    {
        // 获取缓存 设置过滤模式，一般都用线性模式，在纹理缩放的时候这个设置会影响渲染
        RenderTexture tempTexture = RenderTexture.GetTemporary(renderWidth,renderHeight,1,format);
        tempTexture.filterMode = FilterMode.Bilinear;
        // 对屏幕纹理进行指定材质的指定通道渲染
        Graphics.Blit(src,tempTexture,_curMat,passIndex);
        return tempTexture;
    }
}
