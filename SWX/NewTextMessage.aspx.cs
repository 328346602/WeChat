using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Text;
using SWX.DBUtil;
using System.Collections;
using System.Data;
using SWX.Models;
using SWX.Utils;

namespace SWX
{
    public partial class NewTextMessage : PageBase
    {
        public class TextMessage 
        {
            public List<ImgItem> data { get; set; }
        }

        public class ImgItem
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Content { get; set; }
            public string TextUrl { get; set; }
            public string ImgUrl { get; set; }
        }

        private readonly static string UPLOAD_PATH = "\\UploadFile";
        protected void Page_Load(object sender, EventArgs e)
        {
            string json = string.Empty;

            try
            {
                string action = Request["action"];
                if (action == "Load") 
                {
                    string id = Request["Id"];
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(" select * from SWX_ImgItem where TextMessageId = {0} ", id);
                    DataTable dt = MSSQLHelper.Query(sb.ToString()).Tables[0];

                    string list = JsonConvert.SerializeObject(dt);

                    loadtext.Value = list;
                    hiddenid.Value = id;
                }
                if (action == "addPicture")
                {
                    string response = Request["response"];

                    string ImgUrl = System.IO.Path.Combine(UPLOAD_PATH, DateTime.Now.ToString("yyyyMMdd"), response.Split(',')[0] + response.Split(',')[1]).Replace("\\", "/");
                    json = ImgUrl;
                }
                if (action == "saveText") 
                {
                    UserInfo user = AdminUtil.GetLoginUser(this);

                    List<ImgItem> IISS = new List<ImgItem>();
                    ArrayList arrayList = new ArrayList();

                    StringBuilder maxsb = new StringBuilder();
                    string editid = Request["editid"];

                    string savevalue = Request["savecontent"];
                    savevalue = savevalue.Substring(0, savevalue.Length - 1);
                    string[] strs = savevalue.Split('|');
                    for (int i = 0; i < strs.Count(); i++)
                    {
                        string str = "";
                        if (i != 0)
                        {
                            str = strs[i].Substring(1, strs[i].Length - 1);
                        }
                        else
                        {
                            str = strs[i];
                        }

                        if (str == "") { continue; }

                        List<ImgItem> IIS = JsonConvert.DeserializeAnonymousType(str, new List<ImgItem>());
                        IISS.AddRange(IIS);
                    }

                    bool istrue = false;
                    //id为空新建
                    if (string.IsNullOrWhiteSpace(editid))
                    {
                        maxsb.AppendFormat("select max(Id) from SWX_TextMessage");
                        int maxid = MSSQLHelper.GetSingle(maxsb.ToString()) == null ? 0 : int.Parse(MSSQLHelper.GetSingle(maxsb.ToString()).ToString()); ;
                        int insertid = maxid + 1;

                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("insert into SWX_TextMessage(Id,OrgID,Status,CreateTime) ");
                        sb.AppendFormat("values({0},'{1}','{2}','{3}') ", insertid, user.OrgID, true, DateTime.Now);
                        arrayList.Add(sb.ToString());

                        for (int j = 0; j < IISS.Count(); j++)
                        {
                            StringBuilder sbitem = new StringBuilder();
                            sbitem.AppendFormat("insert into SWX_ImgItem(TextMessageId,Title,Author,ImgUrl,Content,TextUrl) ");
                            sbitem.AppendFormat("values({0},'{1}','{2}','{3}','{4}','{5}') ",
                                insertid, IISS[j].Title, IISS[j].Author, IISS[j].ImgUrl, IISS[j].Content, IISS[j].TextUrl);
                            arrayList.Add(sbitem.ToString());
                        }

                        if (arrayList.Count != 1)
                        {
                            istrue = MSSQLHelper.ExecuteSqlTran(arrayList);
                        }
                    }
                    else 
                    {
                        for (int j = 0; j < IISS.Count(); j++)
                        {
                            if (string.IsNullOrWhiteSpace(IISS[j].Id))
                            {
                                StringBuilder sbitem = new StringBuilder();
                                sbitem.AppendFormat("insert into SWX_ImgItem(TextMessageId,Title,Author,ImgUrl,Content,TextUrl) ");
                                sbitem.AppendFormat("values({0},'{1}','{2}','{3}','{4}','{5}') ",
                                    editid, IISS[j].Title, IISS[j].Author, IISS[j].ImgUrl, IISS[j].Content, IISS[j].TextUrl);
                                arrayList.Add(sbitem.ToString());
                            }
                            else 
                            {
                                StringBuilder sbitem = new StringBuilder();
                                sbitem.AppendFormat("update SWX_ImgItem set Title = '{0}',Author = '{1}',ImgUrl = '{2}',Content = '{3}',TextUrl = '{4}' ",
                                    IISS[j].Title, IISS[j].Author, IISS[j].ImgUrl, IISS[j].Content, IISS[j].TextUrl);
                                sbitem.AppendFormat(" WHERE ID= {0} ", IISS[j].Id);
                                arrayList.Add(sbitem.ToString());
                            }
                        }

                        if (arrayList.Count != 0)
                        {
                            istrue = MSSQLHelper.ExecuteSqlTran(arrayList);
                        }
                    }
                    
                    if (istrue) 
                    {
                        json = "1";
                    }
                }

                if (action == "deleteText")
                {
                    string deleteid = Request["deleteid"];
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("delete from SWX_ImgItem where Id = {0}", deleteid);
                    bool istrue = MSSQLHelper.Exists(sb.ToString());
                    
                    json = "1";                    
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
    }
}