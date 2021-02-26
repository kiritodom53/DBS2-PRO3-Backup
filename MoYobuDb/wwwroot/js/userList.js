$(document).ready(function () {
    
    // $('.quick-info-btn').click(function () {
    //     if ($(".quick-info").css("height") == "0px") {
    //         $('.quick-info').css('height', 'auto');
    //         var autoHeight = $('.quick-info').height();
    //         $('.quick-info').css('height', '0px');
    //         $(".quick-info").animate({height: autoHeight}, 400);
    //         $('.quick-info').css('pointerEvents', 'none');
    //         $('.arrowimg2').toggleClass('flip');
    //     } else {
    //         $(".quick-info").animate({height: "0px"}, 400);
    //         $('.quick-info').css('pointerEvents', 'initial');
    //         $('.arrowimg2').toggleClass('flip');
    //     }
    //     return false;
    // });
    
    function isOverflown(element) {
        return element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
    }

    var els = document.getElementsByClassName('desc-text');

    $(window).resize(function () {
        if (isOverflown(els[0])) {
            $('.open a').css('display', 'block');
        } else {
            $('.open a').css('display', 'none');
        }
    });

    
    $('#list').click(function () {
        $('#overlay').fadeIn(300);
    });
    $('#close').click(function () {
        $('#overlay').fadeOut(300);
    });


    $('select').each(function () {
        var $this = $(this), numberOfOptions = $(this).children('option').length;

        $this.addClass('select-hidden');
        $this.wrap('<div class="select"></div>');
        $this.after('<div class="select-styled"></div>');

        var $styledSelect = $this.next('div.select-styled');
        $styledSelect.text($this.children('option').eq(0).text());

        var $list = $('<ul />', {
            'class': 'select-options',
        }).insertAfter($styledSelect);

        for (var i = 0; i < numberOfOptions; i++) {

            var $lista = $('<a/>', {
                
                href: $this.children('option').eq(i).text(),
            }).appendTo($list);
            
            var $temp = $('<li/>', {
                text: $this.children('option').eq(i).text(),
                rel: $this.children('option').eq(i).val(),
            }).appendTo($lista);
        }

        var $listItems = $list.children('li');

        $styledSelect.click(function (e) {
            e.stopPropagation();
            $('div.select-styled.active').not(this).each(function () {
                $(this).removeClass('active').next('ul.select-options').hide();
            });
            $(this).toggleClass('active').next('ul.select-options').toggle();
        });

        $listItems.click(function (e) {
            e.stopPropagation();
            $styledSelect.text($(this).text()).removeClass('active');
            $this.val($(this).attr('rel'));
            $list.hide();
        });

        $(document).click(function () {
            $styledSelect.removeClass('active');
            $list.hide();
        });
    });

    $(".submit-btn").click(function () {
        $(".form").submit();
    });

    if (isOverflown(els[0])) {
        $('.open').css('display', 'block');
        $('div.description').css('padding', '2em 2em 0.5em 2em');
    }
});