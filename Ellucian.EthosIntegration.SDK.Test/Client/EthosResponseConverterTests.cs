/*
 * ******************************************************************************
 *   Copyright 2021 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net.Http;

using Xunit;

namespace Ellucian.Ethos.Integration.Test.Client
{
    public class EthosResponseConverterTests
    {
        HttpResponseMessage respMessage = new HttpResponseMessage();

        [ Fact]
        public void ToJArray_From_ResponseList()
        {
            EthosResponseConverter converter = new EthosResponseConverter();
            List<EthosResponse> responseList = new List<EthosResponse>();
            responseList.Add(new EthosResponse( respMessage.Headers, "[ {\"id\":\"1\"}, {\"id\":\"2\"} ]", 200 ) );
            responseList.Add(new EthosResponse( respMessage.Headers, "[ {\"id\":\"3\"} ]", 200));
            JArray response = converter.ToJArray(responseList);
            Assert.Equal(3, response.Count);
        }

        [Fact]
        public void ToJArray_From_ResponseList_NotArray()
        {
            EthosResponseConverter converter = new EthosResponseConverter();
            List<EthosResponse> responseList = new List<EthosResponse>();
            responseList.Add( new EthosResponse( respMessage.Headers, "[ {\"id\":\"1\"}, {\"id\":\"2\"} ]", 200 ) );
            responseList.Add( new EthosResponse( respMessage.Headers, "{\"id\":\"3\"}", 200 ) );
            // should throw an exception because the 2nd response content is not in a JSON array format
            Assert.Throws<InvalidCastException>( () => converter.ToJArray(responseList) );
        }

        [Fact]
        public void ToStringList_From_ResponseList()
        {
            EthosResponseConverter converter = new EthosResponseConverter();
            List<EthosResponse> responseList = new List<EthosResponse>();
            responseList.Add( new EthosResponse( respMessage.Headers, "[ {\"id\":\"1\"}, {\"id\":\"2\"} ]", 200 ) );
            responseList.Add( new EthosResponse( respMessage.Headers, "[{\"id\":\"3\"}]", 200 ) );
            List<string> response = (List<string>)converter.ToStringList(responseList);
            Assert.Equal(3, response.Count);
        }

    }
}
