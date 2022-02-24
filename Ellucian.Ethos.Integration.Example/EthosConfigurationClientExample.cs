/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Config;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    public class EthosConfigurationClientExample : EthosExamples
    {
        /// <summary>
        /// Uncomment this Main method and comment the Main method in other examples.
        /// You can only have one Main method in the same project.</summary>
        /// <param name="args">Pass api key from the command line, args[0] will then contain the api key.</param>
        /// <returns>Task</returns>
        public static async Task Run()
        {
            await GetAllAvailableResourcesAsync();
            await GetAllAvailableResourcesJsonAsync();
            await FilterAvailableResourcesAsync();
            await GetFiltersAndNamedQueriesAsync();
            await GetLatestVersionHeaderAsync();
            await GetFiltersAsync();
            await GetNamedQueriesAsync();

            await GetVersionHeadersForAppAsync();
            await GetVersionsForAppAsync();
            await GetMajorVersionsOfResourceAsync();
            await IsResourceVersionSupported();
            await IsResourceVersionSupported_FullHeader();
            await IsResourceVersionSupportedAsync_SemVer();
            await GetVersionHeadersOfResourceAsStringsAsync();
            await GetVersionHeadersOfResourceAsync();
            await GetVersionHeaderAsync();
            await GetVersionHeaderAsync_SemVer();
        }

        private async static Task FilterAvailableResourcesAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetAllAvailableResourcesAsJsonAsync();
            var filteredResponse = client.FilterAvailableResources( response, "persons" );
            Console.WriteLine( filteredResponse );
        }

        private async static Task GetAllAvailableResourcesAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetAllAvailableResourcesAsync();
            Console.WriteLine( response );
        }

        private async static Task GetAllAvailableResourcesJsonAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetAllAvailableResourcesAsJsonAsync();
            var formattedString = response.ToString( formatting: Newtonsoft.Json.Formatting.Indented );
            Console.WriteLine( response );
        }

        private async static Task GetFiltersAndNamedQueriesAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetFiltersAndNamedQueriesAsync( "persons" );
            var formattedString = response.ToString( formatting: Newtonsoft.Json.Formatting.Indented );
            Console.WriteLine( formattedString );
        }

        private async static Task GetLatestVersionHeaderAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var latestVersion = await client.GetLatestVersionHeaderAsync( "persons" );
            Console.WriteLine( latestVersion );
        }

        private async static Task GetFiltersAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var filters = await client.GetFiltersAsync( "sections" );
            filters.ToList().ForEach( i =>
            {
                Console.WriteLine( i );
            } );
        }

        private async static Task GetNamedQueriesAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var queries = await client.GetNamedQueriesAsync( "sections" );
            queries.ToList().ForEach( i =>
            {
                Console.WriteLine( $"{ i.Key }\r\n" );
                i.Value.ToList().ForEach( j =>
                {
                    Console.WriteLine( j );
                } );
            } );
        }

        private async static Task GetVersionHeadersForAppAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetVersionHeadersForAppAsync( "persons" );
            response.Distinct().OrderByDescending( v => v ).ToList().ForEach( i =>
              {
                  Console.WriteLine( $"Version Header: { i }" );
              } );
        }

        private async static Task GetVersionsForAppAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetVersionsForAppAsync( "academic-credentials" );
            response.Distinct().OrderByDescending( v => v ).ToList().ForEach( i =>
            {
                Console.WriteLine( $"Version: { i }" );
            } );
        }

        private async static Task GetMajorVersionsOfResourceAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetMajorVersionsOfResourceAsync( "persons" );
            response.Distinct().OrderByDescending( v => v ).ToList().ForEach( i =>
            {
                Console.WriteLine( $"Major Version: { i }" );
            } );
        }

        private async static Task IsResourceVersionSupported()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            int patchVersion = 0;
            int minorVersion = 3;
            int majorVersion = 12;
            var response = await client.IsResourceVersionSupportedAsync( "persons", patchVersion: patchVersion );
            var isOrNot = response ? "is" : "not";
            Console.WriteLine( $"The version { patchVersion } { isOrNot } supported for persons" );
            response = await client.IsResourceVersionSupportedAsync( "persons", minorVersion: minorVersion );
            isOrNot = response ? "is" : "not";
            Console.WriteLine( $"The version { minorVersion } { isOrNot } supported for persons" );
            response = await client.IsResourceVersionSupportedAsync( "persons", majorVersion: majorVersion );
            isOrNot = response ? "is" : "not";
            Console.WriteLine( $"The version { majorVersion } { isOrNot } supported for persons" );
        }

        private async static Task IsResourceVersionSupported_FullHeader()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            string fullHeader = "application/vnd.hedtech.integration.v12.3.0+json";
            var response = await client.IsResourceVersionSupportedAsync( "persons", fullHeader );
            var isOrNot = response ? "is" : "not";
            Console.WriteLine( $"The version { fullHeader } { isOrNot } supported for persons" );

        }

        private async static Task IsResourceVersionSupportedAsync_SemVer()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            SemVer semVer = new SemVer() { Major = 12, Minor = 3, Patch = 0 };
            var response = await client.IsResourceVersionSupportedAsync( "persons", semVer );
            var isOrNot = response ? "is" : "not";
            Console.WriteLine( $"The version { semVer.Major }.{semVer.Minor}.{semVer.Patch} { isOrNot } supported for persons" );

        }
        private static async Task GetVersionHeadersOfResourceAsStringsAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetVersionHeadersOfResourceAsStringsAsync( "persons)" );
            foreach ( var item in response )
            {
                Console.WriteLine( $"Version header: {item}" );
            }
        }

        private static async Task GetVersionHeadersOfResourceAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetVersionHeadersOfResourceAsync( "persons" );
            Console.WriteLine( response.ToString() );
        }

        private static async Task GetVersionHeaderAsync()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            var response = await client.GetVersionHeaderAsync( "persons", 12, 3, 0 );
            Console.WriteLine( response );
        }

        private static async Task GetVersionHeaderAsync_SemVer()
        {
            EthosConfigurationClient client = new EthosClientBuilder( SAMPLE_API_KEY ).BuildEthosConfigurationClient();
            SemVer semVer = new SemVer() { Major = 12, Minor = 3, Patch = 0 };
            var response = await client.GetVersionHeaderAsync( "persons", semVer );
            Console.WriteLine( response );
        }
    }
}
