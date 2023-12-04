using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AfterEffect_Outline : PostEffectsBase
{
    public Material AE_mat;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture screen = RenderTexture.GetTemporary(src.width, src.height);
        Graphics.Blit(src,dest,AE_mat);
        
        RenderTexture.ReleaseTemporary(screen);
    }
}
