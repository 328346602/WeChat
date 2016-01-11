using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using System.


namespace SWX.Utils
{
    /// <summary>
    /// 天气工具类
    /// </summary>
    public class WeatherUtil
    {
        #region 获取天气信息
        /// <summary>
        /// 获取天气信息
        /// </summary>
        public static List<Dictionary<string, string>> GetWeatherInfo()
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();


            string weatherJson = HttpRequestUtil.RequestUrl("http://www.weather.com.cn/data/sk/101220101.html", "GET");

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["Title"] = Tools.GetJsonValue(weatherJson, "city") + "天气预报 " + DateTime.Now.ToString("yyyy年M月d日");
            dict["Description"] = "";
            dict["PicUrl"] = "";
            dict["Url"] = "";
            result.Add(dict);

            dict = new Dictionary<string, string>();
            dict["Title"] = string.Format("温度：{0}℃ 湿度：{1} 风速：{2}{3}级", Tools.GetJsonValue(weatherJson, "temp"),
                Tools.GetJsonValue(weatherJson, "SD"),
                Tools.GetJsonValue(weatherJson, "WD"),
                Tools.GetJsonValue(weatherJson, "WSE"));
            dict["Description"] = "";
            dict["PicUrl"] = "";
            dict["Url"] = "";
            result.Add(dict);
            return result;
        }
        #endregion


        #region 根据城市名查询天气信息
        public static List<Dictionary<string, string>> GetWeatherInfo(string cityName)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            try
            {
                /*
                string unCityName = System.Web.HttpUtility.UrlEncode(cityName);// zhToUncode(cityName);
                string cityJson = HttpRequestUtil.RequestUrl("http://apistore.baidu.com/microservice/cityinfo?cityname=" + unCityName, "GET");
                Log.Write("cityJson>>>>>" + cityJson);
                string cityCode = Tools.GetJsonValue(cityJson, "cityCode");
                Log.Write("cityCode>>>>>" + cityCode);
                 * */
                string cityCode=string.Empty;
                

                if (cityCode != string.Empty)
                {
                    //string weatherJson = HttpRequestUtil.RequestUrl("http://www.weather.com.cn/data/sk/" + cityCode + ".html", "GET");
                    string weatherJson = HttpRequestUtil.RequestUrl("http://www.weather.com.cn/adat/sk/" + cityCode + ".html", "GET");
                    
                    
                    Log.Write("weatherJson>>>>>" + weatherJson);
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict["Title"] = Tools.GetJsonValue(weatherJson, "city") + "天气预报 " + DateTime.Now.ToString("yyyy年M月d日");
                    dict["Description"] = "";
                    dict["PicUrl"] = "";
                    dict["Url"] = "";
                    result.Add(dict);

                    dict = new Dictionary<string, string>();
                    dict["Title"] = string.Format("温度：{0}℃ 湿度：{1} 风速：{2}{3}级", Tools.GetJsonValue(weatherJson, "temp"),
                        Tools.GetJsonValue(weatherJson, "SD"),
                        Tools.GetJsonValue(weatherJson, "WD"),
                        Tools.GetJsonValue(weatherJson, "WSE"));
                    dict["Description"] = "";
                    dict["PicUrl"] = "";
                    dict["Url"] = "";
                    result.Add(dict);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                return result;
            }
        }
        #endregion


        #region 中文转unicode
        public static string uncode(string str)
        {
            string outStr = "";
            Regex reg = new Regex(@"(?i)//u([0-9a-f]{4})");
            outStr = reg.Replace(str, delegate(Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }  
        #endregion


        #region 中文转unicode
        public static string zhToUncode(string ZH)
        {
            //中文转为UNICODE字符

            string str = ZH;
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    //将中文字符转为10进制整数，然后转为16进制unicode字符
                    outStr += "//u" + ((int)str[i]).ToString("x");
                }
            }

            Log.Write("zhToUncode>>>>>" + outStr);
            return outStr;
        }
        #endregion


        #region unicode转中文
        public static string uncodeToZH(string Uncode)
        {




            //UNICODE字符转为中文

            string str = Uncode;
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("//", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            Log.Write("uncodeToZH>>>>>" + outStr);
            return outStr;
        }
        #endregion
    }
}