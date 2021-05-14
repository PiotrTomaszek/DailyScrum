"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

document.getElementById('submitDailyPost').disabled = true;

connection.start().then(function () {
    /*document.getElementById("sendButton").disabled = false;*/
}).catch(function (err) {
    return console.error(err.toString());
});

/*document.getElementById("sendButton").disabled = true;*/



connection.on("TestMethod", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("EnableSubmitPostButton", function () {
    debugger;
    var element = document.getElementById('submitDailyPost');
    element.disabled = false;
});

connection.on("SendDailyPost", function (name, yesterday, today, problem, time, id) {
    var place = document.getElementById('dailyPostPlace');

    var newDiv = document.createElement('div');
    newDiv.classList.add('p-card');
    newDiv.classList.add('bg-white');
    newDiv.classList.add('p-2');
    newDiv.classList.add('px-3');
    newDiv.classList.add('rounded');
    newDiv.classList.add('mb-1');

    newDiv.innerHTML = `<div class="row">
                        <div class="col-2 col-md-1 pt-3">
                            <img src="/avatars/testphoto.jpg" alt="" class="rounded-circle border border-dark" style="width:40px;height:40px" />
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
                                <h6>Co zrobiles wczoraj?</h6>
                                <p>${yesterday}</p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-1 offset-1">
                                <div class="daily-today-line"></div>
                            </div>
                            <div class="col-8">
                                <h6>Co zrobisz dzisiaj?</h6>
                                <p>${today}</p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-1 offset-1">
                                <div class="daily-problem-line"></div>
                            </div>
                            <div class="col-8">
                                <h6>Widzisz jakies problemy?</h6>
                                <p>${problem}</p>
                            </div>
                        </div>
                    </div>`

    place.appendChild(newDiv)
});




// to jest ok ale do refactor
connection.on("ShowSentMessage", function (user, message, date) {
    var li = document.createElement("li");

    li.innerHTML = ` <div class="chat-hour">${date}</div>
                            <div class="chat-text" style="background-color: grey">
                                ${message}
                            </div>
                            <div class="chat-avatar">`

    li.classList.add('chat-right');

    document.getElementById("messagesList").appendChild(li);

    scrollToBottom();

});

connection.on("SendMessageToGroup", function (user, message, date) {
    var li = document.createElement("li");

    li.innerHTML = `<div class="chat-avatar">
                                <img src="/avatars/testphoto.jpg" alt="${user}">
                                <div class="chat-name">${user}</div>
                            </div>
                            <div class="chat-text" style="background-color: coral">
                                ${message}
                            </div>
                            <div class="chat-hour">${date}</div>`;

    li.classList.add('chat-left');

    document.getElementById("messagesList").appendChild(li);

    scrollToBottom();
});

function scrollToBottom() {
    var scroller = document.getElementById('chatHolder');

    scroller.scrollTop = scroller.scrollHeight - scroller.clientHeight
}


// to jest ok
connection.on("SetUserStatus", function (userId, isOnline) {
    var element = document.getElementById(`${userId}-icon`);

    if (isOnline) {
        element.classList.remove("offline");
    }
    else {
        element.classList.add("offline");
    }
});

connection.on("UserConnected", function (name, email, id, photoPath) {
    var li = document.createElement("li");

    var newElement = `<div class="d-flex">
                            <div class="img_cont">
                                <img src="/avatars/${photoPath}" class="rounded-circle user_img">
                                <span id="${id}-icon" class="online_icon offline"></span>
                            </div>
                            <div class="user_info">
                                <span>${name}</span>
                                <p></p>
                            </div>
                         </div>`

    li.innerHTML = newElement;
    li.id = id;
    document.getElementById("usersList").appendChild(li);
});

connection.on("UpdateUserList", function (currentNumber, allMembers) {
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});

connection.on("GenerateUserCounter", function (currentNumber, allMembers) {
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});

connection.on("DisplayTeamName", function (teamname) {
    var element = document.getElementById('team-name');
    element.innerHTML = `${teamname}`
});