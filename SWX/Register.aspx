<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SWX.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Effect.css" rel="stylesheet" type="text/css" />
    <link href="JS/easyui/easyui.css" rel="stylesheet" />
    <style type="text/css">
        .tabFrm tr td:first-child
        {
            text-align: right;
        }
    </style>
    <script src="JS/jquery.min.js"></script>
    <script src="JS/easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        //注册方法
        function SaveUser() {
            var OrgID = $("#OrgID").val();
            var Token = $("#Token").val();
            var AppID = $("#AppID").val();
            var EncodingAESKey = $("#EncodingAESKey").val();
            var UserName = $("#UserName").val();
            var Password = $("#Password").val();
            var SecondPassword = $("#SecondPassword").val();
            var AppSecret = $("#AppSecret").val();
            if ($.trim(OrgID) == "" || OrgID == null) {
                $('#tip').text('微信公众号原始ID不能为空');
                return;
            }
            if ($.trim(Token) == "" || Token == null) {
                $('#tip').text('Token不能为空');
                return;
            }
            /*if ($.trim(AppID) == "" || AppID == null) {
                $('#tip').text('AppID不能为空');
                return;
            }
            if ($.trim(EncodingAESKey) == "" || EncodingAESKey == null) {
                $('#tip').text('EncodingAESKey不能为空');
                return;
            }*/
            if ($.trim(UserName) == "" || UserName == null) {
                $('#tip').text('用户名称不能为空');
                return;
            }
            if ($.trim(Password) == "" || Password == null) {
                $('#tip').text('用户密码不能为空');
                return;
            }
            if ($.trim(SecondPassword) == "" || SecondPassword == null) {
                $('#tip').text('重复密码不能为空');
                return;
            }
            if (Password != SecondPassword) {
                $('#tip').text('两次输入的密码不一致');
                return;
            }
            if (!checkName($.trim(UserName))) {
                $('#tip').text('用户名已存在，请重新填写！');
                return;
            }

            $.ajax({
                type: "post",
                async: false,
                url: "Register.aspx?action=register",
                data: { OrgID: OrgID, Token: Token, AppID: AppID, EncodingAESKey: EncodingAESKey, UserName: UserName, Password: Password, AppSecret: AppSecret },
                dataType: 'json',
                success: function (msg) {
                    if (msg == "1") {
                        closeAdd();
                        alert("注册成功！");
                    } else {
                        alert("注册失败！");
                    }
                }

            });
        }

        //检查用户名是否存在
        function checkName(name) {
            var result = true;
            $.ajax({
                type: "post",
                async: false,
                url: "Register.aspx?action=checkName",
                data: { name: name },
                dataType: 'json',
                success: function (msg) {
                    if (msg == "1") {
                        result = false;
                    }
                }

            });
            return result;
        }
        //清空数据
        function closeAdd() {
            $("#OrgID").val("");
            $("#Token").val("");
            $("#AppID").val("");
            $("#EncodingAESKey").val("");
            $("#UserName").val("");
            $("#Password").val("");
            $("#SecondPassword").val("");
            $("#AppSecret").val("");
        }

    </script>
</head>
<body style="background-color: #eee;">
    <form id="form1" runat="server">
    <div style="padding: 10px 60px 20px 60px">
        <table class="tabFrm" cellpadding="9" align="center">
            <tr>
                <td>
                    微信公众号原始ID：
                </td>
                <td>
                    <input id="OrgID" class="easyui-validatebox SIMPO_Txt_200" type="text" style="height: 25px;
                        width: 200px;" name="OrgID" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Token：
                </td>
                <td>
                    <input id="Token" class="easyui-validatebox SIMPO_Txt_200" type="text" style="width: 200px;
                        height: 25px;" name="Token" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    AppID：
                </td>
                <td>
                    <input class="easyui-validatebox SIMPO_Txt_200" type="text" id="AppID" style="width: 200px;
                        height: 25px;" name="AppID" />
                </td>
            </tr>
            <tr>
                <td>
                    EncodingAESKey：
                </td>
                <td>
                    <textarea id="EncodingAESKey" name="EncodingAESKey" class="SIMPO_Txt_200" rows="5"
                        cols="30" style="font-size: 12px; width: 400px; height: 50px;"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                    AppSecret：
                </td>
                <td>
                    <textarea id="AppSecret" name="AppSecret" class="SIMPO_Txt_200" rows="5" cols="30"
                        style="font-size: 12px; width: 400px; height: 50px;"></textarea>
                </td>
            </tr>
            <tr>
                <td>
                    用户名称：
                </td>
                <td>
                    <input id="UserName" class="easyui-validatebox SIMPO_Txt_200" type="text" name="UserName"
                        style="width: 200px; height: 25px;" />
                </td>
            </tr>
            <tr>
                <td>
                    用户密码：
                </td>
                <td>
                    <input id="Password" class="easyui-validatebox SIMPO_Txt_200" type="password" name="Password"
                        style="width: 200px; height: 25px;" />
                </td>
            </tr>
            <tr>
                <td>
                    重复密码：
                </td>
                <td>
                    <input class="easyui-validatebox SIMPO_Txt_200" type="password" id="SecondPassword"
                        style="width: 200px; height: 25px;" name="SecondPassword" />
                </td>
            </tr>
            <tr>
                <td height="18px" align="left" valign="top" scope="col" colspan="2" style="text-align: center;">
                    <i id="tip" style="font-style: normal; color: #FF0000; font-size: 15px;"></i>
                </td>
            </tr>
        </table>
        <div style="text-align: center; padding: 10px">
            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                onclick="SaveUser();">注册</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                    data-options="iconCls:'icon-cancel'" onclick="closeAdd();">取消</a>
        </div>
    </div>
    </form>
</body>
</html>
