using PENet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class TXTDBSys
{
    /// <summary>
    ///读取DB
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public Dictionary<string, string> ReadDB(out string IP, out int PORT)
    {
        string filePath = Constant.path_url;

        List<string> allDBtxt = new List<string>();

        try
        {
            if (File.Exists(filePath))
                allDBtxt = new List<string>(File.ReadAllLines(filePath));
            else
                PETool.LogMsg("DB不存在", LogLevel.Error);
        }
        catch (Exception ex)
        {
            PETool.LogMsg(ex.Message, LogLevel.Error);
        }
        string[] dbNetDT = allDBtxt[0].Split('|');

        IP = dbNetDT[0];
        PORT = int.Parse(dbNetDT[1]);
        allDBtxt.RemoveAt(0);

        Dictionary<string, string> _tempDic = new Dictionary<string, string>();

        foreach (var item in allDBtxt)
        {
            string[] itemStr = item.Split('|');

            _tempDic.Add(itemStr[0], itemStr[1]);
        }

        return _tempDic;
    }

    /// <summary>
    /// 写入DB数据
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="writeDBStr"></param>
    public void WriteDB(string writeDBStr)
    {
        string filePath = Constant.path_url;

        List<string> allDBtxt = new List<string>();

        try
        {

            if (File.Exists(filePath))
            {
                allDBtxt = new List<string>(File.ReadAllLines(filePath));

                string md5DT = Encrypt(writeDBStr);

                allDBtxt.Add(md5DT);

                File.WriteAllLines(filePath, allDBtxt);
            }

        }
        catch (Exception e)
        {
            PETool.LogMsg("写入DB异常", LogLevel.Error);
        }
    }

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

