using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SWX.Utils;

namespace SWX
{
    /// <summary>
    /// 页面基类
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        #region 登录验证
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //验证代码
            if (!AdminUtil.IsLogin(this))
            {
                Response.Redirect("Login.aspx");
            }
        }
        #endregion

    }
}