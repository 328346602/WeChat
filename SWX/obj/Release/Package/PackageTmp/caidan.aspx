<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="caidan.aspx.cs" Inherits="SWX.CaiDan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="CSS/Effect.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Form.css" rel="stylesheet" type="text/css" />
    <link href="JS/easyui/easyui.css" rel="stylesheet" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/easyui/jquery.easyui.min.js"></script>
    <link href="Js/jqGrid/css/jquery-ui-1.9.2.custom.css" type="text/css" rel="stylesheet" />
    <link href="Js/jqGrid/css/ui.jqgrid.css" type="text/css" rel="stylesheet" />
    <script src="Js/jqGrid/js/i18n/grid.locale-cn.js" type="text/javascript"></script>
    <script src="Js/jqGrid/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="Js/jqGrid/js/grid.treegrid.js" type="text/javascript"></script>
    <script type="text/javascript">

        function onLoadBox() {
            $('#bianhao').combobox({
                valueField: 'Code',
                textField: 'Name',
                editable: false,
                url: 'caidan.aspx?action=menulist'
            });
            $('#bianhao1').combobox({
                valueField: 'Code',
                textField: 'Name',
                editable: false,
                url: 'caidan.aspx?action=menulist'
            });
        }

        $(function () {

            onLoadBox();

            var data = [];
            data.push({ text: "无", value: "", 'selected': 'true' });
            data.push({ text: "click", value: "click" });
            data.push({ text: "view", value: "view" });
            $('#menutype').combobox("loadData", data);
            $('#menutype1').combobox("loadData", data);
            $('#menutype').combobox("options").onSelect = function (d) {
                var power = d.value;
                if (power == 'click') {
                    document.getElementById('menuurl').disabled = true;
                    document.getElementById('menukey').disabled = false;
                    $("#menuurl").val("");
                }
                if (power == 'view') {
                    document.getElementById('menukey').disabled = true;
                    document.getElementById('menuurl').disabled = false;
                    $("#menukey").val("");
                }
                if (power == '') {
                    document.getElementById('menukey').disabled = true;
                    document.getElementById('menuurl').disabled = true;
                    $("#menukey").val("");
                    $("#menuurl").val("");
                }
            }
            $('#menutype1').combobox("options").onSelect = function (d) {
                var power = d.value;
                if (power == 'click') {
                    document.getElementById('menuurl1').disabled = true;
                    document.getElementById('menukey1').disabled = false;
                    $("#menuurl1").val("");
                }
                if (power == 'view') {
                    document.getElementById('menukey1').disabled = true;
                    document.getElementById('menuurl1').disabled = false;
                    $("#menukey1").val("");
                }
                if (power == '') {
                    document.getElementById('menukey').disabled = true;
                    document.getElementById('menuurl').disabled = true;
                    $("#menukey1").val("");
                    $("#menuurl1").val("");
                }
            }
            $("#caidanlist").jqGrid({
                url: 'caidan.aspx?action=tree',
                datatype: "json",
                colNames: ['菜单Name', '菜单ID', '编号', '菜单Type', '菜单Key', '菜单Url', '微信公众号原始ID', '操作'],
                colModel: [

                    { name: 'Name', index: 'Name', sortable: false, width: 100 },
                    { name: 'Id', index: 'Id', sortable: false, width: 100, align: 'center', hidden: true },
                    { name: 'Code', index: 'Code', sortable: false, align: 'center', width: 100, hidden: true },
                    { name: 'Type', index: 'Type', sortable: false, align: 'center', width: 100 },
                    { name: 'MenuKey', index: 'MenuKey', sortable: false, align: 'center', width: 100 },
                    { name: 'Url', index: 'Url', sortable: false, align: 'center', width: 200 },
                    { name: 'OrgID', index: 'OrgID', sortable: false, align: 'center', width: 100, hidden: true },
                    {
                        name: 'caozuo', index: 'caozuo', sortable: false, width: 100, align: 'center', formatter: function (v, o, r) {

                            return '<a href="#" onclick="editMenu(' + r["Id"] + ');" class="easyui-menubutton">修改</a>&nbsp;&nbsp;<a href="#" onclick="deleteMenu(' + r["Id"] + ',\'' + r["Code"] + '\');" class="easyui-menubutton">删除</a>';
                        }
                    }
                ],
                treeGrid: true,
                treeGridModel: 'adjacency', //treeGrid模式，跟json元数据有关 
                ExpandColumn: 'Name',
                ExpandColClick: true, // 树形表格是否展开
                viewrecords: true,
                rownumbers: false,
                height: 'auto',
                width: $("#tree1").width(),
                treeReader: {
                    level_field: "level",
                    parent_id_field: "parent",
                    leaf_field: "isLeaf",
                    expanded_field: "expanded"
                }
            });


            //$('#caidanlist').datagrid({
            //    width: 'auto',
            //    height: 530,
            //    striped: true,
            //    singleSelect: true,
            //    url: 'caidan.aspx?action=Load',
            //    loadMsg: '数据加载中请稍后……',
            //    title: '菜单列表',
            //    rownumbers: true,
            //    fitColumns: true,
            //    pagination: true,
            //    pageSize: 10,  //页面显示条目数
            //    pageList: [50, 100],
            //    columns: [[
            //        { field: 'Id', title: '菜单ID', align: 'center', width: 80, hidden: true },
            //        { field: 'Code', title: '编号', align: 'center', width: 80 },
            //        { field: 'Name', title: '菜单Name', align: 'center', width: 80 },
            //        { field: 'Type', title: '菜单Type', align: 'center', width: 80 },
            //        { field: 'MenuKey', title: '菜单Key', align: 'center', width: 80 },
            //        { field: 'Url', title: '菜单Url', align: 'center', width: 120 },
            //        { field: 'OrgID', title: '微信公众号原始ID', align: 'center', width: 90, hidden: true },
            //        {
            //            field: 'caozuo', title: '操作', align: 'center', width: 50, formatter: function (value, row, index) {
            //                return '<a href="#" onclick="editMenu(' + row.Id + ');" class="easyui-menubutton">修改</a>&nbsp;&nbsp;<a href="#" onclick="deleteMenu(' + row.Id + ');" class="easyui-menubutton">删除</a>';
            //            }
            //        }
            //    ]]
            //});
        });

        //修改
        function editMenu(menuId) {
            document.getElementById("upMenuID").value = menuId;
            $.ajax({
                type: "post",
                async: false,
                url: "caidan.aspx?action=edit",
                data: { id: menuId },
                dataType: 'json',
                success: function (msg) {
                    $("#bianhao1").combobox('setValue', msg[0].Code);
                    $("#menuname1").val(msg[0].Name);
                    $("#menutype1").combobox('setValue', msg[0].Type);
                    $('#menukey1').val(msg[0].MenuKey);
                    $('#menuurl1').val(msg[0].Url);
                    if (msg[0].Type == 'click') {
                        document.getElementById('menuurl1').disabled = true;
                        document.getElementById('menukey1').disabled = false;
                    }
                    if (msg[0].Type == 'view') {
                        document.getElementById('menukey1').disabled = true;
                        document.getElementById('menuurl1').disabled = false;
                    }
                    if (msg[0].Type == '') {
                        document.getElementById('menukey1').disabled = true;
                        document.getElementById('menuurl1').disabled = true;
                    }
                }
            });
            $('#updateMenu').window('open');
        }
        //删除菜单
        function deleteMenu(menuId, code) {
            if (code.length == 2) {
                if (confirm("删除根菜单，会同时删除此根菜单下的子菜单，确定要删除此根菜单吗？")) {
                    $.ajax({
                        type: "post",
                        async: false,
                        url: "caidan.aspx?action=deleteOneMenu",
                        data: { code: code },
                        dataType: 'json',
                        success: function (msg) {
                            if (msg == '1') {
                                alert("删除成功！");
                                onLoadBox();
                                jQuery("#caidanlist").jqGrid('setGridParam').trigger("reloadGrid");
                            } else {
                                alert("删除失败！");
                            }
                        }
                    });
                }
            } else {
                if (confirm("确定要删除此菜单吗？")) {
                    $.ajax({
                        type: "post",
                        async: false,
                        url: "caidan.aspx?action=deleteMenu",
                        data: { id: menuId },
                        dataType: 'json',
                        success: function (msg) {
                            if (msg == '1') {
                                alert("删除成功！");
                                jQuery("#caidanlist").jqGrid('setGridParam').trigger("reloadGrid");
                            } else {
                                alert("删除失败！");
                            }
                        }
                    });
                }
            }


        }
        //修改保存菜单
        function SaveModifyUser() {
            var menuId = $("#upMenuID").val();
            //var code = $("#bianhao1").val();
            var name = $("#menuname1").val();
            var type = $("#menutype1").combobox('getValue')
            var key = $('#menukey1').val();
            var url = $('#menuurl1').val();
            if ($.trim(name) == "" || name == "") {
                alert("菜单名称为必录，不能为空！");
                return;
            }
            $.ajax({
                type: "post",
                async: false,
                url: "caidan.aspx?action=update",
                data: { id: menuId, name: name, type: type, key: key, url: url },
                dataType: 'json',
                success: function (msg) {
                    if (msg == "1") {
                        alert("修改菜单成功！");
                        closeUpdate();
                        jQuery("#caidanlist").jqGrid('setGridParam').trigger("reloadGrid");
                    } else {
                        alert("修改菜单失败！");
                    }
                }
            });
        }
        //关闭修改菜单窗
        function closeUpdate() {
            $("#bianhao1").val("");
            $("#menuname1").val("");
            $('#menukey1').val("");
            $('#menuurl1').val("");
            document.getElementById('menuurl1').disabled = false;
            document.getElementById('menukey1').disabled = false;
            $('#updateMenu').window('close');
        }

        //新增窗口
        function addMenu() {
            $('#addMenu').window('open');
        }
        //新增保存菜单
        function SaveAddUser() {
            var code = $("#bianhao").combobox('getValue');
            var name = $("#menuname").val();
            var type = $("#menutype").combobox('getValue');
            var key = $('#menukey').val();
            var url = $('#menuurl').val();
            if ($.trim(code) == "" || code == "") {
                alert("请选择菜单类型！");
                return;
            }
            if ($.trim(name) == "" || name == "") {
                alert("菜单名称为必录，不能为空！");
                return;
            }
            $.ajax({
                type: "post",
                async: false,
                url: "caidan.aspx?action=add",
                data: { code: code, name: name, type: type, key: key, url: url },
                dataType: 'json',
                success: function (msg) {
                    if (msg == "1") {
                        alert("新增菜单成功！");
                        closeAdd();
                        onLoadBox();
                        jQuery("#caidanlist").jqGrid('setGridParam').trigger("reloadGrid");
                    } else {
                        alert("新增菜单失败！");
                    }
                }
            });
        }
        //关闭新增窗口
        function closeAdd() {
            $("#bianhao").val("");
            $("#menuname").val("");
            $('#menukey').val("");
            $('#menuurl').val("");
            $('#bianhao').combobox('setValue', "");
            $('#menutype').combobox('setValue', "");
            $('#addMenu').window('close');
        }

        //同步到微信
        function createMenu() {
            $.ajax({
                type: "post",
                async: false,
                url: "caidan.aspx?action=updateWX",
                data: {},
                dataType: 'json',
                success: function (data) {
                    if (data.code == 1) {
                        alert("成功！");
                    } else {
                        alert("失败！原因：" + data.msg);
                    }
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" onclick="addMenu()" value="新增" class="SIMPO_Text_Blue" />
        <input type="button" onclick="if(confirm('确定将菜单同步到微信？')) createMenu();" value="同步到微信"
            class="SIMPO_Text_Blue" style="margin-left: 50px;" />
    </div>
    <br />
    <div id="tree1" style="font-size: 12px;">
        <table id="caidanlist">
        </table>
    </div>
    </form>
    <div id="addMenu" class="easyui-window" title="添加菜单" data-options="modal:true,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true,iconCls:'icon-add'"
        style="width: 500px; height: 400px;">
        <div style="padding: 10px 5px 20px 5px;">
            <form id="Form2">
            <table class="tabFrm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tdTitle" style="width: 80px;">
                        上级菜单：
                    </td>
                    <td>
                        <input id="bianhao" name="bianhao" class="easyui-combobox" style="width: 150px; height: 25px;" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单Name：
                    </td>
                    <td>
                        <input id="menuname" name="menuname" type="text" class="SIMPO_Txt_200" style="" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单Type：
                    </td>
                    <td>
                        <input id="menutype" name="menutype" class="easyui-combobox" data-options="editable:false"
                            style="width: 150px; height: 25px;" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单Key：
                    </td>
                    <td>
                        <input id="menukey" name="menukey" type="text" class="SIMPO_Txt_200" style="" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单Url：
                    </td>
                    <td>
                        <textarea id="menuurl" name="menuurl" rows="5" cols="40" class="SIMPO_Txt_200" style="width: 350px;
                            height: 80px;" disabled="disabled"></textarea>
                    </td>
                </tr>
            </table>
            <div style="text-align: center; padding: 20px">
                <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-save'"
                    onclick="SaveAddUser();">保存</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                        data-options="iconCls:'icon-cancel'" onclick="closeAdd();">取消</a>
            </div>
            </form>
        </div>
    </div>
    <div id="updateMenu" class="easyui-window" title="修改菜单" data-options="modal:true,collapsible:false,minimizable:false,maximizable:false,closed:true,closable:true,iconCls:'icon-edit'"
        style="width: 500px; height: 400px;">
        <div style="padding: 10px 5px 20px 5px">
            <form id="Form3">
            <table class="tabFrm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="tdTitle" style="width: 80px;">
                        上级菜单：
                    </td>
                    <td>
                        <input id="bianhao1" name="bianhao" class="easyui-combobox" style="width: 150px;
                            height: 25px;" readonly="false" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单name：
                    </td>
                    <td>
                        <input id="menuname1" name="menuname" type="text" class="SIMPO_Txt_200" style="" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单Type：
                    </td>
                    <td>
                        <input id="menutype1" name="menutype" class="easyui-combobox" data-options="editable:false"
                            style="width: 150px; height: 25px;" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单Key：
                    </td>
                    <td>
                        <input id="menukey1" name="menukey" type="text" class="SIMPO_Txt_200" style="" />
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle">
                        菜单Url：
                    </td>
                    <td>
                        <textarea id="menuurl1" name="menuurl" rows="5" cols="40" class="SIMPO_Txt_200" style="font-size: 12px;
                            width: 350px; height: 80px;"></textarea>
                    </td>
                </tr>
            </table>
            <div style="text-align: center; padding: 20px">
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
