using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArabicAdapterTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ChildRecursion();
    }

    private void ChildRecursion()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            var relative = child.GetRelativePosition(transform);
            relative.x *= -1;
            relative.x -= child.GetRectTransform().rect.width * Math.Sign(relative.x);
            child.position = transform.position + relative;
        }
    }
}
