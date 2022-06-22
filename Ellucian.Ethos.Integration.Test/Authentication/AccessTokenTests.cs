/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Authentication;

using System;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class AccessTokenTests
    {
        DateTime testStartTime = DateTime.Now;

        [Fact]
        public void ApiTokensVerification()
        {
            AccessToken sessionToken = new AccessToken( SampleTestDataRepository.API_KEY, DateTime.Now.AddDays( 1 ) );
            Assert.NotNull( sessionToken );
        }

        [Fact]
        public void ApiTokensVerification_IsValid()
        {
            AccessToken sessionToken = new AccessToken( SampleTestDataRepository.API_KEY, DateTime.Now.AddDays( 1 ) );
            Assert.True( sessionToken.IsValid() );
        }

        [Fact]
        public void ApiTokensVerification_Is_Not_Valid()
        {
            AccessToken sessionToken = new AccessToken( SampleTestDataRepository.API_KEY, DateTime.Now.AddDays( -1 ) );
            Assert.False( sessionToken.IsValid() );
        }

        [Fact]
        public void ApiTokensVerification_GetAuthHeader()
        {
            AccessToken sessionToken = new AccessToken( SampleTestDataRepository.API_KEY, DateTime.Now.AddDays( -1 ) );
            var dict = sessionToken.GetAuthHeader();
            var key = dict [ "Authorization" ];
            Assert.NotNull( sessionToken );
            Assert.NotNull( dict );
            Assert.Equal( $"Bearer { SampleTestDataRepository.API_KEY }", key );
        }
    }
}
