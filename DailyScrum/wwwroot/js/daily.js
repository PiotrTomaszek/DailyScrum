"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

//Disable send button until connection is established
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



connection.on("NotifyJoinedUser", function (user) {
    var encodedMessage = user + " dolaczyl do spotkania.";
    var li = document.createElement("li");
    li.textContent = encodedMessage;
    li.classList.add("text-center");
    li.style.fontWeight = "bold";
    document.getElementById("messagesList").appendChild(li);
});


connection.on("UserDisconnected", function (id) {
    var elem = document.getElementById(`${id}`).parentNode
    elem.parentNode.removeChild(elem);
});



connection.on("UserConnected", function (name, email, id, photoPath) {
    var li = document.createElement("li");

    var newElement = `<div class="d-flex">
                            <div class="img_cont">
                                <img src="/avatars/${photoPath}" class="rounded-circle user_img">
                                <span class="online_icon offline"></span>
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
    debugger;
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});

connection.on("GenerateAllUsers", function (currentNumber, allMembers) {
    debugger;
    var element = document.getElementById('online-team-members');
    element.innerHTML = `${currentNumber}/${allMembers}`;
});





connection.on("DisplayTeamName", function (teamname) {
    var element = document.getElementById('team-name');
    element.innerHTML = `${teamname}`
});


//connection.on("AddUserToList", function (name,email,id) {


//});


//connection.on("UserConnected", function (name, email, id, photoPath) {
//    var li = document.createElement("li");

//    var newElement = `<div class="card b-1 hover-shadow mb-20" style="max-height:100px" id="${id}">
//                            <div class="media card-body">
//                                <div class="media-left pr-12">
//                                    <img src="/avatars/${photoPath}" alt="" class="img-thumbnail" style="max-height:50px;width:auto" />
//                                </div>
//                                <div class="media-body text-center">
//                                    <div class="mb-2">
//                                        <span class="fs-20 pr-16">${name}</span>
//                                    </div>
//                                    <small class="fs-16 fw-300 ls-1">${email}</small>
//                                </div>
//                            </div>
//                        </div>`
//    li.innerHTML = newElement;

//    document.getElementById("usersList").appendChild(li);
//});





//function DailyScrumViewModel() {

//    this.chatUsers = ko.observableArray([]);
//    this.joinedRoom = ko.observable("");


//    this.filteredChatUsers = ko.computed(function () {
//        return this.chatUsers();
//    });

//    this.userList = function () {
//        connection.invoke("GetUsers", this.joinedRoom()).then(function (result) {
//            this.chatUsers.removeAll();
//            for (var i = 0; i < result.length; i++) {
//                this.chatUsers.push(new ChatUser(result[i].firstName,

//            }
//        });
//    };

//    function ChatUser(userName, displayName, avatar, currentRoom, device) {
//        var self = this;
//        self.userName = ko.observable(firstName);
//    }

//}