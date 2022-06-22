/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

namespace Ellucian.Ethos.Integration.Client.Errors
{
    /// <summary>
    /// A Request object that is associated with an Error.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// URI for the error.
        /// </summary>
        [JsonProperty( PropertyName = "uri" )]
        public string URI { get; set; }

        /// <summary>
        /// Headers for the error. This is an array of strings but would typically contain values like this:
        /// "Accept:application/json"
        /// </summary>
        [JsonProperty( "headers" )]
        public string [] Headers { get; set; }

        /// <summary>
        /// Error request payload.
        /// </summary>
        [JsonProperty( "payload" )]
        public string Payload { get; set; }
    }
}