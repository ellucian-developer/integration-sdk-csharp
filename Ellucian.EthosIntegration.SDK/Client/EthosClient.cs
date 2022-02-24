/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Authentication;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Client
{
    /// <summary>
    /// Base HTTP client to interact with Ellucian Ethos API. This class is mostly used internally by the SDK to interact with the Ethos Integration API to retrieve objects via HTTP(S). 
    /// It lightly wraps the <see cref="HttpClient"/> library for simplicity.
    /// </summary>
    public class EthosClient
    {
        #region Fields/Properties

        /// <summary>
        /// A <see cref="Guid"/> api key.
        /// </summary>
        private string ApiKey { get; }

        /// <summary>
        /// Default token expiration time.
        /// </summary>
        private int expirationMinutes = 60;

        /// <summary>
        /// Gets/Sets the number of minutes that a new access token will be valid.
        /// </summary>
        public int ExpirationMinutes
        {
            get { return expirationMinutes; }
            set
            {
                if ( expirationMinutes < 1 || expirationMinutes > 120 )
                {
                    throw new InvalidOperationException( "The 'expirationMinutes' parameter has to be between 1 and 120." );
                }
                expirationMinutes = value;
            }
        }

        /// <summary>
        /// Gets/Sets the automatic refresh behavior for access tokens.
        /// </summary>
        public bool AutoRefresh { get; set; } = true;

        /// <value>
        /// AccessToken used when making calls through the SDK to the Ethos Integration APIs. This is specified by the subclasses of this class.
        /// </value>
        internal AccessToken Token { get; set; }

        /// <summary>
        /// Supported region.
        /// </summary>
        public SupportedRegions Region { get; set; } = SupportedRegions.US;

        /// <value>
        /// The Http client builder which builds an <see cref="HttpClient"/> used for making secure calls to the Ethos Integration API.
        /// </value>
        protected IHttpProtocolClientBuilder HttpProtocolClientBuilder { get; }

        /// <value>
        /// Used to build an EthosResponse from the given HttpResponse in the responseHandler of each call made.
        /// </value>
        protected EthosResponseBuilder EthosResponseBuilder { get; }

        #endregion

        #region ..ctor

        /// <summary>
        /// Constructor called by subclasses of this class.
        /// </summary>
        /// <param name="apiKey">An api key <see cref="Guid"/>.</param>
        /// <param name="client">A <see cref="System.Net.Http.HttpClient"/>.</param>
        public EthosClient( string apiKey, HttpClient client )
        {
            if ( string.IsNullOrWhiteSpace( apiKey ) )
            {
                throw new ArgumentNullException( $"The '{ nameof( apiKey )} ' parameter is required." );
            }
            if ( !Guid.TryParse( apiKey, out Guid result ) )
            {
                throw new FormatException( $"The '{ apiKey }' parameter must be a valid GUID string." );
            }

            if ( client == null )
            {
                throw new ArgumentNullException( $"The '{ nameof( client )}' parameter is required." );
            }
            ApiKey = apiKey;
            this.HttpProtocolClientBuilder = new HttpProtocolClientBuilder( client );
            EthosResponseBuilder ??= new EthosResponseBuilder();
        }

        #endregion

        #region Methods
        /// <summary>
        /// The responseHandler used for each request made. Either supplies an EthosResponse, or throws an HttpResponseException if the HttpStatus code is not a successful code.
        /// </summary>
        /// <param name="response"></param>
        /// <returns>A <see cref="Task"/> of <see cref="EthosResponse"/>.</returns>
        protected async Task<EthosResponse> ResponseHandlerAsync( HttpResponseMessage response )
        {
            int statusCode = ( int ) response.StatusCode;
            if ( ( int ) statusCode >= 200 && statusCode < 300 )
            {
                response.EnsureSuccessStatusCode();
                EthosResponse resp = await EthosResponseBuilder.BuildEthosResponseAsync( response );
                return resp;
            }
            else
            {
                throw new HttpRequestException( await response.Content.ReadAsStringAsync() );
            }
        }

        /// <summary>
        /// Attaches headers to a request.
        /// </summary>
        /// <param name="request">The request being made.</param>
        /// <param name="headers">A map of headers to attach to the request.</param>
        private void AttachHeaders( HttpClient request, Dictionary<string, string> headers )
        {
            if ( headers != null && headers.Any() )
            {
                request.DefaultRequestHeaders.Clear();
                foreach ( string headerName in headers.Keys )
                {
                    if ( headerName.Equals( "Content-Type", StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        request.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( headers [ headerName ] ) );
                    }
                    else
                    {
                        if ( !request.DefaultRequestHeaders.Contains( headerName ) )
                        {
                            request.DefaultRequestHeaders.Add( headerName, headers [ headerName ] );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets an access token. If this client does not currently have an access token, it will make a call to the Ethos Integration
        /// /auth endpoint to get one.Additionally, if autoRefresh is set to true, this will check to see if the current token is
        /// expired and, if so, get a new one.
        /// </summary>
        /// <returns>An access token</returns>
        public async Task<AccessToken> GetAccessTokenAsync()
        {
            if ( this.Token == null || ( !this.Token.IsValid() && this.AutoRefresh ) )
            {
                this.Token = await GetNewTokenAsync();
            }
            return this.Token;
        }

        /// <summary>
        /// Gets a new access token.
        /// </summary>
        /// <returns>An access token</returns>
        /// <exception cref="HttpRequestException">Returns <see cref="HttpRequestException"/> exception if the request fails.</exception>
        private async Task<AccessToken> GetNewTokenAsync()
        {
            string authUrl = $"{ EthosIntegrationUrls.Auth( this.Region )}?expirationMinutes={ ExpirationMinutes }";
            HttpProtocolClientBuilder.Client.DefaultRequestHeaders.Add( "Authorization", $"Bearer { ApiKey }" );
            //make request
            HttpResponseMessage response = await HttpProtocolClientBuilder.Client.PostAsync( new Uri( authUrl ), null );
            _ = response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            DateTime expirationTime = DateTime.Now.AddMinutes( ExpirationMinutes );
            return new AccessToken( responseString, expirationTime );
        }

        /// <summary>
        /// Returns the HttpClient built by the httpProtocolClientBuilder. Used primarily by this class.
        /// </summary>
        /// <returns>The HttpClient used to make Http calls.</returns>
        private HttpClient GetHttpClient()
        {
            var httpClient = HttpProtocolClientBuilder.Client;
            return httpClient;
        }

        /// <summary>
        /// Make an HTTP GET request with no headers. This will return the result as a string so any conversion to JSON or other
        /// results will need to be done on the caller.  Also note that HTTP status codes that throw errors will come back as an 
        /// exception. 
        /// </summary>
        /// <param name="headers">headers attached to the request. typically would include authentication.</param>
        /// <param name="url">URL to make the request to.</param>
        /// <returns>The result of the request as a string.</returns>
        public async Task<string> GetStringAsync( Dictionary<string, string> headers, string url )
        {
            var response = await GetAsync( headers, url );
            return response.Content;
        }

        /// <summary>
        /// Make an HTTP GET request with the given headers. This will return the result as a string so any conversion to JSON or other
        /// results will need to be done on the caller.  Also note that HTTP status codes that throw errors will come back as an 
        /// exception. 
        /// </summary>
        /// <param name="headers">headers attached to the request. typically would include authentication.</param>
        /// <param name="url">URL to make the request to.</param>
        /// <returns>The result of the request as a string.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when request url or request body is null or empty.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown when request failed due to an underlying issue such as network connectivity, DNS failure, 
        /// failure, server certificate validation or timeout.</exception>

        public async Task<EthosResponse> GetAsync( Dictionary<string, string> headers, string url )
        {
            // if the headers are null, then create a new dictionary for the headers
            // so we can add the authorization to it.
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            HttpClient httpClient = GetHttpClient();
            await AddAccessTokenAuthHeaderAsync( headers );
            AttachHeaders( httpClient, headers );

            /*
                Internal comments: 
                The HttpClient that get passed thru the constructor gets created once for every new creation of EthosProxyClient 
                and that's by design based on Microsoft's recommendation found here 
                https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netcore-3.1&viewFallbackFrom=dotnet-plat-ext-3.1
                In code example in the page it states "HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks."
                Remarks: 
                The HttpClient class instance acts as a session to send HTTP requests. An HttpClient instance is a collection of settings applied 
                to all requests executed by that instance. In addition, every HttpClient instance uses its own connection pool, isolating its requests from 
                requests executed by other HttpClient instances, etc...
            */

            HttpResponseMessage response = await httpClient.GetAsync( url );
            EthosResponse resp = await ResponseHandlerAsync( response );
            return resp;
        }

        /// <summary>
        /// Convenience method to make an HTTP call without headers.
        /// </summary>
        /// <param name="url">The URL to GET.</param>
        /// <returns>The response as an EthosRespone containing response headers, response body (content) and the HTTP status code.</returns>
        public Task<EthosResponse> GetAsync( string url )
        {
            return GetAsync( null, url );
        }

        /// <summary>
        /// Make an HTTP HEAD request against the given URL.  This will return an EthosResponse object containing the response
        /// headers.Note that the HTTP status codes that throw errors will come back as an exception via the responseHandler
        /// </summary>
        /// <param name="url">The Request URL.</param>
        /// <returns>The response as an Ethos Response containing headers and HTTP status code.</returns>
        public async Task<EthosResponse> HeadAsync( string url )
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            await AddAccessTokenAuthHeaderAsync( headers );
            HttpClient httpClient = GetHttpClient();
            AttachHeaders( httpClient, headers );

            HttpRequestMessage headRequest = new HttpRequestMessage( HttpMethod.Head, url );
            HttpResponseMessage response = await httpClient.SendAsync( headRequest );
            return await ResponseHandlerAsync( response );
        }

        /// <summary>
        /// Post to the Ethos Integration REST API to get objects back to translate to the SDK model.
        /// </summary>
        /// <param name="headers">A dictionary containing HTTP headers. Usually this will contained the authorization header at minimum.</param>
        /// <param name="url">URL to post to.</param>
        /// <param name="body">The JSON body to post to the URL endpoint.</param>
        /// <returns>A <see cref="EthosResponse"/>.</returns>            
        /// <exception cref="System.ArgumentNullException">Thrown when request url or request body is null or empty.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown when request failed due to an underlying issue such as network connectivity, DNS failure, 
        /// failure, server certificate validation or timeout.</exception>

        public async Task<EthosResponse> PostAsync( Dictionary<string, string> headers, string url, string body = "" )
        {
            await AddAccessTokenAuthHeaderAsync( headers );
            HttpClient httpClient = GetHttpClient();
            AttachHeaders( httpClient, headers );

            StringContent json = null;
            // posting JSON requires that the type be specified as part of the content object instead of in an accept header, 
            // like we do in the Java version. But we still put it in the Accept header to maintain consistency across the SDKs.
            // so I am doing that here.
            if ( !string.IsNullOrEmpty( body ) && headers.ContainsKey( "Accept" ) )
            {
                string jsonContentType = headers [ "Accept" ];
                json = new StringContent( body, System.Text.Encoding.UTF8, jsonContentType );
            }

            HttpResponseMessage result = await httpClient.PostAsync( url, json );
            var response = await ResponseHandlerAsync( result );
            return response;
        }

        /// <summary>
        /// Send a PUT request to the Ethos Integration REST API to get objects back to translate to the SDK model.
        /// </summary>
        /// <param name="headers">A dictionary containing HTTP headers. Usually this will contained the authorization header at minimum.</param>
        /// <param name="url">URL to PUT to.</param>
        /// <param name="body">The JSON body to PUT to the URL endpoint.</param>
        /// <returns>A <see cref="EthosResponse"/>.</returns>            
        /// <exception cref="System.ArgumentNullException">Thrown when request url or request body is null or empty.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown when request failed due to an underlying issue such as network connectivity, DNS failure, 
        /// failure, server certificate validation or timeout.</exception>
        public async Task<EthosResponse> PutAsync(Dictionary<string, string> headers, string url, string body = "")
        {
            await AddAccessTokenAuthHeaderAsync(headers);
            HttpClient client = GetHttpClient();
            AttachHeaders(client, headers);

            StringContent json = null;
            // posting JSON requires that the type be specified as part of the content object instead of in an accept header, 
            // like we do in the Java version. But we still put it in the Accept header to maintain consistency across the SDKs.
            // so I am doing that here.
            if (!string.IsNullOrEmpty(body) && headers.ContainsKey("Accept"))
            {
                string jsonContentType = headers["Accept"];
                json = new StringContent(body, System.Text.Encoding.UTF8, jsonContentType);
            }

            HttpResponseMessage result = await client.PutAsync(url, json);
            var response = await ResponseHandlerAsync(result);
            return response;
        }


        /// <summary>
        /// Send an HTTP or HTTPS DELETE request to a given URL.
        /// </summary>
        /// <param name="headers">A dictionary containing HTTP headers. Usually this will contain the authorization header at minimum.</param>
        /// <param name="url">URL to post to.</param>		            
        /// <exception cref="System.ArgumentNullException">Thrown when request url or request body is null or empty.</exception>
        /// <exception cref="System.Net.Http.HttpRequestException">Thrown when request failed due to an underlying issue such as network connectivity, DNS failure, 
        /// failure, server certificate validation or timeout.</exception>
        /// <exception cref="System.InvalidOperationException">The request message was already sent by the System.Net.Http.HttpClient instance.</exception>
        public async Task DeleteAsync( Dictionary<string, string> headers, string url )
        {
            await AddAccessTokenAuthHeaderAsync( headers );
            HttpClient httpClient = GetHttpClient();
            AttachHeaders( httpClient, headers );

            HttpResponseMessage response = await httpClient.DeleteAsync( url );
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Gets an access token and add it authorization key to the header.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task AddAccessTokenAuthHeaderAsync( Dictionary<string, string> headers )
        {
            AccessToken token = await GetAccessTokenAsync();
            if ( token.GetAuthHeader().TryGetValue( "Authorization", out string authValue ) )
            {
                headers.Remove( "Authorization" );
                headers.Add( "Authorization", authValue );
            }
        }

        #endregion
    }
}
