(function (propertyEditor) {
    propertyEditor.link = {
        openDialog: function (urlField, typeField, displayField) {
            var url = $(urlField).val();
            var type = $(typeField).val();

            var callback = function (newUrl, newType) {
                $(urlField).val(newUrl);
                $(typeField).val(newType);
                $(displayField).val(newUrl);
            };

            top.registerCallback(callback);

            top.propertyEditor.dialogs.openSelectLinkDialog(url, type);
        }
    };
})(top.propertyEditor || (top.propertyEditor = {}));


(function (dialogs) {
    dialogs.openSelectLinkDialog = function (url, type) {
        parent.openModal("Content/Dialogs/SelectLinkDialog.aspx?url=" + escape(url) + "&type=" + type, 500, 240);
        return false;
    };
})(top.propertyEditor.dialogs || (top.propertyEditor.dialogs = {}));