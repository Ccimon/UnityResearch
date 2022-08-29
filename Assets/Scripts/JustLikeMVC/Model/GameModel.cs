using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalModel.Data;

namespace GlobalModel.Data
{
    public enum LevelType
    {
        Normal
    }
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
        //唯一标示
        public string Id;
        //关卡序号
        public int LevelIndex;
        //关卡类型
        public LevelType LevelType; 
        //关卡内容
        public List<List<int>> LevelContain;

        public List<List<int>> TubeList
        {
            get
            {
                return LevelContain;
            }
        }
    }

    public class TubeData:Data
    {
        public int Index;
        public int length = 4;
        public List<int> BallList;
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
    public class GameModel
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
            Debug.Log("GameModel:Init Success");
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

        private OnCaseEnter _StartCallBack;

        private OnCaseEnter _gameStartCallBack
        {
            get
            {
                Debug.Log("getter _startcallback:" + _StartCallBack);
                return _StartCallBack;
            }
            set
            {
                Debug.Log("setter _startcallback:" + _StartCallBack);
                _StartCallBack += value;
            }
        }
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
            LoadLevelData();
            _gameStartCallBack?.Invoke();
        }

        private void AfterLevelChanged()
        {
            LoadLevelData();
        }
        
        /// <summary>
        /// 加载下一关数据
        /// </summary>
        /// <param name="levelIndex">关卡</param>
        private void LoadLevelData(int levelIndex = 0)
        {
            LocalLevelData = LevelManager.Instance.GetLevelData(levelIndex);
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
