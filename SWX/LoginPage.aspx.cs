using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.Utils;

namespace SWX
{
    public partial class LoginPage : PageBase
    {
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                return AdminUtil.IsAdmin(this);
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                object userName = Session["WebUser"];
                if (userName != null) return userName.ToString();
                return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string json = string.Empty;
            try
            {
                switch (action)
                {
                    case "Logout":
                        Session.Clear();
                        json = "1";
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