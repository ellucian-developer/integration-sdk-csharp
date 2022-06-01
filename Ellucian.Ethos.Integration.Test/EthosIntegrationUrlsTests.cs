/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Authentication;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosIntegrationUrlsTests
    {
        [Fact]
        public void EthosIntegrationUrls_ApiPaging()
        {
            string expected = "https://integrate.elluciancloud.com/api/student-cohorts?offset=10&limit=5";
            var actual = EthosIntegrationUrls.ApiPaging( SupportedRegions.US, "student-cohorts", 10, 5 );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }
        
        [Fact]
        public void EthosIntegrationUrls_QAPI()
        {
            string expected = "https://integrate.elluciancloud.com/qapi/student-cohorts";
            var actual = EthosIntegrationUrls.Qapi( SupportedRegions.US, "student-cohorts" );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void EthosIntegrationUrls_Errors()
        {
            string expected = "https://integrate.elluciancloud.com/errors";
            var actual = EthosIntegrationUrls.Errors( SupportedRegions.US );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void EthosIntegrationUrls_Auth()
        {
            string expected = "https://integrate.elluciancloud.com/auth";
            var actual = EthosIntegrationUrls.Auth( SupportedRegions.US );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void EthosIntegrationUrls_BaseUrl()
        {
            string expected = "https://integrate.elluciancloud.com";
            var actual = EthosIntegrationUrls.BaseUrl( SupportedRegions.US );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void EthosIntegrationUrls_ConsumeUrl()
        {
            string expected = "https://integrate.elluciancloud.com/consume?lastProcessedID=1&limit=1";
            var actual = EthosIntegrationUrls.Consume( SupportedRegions.US, 1, 1 );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void EthosIntegrationUrls_ConsumeUrl_With_lastProcessedID()
        {
            string expected = "https://integrate.elluciancloud.com/consume?lastProcessedID=1";
            var actual = EthosIntegrationUrls.Consume( SupportedRegions.US, 1, -1 );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void EthosIntegrationUrls_ConsumeUrl_With_limit()
        {
            string expected = "https://integrate.elluciancloud.com/consume?limit=1";
            var actual = EthosIntegrationUrls.Consume( SupportedRegions.US, -1, 1 );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void EthosIntegrationUrls_ConsumeUrl_With_lastProcessedID_limit_MinusOne_As_Input()
        {
            string expected = "https://integrate.elluciancloud.com/consume";
            var actual = EthosIntegrationUrls.Consume( SupportedRegions.US, -1, -1 );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }
    }
}
