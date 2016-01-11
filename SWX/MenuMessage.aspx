<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuMessage.aspx.cs" Inherits="SWX.MenuMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Effect.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Form.css" rel="stylesheet" type="text/css" />
    <style>
        #uploadifyUploader
        {
            background: url('/JS/uploadify/upload.png') no-repeat;
        }
        #uploadify1Uploader
        {
            background: url('/JS/uploadify/upload.png') no-repeat;
        }
        #uploadify2Uploader
        {
            background: url('/JS/uploadify/upload.png') no-repeat;
        }
        .menukey
        {
            margin: 0;
            padding: 0;
            border-top: solid 1px #fff;
            background-color: #3399aa;
        }
        .menukey li
        {
            list-style-type: none;
            padding-left: 20px;
            padding-top: 10px;
            padding-bottom: 10px;
            border-bottom: solid 1px #fff;
        }
        .menukey li:hover
        {
            cursor: pointer;
            background-color: #eeeea0;
        }
        .menukey li a
        {
            color: White;
            font-weight: bold;
            text-decoration: none;
        }
        .menukey li:hover a
        {
            color: Black !important;
        }
    </style>
    <link href="CSS/picture.css" rel="stylesheet" />
    <link href="JS/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="JS/easyui/easyui.css" rel="stylesheet" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/easyui/jquery.easyui.min.js"></script>
    <script src="JS/uploadify/swfobject.js" type="text/javascript"></script>
    <script src="JS/uploadify/jquery.uploadify.v2.1.0.min.js" type="text/javascript"></script>
    <script src="JS/SimpoWindow.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#caidanlist').datagrid({
                width: 'auto',
                height: 530,
                striped: true,
                singleSelect: true,
                url: 'MenuMessage.aspx?action=Load',
                loadMsg: '数据加载中请稍后……',
                title: '菜单消息列表',
                rownumbers: true,
                fitColumns: true,
                pagination: true,
                pageSize: 10,  //页面显示条目数
                pageList: [100, 200],
                columns: [[
                    { field: 'Id', title: '菜单ID', align: 'center', width: 80, hidden: true },
                    { field: 'Name', title: '菜单名称', align: 'center', width: 90 },
                    { field: 'Title', title: '标题', align: 'center', width: 130 },
                    { field: 'Description', title: '描述', align: 'center', width: 100 },
                    { field: 'PicUrl', title: '图片url', align: 'center', width: 100 },
                    { field: 'Url', title: 'Url', align: 'center', width: 100 },
                    { field: 'Sort', title: '排序', align: 'center', width: 60 },
                    { field: 'OrgID', title: '微信公众号原始ID', align: 'center', width: 90, hidden: true },
                    {
                        field: 'caozuo', title: '操作', align: 'center', width: 70, formatter: function (value, row, index) {
                            return '<a href="#" onclick="preview(' + row.Id + ',\'' + row.Name + '\');" class="easyui-menubutton">预览</a>&nbsp;&nbsp;<a href="#" onclick="editMenu(' + row.Id + ');" class="easyui-menubutton">修改</a>&nbsp;&nbsp;<a href="#" onclick="deleteMenu(' + row.Id + ');" class="easyui-menubutton">删除</a>';
                        }
                    }
                ]]
            });

            // 图片上传插件配置
            $("#uploadify2").uploadify({
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

                    $('#uploadify2').uploadifyUpload();
                },

                // 文件上传完成后触发（每个文件触发一次）
                'onComplete': function (e, queueID, fileObj, response, data) {
                    saveData(response);
                },
                // 当队列中的所有文件全部完成上传时触发
                'onAllComplete': function (e, data) {

                }
            });
            // 修改图片上传插件配置
            $("#uploadify1").uploadify({
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

                    $('#uploadify1').uploadifyUpload();
                },

                // 文件上传完成后触发（每个文件触发一次）
                'onComplete': function (e, queueID, fileObj, response, data) {
                    saveModifyData(response);
                },
                // 当队列中的所有文件全部完成上传时触发
                'onAllComplete': function (e, data) {

                }
            });


            //点击菜单key
            $(".menukey li").click(function () {
                var items = document.getElementsByTagName("a");
                for (var i = 0; i < items.length; i++) {
                    if ($.trim(items[i].innerHTML) == $.trim($(this).find("a").html())) {
                        //$("#main").attr("src", $(items[i]).attr("url"));
                        items[i].click();
                    }
                }

                $(".menukey li").find("a").css("color", "#fff");
                $(this).find("a").css("color", "#eeaa66");
            });

        });

        function menulist(mkey) {
            $("#caidanlist").datagrid('options').queryParams = {
                mkey: mkey
            };
            $("#caidanlist").datagrid('options').url = "MenuMessage.aspx?action=Load";
            $("#caidanlist").datagrid('load');
        }


        function saveData(response) {
            $.ajax({
                type: "post",
                async: false,
                url: "MenuMessage.aspx?action=addPicture",
                data: { response: response },
                success: function (msg) {
                    var filetop = $("#fileurl").val();
                    $("#PicUrl").val(filetop + msg);
                    $("#PictureCountent").html("<img  src='" + msg + "' alt='' style='height:100px; width:400px'/>")
                    alert("上传成功");
                }
            });
        }

        function saveModifyData(response) {
            $.ajax({
                type: "post",
                async: false,
                url: "MenuMessage.aspx?action=addPicture",
                data: { response: response },
                success: function (msg) {
                    var filetop = $("#fileurl").val();
                    $("#PicUrl1").val(filetop + msg);
                    $("#PictureCountent1").html("<img  src='" + msg + "' alt='' style='height:100px; width:400px'/>")
                    alert("上传成功");
                }
            });
        }

        //修改
        function editMenu(menuId) {
            $("#ifnewup").val("2");
            document.getElementById("upMenuID").value = menuId;
            $.ajax({
                type: "post",
                async: false,
                url: "MenuMessage.aspx?action=edit",
                data: { id: menuId },
                dataType: 'json',
                success: function (msg) {
                    $("#MenuKey1").val(msg[0].MenuKey);
                    $("#Title1").val(msg[0].Title);
                    $("#Description1").val(msg[0].Description);
                    $('#PicUrl1').val(msg[0].PicUrl);
                    $('#Url1').val(msg[0].Url);
                    $("#PictureCountent1").html("<img  src='" + msg[0].PicUrl + "' alt='' style='height:100px; width:400px'/>")
                    $('#Sort1').numberbox('setValue', msg[0].Sort);
                }
            });
            $('#updateMenu').window('open');
        }
        //删除菜单
        function deleteMenu(menuId) {
            if (confirm("确定要删除此菜单吗？")) {
                $.ajax({
                    type: "post",
                    async: false,
                    url: "MenuMessage.aspx?action=deleteMenu",
                    data: { id: menuId },
                    dataType: 'json',
                    success: function (msg) {
                        if (msg == '1') {
                            alert("删除成功！");
                            $('#caidanlist').datagrid('reload');
                        } else {
                            alert("删除失败！");
                        }
                    }
                });
            }

        }
        //修改保存菜单
        function SaveModifyUser() {
            var menuId = $("#upMenuID").val();
            var Menukey = $("#MenuKey1").val();
            var Title = $("#Title1").val();
            var Description = $("#Description1").val();
            var PicUrl = $("#PicUrl1").val();
            var Url = $('#Url1').val();
            //var Sort = $('#Sort1').val();
            var Sort = $('#Sort1').numberbox('getValue');
            if ($.trim(Menukey) == "" || Menukey == "") {
                alert("菜单key为必录，不能为空！");
                return;
            }
            if ($.trim(Title) == "" || Title == "") {
                alert("标题为必录，不能为空！");
                return;
            }
            $.ajax({
                type: "post",
                async: false,
                url: "MenuMessage.aspx?action=update",
                data: { id: menuId, Menukey: Menukey, Title: Title, Description: Description, PicUrl: PicUrl, Url: Url, Sort: Sort },
                dataType: 'json',
                success: function (msg) {
                    if (msg == "1") {
                        alert("修改菜单成功！");
                        closeUpdate();
                        $('#caidanlist').datagrid('reload');
                    } else {
                        alert("修改菜单失败！");
                    }
                }
            });
        }
        //关闭修改菜单窗
        function closeUpdate() {
            $("#MenuKey1").val("");
            $("#Title1").val("");
            $("#Description1").val("");
            $('#PicUrl1').val("");
            $('#Url1').val("");
            //$('#Sort1').val("");
            $('#Sort1').numberbox('setValue', "");
            $('#updateMenu').window('close');
        }

        //新增窗口
        function addMenu() {
            $('#addMenu').window('open');
            $("#ifnewup").val("1");
        }
        //新增保存菜单
        function SaveAddUser() {
            var MenuKey = $("#MenuKey").val();
            var Title = $("#Title").val();
            var Description = $("#Description").val();
            var PicUrl = $('#PicUrl').val();
            var Url = $('#Url').val();
            //var Sort = $('#Sort').val();
            if ($('#Sort').val() == "") {
                alert("排序必填！");
                return;
            }
            var Sort = $('#Sort').numberbox('getValue');
            if ($.trim(MenuKey) == "" || MenuKey == "") {
                alert("菜单key为必录，不能为空！");
                return;
            }
            if ($.trim(Title) == "" || Title == "") {
                alert("标题为必录，不能为空！");
                return;
            }
            $.ajax({
                type: "post",
                async: false,
                url: "MenuMessage.aspx?action=add",
                data: { Menukey: MenuKey, Title: Title, Description: Description, PicUrl: PicUrl, Url: Url, Sort: Sort },
                dataType: 'json',
                success: function (msg) {
                    if (msg == "1") {
                        alert("新增菜单成功！");
                        closeNew();
                        $('#caidanlist').datagrid('reload');
                    } else {
                        alert("新增菜单失败！");
                    }
                }
            });
        }
        //关闭新增窗口
        function closeNew() {
            $("#MenuKey").val("");
            $("#Title").val("");
            $("#Description").val("");
            $('#PicUrl').val("");
            $('#Url').val("");
            //$('#Sort').val("");
            $('#Sort').numberbox('setValue', "");
            $('#addMenu').window('close');
        }

        function searchPic() {
            SimpoWin.showWin("选择素材", "NewGroupPicture.aspx", 880, 580);;
        }

        //加载选中图片到页面
        function saveAdd(imgurl) {
            //$('#MessageAdd').window('close');
            //var imgurl = $('input:radio[name="check"]:checked').val();
            var abc = $("#ifnewup").val();
            var filetop = $("#fileurl").val();
            var fileurl = filetop + imgurl;
            if (abc == "1") {
                $("#PicUrl").val(fileurl);
                $("#PictureCountent").html("<img  src='" + imgurl + "' alt='' style='height:100px; width:400px'/>")
            }
            if (abc == "2") {
                $("#PicUrl1").val(fileurl);
                $("#PictureCountent1").html("<img  src='" + imgurl + "' alt='' style='height:100px; width:400px'/>")
            }
        }

        function preview(id, title) {
            SimpoWin.showWin("预览" + title, "PreviewMenuMsg.aspx?id=" + id, 380, 500);
        }

    </script>
