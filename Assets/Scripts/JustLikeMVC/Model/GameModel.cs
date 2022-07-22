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
        public List<List<int>> TubeList;
    }

    public class TubeData
    {
        public int Index;
        public List<Balldata> BallList;
    }

    public class Balldata
    {

    }
}


namespace GlobalModel
{
    public class GameModel
    {
        public delegate void OnIntValueChange(int value);

        private int _localLevel;
        private int _coin;
        private int _lastLevel;
        private int _maxLevel;
        private LevelData _localLevelData;



        private OnIntValueChange _levelEndCallBack;
        private OnIntValueChange _levelStartCallBack;

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

        private void AfterLevelChanged()
        {
            LoadNextLevelData();
        }

        private void LoadNextLevelData()
        {

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


    }
}
