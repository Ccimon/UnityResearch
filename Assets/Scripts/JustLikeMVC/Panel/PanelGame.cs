using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalModel;
using GlobalModel.Data;

public class PanelGame : MonoBehaviour
{
    public static PanelGame Instance;

    [SerializeField]
    private GameObject TubeObj;

    private List<TubeViewController> _tubeViewList = new List<TubeViewController>();
    private List<TubeViewController> _tubeClickBuffer = new List<TubeViewController>();

    public RectTransform GameContentLayout;
    public RectTransform GameCoverLayout;
    public LevelData LocalLevelData
    {
        get
        {
            return GameModel.Instance.LocalLevelData;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            RegisterListener();
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
        }
    }

    #region 事件监听

    /// <summary>
    /// 添加监听事件
    /// </summary>
    private void RegisterListener()
    {
        Debug.Log("GameCallBack Register");
        GameModel.Instance.AddGameStartListener(OnGameStart);
        GameModel.Instance.AddGameEndListener(OnGameEnd);
    }

    private void OnGameStart()
    {
        InitGamePanel();
    }

    private void OnGameEnd()
    {
        GameUtil.ShowToast("GameComplete");
        foreach (var tube in _tubeViewList)
        {
            // tube.
        }
    }
    #endregion
    
    #region 游戏行为
    /// <summary>
    /// 初始化游戏界面
    /// </summary>
    private void InitGamePanel()
    {
        int tubeCount = LocalLevelData.TubeList.Count;
        if (_tubeViewList.Count < tubeCount)
        {
            for (int i = _tubeViewList.Count; i < tubeCount; i++) 
            {
                if (TubeObj == null)
                {
                    break;
                }

                GameObject tube = Instantiate(TubeObj,GameContentLayout.transform);
                tube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                TubeViewController tubeView = tube.GetComponent<TubeViewController>();
                _tubeViewList.Add(tubeView.Init(LocalLevelData.TubeList[i]));
            }
        }
        else
        {
            for (int i = _tubeViewList.Count; i >= 0; i--)
            {
                if (i >= tubeCount)
                {
                    Destroy(_tubeClickBuffer[i]);
                    _tubeViewList.Remove(_tubeClickBuffer[i]);
                }
                else
                {
                    _tubeViewList[i].Init(LocalLevelData.TubeList[i]);
                }
            }
        }
    }
    
    /// <summary>
    /// 点击管子后，对管子进行管理
    /// </summary>
    /// <param name="tube"></param>
    public void TubeClick(TubeViewController tube)
    {
        if (_tubeClickBuffer.Count >= 1)
        {
            var left = _tubeClickBuffer[0];
            var right = tube;
            if (!_tubeClickBuffer.Contains(tube))
            {
                if (right.GetTopColor() < 0 || left.GetTopColor() == right.GetTopColor())
                {
                    if (right.BallPush(left.BallPop()))
                    {
                        right.RefreshView();
                        _tubeClickBuffer.Clear();
                        CheckGameComplete();
                    }
                }
            }
            else
            {
                tube.TubeFall();
                _tubeClickBuffer.Clear();
            }
        }
        else
        {
            if (tube.GetTopColor() >= 0)
            {
                _tubeClickBuffer.Add(tube);
                tube.TubePop();
            }
        }
    }
    
    /// <summary>
    /// 检测游戏是否完成
    /// </summary>
    public void CheckGameComplete()
    {
        int completeCount = 0;
        for (int i = 0; i < _tubeViewList.Count; i++)
        {
            var tube = _tubeViewList[i];
            if (tube.IsComplete)
            {
                completeCount++;
            }
        }

        if (completeCount >= GameModel.Instance.UnSameColor.Count)
        {
            GameComplete();
        }
    }
    
    /// <summary>
    /// 游戏完成行为
    /// </summary>
    private void  GameComplete()
    {
        GameModel.Instance.IsGameComplete = true;
        GameModel.Instance.LocalLevel++;
    }

    #endregion
    
}
