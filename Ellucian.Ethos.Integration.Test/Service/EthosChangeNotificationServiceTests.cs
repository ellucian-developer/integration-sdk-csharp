/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Service;
using Ellucian.Ethos.Integration.Test;

using Moq;

using System;
using System.Reflection;
using System.Threading.Tasks;

using Xunit;

namespace Ellucian.Ethos.Integration.Test.Service
{
    public class EthosChangeNotificationServiceTests
    {
        string apiKey = SampleTestDataRepository.API_KEY;
        string resourceName = "accounting-string-component-values";
        string versionShort = "V8";
        string versionLong = "application/vnd.hedtech.integration.v8+json";

        public EthosChangeNotificationServiceTests()
        {
        }

        [Fact]
        public async Task GetChangeNotificationsAsync()
        {
            EthosChangeNotificationService service = SampleTestDataRepository.GetMockEthosChangeNotificationService();

            var actual = await service.GetChangeNotificationsAsync( null );
            Assert.NotNull( actual );
        }

        [Fact]
        public void BuilderTest_WithResourceAbbreviatedVersionOverride()
        {
            EthosChangeNotificationService service = EthosChangeNotificationService.Build( a =>
             {
                 a.WithConnectionTimeout( 10 )
                  .WithEthosClientBuilder( new EthosClientBuilder( apiKey ) )
                  .WithResourceAbbreviatedVersionOverride( resourceName, versionShort );
             }, apiKey );

            Assert.NotNull( service );
            Assert.Equal( versionLong, service.GetOverriddenResourcesVersion( resourceName ) );
            Assert.Single( service.GetOverriddenResources() );
        }

        [Fact]
        public void WithResourceAbbreviatedVersionOverride_RemoveResource()
        {
            EthosChangeNotificationService service = EthosChangeNotificationService.Build( a =>
            {
                a.WithConnectionTimeout( 10 )
                 .WithEthosClientBuilder( new EthosClientBuilder( apiKey ) )
                 .WithResourceAbbreviatedVersionOverride( resourceName, versionShort );
            }, apiKey );

            service.RemoveResourceVersionOverride( resourceName );

            Assert.NotNull( service );
            Assert.Empty( service.GetOverriddenResources() );
        }

        [Fact]
        public void WithResourceAbbreviatedVersionOverride_RemoveResource_Then_AddResource()
        {
            EthosChangeNotificationService service = EthosChangeNotificationService.Build( a =>
            {
                a.WithConnectionTimeout( 10 )
                 .WithEthosClientBuilder( new EthosClientBuilder( apiKey ) )
                 .WithResourceAbbreviatedVersionOverride( resourceName, versionShort );
            }, apiKey );

            service.RemoveResourceVersionOverride( resourceName );
            Assert.NotNull( service );
            Assert.Empty( service.GetOverriddenResources() );
            service.AddResourceVersionOverride( resourceName, versionLong );
            Assert.NotEmpty( service.GetOverriddenResources() );
        }

        [Fact]
        public void BuilderTest_WithResourceVersionOverride()
        {
            EthosChangeNotificationService service = EthosChangeNotificationService.Build( a =>
            {
                a.WithConnectionTimeout( 10 )
                 .WithEthosClientBuilder( new EthosClientBuilder( apiKey ) )
                 .WithResourceVersionOverride( resourceName, versionLong );
            }, apiKey );

            Assert.NotNull( service );
        }

        [Fact]
        public void BuilderTest_WithResourceVersionOverride_ResourceName_Null_ArgumentNullException()
        {
            _ = Assert.Throws<ArgumentNullException>( () =>
             {
                 EthosChangeNotificationService service = EthosChangeNotificationService.Build( a =>
                 {
                     a.WithConnectionTimeout( 10 )
                      .WithResourceVersionOverride( string.Empty, versionLong );
                 }, apiKey );
             } );
        }

        [Fact]
        public void BuilderTest_WithResourceVersionOverride_Version_Null_ArgumentNullException()
        {
            _ = Assert.Throws<ArgumentNullException>( () =>
            {
                EthosChangeNotificationService service = EthosChangeNotificationService.Build( a =>
                {
                    a.WithConnectionTimeout( 10 )
                     .WithResourceVersionOverride( resourceName, string.Empty );
                }, apiKey );
            } );
        }

        [Fact]
        public void BuilderTest_RemoveResourceVersionOverride_Version_Null_ArgumentNullException()
        {
            _ = Assert.Throws<ArgumentNullException>( () =>
            {
                EthosChangeNotificationService service = EthosChangeNotificationService.Build( a =>
                {
                    a.WithConnectionTimeout( 10 )
                     .WithResourceVersionOverride( resourceName, versionLong );
                }, apiKey );
                service.RemoveResourceVersionOverride( string.Empty );
            } );
        }
    }
}
