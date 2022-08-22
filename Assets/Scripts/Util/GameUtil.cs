
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
using GlobalModel.Data;


public static class GameUtil
{
    private static GameObject _toastObj;
    
    #region 调用函数
    public static Color ConvertEnumToColor(BallColor color)
    {
        switch (color)
        {
            case BallColor.Black:return Color.black;
            case BallColor.Blue: return Color.blue;
            case BallColor.Green: return Color.green;
            case BallColor.Orange: return new Color(0.8f,0.5f,0.0f);
            case BallColor.Yellow: return Color.yellow;
            case BallColor.Purple: return new Color(0.6f, 0.1f, 0.6f);
            case BallColor.Red: return Color.red;
            default: return Color.white;
        }
    }
    
    /// <summary>
    /// 展示一个提示
    /// </summary>
    /// <param name="info">展示用的信息</param>
    public static void ShowToast(string info,float duration = 0.5f,float fadeTime = 1.5f)
    {
        if (_toastObj == null)
        {
            var toastPrefab = Resources.Load<GameObject>("Prefab/ToastObj");
            _toastObj = GameObject.Instantiate(toastPrefab,PanelGame.Instance.GameCoverLayout);
        }
        

        var text = _toastObj.GetComponentInChildren<Text>();
        text.text = info;
        text.transform.parent.GetComponent<CanvasGroup>().DOFade(0,duration).SetDelay(fadeTime);
    }
    #endregion


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
    #endregion
}
