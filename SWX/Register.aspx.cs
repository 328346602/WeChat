using SWX.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SWX
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request["action"];
            string json = string.Empty;
            try
            {
                switch (action)
                {
                    case "checkName":
                        string name = Request["name"];
                        DataTable dt = RegisterDal.CheckName(name);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            json = "1";
                        }
                        else {
                            json = "0";
                        }
                        break;
                    case "register":
                        string OrgID = Request["OrgID"];
                        string Token = Request["Token"];
                        string AppID = Request["AppID"];
                        string EncodingAESKey = Request["EncodingAESKey"];
                        string UserName = Request["UserName"];
                        string Password = Request["Password"];
                        string AppSecret = Request["AppSecret"];
                        int rowcount = RegisterDal.Register(OrgID, Token, AppID, EncodingAESKey, UserName, Password, AppSecret);
                        if(rowcount == 1){
                            json = "1";
                        }
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