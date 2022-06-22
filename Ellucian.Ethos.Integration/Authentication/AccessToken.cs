/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;
using System.Collections.Generic;

namespace Ellucian.Ethos.Integration.Authentication
{
    /// <summary>
    /// An access token that can be used for authentication to make calls to Ethos Integration.
    /// To get an Authorization header that can be used to make HTTP requests, use the <see cref="GetAuthHeader()"/> method. This will
    /// return a Dictionary&lt;string, string&gt; containing a single entry of the Authorization header key/value pair. That can be used as-is or added
    /// to an existing headers map to pass to the EthosClient making the requests.
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// An encoded JWT string.
        /// </summary>
        /// <value>System.string</value>
        protected string Token { get; }

        /// <summary>
        /// Gets the time when this access token will expire.
        /// </summary>		
        /// <value>System.DateTime</value>
        public DateTime ExpirationTime { get; }

        /// <summary>
        /// Creates an instance of an access token that expires at the given time.
        /// </summary>
        /// <param name="token">An encoded JWT string.</param>
        /// <param name="expirationTime">The time when this token will expire.</param>
        public AccessToken( string token, DateTime expirationTime )
        {
            Token = token;
            ExpirationTime = expirationTime;
        }

        /// <summary>
        /// Determine if the session is still valid.
        /// </summary>
        /// <returns>If the token is still valid.</returns>
        public bool IsValid()
        {
            return DateTime.Now <= ExpirationTime;
        }
        /// <summary>
        /// Get an HTTP Authorization header containing the access token.
        /// </summary>
        /// <returns>Authorization header containing the access token.</returns>
        public Dictionary<string, string> GetAuthHeader()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add( "Authorization", $"Bearer { Token }" );
            return dict;
        }
    }
}