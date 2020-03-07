const liveDan = function (url, group, onMessage) {
    var connection = new signalR.HubConnectionBuilder().withUrl(url).build();
    connection.start().then(function () {
        connection.invoke('Connection', group).catch(err => console.error(err));
    }).catch(err => console.error(err));
    connection.on("ReceiveMessage", function (user, message) {
        onMessage(JSON.parse(message));
    });

    return {
        read: function (options) {
            options.success();
        },
        send: function (options) {
            var mess = options.data;
            connection.invoke('SendMessage', group, "user", JSON.stringify(mess)).catch(err => console.error(err));
            options.success();
        }
    };
}