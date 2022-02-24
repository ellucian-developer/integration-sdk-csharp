/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Client.Proxy;
using Ellucian.Ethos.Integration.Service;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Notification
{
    /// <summary>
    /// Service for distributing Ethos ChangeNotifications retrieved from Ethos Integration. An instance of this class should be used to subscribe client application 
    /// ChangeNotification subscribers implements <see cref="IObservable{T}"/> where T is <see cref="ChangeNotification"/>. Service class used for retrieving ChangeNotifications. 
    /// Uses the <see cref="EthosMessagesClient"/> and <see cref="EthosProxyClient"/> to do so. If a specific version of a resource is requested for change notifications,
    /// this service will retrieve the desired version of the resource and return the content of that resource (and version) in the
    /// corresponding change notification. For example, if persons v12 is requested but the change notification retrieved is for
    /// persons v8, this service will retrieve persons v12 and replace the content in the persons v8 notification with the content
    /// of persons v12.
    /// </summary>
    public class EthosChangeNotificationListPollService : AbstractEthosNotificationPollService
    {
        /// <summary>
        /// <see cref="EthosChangeNotificationListSubscription"/> used by client application.
        /// </summary>
        public AbstractEthosNotificationSubscription<IEnumerable<ChangeNotification>> Subscription { get; }

        /// <summary>
        /// Pushes list of notification as they are received.
        /// </summary>
        /// <param name="cnService"><see cref="EthosChangeNotificationService"/> used by client application.</param>
        /// <param name="limit">The number of messages to retrieve at once. A list of ChangeNotifications, overriding any notifications with the desired version of the resource if
        /// notifications in the returned list match those added to this class (by resource name) to be overridden.</param>
        /// <param name="pollingInterval">Delay between each item that is received. Default value is 60 seconds or 1 minute.</param>
        public EthosChangeNotificationListPollService( EthosChangeNotificationService cnService, int? limit, int? pollingInterval )
        {
            if ( cnService == null )
            {
                throw new EthosChangeNotificationSubscriptionException( $"ERROR: {nameof( cnService )} cannot be null." );
            }
            Limit = limit;
            Subscription = new EthosChangeNotificationListSubscription( cnService ) { PollingInterval = pollingInterval ?? 60 };
        }

        /// <summary>
        /// Adds multiple subscribers to subscription.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        public EthosChangeNotificationListPollService AddSubscriber( AbstractEthosChangeNotificationSubscriber<IEnumerable<ChangeNotification>> subscriber )
        {
            subscriber.OnSubscribe( Subscription );//Attach subscription.
            return this;
        }

        /// <summary>
        /// Subscribes the given subscriber to an <see cref="AbstractEthosNotificationSubscription{T}"/>, initiating the process for 
        /// the subscriber to receive ChangeNotifications.
        /// </summary>
        public async Task SubscribeAsync()
        {
            await Subscription.ProcessAsync( Limit );
        }

        /// <summary>
        /// Gets the number of subscribers this publisher is responsible for.
        /// </summary>
        /// <returns>The number of subscribers this publisher has.</returns>
        public int GetNumberOfSubscribers()
        {
            if ( Subscription != null )
            {
                return Subscription.Observers.Count;
            }
            return default;
        }

        /// <summary>
        /// Gets a list of the subscribers this subscription.
        /// </summary>
        /// <returns>A list of subscribers for this subscription.</returns>
        public IEnumerable<T> GetSubscribers<T>()
        {
            List<T> list = new List<T>();
            foreach ( var observer in Subscription.Observers )
            {
                list.Add( ( T ) observer );
            }
            return list;
        }


        /// <summary>
        /// Unsubscribe's the given subscriber from this publisher, removing them from the subscriber map.
        /// </summary>
        /// <param name="subscriber">The subscriber to unsubscribe.</param>
        public void Unsubscribe( AbstractEthosChangeNotificationSubscriber<IEnumerable<ChangeNotification>> subscriber )
        {
            subscriber.Unsubscribe();
        }
    }
}