</head>
<body>
    <form id="form1">
    <div>
        <input type="button" onclick="addMenu()" value="新增" class="SIMPO_Text_Blue" /></div>
    <input type="hidden" id="ifnewup" />
    <input type="hidden" id="fileurl" value="<%=GetFile() %>" />
    <br />
    <div id="departTab" class="easyui-tabs e-tabs">
    <div title="菜单消息列表" class="e-tabs-div">
        <div class="easyui-layout" style="width:auto;height:550px;margin-left:5px;">
            <div data-options="region:'west',split:true" title="菜单key" style="width:120px;">
                <ul class="menukey">
                <%=GetMenusKey() %>
            </ul>
            </div>
            <div data-options="region:'center'" >
                <table id="caidanlist" class="easyui-datagrid"></table>
            </div>
        </div>
    </div>
</div>



    <%--<div>
        <table id="caidanlist" class="easyui-datagrid">
        </table>
    </div>--%>
    </form>
    <div id="addMenu" class="easyui-window" title="添加菜单消息" data-options="modal:true,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true,iconCls:'icon-add'"
        style="width: 800px; height: 450px;">
        <div style="padding: 10px 5px 10px 5px">
            <form id="Form2" runat="server">
            <table class="tabFrm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tdTitle" style="width: 80px;">
                        菜单Key：
                    </td>
                    <td>
                        <%=GetMenus("MenuKey") %>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        标题：
                    </td>
                    <td>
                        <textarea id="Title" name="Title" rows="5" cols="40" class="SIMPO_Txt_200" style="width: 650px;
                            height: 30px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        描述：
                    </td>
                    <td>
                        <textarea id="Description" name="Description" rows="5" cols="40" class="SIMPO_Txt_200"
                            style="width: 650px; height: 50px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        &nbsp;
                    </td>
                    <td>
                        <input type="file" name="uploadify" id="uploadify2" class="uploadbutton" />
                        <span>说明：单个文件大小限制在 2MB 以内</span>
                        <input type="button" onclick="searchPic()" value="从图片库中选择" class="SIMPO_Text_Blue"
                            style="margin-bottom: 15px;" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        图片url：
                    </td>
                    <td>
                        <textarea id="PicUrl" name="PicUrl" rows="5" cols="40" class="SIMPO_Txt_200" style="width: 650px;
                            height: 30px;" runat="server"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        图片：
                    </td>
                    <td>
                        <div id="PictureCountent">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        Url：
                    </td>
                    <td>
                        <textarea id="Url" name="Url" rows="5" cols="40" class="SIMPO_Txt_200" style="width: 650px;
                            height: 50px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        排序：
                    </td>
                    <td>
                        <input id="Sort" name="Sort" type="text" class="easyui-numberbox SIMPO_Txt_200" />
                    </td>
                </tr>
            </table>
            <div style="text-align: center; padding-top: 10px">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                    onclick="SaveAddUser();">保存</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                        data-options="iconCls:'icon-cancel'" onclick="closeNew();">取消</a>
            </div>
            </form>
        </div>
    </div>
    <div id="updateMenu" class="easyui-window" title="修改菜单消息" data-options="modal:true,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true,iconCls:'icon-edit'"
        style="width: 800px; height: 450px;">
        <div style="padding: 10px 5px 20px 5px">
            <form id="Form3">
            <table class="tabFrm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tdTitle" style="width: 80px;">
                        菜单Key：
                    </td>
                    <td>
                        <%=GetMenus("MenuKey1") %>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        标题：
                    </td>
                    <td>
                        <textarea id="Title1" name="Title" rows="2" cols="40" class="SIMPO_Txt_200" style="width: 650px;
                            height: 30px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        描述：
                    </td>
                    <td>
                        <textarea id="Description1" name="Description" rows="2" cols="40" class="SIMPO_Txt_200"
                            style="width: 650px; height: 50px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        &nbsp;
                    </td>
                    <td>
                        <input type="file" name="uploadify" id="uploadify1" class="uploadbutton" />
                        <span>说明：单个文件大小限制在 2MB 以内</span>
                        <input type="button" onclick="searchPic()" value="从图片库中选择" class="SIMPO_Text_Blue"
                            style="margin-bottom: 15px;" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        图片url：
                    </td>
                    <td>
                        <textarea id="PicUrl1" name="PicUrl" rows="2" cols="40" class="SIMPO_Txt_200" style="width: 650px;
                            height: 30px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        图片：
                    </td>
                    <td>
                        <div id="PictureCountent1">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        Url：
                    </td>
                    <td>
                        <textarea id="Url1" name="Url" rows="2" cols="40" class="SIMPO_Txt_200" style="width: 650px;
                            height: 50px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        排序：
                    </td>
                    <td>
                        <input id="Sort1" name="Sort" type="text" class="easyui-numberbox SIMPO_Txt_200" />
                    </td>
                </tr>
            </table>
            <div style="text-align: center; padding-top: 10px">
                <input id="upMenuID" name="upMenuID" type="hidden" />
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                    onclick="SaveModifyUser();">保存</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                        data-options="iconCls:'icon-cancel'" onclick="closeUpdate();">取消</a>
            </div>
            </form>
        </div>
    </div>
</body>
</html>
