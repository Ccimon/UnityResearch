using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMask : MonoBehaviour
{
    public Image ImgMask;

    private void Start()
    {
        if (ImgMask != null)
        {
            SetMask(ImgMask.sprite.texture);
        }
    }

    public void SetMask(Texture mask)
    {
        Image selfImg = GetComponent<Image>();
        selfImg.material.SetTexture("_MaskTex",mask);
    }
}
