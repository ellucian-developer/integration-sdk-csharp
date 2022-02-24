/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Authentication;
using Ellucian.Ethos.Integration.Client;

using System;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    /// <summary>
    /// 
    /// </summary>
    public class GetAccessTokenExample : EthosExamples
    {
        /// <summary>
        /// Uncomment this Main method and comment the Main method ProxyClientExample.cs to run this sample GetTokenAsync().
        /// You can only have one Main method in the same project.</summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Run()
        {
            await GetTokenAsync();
        }

        private static async Task GetTokenAsync()
        {
            IHttpProtocolClientBuilder builder = new HttpProtocolClientBuilder( null );
            var client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosProxyClient();
            Console.WriteLine( $"Using API Key '{ SAMPLE_API_KEY }'\n" );
            Console.WriteLine( $"The token provider's AWS region is '{ client.Region }'\n" );
            Console.WriteLine( $"The token provider's auto-refresh value is '{ client.AutoRefresh }'\n" );
            Console.WriteLine( $"The token provider's configured duration for a token to be valid is '{ client.ExpirationMinutes }' minutes\n" );

            AccessToken token = await client.GetAccessTokenAsync();
            Console.WriteLine( $"The token's valid value is '{token.IsValid()}' and it expires at '{ token.ExpirationTime.ToLongDateString()} : { token.ExpirationTime.ToLongTimeString() }'" );
        }
    }
}
