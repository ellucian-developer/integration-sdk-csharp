/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Notification
{
    /// <summary>
    /// Subscription for list of ChangeNotifications processing.
    /// </summary>
    public class EthosChangeNotificationListSubscription: AbstractEthosNotificationSubscription<IEnumerable<ChangeNotification>>
    {
        /// <summary>
        /// Gets an entire list of messages at given time interval from the subscription queue of the application.
        /// </summary>
        /// <param name="ethosChangeNotificationService">Service class used for retrieving ChangeNotifications.</param>
        public EthosChangeNotificationListSubscription( EthosChangeNotificationService ethosChangeNotificationService )
        {
            if ( ethosChangeNotificationService == null )
            {
                throw new EthosChangeNotificationSubscriptionException( $"ERROR: {nameof( ethosChangeNotificationService )} cannot be null." );
            }

            EthosChangeNotificationService = ethosChangeNotificationService;
        }

        #region Methods

        /// <summary>
        /// Initiating the process for the subscriber to receive ChangeNotifications.
        /// </summary>
        /// <param name="limit">
        /// The number of messages to retrieve at once. A list of ChangeNotifications, overriding any notifications with the desired version of the resource if
        /// notifications in the returned list match those added to this class (by resource name) to be overridden.
        /// </param>
        /// <returns><see cref="Task"/>.</returns>
        public override async Task ProcessAsync( int? limit )
        {
            try
            {
                if ( Observers != null && Observers.Any() )
                {
                    var cns = await RequestAsync( limit ).ConfigureAwait( false ); //Get data for the change notifications.

                    //Notify List subscribers.
                    Notify( cns );

                    //If Cancel is true then exit the method and end polling.
                    if ( Cancel )
                    {
                        EndTransmission();
                        return;
                    }

                    TimeSpan ts = new TimeSpan( 0, 0, 0, PollingInterval, 0 );
                    await Task.Delay( ts );

                    await ProcessAsync( limit ).ConfigureAwait( false );
                }
            }
            catch
            {
                EndTransmission();
                throw;
            }
        }

        #endregion
    }
}