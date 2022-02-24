/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System.Collections.Generic;
using System.Linq;

namespace Ellucian.Ethos.Integration.Client.Proxy.Filter
{
    /// <summary>
    /// Plain filter map used to support filtering for resource versions 6 and prior.
    /// Contains a Dictionary of key/value pairs for each key/value in the URL filter.
    ///     <code><b>
    ///         Examples:
    ///         
    ///         //Creates: "persons?firstName=John&amp;lastName=Smith"
    ///         string resource = "persons";
    ///         string version = "application/vnd.hedtech.integration.v6+json";
    ///         string filterKey = "firstName";
    ///         string filterValue = "John";
    ///         
    ///         FilterMap filterMap = new FilterMap()
    ///         .WithParameterPair( filterKey, filterValue )
    ///         .WithParameterPair( "lastName", "Smith" )
    ///         .Build();
    ///         
    ///         //Another example
    ///         //Creates: "persons?firstName=John"
    ///         string resource = "persons";
    ///         string version = "application/vnd.hedtech.integration.v6+json";
    ///         string filterMapKey = "firstName";
    ///         string filterMapValue = "John";
    ///         
    ///         FilterMap filterMap = new FilterMap()
    ///         .WithParameterPair( filterMapKey, filterMapValue )
    ///         .Build();
    ///     </b></code>
    /// </summary>
    public class FilterMap
    {
        /// <summary>
        /// Map of string key/value pairs containing filter parameters.
        /// </summary>
        private readonly Dictionary<string, string> filterPairMap;

        /// <summary>
        /// Instantiate FilterMap.
        /// </summary>
        public FilterMap()
        {
            filterPairMap = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds a key/value pair as a filter parameter to the filterMap.
        /// </summary>
        /// <param name="filterKey">The key for the filter parameter.</param>
        /// <param name="filterValue">The value of the filter parameter.</param>
        /// <returns>This FilterMap for fluency in supporting multiple filter parameters.</returns>
        public FilterMap WithParameterPair( string filterKey, string filterValue )
        {
            filterPairMap.Add( filterKey, filterValue );
            return this;
        }

        /// <summary>
        /// Builds a FilterMap instance with the parameter pairs previously provided.
        /// </summary>
        /// <returns>Returns this instance with filterPairMap.</returns>
        public FilterMap Build()
        {
            return this;
        }

        /// <summary>
        /// Gets a list of filter pair keys from the filterPairMap.
        /// </summary>
        /// <returns>A IEnumerable&lt;string&gt; of filter keys.</returns>
        public IEnumerable<string> GetFilterMapKeys()
        {
            return this.filterPairMap.Select( k => k.Key ).ToList();
        }

        /// <summary>
        /// Gets a filter map value using the given key.
        /// </summary>
        /// <param name="key">The filter map key used to retrieve a filter map value.</param>
        /// <returns>The value associated with the given key in the filterPairMap.</returns>
        public string GetFilterMapValue( string key )
        {
            if ( this.filterPairMap.TryGetValue( key, out string value ) )
            {
                return value;
            }
            return null;
        }


        /// <summary>
        /// Provides a string representation of this CriteriaFilter. This method should be called to produce the entire
        /// CriteriaFilter string used when making a request containing a criteria filter.
        /// </summary>
        /// <returns>A JSON formatted string containing the proper syntax of a CriteriaFilter.</returns>
        public override string ToString()
        {
            string kvp = string.Empty;
            var keyList = this.GetFilterMapKeys();

            kvp = string.Join( "&", keyList.Select( i => $"{ i }={ GetFilterMapValue( i ) }" ) );
            return $"?{ kvp }";
        }
    }
}
