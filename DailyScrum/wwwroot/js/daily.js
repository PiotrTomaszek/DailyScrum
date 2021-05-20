connection.on("NotifyUsers", function (param1, param2) {
    debugger;

    displayNotofication(param1, param2);
});


//connection.on("DisplayTime", function (time) {
//    var element = document.getElementById('starting-time');
//    element.innerHTML = time;
//})

//connection.on("TestMethod", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

connection.on("StartDaily", function () {
    alert('daily has started');

    connection.invoke("AddNotification", "Daily");



    //displayNotofication('daily', 'start');
});

connection.on("EndDaily", function () {
    alert('daily has ended');


    connection.invoke("AddNotification", "Daily");

    /*displayNotofication('daily', 'end');*/
});

connection.on("EnabledOptions", function (isStarted, roleMember) {
    if (isStarted) {

        if (roleMember === "Scrum Master") {

            var startBtn = document.getElementById('startMeetingHolder');
            var endBtn = document.getElementById('endMeetingHolder');

            // wylaczanie przyciskow
            startBtn.classList.add('disabled');
            endBtn.classList.remove('disabled');

            // zmiana wygladu

            startBtn.classList.add('text-muted');
            endBtn.classList.remove('text-muted');

        } else {
            var addPost = document.getElementById('modalHolder');

            addPost.classList.remove('disabled');;

            addPost.classList.remove('text-muted');
        }

    } else {
        if (roleMember === "Scrum Master") {

            var startBtn = document.getElementById('startMeetingHolder');
            var endBtn = document.getElementById('endMeetingHolder');

            // wylaczanie
            startBtn.classList.remove('disabled');
            endBtn.classList.add('disabled');

            //  wyglad
            startBtn.classList.remove('text-muted');
            endBtn.classList.add('text-muted');

        } else {

            var addPost = document.getElementById('modalHolder');

            // wylaczanie
            addPost.classList.add('disabled');

            // wyglad
            addPost.classList.add('text-muted');
        }
    }
});

// chyba jest ok
connection.on("GenDevOptions", function () {
    var divContainer = document.getElementById('dropdownDailyOptions');

    var devOption = document.createElement('a');
    devOption.classList.add('dropdown-item');
    devOption.classList.add('text-center');
    devOption.classList.add('disabled');
    devOption.id = 'modalHolder';
    devOption.dataset.toggle = 'modal';
    devOption.dataset.target = '#exampleModal';
    devOption.innerHTML = "Dodaj post";

    divContainer.appendChild(devOption);
});


// chyba jest ok
connection.on("GenScrumMasterOptions", function () {
    var divContainer = document.getElementById('dropdownDailyOptions');

    var smOption = document.createElement('a');
    smOption.classList.add('dropdown-item');
    smOption.classList.add('text-center');
    smOption.id = 'startMeetingHolder';
    smOption.dataset.toggle = 'modal';
    smOption.dataset.target = '#startMeetingModal';
    smOption.innerHTML = "Rozpocznij spotkanie";

    divContainer.appendChild(smOption);

    //do poprawy brak modal
    var smOption2 = document.createElement('a');
    smOption2.classList.add('dropdown-item');
    smOption2.classList.add('text-center');
    smOption2.classList.add('disabled');
    smOption2.classList.add('text-muted'); // do poprawy
    smOption2.id = 'endMeetingHolder';
    smOption2.dataset.toggle = 'modal';
    smOption2.dataset.target = '#endMeetingModal';
    smOption2.innerHTML = "Koniec spotkania";

    divContainer.appendChild(smOption2);

    // do sprawdzania starych spotkañ

    var archiveManager = document.createElement('a');
    archiveManager.classList.add('dropdown-item');
    archiveManager.classList.add('text-center');
    archiveManager.style.color = 'black';
    archiveManager.id = 'meetingArchiveManager';
    archiveManager.innerHTML = "Archiwum Daily";
    archiveManager.href = '/meetingsarchive';

    divContainer.appendChild(archiveManager);
});

// nie do konca spoko ale narazie dziala
connection.on("EnableSubmitPostButton", function (hasStarted) {

    var element = document.getElementById('submitDailyPost');

    if (hasStarted) {
        element.disabled = false;
    } else {
        element.disabled = true;
    }
});

