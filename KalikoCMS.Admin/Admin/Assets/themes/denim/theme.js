$(document).ready(function () {
    var sidebar = $('#sidebar');
    var main = $('#main');
    sidebar.mouseenter(function () {
        sidebar.css('width', '180px');
        sidebar.addClass('expanded');
        main.css('left', '180px');
        main.css('right', '-130px');
    });
    sidebar.mouseleave(function () {
        sidebar.css('width', '50px');
        sidebar.removeClass('expanded');
        main.css('left', '50px');
        main.css('right', '0px');
    });
});
