/*
 * ******************************************************************************
 *   Copyright  2021 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    public class EthosExamples
    {
        internal protected static string SAMPLE_API_KEY = string.Empty;
        internal protected static string RECORD_GUID = string.Empty;

        ///// <summary>
        ///// This method is the entry point to all examples.</summary>
        ///// <param name="args">Pass api key from the command line, args[0] will then contain the api key and args[1] may contain record guid.</param>
        ///// <returns>Task</returns>
        public static async Task Main( string[] args )
        {
            if ( args == null || !args.Any() )
            {
                Console.WriteLine( "Please enter an API key as a program argument to run this sample program." );
                return;
            }
            SAMPLE_API_KEY = args[ 0 ];
            var RECORD_GUID = ( args != null && args.Count() > 1 ) ? args[1] : string.Empty;

            await RunProxyClientExampleAsync();
            //await RunMessageClientExampleAsync( apiKey );
            //await RunFilterQueryClientExampleAsync( apiKey );
            //await RunGetAccessTokenExampleAsync( apiKey );
            //await RunEthosErrorsClientExampleAsync( apiKey, recordGuid );
            //await RunEthosConfigurationClientExampleAsync( apiKey );
            //await RunEthosChangeNotificationSubscriberExampleAsync( apiKey );
            //await RunEthosChangeNotificationServiceExampleAsync( apiKey );
        }

        private static async Task RunProxyClientExampleAsync()
        {
            await EthosProxyClientExample.Run();
        }

        private static async Task RunMessageClientExampleAsync()
        {
            await EthosMessagesClientExample.Run();
        }

        private static async Task RunFilterQueryClientExampleAsync()
        {
            await EthosFilterQueryClientExample.Run();
        }
        
        private static async Task RunGetAccessTokenExampleAsync()
        {
            await GetAccessTokenExample.Run();
        }
        
        private static async Task RunEthosErrorsClientExampleAsync()
        {
            await EthosErrorsClientExample.Run();
        }
        
        private static async Task RunEthosConfigurationClientExampleAsync()
        {
            await EthosConfigurationClientExample.Run();
        }
        
        private static async Task RunEthosChangeNotificationSubscriberExampleAsync()
        {
            await EthosChangeNotificationSubscriberExample.Run();
        }
        
        private static async Task RunEthosChangeNotificationServiceExampleAsync()
        {
            await EthosChangeNotificationServiceExample.Run();
        }
    }
}
