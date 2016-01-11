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
    public partial class ModifyPassword : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string json = string.Empty;
            try
            {
                switch (action)
                {
                    case "modifyPwd":
                        string oldpwd = Request["oldpwd"];
                        string newpwd = Request["newpwd"];
                        string md5pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(oldpwd, "MD5");
                        string md5Newpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(newpwd, "MD5");
                        string username = Session["WebUser"].ToString();
                        StringBuilder orgsb = new StringBuilder();
                        orgsb.AppendFormat(@"select Password from SWX_Config where UserName='{0}'", username);
                        DataTable dt = MSSQLHelper.Query(orgsb.ToString()).Tables[0];
                        if (md5pwd != dt.Rows[0]["Password"].ToString())
                        {
                            json = "2";
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat(@"update SWX_Config set Password = '{0}' where UserName = '{1}'", md5Newpwd, username);
                            int row = MSSQLHelper.ExecuteSql(sb.ToString());
                            if (row == 1)
                            {
                                json = "1";
                            }
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