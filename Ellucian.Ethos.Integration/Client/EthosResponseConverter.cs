/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Messages;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

using Ellucian.Ethos.Integration.Client.Errors;

namespace Ellucian.Ethos.Integration.Client
{
    /// <summary>
    /// Converter class extending the <see cref="EthosResponseBuilder"/>, the primary purpose of which is to handle manipulation 
    /// of the response body content for paging calculations, such as trimming the response body content for a given offset or
    /// number of rows.Also converts an <see cref="EthosResponse"/> to <see cref="string"/> format.
    /// </summary>
    public class EthosResponseConverter : EthosResponseBuilder
    {
        /// <summary>
        /// Takes the given <see cref="EthosResponse"/> and trims it's content from the given offset. For example,
        /// if the given content contains 10 rows, and the offset is 3, the returned <see cref="EthosResponse"/> content will
        /// contain rows 3 through the end of the <paramref name="sourceResponse"/> content.
        /// The returned <see cref="EthosResponse"/> will also contain the headers and Http status code from the given <paramref name="sourceResponse"/>.
        /// </summary>
        /// <param name="sourceResponse">The <see cref="EthosResponse"/> to trim content for.</param>
        /// <param name="offset">The offset (row num) from which to begin trimming content.</param>
        /// <returns>An <see cref="EthosResponse"/> containing the same content as the <paramref name="sourceResponse"/>, minus the rows 
        /// in the<paramref name="sourceResponse"/> which occurred before the given offset. The returned <see cref="EthosResponse"/>
        /// will also contain the headers and Http status code from the given<paramref name="sourceResponse"/>. If the <paramref name="sourceResponse"/>
        /// is null, if the<paramref name="sourceResponse"/> content is null or empty, or if the offset is negative, the original
        /// <paramref name="sourceResponse"/> will be returned unchanged.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <see cref="Newtonsoft.Json.Linq.JToken"/>JTokens is null.</exception>
        public EthosResponse TrimContentFromOffset( EthosResponse sourceResponse, int offset )
        {
            if ( !ParamsValid( sourceResponse, offset ) )
            {
                return sourceResponse;
            }
            HttpResponseHeaders headersMap = CopyHeaders( sourceResponse );
            string content = sourceResponse.Content;
            JArray jArray = JArray.Parse( content );
            var jTokens = jArray.Skip( offset );
            StringBuilder contentBuilder = new StringBuilder();
            _ = contentBuilder.Append( JsonConvert.SerializeObject( jTokens ) );
            content = contentBuilder.ToString();
            EthosResponse targetResponse = new EthosResponse( headersMap, content, sourceResponse.HttpStatusCode );
            return targetResponse;
        }

        /// <summary>
        /// Takes the given <see cref="EthosResponse"/> and trims it's content for the given number of rows.  
        /// For example, if the given content contains 10 rows, and numRows is 7, the returned <see cref="EthosResponse"/> content will 
        /// contain rows 0 through 6 (for a total of 7) of the <paramref name="sourceResponse"/> content. The returned <see cref="EthosResponse"/> 
        /// will also contain the headers and Http status code from the given <paramref name="sourceResponse"/>.
        /// </summary>
        /// <param name="sourceResponse">The <see cref="EthosResponse"/> to trim content for.</param>
        /// <param name="numRows">The number of rows to trim the content for.</param>
        /// <returns>An <see cref="EthosResponse"/> containing the same content as the <paramref name="sourceResponse"/>, minus the rows
        /// in the<paramref name="sourceResponse"/> which occur after numRows - 1. The returned <see cref="EthosResponse"/>
        /// will also contain the headers and Http status code from the given<paramref name="sourceResponse"/>. If the <paramref name="sourceResponse"/>
        /// is null, if the<paramref name="sourceResponse"/> content is null or empty, or if the numRows is negative, the original
        /// <paramref name="sourceResponse"/> will be returned unchanged.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <see cref="Newtonsoft.Json.Linq.JToken"/>JTokens is null.</exception>
        public EthosResponse TrimContentForNumRows( EthosResponse sourceResponse, int numRows )
        {
            if ( ParamsValid( sourceResponse, numRows ) == false )
            {
                return sourceResponse;
            }
            HttpResponseHeaders headersMap = CopyHeaders( sourceResponse );
            string content = sourceResponse.Content;
            JArray jArray = JArray.Parse( content );
            var jTokens = jArray.Take( numRows );
            StringBuilder contentBuilder = new StringBuilder();

            _ = contentBuilder.Append( JsonConvert.SerializeObject( jTokens ) );
            content = contentBuilder.ToString();
            EthosResponse targetResponse = new EthosResponse( headersMap, content, sourceResponse.HttpStatusCode );
            return targetResponse;
        }

