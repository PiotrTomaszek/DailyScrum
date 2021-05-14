$(document).ready(function () {
    ResizeContentContainer();
});

window.onresize = ResizeContentContainer;

function ResizeContentContainer() {
    var windowHeight = $(window).height();
    var navbarHeight = document.getElementById('navbarHolder').clientHeight;
    var footerHeight = document.getElementById('footerHolder').clientHeight;

    var calculate = ((windowHeight - (navbarHeight + footerHeight + 100)));
    $("#dailyHolder").innerHeight((calculate) + "px");

}