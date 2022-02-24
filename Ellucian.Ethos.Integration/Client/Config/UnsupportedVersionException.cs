/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;

namespace Ellucian.Ethos.Integration.Client.Config
{
    /// <summary>
    /// The unsupported version that was requested for the some resource.
    /// </summary>
    public class UnsupportedVersionException : Exception
    {
        /// <summary>
        /// Constructs this exception with the given error message.
        /// </summary>
        /// <param name="message">The error message describing the error.</param>
        public UnsupportedVersionException( string message ) : base( message )
        {

        }

        /// <summary>
        /// Constructs this exception with the given error message and unsupported version that was requested for some resource.
        /// </summary>
        /// <param name="message">The error message describing the error.</param>
        /// <param name="unsupportedVersion">The unsupported version of the requested resource.</param>
        public UnsupportedVersionException( string message, string unsupportedVersion ) : base( message )
        {
            this.UnsupportedVersion = unsupportedVersion;
        }

        /// <summary>
        /// Constructs this exception with the given error message, resource name, and unsupported version of the resource.
        /// </summary>
        /// <param name="message">The error message describing the error.</param>
        /// <param name="resourceName">The name of the Ethos resource.</param>
        /// <param name="unsupportedVersion">The unsupported version of the requested resource.</param>
        public UnsupportedVersionException( string message, string resourceName, string unsupportedVersion ) : this( message, unsupportedVersion )
        {
            this.ResourceName = resourceName;
        }

        /// <summary>
        /// The name of the resource that the unsupportedVersion was requested for.
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// The unsupported version that was requested for the some resource.
        /// </summary>
        public string UnsupportedVersion { get; private set; }
    }
}
