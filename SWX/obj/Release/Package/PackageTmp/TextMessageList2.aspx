<%@ Page Title="图文消息列表" Language="C#" AutoEventWireup="true" CodeBehind="TextMessageList2.aspx.cs"
    Inherits="SWX.TextMessageList2" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/message.css" rel="stylesheet" type="text/css" />
    <script src="JS/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function add() {
            window.location = "NewTextMessage.aspx";
        }
        function edit(id) {
            window.location = "NewTextMessage.aspx?action=Load&Id=" + id;
        }
        function deleteText(id) {
            if (!confirm("确定删除吗？")) {
                return;
            }

            $.ajax({
                type: "post",
                async: false,
                url: "TextMessageList2.aspx?action=deleteText",
                data: { Id: id },
                success: function (msg) {
                    if (msg == 1) {
                        alert("删除成功");
                        $("#btnSearch").click();
                    }
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="button" class="SIMPO_Text_Blue" value="新建" onclick="add()" style="margin-left: 50px;" />
    <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="OnSearchButtonClick"
        Style="display: none;" />
    <asp:ScriptManager runat="server" ID="ScriptManager1">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="zpq" id="see" runat="server">
                <ul>
                    <asp:Repeater ID="repeaterList1" runat="server">
                        <ItemTemplate>
                            <li style="float: left; border: 1px solid #E7E7EB; margin-left: 10px;">
                                <div class="appmsg_info">
                                    <em class="appmsg_date">
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
                                    <div class="appmsg_opr">
                                        <ul>
                                            <li class="appmsg_opr_item">
                                                <input type="button" class="SIMPO_Text_Blue" value="编辑" onclick='edit(<%#Eval("Id")%>)' /></li>
                                            <li class="appmsg_opr_item no_extra">
                                                <input type="button" class="SIMPO_Text_Gray" value="删除" onclick='deleteText(<%#Eval("Id")%>)' /></li>
                                        </ul>
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
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
