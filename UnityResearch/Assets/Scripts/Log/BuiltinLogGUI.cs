﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Text;
using Object = System.Object;

/// <summary>
/// BuiltinLogGui 内置LogGUI代码
/// 1.会根据
/// </summary>
public class BuiltinLogGUI : MonoBehaviour
{

    public class LogInfo
    {
        public string log;
        public LogInfoType type;
    }
    
    private class GuiAction
    {
        public string ActionName = "";
        public System.Action Action = null;
        public static Stack<GuiAction> SelfPool = new Stack<GuiAction>();
        
        /// <summary>
        /// 对象池申请
        /// </summary>
        /// <param name="actionName">申请注册的按钮名称</param>
        /// <param name="action">申请注册的回调</param>
        /// <returns>GuiAction对象</returns>
        public static GuiAction Allocate(string actionName = "",System.Action action = null)
        {
            if (SelfPool.Count > 0)
            {
                return SelfPool.Pop();
            }
            else
            {
                return new GuiAction(actionName,action);
            }
        }
        
        private GuiAction(string name,System.Action callFunc)
        {
            ActionName = name;
            Action = callFunc;
        }
        
        /// <summary>
        /// 对象池回收
        /// </summary>
        public void Recycle()
        {
            SelfPool.Push(this);
        }
    }
    
    public enum LogInfoType
    {
        Log,
        Warn,
        Error,
        Other,
        All
    }

    #region 配置信息

    private string _colorFormatStr = "<color={0}>{1}</color>";
    private string _togShowLogStr = "ShowLog";

    private string _colorWhite = "#ffffff";
    private string _colorBlue = "#66ccff";
    private string _colorYellow = "#ffcc66";
    private string _colorRed = "#ff0000";
    private string _colorGreen = "#00cc00";
    #endregion

    #region 公有变量

    //public Text TextReceiver;
    //public Button BtnSendType;
    //public Button BtnSend;
    //[HideInInspector]
    //public bool IsSync = false;
    //[HideInInspector]
    //public string StrReciever = "";

    [Tooltip("存储的Log信息数量上限")]
    public int MaxLogCount = 100;
    
    public Vector4 Padding = Vector4.one;
    #endregion

    #region 私有变量
    private List<LogInfo> _logStrBuffer = new List<LogInfo>();

    private GUIStyle _toggleStyle;
    private GUIContent _toggleContent;
    private GUIStyle _logBox;

    private bool _isShowLogWindow = false;
    private Vector2 _scrollVec = Vector2.zero;
    private float _scrollContentHeight = 0;
    private Rect _toggleRect;
    private Rect _windowRect;
    private LogInfoType _localLogState = LogInfoType.All;
    private string _inputTxt = "Search for log";
    private bool isSearch = false;
    private Dictionary<Type , List<GuiAction>> _typeActionBuffer = new Dictionary<Type , List<GuiAction>>();

    private int LogBufferCount => _logStrBuffer.Count;
    
    #endregion

    #region OnGui绘制
    private void Awake()
    {
        Application.logMessageReceived += OnLogReceive;
    }

    void Start()
    {
        int width = 170;
        int height = 50;
        int y = 150;
        _toggleRect = new Rect(Screen.width / 2 - width / 2, y, width, height);
        _windowRect = new Rect(Padding.x,y + 80 + Padding.y,Screen.width - Padding.z - Padding.x,Screen.height * 0.5f - Padding.w - Padding.y);
        _toggleStyle = new GUIStyle()
        {
            fontSize = 40,
            richText = true
        };

        _toggleContent = new GUIContent()
        {
            image = null,
            text = String.Format(_colorFormatStr, _colorBlue, _togShowLogStr)
        };
    }

    /// <summary>
    /// 接受UnityLog信息
    /// </summary>
    /// <param name="condition">Log信息</param>
    /// <param name="stackTrace">堆栈信息</param>
    /// <param name="type">Log类型</param>
    private void OnLogReceive(string condition, string stackTrace, LogType type)
    {
        LogInfo log = new LogInfo();
        log.log = condition + stackTrace.Remove(stackTrace.Length - 1);
        switch (type)
        {
            case LogType.Log:
                log.type = LogInfoType.Log;
                break;
            case LogType.Warning:
                log.type = LogInfoType.Warn;
                break;
            case LogType.Error:
            case LogType.Exception:
                log.type = LogInfoType.Error;
                break;
            default:
                log.type = LogInfoType.Other;
                break;
        }
        _logStrBuffer.Add(log);

        if (LogBufferCount > MaxLogCount)
        {
            _logStrBuffer.RemoveAt(0);
        }
    }

    /// <summary>
    /// GUI绘制
    /// </summary>
    private void OnGUI()
    {
        //绘制控制的开关，这里用的是Toggle
        _isShowLogWindow = DrawToggle();
        if (_isShowLogWindow)
        {
            //打开的时候Toggle字体为黄色
            _toggleContent.text = String.Format(_colorFormatStr, _colorYellow, _togShowLogStr);
            DrawLogWindow();
            DrawExtraBtn();
            // DrawMemoryInfo();
        }
        else
        {
            //没打开的时候就是淡蓝色
            _toggleContent.text = String.Format(_colorFormatStr, _colorBlue, _togShowLogStr);
        }
    }
    

