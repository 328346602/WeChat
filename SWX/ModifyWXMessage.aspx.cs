using Newtonsoft.Json;
using SWX.DBUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SWX
{
    public partial class ModifyWXMessage : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string json = string.Empty;
            try
            {
                switch (action)
                {
                    case "Load":
                        string username2 = Session["WebUser"].ToString();
                        StringBuilder orgsb = new StringBuilder();
                        orgsb.AppendFormat(@"select * from SWX_Config where UserName='{0}'", username2);
                        DataTable dt = MSSQLHelper.Query(orgsb.ToString()).Tables[0];
                        json = JsonConvert.SerializeObject(dt);
                        break;
                    case "update":
                        string OrgID = Request["OrgID"];
                        string Token = Request["Token"];
                        string AppID = Request["AppID"];
                        string EncodingAESKey = Request["EncodingAESKey"];
                        string AppSecret = Request["AppSecret"];
                        string username = Session["WebUser"].ToString();
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat(@"update SWX_Config set OrgID='{0}',Token = '{1}',AppID='{2}',EncodingAESKey='{3}',AppSecret = '{4}' where UserName='{5}'"
                                        , OrgID, Token, AppID, EncodingAESKey, AppSecret, username);
                        int row = MSSQLHelper.ExecuteSql(sb.ToString());
                        if (row == 1)
                        {
                            json = "1";
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            if (!string.IsNullOrEmpty(json))
            {
                Response.Write(json);
                Response.End();
            }
        }
    }
}