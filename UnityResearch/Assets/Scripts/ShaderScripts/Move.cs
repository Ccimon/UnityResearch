using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(0, 0.5f))
            .Append(transform.DOMoveY(-10, 0.5f))
            .SetLoops(-1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
