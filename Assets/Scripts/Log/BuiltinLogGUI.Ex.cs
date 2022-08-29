using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BuiltinLogGUI
{

    private static BuiltinLogGUI _instance;

    public static BuiltinLogGUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("TestManager").GetComponent<BuiltinLogGUI>();
            }

            return _instance;
        }
    }
    
    protected BuiltinLogGUI()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    
}
