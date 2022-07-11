/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Authentication;
using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Errors;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{

    public class ErrorsClientTest
    {
        private const string SingleErrorId = "1234";

        [Fact]
        public void ErrorsClientExpiredToken()
        {
            AccessToken sessionToken = new AccessToken( SampleTestData.API_KEY, DateTime.Now.AddDays( -1 ) );
            Assert.False( sessionToken.IsValid() );
        }

        [Fact]
        public async Task ErrorsClientNormalPath()
        {
            EthosErrorsClient errorsClient = CreateTestClient( true );
            IEnumerable<EthosResponse> allErrorsResponse = await errorsClient.GetAllErrorsAsync();
            List<EthosResponse> allErrorsList = new List<EthosResponse>( allErrorsResponse );

            EthosResponseConverter converter = new EthosResponseConverter();

            List<EthosError> allErrors = ErrorFactory.CreateErrorListFromJson( allErrorsList [ 0 ].Content );
            Assert.True( allErrors.Count > 0 );
            foreach ( EthosError error in allErrors )
            {
                // check each value and make sure it is valid
                Assert.False( string.IsNullOrEmpty( error.Id ) );
                Assert.False( string.IsNullOrWhiteSpace( error.Id ) );
            }
        }

        [Fact]
        public async Task ErrorsClient_GetAllErrorsAsJArrayAsync()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks );

            var errorsJArray = await client.GetAllErrorsAsJArrayAsync();
            CheckListState( errorsJArray );
        }

        [Fact]
        public async Task ErrorsClient_GetAllErrorsAsStringsAsyncGetAllErrorsAsStringsAsync()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks );

            var errorsArray = await client.GetAllErrorsAsStringsAsync();
            CheckListState( errorsArray );
        }

        [Fact]
        public async Task ErrorsClient_GetAllErrorsAsync()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks);

            var allErrorsArray = await client.GetAllErrorsAsync();
            CheckListState( allErrorsArray );
        }

        [Fact]
        public async Task ErrorsClient_GetAsEthosErrorsAsync()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks );

            var allErrorsArray = await client.GetAsEthosErrorsAsync();
            CheckListState( allErrorsArray );
        }

        [Fact]
        public async Task ErrorsClient_GetAllErrorsAsEthosErrorsAsync()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks );

            var allErrorsArray = await client.GetAllErrorsAsEthosErrorsAsync();
            CheckListState( allErrorsArray );
        }

        [Fact]
        public async Task ErrorsClient_GetErrorsFromOffsetAsEthosErrorsAsync()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks );

            var allErrorsArray = await client.GetErrorsFromOffsetAsEthosErrorsAsync( 5 );
            CheckListState( allErrorsArray );
        }

        [Fact]
        public async Task ErrorsClient_GetAllErrorsWithPageSizeAsJArraysAsync()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks );

            var pageSizeJarray = await client.GetAllErrorsWithPageSizeAsJArraysAsync( 5 );
            CheckListState( pageSizeJarray );
        }

        [Fact]
        public async Task ErrorsClient_CheckPages()
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( true );
            EthosErrorsClient client = new EthosErrorsClient( SampleTestData.API_KEY, mocks );

            IEnumerable<EthosResponse> pagedResponse = await client.GetErrorsFromOffsetWithPageSizeAsync( 2, 2 );
            // this is broken up to be intentionally small to test functionality. Mocked response contains 10 entries so
            // this uses two per page and starts at 2 for a total of 8 entries.
            CheckListState( pagedResponse );
            EthosResponseConverter ethosResponseConverter = new EthosResponseConverter();
            foreach ( EthosResponse page in pagedResponse )
            {
                List<EthosError> pageOfErrors = ErrorFactory.CreateErrorListFromJson( page.Content );
                CheckListState( pageOfErrors );
            }
        }

        [Fact]
        public async Task ErrorsClient_CheckSingleMethods()
        {
            EthosErrorsClient singleRecordMockClient = CreateTestClient( false );
            string errorsStr = await singleRecordMockClient.GetByIdAsStringAsync( SingleErrorId );
            Assert.False( string.IsNullOrEmpty( errorsStr ) );
            Assert.Equal( errorsStr, SampleTestData.GetOneJsonRecordString() );
            EthosError errorFromString = ErrorFactory.CreateErrorFromJson( errorsStr );
            Assert.NotEmpty( errorFromString.Id );

            EthosError errorFromId = await singleRecordMockClient.GetByIdAsEthosErrorAsync( SingleErrorId );
            Assert.NotNull( errorFromId );
            Assert.NotEmpty( errorFromId.Id );

            JObject jsonNode = await singleRecordMockClient.GetByIdAsJObjectAsync( SingleErrorId );
            Assert.NotNull( jsonNode );
            Assert.NotNull( jsonNode.First );
            Assert.NotNull( jsonNode.Last );
            Assert.NotNull( jsonNode.GetValue( "id" ) );
        }

        private void CheckListState( IEnumerable<object> list )
        {
            Assert.NotNull( list );
            Assert.NotEmpty( list );
        }

        private EthosErrorsClient CreateTestClient( bool multiple )
        {
            var mocks = SampleTestData.GetMockErrorMessageClientWithOK( multiple );
            AccessToken sessionToken = new AccessToken( SampleTestData.API_KEY, DateTime.Now.AddDays( 1 ) );
            Assert.True( sessionToken.IsValid() );
            EthosErrorsClient errorsClient = new EthosErrorsClient( SampleTestData.API_KEY, mocks );
            return errorsClient;
        }
    }
}
