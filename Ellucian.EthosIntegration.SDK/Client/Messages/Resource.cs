/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

namespace Ellucian.Ethos.Integration.Client.Messages
{
    /// <summary>
    /// A Resource object that is associated with a change-notification.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// A Resource object that is associated with a change-notification.
        /// </summary>
        /// <param name="id">The ID of the resource.</param>
        /// <param name="name">The name of the resource.</param>
        /// <param name="version">The version of the resource.</param>
        /// <param name="domain">The domain to which the resource belongs.</param>
        /// <remarks>The attribute JsonConstructor is used to parse json and convert in to the object.</remarks>
        [JsonConstructor]
        public Resource( string id, string name, string version, string domain )
        {
            Id = id;
            Name = name;
            Version = version;
            Domain = domain;
        }

        /// <summary>
        /// The ID of the resource.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The name of the resource.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The version of the resource.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// The domain to which the resource belongs.
        /// </summary>
        public string Domain { get; private set; }
    }
}
