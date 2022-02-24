/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

namespace Ellucian.Ethos.Integration.Notification
{

    /// <summary>
    /// Abstract class for change notification.
    /// </summary>
    public abstract class AbstractEthosNotificationPollService
    {
        /// <summary>
        /// The number of messages to retrieve at once. A list of ChangeNotifications, overriding any notifications with the desired version of the resource if
        /// notifications in the returned list match those added to this class (by resource name) to be overridden.
        /// </summary>
        public int? Limit { get; internal set; }
    }
}