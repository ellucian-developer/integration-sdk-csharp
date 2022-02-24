/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;
using System.Linq;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class HttpProtocolClientBuilderTests
    {
        [Fact]
        public void HttpProtocolClientBuilder()
        {
            var expected = SampleTestDataRepository.GetMockHttpClient();

            Assert.NotNull( expected );
            Assert.Equal( 3, expected.DefaultRequestHeaders.Count() );
            Assert.Equal( new TimeSpan( 0, 0, 0, 60000, 0 ), expected.Timeout );
        }
    }
}
