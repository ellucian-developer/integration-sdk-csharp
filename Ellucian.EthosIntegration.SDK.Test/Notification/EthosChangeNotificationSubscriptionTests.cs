/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Service;
using Ellucian.Ethos.Integration.Test;

using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Notification.Test
{
    public class EthosChangeNotificationSubscriptionTests
    {
        EthosChangeNotificationService service = null;

        public EthosChangeNotificationSubscriptionTests()
        {
            service = SampleTestDataRepository.GetMockEthosChangeNotificationService();
        }

        [Fact]
        public async Task EthosChangeNotificationSubscription_RequestAsync()
        {
            AbstractEthosNotificationSubscription<ChangeNotification> subscription = new EthosChangeNotificationSubscription( service );
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
    }
}
