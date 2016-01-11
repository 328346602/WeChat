using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace SWX
{
    /// <summary>
    /// 文件上传工具类
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            string prefix = "";

            if (file != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                prefix = System.Guid.NewGuid().ToString(); // 追加一个前缀，避免上传文件重名
                file.SaveAs(uploadPath + prefix + file.FileName);
                context.Response.Write(prefix + "," + file.FileName); // 下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
            }
            else
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}