/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Client.Errors
{
    /// <summary>
    /// An EthosClient used to perform Create, Read, and Delete operations for Error objects, using the Ethos Integration errors service.
    ///
    /// The preferred way to instantiate this class is via the EthosClientBuilder.
    /// </summary>
    public class EthosErrorsClient: EthosClient
    {
        /// <summary>
        /// Accept header value for Error type.
        /// </summary>
        private const string errorType = "application/vnd.hedtech.errors.v2+json";

        /// <summary>
        /// The default page size (limit) when paging for errors.
        /// </summary>
        public const int DefaultErrorPageSize = 10;

        /// <summary>
        /// The x-total-count header, representing the number of items in the request.
        /// </summary>
        public const string HDR_TOTAL_COUNT = "x-total-count";

        /// <summary>
        /// The remaining count header found in the EthosResponse. This value is equal to the total count minus the page size of the given request/response.
        /// </summary>
        public const string HDR_REMAINING_COUNT = "x-remaining-count";

        /// <summary>
        /// Used to convert EthosResponses to EthosError objects.
        /// </summary>
        private EthosResponseConverter ethosResponseConverter;

        /// <summary>
        /// Constructs an EthosErrorClient using the given API key.
        /// Note that the preferred way to get an instance of this class is through the <see cref="Ellucian.Ethos.Integration.Client.EthosClientBuilder"/>.
        /// </summary>
        /// <param name="apiKey">A valid API key from Ethos Integration. This is required to be a valid 36 character GUID string.
        /// If it is null, empty, <see cref="ArgumentNullException"/> will be thrown or not in a valid GUID format, then a <see cref="FormatException"/> will be thrown.</param>
        /// <param name="client">A HttiClient. If it is null, empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        public EthosErrorsClient( string apiKey, HttpClient client ) : base( apiKey, client )
        {
            this.ethosResponseConverter = new EthosResponseConverter();
        }

        /// <summary>
        /// Get an initial array (page) of Errors from the tenant associated with the access token.
        /// </summary>
        /// <returns>An EthosResponse containing an array of errors in the content body.</returns>
        public async Task<EthosResponse> GetAsync()
        {
            Dictionary<string, string> headers = BuildHeadersMap();
            return await base.GetAsync(headers, EthosIntegrationUrls.Errors(Region));
        }

        /// <summary>
        /// Gets an initial array (page) of Errors from the tenant associated with the access token, as a JArray.
        /// </summary>
        /// <returns>A JArray containing child nodes for each error.</returns>
        public async Task<JArray> GetAsJArrayAsync()
        {
            EthosResponse response = await this.GetAsync();
            return ethosResponseConverter.ToJArray(response);
        }

        /// <summary>
        /// Gets an initial array (page) of Errors from the tenant associated with the access token, as a string.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAsStringAsync()
        {
            return ethosResponseConverter.ToContentString(await GetAsync());
        }

        /// <summary>
        /// Get an initial list of EthosErrors from the tenant associated with the access token.
        /// </summary>
        /// <returns>An initial list of EthosErrors.</returns>
        public async Task<IEnumerable<EthosError>> GetAsEthosErrorsAsync()
        {
            EthosResponse ethosResponse = await GetAsync();
            return ethosResponseConverter.ToEthosErrorList( ethosResponse );
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token, as a list of EthosErrors.
        /// </summary>
        /// <returns>A list of EthosErrors where each EthosError in the list is for an individual error.</returns>
        public async Task<IEnumerable<EthosError>> GetAllErrorsAsEthosErrorsAsync()
        {
            List<EthosError> ethosErrorList = new List<EthosError>();
            var ethosResponseList = await GetAllErrorsAsync();
            foreach ( EthosResponse ethosResponse in ethosResponseList )
            {
                ethosErrorList.AddRange( ethosResponseConverter.ToEthosErrorList( ethosResponse ) );
            }
            return ethosErrorList;
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token from the given offset, as a list of EthosErrors.
        /// </summary>
        /// <param name="offset">The 0 based index from which to begin paging for errors.</param>
        /// <returns>A list of EthosErrors starting from the given offset.</returns>
        public async Task<IEnumerable<EthosError>> GetErrorsFromOffsetAsEthosErrorsAsync( int offset )
        {
            List<EthosError> ethosErrorList = new List<EthosError>();
            IEnumerable<EthosResponse> ethosResponseList = await GetErrorsFromOffsetAsync( offset );
            foreach ( EthosResponse ethosResponse in ethosResponseList )
            {
                ethosErrorList.AddRange( ethosResponseConverter.ToEthosErrorList( ethosResponse ) );
            }
            return ethosErrorList;
        }

        /// <summary>
        /// Get a specific error assigned the ID passed in.
        /// </summary>
        /// <param name="id">Error ID</param>
        /// <returns>An EthosResponse object containing the EthosError in the content body.</returns>            
        /// <exception cref="System.ArgumentNullException">Thrown when request 'id' is null or empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when request url or request body is null or empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when request failed due to an underlying issue such as network connectivity, DNS failure, 
        /// server certificate validation or timeout.</exception>
        public async Task<EthosResponse> GetByIdAsync( string id )
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("The 'id' is a required parameter.");
            }

            Dictionary<string, string> headers = BuildHeadersMap();
            return await base.GetAsync(headers, EthosIntegrationUrls.Errors(Region) + "/" + id);
        }

        /// <summary>
        /// Get a single error as an EthosError object, using the given ID.
        /// </summary>
        /// <param name="id">The ID of the Error to get. This is a required parameter.
        ///     If it is null or empty, then an ArgumentNullException will be thrown.
        /// </param>
        /// <returns>An EthosError object representing the error for the given ID.</returns>
        public async Task<EthosError> GetByIdAsEthosErrorAsync(string id)
        {
            EthosResponse ethosResponse = await GetByIdAsync(id);
            return ethosResponseConverter.ToSingleEthosError(ethosResponse);
        }

        /// <summary>
        /// Get a single error as a JObject, using the given ID.
        /// </summary>
        /// <param name="id">The ID of the Error to get. This is a required parameter.
        ///     If it is null or empty, then an ArgumentNullException will be thrown.
        /// </param>
        /// <returns>A JObject representing the error for the given ID.</returns>
        public async Task<JObject> GetByIdAsJObjectAsync( string id )
        {
            EthosResponse ethosResponse = await GetByIdAsync(id);
            return ethosResponseConverter.ToJObjectSingle(ethosResponse);
        }

        /// <summary>
        /// Get a single error as a JObject, using the given ID.
        /// </summary>
        /// <param name="id">The ID of the Error to get. This is a required parameter.
        ///     If it is null or empty, then an ArgumentNullException will be thrown.
        /// </param>
        /// <returns>A string representing the error for the given ID.</returns>
        public async Task<string> GetByIdAsStringAsync(string id)
        {
            EthosResponse ethosResponse = await GetByIdAsync(id);
            return ethosResponseConverter.ToContentString(ethosResponse);
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token.
        /// </summary>
        /// <returns>A list of EthosResponses where each ethosResponse in the list contains a page of errors.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllErrorsAsync()
        {
            int totalCount = await GetTotalErrorCountAsync();
            return await DoPagingAsync(totalCount, DefaultErrorPageSize, 0);
        }

        /// <summary>
        /// Get all of the errors for the given tenant per access token, as a list of Jarrays.
        /// </summary>
        /// <returns>A list of JArrays where each JArray in the list contains a page of errors.</returns>
        public async Task<IEnumerable<JArray>> GetAllErrorsAsJArrayAsync()
        {
            IEnumerable<EthosResponse> allErrors = await GetAllErrorsAsync();
            return ethosResponseConverter.ToJArrayList(allErrors);
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token, as a list of Strings in JSON format.
        /// </summary>
        /// <returns>A list of strings where each string in the list contains a page of errors.</returns>
        public async Task<IEnumerable<string>> GetAllErrorsAsStringsAsync()
        {
            return ethosResponseConverter.ToPagedStringList(await GetAllErrorsAsync());
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token using the given page size.
        /// </summary>
        /// <param name="pageSize">The limit number of errors to include in each page of errors returned.</param>
        /// <returns>A list of EthosResponses where each ethosResponse in the list contains a page of errors and each page
        /// contains up to the number of errors specified as the page size.</returns>
        public async Task<IEnumerable<EthosResponse>> GetAllErrorsWithPageSizeAsync(int pageSize)
        {
            int totalCount = await GetTotalErrorCountAsync();
            return DoPagingAsync(totalCount, pageSize, 0).Result;
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token using the given page size as a list of JArrays.
        /// </summary>
        /// <param name="pageSize">The limit number of errors to include in each page of errors returned.</param>
        /// <returns>A list of EthosResponses where each ethosResponse in the list contains a page of errors and each page
        /// contains up to the number of errors specified as the page size.</returns>
        public async Task<IEnumerable<JArray>> GetAllErrorsWithPageSizeAsJArraysAsync(int pageSize)
        {
            int totalCount = await GetTotalErrorCountAsync();
            IEnumerable<EthosResponse> allErrors = await GetAllErrorsWithPageSizeAsync(pageSize);
            return ethosResponseConverter.ToJArrayList(allErrors);
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token using the given page size as a list of strings.
        /// </summary>
        /// <param name="pageSize">The limit number of errors to include in each page of errors returned.</param>
        /// <returns>A list of EthosResponses where each ethosResponse in the list contains a page of errors and each page
        /// contains up to the number of errors specified as the page size.</returns>
        public async Task<IEnumerable<string>> GetAllErrorsWithPageSizeAsStringsAsync(int pageSize)
        {
            int totalCount = await GetTotalErrorCountAsync();
            IEnumerable<EthosResponse> allErrors = await GetAllErrorsWithPageSizeAsync(pageSize);
            return ethosResponseConverter.ToPagedStringList(allErrors);
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token from the given offset. Uses the default page size.
        /// </summary>
        /// <param name="offset">The 0 based index from which to begin paging for errors.</param>
        /// <returns>IEnumerable&lt;EthosResponse&gt;</returns>
        public async Task<IEnumerable<EthosResponse>> GetErrorsFromOffsetAsync(int offset)
        {
            int totalCount = await GetTotalErrorCountAsync();
            Task<IEnumerable<EthosResponse>> response = DoPagingAsync(totalCount, DefaultErrorPageSize, offset);
            return response.Result;
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token from the given offset, as a list of JArrays.
        /// Uses the default page size.
        /// </summary>
        /// <param name="offset">The 0 based index from which to begin paging for the given resource.</param>
        /// <returns>IEnumerable&lt;JArray&gt;</returns>
        public async Task<IEnumerable<JArray>> GetErrorsFromOffsetAsJArrayAsync( int offset )
        {
            var response = await GetErrorsFromOffsetAsync(offset);
            return ethosResponseConverter.ToJArrayList(response);
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token from the given offset, as a list of strings.
        /// Uses the default page size.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns>IEnumerable&lt;string&gt;</returns>
        public async Task<IEnumerable<string>> GetErrorsFromOffsetAsJsonStringsAsync(int offset)
        {
            var response = await GetErrorsFromOffsetAsync(offset);
            return ethosResponseConverter.ToPagedStringList(response);
        }
        
        /// <summary>
        /// Gets all of the errors for the given tenant per access token from the given offset. Uses the given page size.
        /// </summary>
        /// <param name="offset">The 0 based index from which to begin paging for errors.</param>
        /// <param name="pageSize">The limit number of Errors to include in each page of errors returned.</param>
        /// <returns>IEnumerable&lt;EthosResponse&gt;</returns>
        public async Task<IEnumerable<EthosResponse>> GetErrorsFromOffsetWithPageSizeAsync(int offset, int pageSize)
        {
            int totalCount = await GetTotalErrorCountAsync();
            IEnumerable<EthosResponse> response = await DoPagingAsync(totalCount, pageSize, offset);
            return response;
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token from the given offset, as a list of JArrays.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="pageSize">The limit number of Errors to include in each page of errors returned.</param>
        /// <returns>IEnumerable&lt;JArray&gt;</returns>
        public async Task<IEnumerable<JArray>> GetErrorsFromOffsetWithPageSizeAsJArrayAsync( int offset, int pageSize)
        {
            var response = await GetErrorsFromOffsetWithPageSizeAsync(offset, pageSize);
            return ethosResponseConverter.ToJArrayList(response);
        }

        /// <summary>
        /// Gets all of the errors for the given tenant per access token from the given offset, as a list of strings.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="pageSize">The limit number of Errors to include in each page of errors returned.</param>
        /// <returns>IEnumerable&lt;string&gt;</returns>
        public async Task<IEnumerable<string>> GetErrorsFromOffsetWithPageSizeAsJsonStringsAsync(int offset, int pageSize)
        {
            var response = await GetErrorsFromOffsetWithPageSizeAsync(offset, pageSize);
            return ethosResponseConverter.ToPagedStringList(response);
        }

        /// <summary>
        /// <p><b>Intended to be used internally within the SDK.</b></p>
        /// Handles paging for errors.If the given pageSize is &lt;= 0, the default page size is used. If the offset is &lt; 0,
        /// 0 will be used for the offset.
        /// </summary>
        /// <param name="totalErrorCount">The total count of errors for the given tenant per access token.</param>
        /// <param name="pageSize">The number of errors to include in each page (EthosResponse) of the list returned.</param>
        /// <param name="offset">The 0 based index from which to begin paging for errors. To get all errors, the offset should be 0.</param>
        /// <returns>IEnumerable&lt;EthosResponse&gt;</returns>
        protected async Task<IEnumerable<EthosResponse>> DoPagingAsync(int totalErrorCount, int pageSize, int offset)
        {
            if (pageSize <= 0)
            {
                pageSize = DefaultErrorPageSize;
            }
            if (offset < 0)
            {
                offset = 0;
            }
            IList<EthosResponse> ethosResponseList = new List<EthosResponse>();
            Dictionary<string, string> headersMap = BuildHeadersMap();
            int numPages = CalculateNumberOfPages(totalErrorCount, pageSize, offset);
            for (int index = 0; index < numPages; index++)
            {
                string url = EthosIntegrationUrls.ErrorsPaging(Region, offset, pageSize);
                EthosResponse response = await GetAsync(headersMap, url);
                ethosResponseList.Add(response);
                offset += pageSize;
            }
            return ethosResponseList;
        }


        /// <summary>
        /// Calculates the number of pages given the input params. Input param values are not validated, so specifying
        /// invalid negative values could produce unpredictable results. A 0 value is valid for the offset, but not for the other params.
        /// <p>
        ///     The calculation is as follows:
        ///     <br/>
        ///     <i>
        ///     double numPages = Math.Ceiling(Convert.ToDouble((totalErrorCount - offset)/pageSize));
        ///     </i>
        ///     <br/>
        ///     The numPages returned value is cast to an int.
        /// </p>
        /// </summary>
        /// <param name="totalErrorCount">The total number of errors.</param>
        /// <param name="pageSize">The limit number of errors to include in each page.</param>
        /// <param name="offset">The 0 based index from which to begin paging for errors.</param>
        /// <returns>The number of pages.</returns>
        public int CalculateNumberOfPages(int totalErrorCount, int pageSize, int offset)
        {
            double numPages = Math.Ceiling( ( Convert.ToDouble( totalErrorCount ) - Convert.ToDouble( offset ) ) / Convert.ToDouble( pageSize ) );
            return Convert.ToInt32( numPages );
        }

        /// <summary>
        /// Create the given Error in the tenant associated with the AccessToken.
        /// See the examples directory for how to do this.
        /// </summary>
        /// <param name="newError">An Error object.</param>
        /// <returns>The response payload from the errors service.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when request url or request body is null or empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when request failed due to an underlying issue such as network connectivity, DNS failure, 
        /// server certificate validation or timeout.</exception>
        public async Task<EthosResponse> PostAsync( EthosError newError )
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers [ "Accept" ] = errorType;
            string errorRequestBody = JsonConvert.SerializeObject( newError );
            var httpResult = await base.PostAsync( headers, EthosIntegrationUrls.Errors( Region ), errorRequestBody );
            return JsonConvert.DeserializeObject<EthosResponse>( httpResult.Content );
        }

        /// <summary>
        /// Delete the Error with the given ID from the tenant associated with the AccessToken.
        /// </summary>
        /// <param name="id">The ID of the Error that you want to delete</param>
        /// <exception cref="System.ArgumentNullException">Thrown when request url or request body is null or empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when request failed due to an underlying issue such as network connectivity, DNS failure, 
        /// server certificate validation or timeout.</exception>
        public async Task DeleteAsync( string id )
        {
            await base.DeleteAsync( new Dictionary<string, string>(), EthosIntegrationUrls.Errors( Region ) + "/" + id );
        }

        /// <summary>
        /// Gets the total number of errors according to the 'x-total-count' response header for the given tenant per access token.
        /// </summary>
        /// <returns>The total number of errors for the given tenant, or 0 if the 'x-total-count' header is not found.</returns>
        public async Task<int> GetTotalErrorCountAsync()
        {
            int totalErrorCount = 0;
            EthosResponse ethosResponse = await GetAsync();
            string totalCountHeader = ethosResponse.GetHeader(HDR_TOTAL_COUNT);
            if (!string.IsNullOrEmpty(totalCountHeader))
            {
                totalErrorCount = int.Parse(totalCountHeader);
            }
            return totalErrorCount;
        }

        /// <summary>
        /// Builds the headers dictionary used when making requests for errors.
        /// </summary>
        /// <returns>A dictionary containing the headers needed for making a request to the Ethos Integration Errors API service.</returns>
        private Dictionary<string, string> BuildHeadersMap()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers["Accept"] = errorType;
            return headers;
        }
    }
}
