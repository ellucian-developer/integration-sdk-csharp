/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Filter.Extensions;
using Ellucian.Ethos.Integration.Client.Proxy.Filter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Client.Proxy
{
    /// <summary>
    /// An EthosProxyClient that provides the ability to submit GET requests supporting filters and/or named queries with support for paging.
    /// </summary>
    public class ColleagueWebApiFilterQueryClient : EthosFilterQueryClient
    {
        /// <summary>
        /// Instantiates this class using the given Colleague API url and credentials.
        /// </summary>
        /// <param name="colleagueApiUrl">The URL to the Colleague API instance. If it is null/empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        /// <param name="colleagueApiUsername">The username used to connect to the Colleague API. If it is null/empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        /// <param name="colleagueApiPassword">The password used to connect to the Colleague API. If it is null/empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        /// <param name="client">A HttpClient. If it is null/empty, then an <see cref="ArgumentNullException"/> will be thrown.</param>
        public ColleagueWebApiFilterQueryClient(string colleagueApiUrl, string colleagueApiUsername, string colleagueApiPassword, HttpClient client)
            : base(colleagueApiUrl, colleagueApiUsername, colleagueApiPassword, client)
        {
            Region = Authentication.SupportedRegions.SelfHosted;
            IntegrationUrls.MAIN_BASE_URL = colleagueApiUrl;
        }
    }
}
