using System.Collections;
using System.Collections.Generic;
using GlobalModel.Data;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TubeViewController控制整个管子信息,以及对Ball的操作行为。
/// </summary>
public class TubeViewController : MonoBehaviour,IViewController<TubeViewController,TubeData>
{

    [SerializeField]
    private GameObject BallObj;
    [SerializeField]
    private List<RectTransform> _ballPosList = new List<RectTransform>();

    private List<BallViewController> _ballViewList = new List<BallViewController>();
    
    private Stack<BallViewController> _ballBuffer = new Stack<BallViewController>();

    private TubeData _tubeData;

    private Button button;

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
             var pos = _ballPosList[i];
             var ball = Instantiate(BallObj, pos);
             var ballview = ball.GetComponent<BallViewController>();
             ballview.Init(ballData);
         }

         return RefreshView(data);
    }

    public TubeViewController RefreshView<TData>(TData data)
    {
        return this;
    }

    public int GetTopColor()
    {
        if (_tubeData.BallList.Count > 0)
        {
            return _tubeData.BallList[_tubeData.BallList.Count - 1].BallColor;
        }

        return -1;
    }

    public void TubeFall()
    {

    }

    public void TubePop()
    {

    }
     
    public List<Balldata> BallPop()
    {
        List<Balldata> dataList = new List<Balldata>();
        int sameColor = -1;
        for (int i = _tubeData.BallList.Count - 1; i >= 0; i--)
        {
            var data = _tubeData.BallList[i];
            if (sameColor == -1)
            {
                sameColor = data.BallColor;
            }

            if (data.BallColor == sameColor)
            {
                _tubeData.BallList.Remove(data);
                dataList.Add(data);
            }
            else
            {
                break;
            }
        }
        return dataList;
    }

    public bool BallPush(List<Balldata> dataList)
    {
        if (dataList.Count < 1)
        {
            return false;
        }

        if (dataList[0].BallColor != GetTopColor())
        {
            return false;
        }

        _tubeData.BallList.AddRange(dataList);

        IsComplete = CheckComplete();
        return true;
    }

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
