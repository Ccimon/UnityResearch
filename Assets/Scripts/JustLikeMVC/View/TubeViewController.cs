using System.Collections;
using System.Collections.Generic;
using GlobalModel.Data;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public bool IsComplete { get; private set; } = false;

    private void Awake()
    {
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
             ballview.Init(ballData);
             _ballBuffer.Push(ballview);
         }

         return this;
    }

    public TubeViewController RefreshView()
    {
        int i = 0;
        foreach (BallViewController view in _ballBuffer)
        {
            view.transform.parent = _ballPosList[i];
            view.transform.DOMove(view.transform.parent.position, 0.2f);
            i++;
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

    public void TubeFall()
    {
        _outBall?.transform.DOMove(_outBall.transform.parent.position, 0.1f);
        _outBall = null;
    }

    public void TubePop()
    {
        GetTopBall()?.transform.DOMove(BallOutPos.position,0.1f);
    }
     
    /// <summary>
    /// 把球弹出去
    /// </summary>
    /// <returns></returns>
    public List<BallViewController> BallPop()
    {
        List<BallViewController> viewList = new List<BallViewController>();
        int sameColor = -1;
        for (int i = _ballBuffer.Count; i > 0; i--)
        {
            BallViewController ballview = _ballBuffer.Pop();
            if (sameColor == -1)
            {
                sameColor = ballview.GetBallColor();
            }

            if (ballview.GetBallColor() == sameColor)
            {
                viewList.Add(ballview);
            }
            else
            {
                _ballBuffer.PushRange(viewList);
                break;
            }
        }
        return viewList;
    }
    
    /// <summary>
    /// 把球放进来
    /// </summary>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public bool BallPush(List<BallViewController> viewlist)
    {
        if (viewlist.Count < 1)
        {
            return false;
        }

        if (GetTopColor() < 0 && viewlist[0].GetBallColor() != GetTopColor())
        {
            return false;
        }

        _ballBuffer.PushRange(viewlist);

        IsComplete = CheckComplete();
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
            view = _ballBuffer.Pop();
            _outBall = view;
            _ballBuffer.Push(view);
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
        if (_tubeData.BallList.Count < 4)
        {
            return false;
        }

        int sameColor = -1;
        for (int i = 0; i < _tubeData.BallList.Count; i++)
        {
            var data = _tubeData.BallList[i];
            if (sameColor == -1)
            {
                sameColor = data.BallColor;
            }

            if (data.BallColor != sameColor)
            {
                return false;
            }
        }
        return true;
    }
}
