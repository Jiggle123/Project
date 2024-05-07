using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class EditorPlane : PlaneBase
{
    public Button m_updateButton, m_cancelButton;

    public InputField m_userNameIpt, m_dataIpt, m_gnederIpt, m_heightIpt, m_weightIpt, m_password;

    private void Awake()
    {
        m_cancelButton.onClick.AddListener(() => { GameManager.m_shelf.CutPlane(PlaneType.MainPlane); });
        m_updateButton.onClick.AddListener(OnUptateClick);
    }

    private void OnUptateClick()
    {
        if (string.IsNullOrEmpty(m_userNameIpt.text) || string.IsNullOrEmpty(m_dataIpt.text) || string.IsNullOrEmpty(m_gnederIpt.text) || string.IsNullOrEmpty(m_heightIpt.text) || string.IsNullOrEmpty(m_weightIpt.text) || string.IsNullOrEmpty(m_password.text))
        {
            TopSystem.Shelf.OpenInfor("数据不能为空......");
            return;
        }

        GameManager.m_shelf.userData.userName = m_userNameIpt.text;
        GameManager.m_shelf.userData.data = m_dataIpt.text;
        GameManager.m_shelf.userData.gneder = m_gnederIpt.text;
        GameManager.m_shelf.userData.height = m_heightIpt.text;
        GameManager.m_shelf.userData.weigh = m_weightIpt.text;
        GameManager.m_shelf.userData.password = m_password.text;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ResUpdate,
            resInsertion = new ResInsertion
            {
                userData = GameManager.m_shelf.userData
            }
        };

        NetSys.Slef.SendMsg(msg);
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
        if (msgPack.msg.rspInsertion.isInsertion)
            GameManager.m_shelf.CutPlane(GameManager.PlaneType.MainPlane);
    }
}
