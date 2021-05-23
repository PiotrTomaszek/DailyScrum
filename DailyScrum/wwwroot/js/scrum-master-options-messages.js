connection.on("ShowAddMessage", function (text) {
    var element = document.getElementById('addTeamMemberMessage');
    element.innerHTML = '';
    element.innerHTML = text;
});

connection.on("ShowRemoveMessage", function (text) {
    var element = document.getElementById('removeTeamMemberMessage');
    element.innerHTML = '';
    element.innerHTML = text;
});