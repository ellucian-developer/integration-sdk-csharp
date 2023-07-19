/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;

namespace Ellucian.Ethos.Integration.Service
{
    /// <summary>
    /// Abstract base service class used by various subclasses.
    /// </summary>
    public abstract class EthosService
    {
        #region ...ctor

        /// <summary>
        /// ...ctor
        /// </summary>
        protected EthosService()
        {

        }
        /// <summary>
        /// Constructs this service with the given API key.
        /// </summary>
        /// <param name="apiKey">apiKey The API key used by the EthosClients of this service when obtaining an access token per request.</param>
        protected EthosService(string apiKey)
        {
            EthosClientBuilder = new EthosClientBuilder(apiKey);
        }

        /// <summary>
        /// Constructs this service with the given Colleague API and credentials.
        /// </summary>
        /// <param name="colleagueApiUrl">The URL to the Colleague API instance.</param>
        /// <param name="colleagueApiUsername">The username used to connect to the Colleague API.</param>
        /// <param name="colleagueApiPassword">The password used to connect to the Colleague API.</param>
        protected EthosService(string colleagueApiUrl, string colleagueApiUsername, string colleagueApiPassword)
        {
            EthosClientBuilder = new EthosClientBuilder(colleagueApiUrl, colleagueApiUsername, colleagueApiPassword);
        }

        /// <summary>
        /// Constructs this service with the given <see cref="Ellucian.Ethos.Integration.Client.EthosClientBuilder"/>.
        /// </summary>
        /// <param name="ethosClientBuilder"></param>
        protected EthosService( EthosClientBuilder ethosClientBuilder )
        {
            EthosClientBuilder = ethosClientBuilder;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The EthosClientBuilder used by the subclasses to build the Ethos clients used by this service. All clients must use
        /// the same API key and timeout values.
        /// </summary>
        protected EthosClientBuilder EthosClientBuilder { get; set; }


        #endregion
    }
}
