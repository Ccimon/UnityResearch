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
        _showLogWindow = DrawToggle();
        if (_showLogWindow)
        {
            Debug.Log("????");
            return;
        }
        Debug.Log("!!!!");
    }

    private bool DrawToggle()
    {
        int width = 500;
        int height = 300;
        return GUI.Toggle(new Rect(Screen.width/2 - width/2,Screen.height/2,width,height),_showLogWindow,"ShowLog");
    }
    #endregion
}