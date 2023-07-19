/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy;
using Ellucian.Ethos.Integration.Client.Errors;
using Ellucian.Ethos.Integration.Config;
using Ellucian.Ethos.Integration.Client.Messages;

namespace Ellucian.Ethos.Integration.Client
{
    /// <summary>
    /// Factory used for building Ethos clients. This is the primary means of building the desired Ethos client class.
    /// This class supports building clients specified by the various client types.
    /// </summary>
    public class EthosClientBuilder
    {

        /// <summary>
        /// The API key used to build the access token within the given EthosClient.
        /// </summary>
        private readonly string apiKey;

        /// <summary>
        /// The connection timeout for the factory. Specified in seconds.
        /// </summary>
        private int? ConnectionTimeout = null;

        private readonly string ColleagueApiUrl;
        private readonly string ColleagueApiUsername;
        private readonly string ColleagueApiPassword;

        /// <summary>
        /// Interface used in HttpProtocolClientBuilder.
        /// </summary>
        protected IHttpProtocolClientBuilder builder;

        /// <summary>
        /// Constructs this class with the given Ethos Integration API key.
        /// </summary>
        /// <param name="apiKey">The API key used to build the access token for the desired Ethos client.</param>
        public EthosClientBuilder( string apiKey )
        {
            this.apiKey = apiKey;
            builder ??= new HttpProtocolClientBuilder( null, ConnectionTimeout );
        }
        /// <summary>
        /// Constructs this class with the given Colleauge API URL/Credentials.
        /// </summary>
        /// <param name="colleagueApiUrl">The URL to the Colleague API instance.</param>
        /// <param name="colleagueApiUsername">The username used to connect to the Colleague API.</param>
        /// <param name="colleagueApiPassword">The password used to connect to the Colleague API.</param>
        public EthosClientBuilder(string colleagueApiUrl, string colleagueApiUsername, string colleagueApiPassword)
        {
            builder ??= new HttpProtocolClientBuilder(null, ConnectionTimeout);
            ColleagueApiUrl = colleagueApiUrl;
            ColleagueApiUsername = colleagueApiUsername;
            ColleagueApiPassword = colleagueApiPassword;
        }

        /// <summary>
        /// Give the client factory a connection timeout so that connections will time out after <code>connectionTimeout</code>
        /// seconds.
        /// </summary>
        /// <param name="connectionTimeout">The connection timeout in seconds.</param>
        /// <returns>Ethos client builder.</returns>
        public EthosClientBuilder WithConnectionTimeout( int connectionTimeout )
        {
            this.ConnectionTimeout = connectionTimeout;
            return this;
        }

        /// <summary>
        /// Gets an <see cref="EthosProxyClient"/> that will use the given API key to authenticate.
        /// </summary>
        /// <returns>A new <see cref="EthosProxyClient"/> using the given apiKey, or null if the apiKey is null.</returns>
        public EthosProxyClient BuildEthosProxyClient()
        {
            return new EthosProxyClient( apiKey, builder.Client );
        }

        /// <summary>
        /// Gets an <see cref="EthosErrorsClient"/> that will use the given API key to authenticate.
        /// </summary>
        /// <returns>A new <see cref="EthosErrorsClient"/> using the given apiKey, or null if the apiKey is null.</returns>
        public EthosErrorsClient BuildEthosErrorsClient()
        {
            return new EthosErrorsClient( apiKey, builder.Client );
        }

        /// <summary>
        /// Gets an <see cref="EthosConfigurationClient"/> given the AccessToken.
        /// </summary>
        /// <returns>A new <see cref="EthosConfigurationClient"/> using the given apiKey, or null if the apiKey is null.</returns>
        public EthosConfigurationClient BuildEthosConfigurationClient()
        {
            return new EthosConfigurationClient( apiKey, builder.Client );
        }

        /// <summary>
        /// Gets an <see cref="EthosMessagesClient"/> given the AccessToken.
        /// </summary>
        /// <returns>A new <see cref="EthosMessagesClient"/> using the given apiKey, or null if the apiKey is null.</returns>
        public EthosMessagesClient BuildEthosMessagesClient()
        {
            return new EthosMessagesClient( apiKey, builder.Client );
        }

        /// <summary>
        /// Gets an <see cref="EthosFilterQueryClient"/> that will use the given API key to authenticate.
        /// </summary>
        /// <returns>An EthosFilterQueryClient using the given apiKey and timeout values.</returns>
        public EthosFilterQueryClient BuildEthosFilterQueryClient()
        {
            return new EthosFilterQueryClient(apiKey, builder.Client);
        }

        /// <summary>
        /// Gets an <see cref="EthosProxyClient"/> that will use the given Colleague credentials to authenticate.
        /// </summary>
        /// <returns>An ColleagueWebApiProxyClient using the given Colleague credentials.</returns>
        public ColleagueWebApiProxyClient BuildColleagueWebApiProxyclient()
        {
            return new ColleagueWebApiProxyClient(ColleagueApiUrl, ColleagueApiUsername, ColleagueApiPassword, builder.Client);
        }

        /// <summary>
        /// Gets an <see cref="ColleagueWebApiFilterQueryClient"/> that will use the Colleague credentials to authenticate.
        /// </summary>
        /// <returns>An ColleagueWebApiFilterQueryClient using the given Colleague credentials.</returns>
        public ColleagueWebApiFilterQueryClient BuildColleagueWebApiFilterQueryClient()
        {
            return new ColleagueWebApiFilterQueryClient(ColleagueApiUrl, ColleagueApiUsername, ColleagueApiPassword, builder.Client);
        }
    }
}
