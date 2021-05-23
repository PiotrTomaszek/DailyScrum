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

document.getElementById("messageInput").addEventListener('keypress', function (e) {
    if (e.key == 'Enter') {
        var message = document.getElementById("messageInput").value;

        if (!(message === "")) {
            connection.invoke("SendMessage", message).catch(function (err) {
                return console.error(err.toString());
            });
        }

        document.getElementById("messageInput").value = '';

        event.preventDefault();
    }
});