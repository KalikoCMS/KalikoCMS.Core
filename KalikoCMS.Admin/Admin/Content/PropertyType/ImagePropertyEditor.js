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
                $(descriptionField).val(newDescription);
                
                if (newImagePath.length > 0) {
                    $(previewImage).attr('src', 'Assets/Images/Thumbnail.ashx?path=' + escape(newImagePath));
                } else {
                    $(previewImage).attr('src', 'assets/images/no-image.jpg');
                    $(originalPathField).val('');
                }
            };

            top.registerCallback(callback);

            top.propertyEditor.dialogs.openEditImageDialog(imagePath, originalPath, cropX, cropY, cropW, cropH, width, height, description);
        }
    };
})(top.propertyEditor || (top.propertyEditor = {}));


(function (dialogs) {
    dialogs.openEditImageDialog = function (imagePath, originalPath, cropX, cropY, cropW, cropH, width, height, description) {
        parent.openModal("Content/Dialogs/EditImageDialog.aspx?imagePath=" + imagePath + "&originalPath=" + originalPath + "&cropX=" + cropX + "&cropY=" + cropY + "&cropW=" + cropW + "&cropH=" + cropH + "&width=" + width + "&height=" + height + "&description=" + escape(description), 710, 500);
        return false;
    };
})(top.propertyEditor.dialogs || (top.propertyEditor.dialogs = {}));