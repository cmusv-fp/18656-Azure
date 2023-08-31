// declare a new namespace for the function
namespace SBQueueReceiver.Function

// import necessary namespaces:
// - Functions.Worker provides tools for creating Azure Functions
// - Extensions.Logging allows us to log diagnostic information
// - Messaging.ServiceBus provides tools for interacting with Azure Service Bus
open Microsoft.Azure.Functions.Worker
open Microsoft.Extensions.Logging
open Azure.Messaging.ServiceBus

// define a new module called SBQueueReceiver
module SBQueueReceiver = 

    // annotate the function to tell Azure that this is an Azure Function named "SBQueueReceiver"
    [<Function("SBQueueReceiver")>]
    // specify that this function will output to a Service Bus Queue named "outqueue" after processing
    [<ServiceBusOutput("outqueue", Connection = "prototypicalfintechapp_SERVICEBUS")>]
    // define the `Run` function which will be triggered when a message arrives in the Service Bus Queue named "inqueue"
    let Run ([<ServiceBusTrigger("inqueue", Connection = "prototypicalfintechapp_SERVICEBUS")>] message: ServiceBusReceivedMessage, context: FunctionContext, output: byref<ServiceBusMessage>) =
        // get a logger from the function's context
        let log = context.GetLogger("SBQueueReceiver")
        // log the info of the received Service Bus message
        log.LogInformation $"Message ID: {message.MessageId}"
        log.LogInformation $"Message Body: {message.Body}"
        // set the message body of the output message to be sent to the "outqueue"
        output <- ServiceBusMessage "message from Service Bus output binding"
        // return the message body explictitly
        // effectively, this will send the message to the "outqueue"
        output
