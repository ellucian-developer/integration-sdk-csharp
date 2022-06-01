/*
 * ******************************************************************************
 *   Copyright  2020-2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Client.Proxy
{
    /// <summary>
    /// An EthosClient used to retrieve data from the Ethos Integration Proxy API.
    /// <para>
    /// Supports( but not limited to ) the following functionality:
    /// <ul>
    ///     <li>Getting data for a given Ethos resource</li>
    ///     <li>Getting an Ethos resource by ID (GUID)</li>
    ///     <li>Getting the page size and/or total count for a resource</li>
    /// </ul>
    /// </para>
    /// 
    /// Supports paging for data in the following formats:
    /// <ul>
    ///     <li>List&lt;<see cref="EthosResponse"/>&gt; a list of EthosResponse objects</li>
    ///     <li>List&lt;<see cref="string"/>&gt; a list of strings</li>
    ///     <li>List&lt;<see cref="JArray"/>&gt; a list of Newtonsoft JArray objects</li>
    /// </ul>
    /// Each item in the list represents one page of data.Each page of data can contain many rows according to the page size.
    /// 
    /// <para>
    /// <b>NOTE: None of the methods in this class should be used to bulk load Ethos data. Such is NOT the intent of this SDK.
    /// It is possible that long running process times could result and/or <see cref="OutOfMemoryException" />s could occur if trying to get
    /// a large quantity of data. Instead, the Ethos bulk loading solution should be used for loading data in Ethos data model format in bulk.</b></para>
    /// </summary>
    public class EthosProxyClient : EthosClient
    {
        /// <summary>
        /// Default value for the accept and content-type header.
        /// </summary>
        protected readonly string DEFAULT_VERSION = "application/json";

        /// <summary>
        /// Default value for page size (limit).
        /// </summary>
        protected readonly int DEFAULT_PAGE_SIZE = 0;

        /// <summary>
        /// Default value for max-page-size header.
        /// </summary>
        protected readonly int DEFAULT_MAX_PAGE_SIZE = 500;

        /// <summary>
        /// Response header for the current date of the response.
        /// </summary>
        public const string HDR_DATE = "Date";

        /// <summary>
        /// Response header for the content-type.
        /// </summary>
        public const string HDR_CONTENT_TYPE = "Content-Type";

        /// <summary>
        /// Response header for the total count (total number of rows) for the given resource.
        /// </summary>
        public const string HDR_X_TOTAL_COUNT = "x-total-count";

        /// <summary>
        /// Response header for the application designation in Ethos Integration.
        /// </summary>
        public const string HDR_APPLICATION_CONTEXT = "x-application-context";

        /// <summary>
        /// Response header for the max page size. Specifies the maximum number of resources returned in a response.
        /// </summary>
        public const string HDR_X_MAX_PAGE_SIZE = "x-max-page-size";

        /// <summary>
        /// Response header for the version of the Ethos resource (data-model).
        /// </summary>
        public const string HDR_X_MEDIA_TYPE = "x-media-type";

        /// <summary>
        /// Response header for content restricted.
        /// </summary>
        public const string HDR_X_CONTENT_RESTRICTED = "x-content-restricted";

        /// <summary>
        /// Response header for the application ID of the application used in Ethos Integration.
        /// </summary>
        public const string HDR_HEDTECH_ETHOS_INTEGRATION_APPLICATION_ID = "hedtech-ethos-integration-application-id";

        /// <summary>
        /// Response header for the application name of the application used in Ethos Integration.
        /// </summary>
        public const string HDR_HEDTECH_ETHOS_INTEGRATION_APPLICATION_NAME = "hedtech-ethos-integration-application-name";

        /// <summary>
        /// Date format.
        /// </summary>
        public const string DATE_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ssK";

        /// <summary>
        /// Converts <see cref="EthosResponse"/> to string <see cref="JArray"/> or <see cref="JObject"/>.
        /// </summary>
        private readonly EthosResponseConverter ethosResponseConverter = new EthosResponseConverter();

        /// <summary>
        /// Constructs an EthosProxyClient using the given API key.
        /// Note that the preferred way to get an instance of this class is through the <see cref="EthosClientBuilder"/>.
        /// </summary>
        /// <param name="apiKey">A valid API key from Ethos Integration. This is required to be a valid 36 character GUID string.
        /// If it is null, empty, <see cref="ArgumentNullException"/> will be thrown or not in a valid GUID format, then a <see cref="FormatException"/> will be thrown.</param>
        /// <param name="client">A HttpClient. If it is null/empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        public EthosProxyClient( string apiKey, HttpClient client ) : base( apiKey, client )
        {

        }

        #region POST

        /// <summary>
        /// Submits a POST request for the given resourceName with the given requestBody. Uses the default version.
        /// The requestBody should be a string in JSON format.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="requestBody">The body of the request to POST for the given resource.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this POST operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        public async Task<EthosResponse> PostAsync( string resourceName, string requestBody )
        {
            return await PostAsync( resourceName, DEFAULT_VERSION, requestBody );
        }

        /// <summary>
        /// Submits a POST request for the given resourceName with the given requestBody. The requestBody should be a string
        /// in JSON format.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="version">The full version header value of the resource used for this POST request.</param>
        /// <param name="requestBody">The body of the request to POST for the given resource.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this POST operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBody"/> is passed as null or empty or white space.</exception>
        public async Task<EthosResponse> PostAsync( string resourceName, string version, string requestBody )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot submit a POST request due to a null or blank resourceName parameter." );
            }
            if ( string.IsNullOrWhiteSpace( requestBody ) )
            {
                throw new ArgumentNullException(
                    $"Error: Cannot submit a POST request for resourceName { resourceName } due to a null or blank requestBody parameter."
                );
            }
            var headers = BuildHeadersMap( version );
            string url = EthosIntegrationUrls.Api( Region, resourceName, null );
            return await base.PostAsync( headers, url, requestBody );
        }

        /// <summary>
        /// Submits a POST request for the given resourceName with the given requestBodyNode. Uses the default version.
        /// This is a convenience method equivalent to <pre>Post(resourceName, requestBodyNode.toString())</pre>.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param> 
        /// <param name="requestBodyNode">The body of the request to POST for the given resource as a JsonNode.</param> 
        /// <returns>An EthosResponse containing the instance of the resource that was added by this POST operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBodyNode"/> is passed as null.</exception>
        public async Task<EthosResponse> PostAsync( string resourceName, JObject requestBodyNode )
        {
            return await PostAsync( resourceName, DEFAULT_VERSION, requestBodyNode );
        }

        /// <summary>
        /// Submits a POST request for the given resourceName with the given requestBodyNode. Uses the default version.
        /// This is a convenience method equivalent to <pre>Post(resourceName, requestBodyNode.toString())</pre>.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="version">The full version header value of the resource used for this POST request.</param>
        /// <param name="requestBodyNode">The body of the request to POST for the given resource as a JsonNode.</param> 
        /// <returns>An EthosResponse containing the instance of the resource that was added by this POST operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBodyNode"/> is passed as null.</exception>
        public async Task<EthosResponse> PostAsync( string resourceName, string version, JObject requestBodyNode )
        {
            ArgumentNullException.ThrowIfNull( requestBodyNode, $"Error: Cannot submit a POST request for resource {resourceName} due to a null or blank {nameof( requestBodyNode )} parameter." );           

            return await PostAsync( resourceName, version, requestBodyNode.ToString() );
        }

        /// <summary>
        /// Submits a POST request for the given resourceName with the given requestBody. The requestBody should be a string in JSON format.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="version">The full version header value of the resource used for this POST request.</param>
        /// <param name="requestBody">The body of the request to POST for the given resource.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this POST operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBody"/> is passed as null or empty or white space.</exception>
        public async Task<EthosResponse> PostQapiAsync( string resourceName, string requestBody, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( $"Error: Cannot submit a POST request due to a null or blank {nameof( resourceName )} parameter." );
            }

            if ( string.IsNullOrWhiteSpace( requestBody ) )
            {
                throw new ArgumentNullException(
                    $"Error: Cannot submit a POST request for resource {resourceName} due to a null or blank {nameof( requestBody )} parameter."
                );
            }
            var headers = BuildHeadersMap( version );
            string url = EthosIntegrationUrls.Qapi( Region, resourceName );
            return await base.PostAsync( headers, url, requestBody );
        }

        #endregion

        #region PUT

        /// <summary>
        /// Submits a PUT request for the given resourceName to update a resource with the given requestBody. Uses the default version.
        /// The requestBody should be a string in JSON format.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="resourceId">The unique id (GUID) for the given resource, as required when making a PUT/update request.</param> 
        /// <param name="requestBody">The body of the request to PUT/update for the given resource.</param> 
        /// <returns>An EthosResponse containing the instance of the resource that was added by this PUT operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceId"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBody"/> is passed as null or empty or white space.</exception>
        public async Task<EthosResponse> PutAsync( string resourceName, string resourceId, string requestBody )
        {
            return await PutAsync( resourceName, resourceId, DEFAULT_VERSION, requestBody );
        }

        /// <summary>
        /// Submits a PUT/update request for the given resourceName with the given requestBody. The requestBody should be a string
        /// in JSON format.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="resourceId">The unique ID for the given resource, as required when making a PUT/update request.</param>
        /// <param name="version">The full version header value of the resource used for this PUT/update request.</param>
        /// <param name="requestBody">The body of the request to PUT/update for the given resource.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this PUT operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceId"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBody"/> is passed as null or empty or white space.</exception>
        public async Task<EthosResponse> PutAsync( string resourceName, string resourceId, string version, string requestBody )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot submit a PUT request due to a null or blank resourceName parameter." );
            }

            if ( string.IsNullOrWhiteSpace( requestBody ) )
            {
                throw new ArgumentNullException(
                    $"Error: Cannot submit a PUT request for resourceName { resourceName } due to a null or blank requestBody parameter."
                );
            }

            var headers = BuildHeadersMap( version );
            string url = EthosIntegrationUrls.Api( Region, resourceName, resourceId );
            return await base.PutAsync( headers, url, requestBody );
        }

        /// <summary>
        /// Submits a PUT request for the given resourceName to update a resource with the given requestBodyNode. Uses the default version.
        /// This is a convenience method equivalent to <pre>put(resourceName, requestBodyNode.toString())</pre>.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="resourceId">The unique id (GUID) for the given resource, as required when making a PUT/update request.</param>
        /// <param name="requestBodyNode">The body of the request to PUT/update for the given resource as a JsonNode.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this PUT operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceId"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBodyNode"/> is passed as null.</exception>
        public async Task<EthosResponse> PutAsync( string resourceName, string resourceId, JObject requestBodyNode )
        {
            return await PutAsync( resourceName, resourceId, DEFAULT_VERSION, requestBodyNode.ToString() );
        }

        /// <summary>
        /// Submits a PUT request for the given resourceName to update a resource with the given requestBodyNode. Uses the default version.
        /// This is a convenience method equivalent to <pre>put(resourceName, requestBodyNode.toString())</pre>.
        /// </summary>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="resourceId">The unique id (GUID) for the given resource, as required when making a PUT/update request.</param>
        /// <param name="version">The full version header value of the resource used for this PUT/update request.</param>
        /// <param name="requestBodyNode">The body of the request to PUT/update for the given resource as a JsonNode.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this PUT operation.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceId"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBodyNode"/> is passed as null.</exception>
        public async Task<EthosResponse> PutAsync( string resourceName, string resourceId, string version, JObject requestBodyNode )
        {
            if ( requestBodyNode == null )
            {
                throw new ArgumentNullException(
                    $"Error: Cannot submit a PUT request for resourceName { resourceName } due to a null or blank requestBody parameter."
                );
            }
            return await PutAsync( resourceName, resourceId, version, requestBodyNode );
        }

        #endregion

        #region DELETE
        /// <summary>
        /// Deletes an instance of the given resource by the given id.
        /// </summary>
        /// <param name="resourceName">The name of the resource to delete an instance of identified by the given id.</param>
        /// <param name="id">The unique ID (GUID) of the resource to delete.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="id"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null.</exception>
        public async Task DeleteAsync( string resourceName, string id )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "Error: Cannot submit a DELETE request due to a null or blank resourceName param." );
            }
            if ( string.IsNullOrWhiteSpace( id ) )
            {
                throw new ArgumentNullException(
                    $"Error: Cannot submit a DELETE request for resourceName { resourceName } due to a null or blank requestBody parameter."
                );
            }
            var headers = BuildHeadersMap( null );
            string url = EthosIntegrationUrls.Api( Region, resourceName, id );
            await base.DeleteAsync( headers, url );
        }

        #endregion

        #region GET/PUT/POST Strongly Typed

        /// <summary>
        /// Gets a resource by ID (GUID) for the given resource name and version.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="id">The unique ID (GUID) of the resource to get.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>The data for a given resource in an <see cref="EthosResponse" /> according to the requested version of the resource.
        /// The <see cref="EthosResponse" /> contains the content body of the resource data as well as headers and
        /// the Http status code.</returns>
        public async Task<EthosResponse> GetByIdAsync<T>(string resourceName, string id, string version = "") where T : class
        {
            EthosResponse ethosResponse = await GetByIdAsync(resourceName, id, version);
            return ConvertEthosResponseContentToType<T>(ethosResponse);
        }


        /// <summary>
        /// Return a strongly typed object of type T in <see cref="EthosResponse"/>.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in the returned page (EthosResponse).</param>
        /// <returns>A strongly typed object.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or as a white space.</exception>
        public async Task<EthosResponse> GetAsync<T>( string resourceName, string version = "", int offset = 0, int pageSize = 0 ) where T : class
        {
            EthosResponse response = await GetAsync( resourceName, version, offset, pageSize );
            return ConvertEthosResponseContentToType<T>( response );
        }

        /// <summary>
        /// Gets all pages for the given resource, version, and page size.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>Returns collection of <see cref="EthosResponse"/>s with each including strongly typed obect collection.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllPagesAsync<T>( string resourceName, string version = "", int pageSize = 0 ) where T : class
        {
            var ethosResponseList = await GetAllPagesAsync( resourceName, version, pageSize );
            return ConvertEthosResponseContentListToType<T>( ethosResponseList );
        }

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size. If numPages is negative, all pages 
        /// will be returned.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetPagesAsync<T>( string resourceName, string version = "", int pageSize = 0, int numPages = 0 ) where T : class
        {
            var ethosResponseList = await GetPagesAsync( resourceName, version, pageSize, numPages );
            ethosResponseList.ToList().ForEach( ethosResponse =>
            {
                ethosResponse.Dto = ethosResponse.Deserialize<T>();
                ethosResponse.Content = string.Empty;
            } );
            return ethosResponseList;
        }

        /// <summary>
        /// Gets all pages for the given resource, version, and page size, from the offset. If the offset is negative, all pages
        /// will be returned.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllPagesFromOffsetAsync<T>( string resourceName, string version = "", int offset = 0, int pageSize = 0 ) where T : class
        {
            var ethosResponseList = await GetAllPagesFromOffsetAsync( resourceName, version, offset, pageSize );
            return ConvertEthosResponseContentListToType<T>( ethosResponseList );
        }

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size, from the given offset. If both the offset
        /// and numPages are negative, all pages will be returned.If the offset is negative, pages up to the numPages will
        /// be returned. If numPages is negative, all pages from the offset will be returned.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetPagesFromOffsetAsync<T>( string resourceName, string version = "", int pageSize = 0, int offset = 0, int numPages = 0 ) where T : class
        {
            var ethosResponseList = await GetPagesFromOffsetAsync( resourceName, version, pageSize, offset, numPages );
            return ConvertEthosResponseContentListToType<T>( ethosResponseList );
        }

        /// <summary>
        /// Gets some number of rows for the given resource, version, and page size. The number of rows is returned in a list of
        /// pages altogether containing the number of rows.If numRows is negative, all pages will be returned.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetRowsAsync<T>( string resourceName, string version = "", int pageSize = 0, int numRows = 0 ) where T : class
        {
            var ethosResponseList = await GetRowsAsync( resourceName, version, pageSize, numRows );
            return ConvertEthosResponseContentListToType<T>( ethosResponseList );
        }

        /// <summary>
        /// Gets some number of rows for the given resource, version, and page size, from the given offset. The number of rows is returned in a list of
        /// pages altogether containing the number of rows. If both the offset and numRows are negative, all pages will be returned.
        /// If the offset is negative, pages up to the numRows will be returned. If numRows is negative, all pages from the offset will be returned.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetRowsFromOffsetAsync<T>( string resourceName, string version = "", int pageSize = 0, int offset = 0, int numRows = 0 ) where T : class
        {
            var ethosResponseList = await GetRowsFromOffsetAsync( resourceName, version, pageSize, offset, numRows );
            return ConvertEthosResponseContentListToType<T>( ethosResponseList );
        }

        /// <summary>
        /// Submits a PUT/update request for the given resourceName with the given requestBody of strongly typed object. The requestBody should be a strstrongly typed object.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="resourceId">The unique ID for the given resource, as required when making a PUT/update request.</param>
        /// <param name="version">The full version header value of the resource used for this PUT/update request.</param>
        /// <param name="requestBody">The body of the request to PUT/update for the given resource.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this PUT operation including type specified by the caller.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBody"/> is passed as null or empty or white space.</exception>
        public async Task<EthosResponse> PutAsync<T>( string resourceName, T requestBody, string resourceId = "", string version = "" ) where T : class
        {
            var jsonSerSettings = new JsonSerializerSettings()
            {
                DateFormatString = DATE_FORMAT
            };
            var reqBody = requestBody is not null ? JsonConvert.SerializeObject( requestBody, jsonSerSettings ) :
                                                     throw new ArgumentNullException( $"Error: Cannot submit a PUT request for a null or blank requestBody parameter." );
            var response = await PutAsync( resourceName, resourceId, version, reqBody );
            return response;
        }

        /// <summary>
        /// Submits a POST request for the given resourceName with the given requestBody of strongly typed object. The requestBody should be a strstrongly typed object.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="resourceName">The name of the resource to add an instance of.</param>
        /// <param name="version">The full version header value of the resource used for this PUT/update request.</param>
        /// <param name="requestBody">The body of the request to POST for the given resource.</param>
        /// <returns>An EthosResponse containing the instance of the resource that was added by this POST operation including type specified by the caller.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="requestBody"/> is passed as null or empty or white space.</exception>
        public async Task<EthosResponse> PostAsync<T>( string resourceName, T requestBody, string version = "" ) where T : class
        {
            var jsonSerSettings = new JsonSerializerSettings()
            {
                DateFormatString = DATE_FORMAT
            };
            var reqBody = requestBody is not null ? JsonConvert.SerializeObject( requestBody, jsonSerSettings ) :
                                                    throw new ArgumentNullException( $"Error: Cannot submit a POST request for a null or blank requestBody parameter." );
            var response = await PostAsync( resourceName, version, reqBody );
            return response;
        }

        /// <summary>
        /// Converts <see cref="EthosResponse"/> content to a strongly typed object of type T.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        internal EthosResponse ConvertEthosResponseContentToType<T>( EthosResponse response ) where T : class
        {
            response.Dto = response.Deserialize<T>();
            response.Content = string.Empty;
            return response;
        }

        /// <summary>
        /// Converts <see cref="EthosResponse"/> content in the list to a strongly typed object of type T.
        /// </summary>
        /// <typeparam name="T">Type to be included in the <see cref="EthosResponse"/> returned specified by caller.</typeparam>
        /// <param name="ethosResponseList"></param>
        /// <returns></returns>
        internal IEnumerable<EthosResponse> ConvertEthosResponseContentListToType<T>( IEnumerable<EthosResponse> ethosResponseList ) where T : class
        {
            ethosResponseList.ToList().ForEach( ethosResponse =>
            {
                ethosResponse.Dto = ethosResponse.Deserialize<T>();
                ethosResponse.Content = string.Empty;
            } );
            return ethosResponseList;
        }

        #endregion

        #region GET        

        /// <summary>
        /// Gets a page of data for the given resource by name.
        /// </summary>        
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <returns>An <see cref="EthosResponse" /> containing an initial page (EthosResponse content) of resource data according
        /// to the requested version of the resource.</returns>
        public new async Task<EthosResponse> GetAsync( string resourceName )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var version = DEFAULT_VERSION;
            Dictionary<string, string> headers = BuildHeadersMap( version );
            string url = $"{ EthosIntegrationUrls.Api( Region, resourceName ) }";
            EthosResponse response = await GetAsync( headers, url );
            return response;
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version.
        /// </summary>        
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>An <see cref="EthosResponse" /> containing an initial page (EthosResponse content) of resource data according
        /// to the requested version of the resource.</returns>
        public async Task<EthosResponse> GetAsync( string resourceName, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            Dictionary<string, string> headers = BuildHeadersMap( version );
            string url = $"{ EthosIntegrationUrls.Api( Region, resourceName ) }";
            EthosResponse response = await GetAsync( headers, url );
            return response;
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<string> GetAsStringAsync( string resourceName, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            EthosResponse ethosResponse = await GetAsync( resourceName, version );
            return ethosResponseConverter.ToContentString( ethosResponse );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>A <see cref="JArray"/> containing an initial page (EthosResponse content) of resource data according </returns>
        public async Task<JArray> GetAsJArrayAsync( string resourceName, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            EthosResponse ethosResponse = await GetAsync( resourceName, version );
            return ethosResponseConverter.ToJArray( ethosResponse );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name, version, offset, and pageSize. A page of data is returned from the
        /// given offset index containing the number of rows( pageSize) specified.If the given offset is negative, it will
        /// not be used.If the given pageSize is 0 or negative, it will not be used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in the returned page (EthosResponse).</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<EthosResponse> GetAsync( string resourceName, string version = "", int offset = 0, int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            Dictionary<string, string> headers = BuildHeadersMap( version );
            string url = $"{ EthosIntegrationUrls.ApiPaging( Region, resourceName, offset, pageSize ) }";
            EthosResponse response = await GetAsync( headers, url );
            return response;
        }

        /// <summary>
        /// Gets a page of data for the given resource by name, version, offset, and pageSize. A page of data is returned from the 
        /// given offset index containing the number of rows( pageSize) specified. If the given offset is negative, it will
        /// not be used. If the given pageSize is 0 or negative, it will not be used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in the returned page (EthosResponse).</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<string> GetAsStringAsync( string resourceName, string version = "", int offset = 0, int pageSize = 0 )
        {
            EthosResponse response = await GetAsync( resourceName, version, offset, pageSize );
            return ethosResponseConverter.ToContentString( response );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name, version, offset, and pageSize. A page of data is returned from the
        /// given offset index containing the number of rows( pageSize) specified.If the given offset is negative, it will
        /// not be used.If the given pageSize is 0 or negative, it will not be used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in the returned page (EthosResponse).</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<JArray> GetAsJArrayAsync( string resourceName, string version = "", int offset = 0, int pageSize = 0 )
        {

            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            EthosResponse response = await GetAsync( resourceName, version, offset, pageSize );
            return ethosResponseConverter.ToJArray( response );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and from the given offset index. The default version and page size are used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<EthosResponse> GetFromOffsetAsync( string resourceName, string version = "", int offset = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            return await GetAsync( resourceName, version, offset, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version, from the given offset index. The default page size is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<string> GetFromOffsetAsStringAsync( string resourceName, string version = "", int offset = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            EthosResponse response = await GetAsync( resourceName, version, offset, DEFAULT_PAGE_SIZE );
            return ethosResponseConverter.ToContentString( response );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version, from the given offset index. The default page size is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<JArray> GetFromOffsetAsJArrayAsync( string resourceName, string version = "", int offset = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            EthosResponse response = await GetAsync( resourceName, version, offset, DEFAULT_PAGE_SIZE );
            return ethosResponseConverter.ToJArray( response );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version, using the given page size. Offset index 0 is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in the returned page (EthosResponse).</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<EthosResponse> GetWithPageSizeAsync( string resourceName, string version = "", int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            return await GetAsync( resourceName, version, 0, pageSize );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version, using the given page size. Offset index 0 is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in the returned page (EthosResponse).</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<string> GetWithPageSizeAsStringAsync( string resourceName, string version = "", int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            EthosResponse response = await GetAsync( resourceName, version, 0, pageSize );
            return ethosResponseConverter.ToContentString( response );
        }

        /// <summary>
        /// Gets a page of data for the given resource by name and version, using the given page size. Offset index 0 is used.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in the returned page (EthosResponse).</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<JArray> GetWithPageSizeAsJArrayAsync( string resourceName, string version = "", int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            EthosResponse response = await GetAsync( resourceName, version, 0, pageSize );
            return ethosResponseConverter.ToJArray( response );
        }
        #endregion

        #region GET All Pages 

        /// <summary>
        /// Gets all pages for the given resource and version. Uses the default page size of the response body content length.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">Name of the resource</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllPagesAsync( string resourceName )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            return await GetAllPagesAsync( resourceName, DEFAULT_VERSION, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets all pages for the given resource and version. Uses the default page size of the response body content length.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">Name of the resource</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllPagesAsync( string resourceName, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            return await GetAllPagesAsync( resourceName, version, DEFAULT_PAGE_SIZE );
        }

        /// <summary>
        /// Gets all pages for the given resource, version, and page size.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllPagesAsync( string resourceName, string version = "", int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            List<EthosResponse> ethosResponseList = new List<EthosResponse>();

            if ( string.IsNullOrWhiteSpace( resourceName ) ) return ethosResponseList;

            Pager pager = Pager.Build( pg =>
            {
                pg
                .ForResource( resourceName )
                .ForVersion( version )
                .WithPageSize( pageSize );
            } );

            pager = await PrepareForPagingAsync( pager );
            pager = ShouldDoPaging( pager, false );

            pager.HowToPage = Pager.PagingType.PageAllPages;
            ethosResponseList = await HandlePagingAsync( pager );
            return ethosResponseList;
        }

        /// <summary>
        /// Gets all pages for the given resource, version, and page size.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<string>> GetAllPagesAsStringsAsync( string resourceName, string version = "", int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetAllPagesAsync( resourceName, version, pageSize );
            return ethosResponseConverter.ToPagedStringList( ethosResponseList );
        }

        /// <summary>
        /// Gets all pages for the given resource, version, and page size.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<JArray>> GetAllPagesAsJArraysAsync( string resourceName, string version = "", int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetAllPagesAsync( resourceName, version, pageSize );
            return ethosResponseConverter.ToJArrayList( ethosResponseList );
        }

        #endregion

        #region Get All Pages From offset

        /// <summary>
        /// Gets all pages for the given resource, version, and page size, from the offset. If the offset is negative, all pages
        /// will be returned.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllPagesFromOffsetAsync( string resourceName, string version = "", int offset = 0, int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return ethosResponseList;
            }

            if ( offset < 1 )
            {
                // Just get all pages if offset is < 1.
                return await GetAllPagesAsync( resourceName, version, pageSize );
            }

            Pager pager = Pager.Build( p =>
            {
                p
                .ForResource( resourceName )
                .ForVersion( version )
                .WithPageSize( pageSize )
                .FromOffSet( offset );
            } );

            pager = await PrepareForPagingAsync( pager );

            pager = ShouldDoPaging( pager, false );

            pager.HowToPage = Pager.PagingType.PageFromOffset;

            ethosResponseList = await HandlePagingAsync( pager );

            return ethosResponseList;
        }

        /// <summary>
        /// Gets all pages for the given resource, version, and page size from the given offset.
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<string>> GetAllPagesFromOffsetAsStringsAsync( string resourceName, string version = "", int offset = 0, int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetAllPagesFromOffsetAsync( resourceName, version, offset, pageSize );
            return ethosResponseConverter.ToPagedStringList( ethosResponseList );
        }

        /// <summary>
        /// Gets all pages for the given resource, version, and page size, from the given offset. 
        /// <para><b>NOTE: This method could result in a long running process and return a large volume of data. It is possible that
        /// an<see cref="OutOfMemoryException" /> could occur if trying to get a large quantity of data. This is NOT intended to be
        /// used for any kind of resource bulk loading of data. The Ethos bulk loading solution should be used for loading
        /// data in Ethos data model format in bulk.</b></para>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<JArray>> GetAllPagesFromOffsetAsJArraysAsync( string resourceName, string version = "", int offset = 0, int pageSize = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetAllPagesFromOffsetAsync( resourceName, version, offset, pageSize );
            return ethosResponseConverter.ToJArrayList( ethosResponseList );
        }


        #endregion

        #region GET number of pages

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size. If numPages is negative, all pages 
        /// will be returned.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetPagesAsync( string resourceName, string version = "", int pageSize = 0, int numPages = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return ethosResponseList;
            }
            if ( numPages < 1 )
            {
                // Just get all pages if numPages is < 1.
                return await GetAllPagesAsync( resourceName, version, pageSize );
            }

            Pager pager = Pager.Build( p =>
            {
                p.ForResource( resourceName )
                .ForVersion( version )
                .WithPageSize( pageSize )
                .ForNumPages( numPages );
            } );

            pager = await PrepareForPagingAsync( pager );

            pager = ShouldDoPaging( pager, false );

            pager.HowToPage = Pager.PagingType.PageToNumPages;

            ethosResponseList = await HandlePagingAsync( pager );

            return ethosResponseList;
        }

        #endregion

        #region GET pages as string

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<string>> GetPagesAsStringsAsync( string resourceName, string version = "", int pageSize = 0, int numPages = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetPagesAsync( resourceName, version, pageSize, numPages );
            return ethosResponseConverter.ToPagedStringList( ethosResponseList );
        }

        #endregion

        #region GET pages as JArray

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<JArray>> GetPagesAsJArraysAsync( string resourceName, string version = "", int pageSize = 0, int numPages = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetPagesAsync( resourceName, version, pageSize, numPages );
            return ethosResponseConverter.ToJArrayList( ethosResponseList );
        }


        #endregion

        #region GET pages from offset

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size, from the given offset. If both the offset
        /// and numPages are negative, all pages will be returned.If the offset is negative, pages up to the numPages will
        /// be returned. If numPages is negative, all pages from the offset will be returned.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetPagesFromOffsetAsync( string resourceName, string version = "", int pageSize = 0, int offset = 0, int numPages = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return ethosResponseList;
            }

            if ( offset < 1 && numPages < 1 )
            {
                // Just get all pages if offset is < 1 and numPages < 1.
                return await GetAllPagesAsync( resourceName, version, pageSize );
            }
            if ( offset < 1 )
            {
                // If offset is < 1, get up to the num pages because numPages is >= 1.
                return await GetPagesAsync( resourceName, version, pageSize, numPages );
            }
            if ( numPages < 1 )
            {
                // If numPages < 1, get from the offset because the offset is >= 1.
                return await GetAllPagesFromOffsetAsync( resourceName, version, offset, pageSize );
            }

            Pager pager = Pager.Build( p =>
                        {
                            p.ForResource( resourceName )
                            .ForVersion( version )
                            .WithPageSize( pageSize )
                            .ForNumPages( numPages )
                            .FromOffSet( offset );
                        } );

            pager = await PrepareForPagingAsync( pager );

            pager = ShouldDoPaging( pager, false );

            pager.HowToPage = Pager.PagingType.PageFromOffsetForNumPages;

            ethosResponseList = await HandlePagingAsync( pager );

            return ethosResponseList;
        }

        #endregion

        #region GET pages from offset as string

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size, from the given offset.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<string>> GetPagesFromOffsetAsStringsAsync( string resourceName, string version = "", int pageSize = 0, int offset = 0, int numPages = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetPagesFromOffsetAsync( resourceName, version, pageSize, offset, numPages );
            return ethosResponseConverter.ToPagedStringList( ethosResponseList );
        }

        #endregion

        #region GET pages from offset as json.

        /// <summary>
        /// Gets some number of pages for the given resource, version, and page size, from the given offset.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numPages">The number of pages of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<JArray>> GetPagesFromOffsetAsJArraysAsync( string resourceName, string version = "", int pageSize = 0, int offset = 0, int numPages = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetPagesFromOffsetAsync( resourceName, version, pageSize, offset, numPages );
            return ethosResponseConverter.ToJArrayList( ethosResponseList );
        }

        #endregion

        #region GET by rows

        /// <summary>
        /// Gets some number of rows for the given resource, version, and page size. The number of rows is returned in a list of
        /// pages altogether containing the number of rows.If numRows is negative, all pages will be returned.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetRowsAsync( string resourceName, string version = "", int pageSize = 0, int numRows = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return ethosResponseList;
            }

            if ( numRows < 1 )
            {
                // If numRows < 1, just get all pages.
                return await GetAllPagesAsync( resourceName, version, pageSize );
            }

            Pager pager = Pager.Build( p =>
                        {
                            p.ForResource( resourceName )
                            .ForVersion( version )
                            .WithPageSize( pageSize )
                            .ForNumRows( numRows );
                        } );

            pager = await PrepareForPagingAsync( pager );

            pager = ShouldDoPaging( pager, true );

            pager.HowToPage = Pager.PagingType.PageToNumRows;

            ethosResponseList = await HandlePagingAsync( pager );

            return ethosResponseList;
        }


        #endregion

        #region GET rows as string

        /// <summary>
        /// Gets some number of rows for the given resource and version. The rows are returned as a list of strings with each string 
        /// representing a resource. The length of the returned list will be the length of the number of rows requested, if there were 
        /// that many available in the source system.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A string list of data for the given resource from the given offset.</returns>
        public async Task<IEnumerable<string>> GetRowsAsStringsAsync( string resourceName, string version = "", int numRows = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetRowsAsync( resourceName, version, 0, numRows );
            return ethosResponseConverter.ToStringList( ethosResponseList );
        }

        #endregion

        #region GET rows as json array.

        /// <summary>
        /// Gets some number of rows for the given resource and version. The rows are returned as a <see cref="JArray"/> with each child 
        /// representing a resource. The length of the returned JArray will be the length of the number of rows requested, if there were 
        /// that many available in the source system.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A JSON array of data for the given resource.</returns>
        public async Task<JArray> GetRowsAsJArrayAsync( string resourceName, string version = "", int numRows = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetRowsAsync( resourceName, version, 0, numRows );
            return ethosResponseConverter.ToJArray( ethosResponseList );
        }

        #endregion

        #region GET rows from offset

        /// <summary>
        /// Gets some number of rows for the given resource, version, and page size, from the given offset. The number of rows is returned in a list of
        /// pages altogether containing the number of rows. If both the offset and numRows are negative, all pages will be returned.
        /// If the offset is negative, pages up to the numRows will be returned. If numRows is negative, all pages from the offset will be returned.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A page of data for the given resource from the given offset with the given page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetRowsFromOffsetAsync( string resourceName, string version = "", int pageSize = 0, int offset = 0, int numRows = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return ethosResponseList;
            }

            if ( numRows < 1 )
            {
                // If numRows < 1, just get all pages from the offset.
                return await GetAllPagesFromOffsetAsync( resourceName, version, offset, pageSize );
            }

            Pager pager = Pager.Build( p =>
                        {
                            p.ForResource( resourceName )
                            .ForVersion( version )
                            .WithPageSize( pageSize )
                            .FromOffSet( offset )
                            .ForNumRows( numRows );
                        } );

            pager = await PrepareForPagingAsync( pager );

            pager = ShouldDoPaging( pager, true );

            pager.HowToPage = Pager.PagingType.PageFromOffsetForNumRows;

            ethosResponseList = await HandlePagingAsync( pager );

            return ethosResponseList;
        }

        /// <summary>
        /// Gets some number of rows for the given resource and version, from the given offset. The rows are returned as a list of strings with each string 
        /// representing a resource.  The length of the returned list will be the length of the number of rows requested, if there were 
        /// that many available in the source system.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A string list of data for the given resource from the given offset.</returns>
        public async Task<IEnumerable<string>> GetRowsFromOffsetAsStringsAsync( string resourceName, string version = "", int offset = 0, int numRows = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetRowsFromOffsetAsync( resourceName, version, 0, offset, numRows );
            return ethosResponseConverter.ToStringList( ethosResponseList );
        }

        /// <summary>
        /// Gets some number of rows for the given resource and version, from the given offset. The rows are returned as a <see cref="JArray"/> with each child 
        /// representing a resource. The length of the returned JArray will be the length of the number of rows requested, if there were 
        /// that many available in the source system.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="offset">The 0 based index from which to get a page of data for the given resource.</param>
        /// <param name="numRows">The number of rows of the given resource to return.</param>
        /// <returns>A JSON array of data for the given resource from the given offset.</returns>
        public async Task<JArray> GetRowsFromOffsetAsJArrayAsync( string resourceName, string version = "", int offset = 0, int numRows = 0 )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            var ethosResponseList = await GetRowsFromOffsetAsync( resourceName, version, 0, offset, numRows );
            return ethosResponseConverter.ToJArray( ethosResponseList );
        }

        #endregion

        #region Get By Id

        /// <summary>
        /// Gets a resource by ID (GUID) for the given resource name and version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="id">The unique ID (GUID) of the resource to get.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>The data for a given resource in an <see cref="EthosResponse" /> according to the requested version of the resource.
        /// The <see cref="EthosResponse" /> contains the content body of the resource data as well as headers and
        /// the Http status code.</returns>
        public async Task<EthosResponse> GetByIdAsync( string resourceName, string id, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            if ( string.IsNullOrWhiteSpace( id ) )
            {
                throw new ArgumentNullException( nameof( id ) );
            }

            var headersMap = BuildHeadersMap( version );
            string url = EthosIntegrationUrls.Api( Region, resourceName, id );
            EthosResponse ethosResponse = await GetAsync( headersMap, url );
            return ethosResponse;
        }

        /// <summary>
        /// Gets a resource by ID (GUID) for the given resource name and version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="id">The unique ID (GUID) of the resource to get.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>The data for a given resource as a <see cref="string"/> according to the requested version of the resource.
        /// Only returns the content body of the <see cref="EthosResponse" />. Does not return header information or the Http status code.</returns>
        public async Task<string> GetAsStringByIdAsync( string resourceName, string id, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            if ( string.IsNullOrWhiteSpace( id ) )
            {
                throw new ArgumentNullException( nameof( id ) );
            }

            EthosResponse ethosResponse = await GetByIdAsync( resourceName, id, version );
            return ethosResponseConverter.ToContentString( ethosResponse );
        }

        /// <summary>
        /// Gets a resource by ID (GUID) for the given resource name and version.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="id">The unique ID (GUID) of the resource to get.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <returns>The data for a given resource as a <see cref="JObject"/> according to the requested version of the resource.
        /// Only returns the content body of the <see cref="EthosResponse" />. Does not return header information or the Http status code.</returns>
        public async Task<JObject> GetAsJObjectByIdAsync( string resourceName, string id, string version = "" )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            if ( string.IsNullOrWhiteSpace( id ) )
            {
                throw new ArgumentNullException( nameof( id ) );
            }
            EthosResponse ethosResponse = await GetByIdAsync( resourceName, id, version );
            return ethosResponseConverter.ToJObjectSingle( ethosResponse );
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Builds a map of headers with the given version. If the version is null or empty, uses the default version.
        /// </summary>
        /// <param name="version">The version to use for the Accept and Content-Type headers, as supplied in the returned map.</param>
        /// <returns>a <see cref="Dictionary{TKey, TValue}"/> of header values including Accept and Content-Type (both set to the given version.</returns>
        internal Dictionary<string, string> BuildHeadersMap( string version )
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            if ( string.IsNullOrWhiteSpace( version ) )
            {
                version = DEFAULT_VERSION;
            }
            headers.Add( "Accept", version );
            //headers.Add( "Content-Type", version );
            return headers;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Uses the given pager object to prepare for paging operations.The pager object is used to contain various
        /// fields required for paging.If the given pager is null, returns the same pager. Sets default values for the
        /// version and offset within the pager as needed and makes an initial call for the pager resource to get metadata
        /// about the resource used for paging. Also sets the page size and the total count within the pager.
        /// </summary>
        /// <param name="pager">The Pager object used holding the required fields for paging.</param>
        /// <returns>The same pager object with the version and offset validated, and the page size and total count set.</returns>
        protected async Task<Pager> PrepareForPagingAsync( Pager pager )
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

            // First make a GET list call without filters to get the x-total-count.
            EthosResponse ethosResponse = await GetAsync( pager.ResourceName, pager.Version );

            //If Ethos web api max page size is smaller than requested page size, then we need to make sure 
            //that we adjust the PageSize in pager and pages are returned with correct x-total-count which 
            //reflect what api allows in x-max-page-size for each page returned and no rows are omitted. 
            //(With Colleague API's, the max page size is either 100 or 200).
            string apiMaxPageCount = GetHeaderValue( ethosResponse, HDR_X_MAX_PAGE_SIZE );
            if ( int.TryParse( apiMaxPageCount, out int maxPageCount ) )
            {
                if ( maxPageCount < pager.PageSize )
                {
                    pager.PageSize = maxPageCount;
                }
            }

            // Set the pageSize.
            if ( pager.PageSize <= DEFAULT_PAGE_SIZE )
            {
                // Set the pageSize from the response body length, if pageSize is <= DEFAULT_PAGE_SIZE.
                int pageSize = await GetPageSizeAsync( pager.ResourceName, pager.Version, ethosResponse );
                pager.PageSize = pageSize;
            }

            string totalCount = GetHeaderValue( ethosResponse, HDR_X_TOTAL_COUNT );
            pager.TotalCount = Convert.ToInt32( totalCount );
            pager.EthosResponse = ethosResponse;
            return pager;
        }

        /// <summary>
        ///  <para><b>Intended to be used internally within the SDK.</b></para>
        ///  Determines whether paging should be done for the resource within the given pager.
        ///  Supports normal paging (by total count ) or paging for number of rows. If normal paging, then the need to page
        ///  is determined by comparing the total count with the page size. If paging for number of rows, the need to page
        ///  is determined by comparing the number of rows with the page size.
        /// </summary>
        /// <param name="pager">The Pager object containing the total count or numRows, and page size to determine the need to page.</param>
        /// <param name="forNumRows">If true, then paging is by numRows. If false, then paging is by total count.</param>
        /// <returns>The given pager object with the shouldDoPaging flag set to true when paging is needed, or false when not.</returns>
        protected Pager ShouldDoPaging( Pager pager, bool forNumRows )
        {
            int total = forNumRows ? pager.NumRows : pager.TotalCount;
            bool shouldPage = NeedToPage( pager.PageSize, total );
            pager.ShouldDoPaging = shouldPage;
            return pager;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// A simple comparison which is used by the other <see cref="NeedToPage(int, int)"/> method, comparing the given page size to the
        /// total count. If the page size is less than the total count, then paging is needed. If not, paging is not needed.
        /// </summary>
        /// <param name="pageSize">The page size for some resource.</param>
        /// <param name="totalCount">The total count of rows for some resource.</param>
        /// <returns>True if paging is needed, false otherwise.</returns>
        private bool NeedToPage( int pageSize, int totalCount )
        {
            return pageSize < totalCount;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// For some resource defined within the given pager, determines whether to page for data, or to use the data
        /// obtained during paging preparation.
        /// </summary>
        /// <param name="pager">A pager previously prepared for paging (see <see cref="PrepareForPagingAsync(Pager)"/> and <see cref="ShouldDoPaging(Pager, bool)" />.</param>
        /// <returns>A list of <see cref="EthosResponse" />s where each <see cref="EthosResponse" /> in the list contains a page of data.</returns>
        private async Task<List<EthosResponse>> HandlePagingAsync( Pager pager )
        {
            List<EthosResponse> ethosResponseList;
            if ( pager.ShouldDoPaging )
            {
                ethosResponseList = await GetDataFromPagingAsync( pager );
            }
            else
            {
                ethosResponseList = GetDataFromInitialContent( pager );
            }
            return ethosResponseList;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Determines how to page given the "howToPage" attribute of the pager. The howToPage attribute must be one of the
        /// defined PagingTypes.
        /// </summary>
        /// <param name="pager">A previously prepared pager with the howToPage attribute set appropriately.</param>
        /// <returns>A list of <see cref="EthosResponse" />s where each <see cref="EthosResponse" /> in the list contains a page of data.</returns>
        private async Task<List<EthosResponse>> GetDataFromPagingAsync( Pager pager )
        {
            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            switch ( pager.HowToPage )
            {
                case Pager.PagingType.PageAllPages:
                    ethosResponseList = await DoPagingForAllAsync( pager.ResourceName, pager.Version, pager.TotalCount, pager.PageSize );
                    break;
                case Pager.PagingType.PageToNumPages:
                    ethosResponseList = await DoPagingForNumPagesAsync( pager.ResourceName, pager.Version, pager.TotalCount, pager.PageSize, pager.NumPages );
                    break;
                case Pager.PagingType.PageFromOffset:
                    ethosResponseList = await DoPagingFromOffsetAsync( pager.ResourceName, pager.Version, null, pager.TotalCount, pager.PageSize, pager.Offset );
                    break;
                case Pager.PagingType.PageFromOffsetForNumPages:
                    ethosResponseList = await DoPagingFromOffsetForNumPagesAsync( pager.ResourceName, pager.Version, pager.TotalCount, pager.PageSize, pager.NumPages, pager.Offset );
                    break;
                case Pager.PagingType.PageToNumRows:
                    ethosResponseList = await DoPagingForNumRowsAsync( pager.ResourceName, pager.Version, pager.TotalCount, pager.PageSize, pager.NumRows );
                    break;
                case Pager.PagingType.PageFromOffsetForNumRows:
                    ethosResponseList = await DoPagingFromOffsetForNumRowsAsync( pager.ResourceName, pager.Version, pager.TotalCount, pager.PageSize, pager.Offset, pager.NumRows );
                    break;
            }
            return ethosResponseList;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Gets resource data from the initial content obtained when preparing for paging.This method does not page for data.
        /// Supports trimming the data content according to the given PagingType( howToPage attribute ).
        /// </summary>
        /// <param name="pager">A pager previously prepared for paging.</param>
        /// <returns>A list of <see cref="EthosResponse" />s containing a single page of data, which may be trimmed according to the
        /// howToPage PagingType specified in the pager.</returns>
        private List<EthosResponse> GetDataFromInitialContent( Pager pager )
        {
            List<EthosResponse> responseList = new List<EthosResponse>();
            switch ( pager.HowToPage )
            {
                case Pager.PagingType.PageAllPages:
                case Pager.PagingType.PageToNumPages:
                    responseList.Add( pager.EthosResponse );
                    break;
                case Pager.PagingType.PageToNumRows:
                    responseList.Add( ethosResponseConverter.TrimContentForNumRows( pager.EthosResponse, pager.NumRows ) );
                    break;
                case Pager.PagingType.PageFromOffset:
                case Pager.PagingType.PageFromOffsetForNumPages:
                    responseList.Add( ethosResponseConverter.TrimContentFromOffset( pager.EthosResponse, pager.Offset ) );
                    break;
                case Pager.PagingType.PageFromOffsetForNumRows:
                    responseList.Add( ethosResponseConverter.TrimContentFromOffsetForNumRows( pager.EthosResponse, pager.Offset, pager.NumRows ) );
                    break;
            }
            return responseList;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Used within the SDK to page for all pages for a given resource by using an offset of 0.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="totalCount">The total count of rows for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <returns>A list of <see cref="EthosResponse" />s where each <see cref="EthosResponse" /> in the list represents a page.</returns>
        private async Task<List<EthosResponse>> DoPagingForAllAsync( string resourceName, string version, int totalCount, int pageSize )
        {
            return await DoPagingFromOffsetAsync( resourceName, version, null, totalCount, pageSize, 0 );
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="totalCount">The total count of rows for the given resource.</param>
        /// <param name="filter">The string resource filter in JSON format contained in the URL, e.g: <pre>?criteria={"names":[{"firstName":"John"}]}</pre></param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>A list of <see cref="EthosResponse" />s where each <see cref="EthosResponse" /> in the list represents a page,
        /// beginning from the given offset index.</returns>
        protected async Task<List<EthosResponse>> DoPagingFromOffsetAsync( string resourceName, string version, string filter, int totalCount, int pageSize, int offset )
        {
            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            Dictionary<string, string> headers = BuildHeadersMap( version );
            decimal numPages = Math.Ceiling( ( Convert.ToDecimal( totalCount ) - Convert.ToDecimal( offset ) ) / Convert.ToDecimal( pageSize ) );
            for ( int i = 0; i < numPages; i++ )
            {
                string url;
                if ( string.IsNullOrWhiteSpace( filter ) )
                {
                    url = EthosIntegrationUrls.ApiPaging( Region, resourceName, offset, pageSize );
                }
                else
                {
                    url = EthosIntegrationUrls.ApiFilterPaging( Region, resourceName, filter, offset, pageSize );
                }
                EthosResponse response = await GetAsync( headers, url );
                ethosResponseList.Add( response );
                offset += pageSize;
            }
            return ethosResponseList;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Used within the SDK to page for some number of pages for a given resource by using an offset of 0.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="totalCount">The total count of rows for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numPages">The number of pages to page for.</param>
        /// <returns>A list of <see cref="EthosResponse" />s where each <see cref="EthosResponse" /> in the list represents a page,
        /// up to some number of pages.</returns>
        private async Task<List<EthosResponse>> DoPagingForNumPagesAsync( string resourceName, string version, int totalCount, int pageSize, int numPages )
        {
            return await DoPagingFromOffsetForNumPagesAsync( resourceName, version, totalCount, pageSize, numPages, 0 );
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Used within the SDK to page for some number of pages for a given resource from the given offset.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="totalCount">The total count of rows for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numPages">The number of pages to page for.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>A list of <see cref="EthosResponse" />s where each <see cref="EthosResponse" /> in the list represents a page,
        /// from the given offset( inclusive) and up to some number of pages( exclusive).</returns>
        private async Task<List<EthosResponse>> DoPagingFromOffsetForNumPagesAsync( string resourceName, string version, int totalCount, int pageSize, int numPages, int offset )
        {
            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            Dictionary<string, string> headers = BuildHeadersMap( version );
            for ( int i = 0; i < numPages; i++ )
            {
                if ( offset >= totalCount )
                {
                    break;
                }
                string url = EthosIntegrationUrls.ApiPaging( Region, resourceName, offset, pageSize );
                EthosResponse response = await GetAsync( headers, url );
                ethosResponseList.Add( response );
                offset += pageSize;
            }
            return ethosResponseList;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Used within the SDK to page for some number of rows for a given resource, using an offset of 0.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="totalCount">The total count of rows for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="numRows"></param>
        /// <returns>A list of <see cref="EthosResponse" />s where each <see cref="EthosResponse" /> in the list represents a page,
        /// up to the number of rows specified or the total count of the resource( whichever is less).</returns>
        private async Task<List<EthosResponse>> DoPagingForNumRowsAsync( string resourceName, string version, int totalCount, int pageSize, int numRows ) =>
            await DoPagingFromOffsetForNumRowsAsync( resourceName, version, totalCount, pageSize, 0, numRows );

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Used within the SDK to page for some number of rows for a given resource, using an offset of 0.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get data for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="totalCount">The total count of rows for the given resource.</param>
        /// <param name="pageSize">The number of rows to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <param name="numRows">The overall number of rows to page for.</param>
        /// <returns></returns>
        private async Task<List<EthosResponse>> DoPagingFromOffsetForNumRowsAsync( string resourceName, string version, int totalCount, int pageSize, int offset, int numRows )
        {
            List<EthosResponse> ethosResponseList = new List<EthosResponse>();
            Dictionary<string, string> headers = BuildHeadersMap( version );
            if ( numRows > totalCount )
            {
                numRows = totalCount; // Ensure the numRows requested is not more than the totalCount.
            }
            decimal numPages = Math.Ceiling( Convert.ToDecimal( numRows ) / Convert.ToDecimal( pageSize ) );
            int totalNum = numRows + offset;
            for ( int i = 0; i < numPages; i++ )
            {
                if ( offset >= totalCount )
                {
                    break;
                }
                int rowsRemaining = totalNum - offset;
                if ( rowsRemaining < pageSize )
                {
                    pageSize = rowsRemaining;
                }
                string url = EthosIntegrationUrls.ApiPaging( Region, resourceName, offset, pageSize );
                EthosResponse response = await GetAsync( headers, url );
                ethosResponseList.Add( response );
                offset += pageSize;
            }
            return ethosResponseList;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para> 
        /// Returns the header value from the given ethosResponse.
        /// </summary>
        /// <param name="ethosResponse">The <see cref="EthosResponse" /> to get a header value from.</param>
        /// <param name="primaryHeaderName">A header name following Ethos standards (such as "x-media-type").</param>
        /// <returns>The value of the given header. Returns null if the given ethosResponse is null or the header is not found.</returns>
        protected static string GetHeaderValue( EthosResponse ethosResponse, string primaryHeaderName )
        {
            return ethosResponse?.GetHeader( primaryHeaderName );
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Gets the page size for the given resourceName, version, and ethosResponse.If the ethosResponse is null, it
        /// will make a call to get the data for the given resource to then calculate the page size from the response body content.
        /// </summary>
        /// <param name="resourceName">The name of the Ethos resource to get the page size for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="ethosResponse">An <see cref="EthosResponse" /> from which to calculate the page size using it's content body length, or null.</param>
        /// <returns>The page size of the given resource.</returns>
        public async Task<int> GetPageSizeAsync( string resourceName, string version = "", EthosResponse ethosResponse = null )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) { throw new ArgumentNullException( nameof( resourceName ) ); }

            int pageSize = 0;
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return pageSize;
            }

            if ( string.IsNullOrWhiteSpace( version ) )
            {
                version = DEFAULT_VERSION;
            }

            if ( ethosResponse == null )
            {
                ethosResponse = await GetAsync( resourceName, version );
            }
            // Set the pageSize from the response body length, if pageSize is <= DEFAULT_PAGE_SIZE.
            if ( !string.IsNullOrWhiteSpace( ethosResponse.Content ) )
            {
                JArray jArray = JsonConvert.DeserializeObject( ethosResponse.Content ) as JArray;
                pageSize = jArray.Count;
            }
            else
            {
                pageSize = await GetMaxPageSizeAsync( resourceName, version, ethosResponse );
            }
            return pageSize;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Gets the max page size for the given resourceName, version, and ethosResponse.If the ethosResponse is null, it
        /// will make a call to get the data for the given resource to then calculate the page size from the response body content.
        /// </summary>
        /// <param name="resourceName">The name of the Ethos resource to get the page size for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="ethosResponse">An <see cref="EthosResponse" /> from which to calculate the page size using it's content body length, or null.</param>
        /// <returns>The max page size for the resource as found using the x-max-page-size header.</returns>
        public async Task<int> GetMaxPageSizeAsync( string resourceName, string version = "", EthosResponse ethosResponse = null )
        {
            int maxPageSize = 0;
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                return maxPageSize;
            }
            if ( string.IsNullOrWhiteSpace( version ) )
            {
                version = DEFAULT_VERSION;
            }
            if ( ethosResponse == null )
            {
                ethosResponse = await GetAsync( resourceName, version );
            }
            string maxPageSizeStr = GetHeaderValue( ethosResponse, HDR_X_MAX_PAGE_SIZE );
            if ( !string.IsNullOrWhiteSpace( maxPageSizeStr ) )
            {
                maxPageSize = Convert.ToInt32( maxPageSizeStr );
            }
            else
            {
                maxPageSize = DEFAULT_MAX_PAGE_SIZE;
            }
            return maxPageSize;
        }

        /// <summary>
        /// <para><b>Intended to be used internally within the SDK.</b></para>
        /// Gets the total count of rows for the given Ethos resource, version, and ethosResponse.If the ethosResponse is null,
        /// a call will be made to get the resource data to then get the total count from the x-total-count header.
        /// </summary>
        /// <param name="resourceName">The name of the Ethos resource to get the total count for.</param>
        /// <param name="version">The desired resource version to use, as provided in the HTTP Accept Header of the request.</param>
        /// <param name="ethosResponse">An <see cref="EthosResponse" /> from which to get the total count, or null.</param>
        /// <returns>The total count for the given resource, or 0 if the resourceName is null or empty.</returns>
        public async Task<int> GetTotalCountAsync( string resourceName, string version = "", EthosResponse ethosResponse = null )
        {
            if ( string.IsNullOrWhiteSpace( resourceName.Trim() ) )
            {
                return 0;
            }
            if ( string.IsNullOrWhiteSpace( version.Trim() ) )
            {
                version = DEFAULT_VERSION;
            }
            if ( ethosResponse == null )
            {
                ethosResponse = await GetAsync( resourceName, version );
            }
            string totalCountStr = GetHeaderValue( ethosResponse, HDR_X_TOTAL_COUNT );

            if ( int.TryParse( totalCountStr, out int count ) )
            {
                return count;
            }
            return 0;
        }

        #endregion
    }
}
