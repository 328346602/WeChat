<%@ Page Title="新建图文消息" Language="C#" AutoEventWireup="true" CodeBehind="NewTextMessage.aspx.cs"
    Inherits="SWX.NewTextMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        #uploadifyUploader
        {
            background: url('/JS/uploadify/upload.png') no-repeat;
        }
    </style>
    <title></title>
    <link rel="stylesheet" type="text/css" href="CSS/Ctrl.css" />
    <link href="CSS/Effect.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Form.css" rel="stylesheet" type="text/css" />
    <link href="CSS/message.css" rel="stylesheet" type="text/css" />
    <link href="JS/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="JS/jquery.min.js" type="text/javascript"></script>
    <script src="JS/json2.js" type="text/javascript"></script>
    <script src="JS/uploadify/swfobject.js" type="text/javascript"></script>
    <script src="JS/uploadify/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#addmsgItem').on('click', function () {
                var sortid = $("#sortid").val();
                var nowid = parseInt(sortid) + 1;
                var html = "<div id='appmsgItem" + nowid + "'>";

                html += "<div class='appmsg_item'><img class='appmsg_thumb' alt='' src='JS/easyui/images/svt.jpg'>";
                html += "<h4 class='appmsg_title'><a target='_blank' href='javascript:;'>标题</a></h4></div>";
                //html += "<img src='' /><label>标题</label>";
                html += "<input type='button' class='SIMPO_Text_Blue' id='edit" + nowid + "' value='编辑'  onclick='edit(" + nowid + ")' />";
                html += "&nbsp;";
                html += "<input type='button' class='SIMPO_Text_Gray' id='delete" + nowid + "' value='删除' onclick='deleteImg(" + nowid + ")' />";
                html += "</div>";
                html += "<input type='hidden' id='hiddencontent" + nowid + "' name='hiddencontent' />";
                html += "<input type='hidden' id='savecontent" + nowid + "' name='savecontent' value='|' />";

                $("#list").append(html);

                $("#sortid").val(nowid);
            });

            // 图片上传插件配置
            $("#uploadify").uploadify({
                'uploader': '/JS/uploadify/uploadify.swf',
                'script': '/UploadHandler.ashx',
                //'buttonImg': '/JS/uploadify/upload.png',
                'wmode': 'transparent',
                'hideButton': true,
                'buttonText': '  ',
                'height': 30,
                'width': 113,
                'cancelImg': '/JS/uploadify/uploadify-cancel.png',
                'folder': '/UploadFile',
                'queueID': 'fileQueue',
                'auto': false,
                'multi': false,
                'removeCompleted': true, //上传成功后的文件，是否在队列中自动删除
                'fileDesc': '支持jpg,png,bmp格式',
                'fileExt': '*.jpg;*.png;*.bmp;',
                'sizeLimit': 2097125, // 单位byte，限制大小 2 MB

                'onSelect': function (e, queueId, fileObj) {
                    if (fileObj.size > 2097125) {
                        alert("上传文件大小不允许超出 2MB！请重新选择");
                        return false;
                    }

                    $('#uploadify').uploadifyUpload();
                },

                // 文件上传完成后触发（每个文件触发一次）
                'onComplete': function (e, queueID, fileObj, response, data) {
                    saveData(response);
                },
                // 当队列中的所有文件全部完成上传时触发
                'onAllComplete': function (e, data) {

                }
            });

            var loadtext = $("#loadtext").val();
            if (loadtext != "" && loadtext != null) {
                var jsonList = $.parseJSON(loadtext);
                $("#sortid").val(jsonList.length);
                var html = "";
                for (var i = 0; i < jsonList.length; i++) {
                    html += "<div id='appmsgItem" + i + "'>";
                    html += "<div class='appmsg_item'><img class='appmsg_thumb' alt='' src='" + jsonList[i].ImgUrl + "'>";
                    html += "<h4 class='appmsg_title'><a target='_blank' href='javascript:;'>" + jsonList[i].Title + "</a></h4></div>";
                    //html += "<img src='" + jsonList[i].ImgUrl + "' /><label>" + jsonList[i].Title + "</label>";
                    html += "<input type='button' class='SIMPO_Text_Blue' id='edit" + i + "' value='编辑'  onclick='edit(" + i + ")' />";
                    html += "&nbsp;";
                    if (i != 0) {
                        html += "<input type='button'  class='SIMPO_Text_Gray' id='delete" + i + "' value='删除' onclick='deleteImg(" + i + ")' />";
                    }

                    var arr = [{ "Name": "Title", "Value": jsonList[i].Title }, { "Name": "Author", "Value": jsonList[i].Author }, { "Name": "Content", "Value": jsonList[i].Content }, { "Name": "TextUrl", "Value": jsonList[i].TextUrl }, { "Name": "ImgUrl", "Value": jsonList[i].ImgUrl}];
                    var savearr = [{ Id: jsonList[i].Id, Title: jsonList[i].Title, Author: jsonList[i].Author, Content: jsonList[i].Content, TextUrl: jsonList[i].TextUrl, ImgUrl: jsonList[i].ImgUrl}];
                    html += "<input type='hidden' id='hiddencontent" + i + "' name='hiddencontent' value='" + JSON.stringify(arr) + "' />";
                    html += "<input type='hidden' id='savecontent" + i + "' name='savecontent' value='" + JSON.stringify(savearr) + "|" + "' />";
                    html += "<input type='hidden' id='hiddenid" + i + "'  value='" + jsonList[i].Id + "' />";
                    html += "</div>";
                    if (i == 0) {
                        $("#editid").val("0");
                        $("#Title").val(jsonList[i].Title);
                        $("#Author").val(jsonList[i].Author);
                        $("#Content").val(jsonList[i].Content);
                        $("#TextUrl").val(jsonList[i].TextUrl);
                        $("#imgsrc").attr("src", jsonList[i].ImgUrl);
                    }
                }
                $("#list").html(html);
            }
        });

        function saveData(response) {
            var editid = $("#editid").val();

            $.ajax({
                type: "post",
                async: false,
                url: "NewTextMessage.aspx?action=addPicture",
                data: { response: response },
                success: function (msg) {
                    $("#appmsgItem" + editid + " img").attr("src", msg);
                    $("#imgsrc").attr("src", msg);
                    hiddenImg();
                    Serialize();
                }
            });
        }

        //编辑按钮
        function edit(id) {
            var editid = $("#editid").val();
            clearInner();

            $("#editid").val(id);
            var editcontent = $("#hiddencontent" + id).val();

            var arr = $.parseJSON(editcontent);

            for (var key in arr) {
                if (arr[key].Name != null) {
                    if (arr[key].Name == "ImgUrl") {
                        $("#imgsrc").attr("src", arr[key].Value);
                        hiddenImg();
                    } else {
                        $("#" + arr[key].Name).val(arr[key].Value);
                    }
                }
            }

            if (arr == null || arr == "") {
                $("#imgsrc").attr("src", "");
                hiddenImg();
            }
        }

        //删除按钮
        function deleteImg(id) {
            var hiddenid = $("#hiddenid" + id).val();
            if (hiddenid == "" || hiddenid == null || hiddenid == "undefined") {
                $("#appmsgItem" + id).remove();
            } else {
                $.ajax({
                    type: "post",
                    async: false,
                    url: "NewTextMessage.aspx?action=deleteText&deleteid=" + hiddenid,
                    success: function (msg) {
                        if (msg == "1") {
                            $("#appmsgItem" + id).remove();
                        } else {
                            alert("删除出错");
                        }
                    }
                });
            }

            var editid = $("#editid").val();
            if (editid == id) {
                clearInner();
            }
        }

        //动态修改标题
        function Title() {
            var editid = $("#editid").val();
            var title = $("#Title").val();
            $("#appmsgItem" + editid + " a").html(title);

            Serialize();
        }

        //序列化数组
        function Serialize() {
            var editid = $("#editid").val();

            var Title = $("#Title").val();
            var Author = $("#Author").val();
            var Content = $("#Content").val();
            var TextUrl = $("#TextUrl").val();
            var ImgUrl = $("#appmsgItem" + editid + " img").attr("src");

            var arr = [{ "Name": "Title", "Value": Title }, { "Name": "Author", "Value": Author }, { "Name": "Content", "Value": Content }, { "Name": "TextUrl", "Value": TextUrl }, { "Name": "ImgUrl", "Value": ImgUrl}];

            $("#hiddencontent" + editid).val(JSON.stringify(arr));

            var Id = $("#hiddenid" + editid).val();
            var savearr = [{ Id: Id, Title: Title, Author: Author, Content: Content, TextUrl: TextUrl, ImgUrl: ImgUrl}];
            $("#savecontent" + editid).val(JSON.stringify(savearr) + "|");
        }

        //清空输入框
        function clearInner() {
            $(".clearInner").val("");
        }

        function hiddenImg() {
            var editid = $("#editid").val();
            var imgsrc = $("#appmsgItem" + editid + " img").attr("src");
            if (imgsrc == "") {
                $("#imgsrc").hide();
            } else {
                $("#imgsrc").show();
            }
        }

        function save() {
            var hiddenid = $("#hiddenid").val();

            $.ajax({
                type: "post",
                async: false,
                url: "NewTextMessage.aspx?action=saveText&editid=" + hiddenid,
                data: $("#form1").serialize(),
                success: function (msg) {
                    if (msg == "1") {
                        alert("保存成功");
                        window.location = "TextMessageList2.aspx";
                    }
                }
            });
        }

        function returnF() {
            window.location = "TextMessageList2.aspx";
        }
    </script>