        /// <summary>
        /// Takes the given <see cref="EthosResponse"/> and trims it's content from the given offset for the given number of rows. For example,
        /// if the given content contains 10 rows, the offset is 3, and numRows is 7, the returned<see cref="EthosResponse"/> content will
        /// contain rows 3 through 6 (for a total of 4) of the<paramref name="sourceResponse"/> content.
        /// The returned <see cref="EthosResponse"/> will also contain the headers and Http status code from the given <paramref name="sourceResponse"/>.
        /// </summary>
        /// <param name="sourceResponse">The <see cref="EthosResponse"/> to trim content for.</param>
        /// <param name="offset">The offset (row num) from which to begin trimming content.</param>
        /// <param name="numRows">The number of rows to trim the content for.</param>
        /// <returns>An <see cref="EthosResponse"/> containing the same content as the <paramref name="sourceResponse"/>, minus the rows
        /// in the <paramref name="sourceResponse"/> which occur before the offset and after numRows - 1. The returned <see cref="EthosResponse"/>
        /// will also contain the headers and Http status code from the given <paramref name="sourceResponse"/>. If the <paramref name="sourceResponse"/>
        /// is null, if the <paramref name="sourceResponse"/> content is null or empty, or if the offset is negative, the original
        /// <paramref name="sourceResponse"/> will be returned unchanged. If the offset is positive but the numRows is negative, then the
        /// <see cref="EthosResponse"/> will contain the same content as the <paramref name="sourceResponse"/>, minus the rows in the
        /// <paramref name="sourceResponse"/> which occur before the offset, just as what <see cref="TrimContentFromOffset"/> returns.</returns>
        public EthosResponse TrimContentFromOffsetForNumRows( EthosResponse sourceResponse, int offset, int numRows )
        {
            EthosResponse response = TrimContentFromOffset( sourceResponse, offset );
            if ( response == sourceResponse )
            {
                return sourceResponse;
            }
            return TrimContentForNumRows( response, numRows );
        }

        /// <summary>
        /// Returns the content body of the given <see cref="EthosResponse"/>, or null if the <see cref="EthosResponse"/> is null.
        /// </summary>
        /// <param name="ethosResponse">The <see cref="EthosResponse"/> to get the content body from.</param>
        /// <returns>The content body of the given <see cref="EthosResponse"/>.</returns>
        public string ToContentString( EthosResponse ethosResponse )
        {
            if ( ethosResponse == null )
            {
                return null;
            }
            return ethosResponse.Content;
        }

        /// <summary>
        /// Returns a <see cref="JArray"/> representation of the content body of the given <see cref="EthosResponse"/>.
        /// </summary>
        /// <param name="ethosResponse">The given <see cref="EthosResponse"/> to convert to a <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> containing the content body of the given <see cref="EthosResponse"/></returns>
        public JArray ToJArray( EthosResponse ethosResponse )
        {
            if ( ethosResponse == null || string.IsNullOrWhiteSpace( ethosResponse.Content ) )
            {
                return null;
            }
            return JsonConvert.DeserializeObject<JArray>( ethosResponse.Content );
        }

        /// <summary>
        /// Converts the given list of <see cref="EthosResponse"/> objects into a <see cref="JArray"/>, where
        /// each item in the array is a single resource.  This combines all the pages of data from each response object 
        /// into a top-level array of resources.
        /// It is expected that the EthosResponse objects have a content in a JSON array format.  If the response is not a JSON array, an 
        /// exception will be thrown while trying to deserialize it.
        /// </summary>
        /// <param name="ethosResponseList">The list of <see cref="EthosResponse"/> objects to convert to a <see cref="JArray"/>.</param>
        /// <returns>a <see cref="JArray"/> where each item in the array is a single resource, or an empty list
        /// if the given ethosResponseList is null.</returns>
        public JArray ToJArray( IEnumerable<EthosResponse> ethosResponseList )
        {
            JArray responseArray = new JArray();

            if ( ethosResponseList == null )
            {
                return responseArray;
            }

            // loop through the responses, deserialize the page of resources into a JSON array, and add those items to the top-level array
            foreach ( EthosResponse ethosResponse in ethosResponseList )
            {
                JArray responsePage = JsonConvert.DeserializeObject<JArray>( ethosResponse.Content );
                responseArray.Add( responsePage.Children() );
            }
            return responseArray;
        }

