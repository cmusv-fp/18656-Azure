// import the necessary namespace to use hosting functionalities provided by .NET
open Microsoft.Extensions.Hosting
    // initialize a host for the application
let host =
    // create a new host builder to configure and create a host
    HostBuilder()
        // set up the default configurations specific for Azure Functions workers
        .ConfigureFunctionsWorkerDefaults()
        // build and return the configured host
        .Build()

// start and run the configured host. This begins listening for and processing Azure Functions triggers
host.Run()
