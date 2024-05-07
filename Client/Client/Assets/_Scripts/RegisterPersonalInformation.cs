using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPersonalInformation : PlaneBase
{
    public Button m_skipButton, m_finishButton;

    public InputField m_userNameIpt, m_dataIpt, m_gnederIpt, m_heightIpt, m_weightIpt;

    private void Awake()
    {
        OnRegsterUIEvent();
    }

    private void OnRegsterUIEvent()
    {
        m_skipButton.onClick.AddListener(OnSkipClick);

        m_finishButton.onClick.AddListener(OnFinishClick);
    }

    private void OnSkipClick()
    {
        GameManager.m_shelf.CutPlane(GameManager.PlaneType.MainPlane);
    }

    private void OnFinishClick()
    {
        if (string.IsNullOrEmpty(m_userNameIpt.text) || string.IsNullOrEmpty(m_dataIpt.text) || string.IsNullOrEmpty(m_gnederIpt.text) || string.IsNullOrEmpty(m_heightIpt.text) || string.IsNullOrEmpty(m_weightIpt.text))
        {
            TopSystem.Shelf.OpenInfor("数据不能为空......");
            return;
        }
        GameManager.m_shelf.userData.userName = m_userNameIpt.text;
        GameManager.m_shelf.userData.data = m_dataIpt.text;
        GameManager.m_shelf.userData.gneder = m_gnederIpt.text;
        GameManager.m_shelf.userData.height = m_heightIpt.text;
        GameManager.m_shelf.userData.weigh = m_weightIpt.text;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ResInsertion,
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

    public void Disponse(MsgPack msgPack)
    {
        if (msgPack.msg.rspInsertion.isInsertion)
            GameManager.m_shelf.CutPlane(GameManager.PlaneType.MainPlane);
    }
}
