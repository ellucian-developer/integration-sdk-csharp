/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Errors;

using System.Collections.Generic;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{

    public class ErrorFactoryTests
    {
        [Fact]
        public void ErrorFactory_CreateErrorFromJson_Null_Json()
        {
            var result = ErrorFactory.CreateErrorFromJson( null );
            Assert.Null( result );
        }

        [Fact]
        public void ErrorFactory_CreateErrorFromJson_EmptyString_Json()
        {
            var result = ErrorFactory.CreateErrorFromJson( string.Empty );
            Assert.Null( result );
        }

        [Fact]
        public void ErrorFactory_CreateErrorFromJson_OneRecord_Type_Error()
        {
            var json = SampleTestDataRepository.GetOneJsonRecordString();
            var result = ErrorFactory.CreateErrorFromJson( json );

            Assert.NotNull( result );
            Assert.IsType<EthosError>( result );
        }

        [Fact]
        public void ErrorFactory_CreateErrorArrayFromJson_OneRecord_Type_Error()
        {
            var json = SampleTestDataRepository.GetErrorMessage();
            var result = ErrorFactory.CreateErrorListFromJson( json );

            Assert.IsType<List<EthosError>>( result );
        }
    }
}
