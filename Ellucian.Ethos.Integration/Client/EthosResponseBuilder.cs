/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Client
{
    /// <summary>
    /// Intended to be used to more easily build an EthosResponse object from the given <see cref="System.Net.Http.HttpResponseMessage"/>.
    /// </summary>
    public class EthosResponseBuilder
    {
        /// <summary>
        /// Builds an <see cref="EthosResponse"/> object from the given <see cref="System.Net.Http.HttpResponseMessage"/>. 
        /// The headerList is built taking header values from the given<see cref="System.Net.Http.HttpResponseMessage"/> for the 
        /// headers listed in <see cref="EthosResponse"/>.Also copies the response body content and Http status code.
        /// </summary>
        /// <param name="response"></param>
        /// <returns>An <see cref="EthosResponse"/>containing the headers values from the given <see cref="System.Net.Http.HttpResponseMessage"/>
        /// for the headers defined in <see cref="EthosResponse"/>, the response body content, and the response Http status code, 
        /// or null if the given <see cref="System.Net.Http.HttpResponseMessage"/> is null.</returns>
        internal async Task<EthosResponse> BuildEthosResponseAsync( HttpResponseMessage response )
        {
            if ( response == null && !response.Headers.Any() ) return null;

            string content = await response.Content.ReadAsStringAsync();
            EthosResponse ethosResponse = new EthosResponse( response.Headers, content, ( int ) response.StatusCode ) { RequestedUrl = response.RequestMessage.RequestUri.ToString() };
            return ethosResponse;
        }
    }
}
