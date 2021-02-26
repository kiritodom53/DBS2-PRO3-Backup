jQuery(document).ready(function ($) {
    var jssor_1_options = {
        $AutoPlaySteps: 5,
        $SlideDuration: 160,
        $Loop: 0,
        $SlideWidth: 100,
        $SlideSpacing: 5,
        $Align: 0,
        $ArrowNavigatorOptions: {
            $Class: $JssorArrowNavigator$,
            $Steps: 3
        }
    };

    var jssor_1_slider = new $JssorSlider$("jssor_1", jssor_1_options);
    var jssor_2_slider = new $JssorSlider$("jssor_2", jssor_1_options);
    var jssor_4_slider = new $JssorSlider$("jssor_4", jssor_1_options);

    /*#region responsive code begin*/

    var MAX_WIDTH = 625;

    function ScaleSlider() {
        var containerElement = jssor_1_slider.$Elmt.parentNode;
        var containerWidth = containerElement.clientWidth;

        if (containerWidth) {

            var expectedWidth = Math.min(MAX_WIDTH || containerWidth, containerWidth);

            jssor_1_slider.$ScaleWidth(expectedWidth);
            jssor_2_slider.$ScaleWidth(expectedWidth);
            jssor_4_slider.$ScaleWidth(expectedWidth);

        } else {
            window.setTimeout(ScaleSlider, 30);
        }
    }

    ScaleSlider();

    $(window).bind("load", ScaleSlider);
    $(window).bind("resize", ScaleSlider);
    $(window).bind("orientationchange", ScaleSlider);
    /*#endregion responsive code end*/
    
});