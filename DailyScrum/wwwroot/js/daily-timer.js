var myVar;

connection.on("DisplayTimer", function (hasStarted, time) {

    var timePlace = document.getElementById('timer');

    debugger;



    if (hasStarted && time > 0) {
        myVar = startTimer(time, timePlace);
    }
    else {
        clearInterval(myVar);
        timePlace.innerHTML = 'Koniec';
    }


});


function startTimer(duration, display) {
    var timer = duration, minutes, seconds;

    return setInterval(function () {
        minutes = parseInt(timer / 60, 10);
        seconds = parseInt(timer % 60, 10);

        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        display.textContent = minutes + ":" + seconds;

        if (--timer < 0) {
            connection.invoke("EndDailyMeeting");
            //timer = duration;
            //a();
        }
    }, 1000);

    //function a() {
    //    clearInterval(myInt)
    //}
}


//function startTimer(duration, display) {
//    var timer = duration, minutes, seconds;

//    var myInt = setInterval(function () {
//        minutes = parseInt(timer / 60, 10);
//        seconds = parseInt(timer % 60, 10);

//        minutes = minutes < 10 ? "0" + minutes : minutes;
//        seconds = seconds < 10 ? "0" + seconds : seconds;

//        display.textContent = minutes + ":" + seconds;

//        if (--timer < 0) {
//            connection.invoke("EndDailyMeeting");
//            //timer = duration;
//            a();
//        }
//    }, 1000);

//    function a() {
//        clearInterval(myInt)
//    }
//}