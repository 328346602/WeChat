using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SWX.DBUtil;
using System.Data;
using SWX.Utils;
using SWX.Models;

namespace SWX
{
    public partial class NewGroupPicture : PageBase
    {
        private static int pageSize = 10;//页面显示数目初始化
        private readonly static string UPLOAD_PATH = "\\UploadFile";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OnDataLoad();
            }

            string json = string.Empty;

            try
            {
                string action = Request["action"];
                if (action == "addPicture")
                {
                    UserInfo user = AdminUtil.GetLoginUser(this);
                    string response = Request["response"];

                    string ImgUrl = System.IO.Path.Combine(UPLOAD_PATH, DateTime.Now.ToString("yyyyMMdd"), response.Split(',')[0] + response.Split(',')[1]).Replace("\\", "/");
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("insert into SWX_GroupPicture(OrgID,ImgUrl,ImgName) ");
                    sb.AppendFormat("values('{0}','{1}','{2}') ", user.OrgID, ImgUrl, response.Split(',')[1]);

                    MSSQLHelper.Exists(sb.ToString());

                    json = "1";
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            if (!string.IsNullOrEmpty(json))
            {
                Response.Write(json);
                Response.End();
            }
        }

        /// <summary>
        /// 分页控件的翻页事件
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void AspNetPager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
        {
            this.AspNetPager.CurrentPageIndex = e.NewPageIndex;
            page1.Visible = true;
            OnDataLoad();
        }
        /// <summary>
        /// 分页控件状态设置
        /// </summary>
        protected void InitAspNetPager()
        {
            UserInfo user = AdminUtil.GetLoginUser(this);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select count(1) from SWX_GroupPicture where OrgID='{0}'", user.OrgID);

            int count = int.Parse(MSSQLHelper.GetSingle(sb.ToString()).ToString());
            this.AspNetPager.RecordCount = count;
            this.AspNetPager.PageSize = pageSize;
        }
        private void OnDataLoad()
        {
            UserInfo user = AdminUtil.GetLoginUser(this);
            InitAspNetPager();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT Top {0} * FROM (SELECT ROW_NUMBER() OVER(ORDER BY Id DESC) AS RowID,* FROM SWX_GroupPicture where OrgID='{1}') T WHERE  T.RowID BETWEEN {2} AND {3} ",
                pageSize, user.OrgID, ((pageSize * (AspNetPager.CurrentPageIndex - 1)) + 1), pageSize * AspNetPager.CurrentPageIndex);

            DataTable dt = MSSQLHelper.Query(sb.ToString()).Tables[0];

            this.repeaterList1.DataSource = dt;
            this.repeaterList1.DataBind();
        }
        protected void OnSearchButtonClick(object sender, EventArgs e)
        {
            OnDataLoad();
        }
    }
}