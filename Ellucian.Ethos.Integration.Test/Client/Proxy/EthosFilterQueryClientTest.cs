/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Filter.Extensions;
using Ellucian.Ethos.Integration.Client.Proxy;
using Ellucian.Ethos.Integration.Client.Proxy.Filter;
using Newtonsoft.Json.Linq;
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
        static readonly string offsetLimit = "?offset%3D0%26limit%3D5";
        static readonly string criteriaFilterStronglyTyped = "?criteria={'id':'78685'}";
        static readonly string namedQueryFilterStr = "?personFilter={\"personFilter\":\"11111111-1111-1111-1111-111111111111\"}";
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
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            var actual = await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, criteriaFilterStr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_NamedQueryFilterStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ) );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, namedQueryFilterStr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilterStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            var actual = await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, version, criteriaFilterStr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_Version_NamedQueryFilterStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ) );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, version, namedQueryFilterStr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithCriteriaFilterAsync_ResourceName_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            var criteriaFilter = new CriteriaFilter().WithSimpleCriteria( "name", ("firstName", "FIRST_NAME") ).BuildCriteria();
            var actual = await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, version, criteriaFilter );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ));
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, fltr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithNamedQueryFilterAsync_ResourceName_Version_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ));
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr );
            Assert.NotNull( actual );
            Assert.Equal( namedQueryUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetPagesWithNamedQueryFilterAsync_ResourceName_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ));
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesWithNamedQueryFilterAsync( namedQueryResourceName, fltr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithNamedQueryFilterAsync_ResourceName_Version_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ) );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithNamedQueryFilterAsync_ResourceName_PageSize_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ));
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesWithNamedQueryFilterAsync( namedQueryResourceName, fltr, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetWithCriteriaFilterAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithCriteriaFilterAsync( "", version, "" ) );
        }

        [Fact]
        public void GetWithCriteriaFilterAsync_CriteriaFilterStr_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithCriteriaFilterAsync( criteriaResourceName, version, "" ) );
        }

        [Fact]
        public async Task GetWithSimpleCriteriaFilterAsync_ResourceName_CriteriaSetName_CriteriaKey_CriteriaValue()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var actual = await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "name", "firstName", "FIRST_NAME" );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetWithSimpleCriteriaFilterAsync_ResourceName_Version_CriteriaSetName_CriteriaKey_CriteriaValue()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            var actual = await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, version, "name", "firstName", "FIRST_NAME" );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( "", "name", "firstName", "FIRST_NAME" ) );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_CriteriaSetName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "", "firstName", "FIRST_NAME" ) );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_CriteriaKey_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "name", " ", "FIRST_NAME" ) );
        }

        [Fact]
        public void GetWithSimpleCriteriaFilterAsync_CriteriaValue_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithSimpleCriteriaValuesAsync( criteriaResourceName, "name", "firstName", "" ) );
        }

        [Fact]
        public async Task GetWithFilterMapAsync_ResourceName_Version_FilterMapStr()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithFilterMap() );
            FilterMap fm = new FilterMap().WithParameterPair( "firstName", "FIRST_NAME" ).Build();
            string fmUrl = $"https://integrate.elluciancloud.com/api/student-cohorts{fm.ToString()}";
            var actual = await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, "?firstName=FIRST_NAME" );
            Assert.NotNull( actual );
            Assert.Equal( fmUrl, actual.RequestedUrl );
        }

        [Fact]
        public async Task GetWithFilterMapAsync_ResourceName_Version_FilterMap()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithFilterMap() );
            FilterMap fm = new FilterMap().WithParameterPair( "firstName", "FIRST_NAME" ).Build();
            string fmUrl = $"https://integrate.elluciancloud.com/api/student-cohorts{fm.ToString()}";
            var actual = await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, fm );
            Assert.NotNull( actual );
            Assert.Equal( fmUrl, actual.RequestedUrl );
        }

        [Fact]
        public void GetWithFilterMapAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( "", version, "filterMapStr" ) );
        }

        [Fact]
        public void GetWithFilterMapAsync_FilterMapStr_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, "" ) );
        }

        [Fact]
        public void GetWithFilterMapAsync_FilterMap_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithFilterMap());
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( "", version, new FilterMap() ) );
        }

        [Fact]
        public void GetWithFilterMapAsync_FilterMap__ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithFilterMap());
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithFilterMapAsync( criteriaResourceName, version, default( FilterMap ) ) );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, filter );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, version, filter );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_CriteriaFilter_PageSize()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );

            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, filter, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter_PageSize()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesWithCriteriaFilterAsync( criteriaResourceName, version, filter, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_CriteriaFilter_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, filter, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, version, filter, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_CriteriaFilter_PageSize_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, filter, 5, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName_Version_CriteriaFilter_PageSize_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var filter = new CriteriaFilter()
                .WithSimpleCriteria( "name", ("firstName", "FIRST_NAME"), ("lastName", "LAST_NAME") );
            var actual = await filterClient
                .GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, version, filter, 5, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetPagesFromOffsetWithCriteriaFilterAsync_ResourceName__ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithCriteriaFilterAsync( "", version, new CriteriaFilter(), 10, 10 ) );
        }

        [Fact]
        public void GetPagesFromOffsetWithCriteriaFilterAsync_criteriaFilter__ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithCriteriaFilterAsync( criteriaResourceName, version, null, 10, 10 ) );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ) );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, fltr, 10 );
            Assert.NotNull( actual );
        }


        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_Version_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ));
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr, offset: 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_PageSize_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ));
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, fltr, 10, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithNamedQueryFilterAsync_ResourceName_Version_PageSize_Offset_NamedQueryFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( namedQueryFilterStr ) );
            var fltr = new NamedQueryFilter( "personFilter" ).WithNamedQuery( "personFilter", "11111111-1111-1111-1111-111111111111" );
            var actual = await filterClient.GetPagesFromOffsetWithNamedQueryFilterAsync( namedQueryResourceName, version, fltr, 10, 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesWithFilterMapAsync_ResourceName_Version_FilterMap()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var actual = await filterClient
                .GetPagesWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                .WithParameterPair( "firstName", "FIRST_NAME" )
                .Build() );
            Assert.NotNull( actual );
        }
        [Fact]
        public async Task GetPagesWithFilterMapAsync_ResourceName_Version_FilterMap_PageSize()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            var actual = await filterClient
                .GetPagesWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                .WithParameterPair( "firstName", "FIRST_NAME" )
                .Build(), 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithFilterMapAsync_ResourceName_Version_FilterMap_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var actual = await filterClient
                .GetPagesFromOffsetWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                .WithParameterPair( "firstName", "FIRST_NAME" )
                .Build(), 10 );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetPagesFromOffsetWithFilterMapAsync_ResourceName_Version_FilterMap_PageSize_OffSet()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            var actual = await filterClient
                   .GetPagesFromOffsetWithFilterMapAsync( criteriaResourceName, version, new FilterMap()
                   .WithParameterPair( "firstName", "FIRST_NAME" )
                   .Build(), 10, 5 );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetPagesFromOffsetWithFilterMapAsync_ResourceName_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithFilterMapAsync( "", version, new FilterMap(), 10, 10 ) );
        }

        [Fact]
        public void GetPagesFromOffsetWithCriteriaFilterAsync_FilterMap_ArgumentNullException()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithFilterMapAsync( criteriaResourceName, version, default( FilterMap ), 10, 10 ) );
        }

        [Fact]
        public async Task GetTotalCountAsync_ResourceName_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, new CriteriaFilter() );
            Assert.NotEqual( default( int ), actual );
            Assert.Equal( 10, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_ResourceName_Version_CriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, new CriteriaFilter(), version );
            Assert.NotEqual( default( int ), actual );
            Assert.Equal( 10, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Empty_ResourceName_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            var actual = await filterClient.GetTotalCountAsync( string.Empty, new CriteriaFilter(), version );
            Assert.Equal( default( int ), actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Null_CriteriaFilter_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, criteria: null, version );
            Assert.Equal( default( int ), actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_ResourceName_Version_FilterMap()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithFilterMap() );
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, new CriteriaFilter(), version );
            Assert.NotEqual( default( int ), actual );
            Assert.Equal( 10, actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_For_FilterMap_Empty_ResourceName_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithFilterMap());
            var actual = await filterClient.GetTotalCountAsync( string.Empty, new FilterMap(), version );
            Assert.Equal( default( int ), actual );
        }

        [Fact]
        public async Task GetTotalCountAsync_Null_FilterMap_ReturnsZero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithFilterMap());
            var actual = await filterClient.GetTotalCountAsync( criteriaResourceName, default( FilterMap ), version );
            Assert.Equal( default( int ), actual );
        }

        [Fact]
        public void GetWithQapiAsync_Exceptions()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithQapiAsync( "", "{}", "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithQapiAsync( "resourceName", " ", "" ) );
        }

        [Fact]
        public void GetWithQapiAsync_JObject_Exceptions()
        {
            JObject jobj = null;
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithQapiAsync( "", JObject.FromObject( new { id = '1' } ), "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithQapiAsync( "resourceName", jobj, "" ) );
        }

        [Fact]
        public void GetWithQapiAsync_JObject_ST_Exceptions()
        {
            JObject jobj = null;
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithQapiAsync<JObject>( "", JObject.FromObject( new { id = '1' } ), "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetWithQapiAsync<JObject>( "resourceName", jobj, "" ) );
        }

        [Fact]
        public async void GetWithQapiAsync_String_WithVersion()
        {
            string requestBody = @"{
                    a: 'b'
                }";
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            EthosResponse response = await filterClient.GetWithQapiAsync( criteriaResourceName, requestBody, version );
            Assert.NotNull( response );
            Assert.NotNull( response.Content );
            Assert.Equal( ( int ) HttpStatusCode.OK, response.HttpStatusCode );
        }

        [Fact]
        public async void GetWithQapiAsync_JObject_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            EthosResponse response = await filterClient.GetWithQapiAsync( criteriaResourceName, jobj, version );
            Assert.NotNull( response );
            Assert.NotNull( response.Content );
            Assert.Equal( ( int ) HttpStatusCode.OK, response.HttpStatusCode );
        }

        [Fact]
        public async void GetWithQapiAsync_StronglyTyped_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockHttpClientWithSingleRecordForStronglyTyped() );
            EthosResponse response = await filterClient.GetWithQapiAsync<JObject>( criteriaResourceName, jobj, version );
            Assert.NotNull( response );
            Assert.NotNull( response.Content );
            Assert.Equal( ( int ) HttpStatusCode.OK, response.HttpStatusCode );
        }

        [Fact]
        public void GetPagesFromOffsetWithQAPIAsync_Exceptions()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithQAPIAsync( "", "{}", "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithQAPIAsync( "resourceName", " ", "" ) );
        }

        [Fact]
        public void GetPagesFromOffsetWithQAPIAsync_JObject_Exceptions()
        {
            JObject jobj = null;
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithQAPIAsync( "", JObject.FromObject( new { id = '1' } ), "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithQAPIAsync( "resourceName", jobj, "" ) );
        }

        [Fact]
        public void GetPagesFromOffsetWithQAPIAsync_JObject_ST_Exceptions()
        {
            JObject jobj = null;
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithQAPIAsync<JObject>( "", JObject.FromObject( new { id = '1' } ), "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesFromOffsetWithQAPIAsync<JObject>( "resourceName", jobj, "" ) );
        }

        [Fact]
        public async void GetPagesFromOffsetWithQAPIAsync_String_WithVersion()
        {
            string requestBody = @"{
                    a: 'b'
                }";
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            IEnumerable<EthosResponse> response = await filterClient.GetPagesFromOffsetWithQAPIAsync( criteriaResourceName, requestBody, version );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public async void GetPagesFromOffsetWithQAPIAsync_String_WithVersion_Paging_Offset()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosQAPIClientWithOK( offsetLimit ) );
            IEnumerable<EthosResponse> response = await filterClient.GetPagesFromOffsetWithQAPIAsync( criteriaResourceName, jobj.ToString(), version, 5, 0 );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public async void GetPagesFromOffsetWithQAPIAsync_JObject_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            IEnumerable<EthosResponse> response = await filterClient.GetPagesFromOffsetWithQAPIAsync( criteriaResourceName, jobj, version );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public async void GetPagesFromOffsetWithQAPIAsync_StronglyTyped_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            IEnumerable<EthosResponse> response = await filterClient.GetPagesFromOffsetWithQAPIAsync<JObject>( criteriaResourceName, jobj, version );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public void GetPagesWithQAPIAsync_Exceptions()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesWithQAPIAsync( "", "{}", "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesWithQAPIAsync( "resourceName", " ", "" ) );
        }

        [Fact]
        public void GetPagesWithQAPIAsync_JObject_Exceptions()
        {
            JObject jobj = null;
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesWithQAPIAsync( "", JObject.FromObject( new { id = '1' } ), "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesWithQAPIAsync( "resourceName", jobj, "" ) );
        }

        [Fact]
        public void GetPagesWithQAPIAsync_JObject_ST_Exceptions()
        {
            JObject jobj = null;
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesWithQAPIAsync<JObject>( "", JObject.FromObject( new { id = '1' } ), "" ) );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await filterClient.GetPagesWithQAPIAsync<JObject>( "resourceName", jobj, "" ) );
        }

        [Fact]
        public async void GetPagesWithQAPIAsync_String_WithVersion()
        {
            string requestBody = @"{
                    a: 'b'
                }";
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            IEnumerable<EthosResponse> response = await filterClient.GetPagesWithQAPIAsync( criteriaResourceName, requestBody, version );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public async void GetPagesWithQAPIAsync_String_WithVersion_Paging_Offset()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosQAPIClientWithOK( offsetLimit ) );
            IEnumerable<EthosResponse> response = await filterClient.GetPagesWithQAPIAsync( criteriaResourceName, jobj, version, 5 );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public async void GetPagesWithQAPIAsync_JObject_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            IEnumerable<EthosResponse> response = await filterClient.GetPagesWithQAPIAsync( criteriaResourceName, jobj, version );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public async void GetPagesWithQAPIAsync_StronglyTyped_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ));
            IEnumerable<EthosResponse> response = await filterClient.GetPagesWithQAPIAsync<JObject>( criteriaResourceName, jobj, version );
            Assert.NotNull( response );
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>( response );
        }

        [Fact]
        public async void GetTotalCountAsync_ResourceName_Null_Returns_Zero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            var response = await filterClient.GetTotalCountAsync( string.Empty, JObject.FromObject( new { id = '1' } ).ToString(), string.Empty );
            Assert.Equal( 0, response );
        }

        [Fact]
        public async void GetTotalCountAsync_QapiRequestBody_Null_Returns_Zero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            var response = await filterClient.GetTotalCountAsync( criteriaResourceName, string.Empty, string.Empty );
            Assert.Equal( 0, response );
        }

        [Fact]
        public async void GetTotalCountAsync_JObject_ResourceName_Null_Returns_Zero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            var response = await filterClient.GetTotalCountAsync( string.Empty, JObject.FromObject( new { id = '1' } ), string.Empty );
            Assert.Equal( 0, response );
        }

        [Fact]
        public async void GetTotalCountAsync_JObject__QapiRequestBody_Null_Returns_Zero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            JObject jObj = null;
            var response = await filterClient.GetTotalCountAsync( criteriaResourceName, jObj, string.Empty );
            Assert.Equal( 0, response );
        }

        [Fact]
        public async void GetTotalCountAsync_ST__ResourceName_Null_Returns_Zero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            var response = await filterClient.GetTotalCountAsync<JObject>( string.Empty, JObject.FromObject( new { id = '1' } ), string.Empty );
            Assert.Equal( 0, response );
        }

        [Fact]
        public async void GetTotalCountAsync_ST__QapiRequestBody_Null_Returns_Zero()
        {
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, new HttpClient() );
            JObject jObj = null;
            var response = await filterClient.GetTotalCountAsync<JObject>( criteriaResourceName, jObj, string.Empty );
            Assert.Equal( 0, response );
        }

        [Fact]
        public async void GetTotalCountAsync_String_WithVersion()
        {
            string requestBody = @"{
                    a: 'b'
                }";
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            int response = await filterClient.GetTotalCountAsync( criteriaResourceName, requestBody, version );
            Assert.IsType<int>( response );
        }

        [Fact]
        public async void GetTotalCountAsync_JObject_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK( criteriaFilterStr ) );
            int response = await filterClient.GetTotalCountAsync( criteriaResourceName, jobj, version );
            Assert.IsType<int>( response );
        }

        [Fact]
        public async void GetTotalCountAsync_StronglyTyped_WithVersion()
        {
            JObject jobj = JObject.FromObject( new { id = '1' } );
            filterClient = new EthosFilterQueryClient( SampleTestData.API_KEY, SampleTestData.GetMockHttpClientWithSingleRecordForStronglyTyped() );
            int response = await filterClient.GetTotalCountAsync<JObject>( criteriaResourceName, jobj, version );
            Assert.IsType<int>( response );
        }

        #region Strongly Typed

        //Test single record response for strongly typed data
        [Theory]
        [InlineData("student-cohorts", "?criteria=%7b%22name%22%3a%7b%22firstName%22%3a%22FIRST_NAME%22%2c%22lastName%22%3a%22LAST_NAME%22%7d%7d", "application/vnd.hedtech.integration.v7.2.0+json")]
        public async void GetWithCriteriaFilterAsync_ConvertEthosResponseContentToType_Success(string criteriaResourceName, string criterFilter, string version)
        {
            filterClient = new EthosFilterQueryClient(SampleTestData.API_KEY, SampleTestData.GetResponseWithSingleForStronglyTyped(criterFilter));
            EthosResponse response = await filterClient.GetWithCriteriaFilterAsync<Test>(criteriaResourceName, criterFilter, version);
            Assert.NotNull(response);
            Assert.Empty(response.Content);
            Assert.NotNull(response.Dto);            
            Assert.IsType<Test>((Test)response.Dto);
            Assert.Equal((int)HttpStatusCode.OK, response.HttpStatusCode);
        }

        [Fact]
        public async void GetWithClassCriteriaFilterAsync_ConvertEthosResponseContentToType_Success()
        {
            filterClient = new EthosFilterQueryClient(SampleTestData.API_KEY, SampleTestData.GetResponseWithSingleForStronglyTyped(criteriaFilterStr));
            EthosResponse response = await filterClient.GetWithCriteriaFilterAsync<Test>(criteriaResourceName, new CriteriaFilter(), version);
            Assert.NotNull(response);
            Assert.Empty(response.Content);
            Assert.NotNull(response.Dto);
            Assert.IsType<Test>((Test)response.Dto);
            Assert.Equal((int)HttpStatusCode.OK, response.HttpStatusCode);
        }

        //Test Null exception with empty strings and whitespaces
        [Theory]
        [InlineData("", "", "")]
        [InlineData(" ", "?criteria=%7b%22name%22%3a%7b%22firstName%22%3a%22FIRST_NAME%22%2c%22lastName%22%3a%22LAST_NAME%22%7d%7d", "application/vnd.hedtech.integration.v7.2.0+json")]
        [InlineData("student-cohorts", "?criteria=%7b%22name%22%3a%7b%22firstName%22%3a%22FIRST_NAME%22%2c%22lastName%22%3a%22LAST_NAME%22%7d%7d", " ")]
        [InlineData("student-cohorts", " ", "application/vnd.hedtech.integration.v7.2.0+json")]
        [InlineData("student-cohorts", "", "")]
        public void GetWithCriteriaFilterAsync_EmptyStrings_ArgumentNullException(string criteriaResourceName, string criterFilter, string version)
        {
            filterClient = new EthosFilterQueryClient(SampleTestData.API_KEY, new HttpClient());
            _ = Assert.ThrowsAsync<ArgumentNullException>(async () => await filterClient.GetWithCriteriaFilterAsync(criteriaResourceName, criterFilter, version));
        }

        //Test array of response for strongly typed data
        [Fact]
        public async void GetWithCriteriaFilterAsync_ConvertEthosResponseContentListToType_Success()
        {

            filterClient = new EthosFilterQueryClient(SampleTestData.API_KEY, SampleTestData.GetResponseWithArrayForStronglyTyped(criteriaFilterStr));
            IEnumerable<EthosResponse> ethosResponseList = await filterClient.GetPagesWithCriteriaFilterAsync<IEnumerable<EthosResponse>>(criteriaResourceName, new CriteriaFilter(), version, 10);
            Assert.NotNull(ethosResponseList);
            foreach (var ethosResponse in ethosResponseList)
            {
                _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>(ethosResponse.Dto);
                Assert.Empty(ethosResponse.Content);
                Assert.Equal((int)HttpStatusCode.OK, ethosResponse.HttpStatusCode);
            }
        }

        #endregion Strongly Typed

        //Test response content with default version
        [Fact]
        public async Task GetWithCriteriaFilterAsync_ResourceName_DefaultVersion_EncodeCriteriaFilter()
        {
            filterClient = new EthosFilterQueryClient(SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK(criteriaFilterStronglyTyped));
            var actual = await filterClient.GetWithCriteriaFilterAsync(criteriaResourceName, criteriaFilterStronglyTyped);
            Assert.NotNull(actual);
        }

        [Fact]
        public async Task GetPagesWithNamedQueryFilterAsync_ResourceName_Version_CriteriaFilter_PageSize()
        {
            filterClient = new EthosFilterQueryClient(SampleTestData.API_KEY, SampleTestData.GetMockSequenceForEthosFilterQueryClientWithOK(namedQueryFilterStr));
            var response = await filterClient
                .GetPagesWithNamedQueryFilterAsync(criteriaResourceName, version, new NamedQueryFilter(), 10);
            Assert.NotNull(response);
            _ = Assert.IsAssignableFrom<IEnumerable<EthosResponse>>(response);
        }

    }
}
