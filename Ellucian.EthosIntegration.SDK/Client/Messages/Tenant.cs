/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

namespace Ellucian.Ethos.Integration.Client.Messages
{
    /// <summary>
    /// A Tenant object that is associated with a change-notification. This holds information
    /// about the ethos tenant where the change occurred.
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// A Tenant object that is associated with a change-notification. This holds information
        /// about the ethos tenant where the change occurred.
        /// </summary>
        /// <param name="id">The ID of the tenant.</param>
        /// <param name="alias">The alias of the tenant.</param>
        /// <param name="name">The name of the tenant.</param>
        /// <param name="environment">The tenant environment.</param>
        /// <remarks>The attribute JsonConstructor is used to parse json and convert in to the object.</remarks>
        [JsonConstructor]
        public Tenant( string id, string alias, string name, string environment )
        {
            Id = id;
            Alias = alias;
            Name = name;
            Environment = environment;
        }

        /// <summary>
        /// The ID of the tenant.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The alias of the tenant.
        /// </summary>
        public string Alias { get; private set; }

        /// <summary>
        /// The name of the tenant.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The tenant environment.
        /// </summary>
        public string Environment { get; private set; }
    }
}
