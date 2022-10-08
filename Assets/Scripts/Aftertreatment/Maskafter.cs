using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Image))]
public class Maskafter : MonoBehaviour
{

    //对外开放的水位属性，每次修改都会刷新Shader渲染
    public float WaterLine {
        get =>_waterLine;

        set => SetWaterLine(value);
    }
    
    private int MaxBlock = 4;
    
    private Image img;
    
    //水位
    private float _waterLine = 3;
    //动画水位
    private float _animLine = 0;
    //水位增长速度
    private float _speed = 0.0125f;
    //可用的Shader参数名
    private string _paramAddY = "_AddY";
    private string _paramIsOpen = "_IsOpen";
    private string _paramMaskTop = "_MaskTop";
    private string _paramMaskBottom = "_MaskBottom";
    private string _paramColor = "_Color";
    private string _paramMaskTexture = "_MaskTex";
    private string _paramMultiX = "_MultiX";
    private string _paramMultiY = "_MultiY";

    private float _addY;
    private int _isOpen;
    private float _maskTop;
    private float _maskBot;
    private Color _color;
    private Texture2D _maskTex;
    private float _multiY;
    private float _multiX;

    #region Inspector面板

    //遮罩图
    public Sprite MaskTexture;
    //用于渲染水面的材质
    public Material WaterMat;

    public float WaterFrequence;

    public float WaterAmplitude;

    public float PaddingTop;
    #endregion

    #region 公有变量

    public float Frequence
    {
        get => _multiX;
        set
        {
            _multiX = value;
            WaterMat.SetFloat(_paramMultiX,value);
        }
    }
    
    public float Amplitude
    {
        get => _multiY;
        set
        {
            _multiY = value;
            WaterMat.SetFloat(_paramMultiY,value);
        }
    }
    
    public float AddY
    {
        get => _addY;
        set
        {
            _addY = value;
            WaterMat.SetFloat(_paramAddY,value);
        }
    }
    
    public bool IsOpen
    {
        get => _isOpen > 0;
        set
        {
            _isOpen = value ? 1 : 0;
            WaterMat.SetFloat(_paramIsOpen,_isOpen);
        }
    }

    public float MaskTop
    {
        get => _maskTop;
        set
        {
            _maskTop = value;
            WaterMat.SetFloat(_paramMaskTop,value);
        }
    }

    public float MaskBot
    {
        get => _maskBot;
        set
        {
            _maskBot = value;
            WaterMat.SetFloat(_paramMaskBottom,value);
        }
    }

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            WaterMat.SetColor(_paramColor,value);
        }
    }

    public Texture2D MaskTex
    {
        get => _maskTex;
        set
        {
            _maskTex = value;
            WaterMat.SetTexture(_paramMaskTexture,value);
        }
    }

    #endregion

    #region 私有方法

    private void Start()
    {
        img = GetComponent<Image>();
        
        InitWaterData(MaskTexture,4,Color.red,4);
    }

    private void InitWaterData(Sprite imgTube,float waterLine,Color waterColor,int maxBlock)
    {
        MaxBlock = maxBlock;
        // _waterLine = waterLine;
        img.material = WaterMat;
        img.sprite = MaskTexture = imgTube;
        Color = waterColor;
        Amplitude = WaterAmplitude;
        Frequence = WaterFrequence;
        SetWaterLine((int)_waterLine);
    }

    private IEnumerator FadeAnimLineToWater()
    {
        while (!_waterLine.Equals(_animLine))
        {
            float sign = Mathf.Sign(_waterLine - _animLine);
            float temp = Mathf.Min(Mathf.Abs(_waterLine - _animLine) , _speed);
            _animLine += sign * temp;
            AddY = _animLine / MaxBlock;
            yield return new WaitForEndOfFrame();
        }
    }
    
    #endregion

    #region 公有方法

    public void SetWaterLine(float index)
    {
        _waterLine = index;
        StartCoroutine(FadeAnimLineToWater());
    }
    
    #endregion
}