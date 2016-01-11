using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.DBUtil;
using System.Data;
using SWX.Utils;
using SWX.DAL;

namespace SWX
{
    public partial class test : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string result = "";
            //string result = WXApi.GetToken("wx78029370fbbd5443", "e89ad54702a996fd490ef7b1959b0162");
            //Response.Write(result);
            //string result = WXApi.UploadMedia("YDJnIPGkg31NV6XVi08Z-YBOLeheMDJQ7_vXclmTdLz8guImg8XStAVZ8Macq5b8596sk9-vijajieEFnN40IU9bDHN_NSU3Md7Q6rYnUIc", "abc");
            //Response.Write(result);
            //Response.Write(WXApi.CreateMenu("YDJnIPGkg31NV6XVi08Z-YBOLeheMDJQ7_vXclmTdLz8guImg8XStAVZ8Macq5b8596sk9-vijajieEFnN40IU9bDHN_NSU3Md7Q6rYnUIc", "gh_173ce0ac1506"));

            //获取Token，并缓存
            //string sToken = null;
            //Session.Add("token", "td482xCygxW95aiQ3VXQk77xCaU_QxLJtcrBARSK8xFinY2lbZ-EqPrw0C3ylA1eBo8Zt54si-KPPAt0t0x8pORzHyS8m_KzjUDtEtmjU4U");
            //if (Session["token"] == null)
            //{
            //    sToken = WXApi.GetToken("wx78029370fbbd5443", "e89ad54702a996fd490ef7b1959b0162");
            //    Session.Add("token", sToken);
            //    Session.Timeout = 2 * 60;
            //}
            //else
            //{
            //    sToken = Session["token"].ToString();
            //}
            //result += sToken + "<br />";

            //result += WXApi.CreateMenu(sToken, "gh_173ce0ac1506") + "<br />";
            //Response.Write(result);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //WXApi.DownloadMedia("http://localhost:2454/UploadFile/aa.png");
            //Response.Write(WXApi.UploadMedia(Session["token"].ToString(), "image", "d:\\_临时文件\\aa.png"));
            //获取天气预报
            //string result = HttpRequestUtil.RequestUrl("http://www.weather.com.cn/data/sk/101220101.html", "GET");
            //Response.Write(result);
            //result = Weixin.Mp.Util.Tools.GetJosnValue(result, "WSE");
            //Response.Write(result);
            //Response.Write(AdminUtil.GetRootUrl());
            //string result = WXApi.CreateMenu(AdminUtil.GetAccessToken(this), AdminUtil.GetLoginUser(this).OrgID);
            //Response.Write(AdminUtil.GetRootUrl());

            //string path = MapPath("UploadFile/aa.png");
            //string id = WXApi.UploadMedia(AdminUtil.GetAccessToken(this), "image", path);
            //Response.Write(id);

            Response.Write(WXMsgUtil.CreateTextJson("abc", WXApi.GetOpenIDs(AdminUtil.GetAccessToken(this))));

        }
    }
}