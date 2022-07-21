using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int Index;
    public List<List<int>> TubeList;
}

public delegate void OnIntValueChange(int value);

public class GameModel
{
    private int _localLevel;
    private LevelData _localLevelData;
    private List<OnIntValueChange> _levelChangeCallBackList = new List<OnIntValueChange>();

    public int LocalLevel
    {
        get => _localLevel;
        set
        {
            _localLevel = value;
            for (int i =0; i < _levelChangeCallBackList.Count; i++)
            {
                var callBack = _levelChangeCallBackList[i];
                callBack(value);
            }
        }
    }
}
