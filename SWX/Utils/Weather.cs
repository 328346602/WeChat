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
        #region 基础成员变量
        private static string url = System.Configuration.ConfigurationManager.AppSettings["Url"].ToString();//接口地址
        private static string appID = System.Configuration.ConfigurationManager.AppSettings["AppID"].ToString();//完整的appID
        private static string privateKey = System.Configuration.ConfigurationManager.AppSettings["PrivateKey"].ToString();//私有Key
        private static string areaID = System.Configuration.ConfigurationManager.AppSettings["AreaID"].ToString();//位置ID
        private static string forecastType = System.Configuration.ConfigurationManager.AppSettings["ForecastType"].ToString();//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
        private static string indexType = System.Configuration.ConfigurationManager.AppSettings["IndexType"].ToString();//指数:index_f(基础接口)；index_v(常规接口)； 3天预报:forecast_f(基础接口)；forecast_v(常规接口)；
        private static string date = DateTime.Now.ToString("yyyyMMddHHHmm");
        private static string appID_Six = appID.Substring(0, 6);
        public static string uri = string.Empty;
        #endregion

        /// <summary>
        /// 获取Key参数的方法
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

        public static WeatherForecast GetForecast(string area)
        {
            WeatherForecast f = new WeatherForecast();
            try
            {
                string json = GetJson(area, "forecast");
                if (!json.Equals(string.Empty) && !json.Contains("error data"))
                {
                    XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(reader);
                    Log.WriteDebug("GetForecast>xdoc>>>>>" + xdoc.InnerText);

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
                    ///获取到的属性信息
                    XmlNode info = xdoc.FirstChild.SelectSingleNode("c");
                    f.AreaID = info.SelectSingleNode("c1").InnerText;
                    f.ENName = info.SelectSingleNode("c2").InnerText;
                    f.CNName = info.SelectSingleNode("c3").InnerText;
                    f.ENCityName = info.SelectSingleNode("c4").InnerText;
                    f.CNCityName = info.SelectSingleNode("c5").InnerText;
                    f.ENProvName = info.SelectSingleNode("c6").InnerText;
                    f.CNProvName = info.SelectSingleNode("c7").InnerText;
                    f.ENCountryName = info.SelectSingleNode("c8").InnerText;
                    f.CNCountryName = info.SelectSingleNode("c9").InnerText;
                    f.CityLevel = uint.Parse(info.SelectSingleNode("c10").InnerText);
                    f.CityAreaCode = info.SelectSingleNode("c11").InnerText;
                    f.PostCode = uint.Parse(info.SelectSingleNode("c12").InnerText);
                    f.Longitude = float.Parse(info.SelectSingleNode("c13").InnerText);
                    f.Latitude = float.Parse(info.SelectSingleNode("c14").InnerText);
                    f.Altitude = int.Parse(info.SelectSingleNode("c15").InnerText);
                    f.RadarStationNo = info.SelectSingleNode("c16").InnerText;

                    ///f0 数据发布时间
                    IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);
                    f.PublishTime = System.DateTime.ParseExact(xdoc.FirstChild.SelectSingleNode("f").SelectSingleNode("f0").FirstChild.InnerText, "yyyyMMddHHmm", culture);
                    
                    ///第一天
                    XmlNode DayOne = xdoc.FirstChild.SelectSingleNode("f").SelectSingleNode("f1").FirstChild;
                    f.DayWeatherCodeOne = DayOne.SelectSingleNode("fa").InnerText;
                    f.NightWeatherCodeOne = DayOne.SelectSingleNode("fb").InnerText;
                    f.DayTemperatureOne = DayOne.SelectSingleNode("fc").InnerText;
                    f.NightTemperatureOne = DayOne.SelectSingleNode("fd").InnerText;
                    f.DayWindDirectionOne = DayOne.SelectSingleNode("fe").InnerText;
                    f.NightWindDirectionOne = DayOne.SelectSingleNode("ff").InnerText;
                    f.DayWindForceOne = DayOne.SelectSingleNode("fg").InnerText;
                    f.NightWindForceOne = DayOne.SelectSingleNode("fh").InnerText;
                    f.SunriseTimeOne = DayOne.SelectSingleNode("fi").InnerText.Split('|')[0];
                    f.SunsetTimeOne = DayOne.SelectSingleNode("fi").InnerText.Split('|')[1];

                    ///第二天
                    XmlNode DayTwo = DayOne.NextSibling;
                    f.DayWeatherCodeTwo = DayTwo.SelectSingleNode("fa").InnerText;
                    f.NightWeatherCodeTwo = DayTwo.SelectSingleNode("fb").InnerText;
                    f.DayTemperatureTwo = DayTwo.SelectSingleNode("fc").InnerText;
                    f.NightTemperatureTwo = DayTwo.SelectSingleNode("fd").InnerText;
                    f.DayWindDirectionTwo = DayTwo.SelectSingleNode("fe").InnerText;
                    f.NightWindDirectionTwo = DayTwo.SelectSingleNode("ff").InnerText;
                    f.DayWindForceTwo = DayTwo.SelectSingleNode("fg").InnerText;
                    f.NightWindForceTwo = DayTwo.SelectSingleNode("fh").InnerText;
                    f.SunriseTimeTwo = DayTwo.SelectSingleNode("fi").InnerText.Split('|')[0];
                    f.SunsetTimeTwo = DayTwo.SelectSingleNode("fi").InnerText.Split('|')[1];
                    
                    ///第三天
                    XmlNode DayThree = DayTwo.NextSibling;
                    f.DayWeatherCodeThree = DayThree.SelectSingleNode("fa").InnerText;
                    f.NightWeatherCodeThree = DayThree.SelectSingleNode("fb").InnerText;
                    f.DayTemperatureThree = DayThree.SelectSingleNode("fc").InnerText;
                    f.NightTemperatureThree = DayThree.SelectSingleNode("fd").InnerText;
                    f.DayWindDirectionThree = DayThree.SelectSingleNode("fe").InnerText;
                    f.NightWindDirectionThree = DayThree.SelectSingleNode("ff").InnerText;
                    f.DayWindForceThree = DayThree.SelectSingleNode("fg").InnerText;
                    f.NightWindForceThree = DayThree.SelectSingleNode("fh").InnerText;
                    f.SunriseTimeThree = DayThree.SelectSingleNode("fi").InnerText.Split('|')[0];
                    f.SunsetTimeThree = DayThree.SelectSingleNode("fi").InnerText.Split('|')[1];

                    #endregion
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("GetForecast错误>>>>>" + ex.Message);
            }
            return f;
        }

        public static WeatherIndex GetIndex(string area)
        {
            WeatherIndex i = new WeatherIndex();
            try
            {
                string json = GetJson(area, "index");
                if (!json.Equals(string.Empty) && !json.Contains("error data"))
                {
                    XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(reader);
                    Log.WriteDebug(xdoc.InnerText);

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
                        XmlNode clIndex = xdoc.FirstChild.SelectSingleNode("i").FirstChild;

                        i.cl = clIndex.SelectSingleNode("i1").InnerText;
                        i.clCN = clIndex.SelectSingleNode("i2").InnerText;
                        i.clCNAlias = clIndex.SelectSingleNode("i3").InnerText;
                        i.clLevel = clIndex.SelectSingleNode("i4").InnerText;
                        i.clDetails = clIndex.SelectSingleNode("i5").InnerText;

                        XmlNode coIndex = clIndex.NextSibling;
                        i.co = coIndex.SelectSingleNode("i1").InnerText;
                        i.coCN = coIndex.SelectSingleNode("i2").InnerText;
                        i.coCNAlias = coIndex.SelectSingleNode("i3").InnerText;
                        i.coLevel = coIndex.SelectSingleNode("i4").InnerText;
                        i.coDetails = coIndex.SelectSingleNode("i5").InnerText;


                        XmlNode ctIndex = coIndex.NextSibling;
                        i.ct = ctIndex.SelectSingleNode("i1").InnerText;
                        i.ctCN = ctIndex.SelectSingleNode("i2").InnerText;
                        i.ctCNAlias = ctIndex.SelectSingleNode("i3").InnerText;
                        i.ctLevel = ctIndex.SelectSingleNode("i4").InnerText;
                        i.ctDetails = ctIndex.SelectSingleNode("i5").InnerText;

                    }
                    catch (Exception ex)
                    {
                        Log.WriteError("GetIndex>>>>>" + ex.Message);
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Log.WriteError("GetIndex错误>>>>>" + ex.Message);
            }
            return i;
        }

        public static string GetJson(string area, string type)
        {
            string json = string.Empty;
            try
            {
                string SQL;
                try
                {
                    SQL = "select AreaID from AreaID_F where AreaID=" + int.Parse(area) + " or NameEN='" + area + "' or NameCN='" + area + "'";
                }
                catch (Exception ex)
                {
                    SQL = "select AreaID from AreaID_F where NameEN='" + area.ToLower() + "' or NameCN=N'" + area + "'";
                }
                Log.WriteDebug(SQL);
                if (type == "index")
                {
                    uri = SetUri(MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString(), indexType);
                }
                else if (type == "forecast")
                {
                    uri = SetUri(MSSQLHelper.Query(SQL).Tables[0].Rows[0][0].ToString(), forecastType);
                }
                json = HttpRequestUtil.RequestUrl(uri, "GET");
                System.GC.Collect();
            }
            catch (Exception ex)
            {
                json = "error data";
                Log.WriteError(string.Format("GetJson错误{0}[{1}]>>>>>{2}", area, type, ex.Message));
            }
            return json;
        }

        private static string SetUri(string areaID, string type)
        {
            try
            {
                privateKey = System.Configuration.ConfigurationManager.AppSettings["PrivateKey"].ToString();//私有Key
                date = DateTime.Now.ToString("yyyyMMddHHHmm");
                string publicKey = string.Format("{0}?areaid={1}&type={2}&date={3}&appid={4}", url, areaID, type, date, appID);//public_key为不包含key在内的完整URL其它部分（此处appid为完整appid）
                uri = string.Format("http://open.weather.com.cn/data/?areaid={0}&type={1}&date={2}&appid={3}&key={4}", areaID, type, date, appID_Six, GetKey(publicKey, privateKey));
                return uri;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message);
                return string.Empty;
            }
        }

        public static string GetForecastPic(string type)
        {
            
            int time = int.Parse(DateTime.Now.ToString("HH"));
            if (time < 6 || time > 18)
            {
                return string.Format("Images/Weather/Night/{0}.png", type);
            }
            else
            {
                return string.Format("Images/Weather/Day/{0}.png", type);
            }
        }

        public static List<Dictionary<string, string>> GetForecastInfo(string area)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            try
            {
                WeatherForecast f = GetForecast(area);
                if (f.CNCityName != string.Empty)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict["Title"] = f.CNCityName + "天气预报 " + DateTime.Now.ToString("yyyy年M月d日");
                    dict["Description"] = "";
                    dict["PicUrl"] = "";
                    dict["Url"] = "";
                    result.Add(dict);

                    dict = new Dictionary<string, string>();
                    dict["Title"] = string.Format("{0}市今天天气情况如下：{1}日出，白天{2},{3}摄氏度，{4}{5}；{6}日落，晚上{7},{8}摄氏度，{9}{10}。--发布时间[{11}]", f.CNCityName, f.SunriseTimeOne, WeatherForecast.GetWeatherCNByCode(f.DayWeatherCodeOne), f.DayTemperatureOne,
                    WeatherForecast.GetWindDirectionCNByCode(f.DayWindDirectionOne), WeatherForecast.GetWindForceCNByCode(f.DayWindForceOne), f.SunsetTimeOne, WeatherForecast.GetWeatherCNByCode(f.NightWeatherCodeOne), f.NightTemperatureOne, WeatherForecast.GetWindDirectionCNByCode(f.NightWindDirectionOne), WeatherForecast.GetWindForceCNByCode(f.NightWindForceOne), f.PublishTime.ToString("yyyy年MM月dd日-HH:mm:ss")).Replace(" ", "");
                    dict["Description"] = "";
                    
                    if(int.Parse(DateTime.Now.ToString("HH")) < 6 || int.Parse(DateTime.Now.ToString("HH")) > 18)
                    {
                        dict["PicUrl"] = string.Format("Images/Weather/Night/{0}.png", f.DayWeatherCodeOne);
                        //Log.WriteLog("晚上");
                    }
                    else //(int.Parse(DateTime.Now.ToString("HH")) > 6 && int.Parse(DateTime.Now.ToString("HH")) < 18)
                    {
                        dict["PicUrl"] = string.Format("Images/Weather/Day/{0}.png",f.DayWeatherCodeOne);
                        //Log.WriteLog("白天");
                    }
                    dict["Url"] = "";
                    result.Add(dict);
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.WriteDebug(ex.Message);
                return result;
            }
        }


    }


    public class WeatherForecast
    {

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

        #region 构造函数，赋初值
        /// <summary>
        /// 构造函数，赋初值
        /// </summary>
        public WeatherForecast()
        {
            AreaID = string.Empty;
            ENName = string.Empty;
            CNName = string.Empty;
            ENCityName = string.Empty;
            CNCityName = string.Empty;
            ENProvName = string.Empty;
            CNProvName = string.Empty;
            ENCountryName = string.Empty;
            CNCountryName = string.Empty;
            CityLevel = 0;
            CityAreaCode = string.Empty;;
            PostCode = 0;
            Longitude = 0;
            Latitude = 0;
            Altitude = 0;
            RadarStationNo = string.Empty;;
            PublishTime = System.DateTime.Now;
            ///第一天
            DayWeatherCodeOne = string.Empty;
            NightWeatherCodeOne = string.Empty;
            DayTemperatureOne = string.Empty;
            NightTemperatureOne = string.Empty;
            DayWindDirectionOne = string.Empty;
            NightWindDirectionOne = string.Empty;
            DayWindForceOne = string.Empty;
            NightWindForceOne = string.Empty;
            SunriseTimeOne = string.Empty;
            SunsetTimeOne = string.Empty;

            ///第二天
            DayWeatherCodeTwo = string.Empty;
            NightWeatherCodeTwo = string.Empty;
            DayTemperatureTwo = string.Empty;
            NightTemperatureTwo = string.Empty;
            DayWindDirectionTwo = string.Empty;
            NightWindDirectionTwo = string.Empty;
            DayWindForceTwo = string.Empty;
            NightWindForceTwo = string.Empty;
            SunriseTimeTwo = string.Empty;
            SunsetTimeTwo = string.Empty;
            ///第三天
            DayWeatherCodeThree = string.Empty;
            NightWeatherCodeThree = string.Empty;
            DayTemperatureThree = string.Empty;
            NightTemperatureThree = string.Empty;
            DayWindDirectionThree = string.Empty;
            NightWindDirectionThree = string.Empty;
            DayWindForceThree = string.Empty;
            NightWindForceThree = string.Empty;
            SunriseTimeThree = string.Empty;
            SunsetTimeThree = string.Empty;
        }
        #endregion

        /// <summary>
        /// 天气代码转换为中文名称
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns>中文天气</returns>
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

        /// <summary>
        /// 天气代码转换为英文名称
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns>英文天气</returns>
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

        /// <summary>
        /// 风向代码转换为中文名称
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns>中文风向</returns>
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

        /// <summary>
        /// 风向代码转换为英文名称
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns>英文风向</returns>
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

        /// <summary>
        /// 风力代码转换为中文名称
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns>中文风力</returns>
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

        /// <summary>
        /// 风力代码转换为英文名称
        /// </summary>
        /// <param name="code">代码值</param>
        /// <returns>英文风力</returns>
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

    public class WeatherIndex
    {
        #region 指数
        /// <summary>
        /// i1
        /// 晨练指数
        /// </summary>
        public string cl { get; set; }

        /// <summary>
        /// i2
        /// 晨练中文名称
        /// </summary>
        public string clCN { get; set; }

        /// <summary>
        /// i3
        /// 晨练别名
        /// </summary>
        public string clCNAlias { get; set; }

        /// <summary>
        /// i4
        /// 晨练级别
        /// </summary>
        public string clLevel { get; set; }

        /// <summary>
        /// i5
        /// 晨练详情
        /// </summary>
        public string clDetails { get; set; }

        /// <summary>
        /// i1
        /// 舒适度指数
        /// </summary>
        public string co { get; set; }

        /// <summary>
        /// i2
        /// 舒适度中文名称
        /// </summary>
        public string coCN { get; set; }

        /// <summary>
        /// i3
        /// 舒适度别名
        /// </summary>
        public string coCNAlias { get; set; }

        /// <summary>
        /// i4
        /// 舒适度级别
        /// </summary>
        public string coLevel { get; set; }

        /// <summary>
        /// i5
        /// 舒适度详情
        /// </summary>
        public string coDetails { get; set; }

        /// <summary>
        /// i1
        /// 穿衣指数
        /// </summary>
        public string ct { get; set; }

        /// <summary>
        /// i2
        /// 穿衣指数中文名称
        /// </summary>
        public string ctCN { get; set; }

        /// <summary>
        /// i3
        /// 穿衣指数别名
        /// </summary>
        public string ctCNAlias { get; set; }

        /// <summary>
        /// i4
        /// 穿衣指数级别
        /// </summary>
        public string ctLevel { get; set; }

        /// <summary>
        /// i5
        /// 穿衣指数详情
        /// </summary>
        public string ctDetails { get; set; }
        #endregion

        #region 构造函数，赋初值
        public WeatherIndex()
        {
            ///晨练指数
            cl = string.Empty;
            clCN = string.Empty;
            clCNAlias = string.Empty;
            clLevel = string.Empty;
            clDetails = string.Empty;
            ///舒适度指数
            co = string.Empty;
            coCN = string.Empty;
            coCNAlias = string.Empty;
            coLevel = string.Empty;
            coDetails = string.Empty;
            ///穿衣指数
            ct = string.Empty;
            ctCN = string.Empty;
            ctCNAlias = string.Empty;
            ctLevel = string.Empty;
            ctDetails = string.Empty;
        }
        #endregion
    }
}