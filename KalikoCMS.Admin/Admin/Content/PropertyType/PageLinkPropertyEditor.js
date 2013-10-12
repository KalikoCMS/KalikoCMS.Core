(function (propertyEditor) {
    propertyEditor.pageLink = {
        openDialog: function (languageField, pageField, displayField) {
            var languageId = languageField.val();
            var pageId = pageField.val();

            var callback = function (newPageId, newPageName) {
                pageField.val(newPageId);
                displayField.val(newPageName);
            };

            top.registerCallback(callback);

            top.propertyEditor.dialogs.openSelectPageDialog(pageId, languageId);
        }
    };
})(top.propertyEditor || (top.propertyEditor = {}));


(function (dialogs) {
    dialogs.openSelectPageDialog = function(pageId, languageId) {
        parent.openModal("Content/Dialogs/SelectPageDialog.aspx?pageId=" + pageId + "&languageId=" + languageId, 500, 400);
        return false;
    };
})(top.propertyEditor.dialogs || (top.propertyEditor.dialogs = {}));