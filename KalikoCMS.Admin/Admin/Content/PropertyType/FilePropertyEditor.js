
propertyEditor.file = {

    openDialog: function (pathField, displayField) {
        var filePath = $(pathField).val();

        var callback = function (newFilePath) {
            $(displayField).val(newFilePath);
            $(pathField).val(newFilePath);
        };

        window.propertyEditor.dialogs.openSelectFileDialog(filePath, callback);
    }
    
};
