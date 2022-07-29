using System.Collections;
using System.Collections.Generic;
using GlobalModel;
using GlobalModel.Data;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理Ball的信息,皮肤,颜色。只有基本切换皮肤的行为。
/// </summary>
public class BallViewController : MonoBehaviour
{
    [SerializeField]
    private Image _ballImage;
    private Balldata _localBallData;

    public Balldata Balldata
    {
        get => _localBallData;
        set
        {
            Balldata = _localBallData;
        }
    }

    private BallViewController Init()
    {

        return this;
    }
}
