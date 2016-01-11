using Newtonsoft.Json;
using SWX.DAL;
using SWX.DBUtil;
using SWX.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.Models;

namespace SWX
{
    public partial class MenuMessage : PageBase
    {
        private readonly static string UPLOAD_PATH = "\\UploadFile";

        public string GetMenus(string selectId)
        {
            UserInfo user = AdminUtil.GetLoginUser(this);
            return MenuDal.GetMenusLevel2(user, selectId);
        }

        public string GetMenusKey()
        {
            UserInfo user = AdminUtil.GetLoginUser(this);
            return MenuDal.GetMenusLevel3(user);
        }

        public string GetFile()
        {
            string jdUrl = AdminUtil.GetRootUrl();
            return jdUrl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string json = string.Empty;
            try
            {
                switch (action)
                {
                    case "Load":
                        int pageSize = Convert.ToInt32(Request["rows"]);
                        int pageNum = Convert.ToInt32(Request["page"]);
                        string userName = Session["WebUser"].ToString();
                        string mkey = Request["mkey"];
                        DataTable dt = MenuMessageDal.GetMenuList(userName,mkey);
                        int count = dt.Rows.Count;
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("total", count);
                        dic.Add("rows", dt);
                        json = JsonConvert.SerializeObject(dic);
                        break;
                    case "edit":
                        string id = Request["id"];
                        DataTable menudt = MenuMessageDal.GetMenuByID(int.Parse(id));
                        json = JsonConvert.SerializeObject(menudt);
                        break;
                    case "add":
                        string Menukey = Request["Menukey"];
                        string Title = Request["Title"];
                        string Description = Request["Description"];
                        string PicUrl = Request["PicUrl"];
                        string Url = Request["Url"];
                        string Sort = Request["Sort"];
                        string username = Session["WebUser"].ToString();
                        int addrow = MenuMessageDal.AddMenu(Menukey, Title, Description, PicUrl, Url, Sort, username);
                        if (addrow == 1)
                        {
                            json = "1";
                        }
                        break;
                    case "update":
                        string menuId = Request["id"];
                        string menukey = Request["Menukey"];
                        string title = Request["Title"];
                        string description = Request["Description"];
                        string picUrl = Request["PicUrl"];
                        string url = Request["Url"];
                        string sort = Request["Sort"];
                        int uprow = MenuMessageDal.UpdateMenu(menuId, menukey, title, description, picUrl, url, sort);
                        if (uprow == 1)
                        {
                            json = "1";
                        }
                        break;
                    case "deleteMenu":
                        var ids = Request["id"];
                        if (ids != null)
                        {
                            int row = MenuMessageDal.DeleteMenu(ids);
                            if (row == 1)
                            {
                                json = "1";
                            }
                            else
                            {
                                json = "0";
                            }

                        }
                        else
                        {
                            json = "0";
                        }
                        break;
                    case "addPicture":
                        string response = Request["response"];
                        //string jdUrl = AdminUtil.GetRootUrl();
                        string ImgUrl = System.IO.Path.Combine(UPLOAD_PATH, DateTime.Now.ToString("yyyyMMdd"), response.Split(',')[0] + response.Split(',')[1]).Replace("\\", "/");

                        json =  ImgUrl;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            if (!string.IsNullOrEmpty(json))
            {
                Response.Write(json);
                Response.End();
            }
        }

    }


}