/*
 * ******************************************************************************
 *   Copyright 2021 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ellucian.Ethos.Integration.Client.Errors
{

    /// <summary>
    /// A factory class to help with building Error objects.
    /// </summary>
    public class ErrorFactory
	{

		/// <summary>
		/// Create an Error object using a JSON string. This will attempt to parse the given string into an Error object.
		/// </summary>
		/// <param name="json">The JSON string representing the error.</param>
		/// <returns>An Error object created from the given JSON string.</returns>
		public static EthosError CreateErrorFromJson( string json )
		{
			if ( string.IsNullOrWhiteSpace( json ) ) return null;

			return JsonConvert.DeserializeObject<EthosError>( json );
		}

		/// <summary>
		/// Create an Error array using a JSON string. This will attempt to parse the given string into an Error array.
		/// </summary>
		/// <param name="json">The JSON string representing the error.</param>
		/// <returns>A List of Errors array created from the given JSON string.</returns>
		public static List<EthosError> CreateErrorListFromJson( string json )
		{
			List<EthosError> errors = new List<EthosError>();
			if (!string.IsNullOrWhiteSpace(json))
			{
				EthosError[] ethosErrors = JsonConvert.DeserializeObject<EthosError[]>(json);
				errors.AddRange(ethosErrors);
			}
			return errors;
		}
	}
}