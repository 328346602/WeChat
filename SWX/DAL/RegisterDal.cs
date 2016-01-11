using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SWX.DBUtil;
using System.Text;
using System.Web.Security;

namespace SWX.DAL
{
    /// <summary>
    /// 注册
    /// </summary>
    public class RegisterDal
    {
        #region 检查用户名是否存在
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        public static DataTable CheckName(string checkname)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from SWX_Config where UserName ='{0}'",checkname);
            return MSSQLHelper.Query(sb.ToString()).Tables[0];
        }
        #endregion

        #region 注册用户信息
        /// <summary>
        /// 注册用户信息
        /// </summary>
        public static int Register(string OrgID, string Token, string AppID, string EncodingAESKey, string UserName, string Password, string AppSecret)
        {
            var md5pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"insert into SWX_Config (OrgID,Token,AppID,EncodingAESKey,UserName,Password,AppSecret) values( '{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                              OrgID, Token, AppID, EncodingAESKey, UserName, md5pwd, AppSecret);
            int row = MSSQLHelper.ExecuteSql(sb.ToString());
            return row;
        }
        #endregion

       

    }
}