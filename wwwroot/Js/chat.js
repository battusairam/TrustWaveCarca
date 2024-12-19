namespace TrustWaveCarca.wwwroot.Js
{
    // chat.js
    // Ensure SignalR connection is made when the page loads
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub?userId=" + userId)  // Pass the userId as a query parameter
        .build();

    // Handle user connection notification
    connection.on("UserConnected", (userId) => {
        console.log(`${userId} has connected`);
    });

    // Handle user disconnection notification
    connection.on("UserDisconnected", (userId) => {
        console.log(`${userId} has disconnected`);
    });

    // Handle receiving a message
    connection.on("ReceiveMessage", (message) => {
        console.log("Received message: ", message);
        // Process received message (display it, etc.)
        displayMessage(message);  // Assuming there's a function to display the message
    });

    // Handle message status updates (e.g., delivered, read)
    connection.on("UpdateMessageStatus", (message) => {
        console.log("Updated message status: ", message);
        // Update message status (e.g., "Delivered", "Read")
        updateMessageStatus(message);  // Assuming there's a function to update message status
    });

    // Start the connection
    connection.start()
        .then(() => {
            console.log("SignalR connection established.");
        })
        .catch(err => {
            console.error("Error while starting connection: ", err);
        });

    // Send a message to another user
    function sendMessage(toUserId, messageContent) {
        connection.invoke("SendMessage", toUserId, messageContent)
            .catch(err => {
                console.error("Error sending message: ", err);
            });
    }

    // Update message display status (for example)
    function displayMessage(message) {
        // Your logic to display the message
        console.log("Displaying message: ", message.Content);
    }

    // Update message status (Delivered, Read, etc.)
    function updateMessageStatus(message) {
        // Your logic to update the message status
        console.log(`Message ${message.Id} status updated to: ${message.Status}`);
    }

}