    /// <summary>
    /// 绘制Toggle开关
    /// </summary>
    /// <returns></returns>
    private bool DrawToggle()
    {
        GUI.Box(_toggleRect, "");
        bool flag =  GUI.Toggle(_toggleRect,_isShowLogWindow,_toggleContent,_toggleStyle);
        return flag;
    }
    /// <summary>
    /// 绘制Log窗口
    /// </summary>
    private void DrawLogWindow()
    {
        GUI.Window(0,_windowRect,OnWindow, _togShowLogStr);
    }
    
    /// <summary>
    /// 绘制注册的回调按钮
    /// </summary>
    private void DrawExtraBtn()
    {
        int line = 3;
        int row = 5;
        float width = _windowRect.width;
        float height = _windowRect.y;
        float btnWidth = width / row;
        float btnHeight = height / line;

        for (int i = 0,index = 0; i < _typeActionBuffer.Count; i++)
        {
            var _actionBuffer = _typeActionBuffer.ElementAt(i);

            for (int j = 0;j < _actionBuffer.Value.Count; j++)
            {
                int y = index / line;
                int x = index % line;
                GuiAction actionObj = _actionBuffer.Value[j];
                var rect = new Rect(
                    _windowRect.x + x * btnWidth,
                    0 + btnHeight * y,
                    btnWidth,
                    btnHeight);
                var flag = GUI.Button(rect,"<size=30>" + actionObj.ActionName +"</size>");
                if (flag)
                {
                    actionObj.Action?.Invoke();
                }

                index++;
            }
        }
    }
    
    /// <summary>
    /// 绘制Log窗口的内容
    /// </summary>
    /// <param name="id"></param>
    private void OnWindow(int id)
    {
        Vector2 startPos = new Vector2(0,25);
        float height = 50;
        float width = _windowRect.width / 4 - 2;
        float cor = _windowRect.width / 8 - width / 2;

        //绘制Log页面的四个分类按钮
        var _typeButton = new GUIStyle(GUI.skin.button);
        _typeButton.fontSize = 30;
        if (GUI.Button(new Rect(startPos.x + _windowRect.width / 4 * 0 + cor, startPos.y, width, height), "All", _typeButton))
        {
            _localLogState = LogInfoType.All;
        }
        if (GUI.Button(new Rect(startPos.x + _windowRect.width / 4 * 1 + cor, startPos.y, width, height), "Log", _typeButton))
        {
            _localLogState = LogInfoType.Log;
        }
        if (GUI.Button(new Rect(startPos.x + _windowRect.width / 4 * 2 + cor, startPos.y, width, height), "Warn", _typeButton))
        {
            _localLogState = LogInfoType.Warn;
        }
        if (GUI.Button(new Rect(startPos.x + _windowRect.width / 4 * 3 + cor, startPos.y, width, height), "Error", _typeButton))
        {
            _localLogState = LogInfoType.Error;
        }

        //绘制输入框,Search按钮,Clear按钮
        float inputWidth = _windowRect.width * 0.6f;
        var inputStyle = new GUIStyle();
        inputStyle.fontSize = 33;
        inputStyle.alignment = TextAnchor.MiddleLeft;
        _inputTxt = GUI.TextField(new Rect(10, 80, inputWidth, 50), _inputTxt, inputStyle);
        GUI.Box(new Rect(10, 80, inputWidth, 50), "");

        if (GUI.Button(new Rect(inputWidth + 15, 80,_windowRect.width * 0.2f,50),"Search",_typeButton))
        {
            isSearch = true;
        }
        if (GUI.Button(new Rect(inputWidth + _windowRect.width * 0.2f + 15, 80, _windowRect.width * 0.2f, 50), "Clear", _typeButton))
        {
            isSearch = false;
            _inputTxt = "";
        }

        //绘制ScrollView并展示Log信息
        Rect positionRect = new Rect(0, 135, _windowRect.width, _windowRect.height - 130);
        Rect viewRect = new Rect(0, 0, _windowRect.width, _scrollContentHeight);
        //Rect bottomRect = new Rect(0, _scrollContentHeight, Screen.width, _scrollContentHeight);
        _scrollVec = GUI.BeginScrollView(positionRect, _scrollVec, viewRect);


        if (_logBox == null)
        {
            _logBox = new GUIStyle();
            _logBox.imagePosition = ImagePosition.TextOnly;
            _logBox.alignment = TextAnchor.LowerLeft;
            _logBox.fontSize = 20;
            _logBox.richText = true;
            _logBox.wordWrap = true;
        }
        
        _scrollContentHeight = 0;
        for (int i = 0; i < LogBufferCount; i++)
        {
            var log = _logStrBuffer[i];
            if (!isSearch && (log.type != _localLogState && _localLogState != LogInfoType.All)) { continue; }
            if (isSearch && !log.log.ToLower().Contains(_inputTxt.ToLower())) { continue; }
            if (log.type == LogInfoType.Log)
            {
                _logBox.normal.textColor = Color.white;
            }else if (log.type == LogInfoType.Warn)
            {
                _logBox.normal.textColor = Color.yellow;
            }
            else if (log.type == LogInfoType.Error)
            {
                _logBox.normal.textColor = Color.red;
            }
            else
            {
                _logBox.normal.textColor = Color.blue;
            }

            var boxHeight =  _logBox.CalcHeight(new GUIContent(log.log), _windowRect.width - 20);
            GUI.Box(new Rect(10, _scrollContentHeight, _windowRect.width - 20, boxHeight), log.log, _logBox);
            _scrollContentHeight += boxHeight + 5;
        }

        GUI.EndScrollView();
    }
    
    
    // private void DrawMemoryInfo()
    // {
    //     float height = 50;
    //     float width = 300;
    //     GUI.TextArea(new Rect(_windowRect.x, _windowRect.y + _windowRect.height + height / 2, width,height),
    //         "Memory:" + GlobalMemory);
    // }
    