        /// <summary>
        /// Converts the given list of <see cref="EthosResponse"/> objects into a list of <see cref="JArray"/>, where
        /// each <see cref="JArray"/> in the list contains the content body of the given<see cref="EthosResponse"/> in the
        /// ethosResponseList.
        /// The expectation is that each EthosResponse in the given list contains a page 
        /// of resources, and therefore each JArray in the returned list contains that same page of resources as a JArray.
        /// </summary>
        /// <param name="ethosResponseList">The list of <see cref="EthosResponse"/> objects to convert to a list of <see cref="JArray"/></param>
        /// <returns>A list of <see cref="JArray"/> where each <see cref="JArray"/> in the list contains a page of resources from the 
        /// corresponding <see cref="EthosResponse"/> object in the ethosResponseList, or an empty list
        /// if the given ethosResponseList is null.</returns>
        public IEnumerable<JArray> ToJArrayList( IEnumerable<EthosResponse> ethosResponseList )
        {
            List<JArray> jArrayList = new List<JArray>();
            if ( ethosResponseList == null )
            {
                return jArrayList;
            }
            foreach ( var ethosResponse in ethosResponseList )
            {
                JArray jArray = JsonConvert.DeserializeObject<JArray>( ethosResponse.Content );
                jArrayList.Add( jArray );
            }
            return jArrayList;
        }

        /// <summary>
        /// Returns a <see cref="JObject"/> representation of the content body of the given <see cref="EthosResponse"/>.
        /// </summary>
        /// <param name="ethosResponse">The given <see cref="EthosResponse"/> to convert to a <see cref="JObject"/>.</param>
        /// <returns>A <see cref="JObject"/> containing the content body of the given <see cref="EthosResponse"/></returns>
        public JObject ToJObjectSingle( EthosResponse ethosResponse )
        {
            if ( ethosResponse == null || string.IsNullOrWhiteSpace( ethosResponse.Content ) )
            {
                return null;
            }
            return JsonConvert.DeserializeObject<JObject>( ethosResponse.Content );
        }

        /// <summary>
        /// Converts the given list of <see cref="EthosResponse"/> objects into a list of <see cref="string"/>, where
        /// each <see cref="string"/> in the list is the content body of the given <see cref="EthosResponse"/> in the
        /// ethosResponseList.
        /// The expectation is that each EthosResponse in the given list contains a page 
        /// of resources, and therefore each String in the returned list contains that same page of resources as a String.
        /// </summary>
        /// <param name="ethosResponseList">The list of <see cref="EthosResponse"/> objects to convert to a list of <see cref="string"/>(s).</param>
        /// <returns>a list of <see cref="string"/> where each <see cref="string"/> in the list contains a page of resources from the
        /// corresponding <see cref="EthosResponse"/> object in the ethosResponseList, or an empty list
        /// if the given ethosResponseList is null.</returns>
        public IEnumerable<string> ToPagedStringList( IEnumerable<EthosResponse> ethosResponseList )
        {
            List<string> stringList = new List<string>();
            if ( ethosResponseList == null )
            {
                return stringList;
            }
            foreach ( EthosResponse ethosResponse in ethosResponseList )
            {
                stringList.Add( ethosResponse.Content );
            }
            return stringList;
        }

        /// <summary>
        /// Converts the given list of <see cref="EthosResponse"/> objects into a list of <see cref="string"/>, where
        /// each <see cref="string"/> in the list is a single resource.  This combines all the pages of data from each response object 
        /// into a top-level list of resources.
        /// It is expected that the EthosResponse objects have a content in a JSON array format.  If the response is not a JSON array, an 
        /// exception will be thrown while trying to deserialize it.
        /// </summary>
        /// <param name="ethosResponseList">The list of <see cref="EthosResponse"/> objects to convert to a list of <see cref="string"/>(s).</param>
        /// <returns>a list of <see cref="string"/> where each <see cref="string"/> in the list is a single resource, or an empty list
        /// if the given ethosResponseList is null.</returns>
        public IEnumerable<string> ToStringList( IEnumerable<EthosResponse> ethosResponseList )
        {
            List<string> stringList = new List<string>();
            if ( ethosResponseList == null )
            {
                return stringList;
            }

            // first get the deserialized JSON array, then convert to strings
            JArray jsonResponse = ToJArray( ethosResponseList );
            foreach ( JToken resource in jsonResponse.Children() )
            {
                stringList.Add( resource.ToString() );
            }
            return stringList;
        }

