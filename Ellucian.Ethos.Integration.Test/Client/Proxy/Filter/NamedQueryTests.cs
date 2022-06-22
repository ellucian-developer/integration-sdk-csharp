/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy.Filter;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

using Xunit;


namespace Ellucian.Ethos.Integration.Test.Client.Proxy.Filter
{
    public class NamedQueryTest
    {

        [Fact]
        public void WithNamedQuery_Tuple()
        {
            //Creates ?instructor={"instructor":{"id":"11111111-1111-1111-1111-111111111111"}}
            string label = "instructor";
            string guid = "11111111-1111-1111-1111-111111111111";
            string expected = $"?instructor={{\"instructor\":{{\"id\":\"{guid}\"}}}}";
            var result = new NamedQueryFilter( label ).WithNamedQuery( label, "id", guid ).BuildNamedQuery();
            Assert.NotNull( result );
            Assert.Equal( expected, result );
        }

        [Fact]
        public void WithNamedQuery_KeyValue()
        {
            //Creates ?keywordSearch={"keywordSearch":"History"}
            string label = "keywordSearch";
            string keywordValue = "History";
            string expected = $"?keywordSearch={{\"keywordSearch\":\"{keywordValue}\"}}";
            var result = new NamedQueryFilter( label ).WithNamedQuery( label, keywordValue ).BuildNamedQuery();
            Assert.NotNull( result );
            Assert.Equal( expected, result );
        }

        [Fact]
        public void WithNamedQuery_Multi_KeyValue()
        {
            //Creates ?accountSpecification={"accountingString":"11-01-01-00-11111-11111","amount":"2000","balanceOn":"2018-04-01","submittedBy":"11111111-1111-1111-1111-111111111112"}
            string label = "accountSpecification";
            string accountingString = "11-01-01-00-11111-11111";
            string amount = "2000";
            string balanceOn = "2018-04-01";
            string submittedBy = "11111111-1111-1111-1111-111111111112";
            string expected = $"?{label}={{\"accountingString\":\"{accountingString}\",\"amount\":\"{amount}\",\"balanceOn\":\"{balanceOn}\",\"submittedBy\":\"{submittedBy}\"}}";

            var result = new NamedQueryFilter( "accountSpecification" )
                                   .WithNamedQuery( ("accountingString", accountingString), ("amount", amount), ("balanceOn", balanceOn), ("submittedBy", submittedBy) )
                                   .BuildNamedQuery();
            Assert.NotNull( result );
            Assert.Equal( expected, result );
        }

        [Fact]
        public void WithNamedQuery_Complex()
        {
            string guid1 = "11111111-1111-1111-1111-111111111111";
            string guid2 = "11111111-1111-1111-1111-111111111112";
            string guid3 = "11111111-1111-1111-1111-111111111113";
            //?registrationStatusesByAcademicPeriod={"statuses":[{"detail":{"id":"11111111-1111-1111-1111-111111111111"}},{"detail":{"id":"11111111-1111-1111-1111-111111111112"}}],"academicPeriod":{"id":"11111111-1111-1111-1111-111111111113"}}
            var result = new NamedQueryFilter( "registrationStatusesByAcademicPeriod" )
                         .WithNamedQuery( "statuses", ("detail", "id", guid1), ("detail", "id", guid2) )
                         .WithNamedQuery( "academicPeriod", "id", guid3 )
                         .BuildNamedQuery();
            string expected = $"?registrationStatusesByAcademicPeriod={{\"statuses\":[{{\"detail\":{{\"id\":\"11111111-1111-1111-1111-111111111111\"}}}},{{\"detail\":{{\"id\":\"11111111-1111-1111-1111-111111111112\"}}}}],\"academicPeriod\":{{\"id\":\"11111111-1111-1111-1111-111111111113\"}}}}";

            Assert.NotNull( result );
            Assert.Equal( expected, result );
        }
    }
}
