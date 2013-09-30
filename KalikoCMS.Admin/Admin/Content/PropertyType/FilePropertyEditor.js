(function (propertyEditor) {
    propertyEditor.file = {
        openDialog: function(pathField, displayField) {
            var filePath = $(pathField).val();

            var callback = function(newFilePath) {
                $(displayField).val(newFilePath);
                $(pathField).val(newFilePath);
            };

            top.registerCallback(callback);
            
            window.propertyEditor.dialogs.openSelectFileDialog(filePath);
        }
    };
})(window.propertyEditor || (window.propertyEditor = {}));


(function(dialogs) {
    dialogs.openSelectFileDialog = function(filePath) {
        parent.openModal("Content/Dialogs/SelectFileDialog.aspx?filePath=" + filePath, 700, 500);
        return false;
    };
})(window.propertyEditor.dialogs || (window.propertyEditor.dialogs = {}));