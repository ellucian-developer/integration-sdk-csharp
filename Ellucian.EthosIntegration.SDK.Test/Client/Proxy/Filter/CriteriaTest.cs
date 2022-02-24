/*
 * ******************************************************************************
 *   Copyright  2021 Ellucian Company L.P. and its affiliates.
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
    public class CriteriaTest
    {
        [Fact]
        public void Empty()
        {
            var result = new CriteriaFilter();
            Assert.NotNull( result );
            Assert.Equal( "?criteria={}", result.BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteriaKeyValue()
        {
            var result = new CriteriaFilter()
                     .WithSimpleCriteria( "lastName", "Smith" )
                     .BuildCriteria();
            Assert.NotNull( result );
            Assert.Equal( "?criteria={\"lastName\":\"Smith\"}", result );
        }

        [Fact]
        public void WithSimpleCriteriaKeyValue_Key_Null_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter().WithSimpleCriteria( "", "Smith" ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteriaKeyValue_Value_Null_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter().WithSimpleCriteria( "lastName", "" ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue()
        {
            var result = new CriteriaFilter()
                     .WithSimpleCriteria( "firstName", "John" )
                     .WithSimpleCriteria( "lastName", "Smith" )
                     .BuildCriteria();
            Assert.NotNull( result );
            Assert.Equal( "?criteria={\"firstName\":\"John\",\"lastName\":\"Smith\"}", result );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_First_Key_Null_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter().WithSimpleCriteria( "", "Smith" )
                     .WithSimpleCriteria( "firstName", "Jon" ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_First_Value_Null_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter().WithSimpleCriteria( "lastName", "" )
                     .WithSimpleCriteria( "firstName", "Jon" ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_Second_Key_Null_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter().WithSimpleCriteria( "lastName", "Smith" )
                     .WithSimpleCriteria( "", "Jon" ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_Second_Value_Null_Exception()
        {
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter().WithSimpleCriteria( "lastName", "Smith" )
                     .WithSimpleCriteria( "firstName", "" ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_With_Dictionary()
        {
            var result = new CriteriaFilter()
                     .WithSimpleCriteria( "names", ("firstName", "John"), ("lastName", "Smith") )
                     .BuildCriteria();
            Assert.NotNull( result );
            Assert.Equal( "?criteria={\"names\":{\"firstName\":\"John\",\"lastName\":\"Smith\"}}", result );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_With_Dictionary_FirstNameKeyNull_ArgumentNullException()
        {
            Dictionary<string, object> names = new Dictionary<string, object>();
            names.Add( "", "John" );
            names.Add( "lastName", "Smith" );
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter()
                     .WithSimpleCriteria( "names", names ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_With_Dictionary_FirstNameValueNull_ArgumentNullException()
        {
            Dictionary<string, object> names = new Dictionary<string, object>();
            names.Add( "firstName", "" );
            names.Add( "lastName", "Smith" );
            _ = Assert.Throws<ArgumentNullException>( () => new CriteriaFilter()
                     .WithSimpleCriteria( "names", names ).BuildCriteria() );
        }

        [Fact]
        public void WithSimpleCriteria2KeyValue_With_Label_Tuple()
        {
            var result = new CriteriaFilter()
                         .WithSimpleCriteria( "names", ("lastName", "Smith"), ("firstName", "John") )
                         .WithSimpleCriteria( "anotherName" )
                         .WithSimpleCriteria( "someName" )

                     .BuildCriteria();
            Assert.NotNull( result );
            Assert.Equal( "?criteria={\"someName\":{\"anotherName\":{\"names\":{\"lastName\":\"Smith\",\"firstName\":\"John\"}}}}", result );
        }

        [Fact]
        public void WithArray_WithSimpleCriterias_WithTuple()
        {
            //?criteria={"myName":[{"nestedName":{"names":{"firstName":"John"}}},{"nestedName":{"names":{"firstName":"John"}}}]}
            CriteriaFilter criteria = new CriteriaFilter();
            var result = criteria.WithArray( "myName",
                         new CriteriaFilter().WithSimpleCriteria( "nestedName",
                         new CriteriaFilter().WithSimpleCriteria( "names", ("firstName", "John") ) ),
                         new CriteriaFilter().WithSimpleCriteria( "nestedName",
                         new CriteriaFilter().WithSimpleCriteria( "names", ("lastName", "Smith") ) ) )
                         .BuildCriteria();

            Assert.NotNull( result );
            Assert.Equal( "?criteria={\"myName\":[{\"nestedName\":{\"names\":{\"firstName\":\"John\"}}},{\"nestedName\":{\"names\":{\"lastName\":\"Smith\"}}}]}", result );
        }

        [Fact]
        public void WithArray2KeyValue_With_Label_Dictionary()
        {
            Dictionary<string, object> names = new Dictionary<string, object>();
            names.Add( "lastName", "Smith" );
            names.Add( "firstName", "John" );
            var result = new CriteriaFilter().WithArray( "names", names).BuildCriteria();
            string expected = "?criteria={\"names\":[{\"lastName\":\"Smith\"},{\"firstName\":\"John\"}]}";
            Assert.NotNull( result );
            Assert.Equal( expected, result );
        }

        [Fact]
        public void WithArray2KeyValue_With_Label_Tuple()
        {
            var result = new CriteriaFilter().WithArray( "emails", ("address", "125949@hdjdj.com"), ("address", "09596 - 03@kdkldk.com") ).BuildCriteria();
            string expected = "?criteria={\"emails\":[{\"address\":\"125949@hdjdj.com\"},{\"address\":\"09596 - 03@kdkldk.com\"}]}";
            Assert.NotNull( result );
            Assert.Equal( expected, result );
        }
    }
}
