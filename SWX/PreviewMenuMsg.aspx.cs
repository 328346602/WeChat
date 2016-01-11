using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.DAL;
using SWX.Models;
using SWX.Utils;

namespace SWX
{
    public partial class PreviewMenuMsg : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request["id"];
            UserInfo user = AdminUtil.GetLoginUser(this);
            string html = MenuMessageDal.GetPreviewMenuMsgHtml(user, id);
            Response.Write(html);
            Response.End();
        }
    }
}