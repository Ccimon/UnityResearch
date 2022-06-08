using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltinLogGUI : MonoBehaviour
{
    private List<string> _logStrBuffer = new List<string>();
    private bool _showLogWindow = false;

    #region 生命周期
    void Start()
    {
            
    }

    private void OnGUI()
    {
        
    }

    private bool DrawToggle()
    {
        int width = 50;
        int height = 50;
        return GUI.Toggle(new Rect(Screen.width/2 - width/2,20,width,height),_showLogWindow,"ShowLog");
    }
    #endregion
}