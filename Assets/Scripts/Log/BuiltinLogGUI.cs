using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BuiltinLogGUI : MonoBehaviour
{
    #region 配置信息
    private static string _mailSender = "2684352466@qq.com";
    private static string _smtpPwd = "hrtvkbapqbpfecii";
    private static string _preTitle = "[GameLogEmial][Product:{0}][Time:{1}]";

    private string _colorFormatStr = "<color={0}>{1}</color>";
    private string _btnShowLogStr = "ShowLog";

    private string _colorWhite = "#ffffff";
    private string _colorBlue = "#66ccff";
    private string _colorYellow = "#ffcc66";
    private string _colorRed = "#ff0000";
    private string _colorGreen = "00aa00";
    #endregion

    #region 私有变量
    private List<string> _logStrBuffer = new List<string>();

    private GUIStyle _toggleStyle;
    private GUIContent _toggleContent;

    private bool _isShowLogWindow = false;
    private Rect _toggleRect;
    private Rect _boxRect;
    private Rect _windowRect;
    #endregion

    #region 生命周期
    void Start()
    {
        int width = 170;
        int height = 50;
        int y = 150;
        _toggleRect = new Rect(Screen.width / 2 - width / 2, y, width, height);
        _boxRect = new Rect(Screen.width / 2 - width / 2, y, width, height);
        _windowRect = new Rect(0,y + 80,Screen.width,400);

        _toggleStyle = new GUIStyle()
        {
            fontSize = 40,
            richText = true
        };

        _toggleContent = new GUIContent()
        {
            image = null,
            text = String.Format(_colorFormatStr, _colorBlue, _btnShowLogStr)
        };

    }

    private void OnGUI()
    {
        _isShowLogWindow = DrawToggle();
        if (_isShowLogWindow)
        {
            _toggleContent.text = String.Format(_colorFormatStr, _colorYellow, _btnShowLogStr);
            DrawLogWindow();
        }
        else
        {
            _toggleContent.text = String.Format(_colorFormatStr, _colorBlue, _btnShowLogStr);
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
        GUI.Window(0,_windowRect,OnWindow,_btnShowLogStr);
    }

    /// <summary>
    /// 绘制Log窗口的内容
    /// </summary>
    /// <param name="id"></param>
    private void OnWindow(int id)
    {
        int height = 80;
        int inter = 5;
        int bottom = 15;
        GUI.Button(new Rect(20, bottom + (height + inter) * 1, height, height), "Debug");
        GUI.Button(new Rect(20, bottom + (height + inter) * 2, bottom, height), "Log");
        GUI.Button(new Rect(20, bottom + (height + inter) * 3, height, height), "Warn");
        GUI.Button(new Rect(20, bottom + (height + inter) * 4, height, height), "Error");

    }
    #endregion

    #region 邮件发送
    /// <summary>
    /// 发送异常日志邮件
    /// </summary>
    static void SendErrorEmail()
    {
        var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_mailSender); // 发送人
        mailMessage.To.Add(_mailSender); // 收件人 可以多个     
        mailMessage.Subject = string.Format(_preTitle,Application.productName,DateTime.Now.ToShortDateString()) ; // 标题
        mailMessage.Body += "ErrorLog:\n"; // 正文
        //添加附件-日志文件
        //if (System.IO.File.Exists(outputLog))
        //    mailMessage.Attachments.Add(new Attachment(outputLog));
        SmtpClient smtpServer = new SmtpClient("smtp.qq.com");  // 所使用邮箱的SMTP服务器
        // 账号授权码 邮箱开通SMTP服务，可生成授权码
        smtpServer.Credentials = new System.Net.NetworkCredential(_mailSender, _smtpPwd) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        //发送邮件回调方法
        smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        //smtpServer.Send(mail);   //同步发送
        smtpServer.SendAsync(mailMessage, new object()); //异步发送
    }

    //发送回调
    static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {

    }

    //非静态封装
    public void SendEmail()
    {
        SendErrorEmail();
    }
    #endregion
}