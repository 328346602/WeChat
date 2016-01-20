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
            string json = WeatherInfo.GetWeather(WeatherInfo.GetWeatherF("wuhan"), "forecast_f");
            Response.Write(json);
            //WeatherInfo wi = new WeatherInfo(json);
            //Response.Write(string.Format("{0}市天气情况如下：[{1}]日出，白天[{2}],[{3}]摄氏度，[{4}]风[{5}]级；[{6}]日落，晚上[{7}],[{8}]摄氏度，[{9}]风[{10}]级。--发布时间[{11}]",wi.CNCityName,wi.SunriseTimeOne,wi.DayWeatherCodeOne,wi.DayTemperatureOne,
            //    wi.DayWindDirectionOne,wi.DayWindForceOne,wi.SunsetTimeOne,wi.NightWeatherCodeOne,wi.NightTemperatureOne,wi.NightWindDirectionOne,wi.NightWindForceOne,wi.PublishTime.ToShortDateString()));
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
    }
}