connection.on("SendDailyPost", function (name, yesterday, today, problem, time, id, photopath) {

    var place = document.getElementById('dailyPostPlace');

    var newDiv = document.createElement('div');
    newDiv.classList.add('p-card');
    newDiv.classList.add('bg-white');
    newDiv.classList.add('p-2');
    newDiv.classList.add('px-3');
    newDiv.classList.add('rounded');
    newDiv.classList.add('mb-1');
    newDiv.classList.add('border');
    newDiv.classList.add('border-dark');

    newDiv.innerHTML = `<div class="row">
                        <div class="col-2 col-md-1 pt-3">
                            <img src="/avatars/${photopath}" alt="" class="rounded-circle border border-dark" style="width:40px;height:40px" />
                        </div>
                        <div class="col-8 col-md-9 text-left">
                            <p class="mt-2">${time}</p>
                            <p><strong>${name}</strong> dodal nowy post do spotkania daily!</p>
                        </div>
                        <div class="col-2 col-md-2">
                            <button class="btn text-center" type="button" data-toggle="collapse" data-target="#collapseUser-${id}" aria-expanded="false" aria-controls="collapseExample">
                                <i class="fas fa-plus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="collapse pt-2" id="collapseUser-${id}" style="background-color:cornsilk">
                        <div class="row">
                            <div class="col-1 offset-1">
                                <div class="daily-yesterday-line"></div>
                            </div>
                            <div class="col-8">
                                <h6><strong>Co zrobiles wczoraj?</strong></h6>
                                <p>${yesterday}</p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-1 offset-1">
                                <div class="daily-today-line"></div>
                            </div>
                            <div class="col-8">
                                <h6><strong>Co zrobisz dzisiaj?</strong></h6>
                                <p>${today}</p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-1 offset-1">
                                <div class="daily-problem-line"></div>
                            </div>
                            <div class="col-8">
                                <h6><strong>Widzisz jakies problemy?</strong></h6>
                                <p>${problem}</p>
                            </div>
                        </div>
                    </div>`;

    place.appendChild(newDiv)
});


// to jest ok ale do refactor
connection.on("ShowSentMessage", function (user, message, date) {
    var li = document.createElement("li");

    li.innerHTML = ` <div class="chat-hour">${date}</div>
                            <div class="chat-text" style="background-color: lightgrey">
                                ${message}
                            </div>
                            <div class="chat-avatar">`

    li.classList.add('chat-right');

    document.getElementById("messagesList").appendChild(li);

    scrollToBottom();

});

connection.on("SendMessageToGroup", function (user, message, date, imgPath) {
    var li = document.createElement("li");

    li.innerHTML = `<div class="chat-avatar">
                                <img src="/avatars/${imgPath}" alt="${user}">
                                <div class="chat-name">${user}</div>
                            </div>
                            <div class="chat-text" style="background-color: bisque">
                                ${message}
                            </div>
                            <div class="chat-hour">${date}</div>`;

    li.classList.add('chat-left');

    document.getElementById("messagesList").appendChild(li);

    scrollToBottom();
});

// to jest ok
function scrollToBottom() {
    var scroller = document.getElementById('chatHolder');

    scroller.scrollTop = scroller.scrollHeight - scroller.clientHeight
}

connection.on("SetUserStatus", function (userId, isOnline) {
    var element = document.getElementById(`${userId}-icon`);

    if (isOnline) {
        element.classList.remove("offline");
    }
    else {
        element.classList.add("offline");
    }
});

connection.on("UserConnected", function (name, email, id, photoPath, role) {
    var li = document.createElement("li");

    var newElement = `<div class="d-flex">
                            <div class="img_cont">
                                <img src="/avatars/${photoPath}" class="rounded-circle user_img">
                                <span id="${id}-icon" class="online_icon offline"></span>
                            </div>
                            <div class="user_info">
                                <span>${name}</span>
                                <p class="text-left">${role}</p>
                            </div>
                         </div>`

    li.innerHTML = newElement;
    li.id = id;
    document.getElementById("usersList").appendChild(li);
});

connection.on("UpdateUserList", function (currentNumber, allMembers) {
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;

    var element = document.getElementById('online-team-members2');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});

connection.on("GenerateUserCounter", function (currentNumber, allMembers) {
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;

    var element = document.getElementById('online-team-members2');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});

connection.on("DisplayTeamName", function (teamname) {
    var element = document.getElementById('team-name');
    element.innerHTML = `${teamname}`
});