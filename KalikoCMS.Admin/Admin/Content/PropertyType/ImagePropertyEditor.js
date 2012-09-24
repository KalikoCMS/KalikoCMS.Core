propertyEditor.image = {

    openDialog: function (pathField, displayField, previewImage) {
        var filePath = $(pathField).val();

        var callback = function (newFilePath) {
            $(displayField).val(newFilePath);
            $(pathField).val(newFilePath);
            $(previewImage).attr('src', '/Admin/Assets/Images/Thumbnail.ashx?path=' + escape(newFilePath));
        };

        window.propertyEditor.dialogs.openSelectFileDialog(filePath, callback);
    }

};
