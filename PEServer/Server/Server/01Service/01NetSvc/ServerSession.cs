﻿/****************************************************
	文件：ServerSession.cs
	作者：SIKI学院——Plane
	邮箱: 1785275942@qq.com
	日期：2018/12/07 5:09   	
	功能：网络会话连接
*****************************************************/

using PENet;
using PEProtocol;

public class ServerSession : PESession<GameMsg> {
    public int sessionID = 0;

    protected override void OnConnected() {

        sessionID = ServerRoot.Instance.GetSessionID();
         
        System.Console.WriteLine("SessionID:" + sessionID + " Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg) {
        System.Console.WriteLine("SessionID: " + sessionID + "   RcvPack CMD:" + ((CMD)msg.cmd).ToString()+System.DateTime.Now);
        NetSvc.Instance.AddMsgQue(this, msg);
    }

    protected override void OnDisConnected() {
        ServerRoot.Instance.RemvoAtClient(this);
    }
}
