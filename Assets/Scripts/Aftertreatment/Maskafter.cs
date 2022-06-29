using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maskafter : MonoBehaviour
{
    public Texture2D MaskTexture;
    public List<Image> ListRenderComponent = new List<Image>();

    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(MaskTexture.width,MaskTexture.height);

        //for ()
        //{

        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 CalculatePercent(RectTransform rt)
    {
        float bottom = rt.localPosition.y - rt.pivot.y * rt.sizeDelta.y;
        float top = rt.localPosition.y + rt.pivot.y * rt.sizeDelta.y;

        return Vector2.zero;
    }
}