using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOpration : MonoBehaviour
{

    public Button BlurButton;
    public GaussBlur BlurComponent;
    public Text LogText;
    
    private bool _isBlur = false;
    
    private void Start()
    {
        BlurButton.onClick.AddListener(OnBtnBlurClick);
        Application.logMessageReceived += LogRecieve;
    }

    private void OnBtnBlurClick()
    {
        _isBlur = !_isBlur;
        Debug.Log("BlurEnabled:" + _isBlur);
        BlurComponent.enabled = _isBlur;
    }

    private void LogRecieve(string condition,string trace,LogType type)
    {
        LogText.text = condition;
    }
}
