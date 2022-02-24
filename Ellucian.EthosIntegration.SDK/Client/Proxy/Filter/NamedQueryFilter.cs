/*
 * ******************************************************************************
 *   Copyright  2021 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Dynamic;

namespace Ellucian.Ethos.Integration.Client.Proxy.Filter
{
    /// <summary>
    /// This class is used to build named queries for use with Ethos resource using version 8 or later. This class uses <see cref="ExpandoObject"/> dynamic object and add methods to generate named query.
    /// There are three overloads of WithNamedQuery method. Following are the examples of some of the named queries used in Ethos API's
    /// <code><b>
    ///     <b><h4>The data used in the examples is made up and should be replaced with real data in your environment.</h4></b>
    ///     Following are just examples of the common patterns you will come across to create named queries.
    ///     
    ///     Resource: sections
    ///     sections?instructor={"instructor":{"id":"11111111-1111-1111-1111-111111111111"}}
    ///     sections?keywordSearch={"keywordSearch":"History"}
    ///     sections?searchable={"searchable":"hidden"}
    ///     
    ///     Resource: persons
    ///     persons?personFilter= {"personFilter":{"id":"11111111-1111-1111-1111-111111111111"}}
    /// 
    ///     Resource: account-funds-available
    ///     account-funds-available?accountSpecification={"accountingString":"11-01-01-00-11111-11111","amount":"2000","balanceOn":"2018-04-01","submittedBy":"11111111-1111-1111-1111-111111111112"}
    ///     
    /// </b></code>
    /// </summary>
    public class NamedQueryFilter
    {
        /// <summary>
        /// This class is used to build criteria for filtering for use with Ethos resource using version 8 or later.
        /// </summary>
        public NamedQueryFilter()
        {
            NamedQueryExpandoObject = new ExpandoObject();
        }

        /// <summary>
        /// This class is used to build criteria for filtering for use with Ethos resource using version 8 or later.
        /// </summary>
        /// <param name="namedQueryLabel">Label for the named query.</param>
        public NamedQueryFilter( string namedQueryLabel )
        {
            NamedQueryLabel = namedQueryLabel;
            NamedQueryExpandoObject = new ExpandoObject();
        }

        /// <summary>
        /// Custom implicit operator.
        /// </summary>
        /// <param name="eo"><see cref="ExpandoObject"/></param>
        public static implicit operator NamedQueryFilter( ExpandoObject eo )
        {
            return new NamedQueryFilter()
            {
                NamedQueryExpandoObject = eo
            };
        }

        /// <summary>
        /// Custom implicit operator.
        /// </summary>
        /// <param name="value"><see cref="CriteriaFilter"/>.</param>        
        public static implicit operator ExpandoObject( NamedQueryFilter value )
        {
            return value.NamedQueryExpandoObject;
        }

        /// <summary>
        /// <see cref="ExpandoObject"/> used to generate the criteria dynamically.
        /// </summary>
        internal protected ExpandoObject NamedQueryExpandoObject { get; private set; }

        /// <summary>
        /// Label for the query.
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, NullValueHandling = NullValueHandling.Ignore )]
        public string NamedQueryLabel { get; set; }
    }

    /// <summary>
    /// This class adds extension methods to <see cref="NamedQueryFilter"/> class.
    /// </summary>
    public static class NamedQueryExtensions
    {

        #region WithNamedQuery
        /// <summary>
        ///     Example:
        ///     <code><b>
        ///     
        ///     //Creates ?keywordSearch={"keywordSearch":"History"}
        ///     string label = "keywordSearch";
        ///     string keywordValue = "History";
        ///     string expected = $"?keywordSearch={{\"keywordSearch\":\"{keywordValue}\"}}";
        ///     var result = new NamedQueryFilter( label ).WithNamedQuery( label, keywordValue ).BuildNamedQuery();
        ///         
        ///     </b></code>
        /// </summary>
        /// <param name="namedQuery">
        ///     <see cref="NamedQueryFilter"/> Represents an object whose members can be dynamically added and removed at run time.
        /// </param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value which is an <see cref="NamedQueryFilter"/>.</param>
        /// <returns><see cref="NamedQueryFilter"/> represents an object whose members can be dynamically added and removed at run time.</returns>
        public static NamedQueryFilter WithNamedQuery( this NamedQueryFilter namedQuery, string key, object value )
        {
            namedQuery.NamedQueryExpandoObject.TryAdd( key, value );
            return namedQuery;
        }

        /// <summary>
        ///     Example:
        ///     <code><b>
        ///     
        ///         //Creates ?instructor={"instructor":{"id":"11111111-1111-1111-1111-111111111111"}}
        ///         string label = "instructor";
        ///         string guid = "11111111-1111-1111-1111-111111111111";
        ///         string expected = $"?instructor={{\"instructor\":{{\"id\":\"{guid}\"}}}}";
        ///         var result = new NamedQueryFilter( label ).WithNamedQuery( label, "id", guid ).BuildNamedQuery();
        ///         
        ///     </b></code>
        /// </summary>
        /// <param name="namedQuery"><see cref="ExpandoObject"/> Represents an object whose members can be dynamically added and removed at run time.</param>
        /// <param name="queryLabel">The root label.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value which is an <see cref="NamedQueryFilter"/>.</param>
        /// <returns><see cref="NamedQueryFilter"/> Represents an object whose members can be dynamically added and removed at run time.</returns>
        public static NamedQueryFilter WithNamedQuery( this NamedQueryFilter namedQuery, string queryLabel, string key, object value )
        {
            ExpandoObject eoTpl = BuildExpando( key, value );
            return namedQuery.WithNamedQuery( queryLabel, eoTpl );
        }

        /// <summary>
        ///     Example:
        ///     <code><b>
        ///     
        ///         //Creates ?accountSpecification={"accountingString":"11-01-01-00-11111-11111","amount":"2000","balanceOn":"2018-04-01","submittedBy":"11111111-1111-1111-1111-111111111112"}
        ///         string label = "accountSpecification";
        ///         string accountingString = "11-01-01-00-11111-11111";
        ///         string amount = "2000";
        ///         string balanceOn = "2018-04-01";
        ///         string submittedBy = "11111111-1111-1111-1111-111111111112";
        ///         string expected = $"?{label}={{\"accountingString\":\"{accountingString}\",\"amount\":\"{amount}\",\"balanceOn\":\"{balanceOn}\",\"submittedBy\":\"{submittedBy}\"}}";
        ///                 var result = new NamedQueryFilter( "accountSpecification" )
        ///                              .WithNamedQuery( ("accountingString", accountingString), ("amount", amount), ("balanceOn", balanceOn), ("submittedBy", submittedBy) )
        ///                              .BuildNamedQuery();
        ///         
        ///     </b></code>
        /// </summary>
        /// <param name="namedQuery"><see cref="NamedQueryFilter"/> Represents an object whose members can be dynamically added and removed at run time.</param>
        /// <param name="values">Dictionary&lt;string, object&gt; names = new Dictionary&lt;string, object&gt;</param>
        /// <returns><see cref="NamedQueryFilter"/> Represents an object whose members can be dynamically added and removed at run time.</returns>
        public static NamedQueryFilter WithNamedQuery( this NamedQueryFilter namedQuery, params (string key, object value) [] values )
        {
            foreach ( var (key, value) in values )
            {
                namedQuery.NamedQueryExpandoObject.TryAdd( key, value );
            }
            return namedQuery;
        }

        /// <summary>
        ///     Example:
        ///         <code><b>
        ///             //Creates section-registrations?registrationStatusesByAcademicPeriod={"statuses":[{"detail":{"id":"11111111-1111-1111-1111-111111111111"}},{"detail":{"id":"11111111-1111-1111-1111-111111111112"}}],"academicPeriod":{"id":"11111111-1111-1111-1111-111111111113"}}
        ///             var filter = new NamedQueryFilter( "registrationStatusesByAcademicPeriod" )
        ///                             .WithNamedQuery( "statuses", ("detail", "id", "11111111-1111-1111-1111-111111111111"), ("detail", "id", "11111111-1111-1111-1111-111111111112") )
        ///                             .WithNamedQuery( "academicPeriod", "id", "11111111-1111-1111-1111-111111111113" )
        ///                             .BuildNamedQuery();
        ///         </b></code>
        /// </summary>
        /// <param name="namedQuery">Instance of NamedQueryFilter object.</param>
        /// <param name="queryLabel">Named query label.</param>
        /// <param name="keyValues">key and value with specific label.</param>
        /// <returns><see cref="NamedQueryFilter"/> Represents an object whose members can be dynamically added and removed at run time.</returns>
        public static NamedQueryFilter WithNamedQuery( this NamedQueryFilter namedQuery, string queryLabel, params (string label, string key, object value) [] keyValues )
        {
            List<ExpandoObject> eoFinal = new List<ExpandoObject>();
            foreach ( var (label, key, value) in keyValues )
            {
                ExpandoObject eo = BuildExpando( key, value );
                ExpandoObject eoTpl = BuildExpando( label, eo );
                eoFinal.Add( eoTpl );
            }
            return namedQuery.WithNamedQuery( queryLabel, eoFinal );
        }

        /// <summary>
        /// Builds Expandobject. 
        /// </summary>
        /// <param name="key">key for the dictionary.</param>
        /// <param name="value">value for the dictionary.</param>
        /// <returns><see cref="ExpandoObject"/>.</returns>
        private static ExpandoObject BuildExpando( string key, object value )
        {
            ExpandoObject eo = new ExpandoObject();
            eo.TryAdd( key, value );
            return eo;
        }

        #endregion
    }
}
