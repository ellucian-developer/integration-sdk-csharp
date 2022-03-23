/*
 * ******************************************************************************
 *   Copyright  2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;

using System;

namespace Ellucian.Ethos.Integration.Client.Extensions
{
    /// <summary>
    /// Extends <see cref="EthosResponse"/>.
    /// </summary>
    public static class EthosResponseExtension
    {
        /// <summary>
        /// Converts ethos response content to an object of type T
        /// </summary>
        /// <typeparam name="T">Type of object to be deserialized.</typeparam>
        /// <param name="ethosResponse">Ethos response.</param>
        /// <returns>Returns deserialized object.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="ethosResponse"/> is passed as null or ethosResponse json content is empty or white space.</exception>
        public static T Deserialize<T>( this EthosResponse ethosResponse )
        {
            if ( ethosResponse == null || ( ethosResponse != null && string.IsNullOrWhiteSpace( ethosResponse.Content ) ) )
            {
                throw new ArgumentNullException( $"Parameter {nameof( ethosResponse )} cannot be null." );
            }
            var obj = JsonConvert.DeserializeObject<T>( ethosResponse.Content );
            return obj;
        }

        /// <summary>
        /// Converts ethos response content to an object of type T
        /// </summary>
        /// <typeparam name="T">Type of object to be serialized.</typeparam>
        /// <param name="ethosResponse">Ethos response containing json content.</param>
        /// <param name="formatting">Json formatting, values are none where json is not formatted and Indented where json is properly indented.</param>
        /// <returns>Returns serialized string.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="ethosResponse"/> is passed as null or ethosResponse json content is empty or white space.</exception>
        public static string Serialize<T>( this EthosResponse ethosResponse, Formatting formatting = Formatting.None )
        {
            if ( ethosResponse == null || ( ethosResponse != null && ethosResponse.Dto == null ) )
            {
                throw new ArgumentNullException( $"Parameter {nameof( ethosResponse.Dto )} cannot be null." );
            }
            ethosResponse.Dto = JsonConvert.SerializeObject( ethosResponse.Dto, formatting );
            return JsonConvert.SerializeObject( ethosResponse.Dto, formatting );
        }
    }
}
