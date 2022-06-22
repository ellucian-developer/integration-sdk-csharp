/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosClientTests
    {
        #region GET Methods

        [Fact]
        public async Task EthosClient_GetAsync_OK()
        {
            string resourceName = "student-cohorts";
            string url = $"{EthosIntegrationUrls.Api( Ethos.Integration.Authentication.SupportedRegions.US, resourceName )}";
            (Dictionary<string, string> dict, HttpClient client) data = SampleTestDataRepository.GetMockClientWithOK();
            EthosClient ethosClient = new EthosClient( SampleTestDataRepository.API_KEY, data.client );
            var result = await ethosClient.GetAsync( data.dict, url );
            var content = JsonConvert.SerializeObject( result.Content );
            Assert.NotNull( result );
            Assert.Equal( ( int ) HttpStatusCode.OK, result.HttpStatusCode );
            Assert.True( !string.IsNullOrWhiteSpace( content ) );
        }

        [Fact]
        public void EthosClient_GetAsync_NotFound()
        {
            string resourceName = "student-cohorts";
            string url = $"{EthosIntegrationUrls.Api( Ethos.Integration.Authentication.SupportedRegions.US, resourceName )}";
            (Dictionary<string, string> dict, HttpClient client) data = SampleTestDataRepository.GetMockClientWithNotFound();
            EthosClient ethosClient = new EthosClient( SampleTestDataRepository.API_KEY, data.client );
            _ = Assert.ThrowsAsync<HttpRequestException>( async () => await ethosClient.GetAsync( data.dict, url ) );
        }

        #endregion

        #region POST

        [Fact]
        public async Task EthosClient_PostAsync()
        {
            string resourceName = "student-cohorts";
            string url = $"{EthosIntegrationUrls.Api( Ethos.Integration.Authentication.SupportedRegions.US, resourceName )}";
            (Dictionary<string, string> dict, HttpClient client) data = SampleTestDataRepository.GetMockClientWithOK();
            EthosClient ethosClient = new EthosClient( SampleTestDataRepository.API_KEY, data.client );
            var result = await ethosClient.PostAsync( data.dict, url );
            var content = JsonConvert.SerializeObject( result.Content );
            Assert.NotNull( result );
            Assert.Equal( ( int ) HttpStatusCode.OK, result.HttpStatusCode );
            Assert.True( !string.IsNullOrWhiteSpace( content ) );
        }

        #endregion

        #region PUT
        [Fact]
        public async Task EthosClient_PutAsync()
        {
            string resourceName = "student-cohorts";
            string url = $"{EthosIntegrationUrls.Api( Ethos.Integration.Authentication.SupportedRegions.US, resourceName )}";
            (Dictionary<string, string> dict, HttpClient client) data = SampleTestDataRepository.GetMockClientWithOK();
            EthosClient ethosClient = new EthosClient( SampleTestDataRepository.API_KEY, data.client );
            var result = await ethosClient.PutAsync( data.dict, url );
            var content = JsonConvert.SerializeObject( result.Content );
            Assert.NotNull( result );
            Assert.Equal( ( int ) HttpStatusCode.OK, result.HttpStatusCode );
            Assert.True( !string.IsNullOrWhiteSpace( content ) );
        }
        #endregion

        #region DELETE

        [Fact]
        public async Task EthosClient_DeleteAsync()
        {
            string resourceName = "student-cohorts";
            string url = $"{EthosIntegrationUrls.Api( Ethos.Integration.Authentication.SupportedRegions.US, resourceName )}";
            (Dictionary<string, string> dict, HttpClient client) data = SampleTestDataRepository.GetMockClientWithOK();
            EthosClient ethosClient = new EthosClient( SampleTestDataRepository.API_KEY, data.client );
            var result = await ethosClient.PostAsync( data.dict, url );
            var content = JsonConvert.SerializeObject( result.Content );
            Assert.NotNull( result );
            Assert.Equal( ( int ) HttpStatusCode.OK, result.HttpStatusCode );
            Assert.True( !string.IsNullOrWhiteSpace( content ) );
        }

        #endregion
    }
}
