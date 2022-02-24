/*
 * ******************************************************************************
 *   Copyright  2021 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{
    public class EthosResponseTests
    {

        [Fact]
        public void EthosResponse_Constructor_With_ThreeInput_Parameters()
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse response = new EthosResponse( headers, "{'id'}: '1234'", 201 );
            var dateHeaderValue = headers.GetValues( "Date" );
            Assert.NotNull( response );
            Assert.Equal( headers.Count(), response.HeadersMap.Count() );
            Assert.Equal( 201, response.HttpStatusCode );
            Assert.NotNull( response.HeadersMap );
            Assert.NotNull( dateHeaderValue );
        }

        [Fact]
        public void EthosResponse_GetHeaderMapKeys()
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse response = new EthosResponse( headers, "{'id'}: '1234'", 201 );
            var actual = response.GetHeaderMapKeys();
            Assert.NotNull( actual );
            Assert.Equal( 2, actual.Count() );
        }

        [Fact]
        public void EthosResponse_GetHeader()
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse response = new EthosResponse( headers, "{'id'}: '1234'", 201 );
            var actual = response.GetHeader( "pragma" );
            Assert.NotNull( actual );
            Assert.Equal( "no-cache", actual );
        }

        [Fact]
        public void EthosResponse_GetHeader_Exception()
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse response = new EthosResponse( headers, "{'id'}: '1234'", 201 );
            var actual = response.GetHeader( "ABC" );
            Assert.Null( actual );
        }

        [Theory]
        [InlineData( "{\"id\":\"123\"}", 200, "{\"id\":\"123\"}" )]
        public void EthosResponse_GetContentAsJson_As_JObject( string actual, int statusCode, string expected )
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse resp = new EthosResponse( headers, actual, statusCode );
            Assert.NotNull( resp );
            Assert.Equal( expected, actual );
            Assert.IsType<JObject>( resp.GetContentAsJson() );
        }

        [Theory]
        [InlineData( "[{\"id\":\"123\"}, {\"id\":\"456\"}]", 200, "[{\"id\":\"123\"}, {\"id\":\"456\"}]" )]
        public void EthosResponse_GetContentAsJson_As_JArray( string actual, int statusCode, string expected )
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse resp = new EthosResponse( headers, actual, statusCode );
            Assert.NotNull( resp );
            Assert.Equal( expected, actual );
            Assert.IsType<JArray>( resp.GetContentAsJson() );
        }

        [Fact]
        public void EthosResponse_GetContentCount_Array()
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse resp = new EthosResponse( headers, "[{\"id\":\"123\"}, {\"id\":\"456\"}]", 200 );
            Assert.NotNull( resp );
            Assert.Equal( 2, resp.GetContentCount() );
            Assert.IsType<JArray>( resp.GetContentAsJson() );
        }

        [Fact]
        public void EthosResponse_GetContentCount_Object()
        {
            System.Net.Http.Headers.HttpResponseHeaders headers = GetHttpResponseHeaders();
            EthosResponse resp = new EthosResponse( headers, "{\"id\":\"456\"}", 200 );
            Assert.NotNull( resp );
            Assert.Equal( 1, resp.GetContentCount() );
            Assert.IsType<JObject>( resp.GetContentAsJson() );
        }

        private static System.Net.Http.Headers.HttpResponseHeaders GetHttpResponseHeaders()
        {
            var message = new HttpResponseMessage();
            var headers = message.Headers;
            var date = DateTimeOffset.Now;
            headers.Add( "Date", new List<string>() { date.ToString( "ddd, dd MMM yyyy HH:mm:ss 'GMT'" ) } );
            headers.Add( "pragma", "no-cache" );
            return headers;
        }
    }
}