        /// <summary>
        /// Converts the given ethosResponse to a list of ChangeNotification object.
        /// </summary>
        /// <param name="ethosResponse">The EthosResponse to convert containing content for the ChangeNotifications.</param>
        /// <returns>A list of ChangeNotifications from the content of the given ethosResponse, 
        /// or null if the ethosResponse is null or it's content is null or blank.</returns>
        public IEnumerable<ChangeNotification> ToCNList( EthosResponse ethosResponse )
        {
            if ( ethosResponse == null && ethosResponse.Content == null )
            {
                return null;
            }
            return ChangeNotificationFactory.CreateCNListFromJson( ethosResponse.Content );
        }

        /// <summary>
        /// Validates whether the given params are valid. If the given <paramref name="sourceResponse"/> is not null, and
        /// contains content that is not null or empty, and if the positiveParam( which is intended to be either the offset
        /// or numRows ) is not less than 0 (positive), then returns true. Otherwise returns false.
        /// </summary>
        /// <param name="sourceResponse">The <see cref="EthosResponse"/> to validate the content for.</param>
        /// <param name="positiveParam">Should be either the offset or numRows as called from other methods.</param>
        /// <returns>False if the given <paramref name="sourceResponse"/> is null, or contains null or empty content, or if the
        /// positive param is less than 0, true otherwise.</returns>
        private bool ParamsValid( EthosResponse sourceResponse, int positiveParam )
        {
            if ( sourceResponse == null || string.IsNullOrWhiteSpace( sourceResponse.Content ) )
            {
                return false;
            }
            if ( positiveParam < 0 )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a copy of the headers map from the given <see cref="EthosResponse"/>. Uses the <see cref="EthosResponse.HeadersMap"/>
        /// in the <see cref="EthosResponseBuilder"/> class to copy headers into the returned headers map.
        /// </summary>
        /// <param name="sourceResponse"></param>
        /// <returns></returns>
        private HttpResponseHeaders CopyHeaders( EthosResponse sourceResponse )
        {
            return sourceResponse.HeadersMap;
        }

        /// <summary>
        /// Converts an EthosResponse to an EthosError object. The given ethosResponse.Content should contain
        /// data for only a single error, and not an entire page of errors.
        /// </summary>
        /// <param name="ethosResponse">
        /// An EthosError from the content of the given ethosResponse, or null if the ethosResponse is
        /// null or its content is null or blank.
        /// </param>
        /// <returns></returns>
        public EthosError ToSingleEthosError( EthosResponse ethosResponse )
        {
            EthosError ethosError = null;
            if ( ( ethosResponse == null ) ||
                ( ethosResponse.Content == null ) ||
                ( string.IsNullOrEmpty( ethosResponse.Content ) ) )
            {
                return ethosError;
            }
            return ErrorFactory.CreateErrorFromJson( ethosResponse.Content );
        }

        /// <summary>
        /// Converts an ethosResponse to a list of EthosError objects. The given ethosResponse.getContent() should contain
        /// an entire page of errors as an errors array in JSON format.
        /// </summary>
        /// <param name="ethosResponse">The EthosResponse to convert containing content for a page of EthosErrors.</param>
        /// <returns>An IEnumerable of EthosErrors from the content of the given ethosResponse, or an empty list if the ethosResponse
        /// is null or it's content is null or blank.</returns>
        public IEnumerable<EthosError> ToEthosErrorList( EthosResponse ethosResponse )
        {
            List<EthosError> ethosErrorList = new List<EthosError>();
            if ( ethosResponse == null ||
                string.IsNullOrWhiteSpace( ethosResponse.Content ) ||
                !ethosResponse.Content.Any() )
            {
                return ethosErrorList;
            }
            return ErrorFactory.CreateErrorListFromJson( ethosResponse.Content );
        }
    }
}
