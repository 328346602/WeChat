using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Security;
using System.Net;
using System.Collections;
using System.Security.Cryptography;

namespace SWX
{
    /// <summary>
    /// WeChat 的摘要说明
    /// </summary>
    public class WeChat : IHttpHandler
    {
        //接入参数
        private string[] OpenParameters = { "signature", "timestamp", "nonce", "echostr" };

        //SQL连接字串，您可以定义成一个字符串，我是从系统设置里取的
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["testmpConnectionString"].ConnectionString;


        public void ProcessRequest(HttpContext context)
        {
            Log.Write("ProcessRequest Start");
            try
            {


                bool isLanding = true;
                foreach (string s in OpenParameters)
                {
                    if (!context.Request.QueryString.AllKeys.Contains(s))
                        isLanding = false;

                }

                //toke 为 AAA时验证是否是微信的请求
                if (isLanding && checkSignature(context.Request.QueryString["signature"], context.Request.QueryString["timestamp"], context.Request.QueryString["nonce"], "AAA"))
                {
                    context.Response.ContentType = "text/plain";
                    string echoString = context.Request.QueryString["echostr"];
                    //LandMPUpdateSQLServer(context);
                    context.Response.Write(echoString);
                }
                else
                {
                    //读取发过来的信息到inputXml变量中
                    Stream sin = context.Request.InputStream;
                    byte[] readBytes;
                    readBytes = new byte[sin.Length];
                    sin.Read(readBytes, 0, readBytes.Length);
                    string inputXml = Encoding.UTF8.GetString(readBytes);

                    //使用XMLDocument加载信息结构
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(inputXml);
                    string stringMsgId = xmlDoc.SelectSingleNode("//MsgId").InnerText;

                    //把传过来的XML数据各个字段区分出来，并且填到fields这个字典变量中去
                    Dictionary<string, string> fields = new Dictionary<string, string>();
                    foreach (XmlNode x in xmlDoc.SelectSingleNode("/xml").ChildNodes)
                    {
                        fields.Add(x.Name, x.InnerText);

                    }
                    //形成返回格式的XML文档
                    string returnXml = "<xml><ToUserName><![CDATA[" +
                      fields["FromUserName"] + "]]></ToUserName><FromUserName><![CDATA[" +
                      fields["ToUserName"] + "]]></FromUserName><CreateTime>" +
                      DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds.ToString() + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" +
                      fields["Content"] + "]]></Content></xml>";

                    context.Response.ContentType = "text/xml";
                    context.Response.Write(returnXml);

                }
            }
            catch(Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private bool checkSignature(string signature, string timestamp, string nonce, string token)
        {
            try
            {
                ArrayList tmpArray = new ArrayList();
                tmpArray.Add(token);
                tmpArray.Add(timestamp);
                tmpArray.Add(nonce);
                tmpArray.Sort();
                string tmpStr = (string)tmpArray[0] + (string)tmpArray[1] + (string)tmpArray[2];

                //建立SHA1对象
                SHA1 sha = new SHA1CryptoServiceProvider();

                //将mystr转换成byte[]
                ASCIIEncoding enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(tmpStr);

                //Hash运算
                byte[] dataHashed = sha.ComputeHash(dataToHash);

                //将运算结果转换成string
                string hash = BitConverter.ToString(dataHashed).Replace("-", "");
                Log.Write("hash:" + hash); //记录日志，不需要可以注释掉

                if (hash.ToLower() == signature.ToLower())
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                Log.Write(ex.Message);
                return false;
            }

        }
    }
}