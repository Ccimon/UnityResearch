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

    public LevelData LocalLevelData
    {
        get
        {
            return GameModel.Instance.LocalLevelData;
        }
    }

    private void Awake()
    {
        if (PanelGame.Instance == null)
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

    private void RegisterListener()
    {
        GameModel.Instance.AddGameStartListener(OnGameStart);
    }

    private void OnGameStart()
    {
        InitGamePanel();
    }

    private void InitGamePanel()
    {
        int width = Screen.width;
        int height = Screen.height;
        int tubeCount = LocalLevelData.TubeList.Count;
        for (int i = 0; i < LocalLevelData.TubeList.Count; i++) 
        {
            if (TubeObj == null)
            {
                break;
            }

            GameObject tube = Instantiate(TubeObj);
            tube.transform.parent = transform;
            tube.transform.SetPositionAndRotation(new Vector3(50 + width/tubeCount*i,height/2,0),new Quaternion());
            TubeViewController tubeView = tube.GetComponent<TubeViewController>();
            _tubeViewList.Add(tubeView.Init(LocalLevelData.TubeList[i]));

        }
    }

    public void TubeClick(TubeViewController tube)
    {
        if (_tubeClickBuffer.Count >= 2)
        {
            var left = _tubeClickBuffer[0];
            var right = _tubeClickBuffer[1];

            if (right.GetTopColor() <= 0 || left.GetTopColor() == right.GetTopColor())
            {
                right.BallPush(left.BallPop());
            }

            left.TubeFall();
            right.TubePop();
            _tubeClickBuffer.Clear();
            _tubeClickBuffer.Add(right);
        }
        else
        {
            if (tube.GetTopColor() > 0)
            {
                var left = _tubeClickBuffer[0];
                left.TubePop();
            }
        }
    }
}
