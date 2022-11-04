using System.Collections;
using System.Collections.Generic;
using GlobalModel.Data;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GlobalModel;

/// <summary>
/// TubeViewController控制整个管子信息,以及对Ball的操作行为。
/// </summary>
public class TubeViewController : MonoBehaviour,IViewController<TubeViewController,TubeData>
{

    [SerializeField]
    private GameObject BallObj;
    [SerializeField]
    private List<RectTransform> _ballPosList = new List<RectTransform>();
    [SerializeField] 
    private RectTransform BallOutPos;

    private Stack<BallViewController> _ballBuffer = new Stack<BallViewController>();

    private TubeData _tubeData;

    private Button button;

    private BallViewController _outBall;
    public bool IsComplete { get; private set; }

    private void Awake()
    {
        IsComplete = false;
        button = transform.GetComponent<Button>();
        button.onClick.AddListener(OnTubeClick);
    }
    
    private void OnTubeClick()
    {
        PanelGame.Instance.TubeClick(this);
    }
    
    public TubeViewController Init<TData>(TData data)
    {
         _tubeData = data as  TubeData;
         if (_tubeData == null)
         {
             return this;
         }
         
         var list = _tubeData.BallList;
         for (int i = 0; i < list.Count; i++)
         {
             var ballData = list[i];
             var ball = Instantiate(BallObj,_ballPosList[i]);
             var ballview = ball.GetComponent<BallViewController>();
             ballview.transform.localPosition = Vector3.zero;
             ballview.Init(ballData);
             _ballBuffer.Push(ballview);
         }

         return this;
    }

    public TubeViewController RefreshView()
    {
        int i = _ballBuffer.Count - 1;
        foreach (BallViewController view in _ballBuffer)
        {
            view.transform.parent = _ballPosList[i].transform;
            view.transform.DOMove(view.transform.parent.position, 0.1f);
            i--;
        }

        return this;
    }
    
    /// <summary>
    /// 获取顶部小球颜色
    /// </summary>
    /// <returns></returns>
    public int GetTopColor()
    {
        BallViewController top = GetTopBall();
        int color = -1;
        if (top != null)
        {
            color = top.GetBallColor();
        }
        return color;
    }

    /// <summary>
    /// 放下顶端小球
    /// </summary>
    public void TubeFall()
    {
        if(IsComplete){return;}
        _outBall?.transform.DOMove(_outBall.transform.parent.position, 0.1f);
        _outBall = null;
    }
    
    /// <summary>
    /// 弹起顶端小球
    /// </summary>
    public void TubePop()
    {
        if(IsComplete){return;}
        GetTopBall()?.transform.DOMove(BallOutPos.position,0.1f);
    }
     
    /// <summary>
    /// 把球弹出去
    /// </summary>
    /// <returns>被弹出的小球的列表</returns>
    public List<BallViewController> BallPop()
    {
        if(IsComplete){return null;}
        List<BallViewController> viewList = new List<BallViewController>();
        int sameColor = -1;
        for (int i = _ballBuffer.Count; i > 0; i--)
        {
            BallViewController ballview = _ballBuffer.Peek();
            if (sameColor == -1)
            {
                sameColor = ballview.GetBallColor();
            }

            if (ballview.GetBallColor() == sameColor)
            {
                viewList.Add(ballview);
                _ballBuffer.Pop();
            }
            else
            {
                break;
            }
        }

        _outBall = null;
        return viewList;
    }
    
    /// <summary>
    /// 把球放进来
    /// </summary>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public bool BallPush(List<BallViewController> viewlist)
    {
        if(IsComplete){return false;}
        if (viewlist.Count < 1)
        {
            return false;
        }

        if (GetTopColor() > 0 && viewlist[0].GetBallColor() != GetTopColor() && _ballBuffer.Count + viewlist.Count > _tubeData.length)
        {
            return false;
        }

        _ballBuffer.PushRange(viewlist);

        IsComplete = CheckComplete();
        _outBall = null;
        return true;
    }

    /// <summary>
    /// 获取顶部的小球
    /// </summary>
    /// <returns></returns>
    private BallViewController GetTopBall()
    {
        BallViewController view;
        if (_outBall == null && _ballBuffer.Count > 0)
        {
            view = _ballBuffer.Peek();
            _outBall = view;
        }
        else
        {
            view = _outBall;
        }
        return view;
    }
    
    /// <summary>
    /// 检测管子是否完成
    /// </summary>
    /// <returns></returns>
    private bool CheckComplete()
    {
        if (_ballBuffer.Count < _tubeData.length)
        {
            return false;
        }

        var list = _ballBuffer.ToArray();
        int sameColor = -1;
        for (int i = 0; i < list.Length; i++)
        {
            var data = list[i];
            if (sameColor == -1)
            {
                sameColor = data.GetBallColor();
            }

            if (data.GetBallColor() != sameColor)
            {
                return false;
            }
        }
        return true;
    }
}
