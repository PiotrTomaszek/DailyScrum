$(document).ready(function () {
    ResizeContentContainer2();
});

window.onresize = ResizeContentContainer2;

function ResizeContentContainer2() {
    var windowHeight = $(window).height();
    var navbarHeight = document.getElementById('navbarHolder').clientHeight;
    var footerHeight = document.getElementById('footerHolder').clientHeight;

    var calculate = ((windowHeight - (navbarHeight + footerHeight + 150)));
    
    var el = document.getElementById('chatHolder');
    el.style.height = (calculate) + "px";
}