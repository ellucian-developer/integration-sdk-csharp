/*
 * ******************************************************************************
 *   Copyright  2021-2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System.Dynamic;

namespace Ellucian.Ethos.Integration.Client.Proxy.Filter
{
    /// <summary>
    /// This class is used to build criteria for filtering for use with Ethos resource using version 8 or later. This class extends <see cref="ExpandoObject"/> dynamic object and 
    /// add methods to generate criteria used for filtering. There are three overloads of WithArray and four overloads of WithSimpleCriteria methods which help you to construct the criteria
    /// listed in the example below as well as in the code example above each method.
    ///     <code><b>
    ///     <b><h4>The data used in the examples is made up and should be replaced with real data in your environment.</h4></b>
    ///         Following are examples of the common patterns you will come across to create filters. Please take a look at the EthosFilterQueryClientExamples.
    ///         
    ///         Examples:
    ///         Ex1  ?criteria={ "firstName":"Angela" }
    ///         Ex2  ?criteria={ "names":{ "lastName":"Smith" } }
    ///         Ex3  ?criteria={ "paymentTarget":{ "deduction":{ "deductionType":{ "id":"11111111-1111-1111-1111-111111111112" } } } }
    ///         Ex4  ?criteria={ "names":{ "firstName":"Angela", "lastName":"Smith" } }
    ///         Ex5  ?criteria={ "statuses":["active","approved"] }
    ///         Ex6  ?criteria={ "academicLevels":[{"id":"11111111-1111-1111-1111-111111111111"},{ "id":"11111111-1111-1111-1111-111111111112"}]}
    ///         Ex7  ?criteria={ "authors":[{"person":{"id":"11111111-1111-1111-1111-111111111111"}}]}
    ///         Ex8  ?criteria={ "names":{ "personalNames":[{ "title":"Mr."},{ "title":"Mr"}]}}
    ///         Ex9  ?criteria={ "credentials":[{"type":{"id":"11111111-1111-1111-1111-111111111111"}},{ "value":"bannerId"}]}
    ///         Ex10 ?criteria={ "academicLevels":[{"id":"11111111-1111-1111-1111-111111111111","firstName":"John"}]}
    ///         Ex11 ?criteria={ "solicitors":[{"solicitor":{"constituent":{"person":{"id":"11111111-1111-1111-1111-111111111111"}}}}]}
    ///         Ex12 ?criteria={ "myName":[{"nestedName":{"names":{"firstName":"John"}}},{ "nestedName":{ "names":{ "firstName":"John"} } }]}
    ///         
    ///     </b></code> 
    /// </summary>
    public class CriteriaFilter
    {
        /// <summary>
        /// This class is used to build criteria for filtering for use with Ethos resource using version 8 or later.
        /// </summary>
        public CriteriaFilter()
        {
            CriteriaExpandoObject = new ExpandoObject();
        }

        /// <summary>
        /// Custom implicit operator.
        /// </summary>
        /// <param name="eo"><see cref="ExpandoObject"/></param>
        public static implicit operator CriteriaFilter( ExpandoObject eo )
        {
            return new CriteriaFilter()
            {
                CriteriaExpandoObject = eo
            };
        }

        /// <summary>
        /// Custom implicit operator.
        /// </summary>
        /// <param name="value"><see cref="CriteriaFilter"/></param>
        public static implicit operator ExpandoObject( CriteriaFilter value )
        {
            return value.CriteriaExpandoObject;
        }

        /// <summary>
        /// <see cref="ExpandoObject"/> used to generate the criteria dynamically.
        /// </summary>
        internal protected ExpandoObject CriteriaExpandoObject { get; private set; }

    }
}
