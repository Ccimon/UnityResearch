using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class BuiltinLogGUI : MonoBehaviour
{

    public class LogInfo
    {
        public string log;
        public LogInfoType type;
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
    private static string _mailSender = "2684352466@qq.com";
    private static string _smtpPwd = "hrtvkbapqbpfecii";
    private static string _preTitle = "[GameLogEmial][Product:{0}][Time:{1}]";
    private static string _smtpServer = "smtp.qq.com";

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

    private int LogBufferCount => _logStrBuffer.Count;
    #endregion

    #region OnGui绘制
    private void Awake()
    {
        Application.logMessageReceived += OnLogReceive;
    }



    void Start()
    {
        //StrReciever = "2942693781@qq.com";
        int width = 170;
        int height = 50;
        int y = 150;
        _toggleRect = new Rect(Screen.width / 2 - width / 2, y, width, height);
        _windowRect = new Rect(0,y + 80,Screen.width,Screen.height * 0.5f);
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

    private void OnGUI()
    {

        _isShowLogWindow = DrawToggle();
        if (_isShowLogWindow)
        {
            _toggleContent.text = String.Format(_colorFormatStr, _colorYellow, _togShowLogStr);
            DrawLogWindow();
            DrawButtonGroup();
        }
        else
        {
            _toggleContent.text = String.Format(_colorFormatStr, _colorBlue, _togShowLogStr);
        }
    }

    private void DrawButtonGroup()
    {

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
    /// 绘制Log窗口的内容
    /// </summary>
    /// <param name="id"></param>
    private void OnWindow(int id)
    {
        Vector2 startPos = new Vector2(0,25);
        float height = 50;
        float width = _windowRect.width / 4 - 2;

        //GUI.Button(new Rect(startPos.x, startPos.y + _windowRect.height / 4 * 0, width, height), "Log");
        //GUI.Button(new Rect(startPos.x, startPos.y + _windowRect.height / 4 * 1, width, height), "Log");
        //GUI.Button(new Rect(startPos.x, startPos.y + _windowRect.height / 4 * 2, width, height), "Log");
        //GUI.Button(new Rect(startPos.x, startPos.y + _windowRect.height / 4 * 3, width, height), "Log");

        float cor = _windowRect.width / 8 - width / 2;

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

        float inputWidth = Screen.width - 240;
        var inputStyle = new GUIStyle();
        inputStyle.fontSize = 33;
        inputStyle.alignment = TextAnchor.MiddleLeft;
        _inputTxt = GUI.TextField(new Rect(10, 80, inputWidth, 50), _inputTxt, inputStyle);
        GUI.Box(new Rect(10, 80, inputWidth, 50), "");

        if (GUI.Button(new Rect(inputWidth + 15, 80,110,50),"Search",_typeButton))
        {
            isSearch = true;
        }
        if (GUI.Button(new Rect(inputWidth + 130, 80, 100, 50), "Clear", _typeButton))
        {
            isSearch = false;
            _inputTxt = "";
        }

        Rect positionRect = new Rect(0, 135, Screen.width, _windowRect.height - 50);
        Rect viewRect = new Rect(0, 0, Screen.width, _scrollContentHeight);
        //Rect bottomRect = new Rect(0, _scrollContentHeight, Screen.width, _scrollContentHeight);
        _scrollVec = GUI.BeginScrollView(positionRect, _scrollVec, viewRect);

        _logBox = new GUIStyle(GUI.skin.box);
        _logBox.imagePosition = ImagePosition.TextOnly;
        _logBox.alignment = TextAnchor.LowerLeft;
        _logBox.fontSize = 20;
        _logBox.richText = true;
        _logBox.wordWrap = true;
        //_logBox.wordWrap = true;

        _scrollContentHeight = 0;
        for (int i = 0; i < LogBufferCount; i++)
        {
            var log = _logStrBuffer[i];
            if (!isSearch && (log.type != _localLogState && _localLogState != LogInfoType.All)) { continue; }
            if (isSearch && !log.log.Contains(_inputTxt)) { continue; }
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

            var boxHeight =  _logBox.CalcHeight(new GUIContent(log.log), Screen.width - 20);
            GUI.Box(new Rect(10, _scrollContentHeight, Screen.width - 20, boxHeight), log.log, _logBox);
            _scrollContentHeight += boxHeight + 5;
        }

        GUI.EndScrollView();
    }
    #endregion

    #region 公有方法
    //public void OnInputTextChange()
    //{
    //    StrReciever = TextReceiver.text;
    //}

    //public void OnBtnSendClick()
    //{
    //    SendEmail();
    //}

    //public void OnBtnSendTypeClick()
    //{
    //    IsSync = !IsSync;
    //    RefreshSendType();
    //}

    //private void RefreshSendType()
    //{
    //    string text = IsSync ? "同步" : "异步";
    //    BtnSendType.transform.GetChild(0).GetComponent<Text>().text = text;
    //}
    #endregion

    #region 邮件发送
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

    //非静态封装
    //public void SendEmail()
    //{
    //    SendErrorEmail(StrReciever,IsSync);
    //}
    #endregion
}