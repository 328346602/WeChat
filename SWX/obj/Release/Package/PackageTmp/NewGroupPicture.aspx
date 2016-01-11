<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewGroupPicture.aspx.cs"
    Inherits="SWX.NewGroupPicture" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/picture.css" rel="stylesheet" type="text/css" />
    <link href="JS/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="JS/easyui/easyui.css" rel="stylesheet" type="text/css" />
    <script src="JS/jquery.min.js" type="text/javascript"></script>
    <script src="JS/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="JS/uploadify/swfobject.js" type="text/javascript"></script>
    <script src="JS/uploadify/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>
    <script src="JS/SimpoWindow.js" type="text/javascript"></script>
</head>
<body>
    <style>
        #uploadifyUploader
        {
            background: url('/JS/uploadify/upload.png') no-repeat;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
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
        });

        function saveData(response) {
            $.ajax({
                type: "post",
                async: false,
                url: "NewGroupPicture.aspx?action=addPicture",
                data: { response: response },
                success: function (msg) {
                    if (msg == "1") {
                        alert('上传成功！');
                        $("#btnSearch").click();
                    }
                }
            });
        }

        //加载选中图片到页面
        function saveAdd() {
            var imgurl = $('input:radio[name="check"]:checked').val();
            var page = SimpoWin.GetWinParent();
            page.saveAdd(imgurl);

            SimpoWin.closeWin();
        }

        function closeAdd() {
            SimpoWin.closeWin();
        }
    </script>
    <div style="padding: 10px 60px 20px 60px">
        <ul id="ul">
            <li style="float: left; width: 120px;">
                <input type="file" name="uploadify" id="uploadify" class="uploadbutton" /></li>
            <li style="float: left; width: 380px;"><span style="line-height: 30px; color: red;">
                说明：单个文件大小限制在 2MB 以内</span></li>
        </ul>
        <div id="fileQueue" style="width: 600px; overflow-y: auto; height: 400px; padding-bottom: 10px;
            clear: both; text-align: left; display: none">
        </div>
    </div>
    <form id="plist" runat="server">
    <div class="img_pick_panel">
        <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="OnSearchButtonClick"
            Style="display: none;" />
        <asp:ScriptManager runat="server" ID="ScriptManager1">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel">
            <ContentTemplate>
                <div class="zpq group2 img_pick" id="see" runat="server">
                    <ul class="group2">
                        <asp:Repeater ID="repeaterList1" runat="server">
                            <ItemTemplate>
                                <li class="img_item js_imgitem">
                                    <div class="img_item_bd">
                                        <img class="pic wxmImg Zoomin" src="<%#Eval("ImgUrl") %>" style="width: 120px; height: 120px">
                                        <span class="check_content"><span class="check_content">
                                            <label class="frm_checkbox_label" for="checkbox1">
                                                <i class="icon_checkbox">
                                                    <input id="radio" name="check" type="radio" value="<%#Eval("ImgUrl") %>"></i><span
                                                        class="lbl_content">
                                                        <%#Eval("ImgName") %></span>
                                            </label>
                                        </span>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
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
    </div>
    </form>
    <div style="text-align: center; padding: 5px; height: 44px; line-height: 44px">
        <a href="javascript:void(0)" class="SIMPO_Text_Blue" onclick="saveAdd();">选择</a>
        <a href="javascript:void(0)" class="SIMPO_Text_Gray" onclick="closeAdd();">取消</a>
    </div>
</body>
</html>