    // [DllImport("kernel32")]
    // public static extern void GlobalMemoryStatus(ref MEMORY_INFO memory);
    //
    // /// <summary>
    // /// 获取当前内存信息
    // /// </summary>
    // private void GetMemoryStatus()
    // {
    //     GlobalMemoryStatus(ref _localMemory);
    // }
    #endregion

    #region 公有方法
    
    /// <summary>
    /// 注册一个OnGui的Button,默认以BuiltinLogGui为关键字注册
    /// </summary>
    /// <param name="btnName">按钮文本</param>
    /// <param name="callback">点击回调</param>
    public void RegisterOnGUIButton(string btnName,Action callback)
    {
        var key = typeof(BuiltinLogGUI);
        _typeActionBuffer.TryGetValue(key, out var list);
        if (list != null)
        {
            list.Add(GuiAction.Allocate(btnName,callback));
        }
        else
        {
            list = new List<GuiAction>();
            _typeActionBuffer.Add(key,list);
            list.Add(GuiAction.Allocate(btnName,callback));
        }
    }
    
    /// <summary>
    /// 注册一个OnGui的Button,选择一个指定类作为关键字注册
    /// </summary>
    /// <param name="btnName">按钮文本</param>
    /// <param name="callback">点击回调</param>
    public void RegisterOnGUIButton<T>(string btnName,Action callback)
    {
        var key = typeof(T);
        _typeActionBuffer.TryGetValue(key, out var list);
        if (list != null)
        {
            list.Add(GuiAction.Allocate(btnName,callback));
        }
        else
        {
            list = new List<GuiAction>();
            _typeActionBuffer.Add(key,list);
            list.Add(GuiAction.Allocate(btnName,callback));
        }
    }
    
    /// <summary>
    /// 取消注册指定类为关键字的按钮
    /// </summary>
    /// <typeparam name="T">指定的类</typeparam>
    public void UnregisterOnGuiButton<T>()
    {
        var key = typeof(T);
        _typeActionBuffer.TryGetValue(key, out var list);
        if (list == null || list.Count <= 0)
        {
            return;
        }

        foreach (var elem in list)
        {
            elem.Recycle();
        }
        
        list.Clear();
    }
    
    #endregion

    #region 邮件发送
    private static string _mailSender = "2684352466@qq.com";
    private static string _smtpPwd = "hrtvkbapqbpfecii";
    private static string _preTitle = "[GameLogEmail][Product:{0}][Time:{1}]";
    private static string _smtpServer = "smtp.qq.com";
    
    
    /// <summary>
    /// 发送异常日志邮件
    /// </summary>
    static void SendErrorEmail(string reciever,bool isSync)
    {
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_mailSender); // 发送人
        mailMessage.To.Add(reciever); // 收件人 可以多个     
        mailMessage.Subject = string.Format(_preTitle,Application.productName,DateTime.Now.ToShortDateString()) ; // 标题
        mailMessage.Body += "ErrorLog:\n"; // 正文
        //添加附件-日志文件
        //if (System.IO.File.Exists(outputLog))
        //    mailMessage.Attachments.Add(new Attachment(outputLog));
        SmtpClient smtpServer = new SmtpClient(_smtpServer);  // 所使用邮箱的SMTP服务器
        // 账号授权码 邮箱开通SMTP服务，可生成授权码
        smtpServer.Credentials = new System.Net.NetworkCredential(_mailSender, _smtpPwd) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        //发送邮件回调方法
        smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        if (isSync)
        {
            smtpServer.Send(mailMessage);   //同步发送

        }
        else
        {
            smtpServer.SendAsync(mailMessage, new object()); //异步发送

        }
    }

    //发送回调
    static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {

    }
    
    #endregion
    
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