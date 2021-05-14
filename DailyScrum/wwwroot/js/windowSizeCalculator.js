$(document).ready(function () {
    ResizeContentContainer();
});

window.onresize = ResizeContentContainer;

function ResizeContentContainer() {
    var windowHeight = $(window).height();
    var navbarHeight = document.getElementById('navbarHolder').clientHeight;
    var footerHeight = document.getElementById('footerHolder').clientHeight;

    var calculate = ((windowHeight - (navbarHeight + footerHeight + 10)));
    console.log("height = " + (calculate / 2))
    $("#dailyHolder").innerHeight((calculate / 2) + "px");

    var el = document.getElementById('chatHolder');
    el.style.height = (calculate / 2.8) + "px";

    var el2 = document.getElementById('usersHolder');
    el.style.height = (calculate / 2.8) + "px";

    var head = document.getElementById('cardHeaderHolder').clientHeight;

    var el3 = document.getElementById('cardBodyHolder');
    el3.style.height = ((calculate / 2.8) - head) + "px";

}