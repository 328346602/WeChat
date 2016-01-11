using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SWX.DBUtil;

namespace SWX.DAL
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserDal
    {
        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        public static DataTable GetAllUserList()
        {
            return MSSQLHelper.Query(string.Format("select * from SWX_Config where UserName<>'admin'")).Tables[0];
        }
        #endregion

        #region 启用/停用用户
        /// <summary>
        /// 启用/停用用户
        /// </summary>
        public static void UpdateUser(int id, int status)
        {
            MSSQLHelper.ExecuteSql(string.Format(@"
                update SWX_Config
                set Status={1}
                where Id={0}", id, status));
        }
        #endregion

    }
}