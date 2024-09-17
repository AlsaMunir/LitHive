"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
connection.start();
connection.on("Msg", function (bookTitle) {
    const banner = document.getElementById("notificationBanner");
    const message = document.getElementById("notificationMessage");
    message.textContent = `New book added:${bookTitle }`;
    banner.style.display = "block";
});