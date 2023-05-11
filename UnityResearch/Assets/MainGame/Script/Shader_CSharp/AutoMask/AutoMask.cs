using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
/*
 * 一个基于C# + 双纹理Shader叠加的遮罩
 * C#负责其中缩放以及偏移的部分,旋转暂不支持
 * Shader负责根据透明度进行遮罩实现
 */
public class AutoMask : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    private Texture MaskTexture;
    [Space(10)]
    [SerializeField]
    // 遮罩是否跟随图片的宽高比变化
    private bool _SyncRectChange = false;

    #region 私有计算用变量

    private Vector2 _MaskOffset = Vector2.one * 0.5f;
    private Vector2 _MaskScale = Vector2.one;


    private Material _Material;
    private Image _Self;
    private Vector2 _Tile;
    private Vector2 _CenterOffset;

    #endregion

    #region 私有Shader内置参数常量

    private const string MASK_ST = "_MaskTex_ST";
    private const string MASK_TEX = "_MaskTex";

    #endregion
    
    #region 公共属性
    public Material Material
    {
        get
        {
            return _Self.material;
        }
        set
        {
            _Self.material = value;
        }
    }

    public Vector2 MaskOffset
    {
        get
        {
            return _MaskOffset;
        }
        set
        {
            _MaskOffset = value;
            SetOffset(_Material,value);
        }
    }

    public Vector2 MaskScale
    {
        get
        {
            return _MaskScale;
        }
        set
        {
            _MaskScale = value;
            SetScale(_Material,value);
        }
    }
    
    #endregion

    void Awake()
    {
        // 获取渲染组件
        _Self = GetComponent<Image>();
        // 初始化材质
        _Self.material = InitMaskMat();
        
        //TODO 测试代码 可删除
        // StartCoroutine(DelayCall());
    }

    IEnumerator DelayCall()
    {
        var wait = new WaitForSeconds(1.0f);
        yield return wait;
        MaskScale = new Vector2(2, 2);
        yield return wait;
        MaskOffset = new Vector2(0.4f, 0.4f);
    }
    
    /// <summary>
    /// 跟随RectTransform的改动同步Shader的宽高比变化
    /// </summary>
    private void OnRectTransformDimensionsChange()
    {
        if (_SyncRectChange)
        {
            ResetSTConstData();
            RefreshMaskST(_Self.material);
        }
    }

    /// <summary>
    /// 初始化材质
    /// </summary>
    /// <returns></returns>
    private Material InitMaskMat()
    {
        Shader maskShader = Shader.Find("shader2d/mask");
        Material material = new Material(maskShader);
        _Material = material;
        ResetSTConstData();
        RefreshMaskST(material);
        return material;
    }
    
    /// <summary>
    /// 刷新Shader参数
    /// </summary>
    /// <param name="material"></param>
    private void RefreshMaskST(Material material)
    {
        material.SetTexture("_MaskTex",MaskTexture);
        material.SetVector("_MaskTex_ST",GetStandardST());
    }
    
    /// <summary>
    /// 重新计算Shader所需的辅助参数
    /// </summary>
    /// <returns></returns>
    private Vector2 ResetSTConstData()
    {
        Rect rect = this.GetRectTransform().rect;
        float tilex = rect.width / MaskTexture.width;
        float tiley = rect.height / MaskTexture.height;

        _CenterOffset = (Vector2.one - new Vector2(tilex,tiley))/2;
        _Tile.x = tilex;
        _Tile.y = tiley;
        return new Vector2(tilex,tiley);
    }
    
    /// <summary>
    /// 计算ST参数
    /// </summary>
    /// <returns></returns>
    private Vector4 GetStandardST()
    {
        Vector4 ST = new Vector4();

        ST.x = _Tile.x / _MaskScale.x;
        ST.y = _Tile.y / _MaskScale.y;
        float offx = Mathf.Clamp(_MaskOffset.x, 0, 1);
        float offy = Mathf.Clamp(_MaskOffset.y, 0, 1);
        ST.z = (- offx * _Tile.x)/_MaskScale.x + 0.5f;
        ST.w = (- offy * _Tile.y)/_MaskScale.y + 0.5f;
        return ST;
    }
    
    /// <summary>
    /// 设置Mask遮罩的Offset
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="position"></param>
    private void SetOffset(Material mat,Vector2 position)
    {
        Vector4 ST = GetStandardST();
        
        _Material.SetVector(MASK_ST,ST);
    }

    /// <summary>
    /// 设置Mask遮罩的Scale
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="position"></param>
    private void SetScale(Material mat,Vector2 scale)
    {
        ResetSTConstData();
        Vector4 ST = GetStandardST();

        _Material.SetVector(MASK_ST,ST);
    }
    
    /// <summary>
    /// 设置纹理
    /// </summary>
    /// <param name="mat"></param>
    /// <param name="texture"></param>
    public void SetTexture(Material mat,Texture2D texture)
    {
        MaskTexture = texture;
        ResetSTConstData();
        Vector4 ST = GetStandardST();
        mat.SetTexture(MASK_TEX,texture);
        mat.SetVector(MASK_ST,ST);
    }
}
