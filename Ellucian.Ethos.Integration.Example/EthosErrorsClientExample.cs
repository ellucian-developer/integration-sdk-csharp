/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Errors;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Sample.CommandLine
{
    /// <summary>
    /// Error client examples.
    /// </summary>
    public class EthosErrorsClientExample : EthosExamples
    {
        /// <summary>
        /// Methods.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Run()
        {
            await GetErrorHeadersAsync();
            await GetErrorsAsync();
            await GetErrorsAsJArrayAsync();
            await GetAsEthosErrorsAsync();
            await GetAllErrorsAsEthosErrorsAsync();
            await GetErrorsFromOffsetAsEthosErrorsAsync();
            await GetErrorsAsStringAsync();
            await GetErrorByIdAsync();
            await GetErrorByIdAsEthosErrorAsync();
            await GetErrorByIdAsJObjectAsync();
            await GetErrorByIdAsStringAsync();
            await CreateErrorAsync();
            await GetTotalErrorCountAsync();
            //*************************************************************
            //Commenting the following methods, see block comment below.
            await GetAllErrorsAsync();
            await GetAllErrorsAsJArrayAsync();
            await GetAllErrorsAsStringsAsync();
            await GetAllErrorsWithPageSizeAsJArrayAsync();
            //*************************************************************/
            await GetErrorsFromOffsetWithPageSizeAsync();
        }

        private static EthosErrorsClient ethosErrorsClient = GetEthosErrorsClient();
        private static EthosErrorsClient GetEthosErrorsClient()
        {
            return new EthosClientBuilder( SAMPLE_API_KEY )
                       .WithConnectionTimeout( 30 )
                       .BuildEthosErrorsClient();
        }

        private static async Task GetErrorHeadersAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetHeader() *******" );
            try
            {
                EthosResponse ethosResponse = await ethosErrorsClient.GetAsync();
                var headerMap = ethosResponse.HeadersMap;
                var headers = headerMap.ToDictionary( a => a.Key, a => string.Join( ";", a.Value ) );

                foreach ( string headerKey in headers.Keys )
                {
                    string header = ethosResponse.GetHeader( headerKey );
                    Console.WriteLine( $"HEADER KEY: {headerKey}, HEADER VALUE: {header}" );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetErrorsAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAsync() *******" );
            try
            {
                EthosResponse ethosResponse = await ethosErrorsClient.GetAsync();
                string totalCountHeader = ethosResponse.GetHeader( EthosErrorsClient.HDR_TOTAL_COUNT );
                string remainingCountHeader = ethosResponse.GetHeader( EthosErrorsClient.HDR_REMAINING_COUNT );
                Console.WriteLine( $"TOTAL ERROR COUNT: {totalCountHeader}" );
                Console.WriteLine( $"REMAINING ERROR COUNT: {remainingCountHeader}" );
                JArray errorsNode = JArray.Parse( ethosResponse.Content );
                var errorsIter = errorsNode.GetEnumerator();
                while ( errorsIter.MoveNext() )
                {
                    var errNode = errorsIter.Current;
                    Console.WriteLine( errNode.ToString() );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetErrorsAsJArrayAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAsJArrayAsync() *******" );
            try
            {
                var errorsNode = await ethosErrorsClient.GetAsJArrayAsync();
                var errorsIter = errorsNode.GetEnumerator();
                while ( errorsIter.MoveNext() )
                {
                    var errNode = errorsIter.Current;
                    Console.WriteLine( errNode.ToString() );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetAsEthosErrorsAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAsEthosErrorsAsync *******" );
            try
            {
                var ethosErrorList = await ethosErrorsClient.GetAsEthosErrorsAsync();
                Console.WriteLine( $"NUMBER OF ERRORS: {ethosErrorList.Count()}" );
                foreach ( EthosError ethosError in ethosErrorList )
                {
                    Console.WriteLine( ethosError.ToString() );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.StackTrace );
            }
        }

        private static async Task GetAllErrorsAsEthosErrorsAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAllErrorsAsEthosErrorsAsync *******" );
            try
            {
                var ethosErrorList = await ethosErrorsClient.GetAllErrorsAsEthosErrorsAsync();
                Console.WriteLine( $"NUMBER OF ERRORS: {ethosErrorList.Count()}" );
                foreach ( EthosError ethosError in ethosErrorList )
                {
                    Console.WriteLine( ethosError.ToString() );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.StackTrace );
            }
        }

        private static async Task GetErrorsFromOffsetAsEthosErrorsAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetErrorsFromOffsetAsEthosErrorsAsync *******" );
            try
            {
                var ethosErrorList = await ethosErrorsClient.GetErrorsFromOffsetAsEthosErrorsAsync( 5 );
                Console.WriteLine( $"NUMBER OF ERRORS: {ethosErrorList.Count()}" );
                foreach ( EthosError ethosError in ethosErrorList )
                {
                    Console.WriteLine( ethosError.ToString() );
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.StackTrace );
            }
        }

        private static async Task GetErrorsAsStringAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAsStringAsync() *******" );
            try
            {
                string errorsStr = await ethosErrorsClient.GetAsStringAsync();
                Console.WriteLine( errorsStr );
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetErrorByIdAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetByIdAsync() *******" );
            try
            {
                if ( !string.IsNullOrWhiteSpace( RECORD_GUID ) )
                {
                    EthosResponse ethosResponse = await ethosErrorsClient.GetByIdAsync( RECORD_GUID );
                    Console.WriteLine( ethosResponse.Content );
                }
                else
                {
                    Console.WriteLine( "******* Skipping ethosErrorClient.GetByIdAsync() because the RECORD_GUID was not set.  Please pass in a valid GUID value as a 2nd program argument to run this method. *******" );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetErrorByIdAsEthosErrorAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetByIdAsEthosErrorAsync() *******" );
            try
            {
                if ( !string.IsNullOrWhiteSpace( RECORD_GUID ) )
                {
                    EthosError ethosError = await ethosErrorsClient.GetByIdAsEthosErrorAsync( RECORD_GUID );
                    Console.WriteLine( ethosError.ToString() );
                }
                else
                {
                    Console.WriteLine( "******* Skipping ethosErrorClient.GetByIdAsEthosErrorAsync() because the RECORD_GUID was not set.  Please pass in a valid GUID value as a 2nd program argument to run this method. *******" );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetErrorByIdAsJObjectAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetByIdAsJObjectAsync() *******" );
            try
            {
                if ( !string.IsNullOrWhiteSpace( RECORD_GUID ) )
                {
                    var errorNode = await ethosErrorsClient.GetByIdAsJObjectAsync( RECORD_GUID );
                    Console.WriteLine( errorNode.ToString() );
                }
                else
                {
                    Console.WriteLine( "******* Skipping ethosErrorClient.GetByIdAsJObjectAsync() because the RECORD_GUID was not set.  Please pass in a valid GUID value as a 2nd program argument to run this method. *******" );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetErrorByIdAsStringAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetByIdAsStringAsync() *******" );
            try
            {
                if ( !string.IsNullOrWhiteSpace( RECORD_GUID ) )
                {
                    string errorStr = await ethosErrorsClient.GetByIdAsStringAsync( RECORD_GUID );
                    Console.WriteLine( errorStr );
                }
                else
                {
                    Console.WriteLine( "******* Skipping ethosErrorClient.getByIdAsString() because the RECORD_GUID was not set.  Please pass in a valid GUID value as a 2nd program argument to run this method. *******" );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task CreateErrorAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.CreateAsync() *******" );
            string errorStr = "{" +
                    "          \"id\": \"00000000-0000-0000-0000-000000000000\"," +
                    "          \"dateTime\": \"2020-10-27T03:10:44.827Z\"," +
                    "          \"severity\": \"error\"," +
                    "          \"responseCode\": 500," +
                    "          \"description\": \"Internal Server Error\"," +
                    "          \"details\": \"This is a more info on the info error\"," +
                    "          \"applicationId\": \"00000000-0000-0000-0000-000000000000\"," +
                    "          \"applicationName\": \"Banner\"," +
                    "          \"correlationId\": \"2468UserMade3242134\"," +
                    "          \"resource\": {" +
                    "            \"id\": \"00000000-0000-0000-0000-000000000000\"," +
                    "            \"name\": \"persons\"" +
                    "          }," +
                    "          \"applicationSubtype\": \"EMA\"" +
                    "}";

            try
            {
                EthosError ethosError = ErrorFactory.CreateErrorFromJson( errorStr );
                Console.WriteLine( "CREATING ETHOS ERROR: " + ethosError.ToString() );
                EthosResponse errorResponse = await ethosErrorsClient.PostAsync( ethosError );
                Console.WriteLine( $"Created Ethos Error: {errorResponse.Content}" );
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetTotalErrorCountAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetTotalErrorCountAsync() *******" );
            try
            {
                int totalErrorCount = await ethosErrorsClient.GetTotalErrorCountAsync();
                Console.WriteLine( $"TOTAL ERROR COUNTER: {totalErrorCount}" );
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        /************************************************************************************************************
         * Commenting out the getAllErrors*() methods due to the potential for a large quantity of errors retrieved
         * at runtime.  But if desired, this code can be uncommented and executed as part of these example methods.
         * Be aware that these commented methods may take some time to run depending on the quantity of errors,
         * since all errors are retrieved.
         ************************************************************************************************************/

        private static async Task GetAllErrorsAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAllErrorsAsync() *******" );
            try
            {
                List<EthosResponse> ethosResponseList = ( await ethosErrorsClient.GetAllErrorsAsync() ).ToList();
                for ( int i = 0; i < ethosResponseList.Count; i++ )
                {
                    Console.WriteLine( $"ERROR PAGE {( i + 1 )}, REQUESTED URL: {ethosResponseList [ i ].RequestedUrl}, ERROR PAGE DETAILS: {ethosResponseList [ i ].Content}" );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetAllErrorsAsJArrayAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAllErrorsAsJArrayAsync() *******" );
            try
            {
                List<JArray> jsonNodePageList = ( await ethosErrorsClient.GetAllErrorsAsJArrayAsync() ).ToList();
                for ( int pageCount = 0; pageCount < jsonNodePageList.Count; pageCount++ )
                {
                    JArray jsonNodePage = jsonNodePageList [ pageCount ];
                    for ( int errorCount = 0; errorCount < jsonNodePage.Count; errorCount++ )
                    {
                        var errorNode = jsonNodePage [ errorCount ];
                        Console.WriteLine( $"ERROR PAGE {( pageCount + 1 )}, ERROR COUNT: {( errorCount + 1 )}, ERROR ID: {errorNode.SelectToken( "id" ).ToString()}, SEVERITY: {errorNode.SelectToken( "severity" ).ToString()}, DESCRIPTION: {errorNode.SelectToken( "description" ).ToString()}" );
                    }
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetAllErrorsAsStringsAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.getAllErrorsAsStrings() *******" );
            try
            {
                List<string> errorsStringList = ( await ethosErrorsClient.GetAllErrorsAsStringsAsync() ).ToList();
                for ( int i = 0; i < errorsStringList.Count; i++ )
                {
                    Console.WriteLine( $"ERROR PAGE {( i + 1 )}, ERROR PAGE DETAILS: {errorsStringList [ i ]}" );
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetAllErrorsWithPageSizeAsJArrayAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.GetAllErrorsWithPageSizeAsJArraysAsync() *******" );
            int pageSize = 15;
            try
            {
                var jsonNodePageList = ( await ethosErrorsClient.GetAllErrorsWithPageSizeAsJArraysAsync( pageSize ) ).ToList();
                for ( int pageCount = 0; pageCount < jsonNodePageList.Count; pageCount++ )
                {
                    JArray jsonNodePage = jsonNodePageList [ pageCount ];
                    for ( int errorCount = 0; errorCount < jsonNodePage.Count; errorCount++ )
                    {
                        var errorNode = jsonNodePage [ errorCount ];
                        Console.WriteLine( $"ERROR PAGE {pageCount + 1}, SPECIFIED PAGE SIZE: {pageSize}, RETURNED PAGE SIZE: {jsonNodePage.Count}, ERROR COUNT: {( errorCount + 1 )}, ERROR ID: {errorNode.SelectToken( "id" ).ToString()}, SEVERITY: {errorNode.SelectToken( "severity" ).ToString()}, DESCRIPTION: {errorNode.SelectToken( "description" ).ToString()}" );
                    }
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }

        private static async Task GetErrorsFromOffsetWithPageSizeAsync()
        {
            Console.WriteLine( "******* ethosErrorClient.getErrorsFromOffsetWithPageSize() *******" );
            EthosResponseConverter ethosResponseConverter = new EthosResponseConverter();
            try
            {
                int pageSize = 15;
                int totalCount = await ethosErrorsClient.GetTotalErrorCountAsync();
                // Calculate the offset to be 95% of the totalCount to avoid paging through potentially tons of errors.
                int offset = ( int ) ( totalCount * 0.95 );
                double expectedNumPages = Math.Ceiling( ( Convert.ToDouble( totalCount ) - Convert.ToDouble( offset ) ) / Convert.ToDouble( pageSize ) );
                Console.WriteLine( string.Format( "Calculated offset of {} which is 95 percent of a total count of {} to avoid paging through potentially lots of errors.", offset, totalCount ) );
                Console.WriteLine( "To run with more paging, manually set the offset to a lower value, or reduce the percentage of the total count." );
                List<EthosResponse> ethosResponseList = ( await ethosErrorsClient.GetErrorsFromOffsetWithPageSizeAsync( offset, pageSize ) ).ToList();
                Console.WriteLine( $"FROM OFFSET: {offset}, EXPECTED NUMBER OF PAGES: {expectedNumPages}, NUMBER OF PAGES RETURNED: {ethosResponseList.Count}" );
                for ( int pageCount = 0; pageCount < ethosResponseList.Count(); pageCount++ )
                {
                    EthosResponse ethosResponse = ethosResponseList [ pageCount ];
                    Console.WriteLine( $"REQUESTED URL: {ethosResponse.RequestedUrl}" );
                    List<EthosError> ethosErrorList = ( ethosResponseConverter.ToEthosErrorList( ethosResponse ) ).ToList();
                    for ( int errorCount = 0; errorCount < ethosErrorList.Count(); errorCount++ )
                    {
                        EthosError ethosError = ethosErrorList [ errorCount ];
                        Console.WriteLine( $"ERROR PAGE {pageCount + 1}, SPECIFIED PAGE SIZE: {pageSize}, RETURNED PAGE SIZE: {ethosErrorList.Count}, ERROR COUNT: {errorCount + 1}, ERROR ID: {ethosError.Id}, SEVERITY: {ethosError.Severity}, DESCRIPTION: {ethosError.Description}" );
                    }
                }
            }
            catch ( Exception ioe )
            {
                Console.WriteLine( ioe.Message );
            }
        }
    }
}
