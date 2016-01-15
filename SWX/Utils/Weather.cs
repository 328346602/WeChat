using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace SWX.Utils
{
    public class Weather
    {
        private static string url = "http://open.weather.com.cn/data/";//接口地址
        private static string appID = "899bd43708e1a133";//完整的appID
        private static string privateKey = "bf24c2_SmartWeatherAPI_6d393ab";//私有Key

        public static string areaID = "101010100";//位置ID
        public static string type = "index_f";//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
        private static string date = DateTime.Now.ToString("yyyyMMddHHHmm");
        private static string appID_Six = appID.Substring(0, 6);

        private static string publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);//public_key为不包含key在内的完整URL其它部分（此处appid为完整appid）


        public static string GetWeather()
        {

            string uri = string.Format("http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}&key={4}", areaID, type, date, appID_Six, GetKey());
            string cityJson = HttpRequestUtil.RequestUrl(uri, "GET");
            return cityJson;
        }

        public static string GetWeather(string areaID, string type)
        {
            try
            {
                Weather.areaID = areaID;
                Weather.type = type;
                Weather.publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);
                string uri = string.Format("http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}&key={4}", areaID, type, date, appID_Six, GetKey());
                //Log.WriteDebug(uri);
                //uri = WeatherUtil.zhToUncode(uri);
                string cityJson = HttpRequestUtil.RequestUrl(uri, "GET");
                Log.WriteDebug(uri);
                Log.WriteDebug(cityJson);
                //cityJson = WeatherUtil.uncodeToZH(cityJson);
                return cityJson;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWeather错误：" + ex.Message);
                throw ex;
            }

        }

        private static string GetKey()
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

    }
}