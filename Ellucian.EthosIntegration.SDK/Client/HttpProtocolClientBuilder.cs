/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication;

namespace Ellucian.Ethos.Integration.Client
{
    /// <summary>
    /// Builds an <see cref="System.Net.Http.HttpClient"/> used for making secure API calls over HTTP.
    /// Uses Tls13, Tls12, Tls11 protocol.
    /// </summary>
    public class HttpProtocolClientBuilder : IHttpProtocolClientBuilder
    {
        /// <summary>
        /// Time in seconds to allow an http connection to time out. Default is
        /// 300 seconds (5 minutes).
        /// </summary>
        private static int CONNECTION_TIMEOUT = 300;

        private const SslProtocols PROTOCOL = SslProtocols.Tls13 | SslProtocols.Tls12 | SslProtocols.Tls11;

        /// <summary>
        /// <see cref="HttpClient"/>.
        /// </summary>
        public HttpClient Client { get; private set; }

        /// <summary>
        /// Instantiates a new Http client builder service and initializes it with default configurations for the
        /// SslProtocols, ClientCertificateOptions and DefaultRequestHeaders.
        /// </summary>
        /// <param name="client">Instance of <see cref="System.Net.Http.HttpClient"/> passed from <see cref="EthosClient"/>.</param>
        /// <param name="connectionTimeOut">Time in seconds to allow an http connection to time out. Default is
        /// 300 seconds (5 minutes).</param>
        public HttpProtocolClientBuilder( HttpClient client, int? connectionTimeOut = null)
        {
            if( client == null )
            {
                client = BuildHttpClient( connectionTimeOut.HasValue? connectionTimeOut : CONNECTION_TIMEOUT );
            }

            Client = client;
        }

        /// <summary>
        /// Creates instance of <see cref="System.Net.Http.HttpClient"/>. Called internally from EthosClientFactory./>
        /// </summary>
        /// <param name="connectionTimeout">An optional parameter indicating the amount of seconds before the client request should time out.</param>
        /// <returns>Returns instance of HttpClient.</returns>
        public HttpClient BuildHttpClient( int? connectionTimeout )
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
            handler.SslProtocols = PROTOCOL;

            HttpClient client = new HttpClient( handler );
            client.DefaultRequestHeaders.Add( "pragma", "no-cache" );
            client.DefaultRequestHeaders.Add( "cache-control", "no-cache" );
            ProductInfoHeaderValue prodHeaderVal = new ProductInfoHeaderValue("EllucianEthosIntegrationSdk-dotnet", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
            client.DefaultRequestHeaders.UserAgent.Add( prodHeaderVal );
            // default to the default connection timeout value.
            int timeoutLength = CONNECTION_TIMEOUT;
            // unless the param passed in HAS a value.
            if ( connectionTimeout.HasValue )
            {
                timeoutLength = connectionTimeout.GetValueOrDefault();
            }
            client.Timeout = new TimeSpan( 0, 0, 0, timeoutLength, 0 );
            Client = client;
            return Client;
        }

        /// <summary>
        /// Creates instance of <see cref="System.Net.Http.HttpClient"/>. Called internally from EthosClientFactory./>
        /// </summary>
        /// <returns>Returns instance of HttpClient with a default timeout.</returns>
        public HttpClient BuildHttpClient()
        {
            this.Client = BuildHttpClient( CONNECTION_TIMEOUT );
            return this.Client;
        }
    }
}
