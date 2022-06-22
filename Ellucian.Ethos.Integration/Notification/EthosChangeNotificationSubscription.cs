/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Service;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Notification
{
    /// <summary>
    /// Subscription for ChangeNotification processing. This subscription runs in a separate thread as configured by the scheduledExecutorService.
    /// </summary>
    public class EthosChangeNotificationSubscription : AbstractEthosNotificationSubscription<ChangeNotification>
    {
        /// <summary>
        /// Gets a messages based on the time interval from the subscription queue of the application.
        /// </summary>
        /// <param name="ethosChangeNotificationService">Service class used for retrieving ChangeNotifications.</param>
        public EthosChangeNotificationSubscription( EthosChangeNotificationService ethosChangeNotificationService )
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

                    //Start loop here
                    foreach ( var changeNotification in cns )
                    {
                        Notify( changeNotification );
                    }

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