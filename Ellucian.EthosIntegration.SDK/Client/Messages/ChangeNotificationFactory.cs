/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace Ellucian.Ethos.Integration.Client.Messages
{
    /// <summary>
    /// A Factory class to help with building ChangeNotification objects.
    /// </summary>
    public class ChangeNotificationFactory
    {
        /// <summary>
        ///  Create a ChangeNotification object using a JSON string. This will attempt to parse the given string into a 
        ///  ChangeNotification object.
        /// </summary>
        /// <param name="json">The JSON string representing a change-notification.</param>
        /// <returns>A ChangeNotification object created from the given JSON string.</returns>
        /// <exception cref="JsonReaderException"/>
        /// <exception cref="ArgumentException"/>
        public static ChangeNotification CreateCNFromJson(string json)
        {
            JObject parsedCN = JObject.Parse( json );
            var cnObject = parsedCN.ToObject<ChangeNotification>();
            return cnObject;
        }

        /// <summary>
        /// Create a ChangeNotification array using a JSON string. This will attempt to parse the given string into a
        /// ChangeNotification array.
        /// </summary>
        /// <param name="json">The JSON string representing a change-notification.</param>
        /// <returns>A ChangeNotification array created from the given JSON string.</returns>       
        /// <exception cref="ArgumentException"/>
        public static IEnumerable<ChangeNotification> CreateCNListFromJson( string json )
        {
            JArray parsedCN = JArray.Parse( json );
            var cnArray = parsedCN.ToObject<IEnumerable<ChangeNotification>>();
            return cnArray;
        }
    }
}
