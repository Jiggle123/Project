using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScene : PlaneBase
{
    public Transform m_mainTransform, m_userTransform;

    public Toggle m_userToggle;

    public Text userNameText, dataText;

    public Button amendButton;

    public Transform UserPlane, squarePlane;

    public Toggle squareToggle,coursesrToggle;

    public Animator playAnimaitor;

    public Button payButton,forwardButton,backButton;

    public Sprite[] sprites;

    bool isPay=true;

    public Button WDButton, TJButton;

    public Transform mediaPlane;

    public Transform CoursesrPlane;

    private void Awake()
    {
        RegisterUIEvent();
    }


    private void OnEnable()
    {
          Screen.SetResolution(1920, 1080, true); // 这里设置分辨率为1920*1080，并以全屏模式执行
       

    }

    private void RegisterUIEvent()
    {
        m_userToggle.onValueChanged.AddListener(OnUserToggleChanger);
        amendButton.onClick.AddListener(()=>
        {
            GameManager.m_shelf.CutPlane(GameManager.PlaneType.EditorPlane);
        });
        squareToggle.onValueChanged.AddListener((a) =>
        {
            squarePlane.gameObject.SetActive(a);

        });
        coursesrToggle.onValueChanged.AddListener((a) =>
        {
            CoursesrPlane.gameObject.SetActive(a);

        });
        payButton.onClick.AddListener(() =>
        {
            if(isPay)
            {
                playAnimaitor.speed = 1;
                payButton.GetComponent<Image>().sprite = sprites[1];
            }
            else
            {
                playAnimaitor.speed = 0;
                payButton.GetComponent<Image>().sprite = sprites[0];
            }
            isPay = !isPay;
        });

        forwardButton.onClick.AddListener(() =>
        {
         float v=   playAnimaitor.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playAnimaitor.Play("太极", -1, v+0.1f);
        });
        backButton.onClick.AddListener(() =>
        {
           float v = playAnimaitor.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playAnimaitor.Play("舞蹈", -1, v - 0.1f);
        });
        WDButton.onClick.AddListener(() =>
        {
            playAnimaitor.Play("舞蹈", -1, 0);
            mediaPlane.gameObject.SetActive(true);
        });
        TJButton.onClick.AddListener(() =>
        {
            playAnimaitor.Play("太极", -1, 0);
            mediaPlane.gameObject.SetActive(true);
        });
    }

    private void OnUserToggleChanger(bool isOn)
    {
        if (isOn)
        {
            UserShow();
            MainHide();

            return;
        }

        MainShow();
    }

    public void MainShow()
    {
        m_mainTransform.gameObject.SetActive(true);
        UserPlane.gameObject.SetActive(false);
    }

    public void MainHide()
    {
        m_mainTransform.gameObject.SetActive(false);
    }

    public void UserShow()
    {
        m_userTransform.gameObject.SetActive(true);
    }

    public void UserHide()
    {
        m_userTransform.gameObject.SetActive(false);
    }

    public override void EnterPlane()
    {
        this.gameObject.SetActive(true);

        userNameText.text = GameManager.m_shelf.userData.userName;
        dataText.text = GameManager.m_shelf.userData.data;

        MainShow();
        UserHide();
    }

    public override void ExitPlane()
    {
        this.gameObject.SetActive(false);
        MainHide();
        UserHide();
    }
}
