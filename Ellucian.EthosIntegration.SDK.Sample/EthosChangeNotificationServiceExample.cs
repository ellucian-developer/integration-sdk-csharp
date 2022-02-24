/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    public class EthosChangeNotificationServiceExample : EthosExamples
    {

        public static async Task Run()
        {
            await GetNotificationsWithoutOverridesExampleAsync();
            await GetNotificationsWithOverridesExampleAsync();
        }

        private static async Task GetNotificationsWithoutOverridesExampleAsync()
        {
            Console.WriteLine( "******* GetNotificationsWithoutOverridesExampleAsync() *******" );
            EthosClientBuilder ethosClientBuilder = new EthosClientBuilder( SAMPLE_API_KEY )
                                                    .WithConnectionTimeout( 30 );
            EthosChangeNotificationService ethosChangeNotificationService = EthosChangeNotificationService.Build( action =>
            {
                action.WithEthosClientBuilder( ethosClientBuilder );
            }, SAMPLE_API_KEY );


            try
            {
                IEnumerable<ChangeNotification> changeNotificationList = await ethosChangeNotificationService.GetChangeNotificationsAsync();
                Console.WriteLine( $"CHANGE NOTIFICATION LIST LENGTH: {changeNotificationList.Count()}" );
                Console.WriteLine( changeNotificationList.ToString() );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        private static async Task GetNotificationsWithOverridesExampleAsync()
        {
            Console.WriteLine( "******* GetNotificationsWithOverridesExampleAsync() *******" );
            EthosClientBuilder ethosClientBuilder = new EthosClientBuilder( SAMPLE_API_KEY )
                                                        .WithConnectionTimeout( 30 );
            // This configuration will override any change notifications for persons that do not have a version of 12.3.0, with
            // content from a persons v8 request for the given persons ID (GUID) for the given change notification.
            EthosChangeNotificationService ethosChangeNotificationService = EthosChangeNotificationService.Build( action =>
            {
                action
                .WithEthosClientBuilder( ethosClientBuilder )
                .WithResourceAbbreviatedVersionOverride( "persons", "v8" );
            }, SAMPLE_API_KEY );

            try
            {
                IEnumerable<ChangeNotification> changeNotificationList = await ethosChangeNotificationService.GetChangeNotificationsAsync();
                Console.WriteLine( $"CHANGE NOTIFICATION LIST LENGTH: {changeNotificationList.Count()}" );
                Console.WriteLine( changeNotificationList.ToString() );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }
    }
}
