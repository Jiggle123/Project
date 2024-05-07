using System.Collections.Generic;

public class DBConfig
{
    public string DBIP;
    public string DBPORT;
    public string DBName;
    public string userName;
    public string UserPassword;
}

public class Constant
{
    public const  string unitTable = "UnityTab";

    public static string path_url = "D:/DB.txt";
}

public class CMDMsg 
{
    public int viewType { get; set; }

    public int msgType { get; set; }

    public string msg { get; set; }
}

public class UserData
{
    public string userName { get; set; }
    public string passWorld { get; set; }
}

public enum ViewType
{
    None,
    userLoginView = 1001,
    userRegisterView,
    MainView,
}

public enum MsgType
{
    UserLogin=3001,
    UserRegister,
    RequestAllRoomData,
    ResponseAllRoomDtata
}

//------房间数据
public class RoomData
{
    public int roomState { get; set; }
    public string roomName { get; set; }
    public string roomParentName { get; set; }
}
public class RoomDatas
{
    public List<RoomData> _roomDatas { get; set; }
}
//--------------