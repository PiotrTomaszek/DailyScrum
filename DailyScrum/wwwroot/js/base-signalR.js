"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/daily").build();

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
