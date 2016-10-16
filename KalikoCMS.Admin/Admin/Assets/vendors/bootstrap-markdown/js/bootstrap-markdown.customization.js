

(function ($) {
    // Replace bootstrap-markdown.js's dialogs for links and images to the Kaliko CMS ones
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

    // Extend bootstrap-markdown.js with header options
    $.fn.markdown.defaults.buttons[0].push({
        name: 'groupHeaders',
        data: [
            {
                name: 'cmdH1',
                title: 'Header 1',
                btnText: "H1",
                callback: function(e) {
                    var selected = e.getSelection();
                    e.replaceSelection('# ' + selected.text);
                    var cursor = selected.start + 2;
                    e.setSelection(cursor, cursor + selected.length);
                }
            },
            {
                name: 'cmdH2',
                title: 'Header 2',
                btnText: "H2",
                callback: function(e) {
                    var selected = e.getSelection();
                    e.replaceSelection('## ' + selected.text);
                    var cursor = selected.start + 3;
                    e.setSelection(cursor, cursor + selected.length);
                }
            },
            {
                name: 'cmdH3',
                title: 'Header 3',
                btnText: "H3",
                callback: function(e) {
                    var selected = e.getSelection();
                    e.replaceSelection('### ' + selected.text);
                    var cursor = selected.start + 4;
                    e.setSelection(cursor, cursor + selected.length);
                }
            }, {
                name: 'cmdH4',
                title: 'Header 4',
                btnText: "H4",
                callback: function(e) {
                    var selected = e.getSelection();
                    e.replaceSelection('#### ' + selected.text);
                    var cursor = selected.start + 5;
                    e.setSelection(cursor, cursor + selected.length);
                }
            }
        ]
    });

    // Extend bootstrap-markdown.js with header options
    $.fn.markdown.defaults.buttons[0].push({
        name: 'groupHelp',
        data: [
            {
                name: 'cmdHelp',
                title: 'Markdown syntax',
                icon: 'icon-question',
                btnClass: 'btn btn-primary btn-sm',
                callback: function(e) {
                    window.open('http://kaliko.com/cms/knowledge-base/markdown-syntax/');
                }
            }
        ]
    });

    // Extend bootstrap-markdown.js with extra options
    $.fn.markdown.defaults.buttons[0].push({
        name: 'groupExtras',
        data: [
            {
                name: 'cmdEmbedImage',
                title: 'Embed image as base64',
                btnText: 'Embed',
                icon: 'icon-image',
                callback: function (e) {
                    var chunk, cursor, selected = e.getSelection();

                    if (selected.length === 0) {
                        chunk = e.__localize('image description');
                    } else {
                        chunk = selected.text;
                    }

                    top.registerCallback(function (imagePath, cropX, cropY, cropW, cropH, originalPath, description) {
                        $.ajax({
                            type: 'POST',
                            url: 'Handlers/Base64Handler.ashx',
                            data: { 'path': imagePath },
                            success: function (result) {
                                e.replaceSelection('![' + description + '](' + result + ')');
                                cursor = selected.start + 2;
                                e.setSelection(cursor, cursor + description.length);
                            },
                            error: function () {
                                alert('Could not preview the Markdown, please check the syntax.');
                            },
                            async: false
                        });
                    });

                    top.propertyEditor.dialogs.openEditImageDialog('', '', '', '', '', '', '', '', chunk);
                }
            }
        ]
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
            e.setSelection(cursor, cursor + description.length);
        });

        top.propertyEditor.dialogs.openEditImageDialog('', '', '', '', '', '', '', '', chunk);
    }

})(jQuery);