using SWX.DBUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SWX
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string json = string.Empty;
            try
            {
                switch (action)
                {
                    case "Login":
                        string username = Request["username"];
                        string password = Request["password"];
                        var md5pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat(@"select * from SWX_Config where UserName='{0}' and Password = '{1}'", username, md5pwd);
                        DataTable dt = MSSQLHelper.Query(sb.ToString()).Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string status = dt.Rows[0]["Status"].ToString();
                            if (status == "1")
                            {
                                Session["WebUser"] = dt.Rows[0]["UserName"];
                                json = "{\"code\":1,\"msg\":\"\"}";
                            }
                            else
                            {
                                json = "{\"code\":0,\"msg\":\"该账号审核中或已停用，请联系管理员\"}";
                            }
                        }
                        else
                        {
                            json = "{\"code\":0,\"msg\":\"用户名或密码不正确\"}";
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