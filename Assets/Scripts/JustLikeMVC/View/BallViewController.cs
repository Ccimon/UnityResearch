using System.Collections;
using System.Collections.Generic;
using GlobalModel.Data;
using UnityEngine;
using UnityEngine.UI;

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

    private void InitBall()
    {

    }
}
