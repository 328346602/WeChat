//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace SWX
//{
//    public partial class Index : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {

//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Web.Security;
using System.Security.Cryptography;

namespace SWX
{
    public partial class Index : System.Web.UI.Page
    {
        const string Token = "TestAppLDM";		//你的token
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string echoStr = Request["echoStr"].ToString();
            string signature = Request["signature"].ToString();
            string timestamp = Request["timestamp"].ToString();
            string nonce = Request["nonce"].ToString();
            string temp = echoStr + signature + timestamp + nonce;
            //temp = new SHA1sha1(temp);
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();

            //将mystr转换成byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(temp);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            if (hash == signature)
            {
                Response.Write("echoStr:" + echoStr + "\r\n" + "hash:" + hash);
                //return echoStr;
            }
            Response.Write(hash);
            //return echoStr;
        }

        private void WriteLog(string text)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory;
            Response.Write(path);
            path = System.IO.Path.Combine(path
              , "Logs\\" + DateTime.Now.ToString("yy-MM-dd"));

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            //string fileFullName = System.IO.Path.Combine(path
            //  , string.Format("{0}.log", DateTime.Now.ToString("yyMMdd-HHmmss")));
            string fileFullName = "Log123.txt";

            using (StreamWriter output = System.IO.File.AppendText(fileFullName))
            {
                output.WriteLine(text);

                output.Close();
            }
        }
    }
}
