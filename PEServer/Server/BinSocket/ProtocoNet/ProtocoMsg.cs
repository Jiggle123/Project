using PENet;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProtocoNet
{
    public enum MsgType
    {
        None = 100,
        userLogin = 1001,
        userRegister,
        createRoom,
        removeRoom,
        startGame,
        overGame
    }

    public class UserData
    {
        public string userName { get; set; }
        public string passWorld { get; set; }
    }

    [Serializable]
    public class ProjectMsg : PEMsg
    {
        public string msgValue;
    }

}
