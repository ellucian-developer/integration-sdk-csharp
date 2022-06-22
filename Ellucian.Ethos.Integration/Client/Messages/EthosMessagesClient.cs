/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Client.Messages
{
    /// <summary>
    ///  An EthosClient used to publish and consume messages using the Ethos Integration messages service.
    ///  
    ///  <p>
    ///  This client accepts an API key that belongs to an ethos application. All requests made from this client are done
    ///  on behalf of the application to which that API key belongs. That means that calling the consume methods
    ///  will try to retrieve change-notifications from the subscription queue of that specific application.It is assumed that
    ///  the application is configured in Ethos Integration to subscribe to some resource changes.</p>
    ///  
    ///  <p>
    ///  Likewise, calling the publish methods will try to publish change-notifications on behalf of the application
    ///  to which the API key belongs. It is assumed that the application is configured in Ethos Integration to own the resources
    ///  that are in the change-notifications being published. If a change-notification is sent to the Ethos Integration
    ///  /publish endpoint for a resource that is not owned by the calling application, it will return a<code>403 Forbidden</code>
    ///  response.</p>
    ///  
    ///  <p>
    ///  The preferred way to instantiate this class is via the { @link com.ellucian.ethos.integration.client.EthosClientBuilder EthosClientBuilder }.</p>
    /// </summary>
    public class EthosMessagesClient : EthosClient
    {
        /// <summary>
        /// The version to use for the Ethos Messages API.
        /// </summary>
        private const string cnType = "application/vnd.hedtech.change-notifications.v2+json";

        /// <summary>
        /// Creates an EthosMessagesClient using the given API key. Note that the preferred way to get an instance of this class 
        /// is through the { @link com.ellucian.ethos.integration.client.EthosClientBuilder EthosClientBuilder }.
        /// </summary>
        /// <param name="apiKey">A valid API key from Ethos Integration. This is required to be a valid 36 character GUID string.
        /// If it is null, empty, or not in a valid GUID format, then an<code> ArgumentNullException</code> will be thrown.</param>
        /// <param name="client">A HttpClient. If it is null/empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        public EthosMessagesClient( string apiKey, HttpClient client ) : base( apiKey, client )
        {

        }

        /// <summary>
        /// Gets a list of messages from the subscription queue of the application. This will return the number of messages specified
        /// by the limit parameter, if there are that many available. The messages will be returned in the format of a       
        /// change-notification.
        /// <p>
        /// The limit can be up to 1000, but the total size of the response payload from Ethos Integration
        /// is limited to 1 MB. If the size limit is reached, the number of messages returned will be less than the specified limit, even if there
        /// are more messages remaining in the queue.</p>
        /// <p>
        /// The lastProcessedID parameter can be used to indicate the ID of the last message that was successfully processed.
        /// This parameter can be used to retrieve messages that have already been retrieved. The messages in the queue have sequential ID's, and the
        /// lastProcessedID parameter corresponds to the ID of a message in the queue.</p>
        /// <p>
        /// Here is an example of how lastProcessedID  can be used. If the application consuming the messages retrieves messages 1-10, but
        /// only successfully processes messages 1-5, it can set the lastProcessedID parameter to 5 in the next invocation.That will give the application
        /// messages 6-10 again.</p>
        /// <p>
        /// When both the limit and lastProcessedID parameters are used together, they do not affect each other. The lastProcessedID gives
        /// a starting point on what message should be the first one returned. The limit is evaluated separately to see how many messages should be returned
        /// (starting with lastProcessedID + 1).</p>
        /// </summary>
        /// <param name="limit">The maximum number of messages to retrieve with a single request. This is required to be an integer between
        /// 1 and 1000, otherwise an InvalidOperationException will be thrown.</param>
        /// <param name="lastProcessedID">The ID of the last message that was successfully processed.</param>
        /// <returns>A list of messages in the change-notification format. This will return up to the given limit of messages if there are that many
        /// available in the queue and they all fit within the 1 MB payload limit.</returns>
        /// <exception cref="InvalidOperationException">The limit is a null-able parameter and if it has value and the value is smaller than 1 or greater than 1000
        /// InvalidOperationException is thrown.</exception>
        public async Task<IEnumerable<ChangeNotification>> ConsumeAsync( int? limit = null, long? lastProcessedID = null )
        {
            if ( ( limit.HasValue && limit < 1 ) || limit > 1000 )
            {
                throw new InvalidOperationException( $"The { nameof( limit ) } parameter has to be between 1 and 1000." );
            }

            return await GetMessagesAsync( limit, lastProcessedID );
        }

        /// <summary>
        /// Gets a list of messages from the subscription queue of the application. This will return the default number of messages, if
        /// available, from Ethos Integration in the format of a change-notification.
        /// <p>
        /// By default, each subsequent call to consume messages will remove the previously retrieved messages from the queue. If you need
        /// to get the same messages( or subset of them ) again, use the consume method that accepts a lastProcessedID parameter.</p>
        /// </summary>
        /// <param name="limit">The limit query parameter.</param>
        /// <param name="lastProcessedID">The lastProcessedID query parameter.</param>
        /// <returns>A list of change-notifications.</returns>
        private async Task<IEnumerable<ChangeNotification>> GetMessagesAsync( int? limit, long? lastProcessedID )
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add( "Accept", cnType );

            EthosResponse response = await GetAsync( headers, EthosIntegrationUrls.Consume( Region, lastProcessedID, limit ) );
            return new EthosResponseConverter().ToCNList( response );
        }

        /// <summary>
        /// Gets the number of available messages in the subscription queue of the application. This will return 0 if
        /// the app has an empty queue or if the app has no subscriber queue.
        /// </summary>
        /// <returns>The number of available messages in the application's queue.</returns>
        public async Task<int> GetNumAvailableMessagesAsync()
        {
            EthosResponse response = await HeadAsync( EthosIntegrationUrls.Consume( Region, -1, -1 ) );
            string remaining = response.GetHeader( "x-remaining" );
            if ( int.TryParse( remaining, out int numMessages ) )
            {
                return numMessages;
            }
            return default( int );
        }
    }
}
