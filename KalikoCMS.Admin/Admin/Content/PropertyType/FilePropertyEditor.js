(function (propertyEditor) {
    propertyEditor.file = propertyEditor.file || {
        openDialog: function (pathField, displayField, context) {
            var filePath = context.$(pathField).val();

            var callback = function (newFilePath) {
                context.$(displayField).val(newFilePath);
                context.$(pathField).val(newFilePath);
            };

            top.registerCallback(callback);
            
            top.propertyEditor.dialogs.openSelectFileDialog(filePath);
        }
    };
})(top.propertyEditor || (top.propertyEditor = {}));


(function(dialogs) {
    dialogs.openSelectFileDialog = dialogs.openSelectFileDialog || function(filePath) {
        parent.openModal("Content/Dialogs/SelectFileDialog.aspx?filePath=" + filePath, 700, 500);
        return false;
    };
})(top.propertyEditor.dialogs || (top.propertyEditor.dialogs = {}));