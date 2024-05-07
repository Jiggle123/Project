using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopSystem : MonoBehaviour
{
    public static TopSystem Shelf = null;

    private Text m_inforText;

    private void Awake()
    {
        m_inforText = GetComponent<Text>();

        Shelf = this;

        this.gameObject.SetActive(false);
    }

    public void OpenInfor(string infor)
    {
        gameObject.SetActive(true);

        StartCoroutine(Open(infor));
    }

    private IEnumerator Open(string str)
    { 
        m_inforText.text = str;

        yield return new WaitForSeconds(5);

        m_inforText.text = "";

        gameObject.gameObject.SetActive(false);
    }
}
