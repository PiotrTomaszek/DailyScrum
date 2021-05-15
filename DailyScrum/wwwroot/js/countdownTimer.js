
var el = document.getElementById('starting-time')

// tutaj nic nie dziala

var month = new Array();
month[0] = "January";
month[1] = "February";
month[2] = "March";
month[3] = "April";
month[4] = "May";
month[5] = "June";
month[6] = "July";
month[7] = "August";
month[8] = "September";
month[9] = "October";
month[10] = "November";
month[11] = "December";


var today = new Date();
var dat = today.getDate();
var test = `${(month[today.getMonth()])} ${dat}, ${today.getFullYear()} ${el.innerHTML.toString()}`;
var countDownDate = new Date().getTime();

var x = setInterval(function () {
debugger;

    var now = new Date().getTime();

    var distance = countDownDate - now;

    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

    document.getElementById("timer").innerHTML = minutes + ":" + seconds;

    if (distance < 0) {
        clearInterval(x);
        document.getElementById("timer").innerHTML = "Koniec";
    }
}, 1000);