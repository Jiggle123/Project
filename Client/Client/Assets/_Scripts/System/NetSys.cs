using PENet;
using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MsgPack
{
    public ClientSession session;
    public GameMsg msg;
    public MsgPack(ClientSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}
public class NetSys : MonoBehaviour
{
    private PESocket<ClientSession, GameMsg> _clientNet;

    public string IP = "127.0.0.1";
    public int Prot = 3154;

    public static NetSys Slef = null;

    public static readonly string obj = "lock";

    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    private void Awake()
    {
        Slef = this;

        Init(IP, Prot);
    }

    /// <summary>
    /// GetView
    /// </summary>
    /// <param name="viewType"></param>
    /// <returns></returns>
    private  void Init(string Ip, int Port)
    {
        _clientNet = new PESocket<ClientSession, GameMsg>();

        _clientNet.StartAsClient(Ip, Port);
    }

    /// <summary>
    /// 添加队列
    /// </summary>
    /// <param name="session"></param>
    /// <param name="msg"></param>
    public void AddMsgQue(ClientSession session, GameMsg msg)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(new MsgPack(session, msg));
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void SendMsg(GameMsg msg)
    {
        try
        {
            _clientNet.session.SendMsg(msg);
        }
        catch (Exception e)
        {
            TopSystem.Shelf.OpenInfor("请检查网络连接......");
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    private void OnDestroy()
    {
        _clientNet.Close();
    }

    /// <summary>
    ///  处理消息
    /// </summary>
    public void DisposeResponst(MsgPack msgPack)
    {
        CMD msgCmd = (CMD)msgPack.msg.cmd;
        switch (msgCmd)
        {
            case CMD.None:
                break;
            case CMD.RspLogin:
                LoginPlane loginPlane= GameManager.m_shelf.GetPlane(GameManager.PlaneType.LoginPlane) as LoginPlane;
                loginPlane.Disponse(msgPack);
                break;
            case CMD.RspRegister:
                Register register = GameManager.m_shelf.GetPlane(GameManager.PlaneType.Register) as Register;
                register.Disponse(msgPack);
                break;
            case CMD.RspInsertion:
                RegisterPersonalInformation registerPersonalInformation = GameManager.m_shelf.GetPlane(GameManager.PlaneType.RegisterPersonalInformation) as RegisterPersonalInformation;
                registerPersonalInformation.Disponse(msgPack);
                break;
            case CMD.RspUpdate:
                EditorPlane editorPlane = GameManager.m_shelf.GetPlane(GameManager.PlaneType.EditorPlane) as EditorPlane;
                editorPlane.Disponse(msgPack);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 设置鼠标状态
    /// </summary>
    /// <param name="_cursorState">状态：true显示，false隐藏</param>
    public void SetCursor(bool _cursorState)
    {
        //隐藏鼠标
        Cursor.visible = _cursorState;
        
        Cursor.lockState = _cursorState ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            lock (obj)
            {
                MsgPack pack = msgPackQue.Dequeue();
                DisposeResponst(pack);
            }
        }
    }
}



