/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Errors;

using System;
using System.Net;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{

    public class ErrorTests
    {
        EthosError error = new EthosError( Guid.NewGuid().ToString(), "Info", ( int ) HttpStatusCode.OK, "Descr 1", "Details 1", "1", "Banner", DateTime.Now.ToString(), "1", "SubType",
            new Resource( "1", "student-cohorts" ), new Request() { URI = "https://integrate.elluciancloud" } );

        EthosError error2 = new EthosError( Guid.NewGuid().ToString(), "Info", ( int ) HttpStatusCode.OK, "Descr 1", "Details 1", "1", "Banner", DateTime.Now.ToString(), "1", "SubType",
            new Resource( "1", "student-cohorts" ), new Request() { URI = "https://integrate.elluciancloud" } );

        [Fact]
        public void Error_Equals_False()
        {
            var isTrue = error.Equals( error2 );
            Assert.False( isTrue );
        }

        [Fact]
        public void Error_Description_Empty_String()
        {
            try
            {
                EthosError err = new EthosError( Guid.NewGuid().ToString(), "Info", ( int ) HttpStatusCode.OK, "  ", "Details 1", "1", "Banner", DateTime.Now.ToString(), "1", "SubType",
            new Resource( "1", "student-cohorts" ), new Request() { URI = "https://integrate.elluciancloud" } );
            }
            catch ( ArgumentNullException e )
            {
                Assert.True( e.GetType().Equals( typeof( ArgumentNullException ) ) );
            }
        }

        [Fact]
        public void Error_Description_Null_String()
        {
            try
            {
                EthosError err = new EthosError( Guid.NewGuid().ToString(), "Info", ( int ) HttpStatusCode.OK, null, "Details 1", "1", "Banner", DateTime.Now.ToString(), "1", "SubType",
            new Resource( "1", "student-cohorts" ), new Request() { URI = "https://integrate.elluciancloud" } );
            }
            catch ( ArgumentNullException e )
            {
                Assert.True( e.GetType().Equals( typeof( ArgumentNullException ) ) );
            }
        }

        [Fact]
        public void Error_Severity_Empty_String()
        {
            try
            {
                EthosError err = new EthosError( Guid.NewGuid().ToString(), " ", ( int ) HttpStatusCode.OK, "Descr 1", "Details 1", "1", "Banner", DateTime.Now.ToString(), "1", "SubType",
            new Resource( "1", "student-cohorts" ), new Request() { URI = "https://integrate.elluciancloud" } );
            }
            catch ( ArgumentNullException e )
            {
                Assert.True( e.GetType().Equals( typeof( ArgumentNullException ) ) );
            }
        }

        [Fact]
        public void Error_Severity_Null_String()
        {
            try
            {
                EthosError err = new EthosError( Guid.NewGuid().ToString(), null, ( int ) HttpStatusCode.OK, "Descr 1", "Details 1", "1", "Banner", DateTime.Now.ToString(), "1", "SubType",
            new Resource( "1", "student-cohorts" ), new Request() { URI = "https://integrate.elluciancloud" } );
            }
            catch ( ArgumentNullException e )
            {
                Assert.True( e.GetType().Equals( typeof( ArgumentNullException ) ) );
            }
        }

        [Fact]
        public void Error_Equal_Operator()
        {
            bool isEqual = false;
            if ( error == error2 )
            {
                isEqual = true;
            }

            Assert.False( isEqual );
        }

        [Fact]
        public void Error_Not_Equal_Operator()
        {
            bool isEqual = false;
            if ( error != error2 )
            {
                isEqual = true;
            }

            Assert.True( isEqual );
        }

        [Fact]
        public void Error_GetHashCode()
        {
            Assert.NotEqual( 0, error.GetHashCode() );
            Assert.NotEqual( error2.GetHashCode(), error.GetHashCode() );
        }

        [Fact]
        public void Error_ToString()
        {
            var strErr = error.ToString();
            var obj = ErrorFactory.CreateErrorFromJson( strErr );
            Assert.True( obj.GetType() == typeof( EthosError ) );
        }
    }
}
