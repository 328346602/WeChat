using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.Utils;
using System.Text;
using System.Data;
using SWX.DBUtil;

using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

using SWX.Utils;

namespace SWX
{
    public partial class Test3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 测试网上的反序列化方法
            //Weather weather = new Weather();
            //json = Weather.GetWeather("101180701", "forecast_f");
            //XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max);
            //XmlDocument xdoc = new XmlDocument();
            //xdoc.Load(reader);
            //for (int i = 0; i < xdoc.ChildNodes.Count; i++)
            //{
            //    //Response.Write(xdoc.ChildNodes[i].Name+xdoc[xdoc.ChildNodes[i].Name].Value);
            //    for (int j = 0; j < xdoc.ChildNodes[i].ChildNodes.Count; j++)
            //    {
            //        Response.Write(xdoc.ChildNodes[i].ChildNodes[j].Name + ">>>>>" + xdoc.ChildNodes[i].ChildNodes[j].InnerText + "\n");
            //        for (int n = 0; n < xdoc.ChildNodes[i].ChildNodes[j].ChildNodes.Count; n++)
            //        {
            //            Response.Write(xdoc.ChildNodes[i].ChildNodes[j].ChildNodes[n].Name + ">>>>>" + xdoc.ChildNodes[i].ChildNodes[j].ChildNodes[n].InnerText + "\n");
            //        }
            //    }
            //}
            //Response.Write(xdoc.InnerText);


            #endregion

            //Response.Write(JsonHepler.JsonSerializerBySingleData(Weather.GetWeather("101090211", "forecast_v")));

            #region 循环获取全部数据
            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat(@"select AreaID from AreaID_V ");
            //DataTable dt;
            //try
            //{
            //    dt = MSSQLHelper.Query(sb.ToString()).Tables[0];
            //}
            //catch (Exception ex)
            //{
            //    Log.WriteError("循环获取全国数据错误>>>>>" + ex.Message);
            //    throw ex;
            //}
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        Log.WriteDebug(Weather.GetWeather(dt.Rows[i][0].ToString(), "forecast_v"));
            //    }
            //}
            //else
            //{
            //    json = "{\"code\":0,\"msg\":\"AreaID_V表中未查询到数据！\"}";
            //}
            #endregion

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Log.WriteLog(DateTime.Now.ToString("HH"));
   
            WeatherForecast wi = Weather.GetForecast(TextBox1.Text);
            WeatherIndex ind = Weather.GetIndex(TextBox1.Text);
            if (!wi.CNCityName.Equals(string.Empty))
            {
                Response.Write(string.Format("{0}市今天天气情况如下：{1}日出，白天{2},{3}摄氏度，{4}{5}；<br/>{6}日落，晚上{7},{8}摄氏度，{9}{10}。--发布时间[{11}]{12}", wi.CNCityName, wi.SunriseTimeOne, WeatherForecast.GetWeatherCNByCode(wi.DayWeatherCodeOne), wi.DayTemperatureOne,
                    WeatherForecast.GetWindDirectionCNByCode(wi.DayWindDirectionOne), WeatherForecast.GetWindForceCNByCode(wi.DayWindForceOne), wi.SunsetTimeOne, WeatherForecast.GetWeatherCNByCode(wi.NightWeatherCodeOne), wi.NightTemperatureOne, WeatherForecast.GetWindDirectionCNByCode(wi.NightWindDirectionOne), WeatherForecast.GetWindForceCNByCode(wi.NightWindForceOne), wi.PublishTime.ToString("yyyy年MM月dd日-HH:mm:ss"), "<br/>").Replace(" ", ""));
                Response.Write(string.Format("<img src=\"{0}\")<img/><br/>", Weather.GetForecastPic(wi.DayWeatherCodeOne)));
                
                Response.Write(string.Format("{0}市明天天气情况如下：{1}日出，白天{2},{3}摄氏度，{4}{5}；<br/>{6}日落，晚上{7},{8}摄氏度，{9}{10}。--发布时间[{11}]{12}", wi.CNCityName, wi.SunriseTimeTwo, WeatherForecast.GetWeatherCNByCode(wi.DayWeatherCodeTwo), wi.DayTemperatureTwo,
                    WeatherForecast.GetWindDirectionCNByCode(wi.DayWindDirectionTwo), WeatherForecast.GetWindForceCNByCode(wi.DayWindForceTwo), wi.SunsetTimeTwo, WeatherForecast.GetWeatherCNByCode(wi.NightWeatherCodeTwo), wi.NightTemperatureTwo, WeatherForecast.GetWindDirectionCNByCode(wi.NightWindDirectionTwo), WeatherForecast.GetWindForceCNByCode(wi.NightWindForceTwo), wi.PublishTime.ToString("yyyy年MM月dd日-HH:mm:ss"), "<br/>").Replace(" ", ""));
                Response.Write(string.Format("<img src=\"{0}\")<img/><br/>", Weather.GetForecastPic(wi.DayWeatherCodeTwo)));
                
                Response.Write(string.Format("{0}市后天天气情况如下：{1}日出，白天{2},{3}摄氏度，{4}{5}；<br/>{6}日落，晚上{7},{8}摄氏度，{9}{10}。--发布时间[{11}]{12}", wi.CNCityName, wi.SunriseTimeThree, WeatherForecast.GetWeatherCNByCode(wi.DayWeatherCodeThree), wi.DayTemperatureThree,
                    WeatherForecast.GetWindDirectionCNByCode(wi.DayWindDirectionThree), WeatherForecast.GetWindForceCNByCode(wi.DayWindForceThree), wi.SunsetTimeThree, WeatherForecast.GetWeatherCNByCode(wi.NightWeatherCodeThree), wi.NightTemperatureThree, WeatherForecast.GetWindDirectionCNByCode(wi.NightWindDirectionThree), WeatherForecast.GetWindForceCNByCode(wi.NightWindForceThree), wi.PublishTime.ToString("yyyy年MM月dd日-HH:mm:ss"), "<br/>").Replace(" ", ""));
                Response.Write(string.Format("<img src=\"{0}\")<img/><br/>", Weather.GetForecastPic(wi.DayWeatherCodeThree)));
                
            }
            else
            {
                Response.Write("未能获取到对应城市的天气预报信息！");
            }

            if (!ind.clCN.Equals(string.Empty))
            {
                Response.Write(string.Format("{0}：{1}<br/>{2}。<br/>{3}<br/>{4}：{5}<br/>{6}。{7}<br/>{8}：{9}<br/>{10}。{11}<br/>{12}", ind.clCN, ind.clCNAlias, ind.clLevel, ind.clDetails, ind.coCN, ind.coCNAlias, ind.coLevel, ind.coDetails, ind.ctCN, ind.ctCNAlias, ind.ctLevel, ind.ctDetails, "<br/>"));
            }
            else
            {
                Response.Write("未能获取到对应城市的天气指数信息！");
            }
        }
    }
}