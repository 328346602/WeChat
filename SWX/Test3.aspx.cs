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
            string json = WeatherInfo.GetWeatherF(TextBox1.Text);
            //Response.Write(json);
            WeatherInfo wi = new WeatherInfo(json,"Forecast");
            Label1.Text = (string.Format("{0}市今天天气情况如下：{1}日出，白天{2},{3}摄氏度，{4}{5}；{6}日落，晚上{7},{8}摄氏度，{9}{10}。--发布时间[{11}]", wi.CNCityName, wi.SunriseTimeOne, WeatherInfo.GetWeatherCNByCode(wi.DayWeatherCodeOne), wi.DayTemperatureOne,
                WeatherInfo.GetWindDirectionCNByCode(wi.DayWindDirectionOne), WeatherInfo.GetWindForceCNByCode(wi.DayWindForceOne), wi.SunsetTimeOne, WeatherInfo.GetWeatherCNByCode(wi.NightWeatherCodeOne), wi.NightTemperatureOne, WeatherInfo.GetWindDirectionCNByCode(wi.NightWindDirectionOne), WeatherInfo.GetWindForceCNByCode(wi.NightWindForceOne), wi.PublishTime.ToString("yyyy年MM月dd日-hh:mm:ss")).Replace(" ",""));
            Label2.Text = (string.Format("{0}市明天天气情况如下：{1}日出，白天{2},{3}摄氏度，{4}{5}；{6}日落，晚上{7},{8}摄氏度，{9}{10}。--发布时间[{11}]", wi.CNCityName, wi.SunriseTimeTwo, WeatherInfo.GetWeatherCNByCode(wi.DayWeatherCodeTwo), wi.DayTemperatureTwo,
                WeatherInfo.GetWindDirectionCNByCode(wi.DayWindDirectionTwo), WeatherInfo.GetWindForceCNByCode(wi.DayWindForceTwo), wi.SunsetTimeTwo, WeatherInfo.GetWeatherCNByCode(wi.NightWeatherCodeTwo), wi.NightTemperatureTwo, WeatherInfo.GetWindDirectionCNByCode(wi.NightWindDirectionTwo), WeatherInfo.GetWindForceCNByCode(wi.NightWindForceTwo), wi.PublishTime.ToString("yyyy年MM月dd日-hh:mm:ss")).Replace(" ", ""));
            Label3.Text = (string.Format("{0}市后天天气情况如下：{1}日出，白天{2},{3}摄氏度，{4}{5}；{6}日落，晚上{7},{8}摄氏度，{9}{10}。--发布时间[{11}]", wi.CNCityName, wi.SunriseTimeThree, WeatherInfo.GetWeatherCNByCode(wi.DayWeatherCodeThree), wi.DayTemperatureThree,
                WeatherInfo.GetWindDirectionCNByCode(wi.DayWindDirectionThree), WeatherInfo.GetWindForceCNByCode(wi.DayWindForceThree), wi.SunsetTimeThree, WeatherInfo.GetWeatherCNByCode(wi.NightWeatherCodeThree), wi.NightTemperatureThree, WeatherInfo.GetWindDirectionCNByCode(wi.NightWindDirectionThree), WeatherInfo.GetWindForceCNByCode(wi.NightWindForceThree), wi.PublishTime.ToString("yyyy年MM月dd日-hh:mm:ss")).Replace(" ", ""));
            
        }
    }
}