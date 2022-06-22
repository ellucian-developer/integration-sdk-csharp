/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;

namespace Ellucian.Ethos.Integration.Client.Config
{
    /// <summary>
    /// Thrown when the given resource is not found in the available resources response.
    /// </summary>
    public class EthosResourceNotFoundException : Exception
    {
        /// <summary>
        /// Constructor taking the error message.
        /// </summary>
        /// <param name="message">The error message for this exception.</param>
        public EthosResourceNotFoundException( string message ) : base( message )
        {

        }

        /// <summary>
        /// Constructor taking the error message and the name of the resource not found in the available resources response.
        /// </summary>
        /// <param name="message">The error message for this exception.</param>
        /// <param name="resourceName">The name of the resource not found.</param>
        public EthosResourceNotFoundException( string message, string resourceName ) : base( message )
        {
            this.ResourceName = resourceName;
        }

        /// <summary>
        /// Gets the name of the resource that was not found.
        /// </summary>
        public string ResourceName { get; private set; }
    }
}
