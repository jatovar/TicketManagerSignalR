let connection = new signalR.HubConnection("/MesaDeTicketsHub");
connection.start().then(function () {
    connection.invoke("ObtieneTickets").then(function (tickets) {
        for (let i = 0; i < tickets.length; i++) {
            console.log(tickets[i]);
        }
    });
});