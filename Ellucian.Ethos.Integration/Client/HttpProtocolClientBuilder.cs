/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Microsoft.Extensions.DependencyInjection;

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

        private const string CLIENT_NAME = "EllucianEthosIntegrationSdk-dotnet";

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
        public HttpProtocolClientBuilder( HttpClient client, int? connectionTimeOut = null )
        {
            if ( client == null )
            {
                client = BuildHttpClient( connectionTimeOut.HasValue ? connectionTimeOut : CONNECTION_TIMEOUT );
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
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient( CLIENT_NAME, client =>
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add( "pragma", "no-cache" );
                client.DefaultRequestHeaders.Add( "cache-control", "no-cache" );
                ProductInfoHeaderValue prodHeaderVal = new ProductInfoHeaderValue( CLIENT_NAME, Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() );
                client.DefaultRequestHeaders.UserAgent.Add( prodHeaderVal );
                client.Timeout = TimeSpan.FromMinutes( ( double ) connectionTimeout );
            } )
            .SetHandlerLifetime( TimeSpan.FromSeconds( CONNECTION_TIMEOUT ) )
            .ConfigurePrimaryHttpMessageHandler( () =>
            {
                return new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Automatic,
                    SslProtocols = PROTOCOL
                };
            } );
            
            var services = serviceCollection.BuildServiceProvider();
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient( CLIENT_NAME );
            Client = httpClient;
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
