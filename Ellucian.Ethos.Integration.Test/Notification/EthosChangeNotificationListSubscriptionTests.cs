/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Service;
using Ellucian.Ethos.Integration.Test;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Notification.Test
{
    public class EthosChangeNotificationListSubscriptionTests
    {
        EthosChangeNotificationService service = null;

        public EthosChangeNotificationListSubscriptionTests()
        {
            service = SampleTestData.GetMockEthosChangeNotificationService();
        }

        [Fact]
        public async Task EthosChangeNotificationSubscription_RequestAsync()
        {
            AbstractEthosNotificationSubscription<IEnumerable<ChangeNotification>> subscription = new EthosChangeNotificationListSubscription( service );
            Assert.NotNull( subscription );
            var actuals = await subscription.RequestAsync( null );

            foreach ( var actual in actuals )
            {
                Assert.NotNull( actual );
                Assert.NotNull( actual.Content );
                Assert.NotNull( actual.Resource );
                Assert.NotNull( actual.ContentType );
                Assert.NotNull( actual.Id );
                Assert.NotNull( actual.Operation );
                Assert.NotNull( actual.Publisher );
            }
        }

        [Fact]
        public void EthosChangeNotificationSubscriptionException_Null_Service()
        {
            _ = Assert.Throws<EthosChangeNotificationSubscriptionException>( () => new EthosChangeNotificationPollService( null, 2, 5 ) );
        }
    }
}
