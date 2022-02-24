/*
 * ******************************************************************************
 *   Copyright 2021 Ellucian Company L.P. and its affiliates.
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
        protected EthosService( string apiKey )
        {
            //ApiKey = apiKey;
            EthosClientBuilder = new EthosClientBuilder( apiKey );
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

        ///// <summary>
        ///// The API key used to build the AccessToken within the given EthosClient used by this service.
        ///// </summary>
        //public string ApiKey { get; protected set; }

        /// <summary>
        /// The EthosClientBuilder used by the subclasses to build the Ethos clients used by this service.  All clients must use
        /// the same API key and timeout values.
        /// </summary>
        protected EthosClientBuilder EthosClientBuilder { get; set; }


        #endregion
    }
}
