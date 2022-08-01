using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalModel;
using GlobalModel.Data;

public class PanelGame : MonoBehaviour
{
    [SerializeField]
    private GameObject TubeObj;

    private List<TubeViewController> _tubeViewList = new List<TubeViewController>();
    public LevelData LocalLevelData
    {
        get
        {
            return GameModel.Instance.LocalLevelData;
        }
    }

    public void Start()
    {
        InitGamePanel();
    }

    private void RegisterListener()
    {
        GameModel.Instance.AddGameStartListener(OnGameStart);
    }

    private void OnGameStart()
    {
        
    }

    private void InitGamePanel()
    {
        int width = Screen.width;
        for (int i = 0; i < LocalLevelData.TubeList.Count; i++) 
        {
            if (TubeObj == null)
            {
                break;
            }

            GameObject tube = Instantiate(TubeObj);
            tube.transform.parent = transform;
            TubeViewController tubeView = tube.GetComponent<TubeViewController>();
            _tubeViewList.Add(tubeView.Init(LocalLevelData.TubeList[i]));

        }
    }
}
