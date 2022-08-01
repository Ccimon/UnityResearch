using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalModel.Data;

namespace GlobalModel.Data
{
    public enum BallColor
    {
        Red,
        White,
        Black,
        Blue,
        Yellow,
        Purple,
        Grren,
        Orange
    }

    public class LevelData
    {
        public int Index;
        public List<TubeData> TubeList;
    }

    public class TubeData
    {
        public int Index;
        public int length = 4;
        public List<Balldata> BallList;
    }

    public class Balldata
    {
        public int BallColor;
    }
}

/// <summary>
/// GlobalModel是所有Model的集合,用来存放所有的Model代码
/// </summary>
namespace GlobalModel
{
    /// <summary>
    /// GameModel管理所有的游戏相关数据,关卡,关卡信息,关卡流程。
    /// 向外提供多个委托,提供流程回调
    /// </summary>
    public class GameModel
    {
        public delegate void OnIntValueChange(int value);
        public delegate void OnCaseEnter();

        private static GameModel _instance = null;
        public static GameModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameModel();
                    _instance.Init();
                }
                return _instance;
            }
        }

        private int _localLevel;
        private int _coin;
        private int _lastLevel;
        private int _maxLevel;
        public LevelData LocalLevelData { get; private set; }

        private OnIntValueChange _levelEndCallBack;
        private OnIntValueChange _levelStartCallBack;

        private OnCaseEnter _gameStartCallBack;
        private OnCaseEnter _gameEndCallBack;

        public int LocalLevel
        {
            get => _localLevel;
            set
            {
                _localLevel = value;
                _levelEndCallBack(_localLevel);
                AfterLevelChanged();
            }
        }

        private void Init()
        {
            LoadNextLevelData();
        }

        private void AfterLevelChanged()
        {
            LoadNextLevelData();
        }

        private void LoadNextLevelData(int levelIndex = 0)
        {
            LocalLevelData = new LevelData();
            LocalLevelData.Index = levelIndex;
            LocalLevelData.TubeList = new List<TubeData>();
            var list = LocalLevelData.TubeList;
            var tubeData0 = new TubeData();
            var ballList0= tubeData0.BallList = new List<Balldata>();
            ballList0.Add(new Balldata() { BallColor = 0});
            ballList0.Add(new Balldata() { BallColor = 1 });
            ballList0.Add(new Balldata() { BallColor = 0 });
            ballList0.Add(new Balldata() { BallColor = 1 });
            var tubeData1 = new TubeData();
            list.Add(tubeData0);
            var ballList1 = tubeData1.BallList = new List<Balldata>();
            ballList1.Add(new Balldata() { BallColor = 1 });
            ballList1.Add(new Balldata() { BallColor = 0 });
            ballList1.Add(new Balldata() { BallColor = 1 });
            ballList1.Add(new Balldata() { BallColor = 0 });
            list.Add(tubeData1);
            var tubeData2 = new TubeData();
            tubeData2.BallList = new List<Balldata>();
            list.Add(tubeData2);
        }

        public void AddLevelEndListener(OnIntValueChange callback)
        {
            _levelEndCallBack += callback;
        }

        public void RemoveLevelEndListener(OnIntValueChange callback)
        {
            _levelEndCallBack -= callback;
        }

        public void AddLevelStartListener(OnIntValueChange callback)
        {
            _levelStartCallBack += callback;
        }

        public void RemoveLevelStartListener(OnIntValueChange callback)
        {
            _levelStartCallBack -= callback;
        }

        public void AddGameStartListener(OnCaseEnter callback)
        {
            _gameStartCallBack += callback;
        }

        public void RemoveGameStartListener(OnCaseEnter callback)
        {
            _gameStartCallBack -= callback;
        }

        public void AddGameEndListener(OnCaseEnter callback)
        {
            _gameEndCallBack += callback;
        }

        public void RemoveGameEndListener(OnCaseEnter callback)
        {
            _gameEndCallBack -= callback;
        }
    }
}
