$(document).ready(function ($) {
    function isOverflown(element) {
        return element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
    }
    var $aboutMe = $(".profil-aboutMe");

    if (isOverflown($(".profil-aboutMe"))) {
        $aboutMe.css('padding-right', '0.3rem');
    }
    $(window).resize(function () {
        if (isOverflown($aboutMe)) {
            $aboutMe.css('padding-right', '0.3rem');
        }
    });
});

