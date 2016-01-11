using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SWX.DBUtil;
using System.Data;
using System.Collections;
using SWX.Utils;
using SWX.Models;

namespace SWX
{
    public partial class TextMessageList : PageBase
    {
        private static int pageSize = 4;//页面显示数目初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OnDataLoad();
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
            sb.AppendFormat("select count(1) from SWX_TextMessage where OrgID='{0}'", user.OrgID);

            int count = int.Parse(MSSQLHelper.GetSingle(sb.ToString()).ToString());
            this.AspNetPager.RecordCount = count;
            this.AspNetPager.PageSize = pageSize;
        }
        private void OnDataLoad()
        {
            UserInfo user = AdminUtil.GetLoginUser(this);
            InitAspNetPager();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" SELECT Top {0} * FROM (SELECT ROW_NUMBER() OVER(ORDER BY tm.Id DESC) AS RowID,", pageSize);
            sb.AppendFormat(" tm.* FROM SWX_TextMessage tm ");
            //sb.AppendFormat(" left join SWX_ImgItem ii on tm.Id = ii.TextMessageId");
            sb.AppendFormat(" where OrgID='{0}'", user.OrgID);
            sb.AppendFormat(" ) T WHERE  T.RowID BETWEEN {0} AND {1}", ((pageSize * (AspNetPager.CurrentPageIndex - 1)) + 1), pageSize * AspNetPager.CurrentPageIndex);

            DataTable dt = MSSQLHelper.Query(sb.ToString()).Tables[0];

            this.repeaterList1.DataSource = dt;
            this.repeaterList1.DataBind();
        }

        protected DataTable BindData(string Id)
        {
            StringBuilder sb2 = new StringBuilder();
            sb2.AppendFormat(" select * from SWX_ImgItem where TextMessageId = {0} ", Id);
            DataTable dt2 = MSSQLHelper.Query(sb2.ToString()).Tables[0];

            return dt2;
        }
    }
}