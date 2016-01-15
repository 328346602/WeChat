﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.Utils;
using System.Text;
using System.Data;
using SWX.DBUtil;

namespace SWX
{
    public partial class Test3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string json = string.Empty;
            //Log.WriteLog(Weather.GetWeather("101010100|101180701", "forecast_f"));
            Response.Write(Weather.GetWeather("101180701", "forecast_v"));

            
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select AreaID from AreaID_V ");
            DataTable dt;
            try
            {
                dt = MSSQLHelper.Query(sb.ToString()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Log.WriteDebug(Weather.GetWeather(dt.Rows[i][0].ToString(), "forecast_v"));
                }
            }
            else
            {
                json = "{\"code\":0,\"msg\":\"AreaID_V表中未查询到数据！\"}";
            }

        }
    }
}