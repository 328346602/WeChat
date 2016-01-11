<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TextMessageList.aspx.cs"
    Inherits="SWX.TextMessageList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/message.css" rel="stylesheet" type="text/css" />
    <script src="JS/jquery.min.js" type="text/javascript"></script>
    <script src="JS/SimpoWindow.js" type="text/javascript"></script>
    <script type="text/javascript">
        //加载选中图片到页面
        function saveText() {
            var page = SimpoWin.GetWinParent();
            var Id = $('input:radio[name="check"]:checked').val();
            page.saveText(Id);

            SimpoWin.closeWin();
        }

        function closeAdd() {
            SimpoWin.closeWin();
        }
    </script>
</head>
<body>
    <form id="formtext" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="zpq" id="see" runat="server">
                <ul>
                    <asp:Repeater ID="repeaterList1" runat="server">
                        <ItemTemplate>
                            <li style="float: left; border: 1px solid #666666; margin-left: 10px; margin-bottom: 10px">
                                <div class="appmsg_info">
                                    <em class="appmsg_date">
                                        <input id="radio" name="check" type="radio" value="<%#Eval("Id") %>" style="margin-left: 5px">
                                        <%#Eval("CreateTime", "{0:yyyy-MM-dd HH:mm}")%></em>
                                </div>
                                <div class="appmsg multi">
                                    <div class="appmsg_content">
                                        <asp:Repeater ID="rptList" DataSource='<%#BindData(DataBinder.Eval(Container.DataItem, "Id").ToString())%>'
                                            runat="server">
                                            <ItemTemplate>
                                                <div class="appmsg_item">
                                                    <img class="appmsg_thumb" alt="" src="<%#Eval("ImgUrl") %>">
                                                    <h4 class="appmsg_title">
                                                        <a target="_blank" href="javascript:;">
                                                            <%#Eval("Title")%></a>
                                                    </h4>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div style="clear: both">
            </div>
            <div class="divPager" id="page1" runat="server">
                <webdiyer:AspNetPager ID="AspNetPager" runat="server" CssClass="anpager" centercurrentpagebutton="True"
                    NextPageText="下一页" PrevPageText="上一页" ShowPageIndexBox="Never" CurrentPageButtonStyle="padding:4px 8px;background: #0088cc none repeat scroll 0 0;border: 1px solid #2b55af;color: #FFFFFF;"
                    NumericButtonCount="5" AlwaysShow="True" CurrentPageButtonPosition="Center" ShowFirstLast="false"
                    OnPageChanging="AspNetPager_PageChanging">
                </webdiyer:AspNetPager>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="AspNetPager" EventName="PageChanging" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
    <div style="text-align: center; padding: 5px; height: 44px; line-height: 44px">
        <a href="javascript:void(0)" class="SIMPO_Text_Blue" onclick="saveText();">选择</a>
        <a href="javascript:void(0)" class="SIMPO_Text_Gray" onclick="closeAdd();">取消</a>
    </div>
</body>
</html>
