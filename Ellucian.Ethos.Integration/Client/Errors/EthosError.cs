/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

using System;

namespace Ellucian.Ethos.Integration.Client.Errors
{
    /// <summary>
    /// An Ellucian Ethos Integration Error object.
    /// </summary>
    public class EthosError
    {
        /// <summary>
        /// An Ellucian Ethos Integration Error object.
        /// </summary>
        /// <param name="id">A GUID for this error.</param>
        /// <param name="severity">(required) The severity of this error. This should be one of info, warning, or error.</param>
        /// <param name="responseCode">(required) An integer response code associated with this error.</param>
        /// <param name="description">(required) A description for this error.</param>
        /// <param name="details">A detailed message for this error.</param>
        /// <param name="applicationId">The ID of the application reporting this error.</param>
        /// <param name="applicationName">The name of the application reporting this error.</param>
        /// <param name="dateTime">The Date that the error occurred.</param>
        /// <param name="correlationId">The id of the original operation (event or message) to which this error can be traced.</param>
        /// <param name="applicationSubType">The sub-type of the application reporting this error. This could be used to describe a sub-system</param>
        /// <param name="resource">The Resource associated with this error</param>
        /// <param name="request">The Request that caused this error</param>
        public EthosError( string id, string severity, int responseCode, string description, string details, string applicationId, string applicationName, string dateTime,
            string correlationId, string applicationSubType, Resource resource, Request request )
        {
            if ( !string.IsNullOrWhiteSpace( id ) && Guid.TryParse( id, out Guid guidResult ) )
            {
                Id = guidResult.ToString();
            }
            else
            {
                throw new ArgumentNullException( $"The '{ nameof( id ) }' parameter must be a valid GUID string." );
            }

            if ( !string.IsNullOrWhiteSpace( severity ) )
            {
                Severity = severity;
            }
            else
            {
                throw new ArgumentNullException( $"The '{ nameof( severity ) }' argument is required." );
            }
            ResponseCode = responseCode;
            if ( !string.IsNullOrWhiteSpace( description ) )
            {
                Description = description;
            }
            else
            {
                throw new ArgumentNullException( $"The '{ nameof( description ) }' argument is required." );
            }
            Details = details;
            ApplicationId = applicationId;
            ApplicationName = applicationName;
            try
            {
                if ( !string.IsNullOrWhiteSpace( dateTime ) && System.DateTime.TryParse( dateTime, out System.DateTime dateResult ) )
                {
                    this.DateTime = dateResult;
                }
            }
            catch ( Exception )
            {
                this.DateTime = default( DateTime? );
            }
            CorrelationId = correlationId;
            ApplicationSubType = applicationSubType;
            Resource = resource;
            Request = request;
        }

        /// <summary>
        /// The Info level severity for an Error object.
        /// </summary>
        public const string InfoLevel = "info";

        /// <summary>
        /// The Warning level severity for an Error object.
        /// </summary>
        public const string WarningLevel = "warning";

        /// <summary>
        /// The Error level severity for an Error object.
        /// </summary>
        public const string ErrorLevel = "error";

        private string severity;
        private string description;

        /// <summary>
        /// Get the ID for this error.
        /// </summary>
        /// <return>The Id.</return>
        [JsonProperty( "id" )]
        public string Id { get; set; }

        /// <summary>
        /// Get the Severity for the error. This should be one of info, warning, or error.
        /// </summary>
        /// <return>The Severity.</return>
        [JsonProperty( "severity" )]
        public string Severity
        {
            get
            {
                return this.severity;
            }
            set
            {
                if ( value == null )
                {
                    throw new ArgumentNullException( "The 'severity' argument is required." );
                }
                this.severity = value;
            }
        }

        /// <summary>
        /// Get the response code associated with this error.
        /// </summary>
        /// <return>The ResponseCode.</return>       
        [JsonProperty( "responseCode" )]
        public int ResponseCode { get; set; }

        /// <summary>
        /// Get the description for this error.
        /// </summary>
        /// <return>The Description.</return>
        [JsonProperty( "description" )]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if ( string.IsNullOrWhiteSpace( value ) )
                {
                    throw new ArgumentNullException( "The 'description' argument is required." );
                }
                this.description = value;
            }
        }

        /// <summary>
        /// Get the detailed message for this error.
        /// </summary>
        /// <return>The Details.</return>
        [JsonProperty( "details" )]
        public string Details { get; set; }

        /// <summary>
        /// Get the ID of the application that reported the error.
        /// </summary>
        /// <return>The ApplicationId.</return>
        [JsonProperty( "applicationId" )]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Get the name of the application that reported the error.
        /// </summary>
        /// <return>The ApplicationName.</return>
        [JsonProperty( "applicationName" )]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Get the date and time that the error was reported.
        /// </summary>
        /// <return>The DateTime.</return>
        [JsonProperty( "dateTime" )]
        public DateTime? DateTime { get; set; } = System.DateTime.Now;

        /// <summary>
        /// Get the correlation ID for this error. This is the ID of the original operation (event or message) to which this error can be traced.
        /// </summary>
        /// <return>The CorrelationId.</return>
        [JsonProperty( "correlationId" )]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Get the sub-type of the application that reported the error. This could be used to describe a sub-system.
        /// </summary>
        /// <return>A string value for the application sub-type.</return>
        [JsonProperty( "applicationSubtype" )]
        public string ApplicationSubType { get; set; }

        /// <summary>
        /// Get original request that caused the error.
        /// </summary>
        /// <return>A request object.</return>
        [JsonProperty( "request", NullValueHandling = NullValueHandling.Ignore )]
        public Request Request { get; set; }

        /// <summary>
        /// Get the resource associated with this error.
        /// </summary>
        /// <return>A resource object.</return>
        [JsonProperty( "resource" )]
        public Resource Resource { get; set; }

        /// <summary>
        /// Get the Error object as a JSON string.
        /// </summary>
        /// <returns>JSON string for the error object.</returns>
        public override string ToString() => JsonConvert.SerializeObject( this, Formatting.Indented );
    }
}
