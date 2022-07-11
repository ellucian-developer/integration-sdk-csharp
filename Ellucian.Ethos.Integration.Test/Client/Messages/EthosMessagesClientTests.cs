/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;

using System;

using Xunit;

namespace Ellucian.Ethos.Integration.Test.Client.Messages
{
    public class EthosMessagesClientTests
    {
        #region Tests

        [Fact]
        public void EthosMessagesClient_ConsumeAsync_Limit_Zero_InvalidOperationException()
        {
            EthosMessagesClient client = new EthosMessagesClient( SampleTestData.API_KEY, SampleTestData.GetMockEthosMessagesClientWithArrayWithOKStatus() );
            try
            {
                _ = client.ConsumeAsync( 0, null );
            }
            catch ( Exception e )
            {
                Assert.IsType<InvalidOperationException>( e );
            }
        }

        [Fact]
        public void EthosMessagesClient_ConsumeAsync_Limit_Over_Thousand_InvalidOperationException()
        {
            EthosMessagesClient client = new EthosMessagesClient( SampleTestData.API_KEY, SampleTestData.GetMockEthosMessagesClientWithArrayWithOKStatus() );
            try
            {
                _ = client.ConsumeAsync( 1001, null );
            }
            catch ( Exception e )
            {
                Assert.IsType<InvalidOperationException>( e );
            }
        }

        [Fact]
        public void EthosMessagesClient_ConsumeAsync()
        {
            EthosMessagesClient client = new EthosMessagesClient( SampleTestData.API_KEY, SampleTestData.GetMockEthosMessagesClientWithArrayWithOKStatus() );
            var actuals = client.ConsumeAsync( 1, 0 );
            Assert.NotNull( actuals );

        }

        #endregion
    }
}
