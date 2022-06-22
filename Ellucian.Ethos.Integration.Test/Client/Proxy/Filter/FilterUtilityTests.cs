/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy.Filter;

using System;
using System.Collections.Generic;
using System.Dynamic;

using Xunit;


namespace Ellucian.Ethos.Integration.Test.Client.Proxy.Filter
{
    public class FilterUtilityTest
    {
        [Fact]
        public void Validate()
        {
            //Creates ?accountSpecification={"accountingString":"11-01-01-00-11111-11111","amount":"2000","balanceOn":"2018-04-01","submittedBy":"11111111-1111-1111-1111-111111111112"}
            string label = "accountSpecification";
            string accountingString = "11-01-01-00-11111-11111";
            string amount = "";
            string balanceOn = "2018-04-01";
            string submittedBy = "11111111-1111-1111-1111-111111111112";
            string expected = $"?{label}={{\"accountingString\":\"{accountingString}\",\"amount\":\"{amount}\",\"balanceOn\":\"{balanceOn}\",\"submittedBy\":\"{submittedBy}\"}}";

            _ = Assert.Throws<ArgumentNullException>( () => new NamedQueryFilter( "accountSpecification" )
                                  .WithNamedQuery( ("accountingString", accountingString), ("amount", amount), ("balanceOn", balanceOn), ("submittedBy", submittedBy) )
                                  .BuildNamedQuery() );
        }

        [Fact]
        public void Add()
        {
            ExpandoObject eo = FilterUtility.Add( "FirstName", "John" );

            IDictionary<string, object> dict = ( IDictionary<string, object> ) eo;
            Assert.NotNull( dict );
            Assert.True( dict.ContainsKey( "FirstName" ) );
            Assert.True( dict.Values.Contains( "John" ) );
        }

        [Fact]
        public void BuildCriteria()
        {
            ExpandoObject eo = FilterUtility.Add( "FirstName", "John" );
            var actual = FilterUtility.BuildCriteria( eo );
            Assert.NotNull( actual );
            Assert.Equal( "?criteria={\"FirstName\":\"John\"}", actual );
        }

        [Theory]
        [InlineData( "FirstName", "John", "?criteria={\"FirstName\":\"John\"}" )]
        [InlineData( "LastName", "Smith", "?criteria={\"LastName\":\"Smith\"}" )]
        public void BuildCriteria2( string key, string value, string expected )
        {
            ExpandoObject eo = FilterUtility.Add( key, value );
            var actual = FilterUtility.BuildCriteria( eo );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void BuildNamedQuery()
        {
            string label = "accountSpecification";
            string accountingString = "11-01-01-00-11111-11111";
            string amount = "100";
            string balanceOn = "2018-04-01";
            string submittedBy = "11111111-1111-1111-1111-111111111112";
            string expected = $"?{label}={{\"accountingString\":\"{accountingString}\",\"amount\":\"{amount}\",\"balanceOn\":\"{balanceOn}\",\"submittedBy\":\"{submittedBy}\"}}";

            NamedQueryFilter eo = new NamedQueryFilter( "accountSpecification" )
                                  .WithNamedQuery( ("accountingString", accountingString), ("amount", amount), ("balanceOn", balanceOn), ("submittedBy", submittedBy) );
            var actual = FilterUtility.BuildNamedQuery( eo );
            Assert.NotNull( actual );
            Assert.Equal( expected, actual );
        }

        [Fact]
        public void Build_Null()
        {
            _ = Assert.Throws<ArgumentNullException>( () => FilterUtility.Build<ExpandoObject>( null ) );
        }
    }
}
