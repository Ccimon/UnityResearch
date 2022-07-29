using System.Collections;
using System.Collections.Generic;
using GlobalModel.Data;
using UnityEngine;

/// <summary>
/// TubeViewController控制整个管子信息,以及对Ball的操作行为。
/// </summary>
public class TubeViewController : MonoBehaviour
{

    [SerializeField]
    private GameObject BallObj;
    [SerializeField]
    private List<RectTransform> _ballPosList = new List<RectTransform>();

    private List<BallViewController> _ballViewList = new List<BallViewController>();

    private TubeData _tubeData;

    public TubeViewController Init(TubeData data)
    {
        _tubeData = data;

        return this;
    }
}
