/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Notification;
using Ellucian.Ethos.Integration.Service;

using System;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    public class EthosChangeNotificationSubscriberExample : EthosExamples
    {
        private static ClientAppChangeNotificationSubscriber myChangeNotificationSubscriber;
        private static ClientAppChangeNotificationSubscriber myChangeNotificationSubscriber1;

        public static async Task Run()
        {
            try
            {
                await SubscribeToChangeNotifications();
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }

            try
            {
                await SubscribeToChangeNotificationsList();
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// Change notification polling example.
        /// </summary>
        /// <returns></returns>
        private static async Task SubscribeToChangeNotifications()
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
            myChangeNotificationSubscriber1 = new ClientAppChangeNotificationSubscriber();
            EthosChangeNotificationPollService service = new EthosChangeNotificationPollService( cnService, limit, 5 )
                .AddSubscriber( myChangeNotificationSubscriber )
                .AddSubscriber( myChangeNotificationSubscriber1 );
            await service.SubscribeAsync();
        }

        /// <summary>
        /// Change notification polling example.
        /// </summary>
        /// <returns></returns>
        private static async Task SubscribeToChangeNotificationsList()
        {
            EthosClientBuilder ethosClientBuilder = new EthosClientBuilder( SAMPLE_API_KEY )
                                                    .WithConnectionTimeout( 30 );
            EthosChangeNotificationService cnService = EthosChangeNotificationService.Build( action =>
            {
                action.WithEthosClientBuilder( ethosClientBuilder );
            }, SAMPLE_API_KEY );

            int? limit = 2;

            ClientAppChangeNotificationListSubscriber subscriber = new ClientAppChangeNotificationListSubscriber();
            ClientAppChangeNotificationListSubscriber subscriber1 = new ClientAppChangeNotificationListSubscriber();
            EthosChangeNotificationListPollService service = new EthosChangeNotificationListPollService( cnService, limit, 5 )
                .AddSubscriber( subscriber )
                .AddSubscriber( subscriber1 );

            await service.SubscribeAsync();
        }
    }
}