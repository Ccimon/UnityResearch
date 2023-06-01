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
    private Shader _curShader;

    public int DownSampleValue = 2;
    public float BlurSpreadSize = 2;
    public float BlurIterations = 3;
    
    const string SHADER_NAME = "Learning Unity Shader/Lecture 15/RapidBlurEffect";
    private const string SHADER_PARAM_DOWNSAMPLE = "_DownSampleNum";
    
    void Start()
    {
        _curShader = Shader.Find(SHADER_NAME);
        _curMat = new Material(_curShader);
        Debug.Log("Prepare Material");
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Debug.Log("RenderImage");
        if (_curMat != null)
        {
            float width = 1.0f / (1.0f * (1 << DownSampleValue));
            _curMat.SetFloat(SHADER_PARAM_DOWNSAMPLE,BlurSpreadSize * width);
            src.filterMode = FilterMode.Bilinear;

            int renderWidth = src.width >> DownSampleValue;
            int renderHeight = src.height >> DownSampleValue;

            RenderTexture renderBuffer = BlitTargetTexture(src,renderWidth,renderHeight,src.format,0);

            for (int i = 0; i < BlurIterations; i++)
            {
                RenderTexture tempBuffer = BlitTargetTexture(renderBuffer,renderWidth,renderHeight,src.format, 1);
                tempBuffer = BlitTargetTexture(renderBuffer,renderWidth,renderHeight,src.format, 2);
                RenderTexture.ReleaseTemporary(renderBuffer);
                renderBuffer = tempBuffer;
            }
            Graphics.Blit(renderBuffer,dest);
            RenderTexture.ReleaseTemporary(renderBuffer);
        }
        else
        {
            Graphics.Blit(src,dest);
        }
    }

    RenderTexture BlitTargetTexture(RenderTexture src,int renderWidth,int renderHeight,RenderTextureFormat format,int passIndex)
    {
        RenderTexture tempTexture = RenderTexture.GetTemporary(renderWidth,renderHeight,1,format);
        tempTexture.filterMode = FilterMode.Bilinear;
        Graphics.Blit(src,tempTexture,_curMat,passIndex);
        return tempTexture;
    }
}
