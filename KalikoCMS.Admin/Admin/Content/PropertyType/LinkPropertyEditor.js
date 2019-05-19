(function (propertyEditor) {
    var localTop = top;

    propertyEditor.link = propertyEditor.link || {
        openDialog: function (urlField, typeField, displayField) {
            var url = $(urlField).val();
            var type = $(typeField).val();

            var callback = function (newUrl, newType) {
                $(urlField).val(newUrl);
                $(typeField).val(newType);
                $(displayField).val(newUrl);
            };

            localTop.registerCallback(callback);

            localTop.propertyEditor.dialogs.openSelectLinkDialog(url, type);
        }
    };
})(top.propertyEditor || (top.propertyEditor = {}));


(function (dialogs) {
    var localParent = parent;

    dialogs.openSelectLinkDialog = dialogs.openSelectLinkDialog || function (url, type) {
        localParent.openModal("Content/Dialogs/SelectLinkDialog.aspx?url=" + escape(url) + "&type=" + type, 500, 240);
        return false;
    };
})(top.propertyEditor.dialogs || (top.propertyEditor.dialogs = {}));