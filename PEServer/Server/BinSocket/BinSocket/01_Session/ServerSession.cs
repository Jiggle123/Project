using BinSocket;
using PENet;
using Protocol;

public class ServerSession : PESession<NetMsg>
{
    protected override void OnConnected()
    {
        PETool.LogMsg("客户端已连接...");      
    }

    protected override void OnDisConnected()
    {
        
        PETool.LogMsg("客户端已关闭...");
    }

    protected override void OnReciveMsg(NetMsg msg)
    {
        Server.Instance.DisposeRequest(msg.text,this);

        PETool.LogMsg("客户端发送请求...");
    }
}

