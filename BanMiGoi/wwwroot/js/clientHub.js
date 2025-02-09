﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ConnectedHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");

    // Check if the user is "Admin"
    if (user === "Admin") {
        li.classList.add("sender"); // Apply receiver style for messages from other users
    } else {
        li.classList.add("receiver"); // Apply sender style for messages from the current user
    }

    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} : ${message}`;
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
