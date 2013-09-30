(function (propertyEditor) {
    propertyEditor.pageLink = {
        openDialog: function(languageField, pageField, displayField) {
            var languageId = $(languageField).val();
            var pageId = $(pageField).val();

            var callback = function(newPageId, newPageName) {
                $(pageField).val(newPageId);
                $(displayField).val(newPageName);
            };

            top.registerCallback(callback);
            
            window.propertyEditor.dialogs.openSelectPageDialog(pageId, languageId);
        }
    };
})(window.propertyEditor || (window.propertyEditor = {}));


(function (dialogs) {
    dialogs.openSelectPageDialog = function(pageId, languageId) {
        parent.openModal("Content/Dialogs/SelectPageDialog.aspx?pageId=" + pageId + "&languageId=" + languageId, 500, 400);
        return false;
    };
})(window.propertyEditor.dialogs || (window.propertyEditor.dialogs = {}));