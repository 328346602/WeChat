using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.DBUtil;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using SWX.Utils;
using System.IO;
using SWX.Models;
using SWX.DAL;

namespace SWX
{
    public partial class NewGroupNews : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string json = string.Empty;

            try
            {
                string action = Request["action"];
                if (action == "findText")
                {
                    string Id = Request["Id"];
                    StringBuilder sb1 = new StringBuilder();
                    sb1.AppendFormat(" select * from SWX_TextMessage where Id = {0} ", Id);
                    DataTable dt1 = MSSQLHelper.Query(sb1.ToString()).Tables[0];
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("CreateTime", dt1.Rows[0]["CreateTime"].ToString());

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(" select * from SWX_ImgItem where TextMessageId = {0} ", Id);
                    DataTable dt = MSSQLHelper.Query(sb.ToString()).Tables[0];
                    dic.Add("List", JsonConvert.SerializeObject(dt));

                    json = JsonConvert.SerializeObject(dic);
                }
                if (action == "send")
                {
                    json = Send();
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
        /// 群发
        /// </summary>
        public string Send()
        {
            string type = Request["type"];
            string data = Request["data"];

            string access_token = AdminUtil.GetAccessToken(this); //获取access_token
            List<string> openidList = WXApi.GetOpenIDs(access_token); //获取关注者OpenID列表
            UserInfo loginUser = AdminUtil.GetLoginUser(this); //当前登录用户 

            string resultMsg = null;

            //发送文本
            if (type == "1")
            {
                resultMsg = WXApi.Send(access_token, WXMsgUtil.CreateTextJson(data, openidList));
            }

            //发送图片
            if (type == "2")
            {
                string path = MapPath(data);
                if (!File.Exists(path))
                {
                    return "{\"code\":0,\"msg\":\"要发送的图片不存在\"}";
                }
                string msg = WXApi.UploadMedia(access_token, "image", path);
                string media_id = Tools.GetJsonValue(msg, "media_id");
                resultMsg = WXApi.Send(access_token, WXMsgUtil.CreateImageJson(media_id, openidList));
            }

            //发送图文消息
            if (type == "3")
            {
                DataTable dt = ImgItemDal.GetImgItemTable(loginUser.OrgID, data);
                string articlesJson = ImgItemDal.GetArticlesJsonStr(this, access_token, dt);
                string newsMsg = WXApi.UploadNews(access_token, articlesJson);
                string newsid = Tools.GetJsonValue(newsMsg, "media_id");
                resultMsg = WXApi.Send(access_token, WXMsgUtil.CreateNewsJson(newsid, openidList));
            }

            //结果处理
            if (!string.IsNullOrWhiteSpace(resultMsg))
            {
                string errcode = Tools.GetJsonValue(resultMsg, "errcode");
                string errmsg = Tools.GetJsonValue(resultMsg, "errmsg");
                if (errcode == "0")
                {
                    return "{\"code\":1,\"msg\":\"\"}";
                }
                else
                {
                    return "{\"code\":0,\"msg\":\"errcode:"
                        + errcode + ", errmsg:"
                        + errmsg + "\"}";
                }
            }
            else
            {
                return "{\"code\":0,\"msg\":\"type参数错误\"}";
            }
        }

    }
}