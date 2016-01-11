using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SWX.DBUtil;
using System.Text;
using SWX.Models;

namespace SWX.DAL
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuMessageDal
    {
        #region 读取菜单消息列表
        /// <summary>
        /// 读取菜单列表
        /// </summary>
        public static DataTable GetMenuList(string userName,string mkey)
        {
            StringBuilder orgsb = new StringBuilder();
            orgsb.AppendFormat(@"select OrgID from SWX_Config where UserName='{0}'", userName);
            DataTable dt = MSSQLHelper.Query(orgsb.ToString()).Tables[0];
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
//                sb.AppendFormat(@"
//                    select mm.*, wm.Name 
//                    from SWX_MenuMsg mm
//                    left join SWX_WxMenu wm on wm.MenuKey=mm.MenuKey 
//                    where mm.OrgID = '{0}' 
//                    order by wm.Code,mm.sort", dt.Rows[0]["OrgID"].ToString());
                sb.Append("select mm.*, wm.Name from SWX_MenuMsg mm left join SWX_WxMenu wm on wm.MenuKey=mm.MenuKey where mm.OrgID ='" + dt.Rows[0]["OrgID"].ToString() +"'");
                if (mkey != null) {
                    sb.Append(" and mm.MenuKey ='" +mkey+"'");
                }
                sb.Append(" order by wm.Code,mm.sort");
            }
            return MSSQLHelper.Query(sb.ToString()).Tables[0];
        }
        #endregion

        #region 根据菜单消息id获取信息
        /// <summary>
        /// 根据菜单消息id获取信息
        /// </summary>
        public static DataTable GetMenuByID(int menuId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select * from SWX_MenuMsg where id = {0}", menuId);
            return MSSQLHelper.Query(sb.ToString()).Tables[0];
        }
        #endregion

        #region 根据菜单id删除
        /// <summary>
        /// 根据菜单id删除
        /// </summary>
        public static int DeleteMenu(string menuId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"delete from SWX_MenuMsg where id = {0}", menuId);
            int row = MSSQLHelper.ExecuteSql(sb.ToString());
            return row;
        }
        #endregion

        #region 根据菜单id修改保存
        /// <summary>
        /// 根据菜单id修改保存
        /// </summary>
        public static int UpdateMenu(string menuId, string menukey, string title, string description, string picUrl, string url, string sort)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"update SWX_MenuMsg set Menukey = '{0}',Title='{1}',Description='{2}',PicUrl = '{3}',Url ='{4}',Sort={5}  where id = {6}",
                              menukey, title, description, picUrl, url, int.Parse(sort), menuId);
            int row = MSSQLHelper.ExecuteSql(sb.ToString());
            return row;
        }
        #endregion

        #region 新增菜单消息保存
        /// <summary>
        /// 新增菜单消息保存
        /// </summary>
        public static int AddMenu(string Menukey, string Title, string Description, string PicUrl, string Url, string Sort, string username)
        {
            int row = 0;
            StringBuilder orgsb = new StringBuilder();
            orgsb.AppendFormat(@"select OrgID from SWX_Config where UserName='{0}'", username);
            DataTable dt = MSSQLHelper.Query(orgsb.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                string OrgID = dt.Rows[0]["OrgID"].ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"insert into SWX_MenuMsg values( '{0}','{1}','{2}','{3}','{4}',{5},'{6}')",
                                  Menukey, Title, Description, PicUrl, Url, int.Parse(Sort), OrgID);
                row = MSSQLHelper.ExecuteSql(sb.ToString());
            }

            return row;
        }
        #endregion

        #region 预览菜单消息
        /// <summary>
        /// 预览菜单消息
        /// </summary>
        public static string GetPreviewMenuMsgHtml(UserInfo user, string id)
        {
            DataTable dt = MSSQLHelper.Query(string.Format(@"
                select * 
                from SWX_MenuMsg
                where OrgId='{0}'
                and MenuKey=
                (select MenuKey 
                from SWX_MenuMsg
                where Id={1})
                order by Sort", user.OrgID, id)).Tables[0];

            StringBuilder sb = new StringBuilder();

            if (dt.Rows.Count == 1)
            {
                #region 单条记录拼HTML
                sb.Append("<table align='center' cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:310px;'><tr><td>");
                string title = dt.Rows[0]["Title"].ToString();
                if (title.Length > 36) title = title.Substring(0, 36);
                sb.Append("<div style='width:300px; padding:5px; font-size:15px; font-weight:bold; ' >");
                sb.Append(string.Format("{0}", title));
                sb.Append("</div>");
                sb.Append("<div style='width:300px; padding:5px; font-size:12px; color:#666; ' >");
                sb.Append(string.Format("{0}", DateTime.Now.ToString("M月d日")));
                sb.Append("</div>");
                sb.Append("<div style='width:300px; height:120px; padding:5px; ' >");
                sb.Append(string.Format("<img alt='' src='{0}' style='width:300px; height:120px;' />", dt.Rows[0]["PicUrl"].ToString()));
                sb.Append("</div>");
                sb.Append("<div style='width:300px; padding:5px; font-size:12px; color:#666;' >");
                sb.Append(string.Format("{0}", dt.Rows[0]["Description"].ToString().Replace("\n", "<br />")));
                sb.Append("</div>");
                string url = dt.Rows[0]["Url"].ToString();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    sb.Append("<div style='width:300px; padding:5px; font-size:12px; margin-top:10px;' >");
                    sb.Append(string.Format("{0}", "查看全文"));
                    sb.Append("</div>");
                }
                sb.Append("</td></tr></table>");
                #endregion
            }
            else
            {
                #region 多条记录拼HTML
                sb.Append("<table align='center' cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:310px;'><tr><td>");
                sb.Append("<div align='center' style='width:300px; height:120px; padding:5px; border-top:solid 1px #ddd; border-bottom:solid 1px #ddd; border-left:solid 1px #ddd; border-right:solid 1px #ddd;' >");
                sb.Append(string.Format("<img alt='' src='{0}' style='width:300px; height:120px;' />", dt.Rows[0]["PicUrl"].ToString()));
                sb.Append("</div>");
                string title = dt.Rows[0]["Title"].ToString();
                if (title.Length > 36) title = title.Substring(0, 36);
                if (title.Length > 18)
                {
                    sb.Append("<div style='position:absolute; z-index:999; width:290px; height:33px; font-size:15px; margin-left:6px; margin-top:-49px; padding:5px; color:#fff; font-weight:bold; filter:alpha(opacity=60); -moz-opacity:0.6; -khtml-opacity: 0.6; opacity: 0.6; background-color:#000; ' >");
                }
                else
                {
                    sb.Append("<div style='position:absolute; z-index:999; width:290px; height:17px; font-size:15px; margin-left:6px; margin-top:-33px; padding:5px; color:#fff; font-weight:bold; filter:alpha(opacity=60); -moz-opacity:0.6; -khtml-opacity: 0.6; opacity: 0.6; background-color:#000; ' >");
                }
                sb.Append(string.Format("{0}", title));
                sb.Append("</div>");

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    sb.Append("<div style='width:300px; padding:5px; border-bottom:solid 1px #ddd; border-left:solid 1px #ddd; border-right:solid 1px #ddd;'>");
                    sb.Append("<table cellpadding='0' cellspacing='0' style='border-collapse:collapse; width:100%; '><tr>");
                    string picUrl = dt.Rows[i]["PicUrl"].ToString();
                    if (string.IsNullOrWhiteSpace(picUrl))
                    {
                        sb.Append("<td style='padding:5px; font-size:15px; line-height:20px;'>");
                        sb.Append(dt.Rows[i]["Title"].ToString().Replace("\n", "<br />"));
                        sb.Append("</td>");
                    }
                    else
                    {
                        sb.Append("<td style='width:250px; padding:5px; font-size:15px; line-height:20px;'>");
                        sb.Append(dt.Rows[i]["Title"].ToString().Replace("\n", "<br />"));
                        sb.Append("</td>");
                        sb.Append("<td style='vertical-align:top; '>");
                        sb.Append(string.Format("<img alt='' src='{0}' style='width:50px; height:50px;' />", dt.Rows[i]["PicUrl"].ToString()));
                        sb.Append("</td>");
                    }
                    sb.Append("</tr></table>");
                    sb.Append("</div>");
                }
                sb.Append("</td></tr></table>");
                #endregion
            }

            return sb.ToString();
        }
        #endregion

    }
}