



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