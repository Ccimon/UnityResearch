using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTest : MonoBehaviour,ICell
{
    public void Init(object data)
    {
        Debug.Log("data:" + data.ToString());
    }
}
