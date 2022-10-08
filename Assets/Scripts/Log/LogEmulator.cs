using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEmulator : MonoBehaviour
{
    // Start is called before the first frame update
    float time = 0;
    private int times = 0;
    void Start()
    {
        BuiltinLogGUI.Instance.RegisterOnGUIButton("Test",Say);
    }

    // Update is called once per frame
    void Update()
    {
        // time += Time.deltaTime;
        // if (time > 1)
        // {
        // Debug.Log("TestLog" + time);
        //     time--;
        // }
    }

    void Say()
    {
        times++;
        Debug.Log("Test Call Back");
        if (times >= 10)
        {
            BuiltinLogGUI.Instance.UnregisterOnGuiButton<BuiltinLogGUI>();
        }
    }
}
