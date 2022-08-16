
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using GlobalModel.Data;
using UnityEngine.UI;

public static class GameUtil
{
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

    public static void ShowToast(string info)
    {
        var toastPrefab = Resources.Load<GameObject>("Prefab/ToastObj");
        var toast = GameObject.Instantiate(toastPrefab,PanelGame.Instance.GameCoverLayout);
        var text = toast.GetComponentInChildren<Text>();
        text.text = info;
        text.transform.parent.GetComponent<CanvasGroup>().DOFade(0,0.5f).SetDelay(1.5f);
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

}
