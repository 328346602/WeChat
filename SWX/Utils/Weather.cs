using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Xml;

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
        
        StringBuilder sbWeather = new StringBuilder();

        public Weather()
        {
            
        }

        public Weather(string Json)
        {
            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(Json), XmlDictionaryReaderQuotas.Max);
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(reader);
            
            //for (int i = 0; i < xdoc.ChildNodes.Count; i++)
            {
                //Response.Write(xdoc.ChildNodes[i].Name+xdoc[xdoc.ChildNodes[i].Name].Value);
                //for (int j = 0; j < xdoc.ChildNodes[i].ChildNodes.Count; j++)
                {
                    int j = 0;
                    //sbWeather.Append(string.Format("区域ID：{0}\n城市英文名:{1}\n城市中文名:{2}\n城市所在市英文名:{3}\n城市所在市中文名:{4}\n城市所在省英文名:{5}\n城市所在省中文名:{6}\n城市所在国家英文名:{7}\n城市所在国家中文名:{8}\n城市级别:{9}\n城市区号:{11}\n邮编:{11}\n经度:{12}\n纬度:{13}\n海拔:{14}\n雷达站号:{15}", xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText, xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText));

                    //sbWeather.Append(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText);

                    //for (int n = 0; n < xdoc.ChildNodes[i].ChildNodes[j].ChildNodes.Count; n++)
                    //{
                    //try
                    //{
                    //    sbWeather.Append(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[j].InnerText + "\n>>>>>\n" + xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[j + 1].InnerText);
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //    Log.WriteError(ex.Message);
                    //}
                    //}

                    int i = 0;
                    sbWeather.Append(string.Format("{0}\r\n 白天天气现象编号：{1}\r\n 晚上天气现象编号：{2}\r\n 白天天气温度(摄氏度)：{3}\r\n 晚上天气温度(摄氏度)：{4}\r\n 白天风向编号：{5}\r\n 晚上风向编号：{6}\r\n 白天风力编号：{7}\r\n 晚上风力编号：{8}\r\n 日出日落时间(中间用|分割)：{9}", "测试错误>>>>>", xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText));
                    //sbWeather.Append(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText);
                }
            }
        }


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
                Log.WriteDebug(uri);
                //uri = WeatherUtil.zhToUncode(uri);
                System.GC.Collect();
                string cityJson = HttpRequestUtil.RequestUrl(uri, "GET");
                //Log.WriteDebug(uri);
                Log.WriteDebug(cityJson);
                //cityJson = WeatherUtil.uncodeToZH(cityJson);
                return cityJson;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWeather错误>>>>>" + areaID + ">>>>>" + ex.Message);
                return "GetWeather错误>>>>>" + areaID + ">>>>>" + ex.Message;
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


        private static string GetWeatherValue(string json,string type)
        {
            try
            {
                StringBuilder sbValue = new StringBuilder();
                #region 解析指数数据
                if (type.Substring(0, 5) == "index")
                {
                    //value = string.Format("{0}:{1}\n{2}:{3}\n{4}:{5}\n{6}:{7}\n",Tools.GetJsonValue(););
                    JsonHepler.JsonSerializerBySingleData(json);
                    
                    
                }
                #endregion


                #region 解析预报数据
                else if (type.Substring(0, 7) == "forcast")
                {
                    sbValue.Append(Tools.GetJsonValue(json, "c5"));
                    sbValue.Append(" ");
                    sbValue.Append(Tools.GetJsonValue(json, "c9") + "\n");

                    sbValue.Append(Tools.GetJsonValue(json, "c5"));
                    sbValue.Append(" ");
                    sbValue.Append(Tools.GetJsonValue(json, "c9") + "\n");
                }
                #endregion
                
                return json;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWeatherValue()错误>>>>>" + ex.Message);
                throw ex;
            }
        }

        public string ToString()
        {
            return sbWeather.ToString();
        }
    }
}