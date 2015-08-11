(function (propertyEditor) {
    propertyEditor.collection = {
        openDialog: function (container, valueField, className) {
            var callback = function (val, exerpt) {
                var list = $(container).find("ul.sortable-collection");
                var content = '<a href="#" onclick="top.propertyEditor.collection.editField(this);return false;" class="pull-right"><i class="icon icon-edit"></i>edit</a><i class="icon icon-sort"></i> ' + exerpt;
                if (valueField == null && val != null) {
                    var newField = $('<li class="btn btn-default collection-item"></li>');
                    newField.attr('data-value', val).html(content);
                    list.append(newField);
                }
                else if (val == null) {
                    $(valueField).remove();
                }
                else {
                    $(valueField).attr('data-value', val).html(content);
                }

                list.trigger('sortupdate');
            };

            top.registerCallback(callback);

            var value = null;
            if (valueField != null) {
                value = $(valueField).attr('data-value');
            }

            top.propertyEditor.dialogs.openEditCollectionPropertyDialog(className, value);
        },
        editField: function (element) {
            var listItem = $(element).parent();
            var list = $(listItem).parent();
            var className = list.attr("data-type");
            var containerId = $(list).parent().attr('id');
            
            propertyEditor.collection.openDialog('#' + containerId, listItem, className);
        }
    };
})(top.propertyEditor || (top.propertyEditor = {}));


(function (dialogs) {
    dialogs.openEditCollectionPropertyDialog = function(className, value) {
        parent.openModal('Content/Dialogs/EditCollectionPropertyDialog.aspx?propertyType=' + className + '&value=' + escape(value), 780, 460);
        return false;
    };
})(top.propertyEditor.dialogs || (top.propertyEditor.dialogs = {}));


$(document).ready(function () {
    $(".sortable-collection")
        .sortable({ update: sortUpdate })
        .bind("sortupdate", sortUpdate)
        .disableSelection();

    function sortUpdate(event, ui) {
        var className = $(this).attr("data-type");
        var value = '{"$types":{"KalikoCMS.PropertyType.CollectionProperty`1[[' + className +']], KalikoCMS.Engine":"1"},"$type":"1", "Items": [';

        $(this).children("li").each(function () {
            value += $(this).attr("data-value") + ",";
        });
        value = value.replace(/,$/, "") + "]}";

        $(this).parent().find(".collection-value").val(value);
    }
});