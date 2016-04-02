

(function ($) {
    $.fn.markdown.defaults.buttons[0].forEach(function (group) {
        group.data.forEach(function (button) {
            if (button.name === 'cmdUrl') {
                button.callback = urlCallback;
            }
            if (button.name === 'cmdImage') {
                button.icon = { 'fa-3': 'icon-image' };
                button.callback = imageCallback;
            }
        });
    });

    function urlCallback(e) {
        var chunk, cursor, selected = e.getSelection();

        if (selected.length === 0) {
            chunk = 'link description';
        } else {
            chunk = selected.text;
        }

        top.registerCallback(function(newUrl, newType) {
            e.replaceSelection('[' + chunk + '](' + newUrl + ')');
            cursor = selected.start + 1;
            e.setSelection(cursor, cursor + chunk.length);
        });

        top.propertyEditor.dialogs.openSelectLinkDialog(chunk, 0);
    }

    function imageCallback(e) {
        var chunk, cursor, selected = e.getSelection();

        if (selected.length === 0) {
            chunk = e.__localize('image description');
        } else {
            chunk = selected.text;
        }

        top.registerCallback(function (imagePath, cropX, cropY, cropW, cropH, originalPath, description) {
            e.replaceSelection('![' + description + '](' + imagePath + ')');
            cursor = selected.start + 2;
            e.setSelection(cursor, cursor + imagePath.length);
        });

        top.propertyEditor.dialogs.openEditImageDialog('', '', '', '', '', '', '', '', chunk);
    }

})(jQuery);