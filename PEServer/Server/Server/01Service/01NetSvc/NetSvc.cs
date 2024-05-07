
using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session, GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

public class NetSvc
{
    private static NetSvc instance = null;
    public static NetSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetSvc();
            }
            return instance;
        }
    }
    public static readonly string obj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public void Init(string Ip, int Port)
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();

        server.StartAsServer(Ip, Port);
        System.Console.WriteLine(string.Format("Ip:{0},Port:{1}", Ip, Port));
    }

    public void AddMsgQue(ServerSession session, GameMsg msg)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(new MsgPack(session, msg));
        }
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            //PECommon.Log("QueCount:" + msgPackQue.Count);
            lock (obj)
            {
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        CMD cmd = (CMD)pack.msg.cmd;

        switch (cmd)
        {
            case CMD.None:
                break;
            case CMD.ReqLogin:
                UserData userData = ServerRoot.Instance._connHelper.GetResultByUserid(pack.msg.reqLogin.emil);

                bool isLogin = (userData != null);

                RspLogin rspLogin = new RspLogin
                {
                    isLogin = isLogin,
                    userData = userData
                };

                GameMsg gameMsg = new GameMsg
                {
                    cmd = (int)CMD.RspLogin,
                    rspLogin = rspLogin
                };

                pack.session.SendMsg(gameMsg);
                break;
            case CMD.ReqRegister:

                UserData RegisterData = ServerRoot.Instance._connHelper.GetResultByUserid(pack.msg.resRegister.emial);

                bool registerState = false;

                if (RegisterData == null)
                {
                    RegisterData = new UserData("", "", "", "", "", pack.msg.resRegister.emial, pack.msg.resRegister.password);

                    registerState = ServerRoot.Instance._connHelper.InsertData(RegisterData);

                    if(registerState)
                        RegisterData = ServerRoot.Instance._connHelper.GetResultByUserid(RegisterData.email);
                }

                GameMsg ReqRegisterMsg = new GameMsg
                {
                    cmd = (int)CMD.RspRegister,
                    rspRegister = new RspRegister
                    {
                        isRegister = registerState,
                        userData= RegisterData,
                    }
                };

                pack.session.SendMsg(ReqRegisterMsg);
                break;

            case CMD.ResInsertion:
                bool ResInsertionState = ServerRoot.Instance._connHelper.UpdateOrAddResult(pack.msg.resInsertion.userData);

                GameMsg ReqRegisterResInsertionMsg = new GameMsg
                {
                    cmd = (int)CMD.RspInsertion,
                    rspInsertion = new RspInsertion
                    {
                        isInsertion=ResInsertionState
                     
                    }
                };

                pack.session.SendMsg(ReqRegisterResInsertionMsg);
                break;
            case CMD.ResUpdate:
                bool UpdateState = ServerRoot.Instance._connHelper.UpdateOrAddResult(pack.msg.resInsertion.userData);

                GameMsg UpdateMsg = new GameMsg
                {
                    cmd = (int)CMD.RspUpdate,
                    rspInsertion = new RspInsertion
                    {
                        isInsertion = UpdateState
                    }
                };

                pack.session.SendMsg(UpdateMsg);
                break;
            default:
                break;
        }
    }
}
