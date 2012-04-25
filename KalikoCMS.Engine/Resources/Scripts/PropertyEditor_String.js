// String Property ======================================================

var webolution_propertytype_stringproperty = {
    onMouseOver: function (target) {
        kQ(target).addClass("hover");
        //kQ(target).css({ 'border': '1px dotted #999999', 'margin': '-1px', 'background': '#eeeeee' });
    },

    onMouseOut: function (target) {
        kQ(target).removeClass("hover");
        //kQ(target).css({ 'border': 'none', 'borderTop': 'none', 'margin': '0px', 'background': 'none' });
    },

    onStartEdit: function (target) {
        kQ.editor.currentProperty = target;
        var _o = kQ(target);

        this.onMouseOut(target);

        kQ(target).bind("paste", function (e) {
            try {
                clipboardData.setData("text", clipboardData.getData("text"));
            }
            catch (e) {
                //var _o = kQ(this);
                //setTimeout(function () { _o.html(_o.text().replace(/\n/g,"<br />")); }, 100);
            }
        });

        kQ.editor.undoBuffer = _o.html();
        target.contentEditable = true;
    },

    onSave: function (target) {
        return kQ(target).text();
    },

    onSaveComplete: function (target) {
        this.onMouseOut(target);
        target.contentEditable = false;
    },

    onCancel: function (target) {
        this.onMouseOut(target);
        target.contentEditable = false;
        kQ(target).html(kQ.editor.undoBuffer);
    }
}
