using System;
using System.Collections.Generic;
using PENet;

namespace PEProtocol
{
    public enum CMD
    {
        None = 0,

        //登录相关 100
        ReqLogin = 101,
        RspLogin = 102,

        //注册相关 
        ReqRegister,
        RspRegister,

        ResInsertion,
        RspInsertion,

        ResUpdate,
        RspUpdate
    }

    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ResRegister resRegister;
        public RspRegister rspRegister;

        public ResInsertion resInsertion;
        public RspInsertion rspInsertion;
    }

    [Serializable]
    public class UserData
    {
        public int id;
        public string userName;
        public string data;
        public string gneder;
        public string height;
        public string weigh;
        public string email;
        public string password;
        public UserData(string userName, string data, string gneder, string height, string weigh, string email, string password)
        {
            this.userName = userName;
            this.data = data;
            this.gneder = gneder;
            this.height = height;
            this.weigh = weigh;
            this.email = email;
            this.password = password;
            this.id = id;
        }
    }

    [Serializable]
    public class ReqLogin
    {
        public string emil;
        public string password;
    }
    [Serializable]
    public class RspLogin
    {
        public bool isLogin;

        public UserData userData;
    }

    [Serializable]
    public class ResRegister
    {
        public string emial;
        public string password;
    }
    [Serializable]
    public class RspRegister
    {
        public bool isRegister;
        public UserData userData;
    }

    [Serializable]
    public class ResInsertion
    {
        public UserData userData;
    }
    [Serializable]
    public class RspInsertion
    {
        public bool isInsertion;
    }
}