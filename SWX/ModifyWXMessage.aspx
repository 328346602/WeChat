<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyWXMessage.aspx.cs"
    Inherits="SWX.ModifyWXMessage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="JS/easyui/easyui.css" rel="stylesheet" />
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Effect.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Form.css" rel="stylesheet" type="text/css" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                type: "post",
                async: false,
                url: "ModifyWXMessage.aspx?action=Load",
                dataType: 'json',
                success: function (msg) {
                    $("#OrgID").val(msg[0].OrgID);
                    $("#Token").val(msg[0].Token);
                    $("#AppID").val(msg[0].AppID);
                    $('#EncodingAESKey').val(msg[0].EncodingAESKey);
                    $('#AppSecret').val(msg[0].AppSecret);
                }
            });
        });

        function modifyMessage() {
            var OrgID = $("#OrgID").val();
            var Token = $("#Token").val();
            var AppID = $("#AppID").val();
            var EncodingAESKey = $("#EncodingAESKey").val();
            var AppSecret = $("#AppSecret").val();
            if ($.trim(OrgID) == "" || OrgID == null) {
                $('#tip').text('微信公众号原始ID不能为空！');
                return;
            }
            if ($.trim(Token) == "" || Token == null) {
                $('#tip').text('Token不能为空！');
                return;
            }
            /*if ($.trim(AppID) == "" || AppID == null) {
                $('#tip').text('AppID不能为空！');
                return;
            }
            if ($.trim(EncodingAESKey) == "" || EncodingAESKey == null) {
                $('#tip').text('EncodingAESKey不能为空！');
                return;
            }*/
            $.ajax({
                type: "post",
                async: false,
                url: "ModifyWXMessage.aspx?action=update",
                dataType: 'json',
                data: { OrgID: OrgID, Token: Token, AppID: AppID, EncodingAESKey: EncodingAESKey, AppSecret: AppSecret },
                success: function (msg) {
                    if (msg == "1") {
                        $('#tip').text('');
                        alert("修改信息成功！");
                    } else {
                        alert("修改信息失败！");
                    }
                }
            });
        }

    </script>
</head>
<body style="background-color: White;">
    <form id="form1" runat="server" style="">
    <div class="panel easyui-fluid" style="">
        <div class="panel-header" style="width: 800px;">
            <div style="color: #0e2d5f; font-size: 12px; font-weight: bold; height: 16px; line-height: 16px;">
                修改微信账号信息页面</div>
            <div style="right: 5px; width: auto;">
            </div>
        </div>
        <div class="easyui-panel panel-body" style="padding: 20px; width: 812px; padding-left: 50px;">
            <table class="tabFrm" cellpadding="0" cellspacing="0">
                <tbody>
                    <tr>
                        <td class="tdTitle" style="width: 100px">
                            微信公众号原始ID
                        </td>
                        <td>
                            <input id="OrgID" class="easyui-validatebox SIMPO_Txt_200" type="text" name="OrgID"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitle">
                            Token
                        </td>
                        <td>
                            <input id="Token" class="easyui-validatebox SIMPO_Txt_200" type="text" name="Token"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitle">
                            AppID
                        </td>
                        <td>
                            <input class="easyui-validatebox SIMPO_Txt_200" type="text" id="AppID" name="AppID" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitle">
                            EncodingAESKey
                        </td>
                        <td>
                            <textarea id="EncodingAESKey" name="EncodingAESKey" rows="3" cols="30" class="SIMPO_Txt_200"
                                style="height: 30px; width: 450px;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitle">
                            AppSecret
                        </td>
                        <td>
                            <textarea id="AppSecret" name="AppSecret" rows="3" cols="30" class="SIMPO_Txt_200"
                                style="height: 30px; width: 450px;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <i id="tip" style="font-style: normal; color: #FF0000; padding-left: 50px; font-size: 12px;">
                            </i>
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
            <div style="text-align: center">
                <a id="" class="easyui-linkbutton l-btn l-btn-small" onclick="modifyMessage()" href="javascript:void(0)">
                    <%--<span class="l-btn-left l-btn-icon-left">--%>
                    <span class="l-btn-text">修改微信信息</span>
                    <%-- <span class="l-btn-icon icon-save"> </span>--%>
                    <%--</span>--%>
                </a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
