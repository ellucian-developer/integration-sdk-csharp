/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Ellucian.Ethos.Integration.Test.Client.Messages
{
    public class ChangeNotificationTests
    {
        [Fact]
        public void ChangeNotificationTests_SingleRecord()
        {
            var data = SampleTestDataRepository.GetChangeNotificationSingleJson();
            var o = JObject.Parse( data );
            var actual = o.ToObject<ChangeNotification>();

            Assert.IsType<ChangeNotification>( actual );
            Assert.IsType<DateTime>( actual.Published );
            Assert.IsType<Publisher>( actual.Publisher );
            Assert.IsType<Resource>( actual.Resource );
            Assert.IsType<string>( actual.Operation );
            Assert.IsType<string>( actual.ContentType );
            Assert.IsType<JObject>( actual.Content );
        }

        [Fact]
        public void ChangeNotificationTests_ArrayRecord()
        {
            var data = SampleTestDataRepository.GetChangeNotificationArrayJson();
            JArray parsedCN = JArray.Parse( data );
            var actuals = parsedCN.ToObject<IEnumerable<ChangeNotification>>();

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
