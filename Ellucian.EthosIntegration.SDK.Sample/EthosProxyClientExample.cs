/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Proxy;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    /// <summary>
    /// Methods to illustrate how to work with the Ellucian Ethos Integration C# SDK. These examples are the
    /// most up to date place to see how to perform operations, and all operations should work. We use these to
    /// verify this code too!
    /// </summary>
    public class EthosProxyClientExample: EthosExamples
    {
        /// <summary>
        /// An example of how to use <see cref="EthosProxyClient"/>.
        /// <para>Example: </para>
        /// <code>var proxyClient = ethosClientFactory.GetEthosProxyClient( SAMPLE_API_KEY );</code>
        /// To call from command line pass parameters as follows, ProxyClientExample.exe &lt;SAMPLE_API_KEY&gt; RECORD_GUID
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Run()
        {
            await DoGetResourceByIdExample();
            await DoGetResourceAsStringByIdExample();
            await DoGetResourceAsJsonByIdExample();
            await DoGetResourcePageSizeExample();
            await DoGetResourceMaxPageSizeExample();
            await DoGetResourceExample();
            await DoGetResourceAsstringExample();
            await DoGetResourceAsJsonExample();
            await DoGetAllPagesExample();
            await DoGetAllPagesAsstringsExample();
            await DoGetAllPagesAsJsonsExample();
            await DoGetAllPagesFromOffsetExample();
            await DoGetAllPagesFromOffsetAsStringsExample();
            await DoGetAllPagesFromOffsetAsJsonsExample();
            await DoGetPagesExample();
            await DoGetPagesAsstringsExample();
            await DoGetPagesAsJsonsExample();
            await DoGetPagesFromOffsetExample();
            await DoGetPagesFromOffsetAsstringsExample();
            await DoGetPagesFromOffsetAsJsonsExample();
            await DoGetRowsExample();
            await DoGetRowsAsstringsExample();
            await DoGetRowsAsJsonExample();
            await DoGetRowsFromOffsetExample();
            await DoGetRowsFromOffsetAsstringsExample();
            await DoGetRowsFromOffsetAsJsonsExample();
            await DoSimplePersonsIterationExample();
            await DoCrudExample();
        }

        private static EthosProxyClient GetEthosProxyClient()
        {
            EthosClientBuilder ethosClientBUilder = new EthosClientBuilder( SAMPLE_API_KEY ).WithConnectionTimeout( 120 );
            return ethosClientBUilder.BuildEthosProxyClient();
        }


        /// <summary>
        /// PrintHeaders
        /// </summary>
        /// <param name="ethosResponse"></param>
        private static void PrintHeaders( EthosResponse ethosResponse )
        {
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_DATE ) }" );
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_CONTENT_TYPE ) }" );
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_X_TOTAL_COUNT ) }" );
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_APPLICATION_CONTEXT ) }" );
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_X_MAX_PAGE_SIZE ) }" );
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_X_MEDIA_TYPE ) }" );
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_HEDTECH_ETHOS_INTEGRATION_APPLICATION_ID ) }" );
            Console.WriteLine( $"Header: { ethosResponse.GetHeader( EthosProxyClient.HDR_HEDTECH_ETHOS_INTEGRATION_APPLICATION_NAME ) }" );
        }

        #region All examples

        /// <summary>
        /// DoGetResourceByIdExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourceByIdExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            string resource = "student-cohorts";
            string id = RECORD_GUID;
            try
            {

                if ( !string.IsNullOrWhiteSpace( id ) )
                {
                    EthosResponse ethosResponse = await ethosProxyClient.GetByIdAsync( resource, id );
                    Console.WriteLine( "******* Get resource by ID example. *******" );
                    Console.WriteLine( $"RESOURCE: { resource }" );
                    Console.WriteLine( $"RESOURCE ID: { id }" );
                    Console.WriteLine( $"RESPONSE: { ethosResponse.Content }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetResourceAsStringByIdExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourceAsStringByIdExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            string resource = "student-cohorts";
            string id = RECORD_GUID;

            try
            {

                if ( !string.IsNullOrWhiteSpace( id ) )
                {
                    string ethosResponse = await ethosProxyClient.GetAsStringByIdAsync( resource, id, "application/vnd.hedtech.integration.v7.2.0+json" );
                    Console.WriteLine( "******* Get resource by ID example. *******" );
                    Console.WriteLine( $"RESOURCE: { resource }" );
                    Console.WriteLine( $"RESOURCE ID: { id }" );
                    Console.WriteLine( $"RESPONSE: { ethosResponse }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetResourceAsJsonByIdExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourceAsJsonByIdExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            string resource = "student-cohorts";
            string id = RECORD_GUID;
            try
            {
                if ( !string.IsNullOrWhiteSpace( id ) )
                {
                    var jsonNode = await ethosProxyClient.GetAsJObjectByIdAsync( resource, id );
                    Console.WriteLine( "******* Get resource by ID example. *******" );
                    Console.WriteLine( $"RESOURCE: { resource }" );
                    Console.WriteLine( $"RESOURCE ID: { id }" );
                    Console.WriteLine( $"RESPONSE: { jsonNode.ToString() }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetResourcePageSizeExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourcePageSizeExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            string resource = "student-cohorts";
            try
            {
                int pageSize = await ethosProxyClient.GetPageSizeAsync( resource );
                Console.WriteLine( "******* Get resource page size example. *******" );
                Console.WriteLine( $"RESOURCE: {resource}" );
                Console.WriteLine( $"Page Size: {pageSize}" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetResourceMaxPageSizeExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourceMaxPageSizeExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            string resource = "student-cohorts";
            try
            {
                int pageSize = await ethosProxyClient.GetMaxPageSizeAsync( resource );
                Console.WriteLine( "******* Get resource max page size example. *******" );
                Console.WriteLine( $"RESOURCE: {resource}" );
                Console.WriteLine( $"MAX PAGE SIZE: {pageSize}" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetResourceExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourceExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            EthosResponse ethosResponse = null;
            try
            {
                string resourceName = "student-cohorts";
                ethosResponse = await ethosProxyClient.GetAsync( resourceName, string.Empty );
                string totalCountHdr = ethosResponse.GetHeader( EthosProxyClient.HDR_X_TOTAL_COUNT );
                Console.WriteLine( "******* Get data for resource example, no paging. *******" );
                Console.WriteLine( $"Get data for resource: {resourceName}" );
                PrintHeaders( ethosResponse );
                Console.WriteLine( "getResource() TOTAL COUNT: " + totalCountHdr );
                Console.WriteLine( "getResource() RESPONSE: " + ethosResponse );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetResourceAsstringExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourceAsstringExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                string response = await ethosProxyClient.GetAsStringAsync( resourceName, string.Empty );
                string jsonNode = JsonConvert.SerializeObject( response );
                Console.WriteLine( "******* Get data for resource as string example, no paging. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( "getResource() PAGE SIZE: " + jsonNode.Length );
                Console.WriteLine( "getResource() RESPONSE: " + response );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetResourceAsJsonExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetResourceAsJsonExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                var jsonNode = await ethosProxyClient.GetAsJArrayAsync( resourceName, "" );
                Console.WriteLine( "******* Get data for resource as Json example, no paging. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( "getResource() PAGE SIZE: " + jsonNode.Count );
                Console.WriteLine( "getResource() RESPONSE: " + jsonNode.ToString() );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetAllPagesExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetAllPagesExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                var ethosResponseList = await ethosProxyClient.GetAllPagesAsync( resourceName, "", 15 );
                Console.WriteLine( "******* Get all pages with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                int counter = ethosResponseList.Count();
                for ( int i = 0; i < counter; i++ )
                {
                    var content = ethosResponseList.ElementAt( i ).Content;
                    Console.WriteLine( $"PAGE { i + 1 } : { content }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { content.Length }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetAllPagesAsstringsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetAllPagesAsstringsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                //ObjectMapper objectMapper = new ObjectMapper();
                var stringList = await ethosProxyClient.GetAllPagesAsStringsAsync( resourceName, "", 15 );
                Console.WriteLine( "******* Get all pages as strings with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                int counter = stringList.Count();
                for ( int i = 0; i < counter; i++ )
                {
                    string jsonNode = stringList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { stringList.ElementAt( i ) }" );
                    Console.WriteLine( $"PAGE { i + 1 }: SIZE: { jsonNode.Length}" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetAllPagesAsJsonsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetAllPagesAsJsonsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                var jsonNodeList = await ethosProxyClient.GetAllPagesAsJArraysAsync( resourceName, "", 15 );
                Console.WriteLine( "******* Get all pages as Jsons with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                int counter = jsonNodeList.Count();

                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = jsonNodeList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { jsonNode.ToString() }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Count }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetAllPagesFromOffsetExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetAllPagesFromOffsetExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                int totalCount = await ethosProxyClient.GetTotalCountAsync( resourceName );
                // Calculate the offset to be 95% of the totalCount to avoid paging through potentially tons of pages.
                int offset = ( int ) ( totalCount * 0.95 );
                var ethosResponseList = await ethosProxyClient.GetAllPagesFromOffsetAsync( resourceName, "", offset );
                Console.WriteLine( "******* Get all pages from offset example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"Calculated offset of { offset } which is 95 percent of a total count of { totalCount } to avoid paging through potentially lots of pages." );
                Console.WriteLine( "To run with more paging, manually set the offset to a lower value, or reduce the percentage of the total count." );
                int counter = ethosResponseList.Count();
                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = ethosResponseList.ElementAt( i ).Content;
                    Console.WriteLine( $"PAGE { i + 1 }: { ethosResponseList.ElementAt( i ).Content}" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Length }" );
                    Console.WriteLine( $"OFFSET: { offset }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetAllPagesFromOffsetAsstringsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetAllPagesFromOffsetAsStringsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                int totalCount = await ethosProxyClient.GetTotalCountAsync( resourceName );
                // Calculate the offset to be 95% of the totalCount to avoid paging through potentially tons of pages.
                int offset = ( int ) ( totalCount * 0.95 );
                var stringList = await ethosProxyClient.GetAllPagesFromOffsetAsStringsAsync( resourceName, "", offset, 0 );
                Console.WriteLine( "******* Get all pages from offset as strings example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"Calculated offset of { offset } which is 95 percent of a total count of { totalCount } to avoid paging through potentially lots of pages." );
                Console.WriteLine( "To run with more paging, manually set the offset to a lower value, or reduce the percentage of the total count." );
                int counter = stringList.Count();
                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = stringList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { stringList.ElementAt( i ) }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Length }" );
                    Console.WriteLine( $"OFFSET: { offset }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetAllPagesFromOffsetAsJsonsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetAllPagesFromOffsetAsJsonsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                int totalCount = await ethosProxyClient.GetTotalCountAsync( resourceName );
                // Calculate the offset to be 95% of the totalCount to avoid paging through potentially tons of pages.
                int offset = ( int ) ( totalCount * 0.95 );
                var jsonNodeList = await ethosProxyClient.GetAllPagesFromOffsetAsJArraysAsync( resourceName, "", offset );
                Console.WriteLine( "******* Get all pages from offset as Jsons example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"Calculated offset of { offset } which is 95 percent of a total count of { totalCount } to avoid paging through potentially lots of pages." );
                Console.WriteLine( "To run with more paging, manually set the offset to a lower value, or reduce the percentage of the total count." );
                int counter = jsonNodeList.Count();
                for ( int i = 0; i < counter; i++ )
                {
                    JArray jsonNode = jsonNodeList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { jsonNode.ToString() }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Count() }" );
                    Console.WriteLine( $"OFFSET: { offset }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetPagesExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetPagesExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                var ethosResponseList = await ethosProxyClient.GetPagesAsync( resourceName, "", 15, 3 );
                Console.WriteLine( "******* Get some number of pages with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                int counter = ethosResponseList.Count();

                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = ethosResponseList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { jsonNode.Content }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Content.Length }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetPagesAsstringsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetPagesAsstringsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                var stringList = await ethosProxyClient.GetPagesAsStringsAsync( resourceName, "", 15, 3 );
                Console.WriteLine( "******* Get some number of pages as strings with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                int counter = stringList.Count();
                for ( int i = 0; i < counter; i++ )
                {
                    string jsonNode = stringList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { stringList.ElementAt( i ) }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Length }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetPagesAsJsonsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetPagesAsJsonsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                var jsonNodeList = await ethosProxyClient.GetPagesAsJArraysAsync( resourceName, "", 15, 3 );
                Console.WriteLine( "******* Get some number of pages as Jsons with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                int counter = jsonNodeList.Count();

                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = jsonNodeList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { jsonNode.ToString() }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Count() }" );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetPagesFromOffsetExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetPagesFromOffsetExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                int pageSize = 15;
                int offset = 10;
                int numPages = 3;
                var ethosResponseList = await ethosProxyClient.GetPagesFromOffsetAsync( resourceName, "", pageSize, offset, numPages );
                Console.WriteLine( "******* Get some number of pages with page size from offset example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"OFFSET: { offset }" );
                int counter = ethosResponseList.Count();

                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = ethosResponseList.ElementAt( i );
                    Console.WriteLine( $"PAGE{ i + 1 }: { jsonNode.Content }" );
                    Console.WriteLine( $"PAGE{ i + 1 } SIZE: { jsonNode.Content.Length }" );
                }
                //Console.WriteLine( $"NUM PAGES: { }", ethosResponseList.size() ) );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetPagesFromOffsetAsstringsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetPagesFromOffsetAsstringsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                int pageSize = 15;
                int offset = 10;
                int numPages = 3;
                var stringList = await ethosProxyClient.GetPagesFromOffsetAsStringsAsync( resourceName, "", pageSize, offset, numPages );
                Console.WriteLine( "******* Get some number of pages as strings with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"OFFSET: { offset }" );
                int counter = stringList.Count();

                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = stringList.ElementAt( i );
                    Console.WriteLine( $"PAGE { i + 1 }: { jsonNode }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Length }" );
                }
                //Console.WriteLine( string.format( "NUM PAGES: %s", stringList.size() ) );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetPagesFromOffsetAsJsonsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetPagesFromOffsetAsJsonsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                int pageSize = 15;
                int offset = 10;
                int numPages = 3;
                var jsonNodeList = await ethosProxyClient.GetPagesFromOffsetAsJArraysAsync( resourceName, "", pageSize, offset, numPages );
                Console.WriteLine( "******* Get some number of pages from offset as Jsons with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"OFFSET: { offset }" );
                int counter = jsonNodeList.Count();

                for ( int i = 0; i < counter; i++ )
                {
                    var jsonNode = jsonNodeList.ElementAt( i );
                    Console.WriteLine( $"PAGE {  i + 1 }: {jsonNode.ToString()}" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Count() }" );
                }
                //Console.WriteLine( string.format( "NUM PAGES: %s", jsonNodeList.size() ) );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetRowsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetRowsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                string version = "application/vnd.hedtech.integration.v7.2.0+json";
                int pageSize = 15;
                int numRows = 40;
                int rowCount = 0;
                var ethosResponseList = await ethosProxyClient.GetRowsAsync( resourceName, version, pageSize, numRows );
                Console.WriteLine( "******* Get some number of rows with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );

                for ( int i = 0; i < ethosResponseList.Count(); i++ )
                {
                    EthosResponse ethosResponse = ethosResponseList.ElementAt( i );
                    JArray jsonNode = JsonConvert.DeserializeObject( ethosResponse.Content ) as JArray;
                    rowCount += jsonNode.Count;
                    Console.WriteLine( $"PAGE { i + 1 }: { ethosResponse.Content }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Count }" );
                }
                Console.WriteLine( $"NUM ROWS: { rowCount }" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetRowsAsstringsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetRowsAsstringsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                string version = "application/vnd.hedtech.integration.v7.2.0+json";
                int numRows = 40;
                var stringList = await ethosProxyClient.GetRowsAsStringsAsync( resourceName, version, numRows );
                Console.WriteLine( "******* Get some number of rows as strings example. *******" );
                Console.WriteLine( $"Get data for resource: {resourceName}" );
                Console.WriteLine( $"RESPONSE: { string.Join( ',', stringList ) }" );
                Console.WriteLine( $"NUM ROWS: { stringList.Count() }" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetRowsAsJsonsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetRowsAsJsonExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                string version = "application/vnd.hedtech.integration.v7.2.0+json";
                int numRows = 40;
                var jsonNode = await ethosProxyClient.GetRowsAsJArrayAsync( resourceName, version, numRows );
                Console.WriteLine( "******* Get some number of rows as JSON objects example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"RESPONSE: { jsonNode.ToString() }" );
                Console.WriteLine( $"NUM ROWS: { jsonNode.Count }" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetRowsFromOffsetExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetRowsFromOffsetExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "persons";
                string version = "application/json";
                int pageSize = 300;
                int offset = 0;
                int numRows = 1000;
                int rowCount = 0;
                var ethosResponseList = await ethosProxyClient.GetRowsFromOffsetAsync( resourceName, version, pageSize, offset, numRows );
                Console.WriteLine( "******* Get some number of rows from offset with page size example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"OFFSET: { offset }" );

                for ( int i = 0; i < ethosResponseList.Count(); i++ )
                {
                    EthosResponse ethosResponse = ethosResponseList.ElementAt( i );
                    JArray jsonNode = JsonConvert.DeserializeObject( ethosResponse.Content ) as JArray;
                    rowCount += jsonNode.Count;
                    //Uncomment the following line if you wish to print json to the console.
                    //Console.WriteLine( $"PAGE { i + 1 }: { ethosResponse.Content }" );
                    Console.WriteLine( $"PAGE { i + 1 } SIZE: { jsonNode.Count }" );
                    Console.WriteLine( $"Requested Url: { ethosResponse.RequestedUrl }" );
                }
                Console.WriteLine( $"NUM ROWS: { rowCount }" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetRowsFromOffsetAsstringsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetRowsFromOffsetAsstringsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                string version = "application/json";
                int offset = 10;
                int numRows = 40;
                var stringList = await ethosProxyClient.GetRowsFromOffsetAsStringsAsync( resourceName, version, offset, numRows );
                Console.WriteLine( "******* Get some number of rows from offset as strings example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"OFFSET: { offset }" );
                Console.WriteLine( $"RESPONSE: { string.Join( ',', stringList ) }" );
                Console.WriteLine( $"NUM ROWS: { stringList.Count() }" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// DoGetRowsFromOffsetAsJsonsExample
        /// </summary>
        /// <returns></returns>
        private static async Task DoGetRowsFromOffsetAsJsonsExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            try
            {
                string resourceName = "student-cohorts";
                string version = "application/json";
                int offset = 10;
                int numRows = 40;
                var jsonNode = await ethosProxyClient.GetRowsFromOffsetAsJArrayAsync( resourceName, version, offset, numRows );
                Console.WriteLine( "******* Get some number of rows from offset as JSON objects example. *******" );
                Console.WriteLine( $"Get data for resource: { resourceName }" );
                Console.WriteLine( $"OFFSET: { offset }" );
                Console.WriteLine( $"RESPONSE: { jsonNode.ToString() }" );
                Console.WriteLine( $"NUM ROWS: { jsonNode.Count }" );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
        }

        /// <summary>
        /// An example of Create, Read, Update, and Delete using the EthosProxyClient.
        /// </summary>
        /// <returns>an awaitable task.</returns>
        private static async Task DoCrudExample()
        {
            EthosProxyClient client = GetEthosProxyClient();
            EthosResponseConverter converter = new EthosResponseConverter();
            try
            {
                IEnumerable<EthosResponse> responses = await client.GetRowsAsync( "persons", "", 1, 1 );
                EthosResponse firstInList = responses.ElementAt( 0 );
                JToken person = converter.ToJArray( firstInList ).ElementAt( 0 );
                string personId = person[ "id" ].ToString();

                JObject personHold = new JObject();
                personHold[ "id" ] = "00000000-0000-0000-0000-000000000000";
                personHold[ "startOn" ] = DateTime.Now.ToString( "yyyy-MM-ddThh:mm:ssZ" );

                JObject personForPersonHold = new JObject();
                personForPersonHold[ "id" ] = personId;
                personHold[ "person" ] = personForPersonHold;
                JObject categoryForPersonHold = new JObject();
                categoryForPersonHold[ "category" ] = "financial";
                personHold[ "type" ] = categoryForPersonHold;

                EthosResponse response = await client.PostAsync( "person-holds", personHold );
                Console.WriteLine( "Created a 'person-holds' record:" );
                Console.WriteLine( response.Content );
                Console.WriteLine();

                // get the ID of the new record
                JObject personHoldResponse = converter.ToJObjectSingle( response );
                string newId = personHoldResponse[ "id" ].ToString();

                // change the date on the person-hold record, and send a put request to update it
                personHold.Remove( "id" );
                DateTime newHoldEnd = DateTime.Now.AddDays( 1 );
                personHold[ "startOn" ] = newHoldEnd.ToString( "yyyy-MM-ddThh:mm:ssZ" );
                response = await client.PutAsync( "person-holds", newId, personHold );
                Console.WriteLine( $"Successfully updated person-holds record {newId}" );

                // delete the record
                await client.DeleteAsync( "person-holds", newId );
                Console.WriteLine( $"Successfully deleted person-holds record {newId}" );

                // attempt to get the formerly created, now deleted, record, and make sure it is no longer there.
                try
                {
                    await client.GetByIdAsync( "person-holds", newId );
                }
                catch
                {
                    Console.WriteLine( $"Failed to get person-holds record {newId}.  The delete was successful." );
                }

            }
            catch ( Exception e )
            {
                Console.WriteLine( "An error occured while performing the update ", e.Message );
                Console.WriteLine( e.StackTrace );
            }
        }

        /// <summary>
        /// SimplePersonsIterationExample
        /// </summary>
        /// <returns>Task</returns>
        private static async Task DoSimplePersonsIterationExample()
        {
            EthosProxyClient ethosProxyClient = GetEthosProxyClient();
            JArray persons = await ethosProxyClient.GetAsJArrayAsync( "persons", string.Empty );
            foreach ( JToken person in persons )
            {
                string id = person[ "id" ].ToString();
                string fullName = person[ "names" ][ 0 ][ "fullName" ].ToString();
                Console.WriteLine( $"{fullName} has a person ID of {id}" );
            }
        }
        #endregion

    }
}
