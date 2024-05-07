using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

public class MySQLAccesSvc
{
    #region 字段和属性
    private MySqlConnection mySqlConnection = null;//数据库的连接引用
   
    private Server Server;

    private DBConfig dbConfig;
    #endregion

    #region  方法
    public MySQLAccesSvc(DBConfig dBConfig , Server server )
    {
        this.Server = server;
        this.dbConfig = dBConfig;


        ConnectMySQL();//连接
    }

    /// <summary>
    /// 连接SQL
    /// </summary>
    private void ConnectMySQL()
    {
        try
        {
            string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4}", dbConfig.DBName, dbConfig.DBIP, dbConfig.userName, dbConfig.UserPassword, dbConfig.DBPORT);
            mySqlConnection = new MySqlConnection(mySqlString);
            mySqlConnection.Open();//打开
        }
        catch (Exception e)
        {
            Console.WriteLine ("服务器连接失败，请重新检查MySql服务是否打开。" + e.Message.ToString());
            //TipsSystem.Self.OpenTips("数据库连接失败，请重新检查服务是否打开...");
        }
    }

    /// <summary>
    /// 关闭SQL
    /// </summary>
    public void CloseMySQL()
    {
        if (mySqlConnection != null)
        {
            mySqlConnection.Clone();
            mySqlConnection.Dispose();
            mySqlConnection = null;
        }
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public bool QueryUserData(string userName, string userPass)
    {
        try
        {
            string acc = Encrypt(userName);
            string myqStr = string.Format("select * from {0} where Account = @acct", Constant.unitTable);

            using (MySqlCommand cmd = new MySqlCommand(myqStr, mySqlConnection))
            {
                cmd.Parameters.AddWithValue("acct", acc);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string _pass = Decrypt(reader.GetString(dbConfig.UserPassword));//将MD5码转化为正常字符串
                    if (_pass.Equals(userPass))
                    {
                        #region  数据库查询

                        //UserData userData = new UserData
                        //{
                        //    Account = Decrypt(reader.GetString(Constant.Account)),
                        //    Password = Decrypt(reader.GetString(Constant.Password)),
                        //    usetType = (UsetType)(int.Parse(Decrypt(reader.GetString(Constant.UserType)))),
                        //    Phone = Decrypt(reader.GetString(Constant.phone)),
                        //    UserLog = Decrypt(reader.GetString(Constant.UserLog)),
                        //    createTime = Decrypt(reader.GetString(Constant.createTime)),

                        //};
                        #endregion

                        reader.Close();

                        return true;
                    }
                }
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("查询查询数据库时:" + e);
        }
        return false;
    }

    #region 注释
    /////<summary>
    /////根据id来查询用户数据
    ///// </summary>
    //public UserData QueryUserDataById(string id)
    //{
    //    try
    //    {
    //        string myqStr = string.Format("select * from {0} where id = @id", mySQLData.Table);

    //        using (MySqlCommand cmd = new MySqlCommand(myqStr, mySqlConnection))
    //        {
    //            cmd.Parameters.AddWithValue("id", id);
    //            MySqlDataReader reader = cmd.ExecuteReader();
    //            if (reader.Read())
    //            {
    //                UserData userData = new UserData
    //                {
    //                    Account = Decrypt(reader.GetString(Constant.Account)),
    //                    Password = Decrypt(reader.GetString(Constant.Password)),
    //                    usetType = (UsetType)(int.Parse(Decrypt(reader.GetString(Constant.UserType)))),
    //                    Phone = Decrypt(reader.GetString(Constant.phone)),
    //                    UserLog = Decrypt(reader.GetString(Constant.UserLog)),
    //                    createTime = Decrypt(reader.GetString(Constant.createTime)),

    //                };
    //                reader.Close();
    //                return userData;
    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("查询查询数据库时:" + e);
    //    }
    //    return null;
    //}
    ///// <summary>
    ///// 获取数据库中所有的数据
    ///// </summary>
    //public void GetSQLAllData()
    //{
    //    string cmdStr = string.Format("select * from {0};", mySQLData.Table);

    //    MySqlCommand cmd = new MySqlCommand(cmdStr, mySqlConnection);
    //    MySqlDataReader m = cmd.ExecuteReader();//获取读取器返回的是表中所有数据

    //    while (m.Read())
    //    {
    //        UserData userData = new UserData();
    //        userData.id = m[Constant.id].ToString();
    //        userData.Account = Decrypt((string)m[Constant.Account]);
    //        userData.Password = Decrypt((string)m[Constant.Password]);
    //        userData.usetType = (UsetType)(int.Parse(Decrypt((string)m[Constant.UserType])));
    //        userData.Phone = Decrypt((string)m[Constant.phone]);
    //        userData.UserLog = Decrypt((string)m[Constant.UserLog]);
    //        userData.createTime = Decrypt((string)m[Constant.createTime]);
    //        gm.SQLUserData_Lst.Add(userData);
    //    }

    //    m.Close();//关闭读取器
    //}
    #endregion
    
    /// <summary>
    /// 添加数据
    /// </summary>
    public bool AddSQLData(UserData userData)
    {
        try
        {
            string cmdStr = string.Format("insert into {0} set  ", Constant.unitTable);
            MySqlCommand cmd = new MySqlCommand(cmdStr + "Account = @act,Password = @ps,UserType = @tp,phone= @ph,UserLog= @log,createTime = @cre", mySqlConnection);
            //cmd.Parameters.AddWithValue("act", Encrypt(userData.Account));
            //cmd.Parameters.AddWithValue("ps", Encrypt(userData.Password));
            //cmd.Parameters.AddWithValue("tp", Encrypt(((int)userData.usetType).ToString()));
            //cmd.Parameters.AddWithValue("ph", Encrypt(userData.Phone));
            //cmd.Parameters.AddWithValue("log", Encrypt(userData.UserLog));
            //cmd.Parameters.AddWithValue("cre", Encrypt(userData.createTime));
            cmd.ExecuteNonQuery();
            cmd.Clone();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(string .Format("添加数据是出现异常：{0}",e));
            return false;
        }
    }

    ///// <summary>
    ///// 删除数据库中的某条数据
    ///// </summary>
    ///// <returns></returns>
    //public bool RemoveSQLData(string userName)
    //{
    //    try
    //    {
    //        string iser = Encrypt(userName);


    //        string sqlStateMent = string.Format("delete from {0} where {1} = '{2}'", mySQLData.Table, "Account", Encrypt(userName));


    //        MySqlCommand mySqlCommand = new MySqlCommand(sqlStateMent, mySqlConnection);
    //        int result = mySqlCommand.ExecuteNonQuery();

    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError(e);
    //        return false;
    //    }
    //}

    ///// <summary>
    ///// 修改数据
    ///// </summary>
    ///// <param name="UserName"></param>
    ///// <returns></returns>
    //public bool UpdateSQLData(UserData userData)
    //{
    //    try
    //    {
    //        CloseMySQL();

    //        string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4}", mySQLData.DataBaseName, mySQLData.IP, mySQLData.UserName, mySQLData.Password, mySQLData.PORT);

    //        mySqlConnection = new MySqlConnection(mySqlString);

    //        mySqlConnection.Open();//打开
    //        string sql = "update userinfo set Password=@Pass,UserType=@type,phone=@ph,UserLog=@log,createTime=@time where Account=@Act";
    //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);



    //        mySqlCommand.Parameters.AddWithValue("Act", Encrypt(userData.Account));
    //        mySqlCommand.Parameters.AddWithValue("Pass", Encrypt(userData.Password));
    //        mySqlCommand.Parameters.AddWithValue("type", Encrypt(((int)userData.usetType).ToString()));
    //        mySqlCommand.Parameters.AddWithValue("ph", Encrypt(userData.Phone));
    //        mySqlCommand.Parameters.AddWithValue("log", Encrypt(userData.UserLog));
    //        mySqlCommand.Parameters.AddWithValue("time", Encrypt(userData.createTime));
    //        mySqlCommand.ExecuteNonQuery();

    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(string.Format("修改用户数据时出现异常：{0}", e));
    //        return false;
    //    }
    //}

    ///// <summary>
    ///// 修改用户数据
    ///// </summary>
    ///// <param name="UserName"></param>
    ///// <returns></returns>
    //public string ModifySQLData(UserData userData)
    //{
    //    try
    //    {
    //        CloseMySQL();
    //        string ChangeInfo = "";//所修改的具体信息
    //        string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4}", mySQLData.DataBaseName, mySQLData.IP, mySQLData.UserName, mySQLData.Password, mySQLData.PORT);

    //        mySqlConnection = new MySqlConnection(mySqlString);
    //        mySqlConnection.Open();//打开
          
    //        string sql = "update userinfo set Account = @Act, Password=@Pass,UserType=@type,phone=@ph,UserLog=@log,createTime=@time where id=@id";
    //        MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);


    //        mySqlCommand.Parameters.AddWithValue("Act", Encrypt(userData.Account));
    //        mySqlCommand.Parameters.AddWithValue("Pass", Encrypt(userData.Password));
    //        mySqlCommand.Parameters.AddWithValue("type", Encrypt(((int)userData.usetType).ToString()));
    //        mySqlCommand.Parameters.AddWithValue("ph", Encrypt(userData.Phone));
    //        mySqlCommand.Parameters.AddWithValue("log", Encrypt(userData.UserLog));
    //        mySqlCommand.Parameters.AddWithValue("time", Encrypt(userData.createTime));
    //        mySqlCommand.Parameters.AddWithValue("id", userData.id);

    //        mySqlCommand.ExecuteNonQuery();

    //        return ChangeInfo;
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(string .Format("修改用户数据时出现异常：{0}",e));
    //        return null;
    //    }
    //}

   
    #endregion

    #region MD5
    /// <summary>
    /// 加密  返回加密后的结果
    /// </summary>
    /// <param name="toE">需要加密的数据内容</param>
    /// <returns></returns>
    private static string Encrypt(string toE)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578902223367877723456789012");
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toE);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    /// <summary>
    /// 解密  返回解密后的结果
    /// </summary>
    /// <param name="toD">加密的数据内容</param>
    /// <returns></returns>
    public static string Decrypt(string toD)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578902223367877723456789012");
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        byte[] toEncryptArray = Convert.FromBase64String(toD);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    #endregion
}

