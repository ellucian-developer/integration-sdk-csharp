/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System.Net.Http;

namespace Ellucian.Ethos.Integration.Client
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpProtocolClientBuilder
	{
		/// <summary>
		/// Builds and exposes the <see cref="HttpClient"/>.
		/// </summary>
		HttpClient Client { get; }

		/// <summary>
		/// Creates instance of <see cref="System.Net.Http.HttpClient"/>. Called internally from EthosClientFactory./>
		/// </summary>
		/// <param name="connectionTimeout">An optional parameter indicating the amount of seconds before the client request should time out.</param>
		HttpClient BuildHttpClient( int? connectionTimeout );

		/// <summary>
		/// Creates instance of <see cref="System.Net.Http.HttpClient"/>. Called internally from EthosClientFactory./>
		/// </summary>
		HttpClient BuildHttpClient();
	}
}
