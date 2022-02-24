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
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Notification
{
    /// <summary>
    /// Abstract class for change notification. It implements the <see cref="IObservable{T}"/> interface.
    /// </summary>
    public abstract class AbstractEthosNotificationSubscription<T> : IObservable<T>
    {
        /// <summary>
        /// Subscription used for retrieving ChangeNotifications. Uses the <see cref="EthosMessagesClient"/> and
        /// <see cref="EthosProxyClient"/> to do so. If a specific version of a resource is requested for change notifications,
        /// this service will retrieve the desired version of the resource and return the content of that resource (and version) in the
        /// corresponding change notification. For example, if persons v12 is requested but the change notification retrieved is for
        /// persons v8, this service will retrieve persons v12 and replace the content in the persons v8 notification with the content
        /// of persons v12.
        /// </summary>
        internal EthosChangeNotificationService EthosChangeNotificationService { get; set; }
        /// <summary>
        /// Delay between each item that is received.
        /// </summary>
        internal int PollingInterval { get; set; }

        /// <summary>
        /// List of observers.
        /// </summary>
        internal List<IObserver<T>> Observers = new List<IObserver<T>>();


        /// <summary>
        /// Set to true if clients want to cancel the polling operation.
        /// <code>
        ///     Cancel = true;
        /// </code>
        /// </summary>
        public bool Cancel { get; set; } = false;

        /// <Summary>
        /// Notifies the provider that an observer is to receive notifications.
        /// </Summary>
        /// <param name="observer">The object that is to receive notifications.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications
        /// before the provider has finished sending them.</returns>
        public IDisposable Subscribe( IObserver<T> observer )
        {
            if ( !Observers.Contains( observer ) )
                Observers.Add( observer );
            return new Unsubscriber( Observers, observer );
        }

        /// <summary>
        /// Request to get a change notification.
        /// </summary>
        /// <param name="limit">The maximum number of messages to retrieve with a single request. This is required to be an integer between
        /// 1 and 1000, otherwise an InvalidOperationException will be thrown.</param>
        /// <exception cref="EthosChangeNotificationSubscriptionException">If <see cref="EthosChangeNotificationService"/> fails to get change notifications and throws exception.</exception>
        public async Task<IEnumerable<ChangeNotification>> RequestAsync( int? limit )
        {
            List<ChangeNotification> changeNotifications = new List<ChangeNotification>();
            try
            {
                var cns = await EthosChangeNotificationService.GetChangeNotificationsAsync( limit ).ConfigureAwait( false );
                changeNotifications.AddRange( cns.ToList() );
            }
            catch ( Exception e )
            {
                string msg = $"ERROR: Exception thrown retrieving change notifications from EthosChangeNotificationSubscription.\r\n{e.Message}.";
                throw new EthosChangeNotificationSubscriptionException( msg );
            }
            return changeNotifications;
        }

        /// <summary>
        /// Processes the change notifications. Implemented in child class.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public virtual Task ProcessAsync( int? limit )
        {
            //overridden in child class.
            return default;
        }

        /// <summary>
        /// Notifies the subscribers of change notifications.
        /// </summary>
        /// <param name="t"></param>
        internal void Notify( T t )
        {
            foreach ( var observer in Observers )
            {
                try
                {
                    observer.OnNext( t );
                }
                catch ( Exception e )
                {
                    string message = $"ERROR: Error occured while client subscriber processing the change notification(s).\r\n{e.Message}.";
                    var ex = new EthosChangeNotificationSubscriptionException( message, e );
                    observer.OnError( ex );
                }
            }
        }

        /// <summary>
        /// When no further data is available, the method calls each observer's OnCompleted method and then clears the internal list of observers.
        /// </summary>
        internal void EndTransmission()
        {
            if ( Observers != null )
            {
                foreach ( var observer in Observers.ToArray() )
                    if ( Observers.Contains( observer ) )
                        observer.OnCompleted();

                Observers.Clear();
            }
        }

        /// <summary>
        /// An IDisposable implementation that enables observers to stop receiving notifications.
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<T>> _observers;
            private IObserver<T> _observer;

            public Unsubscriber( List<IObserver<T>> observers, IObserver<T> observer )
            {
                _observers = observers;
                _observer = observer;
            }

            /// <summary>
            /// Disposes observers.
            /// </summary>
            public void Dispose()
            {
                if ( _observers != null && _observer != null && _observers.Contains( _observer ) )
                    _observers.Remove( _observer );
            }
        }
    }
}