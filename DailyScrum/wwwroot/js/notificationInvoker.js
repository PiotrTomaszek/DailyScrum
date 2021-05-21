
connection.on("Notification", function (from , sth) {
    displayNotofication(from, sth);
});


function displayNotofication(whatKind, additionalParameter) {
    //meeting  chyba ok
    //chat oraz problemy

    debugger;

    var actualWindowPage = window.location.pathname;

    if (whatKind === 'daily') {

        if (actualWindowPage === '/') {
            console.log('on meeting');
        } else {
            if (additionalParameter === 'start') {

                generateBell(meetingBellNotify);

            } else {
            }
        }
    }
    else if (whatKind === 'chat') {
        if (actualWindowPage === '/chat') {
            console.log('on chat');
        } else {

        }
    }

}

function generateBell(placeToSpawn) {
    var element = document.getElementById(placeToSpawn);

    if (element.innerHTML == '') {
        var bell = document.createElement('i');
        bell.classList.add('fa');
        bell.classList.add('fa-bell');
        bell.classList.add('faa-ring');
        bell.classList.add('animated');
        element.appendChild(bell)
    }
}
