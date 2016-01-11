using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using SWX.DBUtil;
using System.IO;
using SWX.Utils;

namespace SWX.DAL
{
    /// <summary>
    /// 群发图文消息表
    /// </summary>
    public class ImgItemDal
    {
        #region 获取图文消息列表
        /// <summary>
        /// 获取图文消息列表
        /// </summary>
        public static DataTable GetImgItemTable(string orgID, string textMessageId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"
                select * 
                from SWX_ImgItem ii
                left join SWX_TextMessage tm on tm.Id=ii.TextMessageId
                where tm.OrgID='{0}' and tm.Id={1}", orgID, textMessageId);

            return MSSQLHelper.Query(sb.ToString()).Tables[0];
        }
        #endregion

        #region 拼接图文消息素材Json字符串
        /// <summary>
        /// 拼接图文消息素材Json字符串
        /// </summary>
        public static string GetArticlesJsonStr(PageBase page, string access_token, DataTable dt)
        {
            StringBuilder sbArticlesJson = new StringBuilder();

            sbArticlesJson.Append("{\"articles\":[");
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string path = page.MapPath(dr["ImgUrl"].ToString());
                if (!File.Exists(path))
                {
                    return "{\"code\":0,\"msg\":\"要发送的图片不存在\"}";
                }
                string msg = WXApi.UploadMedia(access_token, "image", path); // 上图片返回媒体ID
                string media_id = Tools.GetJsonValue(msg, "media_id");
                sbArticlesJson.Append("{");
                sbArticlesJson.Append("\"thumb_media_id\":\"" + media_id + "\",");
                sbArticlesJson.Append("\"author\":\"" + dr["Author"].ToString() + "\",");
                sbArticlesJson.Append("\"title\":\"" + dr["Title"].ToString() + "\",");
                sbArticlesJson.Append("\"content_source_url\":\"" + dr["TextUrl"].ToString() + "\",");
                sbArticlesJson.Append("\"content\":\"" + dr["Content"].ToString() + "\",");
                sbArticlesJson.Append("\"digest\":\"" + dr["Content"].ToString() + "\",");
                if (i == dt.Rows.Count - 1)
                {
                    sbArticlesJson.Append("\"show_cover_pic\":\"1\"}");
                }
                else
                {
                    sbArticlesJson.Append("\"show_cover_pic\":\"1\"},");
                }
                i++;
            }
            sbArticlesJson.Append("]}");

            return sbArticlesJson.ToString();
        }
        #endregion

    }
}