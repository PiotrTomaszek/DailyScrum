"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").disabled = true;
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;

    if (!(message === "")) {
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
    }

    document.getElementById("messageInput").value = '';

    event.preventDefault();
});


connection.on("TestMethod", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});



connection.on("ShowSentMessage", function (user, message, date) {
    var li = document.createElement("li");

    li.innerHTML = ` <div class="chat-hour">${date}</div>
                            <div class="chat-text" style="background-color: grey">
                                ${message}
                            </div>
                            <div class="chat-avatar">`

    li.classList.add('chat-right');

    document.getElementById("messagesList").appendChild(li);
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
});


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


//connection.on("NotifyJoinedUser", function (user) {
//    var encodedMessage = user + " dolaczyl do spotkania.";
//    var li = document.createElement("li");
//    li.textContent = encodedMessage;
//    li.classList.add("text-center");
//    li.style.fontWeight = "bold";
//    document.getElementById("messagesList").appendChild(li);
//});

//connection.on("GetAllUsersStatus", function (userId, online) {
//    debugger;
//    if (online) {
//        var element = document.getElementById(`${userId}-icon`);
//        element.classList.remove("offline");
//    }
//});