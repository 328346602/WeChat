<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="SWX.LoginPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="CSS/index.css" rel="stylesheet" type="text/css" />
    <style>
        .top
        {
            height: 90px;
            width: 100%;
            background-color: #007366;
        }
        .topright
        {
            float: right;
            height: 60px;
            padding-top: 20px;
            position: relative;
            right: 20px;
            width: 190px;
        }
        .topright p
        {
            color: #fff;
            display: block;
            float: right;
            font-size: 12px;
            height: 30px;
            line-height: 30px;
            position: relative;
            width: 190px;
        }
        .topright p span
        {
            display: block;
            position: absolute;
            right: 0;
        }
        .pagecenter
        {
            width: 100%;
            height: 100%;
            margin-top: 0px;
        }
        .pageleft
        {
            width: 15%;
            height: 650px;
            float: left; /* background-color: rgb(0, 115, 102); */
            background-color: #ddd;
        }
        .pageright
        {
            width: 85%;
            height: 650px;
            float: right;
        }
        
        .nav
        {
            margin: 0;
            padding: 0;
            border-top: solid 1px #fff;
            background-color: rgb(0, 115, 102);
        }
        .nav li
        {
            list-style-type: none;
            padding-left: 20px;
            padding-top: 10px;
            padding-bottom: 10px;
            border-bottom: solid 1px #fff;
        }
        .nav li:hover
        {
            cursor: pointer;
            background-color: #eeeea0;
        }
        .nav li a
        {
            color: White;
            font-weight: bold;
            text-decoration: none;
        }
        .nav li:hover a
        {
            color: Black !important;
        }
    </style>
    <link href="JS/easyui/easyui.css" rel="stylesheet" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/easyui/jquery.easyui.min.js"></script>
    <script src="JS/SimpoWindow.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //点击菜单
            $(".nav li").click(function () {
                var items = document.getElementsByTagName("a");
                for (var i = 0; i < items.length; i++) {
                    if ($.trim(items[i].innerHTML) == $.trim($(this).find("a").html())) {
                        //items[i].click();
                        $("#main").attr("src", $(items[i]).attr("url"));
                    }
                }

                $(".nav li").find("a").css("color", "#fff");
                $(this).find("a").css("color", "#eeaa66");
            });

            var h = $(window).height() - 100;
            $(".pageleft").height(h);
            $(".pageright").height(h);
        });


        function logout() {
            $.ajax({
                type: "post",
                async: false,
                url: "LoginPage.aspx?action=Logout",
                dataType: 'json',
                success: function (msg) {
                    if (msg == '1') {
                        window.location.href = '/Login.aspx';
                    } else {
                        alert("退出失败！");
                    }
                }
            });
        }
       
    </script>
</head>
<body>
    <div class="top">
        <div style="float: left; width: 440px;">
            <h2>
                <span style="color: White; margin-left: 100px;">辛普微信平台</span></h2>
        </div>
        <div class="topright">
            <div id="divTime" style="float: right; color: #fff;">
                欢迎
                <%=UserName%>
            </div>
            <p>
                <span><a target="main" href="ModifyPassword.aspx" id="I2" onclick="changePage(this)"
                    style="text-decoration: none;">修改密码</a> <a onclick="logout()" href="javascript:void()"
                        style="margin-left: 10px; text-decoration: none;">退出</a> </span>
            </p>
        </div>
    </div>
    <div style="height: 5px; background-color: #e6e6e6; border-bottom: solid 1px #ddd;">
    </div>
    <div class="pagecenter">
        <div class="pageleft">
            <ul class="nav">
                <%if (IsAdmin)
                  { %>
                <li><a href="UserManager.aspx" target="main">用户管理 </a></li>
                <%}
                  else
                  { %>
                <li><a href="javascript:void(0);" url="caidan.aspx" target="main">菜单 </a></li>
                <li><a href="javascript:void(0);" url="MenuMessage.aspx" target="main">菜单消息 </a>
                </li>
                <li><a href="javascript:void(0);" url="ModifyWXMessage.aspx" target="main">修改微信信息 </a>
                </li>
                <li><a href="javascript:void(0);" url="NewGroupPicture2.aspx" target="main">图片库 </a>
                </li>
                <li><a href="javascript:void(0);" url="TextMessageList2.aspx" target="main">图文消息 </a>
                </li>
                <li><a href="javascript:void(0);" url="NewGroupNews.aspx" target="main">群发 </a></li>
                <%} %>
            </ul>
        </div>
        <div class="pageright">
            <iframe id="main" name="main" frameborder="0" align="left" style="width: 100%; height: 100%;
                background-color: White;"></iframe>
        </div>
    </div>
</body>
</html>
