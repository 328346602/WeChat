<%@ Page Title="新建群发消息" Language="C#" AutoEventWireup="true" CodeBehind="NewGroupNews.aspx.cs"
    Inherits="SWX.NewGroupNews" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/picture.css" rel="stylesheet" type="text/css" />
    <link href="CSS/message.css" rel="stylesheet" type="text/css" />
    <link href="JS/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="JS/easyui/easyui.css" rel="stylesheet" type="text/css" />
    <script src="JS/jquery.min.js" type="text/javascript"></script>
    <script src="JS/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="JS/uploadify/swfobject.js" type="text/javascript"></script>
    <script src="JS/uploadify/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>
    <script src="JS/json2.js" type="text/javascript"></script>
    <script src="JS/SimpoWindow.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Countent").html($("#WordsCountent").html());
            $('#Words').on('click', function () {
                $('.tab div').removeClass();
                $('#Wordsdiv').addClass("wdxxs");
                $('#Picturediv').addClass("hyxxs");
                $('#TextMessagediv').addClass("hyxxs");

                $("#Countent").html($("#WordsCountent").html());
            });
            $('#Picture').on('click', function () {
                $('.tab div').removeClass();
                $('#Wordsdiv').addClass("hyxxs");
                $('#Picturediv').addClass("wdxxs");
                $('#TextMessagediv').addClass("hyxxs");

                SimpoWin.showWin("选择素材", "NewGroupPicture.aspx", 880, 580);
                //                $('#p').panel('refresh', 'NewGroupPicture.aspx');
                //                $('#MessageAdd').window('open');
            });
            $('#TextMessage').on('click', function () {
                $('.tab div').removeClass();
                $('#Wordsdiv').addClass("hyxxs");
                $('#Picturediv').addClass("hyxxs");
                $('#TextMessagediv').addClass("wdxxs");

                SimpoWin.showWin("选择素材", "TextMessageList.aspx", 880, 580);
                //$('#p').panel('refresh', 'TextMessageList.aspx');
                //$('#MessageAdd').window('open');
            });


        });

        //加载选中图片到页面
        function saveAdd(imgurl) {
            $("#PictureCountent").html("<img width='144' height='126' src='" + imgurl + "' alt='' />")
            $("#Countent").html($("#PictureCountent").html());
        }

        //加载选中的图文消息
        function saveText(Id) {
            $.ajax({
                type: "post",
                async: false,
                url: "NewGroupNews.aspx?action=findText",
                data: { Id: Id },
                success: function (data) {
                    var jsonList = $.parseJSON(data);
                    var html = "<div class='appmsg_info'><em class='appmsg_date'>" + jsonList.CreateTime + "</em></div>";
                    html += "<div class='appmsg multi'><div class='appmsg_content'>";
                    var jsonTextList = $.parseJSON(jsonList.List);
                    for (var i = 0; i < jsonTextList.length; i++) {
                        html += "<div class='appmsg_item'>";
                        html += "<img class='appmsg_thumb' alt='' src=" + jsonTextList[i].ImgUrl + ">";
                        html += "<h4 class='appmsg_title'>";
                        html += "<a target='_blank' href='javascript:;'>" + jsonTextList[i].Title + "</a></h4></div>";
                    }
                    html += "</div></div>";
                    //图文隐藏Id
                    html += "<input type='hidden' id='hiddenTextId'  value='" + Id + "' />";

                    $("#MessageContent").html(html);
                    $("#Countent").html($("#MessageContent").html());
                }
            });
        }

        //群发
        function send(obj) {
            if ($.trim($("#Countent").html()) == "") {
                alert("发送内容不能为空");
                return;
            }

            var sendType = $("#sendType").val();
            var data;
            if (sendType == "1") {
                data = $("#Countent").text();
            }
            if (sendType == "2") {
                if ($("#Countent").find("img")) {
                    data = $("#Countent").find("img").attr("src");
                }
            }
            if (sendType == "3") {
                if ($("#hiddenTextId")) {
                    data = $("#hiddenTextId").val();
                }
            }

            if (!data || $.trim(data) == "") {
                alert("发送内容不能为空");
                return;
            }

            $(obj).removeClass("SIMPO_Text_Blue");
            $(obj).addClass("SIMPO_Text_Gray");
            $(obj).attr("disabled", "disabled");
            $(obj).val("发送中...");

            setTimeout(function () {
                //执行群发
                $.ajax({
                    type: "post",
                    async: false,
                    url: "NewGroupNews.aspx?action=send",
                    data: { type: sendType, data: data },
                    dataType: 'json',
                    success: function (data) {
                        if (data.code == 1) {
                            $("#Countent").html("");
                            $(obj).removeAttr("disabled");
                            $(obj).removeClass("SIMPO_Text_Gray");
                            $(obj).addClass("SIMPO_Text_Blue");
                            $(obj).val("发送");
                            alert("发送成功！");
                        } else {
                            $(obj).removeAttr("disabled");
                            $(obj).removeClass("SIMPO_Text_Gray");
                            $(obj).addClass("SIMPO_Text_Blue");
                            $(obj).val("发送");
                            alert("发送失败！错误信息：" + data.msg);
                        }
                    },
                    error: function () {
                        $(obj).removeAttr("disabled");
                        $(obj).removeClass("SIMPO_Text_Gray");
                        $(obj).addClass("SIMPO_Text_Blue");
                        $(obj).val("发送");
                    }
                });
            }, 10);
        }

        //设置发送类型
        function setSendType(type) {
            $("#sendType").val(type);
        }
    </script>
</head>
<body>
    <input type="hidden" id="sendType" value="1" />
    <div class="tab">
        <div id="Wordsdiv" class="wdxxs">
            <a id="Words" href="javascript:;" onclick="setSendType(1)">文字</a>
        </div>
        <div id="Picturediv" class="hyxxs">
            <a id="Picture" href="javascript:;" onclick="setSendType(2)">图片</a>
        </div>
        <div id="TextMessagediv" class="hyxxs">
            <a id="TextMessage" href="javascript:;" onclick="setSendType(3)">图文消息</a>
        </div>
    </div>
    <div id="Countent" style="border: 1px solid #E4E4E4; min-height: 216px">
    </div>
    <input type="button" value="发送" onclick="send(this)" class="SIMPO_Text_Blue" />
    <div id="WordsCountent" style="display: none">
        <div class="edit_area js_editorArea" contenteditable="true" style="overflow-x: hidden;
            overflow-y: auto;">
        </div>
    </div>
    <div id="PictureCountent" style="display: none">
    </div>
    <div id="MessageContent" style="display: none">
    </div>
    <%-- <div id="MessageAdd" class="easyui-window" title="选择素材" data-options="modal:true,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true,iconCls:'icon-add'"
        style="padding: 10px; width: 880px; height: 580px">
        <div id="p" class="easyui-panel" style="padding: 10px; height: 530px; width: 840px;"
            data-options="title:'',iconCls:'icon-save',
						collapsible:true,minimizable:true,maximizable:true,closable:true">
        </div>
    </div>--%>
</body>
</html>
