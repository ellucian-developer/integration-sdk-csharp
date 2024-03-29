﻿/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy.Filter;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Ellucian.Ethos.Integration.Client.Filter.Extensions
{
    /// <summary>
    /// This class adds extension methods to <see cref="CriteriaFilter"/> class.
    /// </summary>
    public static class CriteriaExtension
    {
        #region WithSimpleCriteria

        /// <summary>
        ///     Example:     
        ///     <code><b>
        ///     
        ///         //Creates ?criteria={"lastName":"Smith"}
        ///         var criteria = new Criteria().WithSimpleCriteria( "lastName", "Smith" );
        ///         
        ///     </b></code>                     
        /// </summary>
        /// <param name="criteria"><see cref="CriteriaFilter"/>.</param>
        /// <param name="key">key for the json object.</param>
        /// <param name="value">value for the json object.</param>
        /// <returns><see cref="CriteriaFilter"/> represents an object whose members can be dynamically added and removed at run time.</returns>
        public static CriteriaFilter WithSimpleCriteria( this CriteriaFilter criteria, string key, object value )
        {
            if ( value is CriteriaFilter crt )
            {
                criteria.CriteriaExpandoObject.TryAdd( key, crt.CriteriaExpandoObject );
            }
            else
            {
                criteria.CriteriaExpandoObject.TryAdd( key, value );
            }
            return criteria;
        }

        /// <summary>
        ///     Example:
        ///     <code><b>
        ///         
        ///         //Creates ?criteria={"names":{"lastName":"Smith"}}
        ///         var criteria = new Criteria().WithSimpleCriteria( "names", ("lastName", "Smith") );
        ///         
        ///     </b></code>
        /// </summary>
        /// <param name="criteria"><see cref="CriteriaFilter"/>.</param>
        /// <param name="label">The label for the json object.</param>
        /// <param name="tuple"><see cref="Tuple{T1, T2}"/>A tuple object with key and the value.</param>
        /// <returns><see cref="CriteriaFilter"/> Represents an object whose members can be dynamically added and removed at run time.</returns>
        public static CriteriaFilter WithSimpleCriteria( this CriteriaFilter criteria, string label, (string key, object value) tuple )
        {
            ExpandoObject eo = new ExpandoObject();
            if ( tuple.value is CriteriaFilter crt )
            {
                _ = eo.TryAdd( tuple.key, crt.CriteriaExpandoObject );
            }
            else
            {
                _ = eo.TryAdd( tuple.key, tuple.value );
            }
            return criteria.WithSimpleCriteria( label, eo );
        }

        /// <summary>
        ///     Example:
        ///     <code><b>
        ///         
        ///         //Creates ?criteria={"names":{"firstName":"John","lastName":"Smith"}}
        ///         filter = new Criteria().WithSimpleCriteria( "names", ("firstName", "John"), ("lastName", "Smith") );
        ///                                       
        ///     </b></code>
        /// </summary>
        /// <param name="criteria"><see cref="CriteriaFilter"/> Represents an object whose members can be dynamically added and removed at run time.</param>
        /// <param name="label">The label for the json object.</param>
        /// <param name="tuples">Array of <see cref="Tuple{T1, T2}"/> object with key and the value.</param>
        /// <returns><see cref="CriteriaFilter"/> represents an object whose members can be dynamically added and removed at run time.</returns>
        public static CriteriaFilter WithSimpleCriteria( this CriteriaFilter criteria, string label, params (string key, object value) [] tuples )
        {
            ExpandoObject eo = new ExpandoObject();
            foreach ( var tuple in tuples )
            {
                if ( tuple.value is CriteriaFilter crt )
                {
                    _ = eo.TryAdd( tuple.key, crt.CriteriaExpandoObject );
                }
                else
                {
                    _ = eo.TryAdd( tuple.key, tuple.value );
                }
            }
            return criteria.WithSimpleCriteria( label, eo );
        }

        /// <summary>
        ///     Example:
        ///     <code><b>
        ///     
        ///         //Creates ?criteria={"solicitors":[{"solicitor":{"constituent":{"person":{"id":"11111111-1111-1111-1111-111111111111"}}}},{"solicitor":{"constituent":{"person":{"id":"11111111-1111-1111-1111-111111111111"}}}}]}
        ///         var solicitors = new Criteria().WithSimpleCriteria( "constituent", new Criteria().WithSimpleCriteria( "person", ("id", "11111111-1111-1111-1111-111111111111") ) ).WithSimpleCriteria( "solicitor" );
        ///         var filter = new Criteria().WithArray( "solicitors", solicitors, solicitors ).BuildCriteria();
        ///                                       
        ///     </b></code>
        /// </summary>
        /// <param name="criteria"><see cref="CriteriaFilter"/> Represents an object whose members can be dynamically added and removed at run time.</param>
        /// <param name="label">The label for the json object.</param>
        /// <returns><see cref="CriteriaFilter"/> represents an object whose members can be dynamically added and removed at run time.</returns>
        public static CriteriaFilter WithSimpleCriteria( this CriteriaFilter criteria, string label )
        {
            ExpandoObject eo = new ExpandoObject();
            _ = eo.TryAdd( label, criteria.CriteriaExpandoObject );
            return eo;
        }

        #endregion

        #region WithArray

        /// <summary>
        ///     Examples:
        ///     <code><b>
        ///     
        ///         //Creates ?criteria={"credentials":[{"type":"bannerSourcedId","value":"684"}]}
        ///         string criteriaFilterStr = new CriteriaFilter().WithArray( "credentials", new CriteriaFilter()
        ///                                                        .WithSimpleCriteria( "type", "bannerSourcedId" )
        ///                                                        .WithSimpleCriteria( "value", "684" ) ).BuildCriteria();
        ///                                       
        ///         
        ///     </b></code>
        ///     <code><b>
        ///     
        ///         //Creates vendors?criteria={"statuses":["active","approved"]}
        ///         var result = new Criteria().WithArray( "statuses", new [] { "active", "approved" } ).BuildCriteria();
        ///         
        ///     </b></code>
        /// </summary>
        /// <param name="criteria"><see cref="CriteriaFilter"/>.</param>
        /// <param name="label">Root label.</param>
        /// <param name="values">params <see cref="object"/>[].</param>
        /// <returns><see cref="CriteriaFilter"/> represents an object whose members can be dynamically added and removed at run time.</returns>
        public static CriteriaFilter WithArray( this CriteriaFilter criteria, string label, params object [] values )
        {
            List<object> criterias = new List<object>();
            values.ToList().ForEach( i =>
            {
                if ( i is CriteriaFilter )
                {
                    criterias.Add( ( ( CriteriaFilter ) i ).CriteriaExpandoObject );
                }
                else
                {
                    criterias.Add( i );
                }
            } );
            return criteria.WithSimpleCriteria( label, criterias );
        }

        /// <summary>
        ///     Example:
        ///     <code><b>
        ///     
        ///         //Creates ?criteria={"names":[{"lastName":"Smith"},{"firstName":"John"}]}       
        ///         Dictionary&lt;string, object&gt; names = new Dictionary&lt;string, object&gt;();        
        ///         names.Add( "lastName", "smith" );
        ///         names.Add( "fistName", "Jon" );            
        /// 
        ///         var result = new Criteria().WithArray( "names", names ).BuildCriteria();
        ///         
        ///     </b></code>
        /// </summary>
        /// <param name="criteria"><see cref="CriteriaFilter"/>.</param>
        /// <param name="label">Root label.</param>
        /// <param name="values">A <see cref="Dictionary{TKey, TValue}"/>with key value pairs.</param>
        /// <returns><see cref="CriteriaFilter"/> represents an object whose members can be dynamically added and removed at run time.</returns>
        public static CriteriaFilter WithArray( this CriteriaFilter criteria, string label, Dictionary<string, object> values )
        {
            List<ExpandoObject> eoList = new List<ExpandoObject>();
            foreach ( var value in values )
            {
                var obj = FilterUtility.Add( value.Key, value.Value );
                eoList.Add( obj );
            }
            return criteria.WithSimpleCriteria( label, eoList );
        }

        /// <summary>
        ///     Example:
        ///     <code><b>
        ///     
        ///         //Creates ?criteria={"emails":[{"address":"abc@def.com"},{"address":"12345@abc.com"}]}
        ///         string resource = "persons";
        ///         string email1 = "abc@def.com";
        ///         string email2 = "12345@abc.com";
        ///         string criteriaFilterStr = new Criteria().WithArray( "emails", ("address", email1), ("address", email2) ).BuildCriteria();
        ///         
        ///     </b></code>
        /// </summary>
        /// <param name="criteria"><see cref="CriteriaFilter"/>.</param>
        /// <param name="label">Root label.</param>
        /// <param name="values"><see cref="Tuple{T1, T2}"/>[]</param>
        /// <returns><see cref="CriteriaFilter"/> represents an object whose members can be dynamically added and removed at run time.</returns>
        public static CriteriaFilter WithArray( this CriteriaFilter criteria, string label, params (string key, object value) [] values )
        {
            List<ExpandoObject> eoList = new List<ExpandoObject>();
            foreach ( var tuple in values )
            {
                ExpandoObject eo = FilterUtility.Add( tuple.key, tuple.value );
                eoList.Add( eo );
            }
            return criteria.WithSimpleCriteria( label, eoList );
        }

        #endregion

    }
}