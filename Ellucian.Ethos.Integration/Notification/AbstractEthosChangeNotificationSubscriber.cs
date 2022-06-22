/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;

namespace Ellucian.Ethos.Integration.Notification
{
    /// <summary>
    /// This is an abstract class implements an <see cref="IObserver{T}"/> interface. The methods <see cref="OnChangeNotification"/> and <see cref="OnChangeNotificationError"/> 
    /// are to be implemented in the client application.
    /// </summary>
    public abstract class AbstractEthosChangeNotificationSubscriber<T> : IObserver<T>
    {
        /// <summary>
        /// Used for unsubscribe observer.
        /// </summary>
        private protected IDisposable Unsubscriber;

        /// <summary>
        /// The subscription used by this subscriber.
        /// </summary>
        internal protected AbstractEthosNotificationSubscription<T> Subscription;

        /// <summary>
        /// Indicates whether the subscription for this subscriber is running, or not.
        /// </summary>
        /// <returns>true if the subscription is running, false if not (if it were canceled).</returns>
        public bool IsSubscriptionRunning()
        {
            if ( Subscription == null || Subscription.Cancel )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Cancels the subscription of <see cref="AbstractEthosNotificationSubscription{T}"/>.
        /// </summary>
        public void CancelSubscription()
        {
            Subscription.Cancel = true;
            Unsubscribe();
        }

        /// <summary>
        /// Notify the client when item is pulled. 
        /// </summary>
        /// <param name="t">Generic type.</param>
        public virtual void OnChangeNotification( T t )
        {
            //Implemented in the client code.
        }

        /// <summary>
        /// Subscribes to an observable.
        /// </summary>
        /// <param name="observable"></param>
        public void OnSubscribe( AbstractEthosNotificationSubscription<T> observable )
        {
            if ( observable != null )
            {
                Subscription = observable;
                Unsubscriber = observable.Subscribe( this );
            }
        }

        /// <summary>
        /// Unsubscribe the observer(s) when completed.
        /// </summary>
        public void OnCompleted()
        {
            Unsubscribe();
        }

        /// <summary>
        /// Here handle any errors that occured.
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnChangeNotificationError( Exception e )
        {
            //Implemented in client subscriber.
        }

        /// <summary>
        /// Notifies client when error occurs.
        /// </summary>
        /// <param name="ex"><see cref="Exception"/>.</param>
        public void OnError( Exception ex )
        {
            OnChangeNotificationError( ex );
        }

        /// <summary>
        /// Calls the next item from the list as it progresses thru the list.
        /// </summary>
        /// <param name="value"></param>
        public void OnNext( T value )
        {
            OnChangeNotification( value );
        }

        /// <summary>
        /// Unsubscribe and dispose the observer.
        /// </summary>
        public void Unsubscribe()
        {
            Unsubscriber.Dispose();
        }
    }
}
