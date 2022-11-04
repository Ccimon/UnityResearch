using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using GlobalModel.Data;

public class LevelManager : MolaSingleton<LevelManager>
{
    #region 私有变量
    //关卡列表
    //private List<LevelData> _levelDataList;
    //总关卡号
    private int _totalLevelCount;
    private List<bool> _addTubeLevel = new List<bool>();
    #endregion

    #region 公有变量
    //向外暴露的关卡列表
    //IReadOnlyList<LevelData> LevelDataList { get => _levelDataList; }
    //总关卡号
    public int TotalLevelCount { get => _totalLevelCount; }

    public List<bool> AddTubeLevel { get => _addTubeLevel; }
    #endregion

    #region 生命周期

    #endregion

    #region 公有方法
    public void Init()
    {
        TextAsset levelDataFile = Resources.Load<TextAsset>(string.Format("{0}/{1}", "Level", GameConfig.LevelCountFile));
        _totalLevelCount = int.Parse(levelDataFile.text);
        //RefreshAddTubeList();
        //CheckLevel();
    }

    //public void InitAddTubeStatus()
    //{
    //    _addTubeLevel = StatusManager.TubeUseConditionList;
    //}

    /// <summary>
    /// 检查每个关卡小球数量对不对
    /// </summary>
    public bool CheckLevel()
    {
        int ErrorLevelCount = 0;
        for (int levelIndex = 0; levelIndex < _totalLevelCount; levelIndex++)
        {
            LevelData levelData = GetLevelData(levelIndex);

            List<int> ballList = new List<int>(new int[13]);

            for (int tubeIndex = 0; tubeIndex < levelData.TubeList.Count; tubeIndex++)
            {
                for (int ballIndex = 0; ballIndex < levelData.TubeList[tubeIndex].Count; ballIndex++)
                {
                    int colorIndex = levelData.TubeList[tubeIndex][ballIndex];
                    ballList[colorIndex]++;
                }
            }

            for (int i = 1; i < ballList.Count; i++)
            {
                int ballCount = ballList[i];
                if (ballCount != 0 && ballCount != 4)
                {
                    if (levelIndex + 1 == 118)
                    {
                    }
                    ErrorLevelCount++;
                    break;
                }
            }
        }

        return ErrorLevelCount == 0;
    }

    /// <summary>
    /// 检查那些关卡有一上来就有完成管
    /// </summary>
    public void CheckLevelHaveCompleteTube()
    {
        for (int levelIndex = 0; levelIndex < _totalLevelCount; levelIndex++)
        {
            LevelData levelData = GetLevelData(levelIndex);

            for (int tubeIndex = 0; tubeIndex < levelData.TubeList.Count; tubeIndex++)
            {
                int colorIndex = levelData.TubeList[tubeIndex][0];
                int sameCount = 0;
                for (int ballIndex = 0; ballIndex < levelData.TubeList[tubeIndex].Count; ballIndex++)
                {
                    if (levelData.TubeList[tubeIndex][ballIndex] == colorIndex && colorIndex != 0)
                    {
                        sameCount++;
                    }
                }
                if (sameCount == 4)
                {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 拿取关卡数据
    /// </summary>
    /// <param name="levelIndex">关卡序号</param>
    /// <returns></returns>
    public LevelData GetLevelData(int levelIndex)
    {
        string fileName = string.Format(GameConfig.LevelFile, levelIndex);
        TextAsset levelDataFile = Resources.Load<TextAsset>(string.Format("{0}/{1}", "Level" , fileName));

        //如果有文件，读取文件
        if (levelDataFile != null)
        {
            string encryptStr = levelDataFile.text;
            string jsonStr = GameUtil.Decrypt(encryptStr);
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonStr);
            return levelData;
        }
        //如果没有这个序号，返回null。可能是缺失文件或者是通关了
        else
        {
            return null;
        }
    }

    //public void RefreshAddTubeList(int levelIndex,bool value)
    //{
    //    Loom.RunAsync(() =>
    //    {
    //        try
    //        {
    //            _addTubeLevel[levelIndex] = value;
    //            StatusManager.TubeUseConditionList = _addTubeLevel;
    //            this.Log("Level", "Refreshed");
    //        }
    //        catch (System.Exception ex)
    //        {
    //            this.Log("Level", ex.ToString());
    //        }
    //    });
    //}
    #endregion

    #region 动作

    #endregion

    #region 私有方法

    #endregion

}
