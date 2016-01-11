using System;
using System.Web;

namespace SWX
{
    public partial class test2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HttpPostedFile file = Request.Files[0];
                file.SaveAs(MapPath("\\UploadFile\\" + file.FileName));
                Response.Write("Success\r\n");
            }
            catch(Exception ex)
            {
                Response.Write("Error\r\n");
                //throw ex;
            }
        }
    }
}