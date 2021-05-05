"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("TestMethod", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("NotifyJoinedUser", function (user) {
    var encodedMessage = user + " dolaczyl do spotkania.";
    var li = document.createElement("li");
    li.textContent = encodedMessage;
    li.classList.add("text-center");
    document.getElementById("messagesList").appendChild(li);
});



connection.on("UserDisconnected", function (id) {
    var elem = document.getElementById(`${id}`).parentNode
    elem.parentNode.removeChild(elem);
});



connection.on("UserConnected", function (name, email, id, photoPath) {
    var li = document.createElement("li");

    var newElement = `<div class="card b-1 hover-shadow mb-20" style="max-height:100px" id="${id}">
                            <div class="media card-body">
                                <div class="media-left pr-12">
                                    <img src="/avatars/${photoPath}" alt="" class="img-thumbnail" style="max-height:50px;width:auto" />
                                </div>
                                <div class="media-body">
                                    <div class="mb-2">
                                        <span class="fs-20 pr-16">${name}</span>
                                    </div>
                                    <small class="fs-16 fw-300 ls-1">${email}</small>
                                </div>
                            </div>
                        </div>`
    li.innerHTML = newElement;

    document.getElementById("usersList").appendChild(li);
});





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