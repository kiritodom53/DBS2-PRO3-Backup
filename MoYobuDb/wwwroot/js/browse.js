$(document).ready(function () {
    var $box = $(".img-box");
    $box.hover(
        function () {
            //$(this).children("p").css("visibility", "visible");
            $(this).children("p").animate({ opacity: "1" }, 250);
        },
        function () {
            $(this).children("p").animate({ opacity: "0" }, 250);
        }
    );
});