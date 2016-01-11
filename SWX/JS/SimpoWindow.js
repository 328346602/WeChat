/**
* easyui的window插件再次封装
* 2014年11月10日
*/

SimpoWin = {
    showWin: function showWindow(title, url, width, height) {
        if (!top.SimpoWinId) top.SimpoWinId = 0;
        var divId = "simpoWin" + top.SimpoWinId;
        top.$("body").append('<div id="' + divId + '"></div>');

        top.$('#' + divId).window({
            modal: true,
            title: title,
            width: width,
            height: height,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            content: function () {
                return '<iframe frameborder="0" src="' + url + '" style="width: ' + (width - 14).toString() + 'px; height: ' + (height - 39).toString() + 'px; margin: 0;">';
            },
            onClose: function () {
                top.$('#' + divId).window('destroy');
                top.SimpoWinId--;
            }
        }).window('open');

        top.SimpoWinId++;
        switch (top.SimpoWinId) {
            case 1:
                top.SimpoWinParent1 = window;
                break;
            case 2:
                top.SimpoWinParent2 = window;
                break;
            case 3:
                top.SimpoWinParent3 = window;
                break;
            default:
                top.SimpoWinParent = window;
                break;
        }
    },

    closeWin: function () {
        var divId = "simpoWin" + (top.SimpoWinId - 1).toString();
        top.$('#' + divId).window('close');
    },

    GetWinParent: function () {
        switch (top.SimpoWinId) {
            case 1:
                return top.SimpoWinParent1;
            case 2:
                return top.SimpoWinParent2;
            case 3:
                return top.SimpoWinParent3;
            default:
                return top.SimpoWinParent;
        }
    }
}
