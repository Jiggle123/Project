using PENet;
using Protocol;
using System;
using System.Collections.Generic;

public class Server
{
    PESocket<ServerSession, NetMsg> _serverConnent;

    public List<Room> _allRoomList = new List<Room>();

    public TXTDBSys dbSys;

    private Dictionary<string, string> _allUserDBDic = new Dictionary<string, string>();//ALLDB数据


    private static Server _Self = null;

    public string Ip;
    public int Port;

    public static Server Instance
    {
        get
        {
            if (_Self == null)
            {
                _Self = new Server();
            }
            return _Self;
        }
    }

    public Server()
    {
        dbSys = new TXTDBSys();

        _allUserDBDic = dbSys.ReadDB(out Ip, out Port);

        _serverConnent = new PESocket<ServerSession, NetMsg>();

        _serverConnent.StartAsServer(Ip, Port);

        PETool.LogMsg(string.Format("IP:{0},PROT:{1}", Ip, Port));
    }


    /// <summary>
    /// 是否存在用户
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public bool UserLoginisPass(UserData userData)
    {
        if (_allUserDBDic.ContainsKey(userData.userName))
        {
            if (_allUserDBDic[userData.userName] == userData.passWorld)
                return true;
        }
        return false;
    }

    /// <summary>
    ///  处理消息
    /// </summary>
    public void DisposeRequest(string msg, ServerSession serverSession)
    {
        CMDMsg CMDMSG = LitJson.JsonMapper.ToObject<CMDMsg>(msg);

        switch ((MsgType)CMDMSG.msgType)
        {

            case MsgType.UserLogin:
                LoginUser(CMDMSG, serverSession);
                break;
            case MsgType.UserRegister:
                RegisterUser(CMDMSG, serverSession);
                break;
            case MsgType.RequestAllRoomData:
                GetAllRoomData(CMDMSG, serverSession);
                break;
        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="msg"></param>
    public void SendMsg(ServerSession clientSession, string msg)
    {
        NetMsg netMsg = new NetMsg
        {
            text = msg
        };

        clientSession.SendMsg(netMsg);
    }

    #region 响应
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="CMDMSG"></param>
    /// <param name="serverSession"></param>
    private void LoginUser(CMDMsg CMDMSG, ServerSession serverSession)
    {
        UserData tempLoginuserData = LitJson.JsonMapper.ToObject<UserData>(CMDMSG.msg);

        bool isState = UserLoginisPass(tempLoginuserData);

        CMDMSG.msg = Convert.ToInt32(isState).ToString();

        string msg = LitJson.JsonMapper.ToJson(CMDMSG);

        SendMsg(serverSession,msg);
    }



    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="CMDMSG"></param>
    /// <param name="serverSession"></param>
    public void RegisterUser(CMDMsg CMDMSG, ServerSession serverSession)
    {
        UserData _tempRegisterUserData = LitJson.JsonMapper.ToObject<UserData>(CMDMSG.msg);

        bool isState = UserLoginisPass(_tempRegisterUserData);

        if (!isState)
        {
            string userAccout = string.Format("{0}|{1}", _tempRegisterUserData.userName, _tempRegisterUserData.passWorld);

            dbSys.WriteDB(userAccout);
        }

        CMDMSG.msg = Convert.ToInt32(isState).ToString();

        string msg = LitJson.JsonMapper.ToJson(CMDMSG);

        SendMsg(serverSession, msg);
    }

    /// <summary>
    /// 获取所有用户数据
    /// </summary>
    /// <param name="CMDMSG"></param>
    /// <param name="serverSession"></param>
    public void GetAllRoomData(CMDMsg CMDMSG, ServerSession serverSession)
    {
        RoomDatas roomData = new RoomDatas();

        List<RoomData> _tempRoomDTs = new List<RoomData>();

        if (_allUserDBDic.Count > 0)
        {
            foreach (var item in _allRoomList)
            {
                RoomData data = new RoomData
                {
                    roomName = item.roomName,
                    roomParentName = item.roomParentName,
                    roomState = (int)item.roomState
                };

                _tempRoomDTs.Add(data);
            }
        }

        roomData._roomDatas = _tempRoomDTs;

        string _GetAllRoomMsg = LitJson.JsonMapper.ToJson(roomData);

        CMDMsg cMDMsg = new CMDMsg
        {
            msg = _GetAllRoomMsg,
            msgType = (int)MsgType.ResponseAllRoomDtata,
            viewType = (int)ViewType.MainView,
        };

        string msg = LitJson.JsonMapper.ToJson(cMDMsg);

        SendMsg(serverSession,msg);
    }
    #endregion

    /// <summary>
    /// 获取所有客户端应用
    /// </summary>
    /// <returns></returns>
    public List<ServerSession> GetAllClient()
    {
        return _serverConnent.GetSesstionLst();
    }

}

