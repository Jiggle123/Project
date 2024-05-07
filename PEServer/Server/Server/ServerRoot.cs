using PEProtocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class ServerRoot
{
    private static ServerRoot instance = null;

    public string IP;

    public int PORT = 12306;

    public ConnHelper _connHelper;

    public static ServerRoot Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ServerRoot();
            }
            return instance;
        }
    }

    /// <summary>
    /// 所用的客户端引用
    /// </summary>
    private List<ServerSession> _allClientSessions = new List<ServerSession>();

    /// <summary>
    /// 添加客户端
    /// </summary>
    /// <param name="clientName"></param>
    /// <param name="clientSession"></param>
    /// <returns></returns>
    public bool AddClientSession(ServerSession clientSession)
    {
        if (!_allClientSessions.Contains(clientSession))
        {
            _allClientSessions.Add(clientSession);
            System.Console.WriteLine("客户端建立连接...");
            return true;
        }

        System.Console.WriteLine("客户端异地登录...");

        return false;
    }

    /// <summary>
    /// 移除客户端
    /// </summary>
    /// <param name="clientName"></param>
    /// <returns></returns>
    public bool RemvoAtClient(ServerSession clientSession)
    {
        if (_allClientSessions.Contains(clientSession))
        {
            _allClientSessions.Remove(clientSession);

            System.Console.WriteLine("客户端断开连接...");
            return true;
        }

        System.Console.WriteLine("移除客户端失败，该客户端没有建立连接...");

        return false;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        NetSvc _netSvc = new NetSvc();

        IP = GetLocalIP();

        _netSvc.Init(IP, PORT);

        InitSvc();
    }
    public string GetLocalIP()
    {
        try
        {
            string HostName = Dns.GetHostName(); //得到主机名
            IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    return IpEntry.AddressList[i].ToString();
                }
            }
            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine("获取本机IP出错:" + ex.Message);
            return "";
        }
    }
    /// <summary>
    /// 初始化服务
    /// </summary>
    private void InitSvc()
    { 
        _connHelper = new ConnHelper();
    }

    public void Update()
    {
        NetSvc.Instance.Update();
    }

    private int SessionID = 0;
    public int GetSessionID()
    {
        if (SessionID == int.MaxValue)
        {
            SessionID = 0;
        }
        return SessionID += 1;
    }
}
