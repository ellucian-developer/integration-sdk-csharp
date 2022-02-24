/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Messages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    /// <summary>
    /// 
    /// </summary>
    public class EthosMessagesClientExample : EthosExamples
    {
        /// <summary>
        /// Uncomment this Main method and comment the Main method in other examples to GetChangeNotificationsAsync().
        /// You can only have one Main method in the same project.</summary>
        /// <param name="args">Pass api key from the command line, args[0] will then contain the api key.</param>
        /// <returns>Task</returns>
        public static async Task Run()
        {
            await GetChangeNotificationsAsync();
            await ConsumeMessagesAsync();
        }

        private static async Task GetChangeNotificationsAsync()
        {
            EthosMessagesClient client = new EthosClientBuilder( SAMPLE_API_KEY ).WithConnectionTimeout( 120 ).BuildEthosMessagesClient();

            int numMessages = await client.GetNumAvailableMessagesAsync();
            Console.WriteLine( $"Number of available messages: { numMessages }" );
        }

        private static async Task ConsumeMessagesAsync()
        {
            EthosMessagesClient client = new EthosClientBuilder( SAMPLE_API_KEY ).WithConnectionTimeout( 120 ).BuildEthosMessagesClient();
            IEnumerable<ChangeNotification> cnList = await client.ConsumeAsync();
            Console.WriteLine( $"Retrieved '{ cnList.Count() }' messages.\n" );
            Console.WriteLine( $"Requesting the same set of messages again, using 'lastProcessedID=0'." );
            cnList = await client.ConsumeAsync( null, 0 );
            Console.WriteLine( $"Retrieved '{ cnList.Count() }' messages.\n" );
        }
    }
}
