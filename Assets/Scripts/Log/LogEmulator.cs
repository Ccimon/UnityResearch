using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEmulator : MonoBehaviour
{
    // Start is called before the first frame update
    float time = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            Debug.Log("TestLog" + time);
            time--;
        }
    }
}
