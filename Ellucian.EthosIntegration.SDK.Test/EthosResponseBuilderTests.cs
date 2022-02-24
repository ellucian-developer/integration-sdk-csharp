/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosResponseBuilderTests : EthosResponseBuilder
    {
        [Fact]
        public void EthosResponseBuilder_BuildEthosResponse()
        {

            EthosResponseBuilder builder = new EthosResponseBuilder();
            Assert.NotNull( builder );            
        }
    }
}
