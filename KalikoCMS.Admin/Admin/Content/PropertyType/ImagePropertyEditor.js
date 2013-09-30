(function (propertyEditor) {
    propertyEditor.image = {
        openDialog: function (pathField, previewImage, originalPathField, cropXField, cropYField, cropWField, cropHField, width, height, descriptionField) {
            var imagePath = $(pathField).val();
            var originalPath = $(originalPathField).val();
            var cropX = $(cropXField).val();
            var cropY = $(cropYField).val();
            var cropW = $(cropWField).val();
            var cropH = $(cropHField).val();
            var description = $(descriptionField).val();

            var callback = function (newImagePath, newCropX, newCropY, newCropW, newCropH, newOriginalPath, newDescription) {
                $(pathField).val(newImagePath);
                $(originalPathField).val(newOriginalPath);
                $(cropXField).val(newCropX);
                $(cropYField).val(newCropY);
                $(cropWField).val(newCropW);
                $(cropHField).val(newCropH);
                $(previewImage).attr('src', 'Assets/Images/Thumbnail.ashx?path=' + escape(newImagePath));
                $(descriptionField).val(newDescription);
            };

            top.registerCallback(callback);

            propertyEditor.dialogs.openEditImageDialog(imagePath, originalPath, cropX, cropY, cropW, cropH, width, height, description);
        }
    };
})(window.propertyEditor || (window.propertyEditor = {}));


(function (dialogs) {
    dialogs.openEditImageDialog = function (imagePath, originalPath, cropX, cropY, cropW, cropH, width, height, description) {
        parent.openModal("Content/Dialogs/EditImageDialog.aspx?imagePath=" + imagePath + "&originalPath=" + originalPath + "&cropX=" + cropX + "&cropY=" + cropY + "&cropW=" + cropW + "&cropH=" + cropH + "&width=" + width + "&height=" + height + "&description=" + escape(description), 710, 500);
        return false;
    };
})(window.propertyEditor.dialogs || (window.propertyEditor.dialogs = {}));