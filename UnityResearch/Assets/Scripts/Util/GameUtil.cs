
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using DG.Tweening;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


public static class GameUtil
{
    private static GameObject _toastObj;



    #region 拓展函数
    
    /// <summary>
    /// 将一个列表的数据放入栈中
    /// </summary>
    /// <param name="stack"></param>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    public static void PushRange<T>(this Stack<T> stack,List<T> list)
    {
        while (list.Count > 0)
        {
            stack.Push(list.Pop());
        }
    }
    
    /// <summary>
    /// 弹出列表里的最后一个元素,会执行移除
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Pop<T>(this List<T>list)
    {
        T item = default;
        if (list.Count > 0)
        {
            item = list[list.Count - 1];
            list.Remove(item);
        }

        return item;
    }
    
    /// <summary>
    /// 设置节点的RectTransform的位置信息
    /// </summary>
    /// <param name="gameObj"></param>
    /// <param name="position"></param>
    public static void SetUIPosition(this MonoBehaviour gameObj,Vector3 position)
    {
        RectTransform rtrans = gameObj.GetComponent<RectTransform>();
        if (rtrans != null)
        {
            rtrans.SetPositionAndRotation(position,Quaternion.identity);
        }
    }
    
    /// <summary>
    /// 将向量放大指定倍数
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="scale"></param>
    public static Vector3 Multi(this Vector3 vec,float scale)
    {
        vec.x *= scale;
        vec.y *= scale;
        vec.z *= scale;
        return vec;
    }
    
    #endregion

    #region 加密解密类
    /// <summary>
    /// 加密订单列表明文
    /// </summary>
    /// <param name="toE"></param>
    /// <returns></returns>
    public static string Encrypt(string toE)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(GameConfig.EncryptKeyWord);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        byte[] toEncryptArray = Encoding.UTF8.GetBytes(toE);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// 解析订单列表密文
    /// </summary>
    /// <param name="toD"></param>
    /// <returns></returns>
    public static string Decrypt(string toD)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(GameConfig.EncryptKeyWord);

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        byte[] toEncryptArray = Convert.FromBase64String(toD);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Encoding.UTF8.GetString(resultArray);
    }
    
    /// <summary>
    /// 获取相较于一个物体的相对位置
    /// </summary>
    /// <param name="self">自己的位置</param>
    /// <param name="other">目标位置</param>
    /// <returns>相对位置坐标</returns>
    public static Vector3 GetRelativePosition(this Transform self,Transform other)
    {
        Vector3 distance = self.position - other.position;
        Vector3 relative = Vector3.zero;
        relative.x = Vector3.Dot(distance, other.right.normalized);
        relative.y = Vector3.Dot(distance, other.up.normalized);
        relative.z = Vector3.Dot(distance, other.forward.normalized);
        return relative;
    }
    
    /// <summary>
    /// 获取相较于一个物体的相对位置
    /// </summary>
    /// <param name="self">自己的位置</param>
    /// <param name="other">目标位置</param>
    /// <returns>相对位置坐标</returns>
    public static Vector3 GetRelativePosition(this Vector3 self,Transform other)
    {
        Vector3 distance = other.position - self;
        Vector3 relative = Vector3.zero;
        relative.x = Vector3.Dot(distance, other.right.normalized);
        relative.y = Vector3.Dot(distance, other.up.normalized);
        relative.z = Vector3.Dot(distance, other.forward.normalized);
        return relative;
    }
    
    /// <summary>
    /// 获取节点的RectTransform
    /// </summary>
    /// <param name="self">当前节点</param>
    /// <returns>RectTransform组件</returns>
    public static RectTransform GetRectTransform(this MonoBehaviour self)
    {
        return self.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 获取节点的RectTransform
    /// </summary>
    /// <param name="self">当前节点</param>
    /// <returns>RectTransform组件</returns>
    public static RectTransform GetRectTransform(this GameObject self)
    {
        return self.GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// 获取节点的RectTransform
    /// </summary>
    /// <param name="self">当前节点</param>
    /// <returns>RectTransform组件</returns>
    public static RectTransform GetRectTransform(this Transform self)
    {
        return self.GetComponent<RectTransform>();
    }
    #endregion
}
