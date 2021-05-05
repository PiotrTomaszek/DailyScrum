$(document).ready(function () {
  ResizeContentContainer();
});

window.onresize = ResizeContentContainer;

function ResizeContentContainer() {
  var windowHeight = $(window).height();
  var navbarHeight = document.getElementById('navbarHolder').clientHeight;
  //console.log(windowHeight);
  //console.log(navbarHeight);
  $("#chatHolder").innerHeight(((windowHeight - navbarHeight) / 2) + "px");
}