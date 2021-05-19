"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

connection.start().then(function () {
    /*document.getElementById("sendButton").disabled = false;*/
}).catch(function (err) {
    return console.error(err.toString());
});
/*document.getElementById("sendButton").disabled = true;*/
