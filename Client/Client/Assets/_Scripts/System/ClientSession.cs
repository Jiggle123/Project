using PENet;
using PEProtocol;
using UnityEngine;

public class ClientSession : PESession<GameMsg>
{
    public int sessionID = 0;

    protected override void OnConnected()
    {
        Debug.Log(" Client Connect");
    }

    protected override void OnDisConnected()
    {
        Debug.Log(" Client Offline");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Debug.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString()+ System.DateTime.Now);
        NetSys.Slef.AddMsgQue(this, msg);
    }
}

