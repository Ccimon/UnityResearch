using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var cell = GetComponentInChildren<ViewTest>();
        cell.Init(1004);
        Debug.Log(cell is ICell);
    }
    
    
}
