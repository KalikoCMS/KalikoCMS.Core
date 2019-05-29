



function warnBeforeLeavingIfChangesBeenMade() {
    $('input').change(function () {
        if (window.warningRegistered !== true) {
            window.onbeforeunload = function () { return 'There might be changes that will be lost!'; };
            window.warningRegistered = true;
        }
    });

    $('.form-actions a').click(function () {
        window.onbeforeunload = null;
    });
    $('.form-actions button').click(function () {
        window.onbeforeunload = null;
    });
    $('.form-actions input').click(function () {
        window.onbeforeunload = null;
    });
}


function initHtmlEditor(assetsPath) {
    // TODO: Get editor options/toolbar from property attribute and web.config
    tinymce.init({
        skin_url: assetsPath + 'vendors/tinymce/skins/lightgray',
        selector: "textarea.html-editor",
        plugins: [
            "advlist autolink lists link image charmap anchor",
            "searchreplace visualblocks code hr",
            "media table contextmenu paste"
        ],
        resize: true,
        height: 300,
        menu: {
            edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall' },
            insert: { title: 'Insert', items: 'link image media | charmap anchor hr' },
            view: { title: 'View', items: 'visualaid visualblocks' },
            format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript | formats | removeformat' },
            table: { title: 'Table', items: 'inserttable tableprops deletetable | cell row column' },
            tools: { title: 'Tools', items: 'code' }
        },
        paste_data_images: true,
        extended_valid_elements: "i[class],span,span[class]",
        convert_urls: false,
        relative_urls: false,
        toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | code",
        file_picker_callback: function (callback, value, meta) {
            if (meta.filetype === 'file') {
                top.registerCallback(function (newUrl, newType) { callback(newUrl); });
                top.propertyEditor.dialogs.openSelectLinkDialog(value, 0);
            }
            if (meta.filetype === 'image') {
                top.registerCallback(function (imagePath, cropX, cropY, cropW, cropH, originalPath, description) { callback(imagePath, { alt: description }); });
                top.propertyEditor.dialogs.openEditImageDialog(value, value, '', '', '', '', '', '', '');
            }
        }
    });
}


function initMarkdownEditor() {
    $(".markdown-editor").markdown({
        savable: false,
        iconlibrary: 'fa-3',
        reorderButtonGroups: ['groupHeaders', 'groupFont', 'groupLink', 'groupMisc', 'groupExtras', 'groupUtil', 'groupHelp'],
        onPreview: function (e) {
            var retval = "";
            jQuery.ajax({
                type: 'POST',
                url: 'Handlers/MarkdownHandler.ashx',
                data: { 'markdown': e.getContent() },
                success: function (result) {
                    retval = result;
                },
                error: function () {
                    alert('Could not preview the Markdown, please check the syntax.');
                },
                async: false
            });

            return retval;
        }
    });
}


function initDropDowns() {
    $('select.selectbox')
        .each(function(index) {
            var element = $(this);

            var render;
            if (element.hasClass('dropdown--enhanced')) {
                render = renderEnhancedOption;
            }
            else if (element.hasClass('dropdown--full')) {
                render = renderFullOption;
            }
            else {
                render = renderSimpleOption;
            }

            element.selectric({
                optionsItemBuilder: function(itemData, element, index) {
                    var description = element.attr('data-description');
                    var image = element.attr('data-image');
                    return render(itemData.text, description, image);
                },
                labelBuilder: function(currItem) {
                    var description = currItem.element.attr('data-description');
                    var image = currItem.element.attr('data-image');
                    return render(currItem.text, description, image);
                }
            });
        });
}

function renderSimpleOption(text, description, image) {
    return text;
}
function renderEnhancedOption(text, description, image) {
    return '<div><div class="dropdown__title">' + text + '</div><div class="dropdown__description">' + description + '</div></div>';
}
function renderFullOption(text, description, image) {
    return '<div class="dropdown__item"><div><img src="' + image + '" class=\"dropdown__image\" /></div><div class="dropdown__block"><div class="dropdown__title">' +
        text + '</div><div class="dropdown__description">' + description + '</div></div></div>';
}