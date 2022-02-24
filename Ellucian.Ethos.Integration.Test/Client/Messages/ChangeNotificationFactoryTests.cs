/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Test;

using Newtonsoft.Json.Linq;

using System;
using System.Linq;

using Xunit;

namespace Ellucian.Ethos.Integration.Test.Client.Messages
{
    public class ChangeNotificationFactoryTests
    {
        [Fact]
        public void ChangeNotificationFactory_SingleRecord()
        {
            var data = SampleTestDataRepository.GetChangeNotificationSingleJson();
            var actual = ChangeNotificationFactory.CreateCNFromJson( data );

            Assert.IsType<ChangeNotification>( actual );
            Assert.IsType<DateTime>( actual.Published );
            Assert.IsType<Publisher>( actual.Publisher );
            Assert.IsType<Resource>( actual.Resource );
            Assert.IsType<string>( actual.Operation );
            Assert.IsType<string>( actual.ContentType );
            Assert.IsType<JObject>( actual.Content );
        }

        [Fact]
        public void ChangeNotificationFactory_ArrayRecord()
        {
            var data = SampleTestDataRepository.GetChangeNotificationArrayJson();
            var actuals = ChangeNotificationFactory.CreateCNListFromJson( data );

            Assert.Equal( 2, actuals.Count() );
            foreach ( var actual in actuals )
            {
                Assert.IsType<ChangeNotification>( actual );
                Assert.IsType<DateTime>( actual.Published );
                Assert.IsType<Publisher>( actual.Publisher );
                Assert.IsType<Resource>( actual.Resource );
                Assert.IsType<string>( actual.Operation );
                Assert.IsType<string>( actual.ContentType );
                Assert.IsType<JObject>( actual.Content );
            }
        }
    }
}
