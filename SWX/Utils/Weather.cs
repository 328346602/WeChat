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

                return new WeatherInfo( GetWeather(MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString(), "forecast_f"));
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
        public WeatherInfo()
        {

        }
        public WeatherInfo(string Json)
        {
            XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(Json), XmlDictionaryReaderQuotas.Max);
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(reader);

            //for (int i = 0; i < xdoc.ChildNodes.Count; i++)
            {
                try
                //Response.Write(xdoc.ChildNodes[i].Name+xdoc[xdoc.ChildNodes[i].Name].Value);
                //for (int j = 0; j < xdoc.ChildNodes[i].ChildNodes.Count; j++)
                {
                    int j = 0;

                    AreaID = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    ENName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    CNName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    ENCityName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    CNCityName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    ENProvName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    CNProvName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    ENCountryName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    CNCountryName = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    CityLevel = uint.Parse(xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText);
                    CityAreaCode = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
                    PostCode = uint.Parse(xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText);
                    Longitude = float.Parse(xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText);
                    Latitude = float.Parse(xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText);
                    Altitude = int.Parse(xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText);
                    RadarStationNo = xdoc.ChildNodes[0].ChildNodes[0].ChildNodes[j++].InnerText;
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
                    DayWeatherCodeOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    NightWeatherCodeOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    DayTemperatureOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    NightTemperatureOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    DayWindDirectionOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    NightWindDirectionOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    DayWindForceOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    NightWindForceOne = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText;
                    SunriseTimeOne = (xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i].InnerText).Split('|')[0];
                    SunsetTimeOne = (xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i].InnerText).Split('|')[1];
                    i = 0;
                    DayWeatherCodeTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    NightWeatherCodeTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    DayTemperatureTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    NightTemperatureTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    DayWindDirectionTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    NightWindDirectionTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    DayWindForceTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    NightWindForceTwo = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i++].InnerText;
                    SunriseTimeTwo = (xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i].InnerText).Split('|')[0];
                    SunsetTimeTwo = (xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[i].InnerText).Split('|')[1];
                    i = 0;
                    DayWeatherCodeThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    NightWeatherCodeThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    DayTemperatureThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    NightTemperatureThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    DayWindDirectionThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    NightWindDirectionThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    DayWindForceThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    NightWindForceThree = xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i++].InnerText;
                    SunriseTimeThree = (xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i].InnerText).Split('|')[0];
                    SunsetTimeThree = (xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[2].ChildNodes[i].InnerText).Split('|')[1];
                    Log.WriteDebug("SunsetTimeThree>>>>>" + SunsetTimeThree);
                    //sbWeather.Append(string.Format("{0}\r\n 白天天气现象编号：{1}\r\n 晚上天气现象编号：{2}\r\n 白天天气温度(摄氏度)：{3}\r\n 晚上天气温度(摄氏度)：{4}\r\n 白天风向编号：{5}\r\n 晚上风向编号：{6}\r\n 白天风力编号：{7}\r\n 晚上风力编号：{8}\r\n 日出日落时间(中间用|分割)：{9}", "测试错误>>>>>", xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText, xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText));
                    //sbWeather.Append(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[i++].InnerText);
                    IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
                    Log.WriteDebug(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText);
                    PublishTime = System.DateTime.ParseExact(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText, "yyyyMMddHHmm", culture);
                    //DateTime.Parse(xdoc.ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText);
                }
                catch (Exception ex)
                {
                    Log.WriteError("解析Json错误>>>>>" + ex.Message);
                }
            }
        }
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

        public static string GetWeather(string areaID = "101010100", string type = "index_f")
        {
            try
            {
                Log.WriteDebug("areaID=" + areaID+",type="+type);
                string url = "http://open.weather.com.cn/data/";//接口地址
                string appID = "899bd43708e1a133";//完整的appID
                string privateKey = "bf24c2_SmartWeatherAPI_6d393ab";//私有Key

                //string areaID = "101010100";//位置ID
                //string type = "index_f";//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
                string date = DateTime.Now.ToString("yyyyMMddHHHmm");
                string appID_Six = appID.Substring(0, 6);

                string publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);//public_key为不包含key在内的完整URL其它部分（此处appid为完整appid）


                //publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);
                string uri = string.Format("http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}&key={4}", areaID, type, date, appID_Six, GetKey(publicKey,privateKey));
                
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
                string SQL = "select AreaID from AreaID_F where AreaID=" + int.Parse(areaCode) + " or NameEN='" + areaCode + "' or NameCN='" + areaCode + "'";
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