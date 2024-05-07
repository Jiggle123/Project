using MySql.Data.MySqlClient;
using PEProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




public class ConnHelper
{
    /// <summary>
    /// 连接数据库用的字符串
    /// </summary>
    public const string CONNECTIONSTRING = "datasource=127.0.0.1;port=3306;database=userdata;user=root;pwd=ljj020629;";

    private  MySqlConnection conn;

    /// <summary>
    /// 连接数据库
    /// </summary>
    /// <returns></returns>
    public ConnHelper()
    {
         conn = new MySqlConnection(CONNECTIONSTRING);
        try
        {
            conn.Open();
          
        }
        catch (Exception e)
        {
            Console.WriteLine("链接数据库的时候实现异常：" + e);
        }
    }

    public void CloseConnection()
    {
        if (conn != null)
            conn.Close();
        else
        {
            Console.WriteLine("MySqlConnection不能为空");
        }
    }


    public UserData GetResultByUserid( string  emailID)
    {
        MySqlDataReader reader = null;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from user where email = @email", conn);
            cmd.Parameters.AddWithValue("email", emailID);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string userName = reader.GetString("userName");
                string data = reader.GetString("data");
                string gneder = reader.GetString("gneder");
                string height = reader.GetString("height");
                string weigh = reader.GetString("weigh");
                string email = reader.GetString("email");
                string password = reader.GetString("password");
                int id = reader.GetInt32("id");

                UserData res = new UserData(userName, data, gneder, height, weigh, email, password);

                res.id = id;

                return res;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("在GetResultByUserid的时候出现异常：" + e);
        }
        finally
        {
            if (reader != null) reader.Close();
        }
        return null;
    }

    public bool  InsertData( UserData res)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand("insert into user set userName=@userName,data=@data,gneder=@gneder,height=@height,weigh=@weigh,email=@email,password=@password", conn);

            cmd.Parameters.AddWithValue("userName", res.userName);
            cmd.Parameters.AddWithValue("data", res.data);
            cmd.Parameters.AddWithValue("gneder", res.gneder);
            cmd.Parameters.AddWithValue("height", res.height);
            cmd.Parameters.AddWithValue("weigh", res.weigh);
            cmd.Parameters.AddWithValue("email", res.email);
            cmd.Parameters.AddWithValue("password", res.password);

            cmd.ExecuteNonQuery();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("在插入数据的时候出现异常：" + e);
            return false;
        }
    }

    public bool  UpdateOrAddResult( UserData res)
    {
        try
        {
            MySqlCommand cmd = null;

            cmd = new MySqlCommand("update user set userName=@userName,data=@data,gneder=@gneder,height=@height,weigh=@weigh,password=@password, email=@email where id=@id ", conn);
       //     string sqlCmd = string.Format("UPDATE `userdata`.`user` SET `userName`='{0}', `data`='{1}', `gneder`='{2}', `height`='{3}', `weigh`='{4}', `email`='{5}', `password`='{6}' WHERE `id`='{7}';", res.userName, res.data, res.gneder, res.height, res.weigh, res.email,res.password, res.id);
         //   cmd = new MySqlCommand(sqlCmd);

            cmd.Parameters.AddWithValue("userName", res.userName);
            cmd.Parameters.AddWithValue("data", res.data);
            cmd.Parameters.AddWithValue("gneder", res.gneder);
            cmd.Parameters.AddWithValue("height", res.height);
            cmd.Parameters.AddWithValue("weigh", res.weigh);
            cmd.Parameters.AddWithValue("password", res.password);
            cmd.Parameters.AddWithValue("email", res.email);
            cmd.Parameters.AddWithValue("id", res.id);

            cmd.ExecuteNonQuery();

            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine("在Update的时候出现异常：" + e);
            return false;
        }
    }
}

