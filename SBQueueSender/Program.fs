open System
open Azure.Messaging.ServiceBus
open Azure.Identity

// number of messages to be sent to the queue
let numOfMessages = 1

let sendMessageBatchAsync(namespaceName : string, queueName : string) =
    let fullyQualifiedNamespce = namespaceName + ".servicebus.windows.net"
    async {
        // the client that owns the connection and can be used to create senders and receivers
        let client = ServiceBusClient(fullyQualifiedNamespce, DefaultAzureCredential())
        // the sender used to publish messages to the queue
        let sender = client.CreateSender(queueName)
        // The Service Bus client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when messages are being published or read
        // regularly.

        // create a batch 
        let! messageBatch = sender.CreateMessageBatchAsync().AsTask() |> Async.AwaitTask

        // Loop to add messages to the batch
        for i = 1 to numOfMessages do
            // try adding a message to the batch
            if not (messageBatch.TryAddMessage(ServiceBusMessage (sprintf "Message %d" i))) then
                // if it is too large for the batch
                failwith (sprintf "The message %d is too large to fit in the batch." i)

        // Sending the batch of messages
        try
            // Use the producer client to send the batch of messages to the Service Bus queue
            sender.SendMessagesAsync(messageBatch) |> Async.AwaitTask |> ignore
            Console.WriteLine(sprintf "A batch of %d messages has been published to the queue." numOfMessages)
        finally
            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            sender.DisposeAsync() |> ignore
            client.DisposeAsync() |> ignore
    }

// Running the async function to send messages to the queue
sendMessageBatchAsync("prototypicalfintechapp", "inqueue") |> Async.RunSynchronously
