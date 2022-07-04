using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maskafter : MonoBehaviour
{
    //遮罩图
    public Texture2D MaskTexture;
    //用于渲染睡眠的材质
    public Material WaterMat;
    //用于存储渲染水位的组件，这里泛型放的是Image，按理来说可以放Image，RawImage，Text等可渲染UI的共同父类
    public List<Image> ListRenderComponent = new List<Image>();
    //对外开放的水位属性，每次修改都会刷新Shader渲染
    public int WaterLine {
        get {
            return _waterLine;
        }
        set {
            _waterLine = value;
            RefreshWater();
        }
    }

    //存放了所有渲染水位组件所用的材质
    private List<Material> ListMaterial = new List<Material>();
    //自己的UI变换
    private RectTransform rectTransform;
    //水位
    private int _waterLine = 0;
    //可用的Shader参数名
    private string _paramAddY = "_AddY";
    private string _paramIsOpen = "_IsOpen";
    private string _paramMaskTop = "_MaskTop";
    private string _paramMaskBottom = "_MaskBottom";
    private string _paramColor = "_Color";

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(MaskTexture.width,MaskTexture.height);

        RefreshWater();
    }

    // Update is called once per frame
    void Update()
    {



    }

    //计算每个图片在遮罩下的渲染范围
    Vector2 CalculatePercent(RectTransform rt)
    {
        Vector2 percent = new Vector2();
        float bottom = rt.localPosition.y + rectTransform.sizeDelta.y * rectTransform.pivot.y;
        float top = rt.localPosition.y + rectTransform.sizeDelta.y * rectTransform.pivot.y  +  rt.sizeDelta.y;
        percent.x = bottom / rectTransform.sizeDelta.y;
        percent.y = top / rectTransform.sizeDelta.y;
        return percent;
    }

    private void RefreshWater()
    {
        for (int i = 0; i < ListRenderComponent.Count; i++)
        {
            var img = ListRenderComponent[i];
            //计算每格水对应在遮罩里的哪段范围
            var range = CalculatePercent(img.GetComponent<RectTransform>());
            if (ListMaterial.Count <= i)
            {
                ListMaterial.Add(new Material(WaterMat));
            }
            img.material = ListMaterial[i];
            img.material.SetFloat(_paramMaskTop, range.y);
            img.material.SetFloat(_paramMaskBottom, range.x);
            img.material.SetColor(_paramColor, img.color);

            if (i > WaterLine)
            {
                //大于水位的就不渲染
                SetInvisible(img);
            }
            else if(i < WaterLine)
            {
                //等于水位的要渲染，并执行动画
                SetAnimate(img);
            }
            else
            {
                //小于水位的要渲染，并且不执行动画
                SetIdle(img);
            }
        }
    }

    private void SetAnimate(Image img)
    {
        img.material.SetFloat(_paramIsOpen, 1);
        img.material.SetFloat(_paramAddY, 0);
        img.gameObject.SetActive(true);
    }

    private void SetIdle(Image img)
    {
        img.material.SetFloat(_paramIsOpen, 0);
        img.gameObject.SetActive(true);
    }

    private void SetInvisible(Image img)
    {
        img.material.SetFloat(_paramIsOpen, 0);
        img.gameObject.SetActive(false);
    }

    public void StartWaterAnimation(Material mat,int Index)
    {
        _waterLine = Index;
        for (int i = 0; i < ListRenderComponent.Count; i++)
        {
            if (i > Index)
            {

            }else if (i<Index)
            {

            }
            else
            {

            }
        }
    }
}