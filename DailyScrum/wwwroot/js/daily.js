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
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


////$(document).ready(function () {
////    var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

////    connection.start().then(function () {
////        console.log('SignalR Started...')
////        setTimeout(function () {
////            if (viewModel.chatRooms().length > 0) {
////                viewModel.joinRoom(viewModel.chatRooms()[0]);
////            }
////        }, 250);
////    }).catch(function (err) {
////        return console.error(err);
////    });

////    connection.on("TestMethod", function (message) {
////        console.log(message);
////        alert(message);
////    });

////});