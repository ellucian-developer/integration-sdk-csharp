/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy;
using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Errors;

using System;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosErrorsClientTests
    {
        [Fact]
        public void EthosClientFactory_GetEthosProxyClient_SessionToken_Null()
        {
            Assert.Throws<ArgumentNullException>( () => new EthosClientBuilder(null).BuildEthosProxyClient() );
        }

        [Fact]
        public void EthosClientFactory_GetEthosProxyClient_With_SessionToken()
        {
            var client = new EthosClientBuilder(SampleTestDataRepository.API_KEY).BuildEthosProxyClient();
            Assert.NotNull( client );
            Assert.IsType<EthosProxyClient>( client );
        }

        [Fact]
        public void EthosClientFactory_GetEthosErrorsClient_SessionToken_()
        {
            Assert.Throws<ArgumentNullException>( () => new EthosClientBuilder(null).BuildEthosErrorsClient() );
        }

        [Fact]
        public void EthosClientFactory_GetEthosErrorsClient_With_SessionToken()
        {
            var client = new EthosClientBuilder(SampleTestDataRepository.API_KEY).BuildEthosErrorsClient();
            Assert.NotNull( client );
            Assert.IsType<EthosErrorsClient>( client );
        }
    }
}
