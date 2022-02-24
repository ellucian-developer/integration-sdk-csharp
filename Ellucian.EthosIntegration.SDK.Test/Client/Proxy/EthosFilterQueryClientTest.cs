/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy;
using Ellucian.Ethos.Integration.Client.Proxy.Filter;

using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosFilterQueryClientTest
    {
        private EthosFilterQueryClient filterClient;
        static readonly string criteriaResourceName = "student-cohorts";
        static readonly string namedQueryResourceName = "student-cohorts";
        readonly string version = "application/vnd.hedtech.integration.v7.2.0+json";
        static readonly string criteriaFilterStr = "?criteria=%7b%22name%22%3a%7b%22firstName%22%3a%22FIRST_NAME%22%2c%22lastName%22%3a%22LAST_NAME%22%7d%7d";
        //static readonly string namedQueryfilter = "{\"personFilter\":\"11111111-1111-1111-1111-111111111111\"}";
        static readonly string namedQueryFilterStr = "?personFilter={\"personFilter\":\"11111111-1111-1111-1111-111111111111\"}";
        private static readonly string criteriaUrl = $"https://integrate.elluciancloud.com/api/{criteriaResourceName}{criteriaFilterStr}";
        private static readonly string namedQueryUrl = $"https://integrate.elluciancloud.com/api/{namedQueryResourceName}{namedQueryFilterStr}";

        [Fact]
        public void EthosFilterQueryClient_New()
        {
            filterClient = new EthosFilterQueryClient( Guid.NewGuid().ToString(), new HttpClient() );
            Assert.NotNull( filterClient );
            Assert.IsType<EthosFilterQueryClient>( filterClient );
        }

        [Fact]
        public async Task GetWithCriteriaFilterAsync_ResourceName_CriteriaFilterStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, criteriaFilterStr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_NamedQueryFilterStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, namedQueryFilterStr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilterStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, version, criteriaFilterStr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_Version_NamedQueryFilterStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, version, namedQueryFilterStr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithCriteriaFilterAsync_ResourceName_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var criteriaFilter = new CriteriaFilter().WithSimpleCriteria( "name", ("firstName", "FIRST_NAME") ).BuildCriteria();
            var actual = await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, version, criteriaFilter );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, fltr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_Version_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetPagesWithNamedQueryFilterAsync_ResourceName_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesWithNamedQueryFilterAsync( namedQueryResourceName, fltr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithNamedQueryFilterAsync_ResourceName_Version_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithNamedQueryFilterAsync_ResourceName_PageSize_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesWithNamedQueryFilterAsync( namedQueryResourceName, fltr, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetWithCriteriaFilterAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithCriteriaFilterAsync( "", version, "" ) );
        }

        [Fact]
        public void GetWithCriteriaFilterAsync_CriteriaFilterStr_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, version, "" ) );
        }

        [Fact]
        public async Task GetWithSimpleCriteriaFilterAsync_ResourceName_CriteriaSetName_CriteriaKey_CriteriaValue()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "name", "firstName", "FIRST_NAME" );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithSimpleCriteriaFilterAsync_ResourceName_Version_CriteriaSetName_CriteriaKey_CriteriaValue()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, version, "name", "firstName", "FIRST_NAME" );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( "", "name", "firstName", "FIRST_NAME" ) );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_CriteriaSetName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "", "firstName", "FIRST_NAME" ) );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_CriteriaKey_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "name", " ", "FIRST_NAME" ) );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_CriteriaValue_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "name", "firstName", "" ) );
        }

        [Fact]
        public async Task GetWithFilterMapAsync_ResourceName_Version_FilterMapStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            FilterMap fm = new FilterMap().WithParameterPair( "firstName", "FIRST_NAME" ).Build();
            string fmUrl = $"https://integrate.elluciancloud.com/api/student-cohorts{fm.ToString()}";
            var actual = await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, "?firstName=FIRST_NAME" );
            Assert.NotNull( actual );
            Assert.Equal( fmUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithFilterMapAsync_ResourceName_Version_FilterMap()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            FilterMap fm = new FilterMap().WithParameterPair( "firstName", "FIRST_NAME" ).Build();
            string fmUrl = $"https://integrate.elluciancloud.com/api/student-cohorts{fm.ToString()}";
            var actual = await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, fm );
            Assert.NotNull( actual );
            Assert.Equal( fmUrl, actual.RequestedUrl );
        }

        [Fact]
        public void GetWithFilterMapAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( "", version, "filterMapStr" ) );
        }

        [Fact]
        public void GetWithFilterMapAsync_FilterMapStr_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, "" ) );
        }

        [Fact]
        public void GetWithFilterMapAsync_FilterMap_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( "", version, new FilterMap() ) );
        }

        [Fact]
        public void GetWithFilterMapAsync_FilterMap__ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, default( FilterMap ) ) );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, filter );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, version, filter );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_CriteriaFilter_PageSize()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );

            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, filter, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter_PageSize()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, version, filter, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_CriteriaFilter_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, filter, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, version, filter, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_CriteriaFilter_PageSize_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, filter, 5, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter_PageSize_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, version, filter, 5, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName__ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithCriteriaFilterAsync( "", version, new CriteriaFilter(), 10, 10 ) );
        }

        [Fact]
        public void GetPagesFromOffsetWithCriteriaFilterAsync_criteriaFilter__ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, version, null, 10, 10 ) );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, fltr, 10 );
            Assert.NotNull( actual );
        }


        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_Version_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr, offset: 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_PageSize_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, fltr, 10, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_Version_PageSize_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ).httpClient );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr, 10, 10 );
            Assert.NotNull( actual );
        }        

        [Fact]
        public async Task GetPagesWithFilterMapAsync_ResourceName_Version_FilterMap()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient
                .GetPagesWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                .WithParameterPair( "firstName", "FIRST_NAME" )
                .Build() );
            Assert.NotNull( actual );
        }
        [Fact]
        public async Task GetPagesWithFilterMapAsync_ResourceName_Version_FilterMap_PageSize()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient
                .GetPagesWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                .WithParameterPair( "firstName", "FIRST_NAME" )
                .Build(), 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithFilterMapAsync_ResourceName_Version_FilterMap_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient
                .GetPagesFromOffsetWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                .WithParameterPair( "firstName", "FIRST_NAME" )
                .Build(), 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithFilterMapAsync_ResourceName_Version_FilterMap_PageSize_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient
                   .GetPagesFromOffsetWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                   .WithParameterPair( "firstName", "FIRST_NAME" )
                   .Build(), 10, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetPagesFromOffsetWithFilterMapAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithFilterMapAsync( "", version, new FilterMap(), 10, 10 ) );
        }

        [Fact]
        public void GetPagesFromOffsetWithCriteriaFilterAsync_FilterMap_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithFilterMapAsync( criteriaResourceName, version, default( FilterMap ), 10, 10 ) );
        }

        [Fact]
        public async Task GetTotalCountAsync_ResourceName_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, new CriteriaFilter() );
            Assert.NotEqual( default( int ), actual );
            Assert.Equal( 10, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_ResourceName_Version_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, version, new CriteriaFilter() );
            Assert.NotEqual( default( int ), actual );
            Assert.Equal( 10, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Empty_ResourceName_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetTotalCountAsync( string.Empty, version, new CriteriaFilter() );
            Assert.Equal( default( int ), actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Null_CriteriaFilter_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ).httpClient );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, version, criteria: null );
            Assert.Equal( default( int ), actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_ResourceName_Version_FilterMap()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, version, new CriteriaFilter() );
            Assert.NotEqual( default( int ), actual );
            Assert.Equal( 10, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_For_FilterMap_Empty_ResourceName_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            var actual = await filterClient.GetTotalCountAsync( string.Empty, version, new FilterMap() );
            Assert.Equal( default( int ), actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Null_FilterMap_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockSequenceForEthosFilterQueryClientWithFilterMap().httpClient );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, version, default( FilterMap ) );
            Assert.Equal( default( int ), actual );
        }
    }
}
