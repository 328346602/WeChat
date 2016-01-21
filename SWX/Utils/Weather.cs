using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Xml;

using SWX.DBUtil;
using System.Data;
using System.IO;

namespace SWX.Utils
{
    public class Weather
    {
        private static string url = System.Configuration.ConfigurationManager.AppSettings["Url"].ToString();//接口地址
        private static string appID = System.Configuration.ConfigurationManager.AppSettings["AppID"].ToString();//完整的appID
        private static string privateKey = System.Configuration.ConfigurationManager.AppSettings["PrivateKey"].ToString();//私有Key
        public static string areaID = System.Configuration.ConfigurationManager.AppSettings["AreaID"].ToString();//位置ID
        public static string type = System.Configuration.ConfigurationManager.AppSettings["Type"].ToString();//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
        private static string date = DateTime.Now.ToString("yyyyMMddHHHmm");
        private static string appID_Six = appID.Substring(0, 6);

        private static string publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);//public_key为不包含key在内的完整URL其它部分（此处appid为完整appid）
        
        StringBuilder sbWeather = new StringBuilder();


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public static string GetWeatherF(string areaCode)
        {
            try
            {
                string SQL = "select AreaID from AreaID_F where AreaID=" + areaCode + " or NameEN=" + areaCode + " or NameCN=" + areaCode;

                return GetWeather(MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString(), "forecast_f");
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public static WeatherInfo GetWeatherV(string areaCode)
        {
            try
            {
                string SQL = "select AreaID from AreaID_V where AreaID=" + areaCode + " or NameEN=" + areaCode + " or NameCN=" + areaCode;

                return new WeatherInfo( GetWeather(MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString(), "forecast_f"),"forecast");
            }
            catch (Exception ex)
            {
                return new WeatherInfo();
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

        public static string GetWeatherCNByCode(string code)
        {
            string SQL = "select CN from WeatherCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWeatherENByCode(string code)
        {
            string SQL = "select EN from WeatherCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindDirectionCNByCode(string code)
        {
            string SQL = "select CN from WindDirectionCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindDirectionENByCode(string code)
        {
            string SQL = "select EN from WindDirectionCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindForceCNByCode(string code)
        {
            string SQL = "select CN from WindForceCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindForceENByCode(string code)
        {
            string SQL = "select EN from WindForceCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }
    }

    public class WeatherInfo
    {
        #region 基础成员变量
        private static string url = System.Configuration.ConfigurationManager.AppSettings["Url"].ToString();//接口地址
        private static string appID = System.Configuration.ConfigurationManager.AppSettings["AppID"].ToString();//完整的appID
        private static string privateKey = System.Configuration.ConfigurationManager.AppSettings["PrivateKey"].ToString();//私有Key
        private static string areaID = System.Configuration.ConfigurationManager.AppSettings["AreaID"].ToString();//位置ID
        private static string type = System.Configuration.ConfigurationManager.AppSettings["Type"].ToString();//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
        private static string date = DateTime.Now.ToString("yyyyMMddHHHmm");
        private static string appID_Six = appID.Substring(0, 6);
        private static string publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);//public_key为不包含key在内的完整URL其它部分（此处appid为完整appid）
        private static string uri = string.Format("http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}&key={4}", areaID, type, date, appID_Six, GetKey(publicKey, privateKey));
        #endregion

        #region c1-c16，f0，地区信息
        /// <summary>
        /// c1
        /// 区域ID
        /// </summary>
        public string AreaID { get; set; }

        /// <summary>
        /// c2
        /// 城市英文名
        /// </summary>
        public string ENName { get; set; }

        /// <summary>
        /// c3
        /// 城市中文名
        /// </summary>
        public string CNName { get; set; }

        /// <summary>
        /// c4
        /// 城市所在市英文名
        /// </summary>
        public string ENCityName { get; set; }

        /// <summary>
        /// c5
        /// 城市所在市中文名
        /// </summary>
        public string CNCityName { get; set; }

        /// <summary>
        /// c6
        /// 城市所在省英文名
        /// </summary>
        public string ENProvName { get; set; }

        /// <summary>
        /// c7
        /// 城市所在省中文名
        /// </summary>
        public string CNProvName { get; set; }

        /// <summary>
        /// c8
        /// 城市所在国家英文名
        /// </summary>
        public string ENCountryName { get; set; }

        /// <summary>
        /// c9
        /// 城市所在国家中文名
        /// </summary>
        public string CNCountryName { get; set; }

        /// <summary>
        /// c10
        /// 城市级别
        /// </summary>
        public uint CityLevel { get; set; }

        /// <summary>
        /// c11
        /// 城市区号
        /// </summary>
        public string CityAreaCode { get; set; }

        /// <summary>
        /// c12
        /// 邮编
        /// </summary>
        public uint PostCode { get; set; }

        /// <summary>
        /// c13
        /// 经度
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// c14
        /// 纬度
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// c15
        /// 海拔
        /// </summary>
        public int Altitude { get; set; }

        /// <summary>
        /// c16
        /// 雷达站号
        /// </summary>
        public string RadarStationNo { get; set; }

        /// <summary>
        /// f0
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        #endregion

        #region fa-fi，预报信息

        #region 白天天气现象编号
        /// <summary>
        /// fa
        /// 今天白天天气现象编号
        /// </summary>
        public string DayWeatherCodeOne { get; set; }

        /// <summary>
        /// fa
        /// 明天白天天气现象编号
        /// </summary>
        public string DayWeatherCodeTwo { get; set; }

        /// <summary>
        /// fa
        /// 后天白天天气现象编号
        /// </summary>
        public string DayWeatherCodeThree { get; set; }
        #endregion

        #region 晚上天气现象编号
        /// <summary>
        /// fb
        /// 今天晚上天气现象编号
        /// </summary>
        public string NightWeatherCodeOne { get; set; }

        /// <summary>
        /// fb
        /// 明天晚上天气现象编号
        /// </summary>
        public string NightWeatherCodeTwo { get; set; }

        /// <summary>
        /// fb
        /// 后天晚上天气现象编号
        /// </summary>
        public string NightWeatherCodeThree { get; set; }
        #endregion

        #region 白天天气温度
        /// <summary>
        /// fc
        /// 今天白天天气温度(摄氏度)
        /// </summary>
        public string DayTemperatureOne { get; set; }

        /// <summary>
        /// fc
        /// 明天白天天气温度(摄氏度)
        /// </summary>
        public string DayTemperatureTwo { get; set; }

        /// <summary>
        /// fc
        /// 后天白天天气温度(摄氏度)
        /// </summary>
        public string DayTemperatureThree { get; set; }
        #endregion

        #region 晚上天气温度
        /// <summary>
        /// fd
        /// 今天晚上天气温度(摄氏度)
        /// </summary>
        public string NightTemperatureOne { get; set; }

        /// <summary>
        /// fd
        /// 明天晚上天气温度(摄氏度)
        /// </summary>
        public string NightTemperatureTwo { get; set; }

        /// <summary>
        /// fd
        /// 后天晚上天气温度(摄氏度)
        /// </summary>
        public string NightTemperatureThree { get; set; }
        #endregion


        #region 白天风向编号
        /// <summary>
        /// fe
        /// 今天白天风向编号
        /// </summary>
        public string DayWindDirectionOne { get; set; }

        /// <summary>
        /// fe
        /// 明天白天风向编号
        /// </summary>
        public string DayWindDirectionTwo { get; set; }

        /// <summary>
        /// fe
        /// 后天白天风向编号
        /// </summary>
        public string DayWindDirectionThree { get; set; }
        #endregion

        #region 晚上风向编号
        /// <summary>
        /// ff
        /// 今天晚上风向编号
        /// </summary>
        public string NightWindDirectionOne { get; set; }

        /// <summary>
        /// ff
        /// 明天晚上风向编号
        /// </summary>
        public string NightWindDirectionTwo { get; set; }

        /// <summary>
        /// ff
        /// 后天晚上风向编号
        /// </summary>
        public string NightWindDirectionThree { get; set; }
        #endregion

        #region 白天风力编号
        /// <summary>
        /// fg
        /// 今天白天风力编号
        /// </summary>
        public string DayWindForceOne { get; set; }

        /// <summary>
        /// fg
        /// 明天白天风力编号
        /// </summary>
        public string DayWindForceTwo { get; set; }

        /// <summary>
        /// fg
        /// 后天白天风力编号
        /// </summary>
        public string DayWindForceThree { get; set; }
        #endregion

        #region 晚上风力编号
        /// <summary>
        /// fh
        /// 今天晚上风力编号
        /// </summary>
        public string NightWindForceOne { get; set; }

        /// <summary>
        /// fh
        /// 明天晚上风力编号
        /// </summary>
        public string NightWindForceTwo { get; set; }

        /// <summary>
        /// fh
        /// 后天晚上风力编号
        /// </summary>
        public string NightWindForceThree { get; set; }
        #endregion

        #region 日出时间
        /// <summary>
        /// fi
        /// 今天日出时间
        /// </summary>
        public string SunriseTimeOne { get; set; }

        /// <summary>
        /// fi
        /// 明天日出时间
        /// </summary>
        public string SunriseTimeTwo { get; set; }

        /// <summary>
        /// fi
        /// 后天日出时间
        /// </summary>
        public string SunriseTimeThree { get; set; }
        #endregion

        #region 日落时间
        /// <summary>
        /// fi
        /// 今天日落时间
        /// </summary>
        public string SunsetTimeOne { get; set; }

        /// <summary>
        /// fi
        /// 明天日落时间
        /// </summary>
        public string SunsetTimeTwo { get; set; }

        /// <summary>
        /// fi
        /// 后天日落时间
        /// </summary>
        public string SunsetTimeThree { get; set; }
        #endregion

        #endregion

        private static string SetUri(string areaID,string type)
        {
            try
            {
                privateKey = System.Configuration.ConfigurationManager.AppSettings["PrivateKey"].ToString();//私有Key
                type = System.Configuration.ConfigurationManager.AppSettings["Type"].ToString();//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
                date = DateTime.Now.ToString("yyyyMMddHHHmm");
                publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);//public_key为不包含key在内的完整URL其它部分（此处appid为完整appid）
                uri = string.Format("http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}&key={4}", areaID, type, date, appID_Six, GetKey(publicKey, privateKey));
                return uri;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public WeatherInfo()
        {

        }
        public WeatherInfo(string json,string type)
        {
            if (!json.Equals(string.Empty))
            {
                XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(reader);
                Log.WriteDebug(xdoc.InnerText);
                if (type.ToLower() == "forecast")
                {

                    #region 输出完整的XML，分析格式
                    //StringWriter stringWriter = new StringWriter();
                    //XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                    //xmlTextWriter.Formatting = Formatting.Indented;
                    //foreach (XmlNode xmlNode in xdoc)
                    //{
                    //    xmlNode.WriteTo(xmlTextWriter);
                    //}

                    //Log.WriteLog(stringWriter.ToString());
                    #endregion

                    #region 用xml解析json,并为相关的成员变量赋值
                    try
                    {
                        ///获取到的属性信息
                        XmlNode info = xdoc.FirstChild.SelectSingleNode("c");

                        AreaID = info.SelectSingleNode("c1").InnerText;
                        ENName = info.SelectSingleNode("c2").InnerText;
                        CNName = info.SelectSingleNode("c3").InnerText;
                        ENCityName = info.SelectSingleNode("c4").InnerText;
                        CNCityName = info.SelectSingleNode("c5").InnerText;
                        ENProvName = info.SelectSingleNode("c6").InnerText;
                        CNProvName = info.SelectSingleNode("c7").InnerText;
                        ENCountryName = info.SelectSingleNode("c8").InnerText;
                        CNCountryName = info.SelectSingleNode("c9").InnerText;
                        CityLevel = uint.Parse(info.SelectSingleNode("c10").InnerText);
                        CityAreaCode = info.SelectSingleNode("c11").InnerText;
                        PostCode = uint.Parse(info.SelectSingleNode("c12").InnerText);
                        Longitude = float.Parse(info.SelectSingleNode("c13").InnerText);
                        Latitude = float.Parse(info.SelectSingleNode("c14").InnerText);
                        Altitude = int.Parse(info.SelectSingleNode("c15").InnerText);
                        RadarStationNo = info.SelectSingleNode("c16").InnerText;

                        ///f0 数据发布时间
                        IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
                        //Log.WriteDebug(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText);
                        PublishTime = System.DateTime.ParseExact(xdoc.FirstChild.SelectSingleNode("f").SelectSingleNode("f0").FirstChild.InnerText, "yyyyMMddHHmm", culture);

                        XmlNode DayOne = xdoc.FirstChild.SelectSingleNode("f").SelectSingleNode("f1").FirstChild;
                        Log.WriteLog(DayOne.InnerText);
                        DayWeatherCodeOne = DayOne.SelectSingleNode("fa").InnerText;
                        NightWeatherCodeOne = DayOne.SelectSingleNode("fb").InnerText;
                        DayTemperatureOne = DayOne.SelectSingleNode("fc").InnerText;
                        NightTemperatureOne = DayOne.SelectSingleNode("fd").InnerText;
                        DayWindDirectionOne = DayOne.SelectSingleNode("fe").InnerText;
                        NightWindDirectionOne = DayOne.SelectSingleNode("ff").InnerText;
                        DayWindForceOne = DayOne.SelectSingleNode("fg").InnerText;
                        NightWindForceOne = DayOne.SelectSingleNode("fh").InnerText;
                        SunriseTimeOne = DayOne.SelectSingleNode("fi").InnerText.Split('|')[0];
                        SunsetTimeOne = DayOne.SelectSingleNode("fi").InnerText.Split('|')[1];

                        ///第二天
                        ///
                        XmlNode DayTwo = DayOne.NextSibling;
                        Log.WriteLog("DayTwo>>>>>" + DayTwo.InnerText);
                        DayWeatherCodeTwo = DayTwo.SelectSingleNode("fa").InnerText;
                        NightWeatherCodeTwo = DayTwo.SelectSingleNode("fb").InnerText;
                        DayTemperatureTwo = DayTwo.SelectSingleNode("fc").InnerText;
                        NightTemperatureTwo = DayTwo.SelectSingleNode("fd").InnerText;
                        DayWindDirectionTwo = DayTwo.SelectSingleNode("fe").InnerText;
                        NightWindDirectionTwo = DayTwo.SelectSingleNode("ff").InnerText;
                        DayWindForceTwo = DayTwo.SelectSingleNode("fg").InnerText;
                        NightWindForceTwo = DayTwo.SelectSingleNode("fh").InnerText;
                        SunriseTimeTwo = DayTwo.SelectSingleNode("fi").InnerText.Split('|')[0];
                        SunsetTimeTwo = DayTwo.SelectSingleNode("fi").InnerText.Split('|')[1];
                        ///第三天


                        XmlNode DayThree = DayTwo.NextSibling;
                        Log.WriteLog("DayThree>>>>>" + DayThree.InnerText);
                        DayWeatherCodeThree = DayThree.SelectSingleNode("fa").InnerText;
                        NightWeatherCodeThree = DayThree.SelectSingleNode("fb").InnerText;
                        DayTemperatureThree = DayThree.SelectSingleNode("fc").InnerText;
                        NightTemperatureThree = DayThree.SelectSingleNode("fd").InnerText;
                        DayWindDirectionThree = DayThree.SelectSingleNode("fe").InnerText;
                        NightWindDirectionThree = DayThree.SelectSingleNode("ff").InnerText;
                        DayWindForceThree = DayThree.SelectSingleNode("fg").InnerText;
                        NightWindForceThree = DayThree.SelectSingleNode("fh").InnerText;
                        SunriseTimeThree = DayThree.SelectSingleNode("fi").InnerText.Split('|')[0];
                        SunsetTimeThree = DayThree.SelectSingleNode("fi").InnerText.Split('|')[1];

                    }
                    catch (Exception ex)
                    {
                        Log.WriteError("解析Json错误>>>>>" + ex.Message);
                    }
                    #endregion
                }
                else if (type.ToLower() == "index")
                {

                }
            }
        }
        

        public static string GetWeather(string areaID = "101010100", string type = "index_f")
        {
            try
            {
                uri = SetUri(areaID, type);
                string cityJson = HttpRequestUtil.RequestUrl(uri, "GET");
                Log.WriteDebug(uri);
                Log.WriteDebug(cityJson);
                //cityJson = WeatherUtil.uncodeToZH(cityJson);
                System.GC.Collect();
                return cityJson;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWeather错误>>>>>" + areaID + ">>>>>" + ex.Message);
                return "GetWeather错误>>>>>" + areaID + ">>>>>" + ex.Message;
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        private static string GetKey(string publicKey, string privateKey)
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

        private static string GetWeatherValue(string json, string type)
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

        public static string GetWeatherCNByCode(string code)
        {
            string SQL = "select CN from WeatherCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWeatherENByCode(string code)
        {
            string SQL = "select EN from WeatherCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindDirectionCNByCode(string code)
        {
            string SQL = "select CN from WindDirectionCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindDirectionENByCode(string code)
        {
            string SQL = "select EN from WindDirectionCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindForceCNByCode(string code)
        {
            string SQL = "select CN from WindForceCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWindForceENByCode(string code)
        {
            string SQL = "select EN from WindForceCode where code=" + code;
            try
            {

                return MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return code;
            }
        }

        public static string GetWeatherF(string areaCode)
        {
            try
            {
                string SQL;
                try
                {
                     SQL = "select AreaID from AreaID_F where AreaID=" + int.Parse(areaCode) + " or NameEN='" + areaCode + "' or NameCN='" + areaCode + "'";
                }
                catch (Exception ex)
                {
                     SQL = "select AreaID from AreaID_F where NameEN='" + areaCode.ToLower() + "' or NameCN=N'" + areaCode + "'";
                }
                
                Log.WriteDebug("SQL>>>>>"+SQL);
                return GetWeather(MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString(), "forecast_f");
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}