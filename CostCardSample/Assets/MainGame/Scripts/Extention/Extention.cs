using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = System.Random;

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
    
    /// <summary>
    /// 随机获取几个元素的下标
    /// </summary>
    /// <param name="list"></param>
    /// <param name="count"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<int> GetRandomElementsIndex<T>(this List<T> list,int count)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < count; i++)
        {
            result.Add(UnityEngine.Random.Range(0,list.Count));
        }

        return result;
    }
    
    /// <summary>
    /// 获取随机元素
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
