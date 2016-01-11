<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManager.aspx.cs" Inherits="SWX.UserManager" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="CSS/Effect.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Ctrl.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Form.css" rel="stylesheet" type="text/css" />
    <link href="JS/easyui/easyui.css" rel="stylesheet" />
    <script src="JS/jquery.min.js"></script>
    <script src="JS/easyui/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        $(function () {

            $('#list').datagrid({
                width: 'auto',
                height: 530,
                striped: true,
                singleSelect: true,
                url: 'UserManager.aspx?action=load',
                loadMsg: '数据加载中请稍后……',
                title: '菜单列表',
                rownumbers: true,
                fitColumns: true,
                pagination: true,
                pageSize: 10,  //页面显示条目数
                pageList: [20, 30, 50, 100],
                columns: [[
                    { field: 'Id', title: '用户ID', align: 'center', width: 80, hidden: true },
                    { field: 'Status', title: '状态', align: 'center', width: 80, hidden: true },
                    { field: 'UserName', title: '用户名', align: 'center', width: 80 },
                    { field: 'OrgID', title: '微信公众号原始ID', align: 'center', width: 80 },
                    { field: 'AppID', title: 'AppID(应用ID)', align: 'center', width: 80 },
                    { field: 'zt', title: '状态', align: 'center', width: 80, formatter: function (value, row, index) {
                        return row.Status == 1 ? "正常" : "未启用";
                    }
                    },
                    {
                        field: 'caozuo', title: '操作', align: 'center', width: 50,
                        formatter: function (value, row, index) {
                            if (row.Status == 1) {
                                return '<a href="javascript:void(0);" onclick="ty(' + row.Id + ');" class="easyui-menubutton">停用</a>';
                            }
                            else {
                                return '<a href="javascript:void(0);" onclick="qy(' + row.Id + ');" class="easyui-menubutton">启用</a>';
                            }
                        }
                    }
                ]]
            });
        });

        //启用
        function qy(id) {
            if (confirm("确定要启用用户？")) {
                $.ajax({
                    type: "post",
                    async: false,
                    url: "UserManager.aspx?action=qy",
                    data: { id: id },
                    dataType: 'json',
                    success: function (msg) {
                        if (msg == '1') {
                            alert("启用成功！");
                            $('#list').datagrid('reload');
                        } else {
                            alert("操作失败！");
                        }
                    }
                });
            }
        }

        //停用
        function ty(id) {
            if (confirm("确定要启用用户？")) {
                $.ajax({
                    type: "post",
                    async: false,
                    url: "UserManager.aspx?action=ty",
                    data: { id: id },
                    dataType: 'json',
                    success: function (msg) {
                        if (msg == '1') {
                            alert("停用成功！");
                            $('#list').datagrid('reload');
                        } else {
                            alert("操作失败！");
                        }
                    }
                });
            }
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <br />
    <div>
        <table id="list" class="easyui-datagrid">
        </table>
    </div>
    </form>
</body>
</html>
