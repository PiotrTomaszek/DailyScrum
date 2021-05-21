
connection.on("Notification", function (from, place) {

    debugger;

    displayNotofication(from, place);
});


function displayNotofication(whatKind, place) {
    //meeting  chyba ok
    //chat oraz problemy

    debugger;

    var actualWindowPage = window.location.pathname;

    if (whatKind === 'daily') {

        if (actualWindowPage === '/') {
            console.log('on meeting');
            connection.invoke("RemoveNotification", 'Daily');
        } else {
            generateBell(place);
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
