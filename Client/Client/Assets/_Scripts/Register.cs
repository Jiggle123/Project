using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : PlaneBase
{
    public InputField m_emailinputField, m_passwordinputField, m_cofirninputField;

    public Button m_signUpButton;

    private void Awake()
    {
        RegsterUIEvent();
    }

    private void RegsterUIEvent()
    {
        m_signUpButton.onClick.AddListener(OnRegistClick);
    }

    private void OnRegistClick()
    {
        if (string.IsNullOrEmpty(m_emailinputField.text) || string.IsNullOrEmpty(m_passwordinputField.text) || string.IsNullOrEmpty(m_cofirninputField.text))
        {
            TopSystem.Shelf.OpenInfor("请输入邮箱或密码......");
            return;
        }
        else if (!m_passwordinputField.text.Equals(m_cofirninputField.text))
        {
            TopSystem.Shelf.OpenInfor("两次密码输入不一致......");
            return;
        }

        GameManager.m_shelf.userData= new UserData("", "", "", "", "", m_emailinputField.text, m_passwordinputField.text);

        GameMsg gameMsg = new GameMsg
        {
            cmd = (int)CMD.ReqRegister,
            resRegister = new ResRegister
            {
                emial = m_emailinputField.text,
                password = m_passwordinputField.text
            }
        };

        NetSys.Slef.SendMsg(gameMsg);
    }

    public override void EnterPlane()
    {
        this.gameObject.SetActive(true);
    }

    public override void ExitPlane()
    {
        this.gameObject.SetActive(false);
    }

    public  void Disponse(MsgPack msgPack)
    {
        if (!msgPack.msg.rspRegister.isRegister)
        {
            TopSystem.Shelf.OpenInfor("存在相同的邮箱......");

            return;
        }

        GameManager.m_shelf.userData = msgPack.msg.rspRegister.userData;

        GameManager.m_shelf.CutPlane(GameManager.PlaneType.RegisterPersonalInformation);
    }
}
