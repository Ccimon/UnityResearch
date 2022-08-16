using System.Collections;
using System.Collections.Generic;
using GlobalModel;
using UnityEngine;

/// <summary>
/// LaunchManager负责对游戏内的所有Model依次初始化
/// </summary>
public class LaunchManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        var model = GameModel.Instance;
        model.Init();
    }
}