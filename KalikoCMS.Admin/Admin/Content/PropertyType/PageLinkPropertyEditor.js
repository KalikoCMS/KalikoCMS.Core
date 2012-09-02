

propertyEditor.pageLink = {

    openDialog: function (languageField, pageField, displayField) {
        var languageId = $(languageField).val();
        var pageId = $(pageField).val();

        var callback = function(newPageId, newPageName) {
            $(pageField).val(newPageId);
            $(displayField).val(newPageName);
        };

        window.propertyEditor.dialogs.openSelectPageDialog(pageId, languageId, callback);
    }
};
