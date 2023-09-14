# Ellucian Ethos Integration SDK
> Modifications by Intuition Payment Solutions

![Azure DevOps Release Pipeline Status](https://vsrm.dev.azure.com/intuitionps/_apis/public/Release/badge/01c84548-9607-4655-80e7-6ad95390a38c/18/46)

[![Build and Publish to Azure Artifacts / GitHub Packages](https://github.com/iNtuitionPS/ethos-integration-sdk-fork/actions/workflows/publish-nupkg.yml/badge.svg)](https://github.com/iNtuitionPS/ethos-integration-sdk-fork/actions/workflows/publish-nupkg.yml)

| Branch | Build Pipeline Status |
|:---|:---|
| `main` | <img align="center" src="https://dev.azure.com/intuitionps/iNtuition%20API%20Service%20Environment/_apis/build/status%2FBuild%20Pipeline%20-%20CI%20%5B%20Ethos%20Integration%20SDK%20%5D?branchName=main"> |
| `developmment` | N/A |

#

Ethos Integration SDK provides utilities and libraries that make it easier for developers to quickly start building Ethos-based integrations.

The Ethos Integration SDK for .NET allows you to easily develop applications in C# that integrate with Ellucian Ethos Integration. The SDK
builds and executes HTTP requests and manages the responses. This allows your application to use the C# library methods to communicate
with Ethos Integration, without the need to call the REST APIs directly.

The Ethos Integration SDK for .NET simplifies use of Ethos Integration by providing a set of libraries that .NET developers are familiar with.
The Ethos Integration SDK makes the application development process less expensive and more efficient.

## Table of contents

1. [Setup](#setup)
1. [Quick Start](#quick-start)
1. [Full API Doc](#full-api-documentation)
1. [Examples](#examples)

# Setup

This SDK is available for download from the [nuget repository](https://www.nuget.org/packages/Ellucian.Ethos.Integration/).

Before using the SDK, you will need to download and install the following required software:

* A .NET runtime. The SDK was built in .NET Core 6.0.301
* nuget

You will need an API key from an Ethos Integration application. It is expected that the application that the API key belongs to is already
configured properly in Ethos Integration.  Please refer to Ellucian documentation for more information about how to get an API key and configure
Ethos Integration applications.

We also recommend:

* A C# IDE such as
  * [Visual Studio](https://visualstudio.microsoft.com/)
  * [Visual Studio Code](https://code.visualstudio.com/docs/languages/csharp)
  * [JetBrains Rider](https://www.jetbrains.com/rider//)
* [.NET Core](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/install) - if you're on Mac or Linux or just really like the commandline on Windows.

In general your choice of C# IDE will work fine as long as you can do manage nuget with it.

# Quick Start

To make API requests against the Ethos Integration services, you will first need to create a client object.  To create a client
object, use the **EthosClientBuilder**, found in the `Ellucian.Ethos.Integration.Client` namespace.

The types of clients that you can create are as follows:

* `EthosConfigurationClient` - make requests for getting config data, such as /appConfig and /available-resources
* `EthosErrorsClient` - perform create, read, and delete operations against the EI errors service
* `EthosMessagesClient` - get messages from a subscriber queue
* `EthosProxyClient` - perform CRUD operations for Ethos Data Models and other resources using the EI proxy API

To use the EthosClientBuilder, you will need to create a new instance of it, passing a valid Ethos Integration API key to the
constructor.  The API key is used to get an access token from Ethos Integration, allowing the client to authenticate to the EI
services when making requests.

Here is a quick example of creating a proxy client and making a simple GET request for the 'courses' resource.  See the
[examples](#examples) section for more detailed code examples.

One thing to note is that all of these clients return `Task` types, and are `awaitable`.  The examples cover how to handle this in the most common, easiest to use, typical way: using an `async` method and `await`ing the result.

```csharp
using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Proxy;

// Create Proxy Client
string apiKey = "11111111-1111-1111-1111-111111111111";
EthosProxyClient proxyClient = new EthosClientBuilder(apiKey).BuildEthosProxyClient();
EthosResponse response = await proxyClient.GetAsync("courses");
```

The `EthosResponse` object is returned from many of the methods in the proxy client.  It holds information about the HTTP response,
such as the content (response body), response headers, and status code.  It also holds a `requestedUrl` property that can show you
the URL that was built by the client and used to make the request.

There are overloaded methods in the `EthosProxyClient` that let you get the response as a `string` or as a `Newtonsoft.Json.Linq.JObject` object.
These response types contain only the content portion of the `EthosResponse`.

```csharp
// get as string.  string.Empty tells the SDK to get the latest version.
string response = await proxyClient.GetAsStringAsync​("persons", string.Empty);

// get as JArray.  string.Empty tells the SDK to get the latest version.
JArray response = await proxyClient.GetAsJArrayAsync("persons", string.Empty);
```

The [Json.NET](https://www.newtonsoft.com/json) is heavily used in this SDK to manage serializing and parsing
JSON objects.  The `JObject`, `JToken` and `JArray` objects make it easy to read the JSON properties of the responses.  It provides a way to generically
manage all the Ethos Data Models and other resource types that could be returned from Ethos Integration without having to create and
manage C# objects for every possible one.

Here is an example of getting properties from a list of persons, then printing out the `id` and `fullName` attribute.

```csharp
    JArray persons = await proxyClient.GetAsJArrayAsync("persons", string.Empty);
    foreach (JToken person in persons)
    {
        string id = person["id"].Tostring();
        // a person can have more than one name, but we just want the first one for simplicity.
        string fullName = person["names"][0]["fullName"].Tostring();
        Console.WriteLine($"{fullName} has a person ID of {id}");
    }
```

The proxy client is different than the other clients because the response objects could be in any JSON format that the authoritative
applications return.  This is why it is necessary to handle the responses as strings or generic JSON nodes.

The other clients, such as `EthosMessagesClient` and `EthosErrorsClient`, return more specialized objects since the responses
have defined JSON schemas.  They return `EthosError` and `ChangeNotification` objects.  See the full API doc for more details.

# Full API Documentation

The full SDK API for the C# SDK is hosted on our [Github Pages site](https://ellucian-developer.github.io/integration-sdk-doc/csharp/api/Ellucian.Ethos.Integration.html). This site uses the DocFX tool to generate API documentation.

# Examples

The following are the code-snippet examples of how to use the Ethos Integration SDK for C#. For more in-depth examples
please refer to the [integration SDK C# example project](https://github.com/ellucian-developer/integration-sdk-csharp-examples) in Github.

For example:
To start the project, run following command from the command prompt

`dotnet run "11111111-1111-1111-1111-111111111111"`

or alternatively you can run it from Visual Studio menu Debug --> Start Debugging (F5 command key)/Start Without Debugging (Ctrl+F5) or

That will run the examples specified in the `SDKExamples.cs` file.

### Making Requests to the Proxy API

Get a page of 'courses' resources using a specific version.

```csharp
EthosProxyClient proxyClient = new EthosClientBuilder(apiKey).BuildEthosProxyClient();
EthosResponse response = await proxyClient.GetAsync("courses", "application/vnd.hedtech.integration.v16.1.0+json");
```

Get a page of 'persons' resources for a major version.

```csharp
// get the full version header for v12 of 'persons'
EthosConfigurationClient configClient = new EthosClientBuilder(apiKey).BuildEthosConfigurationClient();
string version = await configClient.GetVersionHeader​Async("persons", 12);

EthosResponse response = await proxyClient.GetAsync("persons", version);
```

Get a single 'employees' row using an ID.

```csharp
EthosResponse response = await proxyClient.GetByIdAsync("employees", "11111111-1111-1111-1111-111111111111");
```

Create a new record with a POST request.

```csharp
EthosResponse response = await proxyClient.PostAsync("colors", "{ \"id\":\"00000000-0000-0000-0000-000000000000\", \"name\": \"green\" }");
```

Update a record with a PUT request.

```csharp
EthosResponse response = await proxyClient.PutAsync("colors", "11111111-1111-1111-1111-111111111111", "{ \"name\": \"forest green\" }");
```

Delete a record with a DELETE request.

```csharp
await proxyClient.DeleteAsync("colors", "11111111-1111-1111-1111-111111111111");
```

Make requests that use paging.  This will handle sending multiple HTTP requests to get multiple pages of data with a single operation.  
**Use caution when trying to get all pages of a resource in a single request.  Depending on the resource, this could cause a long
running operation that will cause a timeout, or it could return a huge amount of data that could cause an out of memory error.**

```csharp
// get all the pages of data for the 'buildings' resource using the default page size
var ethosResponseList = await proxyClient.GetAllPagesAsync( "buildings");

// get the max page size for the 'persons' resource
int pageSize = proxyClient.GetMaxPageSizeAsync( "persons" );

// get 5 pages of persons data using the default version and maximum allowed page size
// return the data as a list of JArray objects
var jsonNodeList = await proxyClient.GetPagesAsJArraysAsync( "persons", "", pageSize, 5 );

// get 2 pages of courses data starting from an offset of 100, using the default version and page size
// return the data as a list of Strings
var stringList = await proxyClient.GetPagesFromOffsetAsStringsAsync( "courses", "", 0, 100, 2 );
```

### Making Criteria Filter Requests to the Proxy API

Requests using criteria filters can be made using the EthosFilterQueryClient. SimpleCriteria can be built using the CriteriaFilter
WithSimpleCriteria or WithArray methods. Once built, you call BuildCriteria or BuildNamedQuery method to get a string representation of the filter.
You can also directly pass CriteriaFilter or NamedQueryFilter to methods in EthosFilterQueryClient. This is covered in the documentation and example code.
Some knowledge of the desired criteria filter syntax is needed for use with the given Ethos resource. The following is a brief example code snippet.

```csharp
//Creates ?criteria={"names":{"firstName":"John","lastName":"Smith"}}
string resource = "persons";
string version = "application/vnd.hedtech.integration.v12.3.0+json";
string criteriaFilterStr = new CriteriaFilter().WithArray( "names", 
                           new CriteriaFilter().WithSimpleCriteria( "firstName", "John" )
                                               .WithSimpleCriteria( "lastName", "smith" ) ).BuildCriteria();
Console.WriteLine( criteriaFilterStr );
EthosFilterQueryClient ethosFilterQueryClient = GetEthosFilterQueryClient();
try
{
    EthosResponse ethosResponse = await ethosFilterQueryClient.GetWithCriteriaFilterAsync( resource, version, criteriaFilterStr );
    Console.WriteLine( $"REQUESTED URL: { ethosResponse.RequestedUrl }" );
    Console.WriteLine( $"Number of resources returned: { ethosResponse.ContentAsJson.Count }\r\n" );
    Console.WriteLine( ethosResponse.ContentAsJson.ToString() );
}
catch ( Exception e )
{
    Console.WriteLine( e.Message );
}
```

Examples of using a named query filter and/or filter map are also available in the example code.

### Consuming Subscriber Messages

Call the consume endpoint to get the default number of new messages from your application's subscriber queue.  The messages are
returned as ChangeNotification objects.

```csharp
EthosMessagesClient messagesClient = new EthosClientBuilder(apiKey).GetEthosMessagesClient();
IEnumerable<ChangeNotification> changeNotifications = await messagesClient.ConsumeAsync();
```

Call the consume endpoint to get available messages from your application's subscriber queue, starting after message ID of '10'.

```csharp
IEnumerable<ChangeNotification> changeNotifications = await messagesClient.Consume(10, 10);
```

The '10' parameter sent to this method is used to send the `lastProcessedID` query parameter in the HTTP request to the /consume
endpoint.  This parameter can be used to indicate the ID of the last message that was successfully processed. It can be used
to retrieve messages that have already been retrieved. The messages in the queue have sequential ID's, and the lastProcessedID parameter
corresponds to the ID of a message in the queue.
Here is an example of how lastProcessedID can be used. If the application consuming the messages retrieves messages 1-10,
but only successfully processes messages 1-5, it can set the lastProcessedID parameter to 5 in the next invocation. That will give
the application messages 6-10 again.

Check to see how many messages are available in your application's subscriber queue.

```csharp
int numMessages = await messagesClient.GetNumAvailableMessagesAsync();
```

### Automated Polling for Subscriber Messages

Setup automated polling for subscribing to ChangeNotification messages.  The SDK can automatically provide ChangeNotification
messages to a client application when the client application configures a notification poll service to subscribe to a client implementation
of the appropriate abstract subscriber.  As an example, the following shows a client application implementation of an abstract
subscriber, which then receives notifications from the SDK in an automated polling fashion:

```csharp
    // Client implementation of a change notification subscriber...
    public class ClientAppChangeNotificationSubscriber: AbstractEthosChangeNotificationSubscriber<ChangeNotification>
    {
        /// <summary>
        /// This is the method where client would implement their own code to process change notification.
        /// </summary>
        /// <param name="cn"></param>
        public override void OnChangeNotification( ChangeNotification cn )
        {
            Console.WriteLine( "In client application change notification received will be processed e.g. save changes in content to database etc.\r\n" );
            Console.WriteLine( $"Change Notification for: {cn.Resource.Name} with id: {cn.Id} with content type: {cn.ContentType} published on: {cn.Published}.\r\n" );

            /*  
             *  DO NOT ADD FOLLOWING CODE IN YOUR PRODUCTION IMPLEMENTATION. FOLLOWING THREAD IS BEING SLEPT JUST TO SIMULATE
             *  THE ACTION(S) SUCH AS DB INTERACTION OR PASSING CHANGE NOTIFICATION TO YOUR OWN SERVICE ETC...
            */

            TimeSpan ts = new TimeSpan( 0, 0, 0, 5, 0 );
            Thread.Sleep( ts );
        }

        /// <summary>
        /// Here handle any errors that occured.
        /// </summary>
        /// <param name="e"></param>
        public override void OnChangeNotificationError( Exception e )
        {
            base.OnChangeNotificationError( e );
        }
    }
```

```csharp
    // Client implementation of a change notification list subscriber...
    public class ClientAppChangeNotificationListSubscriber: AbstractEthosChangeNotificationSubscriber<IEnumerable<ChangeNotification>>
    {
        /// <summary>
        /// This is the method where client would implement their own code to process collection of change notifications.
        /// </summary>
        /// <param name="cn"></param>
        public override void OnChangeNotification( IEnumerable<ChangeNotification> cns )
        {
            foreach ( var cn in cns )
            {
                Console.WriteLine( "In client application change notification received will be processed e.g. save changes in content to database etc.\r\n" );
                Console.WriteLine( $"Change Notification for: {cn.Resource.Name} with id: {cn.Id} with content type: {cn.ContentType} published on: {cn.Published}.\r\n" );
            }

            /*  
             *  DO NOT ADD FOLLOWING CODE IN YOUR PRODUCTION IMPLEMENTATION. FOLLOWING THREAD IS BEING SLEPT JUST TO SIMULATE
             *  THE ACTION(S) SUCH AS DB INTERACTION OR PASSING CHANGE NOTIFICATION TO YOUR OWN SERVICE ETC...
            */
            TimeSpan ts = new TimeSpan( 0, 0, 0, 5, 0 );
            Thread.Sleep( ts );
        }

        /// <summary>
        /// Here handle any errors that occured.
        /// </summary>
        /// <param name="e"></param>
        public override void OnChangeNotificationError( Exception e )
        {
            Console.WriteLine( e.Message );
        }
    }
```

Example of using the client application subscriber implementation above, to have it receive notifications.
This code would reside in a client application that uses the SDK.

```csharp
        /// <summary>
        /// Change notification polling example.
        /// </summary>
        /// <returns></returns>
        static async Task SubscribeToChangeNotifications()
        {
            EthosClientBuilder ethosClientBuilder = new EthosClientBuilder( SAMPLE_API_KEY )
                                                    .WithConnectionTimeout( 30 );
            EthosChangeNotificationService cnService = EthosChangeNotificationService.Build( action =>
            {
                action
                .WithEthosClientBuilder( ethosClientBuilder );
            }, SAMPLE_API_KEY );
            int? limit = 2;

            myChangeNotificationSubscriber = new ClientAppChangeNotificationSubscriber();
            EthosChangeNotificationPollService service = new EthosChangeNotificationPollService( cnService, limit, 5 )
                .AddSubscriber( myChangeNotificationSubscriber );
            await service.SubscribeAsync();
        }
```

```csharp
        /// <summary>
        /// Change notification list polling example.
        /// </summary>
        /// <returns></returns>
        static async Task SubscribeToChangeNotificationsList()
        {
            EthosClientBuilder ethosClientBuilder = new EthosClientBuilder( SAMPLE_API_KEY )
                                                    .WithConnectionTimeout( 30 );
            EthosChangeNotificationService cnService = EthosChangeNotificationService.Build( action =>
            {
                action.WithEthosClientBuilder( ethosClientBuilder );
            }, SAMPLE_API_KEY );

            int? limit = 2;

            ClientAppChangeNotificationListSubscriber subscriber = new ClientAppChangeNotificationListSubscriber();
            EthosChangeNotificationListPollService service = new EthosChangeNotificationListPollService( cnService, limit, 5 )
                .AddSubscriber( subscriber ));

            await service.SubscribeAsync();
        }
```

### Getting Configuration Info

Get the configuration information for the application to which the API key belongs.

```csharp
EthosConfigurationClient configClient = new EthosClientBuilder(apiKey).BuildEthosConfigurationClient();

// get app configuration as a string
string appConfig = await configClient.GetAppConfigAsync();

// get app configuration as a JArray
JArray appConfig = await configClient.GetAppConfigJsonAsync();
```

Get the list of available resources from the authoritative applications in your tenant.  This calls the /available-resources
endpoint of Ethos Integration, and returns that data in a string or JArray format.

```csharp
// get as string
string availableResources = await configClient.GetAllAvailableResourcesAsync();

// get as JArray
JArray availableResources = await configClient.GetAllAvailableResourcesAsJsonAsync();
```

There is also an option to get the available resources data that only pertains to your application.  If your application has
credentials configured to call one or more authoritative application API's, then it will have an `ownerOverrides` array defined in the
app config.  This ownerOverrides array determines which authoritative app will serve requests for different resources.  When you get
the available resource data specific to your application, the response will be limited to resources and authoritative apps in your application's
ownerOverrides config.

```csharp
// get as string
string availableResources = await configClient.GetAvailableResourcesForAppAsync();

// get as JArray
JArray availableResources = await configClient.GetAvailableResourcesForAppAsJsonAsync();
```

### Managing Ethos Errors

Create an error in the Ethos Integration errors service.

```csharp
EthosErrorsClient errorsClient = new EthosClientBuilder(apiKey).BuildEthosErrorsClient();
// create an EthosError object from a JSON string
string json = "{" +
                    "          \"id\": \"00000000-0000-0000-0000-000000000000\"," +
                    "          \"dateTime\": \"2020-10-27T03:10:44.827Z\"," +
                    "          \"severity\": \"error\"," +
                    "          \"responseCode\": 500," +
                    "          \"description\": \"Internal Server Error\"," +
                    "          \"details\": \"This is a more info on the info error\"," +
                    "          \"applicationId\": \"00000000-0000-0000-0000-000000000000\"," +
                    "          \"applicationName\": \"Banner\"," +
                    "          \"correlationId\": \"2468UserMade3242134\"," +
                    "          \"resource\": {" +
                    "            \"id\": \"00000000-0000-0000-0000-000000000000\"," +
                    "            \"name\": \"persons\"" +
                    "          }," +
                    "          \"applicationSubtype\": \"EMA\"" +
                    "}";
EthosError error = ErrorFactory.CreateErrorFromJson(json);
// post to errors service
EthosResponse response = await errorsClient.PostAsync(error);
```

Get a single page of errors for your tenant from the errors service.

```csharp
EthosResponse errorResponse = await errorsClient.GetAsync();
```

### Getting Data in a Banner MEP Tenant Environment

An overview of Banner MEP and instructions on how to configure an Ethos tenant for Banner MEP is outside of the scope of this documentation.  This will outline how to use API keys
from the different applications to pull data for the different VPDI codes.

When an Ethos tenant environment is configured for a Banner MEP institution, it will have multiple Banner applications setup that point to the same
Banner implementation, but with different URI's to pull data for different VPDI codes.  There will also be separate client or subscribing applications
setup to make proxy requests and receive change-notifications from the different Banner apps.

In this code example, let's assume that I am working with a tenant environment that has Banner applications setup for 3 VPDI codes representing 3 different campuses of an institution:
* NORTH
* SOUTH
* MAINP

Likewise, there will be 3 client applications that are used to get data from the 3 different Banner campuses.


```csharp
// API keys for my 3 client apps
string northKey = "11111111-1111-1111-1111-111111111111";
string southKey = "11111111-1111-1111-1111-111111111112";
string mainKey = "11111111-1111-1111-1111-111111111113";

// get 'students' data through the proxy api for NORTH campus
EthosProxyClient northProxyClient = new EthosClientBuilder(northKey).BuildEthosProxyClient();
EthosResponse response = await northProxyClient.GetAsync("students");

// get change-notifications from the messages service for SOUTH campus
EthosMessagesClient southMessagesClient = new EthosClientBuilder(southKey).GetEthosMessagesClient();
IEnumerable<ChangeNotification> changeNotifications = await southMessagesClient.ConsumeAsync();

// get data through the proxy and get change-notifications for the MAIN campus
EthosClientBuilder mainClientBuilder = new EthosClientBuilder(mainKey);
// proxy
EthosProxyClient mainProxyClient = mainClientBuilder.BuildEthosProxyClient();
EthosResponse response = await mainProxyClient.GetAsync("students");
// messages
EthosMessagesClient mainMessagesClient = mainClientBuilder.GetEthosMessagesClient();
IEnumerable<ChangeNotification> changeNotifications = await mainMessagesClient.ConsumeAsync();
```

### Using EISDK generated strongly typed objects Object Library

Data can be retrieved from Ethos APIs and then be returned as a concrete type as illustrated below.  Following example gets list of term-codes and then iterate over the individual *TermCodesV100GetRequest* type where each property can be accessed by using the dot notation making it easy to find and use members of a type.

```csharp

try
{
    var response = await proxyClient.GetAsync<IEnumerable<TermCodesV100GetRequest>>( "term-codes" );

    if ( response != null )
    {
        Console.WriteLine( "" );
        foreach ( var item in ( IEnumerable<TermCodesV100GetRequest> ) response.Dto )
        {
            Console.WriteLine( $"Activity Date: { item.ActivityDate }, CODE: { item.Code }, DESC: { item.Desc } " );
        }
    }
}
catch ( Exception ex )
{
    Console.WriteLine( ex.Message );
}

```

See the [EISDK C# object library](https://github.com/ellucian-developer/integration-sdk-csharp-objects) for more info.

See the [EISDK C# object library C# Doc](https://github.com/ellucian-developer/integration-sdk-objects-csharp-doc) to access the C# doc pages.

### QAPI Support in the EISDK for C#

Searching by QAPI allows a secure search instead of passing parameters in the URL. Following example illustrate searching term-codes where acyrcode equals 2017. This example is using strongly typed *TermCodesV100GetRequest* type.

```csharp

string resource = "term-codes";
string version = "application/vnd.hedtech.integration.v1.0.0+json";
TermCodesV100GetRequest requestBody = new TermCodesV100GetRequest() { AcyrCode = "2017" };
try
{
    var ethosResponses = await filterClient.GetPagesFromOffsetWithQAPIAsync<TermCodesV100GetRequest>( resource, requestBody, version, 40, 0 );
    foreach ( var ethosResponse in ethosResponses )
    {
        Console.WriteLine( $"Total records retrieved: {ethosResponse.GetContentCount()}." );
        Console.WriteLine( $"Json content: {ethosResponse.Content}" );
    }
}
catch ( Exception e )
{
    Console.WriteLine( e.Message );
}

```

### Examples project

More examples can be found in the [EISDK C# examples project](https://github.com/ellucian-developer/integration-sdk-csharp-examples).
