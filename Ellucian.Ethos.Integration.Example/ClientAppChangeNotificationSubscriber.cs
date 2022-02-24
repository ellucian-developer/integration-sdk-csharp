/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Notification;

using System;
using System.Threading;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    /// <summary>
    /// This is an example of how client side subscriber class can be implemented. The class should inherit from <see cref="AbstractEthosChangeNotificationSubscriber{T}/> 
    /// where T is <see cref="ChangeNotification}/>.
    /// </summary>
    public class ClientAppChangeNotificationSubscriber : AbstractEthosChangeNotificationSubscriber<ChangeNotification>
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
}
