using System.Collections.Generic;

public enum RoomState
{
    Start,//已开始
    Ready,//准备
    AllFull//房间已满
}

public class Room
{
    private List<UserData> _roomAllUser = new List<UserData>();

    public RoomState roomState = RoomState.Start;

    public string roomParentName;

    public string roomName;

    public Room(string roomParentName, string roomName)
    {
        this.roomParentName = roomParentName;
        this.roomName = roomName;
    }
}

