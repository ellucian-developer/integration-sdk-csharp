/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Service;
using Ellucian.Ethos.Integration.Test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Notification.Test
{
    public class EthosChangeNotificationListPollServiceTests
    {
        [Fact]
        public async Task SubscribeAsync()
        {
            EthosChangeNotificationService ethosChangeNotificationService = SampleTestDataRepository.GetMockEthosChangeNotificationService();

            UnitTestChangeNotificationListSubscriber subscriber = new UnitTestChangeNotificationListSubscriber();
            EthosChangeNotificationListPollService service = new EthosChangeNotificationListPollService( ethosChangeNotificationService, 2, 2 )
                .AddSubscriber( subscriber );

            Task task1 = service.SubscribeAsync();
            Task task2 = Task.Delay( new TimeSpan( 0, 0, 0, 5, 0 ) );
            Task task3 = await Task.WhenAny( task1, task2 );
            await task3.ContinueWith( t => service.Subscription.Cancel = true );
        }
        [Fact]
        public void GetNumberOfSubscribers_Tests()
        {
            EthosChangeNotificationService ethosChangeNotificationService = SampleTestDataRepository.GetMockEthosChangeNotificationService();

            UnitTestChangeNotificationListSubscriber subscriber = new UnitTestChangeNotificationListSubscriber();
            UnitTestChangeNotificationListSubscriber subscriber1 = new UnitTestChangeNotificationListSubscriber();
            EthosChangeNotificationListPollService service = new EthosChangeNotificationListPollService( ethosChangeNotificationService, 2, 2 )
                .AddSubscriber( subscriber )
                .AddSubscriber( subscriber1 );

            Assert.Equal( 2, service.GetNumberOfSubscribers() );
        }

        [Fact]
        public void GetSubscribers_Test()
        {
            EthosChangeNotificationService ethosChangeNotificationService = SampleTestDataRepository.GetMockEthosChangeNotificationService();

            UnitTestChangeNotificationListSubscriber subscriber = new UnitTestChangeNotificationListSubscriber();
            UnitTestChangeNotificationListSubscriber subscriber1 = new UnitTestChangeNotificationListSubscriber();
            EthosChangeNotificationListPollService service = new EthosChangeNotificationListPollService( ethosChangeNotificationService, 2, 2 )
                .AddSubscriber( subscriber )
                .AddSubscriber( subscriber1 );

            var actuals = service.GetSubscribers<UnitTestChangeNotificationListSubscriber>().ToList();

            Assert.Equal( subscriber, actuals [ 0 ] );
            Assert.Equal( subscriber1, actuals [ 1 ] );
        }

        [Fact]
        public void Unsubscribe_Test()
        {
            EthosChangeNotificationService ethosChangeNotificationService = SampleTestDataRepository.GetMockEthosChangeNotificationService();

            UnitTestChangeNotificationListSubscriber subscriber = new UnitTestChangeNotificationListSubscriber();
            UnitTestChangeNotificationListSubscriber subscriber1 = new UnitTestChangeNotificationListSubscriber();
            EthosChangeNotificationListPollService service = new EthosChangeNotificationListPollService( ethosChangeNotificationService, 2, 2 )
                .AddSubscriber( subscriber )
                .AddSubscriber( subscriber1 );
            service.Unsubscribe( subscriber1 );

            Assert.Single( service.GetSubscribers<UnitTestChangeNotificationListSubscriber>() );
        }

        [Fact]
        public void EthosChangeNotificationSubscriptionException_Null_Service()
        {
            _ = Assert.Throws<EthosChangeNotificationSubscriptionException>( () => new EthosChangeNotificationListPollService( null, 2, 5 ) );
        }
    }

    /// <summary>
    /// Mock of the client implementation of the AbstractEthosChangeNotificationSubscriber.
    /// </summary>
    public class UnitTestChangeNotificationListSubscriber: AbstractEthosChangeNotificationSubscriber<IEnumerable<ChangeNotification>>
    {
        public override void OnChangeNotification( IEnumerable<ChangeNotification> cns )
        {
            foreach ( var cn in cns )
            {
                Assert.NotNull( cn );
            }
        }
    }
}
