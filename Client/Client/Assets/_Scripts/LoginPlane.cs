using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class LoginPlane : PlaneBase
{
    public InputField m_emilInputField, m_passwordInputField;

    public Button m_loginButton, m_openRegertButton;

    private void Awake()
    {
        RegisterUIEvent();
    }

    private void RegisterUIEvent()
    {
        m_openRegertButton.onClick.AddListener(OpenRegiterClick);

        m_loginButton.onClick.AddListener(OnLoginClick);
    }

    private void OnLoginClick()
    {
        if (string.IsNullOrEmpty(m_emilInputField.text) || string.IsNullOrEmpty(m_passwordInputField.text))
        {
            TopSystem.Shelf.OpenInfor("«Î ‰»Î” œ‰ªÚ√‹¬Î......");
            return;
        }

        UserData userData1 = new UserData("", "", "", "", "", m_emilInputField.text, m_passwordInputField.text);

        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)CMD.ReqLogin,
            reqLogin = new ReqLogin
            {
                emil = m_emilInputField.text,
                password = m_passwordInputField.text
            }
        };

        NetSys.Slef.SendMsg(gameMsg);

        Clear();
    }

    private void Clear()
    {
        m_emilInputField.text = "";
        m_passwordInputField.text = "";
    }

    private void OpenRegiterClick()
    {
        GameManager.m_shelf.CutPlane(PlaneType.Register);
    }

    public override void EnterPlane()
    {
        this.gameObject.SetActive(true);
    }

    public override void ExitPlane()
    {
        Clear();

        this.gameObject.SetActive(false);
    }

 public  void Disponse(MsgPack msgPack)
    {
        if (!msgPack.msg.rspLogin.isLogin)
        {
            TopSystem.Shelf.OpenInfor("√‹¬Î¥ÌŒÛ......");

            return;
        }

        GameManager.m_shelf.userData = msgPack.msg.rspLogin.userData;

        GameManager.m_shelf.CutPlane(GameManager.PlaneType.MainPlane);
    }
}
