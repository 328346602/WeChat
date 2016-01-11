using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SWX.DBUtil;

namespace SWX.DAL
{
    /// <summary>
    /// 菜单事件
    /// </summary>
    public class MenuMsgDal
    {
        #region 根据菜单事件获取消息
        /// <summary>
        /// 根据菜单事件获取消息
        /// </summary>
        public static DataTable GetMenuMsg(string eventKey)
        {
            return MSSQLHelper.Query(string.Format("select * from SWX_MenuMsg where MenuKey='{0}' order by Sort", eventKey)).Tables[0];
        }
        #endregion

    }
}