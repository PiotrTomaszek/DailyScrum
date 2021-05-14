$(document).ready(function () {
    ResizeContentContainer2();
});

window.onresize = ResizeContentContainer2;

function ResizeContentContainer2() {
    var windowHeight = $(window).height();
    var navbarHeight = document.getElementById('navbarHolder').clientHeight;
    var footerHeight = document.getElementById('footerHolder').clientHeight;

    var calculate = ((windowHeight - (navbarHeight + footerHeight + 100)));
    console.log("height = " + (calculate ))
    
    var el = document.getElementById('chatHolder');
    el.style.height = (calculate) + "px";

    var el2 = document.getElementById('usersHolder');
    el.style.height = (calculate) + "px";

    var head = document.getElementById('cardHeaderHolder').clientHeight;

    var el3 = document.getElementById('cardBodyHolder');
    el3.style.height = ((calculate ) - head) + "px";

}