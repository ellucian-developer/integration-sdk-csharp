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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosProxyClientTests
    {
        private EthosProxyClient proxyClient;
        readonly string resourceName = "student-cohorts";
        string version = "application/vnd.hedtech.integration.v7.2.0+json";
        string guid = "b1a3fc8f-d0cd-4a8b-a6c6-af252f4e49f7";

        #region GET Tests

        [Fact]
        public void GET_Empty_ResourceName_With_HttpClient_Null_ArgumentNullException()
        {
            try
            {
                proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, null );
            }
            catch ( Exception e )
            {
                Assert.IsType<ArgumentNullException>( e );
            }
        }

        [Fact]
        public void GET_Empty_ResourceName_ReturnNull()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsync( string.Empty, string.Empty ) );
        }

        [Fact]
        public async Task GET_ResourceName()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsync( resourceName, string.Empty );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.NotNull( jArray );
            Assert.Equal( ( int ) HttpStatusCode.OK, actual.HttpStatusCode );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public void GET_Empty_ResourceName_WithVersion_ReturnNull()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsync( string.Empty, string.Empty ) );
        }

        [Fact]
        public async Task GET_ResourceName_With_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsync( resourceName, version );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.NotNull( jArray );
            Assert.Equal( ( int ) HttpStatusCode.OK, actual.HttpStatusCode );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetAsString_Empty_ResourceName_WithVersion()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsStringAsync( resourceName, version );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.NotNull( jArray );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetAsJsonArray_Empty_ResourceName_WithVersion()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsJArrayAsync( resourceName, version );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.Count );
        }

        [Fact]
        public async Task GetAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsync( resourceName, "", 0, 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsync( resourceName, version, 0, 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetAsStringAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsStringAsync( resourceName, "", 0, 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetAsStringAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsStringAsync( resourceName, version, 0, 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetAsJsonArrayAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsJArrayAsync( resourceName, "", 0, 10 );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.Count );
        }

        [Fact]
        public async Task GetAsJsonArrayAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAsJArrayAsync( resourceName, version, 0, 10 );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.Count );
        }


        [Fact]
        public async Task GetFromOffsetAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetFromOffsetAsync( resourceName, "", 0 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetFromOffsetAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetFromOffsetAsync( resourceName, version, 0 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetFromOffsetAsStringAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetFromOffsetAsStringAsync( resourceName, "", 0 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetFromOffsetAsStringAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetFromOffsetAsStringAsync( resourceName, version, 0 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetFromOffsetAsJsonArrayAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetFromOffsetAsJArrayAsync( resourceName, "", 0 );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.Count );
        }

        [Fact]
        public async Task GetFromOffsetAsJsonArrayAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetFromOffsetAsJArrayAsync( resourceName, version, 0 );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.Count );
        }

        [Fact]
        public async Task GetWithPageSizeAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetWithPageSizeAsync( resourceName, "", 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetWithPageSizeAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetWithPageSizeAsync( resourceName, version, 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.Content );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetWithPageSizeAsStringAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetWithPageSizeAsStringAsync( resourceName, "", 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetWithPageSizeAsStringAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetWithPageSizeAsStringAsync( resourceName, version, 10 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count );
        }

        [Fact]
        public async Task GetWithPageSizeAsJsonArrayAsync_With_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetWithPageSizeAsJArrayAsync( resourceName, "", 10 );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.Count );
        }

        [Fact]
        public async Task GetWithPageSizeAsJsonArrayAsync_With_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetWithPageSizeAsJArrayAsync( resourceName, version, 10 );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.Count );
        }

        #endregion

        #region GET All Pages

        [Fact]
        public async Task GetAllPages_ResourceName_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actuals = await proxyClient.GetAllPagesAsync( resourceName, version );

            Assert.NotNull( actuals );
            foreach ( var actual in actuals )
            {
                Assert.NotNull( actual );
                Assert.NotNull( actual.Content );
            }
        }

        [Fact]
        public async Task GetAllPages_ResourceName_DefaultVersion()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actuals = await proxyClient.GetAllPagesAsync( resourceName, "" );

            Assert.NotNull( actuals );
            foreach ( var actual in actuals )
            {
                Assert.NotNull( actual );
                Assert.NotNull( actual.Content );
            }
        }

        [Fact]
        public async Task GetAllPages_ResourceName_With_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actuals = await proxyClient.GetAllPagesAsync( resourceName, "", 2 );

            Assert.NotNull( actuals );
            Assert.Equal( 5, actuals.Count() );
        }

        [Fact]
        public async Task GetAllPages_ResourceName_With_Version_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actuals = await proxyClient.GetAllPagesAsync( resourceName, version, 2 );

            Assert.NotNull( actuals );
            Assert.Equal( 5, actuals.Count() );
        }

        [Fact]
        public async Task GetAllPagesAsStrings_ResourceName()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actuals = await proxyClient.GetAllPagesAsStringsAsync( resourceName );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actuals.FirstOrDefault() );
            Assert.NotNull( actuals );
            Assert.Equal( 10, jArray.Count() );
        }

        [Fact]
        public async Task GetAllPagesAsStrings_ResourceName_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actuals = await proxyClient.GetAllPagesAsStringsAsync( resourceName, version );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actuals.FirstOrDefault() );
            Assert.NotNull( actuals );
            Assert.Equal( 10, jArray.Count() );
        }

        [Fact]
        public async Task GetAllPagesAsStrings_ResourceName_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actuals = await proxyClient.GetAllPagesAsStringsAsync( resourceName, "", 2 );
            Assert.NotNull( actuals );
            Assert.Equal( 5, actuals.Count() );
        }

        [Fact]
        public async Task GetAllPagesAsStrings_ResourceName_Version_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actuals = await proxyClient.GetAllPagesAsStringsAsync( resourceName, version, 2 );
            Assert.NotNull( actuals );
            Assert.Equal( 5, actuals.Count() );
        }

        [Fact]
        public async Task GetAllPagesAsJsonArrays_ResourceName()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actuals = await proxyClient.GetAllPagesAsJArraysAsync( resourceName );
            Assert.NotNull( actuals );
            Assert.Equal( 10, actuals.ToList() [ 0 ].Count() );
        }

        [Fact]
        public async Task GetAllPagesAsJsonArrays_ResourceName_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actuals = await proxyClient.GetAllPagesAsJArraysAsync( resourceName, version );
            Assert.NotNull( actuals );
            Assert.Equal( 10, actuals.ToList() [ 0 ].Count() );
        }

        [Fact]
        public async Task GetAllPagesAsJsonArrays_ResourceName_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actuals = await proxyClient.GetAllPagesAsJArraysAsync( resourceName, "", 2 );
            Assert.NotNull( actuals );
            Assert.Equal( 5, actuals.Count() );
        }

        [Fact]
        public async Task GetAllPagesAsJsonArrays_ResourceName_Version_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actuals = await proxyClient.GetAllPagesAsJArraysAsync( resourceName, version, 2 );
            Assert.NotNull( actuals );
            Assert.Equal( 5, actuals.Count() );
        }


        #endregion

        #region Get All Pages From offset

        [Fact]
        public async Task GetAllPagesFromOffset_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsync( resourceName, "", 5 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.FirstOrDefault().Content );
            Assert.NotNull( actual );
            Assert.Equal( 5, jArray.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffset_OffsetIsZero_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsync( resourceName, "", 0, 5 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffset_Version_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsync( resourceName, version, 5 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.Equal( 5, jArray.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffset_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsync( resourceName, version, 5, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 3, actual.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsStrings_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsStringsAsync( resourceName, "", 5, 0 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.FirstOrDefault() );
            Assert.NotNull( actual );
            Assert.Equal( 5, jArray.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsStrings_OffsetIsZero_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsStringsAsync( resourceName, "", 0, 5 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsStrings_Version_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsStringsAsync( resourceName, version, 5, 0 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.FirstOrDefault() );
            Assert.NotNull( actual );
            Assert.Equal( 5, jArray.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsStrings_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsStringsAsync( resourceName, version, 5, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 3, actual.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsJsonArrays_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( resourceName, "", 5 );
            Assert.NotNull( actual );
            Assert.Equal( 5, actual.ToList() [ 0 ].Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsJsonArrays_OffsetIsZero_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( resourceName, "", 0, 5 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsJsonArrays_Version_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( resourceName, version, 5 );
            //JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.FirstOrDefault() );
            Assert.NotNull( actual );
            //Assert.Equal( 5, jArray.Count() );
        }

        [Fact]
        public async Task GetAllPagesFromOffsetAsJsonArrays_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( resourceName, version, 5, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 3, actual.Count() );
        }

        #endregion

        #region Get Pages

        [Fact]
        public async Task GetPages_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsync( resourceName, "", 2 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.FirstOrDefault().Content );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count() );
        }

        [Fact]
        public async Task GetPages_Version_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsync( resourceName, version, 2 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count() );
        }

        [Fact]
        public async Task GetPages_PageSize_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsync( resourceName, "", 5, 1 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count() );
        }

        [Fact]
        public async Task GetPages_Version_PageSize_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsync( resourceName, version, 5, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetPagesAsstrings_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsStringsAsync( resourceName, "", 2 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ] );
            Assert.NotNull( actual );
            Assert.Equal( 10, jArray.Count() );
        }

        [Fact]
        public async Task GetPagesAsstrings_Version_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesAsStringsAsync( resourceName, version, 0, 1 );
            Assert.NotNull( actual );
            Assert.Single( actual );
        }

        [Fact]
        public async Task GetPagesAsstrings_PageSize_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsStringsAsync( resourceName, "", 1, 1 );
            JArray jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ] );
            Assert.NotNull( actual );
            Assert.NotNull( jArray );
        }

        [Fact]
        public async Task GetPagesAsstrings_Version_PageSize_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsStringsAsync( resourceName, version, 1, 1 );
            Assert.NotNull( actual );
            Assert.Single( actual );
        }

        [Fact]
        public async Task GetPagesAsJsonArrays_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsJArraysAsync( resourceName, "", 5 );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.ToList() [ 0 ].Count() );
        }

        [Fact]
        public async Task GetPagesAsJsonArrays_OffsetIsZero_PageSize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesAsJArraysAsync( resourceName, "", 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesAsJsonArrays_Version_Offset()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsJArraysAsync( resourceName, version, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 10, actual.ToList() [ 0 ].Count() );
        }

        [Fact]
        public async Task GetPagesAsJsonArrays_Version_Offset_Pagesize()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesAsJArraysAsync( resourceName, version, 1, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetPagesFromOffset_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsync( resourceName, "", 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffset_PageSize_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsync( resourceName, "", 1, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffset_Version_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsync( resourceName, version, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffset_Version_PageSize_Offset0_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsync( resourceName, version, 1, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffset_Version_PageSize_Offset2_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsync( resourceName, version, 1, 2, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsStrings_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsStringsAsync( resourceName, "", 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsStrings_PageSize_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsStringsAsync( resourceName, "", 1, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsStrings_Version_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsStringsAsync( resourceName, version, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsStrings_Version_PageSize_Offset0_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsStringsAsync( resourceName, version, 1, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsJsonArrays_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsJArraysAsync( resourceName, "", 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsJsonArrays_PageSize_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsJArraysAsync( resourceName, "", 1, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsJsonArrays_Version_Offset_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsJArraysAsync( resourceName, version, 0, 1 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetAsJsonArrays_Version_PageSize_Offset0_NumPages()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetPagesFromOffsetAsJArraysAsync( resourceName, version, 1, 0, 1 );
            Assert.NotNull( actual );
        }

        #endregion

        #region Get by rows

        [Fact]
        public async Task GetRows_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsAsync( resourceName, "", 0, 1 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.Single( jArray );
        }

        [Fact]
        public async Task GetRows_Version_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsAsync( resourceName, version, 0, 1 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.Single( jArray );
        }

        [Fact]
        public async Task GetRows_PageSize_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetRowsAsync( resourceName, "", 1, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetRows_Version_PageSize_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            var actual = await proxyClient.GetRowsAsync( resourceName, "", 1, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetRowsAsStrings_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsAsStringsAsync( resourceName, "", 1 );
            Assert.NotNull( actual );
            Assert.Single( actual );
        }
        
        [Fact]
        public async Task GetRowsAsStrings_Version_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsAsStringsAsync( resourceName, version, 1 );
            Assert.NotNull( actual );
            Assert.Single( actual );
        }
        
        [Fact]
        public async Task GetRowsAsJsonArrays_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsAsJArrayAsync( resourceName, "", 1 );
            Assert.NotNull( actual );
            Assert.Single( actual );
        }

        [Fact]
        public async Task GetRowsAsJsonArrays_Version_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsAsJArrayAsync( resourceName, version, 1 );
            Assert.NotNull( actual );
            Assert.Single( actual );
        }

        [Fact]
        public async Task GetRowsFromOffset_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsync( resourceName, "", 0, 2, 2 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.NotNull( jArray );
            Assert.Equal( 2, jArray.Count );
        }

        [Fact]
        public async Task GetRowsFromOffset_PageSize_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsync( resourceName, "", 2, 2, 2 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.NotNull( jArray );
            Assert.Equal( 2, jArray.Count );
        }

        [Fact]
        public async Task GetRowsFromOffset_Version_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsync( resourceName, version, 0, 2, 2 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.NotNull( jArray );
            Assert.Equal( 2, jArray.Count );
        }

        [Fact]
        public async Task GetRowsFromOffset_Version_PageSize_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsync( resourceName, version, 2, 2, 2 );
            var jArray = JsonConvert.DeserializeObject<JArray>( actual.ToList() [ 0 ].Content );
            Assert.NotNull( actual );
            Assert.NotNull( jArray );
            Assert.Equal( 2, jArray.Count );
        }

        [Fact]
        public async Task GetRowsFromOffsetAsStrings_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsStringsAsync( resourceName, "", 2, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetRowsFromOffsetAsStrings_Version_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsStringsAsync( resourceName, version, 2, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public async Task GetRowsFromOffsetAsJsonArrays_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsJArrayAsync( resourceName, "", 2, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Children().Count() );
        }

        [Fact]
        public async Task GetRowsFromOffsetAsJsonArrays_Version_OffSet_NumRows()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetRowsFromOffsetAsJArrayAsync( resourceName, version, 2, 2 );
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Children().Count() );
        }

        [Fact]
        public async Task GetById_Id()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            var actual = await proxyClient.GetByIdAsync( resourceName, guid );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
        }

        [Fact]
        public async Task GetById_Id_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            var actual = await proxyClient.GetByIdAsync( resourceName, guid, version );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Content );
        }

        [Fact]
        public async Task GetAsstringById_Id()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            var actual = await proxyClient.GetAsStringByIdAsync( resourceName, guid );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetAsstringById_Id_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            var actual = await proxyClient.GetAsStringByIdAsync( resourceName, guid, version );
            Assert.NotNull( actual );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetAsJsonArrayById_Id()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            var actual = await proxyClient.GetAsJObjectByIdAsync( resourceName, guid );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Children().ToList() );
        }

        [Fact]
        public async Task GetAsJsonArrayById_Id_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            var actual = await proxyClient.GetAsJObjectByIdAsync( resourceName, guid, version );
            Assert.NotNull( actual );
            Assert.NotNull( actual.Children().ToList() );
        }

        [Fact]
        public async Task GetMaxPageSizeAsync_ResourceName()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetMaxPageSizeAsync( resourceName );
            Assert.Equal( 500, actual );
        }

        [Fact]
        public async Task GetMaxPageSizeAsync_ResourceName_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetMaxPageSizeAsync( resourceName, version );
            Assert.Equal( 500, actual );
        }

        [Fact]
        public async Task GetPageSizeAsync()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPageSizeAsync( resourceName );
            Assert.NotEqual( 0, actual );
        }

        [Fact]
        public async Task GetPageSizeAsync_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetPageSizeAsync( resourceName, version );
            Assert.NotEqual( 0, actual );
        }

        [Fact]
        public async Task GetPageSizeAsync_Version_EthosResponse()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var ethosResponse = await proxyClient.GetAsync( resourceName, string.Empty );
            var actual = await proxyClient.GetPageSizeAsync( resourceName, version, ethosResponse );
            Assert.NotEqual( 0, actual );
            Assert.Equal( 10, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetTotalCountAsync( resourceName );
            Assert.NotEqual( 0, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Version()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var actual = await proxyClient.GetTotalCountAsync( resourceName, version );
            Assert.NotEqual( 0, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Version_EthosResponse()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            var ethosResponse = await proxyClient.GetAsync( resourceName, string.Empty );
            var actual = await proxyClient.GetTotalCountAsync( resourceName, version, ethosResponse );
            Assert.NotEqual( 0, actual );
            Assert.Equal( 10, actual );
        }

        #endregion

        #region Exception Tests

        [Fact]
        public void GetAsString_Empty_ResourceName_ReturnNull()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOK().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsync( string.Empty, string.Empty ) );
        }

        [Fact]
        public void GetAsString_Empty_ResourceName_WithVersion_ReturnNull()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsync( string.Empty, string.Empty ) );
        }

        [Fact]
        public void GetAsJsonArray_Empty_ResourceName_ReturnNull()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsync( string.Empty, string.Empty ) );
        }

        [Fact]
        public void GetAsJsonArray_Empty_ResourceName_WithVersion_ReturnNull()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsync( string.Empty, string.Empty ) );
        }

        [Fact]
        public void GetAllPagesAsync_ReaourceName_EmptyString_Null_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( string.Empty, "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( null, "" ) );
        }

        [Fact]
        public void GetAllPagesAsync_ReaourceName_EmptyString_Null_Version_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( string.Empty, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( null, version ) );
        }

        [Fact]
        public void GetAllPagesAsync_ReaourceName_EmptyString_Null_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetAllPagesAsync_ReaourceName_EmptyString_Null_Version_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetAllPagesAsStringsAsync_ReaourceName_EmptyString_Null_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( string.Empty ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( null ) );
        }

        [Fact]
        public void GetAllPagesAsStringsAsync_ReaourceName_EmptyString_Null_Version_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( string.Empty, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( null, version ) );
        }

        [Fact]
        public void GetAllPagesAsStringsAsync_ReaourceName_EmptyString_Null_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetAllPagesAsStringsAsync_ReaourceName_EmptyString_Null_Version_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsStringsAsync( null, "", 0 ) );
        }


        [Fact]
        public void GetAllPagesAsJsonArraysAsync_ReaourceName_EmptyString_Null_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( string.Empty ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( null ) );
        }

        [Fact]
        public void GetAllPagesAsJsonArraysAsync_ReaourceName_EmptyString_Null_Version_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( string.Empty, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( null, version ) );
        }

        [Fact]
        public void GetAllPagesAsJsonArraysAsync_ReaourceName_EmptyString_Null_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetAllPagesAsJsonArraysAsync_ReaourceName_EmptyString_Null_Version_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesAsJArraysAsync( null, "", 0 ) );
        }


        [Fact]
        public void GetAllPagesFromOffsetAsync_Offset_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsync_Version_Offset_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( string.Empty, "", 0, 10 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( null, "", 0, 10 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsync_Version_Offset_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( string.Empty, version, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( null, version, 0 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsync_ReaourceName_EmptyString_Null_Version_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( string.Empty, version, 0, 10 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsync( null, version, 0, 10 ) );
        }


        [Fact]
        public void GetAllPagesFromOffsetAsStringsAsync_Offset_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( string.Empty, "", 0, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( null, "", 0, 0 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsStringsAsync_Offset_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( string.Empty, "", 0, 10 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( null, "", 0, 10 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsStringsAsync_Version_Offset_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( string.Empty, version, 0, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( null, version, 0, 0 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsStringsAsync_Version_Offset_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( string.Empty, version, 0, 10 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsStringsAsync( null, version, 0, 10 ) );
        }


        [Fact]
        public void GetAllPagesFromOffsetAsJsonArraysAsync_Offset_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsJsonArraysAsync_Version_Offset_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( string.Empty, "", 0, 10 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( null, "", 0, 10 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsJsonArraysAsync_Version_Offset_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( string.Empty, version, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( null, version, 0 ) );
        }

        [Fact]
        public void GetAllPagesFromOffsetAsJsonArraysAsync_ReaourceName_EmptyString_Null_Version_PageSize_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( string.Empty, version, 0, 10 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAllPagesFromOffsetAsJArraysAsync( null, version, 0, 10 ) );
        }


        [Fact]
        public void GetPagesAsync_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetPagesAsync_Version_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( string.Empty, version, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( null, version, 0 ) );
        }

        [Fact]
        public void GetPagesAsync_PageSize_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( string.Empty, version, 10, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( null, version, 10, 1 ) );
        }

        [Fact]
        public void GetPagesAsync_Version_PageSize_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( string.Empty, version, 10, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsync( null, version, 10, 1 ) );
        }


        [Fact]
        public void GetPagesAsStringsAsync_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( string.Empty, "", 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( null, "", 0 ) );
        }

        [Fact]
        public void GetPagesAsStringsAsync_Version_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( string.Empty, version, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( null, version, 0 ) );
        }

        [Fact]
        public void GetPagesAsStringsAsync_PageSize_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( string.Empty, version, 10, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( null, version, 10, 1 ) );
        }

        [Fact]
        public void GetPagesAsStringsAsync_Version_PageSize_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( string.Empty, version, 10, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsStringsAsync( null, version, 10, 1 ) );
        }


        [Fact]
        public void GetPagesAsJsonArraysAsync_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( string.Empty, "", 0, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( null, "", 0, 0 ) );
        }

        [Fact]
        public void GetPagesAsJsonArraysAsync_Version_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( string.Empty, version, 0 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( null, version, 0 ) );
        }

        [Fact]
        public void GetPagesAsJsonArraysAsync_PageSize_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( string.Empty, version, 10, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( null, version, 10, 1 ) );
        }

        [Fact]
        public void GetPagesAsJsonArraysAsync_Version_PageSize_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( string.Empty, version, 10, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesAsJArraysAsync( null, version, 10, 1 ) );
        }


        [Fact]
        public void GetPagesFromOffsetAsync_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( string.Empty, "", 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( null, "", 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsync_PageSize_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( string.Empty, "", 1, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( null, "", 1, 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsync_Version_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( string.Empty, version, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( null, version, 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsync_Version__PageSize_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( string.Empty, version, 1, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsync( null, version, 1, 0, 1 ) );
        }


        [Fact]
        public void GetPagesFromOffsetAsStringsAsync_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( string.Empty, "", 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( null, "", 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsStringsAsync_PageSize_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( string.Empty, "", 1, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( null, "", 1, 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsStringsAsync_Version_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( string.Empty, version, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( null, version, 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsStringsAsync__PageSize_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( string.Empty, version, 1, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsStringsAsync( null, version, 1, 0, 1 ) );
        }


        [Fact]
        public void GetPagesFromOffsetAsJsonArraysAsync_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( string.Empty, "", 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( null, "", 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsJsonArraysAsync_PageSize_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( string.Empty, "", 1, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( null, "", 1, 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsJsonArraysAsync_Version_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( string.Empty, version, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( null, version, 0, 1 ) );
        }

        [Fact]
        public void GetPagesFromOffsetAsJsonArraysAsync__PageSize_Offset_NumPages_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( string.Empty, version, 1, 0, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetPagesFromOffsetAsJArraysAsync( null, version, 1, 0, 1 ) );
        }


        [Fact]
        public void GetRowsAsync_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( string.Empty, "", 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( null, "", 1 ) );
        }

        [Fact]
        public void GetRowsAsync_Version_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( string.Empty, version, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( null, version, 1 ) );
        }

        [Fact]
        public void GetRowsAsync_PageSize_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( string.Empty, "", 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( null, "", 1, 1 ) );
        }

        [Fact]
        public void GetRowsAsync_Version_PageSize_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( string.Empty, version, 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsync( null, version, 1, 1 ) );
        }


        [Fact]
        public void GetRowsAsStringsAsync_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsStringsAsync( string.Empty, "", 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsStringsAsync( null, "", 1 ) );
        }

        [Fact]
        public void GetRowsAsStringsAsync_Version_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsStringsAsync( string.Empty, version, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsStringsAsync( null, version, 1 ) );
        }

        [Fact]
        public void GetRowsAsJsonArraysAsync_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsJArrayAsync( string.Empty, "", 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsJArrayAsync( null, "", 1 ) );
        }

        [Fact]
        public void GetRowsAsJsonArraysAsync_Version_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsJArrayAsync( string.Empty, version, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsAsJArrayAsync( null, version, 1 ) );
        }
        
        //--
        [Fact]
        public void GetRowsFromOffsetAsync_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( string.Empty, "", 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( null, "", 1, 1 ) );
        }

        [Fact]
        public void GetRowsFromOffsetAsync_PageSize_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( string.Empty, "", 1, 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( null, "", 1, 1, 1 ) );
        }

        [Fact]
        public void GetRowsFromOffsetAsync_Version_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( string.Empty, version, 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( null, version, 1, 1 ) );
        }

        [Fact]
        public void GetRowsFromOffsetAsync_Version_PageSize_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( string.Empty, version, 1, 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsync( null, version, 1, 1, 1 ) );
        }


        [Fact]
        public void GetRowsFromOffsetAsStringsAsync_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsStringsAsync( string.Empty, "", 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsStringsAsync( null, "", 1, 1 ) );
        }

        [Fact]
        public void GetRowsFromOffsetAsStringsAsync_Version_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsStringsAsync( string.Empty, version, 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsStringsAsync( null, version, 1, 1 ) );
        }

        [Fact]
        public void GetRowsFromOffsetAsJsonArraysAsync_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsJArrayAsync( string.Empty, "", 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsJArrayAsync( null, "", 1, 1 ) );
        }

        [Fact]
        public void GetRowsFromOffsetAsJsonArraysAsync_Version_Offset_NumRows_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithArrayWithOKStatus().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsJArrayAsync( string.Empty, version, 1, 1 ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetRowsFromOffsetAsJArrayAsync( null, version, 1, 1 ) );
        }

        //--------------------
        [Fact]
        public void GetByIdAsync_Id_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( string.Empty, guid ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( null, guid ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( resourceName, string.Empty ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( resourceName, null ) );
        }

        [Fact]
        public void GetByIdAsync_Id_Version_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( string.Empty, guid, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( null, guid, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( resourceName, string.Empty, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetByIdAsync( resourceName, null, version ) );
        }

        [Fact]
        public void GetAsStringByIdAsync_Id_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( string.Empty, guid ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( null, guid ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( resourceName, string.Empty ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( resourceName, null ) );
        }

        [Fact]
        public void GetAsStringByIdAsync_Id_Version_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( string.Empty, guid, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( null, guid, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( resourceName, string.Empty, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsStringByIdAsync( resourceName, null, version ) );
        }

        [Fact]
        public void GetAsJObjectByIdAsync_Id_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( string.Empty, guid ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( null, guid ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( resourceName, string.Empty ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( resourceName, null ) );
        }

        [Fact]
        public void GetAsJObjectByIdAsync_Id_Version_ArgumentNullException()
        {
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockClientWithOKSingleRecordWithAccessToken().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( string.Empty, guid, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( null, guid, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( resourceName, string.Empty, version ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await proxyClient.GetAsJObjectByIdAsync( resourceName, null, version ) );
        }

        #endregion

        #region PUT tests
        [Fact]
        public async void PutTest()
        {
            proxyClient = new EthosProxyClient(SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient);
            string resourceName = "resource-062314";
            string resourceId = "resourceId-052412";
            string requestBody = "requestBody";
            EthosResponse response = await proxyClient.PutAsync(resourceName, resourceId, requestBody);
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
        }


        [Fact]
        public async void PutTest_JObject()
        {
            proxyClient = new EthosProxyClient(SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient);
            string resourceName = "resource-062314";
            string resourceId = "resourceId-052412";
            JObject requestBodyObject = JObject.Parse(
                @"{
                    a: 'b'
                }"
            );
            EthosResponse response = await proxyClient.PutAsync(resourceName, resourceId, requestBodyObject);
            CheckResponse(response);
        }

        [Fact]
        public async void PutTest_WithVersion()
        {
            string resourceId = "resourceId-052412"; 
            string requestBody = @"{
                    a: 'b'
                }";
            proxyClient = new EthosProxyClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient );
            EthosResponse response = await proxyClient.PutAsync( resourceName, resourceId, version, requestBody );
            CheckResponse( response );
        }

        [Fact]
        public void PutTest_Exceptions()
        {
            proxyClient = new EthosProxyClient(SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient);
            JObject nullObject = null;
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.PutAsync("resourceName", "resourceId", nullObject));
            string nullString = null;
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.PutAsync("resourceName", "resourceId", nullString));
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.PutAsync("resourceName", "", "requestBody"));
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.PutAsync("", "resourceId", "requestBody"));
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.PutAsync(nullString, "resourceId", "requestBody"));
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.PutAsync("resourceName", null, "requestBody"));
        }

        /// <summary>
        /// Perform some basic checks on an EthosResponse.  This is done to reuse code and for consistency.
        /// </summary>
        /// <param name="response">EthosResponse to check.</param>
        private void CheckResponse(EthosResponse response)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal((int)HttpStatusCode.OK, response.HttpStatusCode);
        }
        #endregion

        #region DELETE tests
        [Fact]
        public async void DeleteTest()
        {
            string resourceName = "resource-408";
            string id = "2018-ktm-250-exc-f";
            proxyClient = new EthosProxyClient(SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient);
            try
            {
                await proxyClient.DeleteAsync(resourceName, id);
            }
            catch (ArgumentException ae)
            {
                Assert.True(false, "Caught argument exception in a test case that should not throw one." + ae.Message);
            }
        }

        [Fact]
        public void DeleteTest_InvalidInputTest()
        {
            proxyClient = new EthosProxyClient(SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosProxyClientWithOKForPaging().httpClient);
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.DeleteAsync(null, "id"));
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.DeleteAsync("", "id"));
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.DeleteAsync("resourceName", null));
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await proxyClient.DeleteAsync("resourceName", ""));
        }
        #endregion
    }
}
