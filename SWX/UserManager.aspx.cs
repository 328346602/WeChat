using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.Utils;
using System.Data;
using SWX.DAL;
using Newtonsoft.Json;

namespace SWX
{
    public partial class UserManager : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 判断是不是超级管理员
            if (!AdminUtil.IsAdmin(this)) //不是超级管理员
            {
                Response.Redirect("Login.aspx");
            }
            #endregion

            string action = Request["action"];
            if (!string.IsNullOrWhiteSpace(action)) action = action.ToLower();
            string json = string.Empty;
            switch (action)
            {
                case "load":
                    int pageSize = Convert.ToInt32(Request["rows"]);
                    int pageNum = Convert.ToInt32(Request["page"]);
                    string userName = Session["WebUser"].ToString();
                    DataTable dt = UserDal.GetAllUserList();
                    int count = dt.Rows.Count;
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("total", count);
                    dic.Add("rows", dt);
                    json = JsonConvert.SerializeObject(dic);
                    break;
                case "qy":
                    UserDal.UpdateUser(int.Parse(Request["id"]), 1);
                    json = "1";
                    break;
                case "ty":
                    UserDal.UpdateUser(int.Parse(Request["id"]), 0);
                    json = "1";
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(json))
            {
                Response.Write(json);
                Response.End();
            }
        }
    }
}