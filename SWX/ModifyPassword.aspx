<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyPassword.aspx.cs"
    Inherits="SWX.ModifyPassword" %>

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
        function modifyPassword() {
            var oldpwd = $("#oldpwd").val();
            var newpwd = $("#newpwd").val();
            var rptpwd = $("#rptpwd").val();
            if (newpwd != rptpwd) {
                $('#tip').text('新密码和重复新密码不一致！');
                return;
            }
            $.ajax({
                type: "post",
                async: false,
                url: "ModifyPassword.aspx?action=modifyPwd",
                data: { oldpwd: oldpwd, newpwd: newpwd },
                dataType: 'json',
                success: function (msg) {
                    if (msg == "1") {
                        alert("修改密码成功！");
                        $("#oldpwd").val("");
                        $("#newpwd").val("");
                        $("#rptpwd").val("");
                    }
                    if (msg == "2") {
                        $('#tip').text('当前密码不正确！');
                        return;
                    }

                }
            });
        }
    </script>
</head>
<body style="background-color: White;">
    <form id="form1" runat="server">
    <div class="panel easyui-fluid">
        <div class="panel-header" style="width: 800px;">
            <div style="color: #0e2d5f; font-size: 12px; font-weight: bold; height: 16px; line-height: 16px;">
                修改密码</div>
            <div style="right: 5px; width: auto;">
            </div>
        </div>
        <div class="easyui-panel panel-body" style="padding: 20px; width: 812px;">
            <table class="tabFrm" cellpadding="0" cellspacing="0" align="center" style="width: 400px;">
                <tbody>
                    <tr>
                        <td class="tdTitle" style="width: 100px">
                            当前密码
                        </td>
                        <td>
                            <input type="password" id="oldpwd" class="SIMPO_Txt_200" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitle">
                            新密码
                        </td>
                        <td>
                            <input type="password" class="SIMPO_Txt_200" id="newpwd" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdTitle">
                            重复新密码
                        </td>
                        <td>
                            <input type="password" class="SIMPO_Txt_200" id="rptpwd" />
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
            <div style="text-align: center;">
                <a id="" class="easyui-linkbutton l-btn l-btn-small" onclick="modifyPassword()" href="javascript:void(0)"
                    group="">
                    <%--<span class="l-btn-left l-btn-icon-left">--%>
                    <span class="l-btn-text">修改密码</span>
                    <%-- <span class="l-btn-icon icon-save"> </span>--%>
                    <%--</span>--%>
                </a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
