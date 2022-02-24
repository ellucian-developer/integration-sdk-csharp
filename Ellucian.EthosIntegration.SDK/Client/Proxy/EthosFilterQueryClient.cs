/*
 * ******************************************************************************
 *   Copyright  2021 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy.Filter;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Client.Proxy
{
    /// <summary>
    /// An EthosProxyClient that provides the ability to submit GET requests supporting filters and/or named queries with support for paging.
    /// </summary>
    public class EthosFilterQueryClient: EthosProxyClient
    {
        // ==========================================================================
        // Attributes
        // ==========================================================================
        /** Prefix value used when specifying criteria filter syntax. */
        public const string CRITERIA_FILTER_PREFIX = "?criteria=";

        /// <summary>
        /// Instantiates this class using the given API key and HttpClient.
        /// </summary>
        /// <param name="apiKey">A valid API key from Ethos Integration. This is required to be a valid 36 character GUID string. 
        /// If it is null, empty, or not in a valid GUID format, then an <code>IllegalArgumentException</code> will be thrown.</param>
        /// <param name="client">A HttpClient. If it is null/empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        public EthosFilterQueryClient( string apiKey, HttpClient client ) : base( apiKey, client )
        {

        }

        /// <summary>
        /// Gets a page of data for the given resource with the given filter. Uses the default version of the resource. 
        /// Makes a non-filter API request if the given criteriaFilterStr is null or blank.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteriaFilterStr">The string resource filter in JSON format contained in the URL, e.g: <pre>?criteria={"names":[{"firstName":"John"}]}</pre></param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according to the requested version and filter of the resource.   
        /// to the requested version and filter of the resource.</returns>
        public async Task<EthosResponse> GetWithCriteriaFilterAsync( string resourceName, string criteriaFilterStr )
        {
            return await GetWithCriteriaFilterAsync( resourceName, DEFAULT_VERSION, criteriaFilterStr );
        }

        /// <summary>
        /// Gets a page of data for the given resource with the given filter. Uses the default version of the resource. 
        /// Makes a non-filter API request if the given namedQueryFilterStr is null or blank.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="namedQueryFilterStr">The string resource filter in JSON format contained in the URL, e.g: <pre>?criteria={"names":[{"firstName":"John"}]}</pre></param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according to the requested version and filter of the resource.   
        /// to the requested version and filter of the resource.</returns>
        public async Task<EthosResponse> GetWithNamedQueryFilterAsync( string resourceName, string namedQueryFilterStr )
        {
            return await GetWithNamedQueryFilterAsync( resourceName, DEFAULT_VERSION, namedQueryFilterStr );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version with the given filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteriaFilterStr">The string resource filter in JSON format contained in the URL, e.g: <pre>?criteria={"names":[{"firstName":"John"}]}</pre></param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according  
        /// to the requested version and filter of the resource.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName is null.</exception>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the criteriaFilterStr is null.</exception>
        /// <exception cref="HttpRequestException">Returns <see cref="HttpRequestException"/> exception if the request fails.</exception>
        public async Task<EthosResponse> GetWithCriteriaFilterAsync( string resourceName, string version, string criteriaFilterStr )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource with criteria filter due to a null or blank resource name." );
            }
            if ( string.IsNullOrWhiteSpace( criteriaFilterStr ) )
            {
                throw new ArgumentNullException( $"Error: Cannot get resource '{ resourceName }' with criteria filter due to a null or blank criteria filter string." );
            }
            criteriaFilterStr = EncodeString( criteriaFilterStr );
            Dictionary<string, string> headers = BuildHeadersMap( version );
            EthosResponse response = await GetAsync( headers, EthosIntegrationUrls.ApiFilter( Region, resourceName, criteriaFilterStr ) );
            return response;
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version with the given filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="namedQueryFilterStr">The string resource filter in JSON format contained in the URL.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according  
        /// to the requested version and filter of the resource.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName is null.</exception>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the namedQuery is null.</exception>
        /// <exception cref="HttpRequestException">Returns <see cref="HttpRequestException"/> exception if the request fails.</exception>
        public async Task<EthosResponse> GetWithNamedQueryFilterAsync( string resourceName, string version, string namedQueryFilterStr )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource with criteria filter due to a null or blank resource name." );
            }
            if ( string.IsNullOrWhiteSpace( namedQueryFilterStr ) )
            {
                throw new ArgumentNullException( $"Error: Cannot get resource '{ resourceName }' with named query due to a null or blank named query string." );
            }
            namedQueryFilterStr = EncodeString( namedQueryFilterStr );
            Dictionary<string, string> headers = BuildHeadersMap( version );
            EthosResponse response = await GetAsync( headers, EthosIntegrationUrls.ApiFilter( Region, resourceName, namedQueryFilterStr ) );
            return response;
        }

        /// <summary>
        ///  Gets a page of data for the given resource by name with the given filter. Uses the default version of the resource.
        ///  Makes a non-filter API request if the given criteriaFilter is null.
        ///  A simple call to criteriaFilter.BuildCriteria() should output the criteria filter portion of the request URL,
        ///  e.g: <code>?criteria={"names":[{"firstName":"John"}]}</code>.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.
        /// A simple call to criteriaFilter.BuildCriteria() should output the criteria filter portion of the request URL,
        /// e.g: <code>?criteria={"names":[{"firstName":"John"}]}</code>.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        public async Task<EthosResponse> GetWithCriteriaFilterAsync( string resourceName, CriteriaFilter criteria )
        {
            return await GetWithCriteriaFilterAsync( resourceName, DEFAULT_VERSION, criteria.BuildCriteria() );
        }

        /// <summary>
        ///  Gets a page of data for the given resource by name with the given filter. Uses the default version of the resource.
        ///  Makes a non-filter API request if the given criteriaFilter is null.
        ///  A simple call to criteriaFilter.BuildCriteria() should output the criteria filter portion of the request URL.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.
        /// A simple call to namedQueryFilter.BuildNamedQuery() should output the criteria filter portion of the request URL,
        /// e.g: <code>?criteria={"names":[{"firstName":"John"}]}</code>.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        public async Task<EthosResponse> GetWithNamedQueryFilterAsync( string resourceName, NamedQueryFilter namedQueryFilter )
        {
            return await GetWithNamedQueryFilterAsync( resourceName, DEFAULT_VERSION, namedQueryFilter );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version with the given filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.
        /// A simple call to criteriaFilter.BuildCriteria() should output the criteria filter portion of the request URL,
        /// e.g: <code>?criteria={"names":[{"firstName":"John"}]}</code>.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according to the requested version and filter of the resource.  
        /// to the requested version and filter of the resource.</returns>
        /// <exception cref="ArgumentNullException">Throws if the given criteriaFilter is null.</exception>
        public async Task<EthosResponse> GetWithCriteriaFilterAsync( string resourceName, string version, CriteriaFilter criteria )
        {
            if ( criteria == null )
            {
                throw new ArgumentNullException( $"Error: Cannot get resource '{ resourceName }' with criteria filter due to a null criteria filter reference." );
            }
            return await GetWithCriteriaFilterAsync( resourceName, version, criteria.BuildCriteria() );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version with the given filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.
        /// A simple call to namedQueryFilter.BuildNamedQuery() should output the named query portion of the request URL.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according to the requested version and filter of the resource.  
        /// to the requested version and filter of the resource.</returns>
        /// <exception cref="ArgumentNullException">Throws if the given criteriaFilter is null.</exception>
        public async Task<EthosResponse> GetWithNamedQueryFilterAsync( string resourceName, string version, NamedQueryFilter namedQueryFilter )
        {
            if ( namedQueryFilter == null )
            {
                throw new ArgumentNullException( $"Error: Cannot get resource '{ resourceName }' with named query due to a null named query reference." );
            }
            return await GetWithNamedQueryFilterAsync( resourceName, version, namedQueryFilter.BuildNamedQuery() );
        }

        /// <summary>
        /// Convenience method to submit a GET request with a single set of criteria filter. This is intended only to be used
        /// for a single set of criteria filter, e.g: <code>?criteria={"names":[{"firstName":"John"}]}</code>, where <b>names</b> is the
        /// criteriaSetName, <b>firstName</b> is the criteriaKey, and <b>John</b> is the criteriaValue. Requests requiring
        /// a more complex criteria filter should first build the Criteria with the necessary criteria, and then call
        /// <code>getWithCriteriaFilterAsync(resourceName, criteriaFilter)</code>.
        /// <p>The parameters criteriaSetName, criteriaKey, and criteriaValue should only specify the values within quotes of the
        /// JSON filter syntax. No JSON syntax (square or angeled braces etc.) should be contained within those parameter values.</p>
        /// <p>Uses the default version of the resource.</p>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteriaSetName">The name of the criteria set that the given criteriaKey and criteriaValue are associated with,
        /// e.g: "<b>names</b>":[{"firstName":"John"}]}, where <b>names</b> is the criteriaSetName associated to the
        /// criteriaKey (firstName) and criteriaValue (John).</param>
        /// <param name="criteriaKey">The JSON label key for the criteria.</param>
        /// <param name="criteriaValue">The value associated with the criteriaKey.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        public async Task<EthosResponse> GetWithSimpleCriteriaValuesAsync( string resourceName, string criteriaSetName, string criteriaKey, string criteriaValue )
        {
            return await GetWithSimpleCriteriaValuesAsync( resourceName, DEFAULT_VERSION, criteriaSetName, criteriaKey, criteriaValue );
        }

        /// <summary>
        /// Convenience method to submit a GET request with a single set of criteria filter. This is intended only to be used
        /// for a single set of criteria filter, e.g: <code>?criteria={"names":[{"firstName":"John"}]}</code>, where <b>names</b> is the
        /// criteriaSetName, <b>firstName</b> is the criteriaKey, and <b>John</b> is the criteriaValue. Requests requiring
        /// a more complex criteria filter should first build the Criteria with the necessary criteria, and then call
        /// <code>getWithCriteriaFilterAsync(resourceName, version, criteriaFilter)</code>.
        /// <p>The parameters criteriaSetName, criteriaKey, and criteriaValue should only specify the values within quotes of the
        /// JSON filter syntax. No JSON syntax (square or angeled braces, etc) should be contained within those parameter values.</p>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteriaSetName">The name of the criteria set that the given criteriaKey and criteriaValue are associated with,
        /// e.g: "<b>names</b>":[{"firstName":"John"}]}, where <b>names</b> is the criteriaSetName associated to the
        /// criteriaKey (firstName) and criteriaValue (John).</param>
        /// <param name="criteriaKey">The JSON label key for the criteria.</param>
        /// <param name="criteriaValue">The value associated with the criteriaKey.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName or criteriaSetName or criteriaKey or criteriaValue is null.</exception>
        public async Task<EthosResponse> GetWithSimpleCriteriaValuesAsync( string resourceName, string version, string criteriaSetName, string criteriaKey, string criteriaValue )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource with filter map due to a null or blank resource name." );
            }
            if ( string.IsNullOrWhiteSpace( criteriaSetName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource due to a null or empty criteria set name parameter." );
            }
            if ( string.IsNullOrWhiteSpace( criteriaKey ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource due to a null or empty criteria key parameter." );
            }
            if ( string.IsNullOrWhiteSpace( criteriaValue ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource due to a null or empty criteria value parameter." );
            }
            var criteriaFilter = new CriteriaFilter().WithSimpleCriteria( criteriaSetName, (criteriaKey, criteriaValue) )
                                                .BuildCriteria();
            return await GetWithCriteriaFilterAsync( resourceName, version, criteriaFilter );
        }

        /// <summary>
        /// Submits a GET request for the given resource and version using the given filterMapStr. The filterMapStr
        /// is intended to support the filter syntax for resources versions 7 and older. An example of a filterMapStr is:
        /// <code>?firstName=James</code>.
        /// <p>This is NOT intended to be used for resource versions after version 7 and/or for criteria filters.</p>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="filterMapStr">A string containing the filter syntax used for request URL filters with resource versions 7 or older.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName or filterMapStr is null.</exception>
        public async Task<EthosResponse> GetWithFilterMapAsync( string resourceName, string version, string filterMapStr )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource with filter map due to a null or blank resource name." );
            }
            if ( string.IsNullOrWhiteSpace( filterMapStr ) )
            {
                throw new ArgumentNullException( $"Error: Cannot get resource '{ resourceName }' with filter map due to a null or blank filter map string." );
            }
            Dictionary<string, string> headers = BuildHeadersMap( version );
            EthosResponse response = await GetAsync( headers, EthosIntegrationUrls.ApiFilter( Region, resourceName, filterMapStr ) );
            return response;
        }

        /// <summary>
        /// Submits a GET request for the given resource and version using the given filterMap. The filterMap
        /// is intended to support the filter syntax for resources versions 7 and older. A FilterMap contains a map of
        /// one or many filter parameter pair(s).  An example of a filterMap string indicating the contents of the map is:
        /// <code>?firstName=James</code>.
        /// <p>This is NOT intended to be used for resource versions after version 7 and/or for criteria filters.</p>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="filterMap">A string containing the filter syntax used for request URL filters with resource versions 7 or older.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName or filterMap is null.</exception>
        public async Task<EthosResponse> GetWithFilterMapAsync( string resourceName, string version, FilterMap filterMap )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get resource with filter map due to a null or blank resource name." );
            }
            if ( filterMap == null )
            {
                throw new ArgumentNullException( $"Error: Cannot get resource '{ resourceName }' with filter map due to a null filter map." );
            }
            return await GetWithFilterMapAsync( resourceName, version, filterMap.ToString() );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified criteria filter. Uses the default version of the resource,
        /// and the page size is derived from the length of the returned response of the request using the criteria filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.
        /// A simple call to criteriaFilter.BuildCriteria() should output the criteria filter portion of the request URL,
        /// e.g: <code>?criteria={"names":[{"firstName":"John"}]}</code>.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        public async Task<List<EthosResponse>> GetPagesWithCriteriaFilterAsync( string resourceName, CriteriaFilter criteria )
        {
            return await GetPagesWithCriteriaFilterAsync( resourceName, DEFAULT_VERSION, criteria, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified namedQueryFilter filter. Uses the default version of the resource,
        /// and the page size is derived from the length of the returned response of the request using the namedQueryFilter filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.
        /// A simple call to namedQueryFilter.BuildNamedQuery() should output the namedQueryFilter filter portion of the request URL.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>
        public async Task<List<EthosResponse>> GetPagesWithNamedQueryFilterAsync( string resourceName, NamedQueryFilter namedQueryFilter )
        {
            return await GetPagesWithNamedQueryFilterAsync( resourceName, DEFAULT_VERSION, namedQueryFilter, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified criteria filter for the given version. Uses the default
        /// page size, which is the length of the returned response of the request using the criteria filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithCriteriaFilterAsync( string resourceName, string version, CriteriaFilter criteria )
        {
            return await GetPagesWithCriteriaFilterAsync( resourceName, version, criteria, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified namedQueryFilter filter for the given version. Uses the default
        /// page size, which is the length of the returned response of the request using the namedQueryFilter filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithNamedQueryFilterAsync( string resourceName, string version, NamedQueryFilter namedQueryFilter )
        {
            return await GetPagesWithNamedQueryFilterAsync( resourceName, version, namedQueryFilter, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified criteria filter and page size. The default version
        /// of the resource is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithCriteriaFilterAsync( string resourceName, CriteriaFilter criteria, int pageSize )
        {
            return await GetPagesWithCriteriaFilterAsync( resourceName, DEFAULT_VERSION, criteria, pageSize );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified namedQueryFilter filter and page size. The default version
        /// of the resource is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <returns>An <code>EthosResponse</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithNamedQueryFilterAsync( string resourceName, NamedQueryFilter namedQueryFilter, int pageSize )
        {
            return await GetPagesWithNamedQueryFilterAsync( resourceName, DEFAULT_VERSION, namedQueryFilter, pageSize );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified criteria filter and page size for the given version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithCriteriaFilterAsync( string resourceName, string version, CriteriaFilter criteria, int pageSize )
        {
            return await GetPagesFromOffsetWithCriteriaFilterAsync( resourceName, version, criteria, pageSize, 0 );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified namedQueryFilter filter and page size for the given version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter named query used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithNamedQueryFilterAsync( string resourceName, string version, NamedQueryFilter namedQueryFilter, int pageSize )
        {
            return await GetPagesFromOffsetWithNamedQueryFilterAsync( resourceName, version, namedQueryFilter, pageSize, 0 );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified criteria filter.
        /// The page size is determined to be the length of the returned response of the request using the criteria filter.
        /// The default version of the resource is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithCriteriaFilterAsync( string resourceName, CriteriaFilter criteria, int offset )
        {
            return await GetPagesFromOffsetWithCriteriaFilterAsync( resourceName, DEFAULT_VERSION, criteria, offset );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified namedQueryFilter filter.
        /// The page size is determined to be the length of the returned response of the request using the namedQueryFilter filter.
        /// The default version of the resource is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="namedQueryFilter"></param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithNamedQueryFilterAsync( string resourceName, NamedQueryFilter namedQueryFilter, int offset )
        {
            return await GetPagesFromOffsetWithNamedQueryFilterAsync( resourceName, DEFAULT_VERSION, namedQueryFilter, offset );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified criteria filter
        /// for the given version. The page size is determined to be the length of the returned response of the request using
        /// the criteria filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithCriteriaFilterAsync( string resourceName, string version, CriteriaFilter criteria, int offset )
        {
            return await GetPagesFromOffsetWithCriteriaFilterAsync( resourceName, version, criteria, DEFAULT_PAGE_SIZE, offset );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified namedQueryFilter filter
        /// for the given version. The page size is determined to be the length of the returned response of the request using
        /// the namedQueryFilter filter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithNamedQueryFilterAsync( string resourceName, string version, NamedQueryFilter namedQueryFilter, int offset )
        {
            return await GetPagesFromOffsetWithNamedQueryFilterAsync( resourceName, version, namedQueryFilter, DEFAULT_PAGE_SIZE, offset );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified criteria filter
        /// and page size for the given version. The default version of the resource is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithCriteriaFilterAsync( string resourceName, CriteriaFilter criteria, int pageSize, int offset )
        {
            return await GetPagesFromOffsetWithCriteriaFilterAsync( resourceName, DEFAULT_VERSION, criteria, pageSize, offset );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified namedQueryFilter
        /// and page size for the given version. The default version of the resource is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithNamedQueryFilterAsync( string resourceName, NamedQueryFilter namedQueryFilter, int pageSize, int offset )
        {
            return await GetPagesFromOffsetWithNamedQueryFilterAsync( resourceName, DEFAULT_VERSION, namedQueryFilter, pageSize, offset );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified criteria filter
        /// and page size for the given version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName or criteriaFilter is null.</exception>
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithCriteriaFilterAsync( string resourceName, string version, CriteriaFilter criteria, int pageSize, int offset )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get pages of resource from offset with criteria filter due to a null or blank resource name." );
            }
            if ( criteria == null )
            {
                throw new ArgumentNullException( $"Error: Cannot get pages of resource '{ resourceName }' from offset with criteria filter due to a null criteria filter reference." );
            }
            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            Pager pager = Pager.Build( pg =>
            {
                pg
                .ForResource( resourceName )
                .ForVersion( version )
                .WithCriteriaFilter( criteria.BuildCriteria() )
                .WithPageSize( pageSize )
                .FromOffSet( offset );
            } );

            pager = await PrepareForPagingAsync( pager );
            pager = ShouldDoPaging( pager, false );
            if ( pager.ShouldDoPaging )
            {
                ethosResponseList = await DoPagingFromOffsetAsync( pager.ResourceName, pager.Version, pager.CriteriaFilter, pager.TotalCount, pager.PageSize, offset );
            }
            else
            {
                ethosResponseList.Add( await GetWithCriteriaFilterAsync( resourceName, version, criteria ) );
            }
            return ethosResponseList;
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified namedQueryFilter
        /// and page size for the given version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName or namedQueryFilter is null.</exception>
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithNamedQueryFilterAsync( string resourceName, string version, NamedQueryFilter namedQueryFilter, int pageSize, int offset )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get pages of resource from offset with named query filter due to a null or blank resource name." );
            }
            if ( namedQueryFilter == null )
            {
                throw new ArgumentNullException( $"Error: Cannot get pages of resource '{ resourceName }' from offset with named query filter due to a null named query filter reference." );
            }
            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            Pager pager = Pager.Build( pg =>
            {
                pg
                .ForResource( resourceName )
                .ForVersion( version )
                .WithNamedQuery( namedQueryFilter.BuildNamedQuery() )
                .WithPageSize( pageSize )
                .FromOffSet( offset );
            } );

            pager = await PrepareForPagingAsync( pager );
            pager = ShouldDoPaging( pager, false );
            if ( pager.ShouldDoPaging )
            {
                ethosResponseList = await DoPagingFromOffsetAsync( pager.ResourceName, pager.Version, pager.NamedQueryFilter, pager.TotalCount, pager.PageSize, offset );
            }
            else
            {
                ethosResponseList.Add( await GetWithNamedQueryFilterAsync( resourceName, version, namedQueryFilter ) );
            }
            return ethosResponseList;
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified filter map and page size for the given version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="filterMap">A previously built FilterMap containing the filter parameters used in the request URL.
        /// A simple call to filterMap.tostring() should output the criteria filter portion of the request URL,
        /// e.g: <code>?firstName=John&amp;lastName=Smith</code>.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithFilterMapAsync( string resourceName, string version, FilterMap filterMap )
        {
            return await GetPagesWithFilterMapAsync( resourceName, version, filterMap, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets all the pages for a given resource using the specified filter map and page size for the given version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="filterMap">A previously built FilterMap containing the filter parameters used in the request URL.
        /// A simple call to filterMap.tostring() should output the criteria filter portion of the request URL,
        /// e.g: <code>?firstName=John&amp;lastName=Smith</code>.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesWithFilterMapAsync( string resourceName, string version, FilterMap filterMap, int pageSize )
        {
            return await GetPagesFromOffsetWithFilterMapAsync( resourceName, version, filterMap, pageSize, 0 );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified filter map and
        /// page size for the given version. The page size is determined to be the length of the returned response of the request using
        /// the filter map.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="filterMap">A previously built FilterMap containing the filter parameters used in the request URL.
        /// A simple call to filterMap.tostring() should output the criteria filter portion of the request URL,
        /// e.g: <code>?firstName=John&amp;lastName=Smith</code>.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithFilterMapAsync( string resourceName, string version, FilterMap filterMap, int offset )
        {
            return await GetPagesFromOffsetWithFilterMapAsync( resourceName, version, filterMap, DEFAULT_PAGE_SIZE, offset );
        }

        /// <summary>
        /// Gets all the pages for a given resource beginning at the given offset index, using the specified filter map and
        /// page size for the given version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="filterMap">A previously built FilterMap containing the filter parameters used in the request URL.
        /// A simple call to filterMap.tostring() should output the criteria filter portion of the request URL,
        /// e.g: <code>?firstName=John&amp;lastName=Smith</code>.</param>
        /// <param name="pageSize">The size (number of rows) of each page returned in the list.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>An <code>List&lt;EthosResponse&gt;</code> containing an initial page (EthosResponse content) of resource data according 
        /// to the requested version and filter of the resource.</returns>  
        public async Task<List<EthosResponse>> GetPagesFromOffsetWithFilterMapAsync( string resourceName, string version, FilterMap filterMap, int pageSize, int offset )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot get pages of resource with filter map due to a null or blank resource name." );
            }
            if ( filterMap == null )
            {
                throw new ArgumentNullException( $"Error: Cannot get pages of resource '{ resourceName }' with filter map due to a null filter map reference." );
            }
            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            Pager pager = Pager.Build( pg =>
            {
                pg
                .ForResource( resourceName )
                .ForVersion( version )
                .WithFilterMap( filterMap.ToString() )
                .WithPageSize( pageSize )
                .FromOffSet( offset );
            } );

            pager = await PrepareForPagingAsync( pager );
            pager = ShouldDoPaging( pager, false );
            if ( pager.ShouldDoPaging )
            {
                ethosResponseList = await DoPagingFromOffsetAsync( pager.ResourceName, pager.Version, pager.FilterMap, pager.TotalCount, pager.PageSize, offset );
            }
            else
            {
                ethosResponseList.Add( await GetWithFilterMapAsync( resourceName, version, filterMap ) );
            }
            return ethosResponseList;
        }

        /// <summary>
        /// Gets the total count of resources available using the given criteriaFilter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <returns>The number of resource instances available when making a GET request using the given criteriaFilter, or 0 if the
        /// given resourceName or criteriaFilter is null.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName is null.</exception>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the criteriaFilterStr is null.</exception>
        public async Task<int> GetTotalCountAsync( string resourceName, CriteriaFilter criteria )
        {
            return await GetTotalCountAsync( resourceName, DEFAULT_VERSION, criteria );
        }

        /// <summary>
        /// Gets the total count of resources available using the given namedQueryFilter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.</param>
        /// <returns>The number of resource instances available when making a GET request using the given namedQueryFilter, or 0 if the
        /// given resourceName or namedQueryFilter is null.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName is null.</exception>
        public async Task<int> GetTotalCountAsync( string resourceName, NamedQueryFilter namedQueryFilter )
        {
            return await GetTotalCountAsync( resourceName, DEFAULT_VERSION, namedQueryFilter );
        }

        /// <summary>
        /// Gets the total count of resources available using the given criteriaFilter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="criteria">A previously built Criteria containing the filter criteria used in the request URL.</param>
        /// <returns>The number of resource instances available when making a GET request using the given criteriaFilter, or 0 if the
        /// given resourceName or criteriaFilter is null.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName is null.</exception>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the criteriaFilterStr is null.</exception>
        public async Task<int> GetTotalCountAsync( string resourceName, string version, CriteriaFilter criteria )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return default( int );
            }
            if ( criteria == null )
            {
                return default( int );
            }
            EthosResponse ethosResponse = await GetWithCriteriaFilterAsync( resourceName, version, criteria.BuildCriteria() );
            string totalCount = GetHeaderValue( ethosResponse, HDR_X_TOTAL_COUNT );
            if ( int.TryParse( totalCount, out int count ) )
            {
                return count;
            }
            return default( int );
        }

        /// <summary>
        /// Gets the total count of resources available using the given namedQueryFilter.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="namedQueryFilter">A previously built namedQueryFilter containing the filter used in the request URL.</param>
        /// <returns>The number of resource instances available when making a GET request using the given namedQueryFilter, or 0 if the
        /// given resourceName or namedQueryFilter is null.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName is null.</exception>
        public async Task<int> GetTotalCountAsync( string resourceName, string version, NamedQueryFilter namedQueryFilter )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return default( int );
            }
            if ( namedQueryFilter == null )
            {
                return default( int );
            }
            EthosResponse ethosResponse = await GetWithNamedQueryFilterAsync( resourceName, version, namedQueryFilter.BuildNamedQuery() );
            string totalCount = GetHeaderValue( ethosResponse, HDR_X_TOTAL_COUNT );
            if ( int.TryParse( totalCount, out int count ) )
            {
                return count;
            }
            return default( int );
        }

        /// <summary>
        /// Gets the total count of resources available using the given filterMap.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version header to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="filterMap">A previously built FilterMap containing the filter parameters used in the request URL.
        /// A simple call to filterMap.Tostring() should output the criteria filter portion of the request URL,
        /// e.g: <code>?firstName=John&amp;lastName=Smith</code>.</param>
        /// <returns>The number of resource instances available when making a GET request using the given criteriaFilter, or 0 if the
        /// given resourceName or criteriaFilter is null.</returns>
        /// <exception cref="ArgumentNullException">Returns <see cref="ArgumentNullException"/> exception if the resourceName is null.</exception>
        public async Task<int> GetTotalCountAsync( string resourceName, string version, FilterMap filterMap )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return default( int );
            }
            if ( filterMap == null )
            {
                return default( int );
            }
            EthosResponse ethosResponse = await GetWithFilterMapAsync( resourceName, version, filterMap.ToString() );
            string totalCount = GetHeaderValue( ethosResponse, HDR_X_TOTAL_COUNT );
            if ( int.TryParse( totalCount, out int count ) )
            {
                return count;
            }
            return default( int );
        }

        /// <summary>
        ///  <p><b>Intended to be used internally within the SDK.</b>
        /// </p>
        /// Overrides the <code>prepareForPaging()</code> method in the EthosProxyClient super class.
        /// <p>
        /// Uses the given pager object to prepare for paging operations. The pager object is used to contain various
        /// fields required for paging. If the given pager is null, returns the same pager. Sets default values for the
        /// version and offset within the pager as needed and makes an initial resource call using the provided pager filter
        /// to get metadata about the resource used for paging. Also sets the page size and the total count within the pager,
        /// and encodes the filter within the pager.</p>
        /// </summary>
        /// <param name="pager">The Pager object used holding the required fields for paging.</param>
        /// <returns>The same pager object with the version and offset validated, the page size and total count set, and the filter encoded.</returns>
        protected new async Task<Pager> PrepareForPagingAsync( Pager pager )
        {
            if ( pager == null )
            {
                return pager;
            }
            if ( string.IsNullOrWhiteSpace( pager.Version ) )
            {
                pager.Version = DEFAULT_VERSION;
            }
            if ( pager.Offset < 1 )
            {
                pager.Offset = 0;
            }

            pager = await PreparePagerForTotalCountAsync( pager );

            // Set the pageSize.
            pager = PreparePagerForPageSize( pager );

            // Encode the criteriaFilter if it is not null.
            if ( pager.CriteriaFilter != null )
            {
                pager.CriteriaFilter = EncodeString( pager.CriteriaFilter );
            }

            // Encode the NamedQueryFilter if it is not null.
            if ( pager.NamedQueryFilter != null )
            {
                pager.NamedQueryFilter = EncodeString( pager.NamedQueryFilter );
            }
            return pager;
        }

        /// <summary>
        /// <b>Intended to be used internally by the SDK.</b>
        /// <p>Prepares the pager object for the total count required for paging calculations.
        /// The total count is derived from the response x-total-count header after making an initial request using filters</p>
        /// (in this case).
        /// </summary>
        /// <param name="pager">The Pager object used holding the required fields for paging.</param>
        /// <returns>The pager object containing the total count. If neither a criteria filter, a named query nor a filter map is specified
        /// in the pager, then the total count will be 0.</returns>
        protected async Task<Pager> PreparePagerForTotalCountAsync( Pager pager )
        {
            EthosResponse ethosResponse = null;
            if ( pager.CriteriaFilter != null )
            {
                ethosResponse = await GetWithCriteriaFilterAsync( pager.ResourceName, pager.Version, pager.CriteriaFilter );
                pager.EthosResponse = ethosResponse;
            }
            else if ( pager.NamedQueryFilter != null )
            {
                ethosResponse = await GetWithNamedQueryFilterAsync( pager.ResourceName, pager.Version, pager.NamedQueryFilter );
                pager.EthosResponse = ethosResponse;
            }
            else if ( pager.FilterMap != null )
            {
                ethosResponse = await GetWithFilterMapAsync( pager.ResourceName, pager.Version, pager.FilterMap );
                pager.EthosResponse = ethosResponse;
            }
            else
            {
                await base.PrepareForPagingAsync( pager );
            }
            if ( ethosResponse != null )
            {
                string totalCount = GetHeaderValue( ethosResponse, HDR_X_TOTAL_COUNT );
                if ( int.TryParse( totalCount, out int count ) )
                {
                    pager.TotalCount = count;
                }
            }
            return pager;
        }

        /// <summary>
        /// <b>Intended to be used internally by the SDK.</b>
        /// <p>
        /// If the page size specified in the pager is &lt;= DEFAULT_PAGE_SIZE, then this method prepares the pager object for the
        /// page size required for paging calculations. The page size is derived from the response body length after making an initial request using filters
        /// (in this case). If the response is null, the DEFAULT_MAX_PAGE_SIZE is used. If the response body is null, the
        /// x-max-page-size header is used.</p>
        /// <p>If the page size specified in the pager is &gt; DEFAULT_PAGE_SIZE, this method does nothing and just returns the given pager.</p>
        /// </summary>
        /// <param name="pager">The pager object used to contain the page size for paging operations.</param>
        /// <returns>The pager object containing the page size.</returns>
        protected Pager PreparePagerForPageSize( Pager pager )
        {
            if ( pager.PageSize <= DEFAULT_PAGE_SIZE )
            {
                if ( pager.EthosResponse == null )
                {
                    pager.PageSize = DEFAULT_MAX_PAGE_SIZE; // Set the page size to the MAX default because there is no ethosResponse.
                }
                // Set the pageSize from the response body length, if pageSize is <= DEFAULT_PAGE_SIZE.
                else if ( !string.IsNullOrWhiteSpace( pager.EthosResponse.Content ) && pager.EthosResponse.GetContentAsJson() != null )
                {
                    int pageSize = pager.EthosResponse.GetContentCount();
                    pager.PageSize = pageSize;
                }
                else
                {
                    string maxPageSizeStr = GetHeaderValue( pager.EthosResponse, HDR_X_MAX_PAGE_SIZE );
                    if ( !string.IsNullOrWhiteSpace( maxPageSizeStr ) )
                    {
                        if ( int.TryParse( maxPageSizeStr, out int pageSize ) )
                        {
                            pager.PageSize = pageSize;
                        }
                    }
                    else
                    {
                        pager.PageSize = DEFAULT_MAX_PAGE_SIZE;
                    }
                }
            }
            return pager;
        }

        /// <summary>
        /// <b>Used internally by the SDK.</b>
        /// <p>
        /// Encodes the given criteriaFilterStr. Supports criteria filter strings that begin with the CRITERIA_FILTER_PREFIX
        /// value "?criteria=", and also those that do not. Encodes only the JSON criteria portion of the filter string, removing
        /// the CRITERIA_FILTER_PREFIX portion if the filter string starts with it. If the filter string does not start with
        /// the CRITERIA_FILTER_PREFIX, the criteriaFilterStr string is simply encoded.</p>
        /// <p>Returns a criteria filter string that begins with the CRITERIA_FILTER_PREFIX, with the JSON filter portion of the string
        /// encoded. Uses UTF-8 encoding.</p>
        /// </summary>
        /// <param name="criteriaFilterStr">The criteria filter string to encode.</param>
        /// <returns>A criteria filter string beginning with the CRITERIA_FILTER_PREFIX with the JSON filter syntax portion of the
        /// string encoded in UTF-8.</returns>
        private string EncodeString( string criteriaFilterStr )
        {
            StringBuilder sb = new StringBuilder();
            string jsonCriteriaStr;
            bool isNamedQuery = false;
            if ( criteriaFilterStr.StartsWith( CRITERIA_FILTER_PREFIX ) )
            {
                // It starts with "?criteria=", so substring the rest of the filter and encode it.
                jsonCriteriaStr = criteriaFilterStr [ ( criteriaFilterStr.IndexOf( "=" ) + 1 ).. ];
            }
            else
            {
                jsonCriteriaStr = criteriaFilterStr;
                isNamedQuery = true;
            }
            jsonCriteriaStr = System.Web.HttpUtility.UrlEncode( jsonCriteriaStr, Encoding.UTF8 );
            if ( !isNamedQuery ) sb.Append( CRITERIA_FILTER_PREFIX );
            sb.Append( jsonCriteriaStr );
            return sb.ToString();
        }
    }
}
