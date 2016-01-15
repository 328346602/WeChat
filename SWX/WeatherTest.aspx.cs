using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;

namespace SWX
{
    public partial class WeatherTest : System.Web.UI.Page
    {
        private static string url = "http://open.weather.com.cn/data/";
        private static string appID = "899bd43708e1a133";
        private static string privateKey = "bf24c2_SmartWeatherAPI_6d393ab";

        private static string areaID = "101010100";
        private static string type = "forecast_v";//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
        private static string date = DateTime.Now.ToString("yyyyMMddHHHmm");
        private static string appID_Six = appID.Substring(0,6);

        private static string publicKey = string.Format("http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}",areaID,type,date,appID);//public_key为不包含key在内的完整URL其它部分（此处appid为完整appid）


      string GetKey()
        {
            //使用SHA1的HMAC

            HMAC hmac = HMACSHA1.Create();
            var data = System.Text.Encoding.UTF8.GetBytes(publicKey);
            //密钥
            var key = System.Text.Encoding.UTF8.GetBytes(privateKey);
            hmac.Key = key;

            //对数据进行签名
            var signedData = hmac.ComputeHash(data);
            return Convert.ToBase64String(signedData);

        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}