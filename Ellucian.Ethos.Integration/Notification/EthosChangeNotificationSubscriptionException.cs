/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;

namespace Ellucian.Ethos.Integration.Notification
{
    /// <summary>
    /// A RuntimeException that can be throws during subscription processing for ChangeNotifications.
    /// </summary>
    public class EthosChangeNotificationSubscriptionException : Exception
    {
        /// <summary>
        /// Constructs this exception with the given error message. 
        /// </summary>
        /// <param name="message">The error message pertinent to ChangeNotification processing.</param>
        public EthosChangeNotificationSubscriptionException( string message ) : base( message )
        {

        }

        /// <summary>
        /// Constructs this exception with the given error message. 
        /// </summary>
        /// <param name="message">The error message pertinent to ChangeNotification processing.</param>
        /// <param name="e">Base exception.</param>
        public EthosChangeNotificationSubscriptionException( string message, Exception e ) : base( message, e )
        {

        }
    }
}
