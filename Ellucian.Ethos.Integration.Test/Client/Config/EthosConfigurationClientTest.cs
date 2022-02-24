/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Test;
using Ellucian.Ethos.Integration.Client.Config;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Config.Test
{
    public class EthosConfigurationClientTest
    {
        readonly string resources = string.Empty;
        readonly string resWithFiltersAndNamedQueries = string.Empty;
        EthosConfigurationClient client;

        public EthosConfigurationClientTest()
        {
            resources = SampleTestDataRepository.GetAvailableResourcesData();
            resWithFiltersAndNamedQueries = SampleTestDataRepository.GetAvailableResourcesWithFiltersAndNamedQueriesData();
        }

        [Fact]
        public void FilterAvailableResources()
        {
            JArray availableResources = JArray.Parse( resources );
            JArray desiredResources = JArray.Parse( @"[
				{
					'resourceName': 'address-types',
					'applicationId': 'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'
				},
				{
					'resourceName': 'course-levels',
					'applicationId': '2f977334-edfd-4408-a227-21663664abc9'
				}
			]" );

            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            JArray response = client.FilterAvailableResources( availableResources, desiredResources );
            Assert.Equal( 2, response.Count );
        }

        [Fact]
        public void FilterAvailableResources_MissingResourceName()
        {
            JArray availableResources = JArray.Parse( resources );
            JArray desiredResources = JArray.Parse( @"[
				{
					'resourceName': 'address-types',
					'applicationId': 'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'
				},
				{
					'resourceName': 'persons',
					'applicationId': 'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'
				}
			]" );

            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            JArray response = client.FilterAvailableResources( availableResources, desiredResources );
            Assert.Single( response );
        }

        [Fact]
        public void FilterAvailableResources_MissingAppId()
        {
            JArray availableResources = JArray.Parse( resources );
            JArray desiredResources = JArray.Parse( @"[
				{
					'resourceName': 'address-types',
					'applicationId': 'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'
				},
				{
					'resourceName': 'buildings',
					'applicationId': '1fe0366e-d223-4164-9454-f6b891b88af5'
				}
			]" );

            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            JArray response = client.FilterAvailableResources( availableResources, desiredResources );
            Assert.Single( response );
        }

        [Fact]
        public void FilterAvailableResources_NoResults()
        {
            JArray availableResources = JArray.Parse( resources );
            JArray desiredResources = JArray.Parse( @"[
				{
					'resourceName': 'addresses',
					'applicationId': 'eef6b098-17fe-4d9e-b8f8-5d949420ffa6'
				},
				{
					'resourceName': 'buildings',
					'applicationId': '1fe0366e-d223-4164-9454-f6b891b88af5'
				}
			]" );

            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            JArray response = client.FilterAvailableResources( availableResources, desiredResources );
            Assert.Empty( response );
        }

        [Fact]
        public void FilterAvailableResource_ByName()
        {
            JArray availableResources = JArray.Parse( SampleTestDataRepository.GetResourceForPersons() );

            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesForFilterAvailableResources() );
            JArray response = client.FilterAvailableResources( availableResources, "persons" );
            Assert.Single( response );
        }

        [Fact]
        public void FilterAvailableResource_ByName_NoResults()
        {
            JArray availableResources = JArray.Parse( SampleTestDataRepository.GetResourceForPersons() );

            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesForFilterAvailableResources() );
            JArray response = client.FilterAvailableResources( availableResources, "BAD_RESOURCE_NAME" );
            Assert.Empty( response );
        }

        #region Filters and Named Queries

        [Fact]
        public void GetFiltersAndNamedQueriesAsync_ResourceName_Null_Excpection()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetFiltersAndNamedQueriesAsync( "", "" ) );
        }

        [Fact]
        public async Task GetFiltersAndNamedQueriesAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetFiltersAndNamedQueriesAsync( "persons" );
            Assert.NotNull( actual );
            Assert.True( actual.ContainsKey( "resourceName" ) );
            Assert.NotNull( actual.GetValue( "resourceName" ) );
            Assert.True( actual.ContainsKey( "version" ) );
            Assert.NotNull( actual.GetValue( "version" ) );
            Assert.True( actual.ContainsKey( "filters" ) );
            Assert.NotNull( actual.GetValue( "filters" ) );
            Assert.True( actual.ContainsKey( "namedQueries" ) );
            Assert.NotNull( actual.GetValue( "namedQueries" ) );
        }

        [Fact]
        public async Task GetResourceDetailsAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetResourceDetailsAsync( "persons" );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetResourceDetailsAsJsonAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetResourceDetailsAsJsonAsync( "persons" );
            Assert.NotNull( actual );
            foreach ( JObject item in actual )
            {
                Assert.True( item.ContainsKey( "appId" ) );
                Assert.True( item.ContainsKey( "appName" ) );
                Assert.True( item.ContainsKey( "resource" ) );
            }
        }

        [Fact]
        public async Task GetResourceDetailsJsonAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetResourceDetailsJsonAsync( "persons" );
            Assert.NotNull( actual );
            foreach ( JObject item in actual )
            {
                Assert.True( item.ContainsKey( "appId" ) );
                Assert.True( item.ContainsKey( "appName" ) );
                Assert.True( item.ContainsKey( "resource" ) );
            }
        }

        [Fact]
        public async Task GetVersionsOfResourceAsStringsAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetVersionsOfResourceAsStringsAsync( "persons" );
            Assert.NotNull( actual );
            _ = Assert.IsAssignableFrom<IEnumerable<string>>( actual );
        }

        [Fact]
        public async Task GetFiltersAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetFiltersAsync( "persons" );
            Assert.NotNull( actual );
        }

        [Fact]
        public async Task GetLatestVersionHeaderAsync()
        {
            string expected = "application/vnd.hedtech.integration.v12.3.0+json";
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetLatestVersionHeaderAsync( "persons" );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public async Task GetFiltersAsyncWith_Version()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetFiltersAsync( "persons", "application/vnd.hedtech.integration.v12.1.0+json" );
            Assert.NotNull( actual );
            _ = Assert.IsAssignableFrom<IEnumerable<string>>( actual );
        }

        [Fact]
        public async Task GetNamedQueriesAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetNamedQueriesAsync( "persons" );
            Assert.NotNull( actual );
            _ = Assert.IsAssignableFrom<Dictionary<string, IEnumerable<string>>>( actual );
        }

        [Fact]
        public async Task GetNamedQueriesAsync_Version()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResourcesWithFiltersAndNamedQueriesData() );

            var actual = await client.GetNamedQueriesAsync( "persons", "application/vnd.hedtech.integration.v12.1.0+json" );
            Assert.NotNull( actual );
            _ = Assert.IsAssignableFrom<Dictionary<string, IEnumerable<string>>>( actual );
        }

        [Fact]
        public void GetVersionHeadersForAppAsync_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetVersionHeadersForAppAsync( "" ) );
        }

        [Fact]
        public async Task GetVersionHeadersForAppAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            var actual = await client.GetVersionHeadersForAppAsync( "address-types" );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetVersionsForAppAsync_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetVersionsForAppAsync( "" ) );
        }

        [Fact]
        public async Task GetVersionsForAppAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            var actual = await client.GetVersionsForAppAsync( "address-types" );
            Assert.NotNull( actual );
        }

        [Fact]
        public void GetMajorVersionsOfResourceAsync_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetMajorVersionsOfResourceAsync( "" ) );
        }

        [Fact]
        public async Task GetMajorVersionsOfResourceAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.GetMajorVersionsOfResourceAsync( "address-types" );
            Assert.NotNull( actual );
        }

        [Fact]
        public void IsResourceVersionSupportedAsync_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.IsResourceVersionSupportedAsync( "" ) );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_True()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", 6, null, null );
            Assert.True( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_With_All_Zeros_False()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", 0, 0, 0 );
            Assert.False( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_With_Major_Zero_False()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", 0, null, null );
            Assert.False( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_With_Minor_Zero_False()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", null, 0, null );
            Assert.False( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_With_Patch_Zero_False()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", null, null, 0 );
            Assert.False( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_With_All_Nulls_False()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", null, null, null );
            Assert.False( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_FullHeader_True()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", "application/vnd.hedtech.integration.v6+json" );
            Assert.True( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_FullHeader_False()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", "application/vnd.hedtech.integration.vBad+json" );
            Assert.False( actual );
        }

        [Fact]
        public void IsResourceVersionSupportedAsync_FullHeader_EmptyResourceName_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.IsResourceVersionSupportedAsync( "", "application/vnd.hedtech.integration.v6+json" ) );
        }

        [Fact]
        public void IsResourceVersionSupportedAsync_FullHeader_EmptyFullVersioHeader_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.IsResourceVersionSupportedAsync( "address-types", " " ) );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_SemVer_True()
        {
            SemVer semVer = new SemVer() { Major = 6, Minor = 1, Patch = 0 };
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", semVer );
            Assert.True( actual );
        }

        [Fact]
        public async Task IsResourceVersionSupportedAsync_SemVer_BadMinor_False()
        {
            SemVer semVer = new SemVer() { Major = 6, Minor = -1, Patch = 0 };
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.IsResourceVersionSupportedAsync( "address-types", semVer );
            Assert.False( actual );
        }

        [Fact]
        public void GetVersionHeadersOfResourceAsStringsAsync_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetVersionHeadersOfResourceAsStringsAsync( "      " ) );
        }

        [Fact]
        public async Task GetVersionHeadersOfResourceAsStringsAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.GetVersionHeadersOfResourceAsStringsAsync( "address-types" );
            Assert.NotNull( actual );
            Assert.IsAssignableFrom<IEnumerable<string>>( actual );
        }

        [Fact]
        public void GetVersionHeadersOfResourceAsync_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetVersionHeadersOfResourceAsync( "      " ) );
        }

        [Fact]
        public async Task GetVersionHeadersOfResourceAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.GetVersionHeadersOfResourceAsync( "address-types" );
            Assert.NotNull( actual );
            Assert.IsType<JArray>( actual );
        }

        [Fact]
        public void GetVersionHeaderAsync_ArgumentNullException()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            _ = Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetVersionHeaderAsync( "      " ) );
        }

        [Fact]
        public async Task GetVersionHeaderAsync_Match_True()
        {
            SemVer semVer = new SemVer() { Major = 6, Minor = 1, Patch = 0 };
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.GetVersionHeaderAsync( "address-types", semVer );
            Assert.NotNull( actual );
            Assert.Equal( "application/vnd.hedtech.integration.v6.1.0+json", actual );
        }

        [Fact]
        public void GetVersionHeaderAsync_Match_UnsupportedVersionException()
        {
            SemVer semVer = new SemVer() { Major = 6, Minor = -1, Patch = -2 };
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            _ = Assert.ThrowsAsync<UnsupportedVersionException>( async () => await client.GetVersionHeaderAsync( "address-types", semVer ) );
        }

        [Fact]
        public async Task GetAppConfigAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            var actual = await client.GetAppConfigAsync();
            Assert.NotNull( actual );
            Assert.IsType<string>( actual );
        }

        [Fact]
        public async Task GetAppConfigJsonAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            var actual = await client.GetAppConfigJsonAsync();
            Assert.NotNull( actual );
            Assert.IsType<JObject>( actual );
        }

        [Fact]
        public async Task GetAllAvailableResourcesAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            var actual = await client.GetAllAvailableResourcesAsync();
            Assert.NotNull( actual );
            Assert.IsType<string>( actual );
        }

        [Fact]
        public async Task GetAllAvailableResourcesAsJsonAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAvailableResources() );
            var actual = await client.GetAllAvailableResourcesAsJsonAsync();
            Assert.NotNull( actual );
            Assert.IsType<JArray>( actual );
        }

        [Fact]
        public async Task GetAvailableResourcesForAppAsJsonAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            var actual = await client.GetAvailableResourcesForAppAsJsonAsync();
            Assert.NotNull( actual );
            Assert.IsType<JArray>( actual );
        }

        [Fact]
        public async Task GetAvailableResourcesForAppAsync()
        {
            client = new EthosConfigurationClient( SampleTestDataRepository.API_KEY, SampleTestDataRepository.GetMockAppConfigFilterAvailableResources() );
            var actual = await client.GetAvailableResourcesForAppAsync();
            Assert.NotNull( actual );
            Assert.IsType<string>( actual );
        }


        #endregion
    }
}
