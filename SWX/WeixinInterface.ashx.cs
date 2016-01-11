using System;
using System.Web;
using SWX.Utils;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Tencent;
using SWX.DAL;
using System.Data;

namespace SWX
{
    /// <summary>
    /// 微信接口
    /// </summary>
    public class WeixinInterface : IHttpHandler
    {
        #region 变量
        //公众平台上开发者设置的token, appID, EncodingAESKey
        string sToken = "TestAppLDM";
        //string sAppID = "wxdcd08978e13e3e39";
        string sAppID = null;
        string sEncodingAESKey = "2iNBcpQ2aBETVIHofGntsgg6P0m5CWGsjBJofGvCzSI";
        #endregion

        #region ProcessRequest
        public void ProcessRequest(HttpContext context)
        {
            Log.WriteDebug("ProcessRequest start");
            try
            {
                Stream stream = context.Request.InputStream;
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, (int)stream.Length);
                string postXmlStr = System.Text.Encoding.UTF8.GetString(byteArray);
                Log.WriteDebug("1");
                if (!string.IsNullOrEmpty(postXmlStr))
                {
                    Log.WriteDebug("IsNullOrEmpty");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(postXmlStr);
                    if (string.IsNullOrWhiteSpace(sToken))
                    {
                        Log.WriteDebug("string.IsNullOrWhiteSpace(sToken)");
                        DataTable dt = ConfigDal.GetConfig(WXMsgUtil.GetFromXML(doc, "ToUserName"));
                        DataRow dr = dt.Rows[0];
                        sToken = dr["Token"].ToString();
                        sAppID = dr["AppID"].ToString();
                        sEncodingAESKey = dr["EncodingAESKey"].ToString();
                        Log.WriteDebug(sToken + "\r\n" + sAppID + "\r\n" + sEncodingAESKey + "\r\n");
                    }
                    Log.WriteDebug("2");
                    if (!string.IsNullOrWhiteSpace(sAppID))  //没有AppID则不解密(订阅号没有AppID)
                    {
                        Log.WriteDebug("!string.IsNullOrWhiteSpace(sAppID)");
                        //解密
                        WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                        string signature = context.Request["msg_signature"];
                        string timestamp = context.Request["timestamp"];
                        string nonce = context.Request["nonce"];
                        Log.WriteDebug(signature + "\r\n" + timestamp + "\r\n" + nonce + "\r\n");
                        string stmp = "";
                        int ret = wxcpt.DecryptMsg(signature, timestamp, nonce, postXmlStr, ref stmp);
                        if (ret == 0)
                        {
                            doc = new XmlDocument();
                            doc.LoadXml(stmp);

                            try
                            {
                                Log.WriteDebug("3");
                                responseMsg(context, doc);
                            }
                            catch (Exception ex)
                            {
                                //FileLogger.WriteErrorLog(context, ex.Message);
                                Log.WriteError(ex.Message);
                            }
                        }
                        else
                        {
                            //FileLogger.WriteErrorLog(context, "解密失败，错误码：" + ret);
                            Log.WriteError("解密失败，错误码：" + ret);
                        }
                    }
                    else
                    {
                        Log.WriteDebug("responseMsg(context, doc);");
                        responseMsg(context, doc);
                    }
                }
                else
                {
                    Log.WriteError("valid(context);");
                    valid(context);
                }
            }
            catch (Exception ex)
            {
                //FileLogger.WriteErrorLog(context, ex.Message);
                Log.WriteError("ProcessRequest" + context.ToString() + ex.Message);
            }
        }
        #endregion

        #region IsReusable
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region valid
        public void valid(HttpContext context)
        {
            try
            {
                var echostr = context.Request["echoStr"].ToString();
                Log.WriteDebug("var echostr = context.Request[\"echoStr\"].ToString();>>>>>" + echostr);
                if (checkSignature(context) && !string.IsNullOrEmpty(echostr))
                {
                    context.Response.Write(echostr);
                    Log.WriteDebug("context.Response.Write(echostr);>>>>>" + echostr);
                    context.Response.Flush();//推送...不然微信平台无法验证token
                }
            }
            catch(Exception ex)
            {
                Log.WriteError("valid() Error>>>>>" + ex.Message);
            }
        }
        #endregion

        #region checkSignature
        public bool checkSignature(HttpContext context)
        {
            var signature = context.Request["signature"].ToString();
            var timestamp = context.Request["timestamp"].ToString();
            var nonce = context.Request["nonce"].ToString();
            var token = sToken;//"weixin";
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                Log.WriteDebug("checkSignature>>>>>true");
                return true;
            }
            else
            {
                Log.WriteDebug("checkSignature>>>>>false");
                return false;
            }
        }
        #endregion

        #region responseMsg
        public void responseMsg(HttpContext context, XmlDocument xmlDoc)
        {
            string result = "";
            string msgType = WXMsgUtil.GetFromXML(xmlDoc, "MsgType");
            switch (msgType)
            {
                case "event":
                    switch (WXMsgUtil.GetFromXML(xmlDoc, "Event"))
                    {
                        case "subscribe": //订阅
                            break;
                        case "unsubscribe": //取消订阅
                            break;
                        case "CLICK":
                            DataTable dtMenuMsg = MenuMsgDal.GetMenuMsg(WXMsgUtil.GetFromXML(xmlDoc, "EventKey"));
                            if (dtMenuMsg.Rows.Count > 0)
                            {
                                List<Dictionary<string, string>> dictList = new List<Dictionary<string, string>>();
                                foreach (DataRow dr in dtMenuMsg.Rows)
                                {
                                    Dictionary<string, string> dict = new Dictionary<string, string>();
                                    dict["Title"] = dr["Title"].ToString();
                                    dict["Description"] = dr["Description"].ToString();
                                    dict["PicUrl"] = dr["PicUrl"].ToString();
                                    dict["Url"] = dr["Url"].ToString();
                                    dictList.Add(dict);
                                }
                                result = WXMsgUtil.CreateNewsMsg(xmlDoc, dictList);
                            }
                            else
                            {
                                result = WXMsgUtil.CreateTextMsg(xmlDoc, "无此消息哦");
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "text":
                    string text = WXMsgUtil.GetFromXML(xmlDoc, "Content");
                    //if (text == "合肥" || text == "合肥天气" || text == "合肥天气预报"
                    //    || text.ToLower() == "hf" || text.ToLower() == "hefei")
                    //{
                    //    result = WXMsgUtil.CreateNewsMsg(xmlDoc, WeatherUtil.GetWeatherInfo());
                    //}
                    //else
                    {
                        result = WXMsgUtil.CreateNewsMsg(xmlDoc, WeatherUtil.GetWeatherInfo(text));
                        //result = WXMsgUtil.CreateTextMsg(xmlDoc, WXMsgUtil.GetTulingMsg(text));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(sAppID)) //没有AppID则不加密(订阅号没有AppID)
            {
                //加密
                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(sToken, sEncodingAESKey, sAppID);
                string sEncryptMsg = ""; //xml格式的密文
                string timestamp = context.Request["timestamp"];
                string nonce = context.Request["nonce"];
                int ret = wxcpt.EncryptMsg(result, timestamp, nonce, ref sEncryptMsg);
                if (ret != 0)
                {
                    //FileLogger.WriteErrorLog(context, "加密失败，错误码：" + ret);
                    Log.WriteDebug("加密失败，错误码：" + ret);
                    return;
                }

                context.Response.Write(sEncryptMsg);
                context.Response.Flush();
            }
            else
            {
                context.Response.Write(result);
                context.Response.Flush();
            }
        }
        #endregion

    }
}