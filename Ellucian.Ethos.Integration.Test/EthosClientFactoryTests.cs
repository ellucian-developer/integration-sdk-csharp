/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy;
using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Errors;

using System;

using Xunit;
using Ellucian.Ethos.Integration.Config;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosClientFactoryTests
    {
        [Fact]
        public void EthosClientFactory_GetEthosProxyClient_SessionToken_Null()
        {
            Assert.Throws<ArgumentNullException>( () => new EthosClientBuilder( null ).BuildEthosProxyClient() );
        }

        [Fact]
        public void EthosClientFactory_GetEthosProxyClient_With_SessionToken()
        {
            var client = new EthosClientBuilder( SampleTestData.API_KEY ).WithConnectionTimeout( 30 ).BuildEthosProxyClient();
            Assert.NotNull( client );
            Assert.IsType<EthosProxyClient>( client );
        }

        [Fact]
        public void EthosClientFactory_GetEthosErrorsClient_SessionToken_()
        {
            Assert.Throws<ArgumentNullException>( () => new EthosClientBuilder( null ).BuildEthosErrorsClient() );
        }

        [Fact]
        public void EthosClientFactory_GetEthosErrorsClient_With_SessionToken()
        {
            var client = new EthosClientBuilder( SampleTestData.API_KEY ).WithConnectionTimeout( 30 ).BuildEthosErrorsClient();
            Assert.NotNull( client );
            Assert.IsType<EthosErrorsClient>( client );
        }

        [Fact]
        public void EthosClientFactory_EthosConfigurationClient_SessionToken_()
        {
            Assert.Throws<ArgumentNullException>( () => new EthosClientBuilder( null ).WithConnectionTimeout( 60 ).BuildEthosErrorsClient() );
        }

        [Fact]
        public void EthosClientFactory_EthosConfigurationClient_With_SessionToken()
        {
            var client = new EthosClientBuilder( SampleTestData.API_KEY ).BuildEthosConfigurationClient();
            Assert.NotNull( client );
            Assert.IsType<EthosConfigurationClient>( client );
        }
    }
}
