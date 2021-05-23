connection.on("Notification", function (from, place) {
    displayNotofication(from, place);
});

function displayNotofication(whatKind, place) {

    var actualWindowPage = window.location.pathname;

    if (whatKind === 'daily') {

        if (actualWindowPage === '/') {
            console.log('on meeting');
            connection.invoke("RemoveNotification", 'daily');
        } else {
            generateBell(place);
        }
    }
    else if (whatKind === 'chat') {
        if (actualWindowPage === '/chat') {
            console.log('on chat');
            connection.invoke("RemoveNotification", 'chat');
        } else {
            generateBell(place);
        }
    }
    else if (whatKind === 'problem') {
        if (actualWindowPage === '/problems') {
            console.log('on chat');
            connection.invoke("RemoveNotification", 'problem');
        } else {
            generateBell(place);
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
