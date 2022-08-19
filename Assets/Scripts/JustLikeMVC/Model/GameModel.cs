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
        Green,
        Orange
    }

    public class Data
    {
        
    }

    public class LevelData:Data
    {
        public int Index;
        public List<TubeData> TubeList;
    }

    public class TubeData:Data
    {
        public int Index;
        public int length = 4;
        public List<Balldata> BallList;
    }

    public class Balldata:Data
    {
        public int BallColor;
    }
}

namespace GlobalModel
{
    /// <summary>
    /// GameModel管理所有的游戏相关数据,关卡,关卡信息,关卡流程。
    /// 向外提供多个委托,提供流程回调
    /// </summary>
    public class GameModel:Object
    {
        private static GameModel _instance = null;
        public static GameModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameModel();
                }
                return _instance;
            }
        }

        private GameModel()
        {
        }

        public delegate void OnIntValueChange(int value);
        public delegate void OnCaseEnter();

        private int _localLevel = 0;
        private int _coin;
        private int _lastLevel;
        private int _maxLevel;
        private bool _isGameComplete = false;

        private OnIntValueChange _levelEndCallBack;
        private OnIntValueChange _levelStartCallBack;

        private OnCaseEnter _gameStartCallBack;
        private OnCaseEnter _gameEndCallBack;

        public LevelData LocalLevelData { get; private set; }
        public List<int> UnSameColor { get; private set; }
        public int LocalLevel
        {
            get => _localLevel;
            set
            {
                _localLevel = value;
                _levelEndCallBack?.Invoke(_localLevel);
                AfterLevelChanged();
            }
        }

        public bool IsGameComplete
        {
            get => _isGameComplete;
            set
            {
                _isGameComplete = value;
                if (value)
                {
                    _gameEndCallBack?.Invoke();
                }
                else
                {
                    _gameStartCallBack?.Invoke();
                }
            }
        }

        /// <summary>
        /// GameModel在这里初始化，并在初始化完成后开始游戏
        /// </summary>
        public void Init()
        {
            if (UnSameColor == null)
            {
                UnSameColor = new List<int>();
            }
            LoadNextLevelData();
            _gameStartCallBack?.Invoke();
        }

        private void AfterLevelChanged()
        {
            LoadNextLevelData();
        }
        
        /// <summary>
        /// 加载下一关数据
        /// </summary>
        /// <param name="levelIndex">关卡</param>
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
            
            UnSameColor.Clear();
            for (int i = 0; i < LocalLevelData.TubeList.Count; i ++)
            {
                var tube = LocalLevelData.TubeList[i];
                for (int j = 0; j < tube.BallList.Count; j ++)
                {
                    var ball = tube.BallList[j];
                    if (!UnSameColor.Contains(ball.BallColor))
                    {
                        UnSameColor.Add(ball. BallColor);
                    }
                }
            }
        }

        #region 游戏流程回调

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

        #endregion
        
    }
}
