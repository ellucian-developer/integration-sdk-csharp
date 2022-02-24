/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

namespace Ellucian.Ethos.Integration.Client.Messages
{
    /// <summary>
    /// A Publisher object that is associated with a change-notification. This holds information
    /// about the application that published the change.
    /// </summary>
    public class Publisher
    {
        /// <summary>
        /// A Publisher object that is associated with a change-notification. This holds information
        /// about the application that published the change.
        /// </summary>
        /// <param name="id">The ID of the publishing application.</param>
        /// <param name="applicationName">The name of the publishing application.</param>
        /// <param name="tenant">The tenant where the change occurred.</param>
        /// <remarks>The attribute JsonConstructor is used to parse json and convert in to the object.</remarks>
        [JsonConstructor]
        public Publisher( string id, string applicationName, Tenant tenant )
        {
            Id = id;
            ApplicationName = applicationName;
            Tenant = tenant;
        }

        /// <summary>
        /// The ID of the publishing application.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The name of the publishing application.
        /// </summary>
        public string ApplicationName { get; private set; }

        /// <summary>
        /// The tenant where the change occurred.
        /// </summary>
        public Tenant Tenant { get; private set; }
    }
}
