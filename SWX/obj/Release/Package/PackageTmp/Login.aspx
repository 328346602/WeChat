<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SWX.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Effect.css" rel="stylesheet" type="text/css" />
    <title></title>
    <style>
        .logincenter
        {
            height: 333px;
            width: 100%;
            margin-top: 13%;
        }
        .loginauto
        {
            width: 900px;
            height: 333px;
            margin: 0 auto;
        }
        .loginleft
        {
            width: 510px;
            float: left;
            height: 333px;
        }
        .loginleft h2
        {
            line-height: 100px;
            font-size: 30px;
            font-family: "黑体"; /*color: #207162;*/
            color: White;
            font-weight: lighter;
        }
        .loginright
        {
            width: 390px;
            float: right;
            height: 333px;
            position: relative;
        }
        .loginbox
        {
            width: 349px;
            height: 348px;
            position: absolute;
            top: -8px;
        }
        .loginbox h2
        {
            display: block;
            width: 349px;
            height: 50px;
            font-size: 20px;
            color: #207162;
            text-align: center;
            line-height: 50px;
            font-family: "微软雅黑" , "黑体";
            font-weight: lighter;
            letter-spacing: 5px;
        }
        .loginbox table
        {
            margin-top: 20px;
        }
        .loginbox table td i
        {
            font-style: normal;
            color: #FF0000;
            padding-left: 50px;
            font-size: 12px;
        }
        .loginbox table td
        {
            font-style: normal;
            color: #207162;
            text-align: center;
        }
    </style>
    <link href="JS/easyui/easyui.css" rel="stylesheet" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        function login() {
            $('#tip').text('正在登录，请稍候...');
            try {
                var username = $('#username').val();
                var password = $('#Password').val()
                if ($.trim(username) == "" || username == "") {
                    $('#tip').text('请填写用户名');
                    return;
                }
                if ($.trim(password) == "" || password == "") {
                    $('#tip').text('请填写密码');
                    return;
                }
                $.ajax({
                    type: "post",
                    async: false,
                    url: "Login.aspx?action=Login",
                    data: { username: username, password: password },
                    dataType: 'json',
                    success: function (msg) {
                        if (msg.code == 1) {
                            window.location.href = '/LoginPage.aspx';
                        } else {
                            $('#tip').text(msg.msg);
                        }
                    }
                });
            }
            catch (ex) {
                $('#tip').text('系统正忙，请稍候登录');
            }

        }

        function regist() {
            window.location = "Register.aspx";
        }
    </script>
</head>
<body style="background-color: rgb(0, 115, 102);">
    <div class="logincenter">
        <div class="loginauto">
            <div class="loginleft" style="color: White;">
                <h2>
                    微信公众平台</h2>
            </div>
            <div class="loginright">
                <div class="loginbox" style="border: solid 1px #ddd; background-color: #eee;">
                    <h2>
                        用户登录</h2>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="text-align: right; width: 100px; padding-right: 20px;">
                                用户名：
                            </td>
                            <td height="55" valign="middle" scope="col" style="text-align: left;">
                                <input type="text" name="username" id="username" class="SIMPO_Txt_150" />
                            </td>
                        </tr>
                        <tr>
                            <td height="20" valign="top" scope="col" colspan="2">
                                <i style="display: none">提示信息！</i>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; padding-right: 20px;">
                                密码：
                            </td>
                            <td height="55" valign="middle" style="text-align: left;">
                                <input type="password" name="Password" id="Password" class="SIMPO_Txt_150" />
                            </td>
                        </tr>
                        <tr>
                            <td height="20" align="left" valign="top" scope="col" colspan="2">
                                <i id="tip" style=""></i>
                            </td>
                        </tr>
                        <tr>
                            <td height="65" align="center" valign="middle" colspan="2">
                                <input name="btnLogin" id="btnLogin" class="SIMPO_Text_Blue" type="button" value="立刻登录"
                                    onclick="login()" style="margin-left: 20px; height: 30px; width: 100px;" />
                                <input name="btnLogin" id="Button1" class="SIMPO_Text_Blue" type="button" value="注 册"
                                    onclick="regist()" style="margin-left: 20px; height: 30px; width: 100px;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
