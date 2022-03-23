/*
 * ******************************************************************************
 *   Copyright  2021 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Ellucian.Ethos.Integration.Client
{
    /// <summary>
    /// Response object used by the Ethos SDK to contain specific response headers, response body content, and the HTTP response status code.
    /// This class contains mostly getter methods on attributes to reduce the possibility of response values being changed.
    /// </summary>
    public class EthosResponse
    {
        /// <summary>
        /// The Http status code of the Http response.
        /// </summary>
        public int HttpStatusCode { get; }

        /// <summary>
        /// The response body content.
        /// </summary>
        public string Content { get; internal set; }

        /// <summary>
        /// The URL that the corresponding request was made for.
        /// </summary>
        public string RequestedUrl { get; set; }

        /// <summary>
        /// Content of a strongly typed object.
        /// </summary>
        public dynamic Dto { get; internal set; }

        /// <summary>
        /// The response body content as JToken.
        /// </summary>
        /// <returns></returns>
        public JToken GetContentAsJson()
        {
            if ( !string.IsNullOrWhiteSpace( Content ) )
            {
                var token = JToken.Parse( Content );
                if ( token is JArray )
                {
                    return token.ToObject<JArray>();
                }
                if ( token is JObject )
                {
                    return token.ToObject<JObject>();
                }
            }
            return null;
        }

        /// <summary>
        /// Gets count of number of rows in the json content. If the content is of type JArray
        /// then we get count from JArray. If its JObject then it's going to be only one.
        /// </summary>
        /// <returns>Total number of records.</returns>
        public int GetContentCount()
        {
            int count = 0;
            if ( !string.IsNullOrWhiteSpace( Content ) )
            {
                var token = JToken.Parse( Content );
                if ( token is JArray )
                {
                    JArray array = token.ToObject<JArray>();
                    if ( array.HasValues && array.Any() )
                    {
                        count = array.Count;
                    }
                }

                if ( token is JObject )
                {
                    JObject obj = token.ToObject<JObject>();
                    if ( obj != null && obj.HasValues )
                    {
                        count = 1;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// The http response headers.
        /// </summary>
        public HttpResponseHeaders HeadersMap { get; internal set; }

        /// <summary>
        /// Gets the keys in the header map, which can be used to retrieve specific header values from <see cref="HttpResponseHeaders"/>.
        /// </summary>
        /// <returns>A list of header map keys.</returns>
        public IEnumerable<string> GetHeaderMapKeys()
        {
            List<string> keySet = new List<string>();

            if ( HeadersMap != null && HeadersMap.Any() )
            {
                HeadersMap.ToList().ForEach( i =>
                {
                    if ( !string.IsNullOrWhiteSpace( i.Key ) )
                    {
                        keySet.Add( i.Key );
                    }
                } );
            }
            return keySet;
        }

        /// <summary>
        /// Returns the header for the given key.
        /// </summary>
        /// <param name="headerKey">The key used to get the header from the headerMap.</param>
        /// <returns>The value for the <see cref="System.Net.Http.Headers.HttpResponseHeaders"/> or null.</returns>
        /// <exception cref="InvalidOperationException"/>
        public string GetHeader( string headerKey )
        {
            if ( string.IsNullOrWhiteSpace( headerKey ) )
            {
                return null;
            }

            if ( HeadersMap != null )
            {
                string headerValue;
                try
                {
                    var headerVal = HeadersMap.GetValues( headerKey );
                    headerValue = headerVal.FirstOrDefault();
                }
                catch ( Exception )
                {
                    headerValue = null;
                }
                return headerValue;
            }
            return null;
        }

        /// <summary>
        /// Instantiates an EthosResponse object with the given parameters. 
        /// The parameters are intended to be supplied from the <see cref="EthosResponseBuilder"/> to build this object from an <see cref="System.Net.Http.HttpResponseMessage"/>.
        /// </summary>
        /// <param name="headersMap">A map of headers containing the defined header constants in this class as keys.</param>
        /// <param name="content">The response body content.</param>
        /// <param name="statusCode">The Http status code of the response.</param>
        public EthosResponse( HttpResponseHeaders headersMap, string content, int statusCode )
        {
            HeadersMap = headersMap;
            Content = content;
            HttpStatusCode = statusCode;
        }
    }
}
