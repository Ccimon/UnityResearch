using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extention
{
    /// <summary>
    /// 获取物体的RectTransform
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static RectTransform GetRectTransform(this GameObject self)
    {
        return self.GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// 获取物体的RectTransform
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static RectTransform GetRectTransform(this MonoBehaviour self)
    {
        return self.GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// 获取物体的RectTransform
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static RectTransform GetRectTransform(this Component self)
    {
        return self.GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// 获取物体的RectTransform
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static RectTransform GetRectTransform(this Transform self)
    {
        return self.GetComponent<RectTransform>();
    }
}
