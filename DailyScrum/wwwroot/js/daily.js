"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

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



//connection.on("NotifyJoinedUser", function (user) {
//    var encodedMessage = user + " dolaczyl do spotkania.";
//    var li = document.createElement("li");
//    li.textContent = encodedMessage;
//    li.classList.add("text-center");
//    li.style.fontWeight = "bold";
//    document.getElementById("messagesList").appendChild(li);
//});

connection.on("GetAllUsersStatus", function (test) {
    debugger;
    var tescik = test;
    if (tescik) {

    }
});

connection.on("SetUserStatus", function (userId, isOnline) {
    debugger;

    var element = document.getElementById(`${userId}-icon`);

    if (isOnline) {
        element.classList.remove("offline");
    }
    else {
        element.classList.add("offline");
    }
});

// to jest ok
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
    /*debugger;*/
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});

connection.on("GenerateUserCounter", function (currentNumber, allMembers) {
    /*debugger;*/
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});

connection.on("DisplayTeamName", function (teamname) {
    var element = document.getElementById('team-name');
    element.innerHTML = `${teamname}`
});