</head>
<body style="padding-left: 50px; width: 1000px">
    <form id="form1" runat="server">
    <input type="hidden" id="loadtext" runat="server" />
    <input type="hidden" id="hiddenid" runat="server" />
    <div style="float: left;">
        <div id="list">
            <div id="appmsgItem1">
                <div class="appmsg_item">
                    <img class="appmsg_thumb" alt="" src="JS/easyui/images/svt.jpg">
                    <h4 class="appmsg_title">
                        <a target="_blank" href="javascript:;">标题</a>
                    </h4>
                </div>
                <input type="button" id="edit1" class="SIMPO_Text_Blue" value="编辑" onclick="edit(1)" />
                <input type="hidden" id="hiddencontent1" name="hiddencontent" />
                <input type="hidden" id="savecontent1" name="savecontent" value="|" />
            </div>
        </div>
        <input type="hidden" value="1" id="sortid" />
        <input type="hidden" value="1" id="editid" />
        <a id="js_add_appmsg" class="create_access_primary appmsg_add" href="javascript:void(0);"
            onclick="return false;">
            <input id="addmsgItem" class="SIMPO_Text_Blue" type="button" value="添加一条" />
        </a>
    </div>
    </form>
    <div id="inner" class="inner" style="float: left">
        <table class="tabFrm" cellpadding="0" cellspacing="0">
            <tr>
                <td class="tdTitle" style="width: 80px;">
                    标题：
                </td>
                <td>
                    <input type="text" id="Title" name="Title" onchange="Title()" class="clearInner SIMPO_Txt_200"
                        style="width: 500px;" />
                </td>
            </tr>
            <tr>
                <td class="tdTitle">
                    作者：
                </td>
                <td>
                    <input type="text" id="Author" name="Author" onchange="Serialize()" class="clearInner SIMPO_Txt_200"
                        style="width: 500px;" />
                </td>
            </tr>
            <tr>
                <td class="tdTitle">
                    封面：
                </td>
                <td>
                    <input type="file" name="uploadify" id="uploadify" class="uploadbutton" />
                    <span style="font-size: 12px; color: Red">说明：单个文件大小限制在 2MB 以内</span>
                </td>
            </tr>
            <tr>
                <td class="tdTitle">
                    图片预览：
                </td>
                <td>
                    <img id="imgsrc" src="" onerror="hiddenImg()" width="80" height="80" />
                </td>
            </tr>
            <tr>
                <td class="tdTitle">
                    正文：
                </td>
                <td>
                    <textarea id="Content" name="Content" onchange="Serialize()" rows="2" cols="40" class="SIMPO_Txt_200 clearInner"
                        style="width: 500px; height: 100px;"></textarea>
                </td>
            </tr>
            <tr>
                <td class="tdTitle">
                    原文连接：
                </td>
                <td>
                    <input type="text" id="TextUrl" name="TextUrl" onchange="Serialize()" class="clearInner SIMPO_Txt_200"
                        style="width: 500px;" />
                </td>
            </tr>
        </table>
    </div>
    <div style="clear: both">
    </div>
    <div style="text-align: center; height: 44px; line-height: 44px">
        <input id="save" type="button" class="SIMPO_Text_Blue" value="保存" onclick="save()" />
        <input id="return" type="button" class="SIMPO_Text_Gray" value="返回" onclick="returnF()" />
    </div>
</body>
</html>
