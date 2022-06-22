/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Ellucian.Ethos.Integration.Client.Proxy.Filter
{
    /// <summary>
    /// This is a utility class used by Criteria and NamedQuery classes.
    /// </summary>
    public static class FilterUtility
    {
        #region Helper Methods

        /// <summary>
        /// Validates the inputs.
        /// </summary>
        /// <param name="expandoObject">ExpandoObject to be checked for nulls.</param>
        public static void Validate( ExpandoObject expandoObject )
        {
            if ( expandoObject != null && expandoObject.Any() )
            {
                if ( IsNull( expandoObject ) )
                {
                    throw new ArgumentNullException( $"{nameof( expandoObject )} cannot be empty or null." );
                }
            }
        }

        /// <summary>
        /// Checks for null values.
        /// </summary>
        /// <param name="expando"></param>
        /// <returns></returns>
        public static bool IsNull( IDictionary<string, object> expando )
        {
            foreach ( var item in expando )
            {
                if ( string.IsNullOrWhiteSpace( item.Key ) )
                {
                    return true;
                }
                if ( item.Value is Dictionary<string, object> )
                {
                    var result = Build( item.Value );
                    if ( result.Equals( "{}" ) )
                    {
                        return true;
                    }

                    return IsNull( ( Dictionary<string, object> ) item.Value );
                }

                if ( string.IsNullOrWhiteSpace( item.Value.ToString() ) )
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates an <see cref="ExpandoObject"/> with key and its value.
        /// </summary>
        /// <param name="key">key for the json object.</param>
        /// <param name="value">value for the json object.</param>
        /// <returns>An <see cref="ExpandoObject"/> with key and its value.</returns>
        public static ExpandoObject Add( string key, object value )
        {
            var eo = new ExpandoObject();
            _ = eo.TryAdd( key, value );
            return eo;
        }

        /// <summary>
        /// This method builds and return criteria as a json fragment.
        /// </summary>
        /// <param name="source">The expando object with various key value pairs.</param>
        /// <returns>Returns formatted criteria filter json fragment used in the url to make a proxy request.</returns>
        public static string BuildCriteria( this CriteriaFilter source )
        {
            Validate( source.CriteriaExpandoObject );
            return $"?criteria={Build( source.CriteriaExpandoObject )}";
        }

        /// <summary>
        /// This method builds and returns a named query as a json fragment.
        /// </summary>
        /// <param name="source"><see cref="NamedQueryFilter"/> Represents an object whose members can be dynamically added and removed at run time.</param>
        /// <returns>Returns formatted named query json fragment used in the url to make a proxy request.</returns>
        public static string BuildNamedQuery( this NamedQueryFilter source )
        {
            Validate( source.NamedQueryExpandoObject );
            return $"?{source.NamedQueryLabel}={Build( source.NamedQueryExpandoObject )}";
        }

        /// <summary>
        /// This method serializes the object of type T and returns the json string.
        /// </summary>
        /// <typeparam name="T">Object of type T to be serialized.</typeparam>
        /// <param name="t">The instance of type T to be serialized.</param>
        /// <returns>Serialized the json string.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="t"/> is null.</exception>
        public static string Build<T>( T t ) where T : class
        {
            if ( t == null )
            {
                throw new ArgumentNullException( $"Parameter {nameof( t )} cannot be null." );
            }
            return JsonConvert.SerializeObject( t );
        }

        #endregion

    }
}