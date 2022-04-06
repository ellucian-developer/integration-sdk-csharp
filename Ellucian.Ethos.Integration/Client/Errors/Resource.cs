/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

namespace Ellucian.Ethos.Integration.Client.Errors
{

    /// <summary>
    /// A Resource object that associated with an Error.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// Create a resource with an id and a name.
        /// </summary>
        /// <param name="id">The id of the resource.</param>
        /// <param name="name">The name of the resource.</param>
        /// <param name="version">The version of the resource.</param>
        public Resource( string id, string name, string version )
        {
            this.Id = id;
            this.Name = name;
            this.Version = version;
        }

        /// <summary>
        /// Get the ID of the resource.
        /// </summary>
        /// <value>resource ID.</value>
        [JsonProperty( "id" )]
        public string Id { get; }

        /// <summary>
        /// Get the name of the resource.
        /// </summary>
        /// <value>resource name</value>
        [JsonProperty( "name" )]
        public string Name { get; }

        /// <summary>
        /// Get the version of the resource.
        /// </summary>
        /// <value>resource version</value>
        [JsonProperty( "version" )]
        public string Version { get; }

    }
}