using Newtonsoft.Json;
using SWX.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SWX.Utils;
using SWX.Models;
using SWX.DBUtil;

namespace SWX
{
    public partial class CaiDan : PageBase
    {
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
                        DataTable dt = MenuDal.GetMenuList(userName);
                        int count = dt.Rows.Count;
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("total", count);
                        dic.Add("rows", dt);
                        json = JsonConvert.SerializeObject(dic);
                        break;
                    case "edit":
                        string id = Request["id"];
                        DataTable menudt = MenuDal.GetMenuByID(int.Parse(id));
                        string codelen = menudt.Rows[0]["Code"].ToString();
                        if (codelen.Length == 2)
                        {
                            menudt.Rows[0]["Code"] = "0";
                        }
                        else
                        {
                            menudt.Rows[0]["Code"] = codelen.Substring(0, 2);
                        }
                        json = JsonConvert.SerializeObject(menudt);
                        break;
                    case "add":
                        string codes = Request["code"];
                        string mname = Request["name"];
                        string mtype = Request["type"];
                        string mkey = Request["key"];
                        string murl = Request["url"];
                        string username = Session["WebUser"].ToString();
                        string mcode = "";
                        if (codes == "0")
                        {
                            mcode = MenuDal.GetOneCode();
                        }
                        else
                        {
                            mcode = MenuDal.GetTwoCode(codes);
                        }
                        int addrow = MenuDal.AddMenu(mcode, mname, mtype, mkey, murl, username);
                        if (addrow == 1)
                        {
                            json = "1";
                        }
                        break;
                    case "update":
                        string menuId = Request["id"];
                        //string code = Request["code"];
                        string name = Request["name"];
                        string type = Request["type"];
                        string key = Request["key"];
                        string url = Request["url"];
                        int uprow = MenuDal.UpdateMenu(menuId, name, type, key, url);
                        if (uprow == 1)
                        {
                            json = "1";
                        }
                        break;
                    case "deleteMenu":
                        var ids = Request["id"];
                        if (ids != null)
                        {
                            int row = MenuDal.DeleteMenu(ids);
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
                    case "deleteOneMenu":
                        var oneCode = Request["code"];
                        if (oneCode != null)
                        {
                            int row = MenuDal.DeleteOneMenu(oneCode, AdminUtil.GetLoginUser(this));
                            if (row > 0)
                            {
                                json = "1";
                            }
                        }
                        break;
                    case "updateWX": //同步到微信
                        UserInfo user = AdminUtil.GetLoginUser(this);
                        string result = WXApi.CreateMenu(AdminUtil.GetAccessToken(this), user.OrgID);
                        if (Tools.GetJsonValue(result, "errcode") == "0")
                        {
                            json = "{\"code\":1,\"msg\":\"\"}";
                        }
                        else
                        {
                            json = "{\"code\":0,\"msg\":\"errcode:"
                                + Tools.GetJsonValue(result, "errcode") + ", errmsg:"
                                + Tools.GetJsonValue(result, "errmsg") + "\"}";
                        }
                        break;
                    case "tree":
                        DataTable menudts = MenuDal.GetOneMenuList(AdminUtil.GetLoginUser(this));
                        List<MenuModel> flowList = new List<MenuModel>();
                        MenuModel flow;
                        foreach (DataRow drFlowType in menudts.Rows)
                        {
                            flow = new MenuModel();
                            flow.id = drFlowType["Code"].ToString();
                            flow.level = 0;
                            flow.parent = "-1";
                            flow.isLeaf = false;
                            flow.expanded = true;

                            flow.Id = int.Parse(drFlowType["Id"].ToString());
                            flow.Code = drFlowType["Code"].ToString();
                            flow.Name = drFlowType["Name"].ToString();
                            flow.Type = drFlowType["Type"].ToString();
                            flow.MenuKey = drFlowType["MenuKey"].ToString();
                            flow.Url = drFlowType["Url"].ToString();
                            flow.OrgID = drFlowType["OrgID"].ToString();
                            flowList.Add(flow);
                            DataTable menudts2 = MenuDal.GetTwoMenuList(flow.Code);
                            foreach (DataRow twoRow in menudts2.Rows)
                            {
                                MenuModel flow2 = new MenuModel();
                                flow2.id = twoRow["Code"].ToString();
                                flow2.level = 1;
                                flow2.parent = twoRow["Code"].ToString().Substring(0, 2);
                                flow2.isLeaf = true;
                                flow2.expanded = true;

                                flow2.Id = int.Parse(twoRow["Id"].ToString());
                                flow2.Code = twoRow["Code"].ToString();
                                flow2.Name = twoRow["Name"].ToString();
                                flow2.Type = twoRow["Type"].ToString();
                                flow2.MenuKey = twoRow["MenuKey"].ToString();
                                flow2.Url = twoRow["Url"].ToString();
                                flow2.OrgID = twoRow["OrgID"].ToString();
                                flowList.Add(flow2);
                            }
                        }
                        json = JsonConvert.SerializeObject(flowList);
                        break;
                    case "menulist":
                        DataTable onedt = MenuDal.GetOneMenuList(AdminUtil.GetLoginUser(this));
                        DataRow onerow = onedt.NewRow();
                        onerow["Code"] = 0;
                        onerow["Name"] = "无";
                        onedt.Rows.InsertAt(onerow, 0);
                        json = JsonConvert.SerializeObject(onedt);
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