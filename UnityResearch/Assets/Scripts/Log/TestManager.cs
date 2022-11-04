using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public bool product;

    public bool showLog;
    
    public bool ShowLog
    {
        get => showLog;
        set
        {
            showLog = value;
            ShowLogCheck();
        }
    }
    
    // Start is called before the first frame update
    public void ShowLogCheck()
    {
        //iOS加Log不卡，所以无论怎样都加
#if UNITY_IOS
        Debug.unityLogger.logEnabled = true;
#else
        if (showLog)
        {
            Debug.unityLogger.logEnabled = true;
        }
        else
        {
            Debug.unityLogger.logEnabled = false;
        }

#endif
    }
    
}
