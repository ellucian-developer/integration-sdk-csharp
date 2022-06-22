/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

namespace Ellucian.Ethos.Integration.Client.Messages
{
    /// <summary>
    /// An object representation of a change-notification. A change-notification is the resource
    /// that is published and consumed via subscriptions through the Ethos Integration messages service.
    /// </summary>
    public class ChangeNotification
    {
        /// <summary>
        /// An object representation of a change-notification.  A change-notification is the resource
        /// that is published and consumed via subscriptions through the Ethos Integration messages service.        
        /// </summary>
        /// <param name="id">The ID of the change-notification.</param>
        /// <param name="published">The date and time that the change-notification was published.</param>
        /// <param name="operation">The operation that occurred on the resource.</param>
        /// <param name="publisher">Information about the publishing application.</param>
        /// <param name="resource">The resource to which the change occurred.</param>
        /// <param name="contentType">The content-type associated with the content object.</param>
        /// <param name="content">The content of the resource that was change.</param>
        /// <remarks>The attribute JsonConstructor is used to parse json and convert in to the object.</remarks>
        [JsonConstructor]
        public ChangeNotification( string id, DateTime published, string operation, Publisher publisher, Resource resource, string contentType, JObject content )
        {
            Id = id;
            Published = published;
            Operation = operation;
            Publisher = publisher;
            Resource = resource;
            ContentType = contentType;
            Content = content;
        }

        /// <summary>
        /// The ID of the change-notification.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The date and time that the change-notification was published.
        /// </summary>
        [JsonProperty()]
        public DateTime Published { get; private set; }

        /// <summary>
        /// Information about the publishing application.
        /// </summary>
        public Publisher Publisher { get; private set; }

        /// <summary>
        /// The resource to which the change occurred.
        /// </summary>
        public Resource Resource { get; private set; }

        /// <summary>
        /// The operation that occurred on the resource.
        /// </summary>
        public string Operation { get; private set; }

        /// <summary>
        /// The content-type associated with the content object.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// The content of the resource that was change.
        /// </summary>
        public JObject Content { get; private set; }
    }
}
