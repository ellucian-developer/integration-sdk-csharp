/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Messages;
using Ellucian.Ethos.Integration.Client.Proxy;
using Ellucian.Ethos.Integration.Config;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Service
{
    /// <summary>
    /// Service class used for retrieving ChangeNotifications. Uses the <see cref="Ellucian.Ethos.Integration.Client.Messages.EthosMessagesClient"/> and
    /// <see cref="Ellucian.Ethos.Integration.Client.Proxy.EthosProxyClient"/> to do so. If a specific version of a resource is requested for change notifications,
    /// this service will retrieve the desired version of the resource and return the content of that resource (and version) in the
    /// corresponding change notification. For example, if persons v12 is requested but the change notification retrieved is for
    /// persons v8, this service will retrieve persons v12 and replace the content in the persons v8 notification with the content
    /// of persons v12.
    /// </summary>
    public class EthosChangeNotificationService : EthosService
    {
        #region ...ctor

        /// <summary>
        /// Instantiates this service class with an api key. 
        /// This constructor is only called from the inner Builder class.
        /// </summary>
        /// <param name="apiKey">A <see cref="Guid"/> api key.</param>
        private EthosChangeNotificationService(string apiKey)
            : this( new EthosClientBuilder(apiKey))
        {

        }

        /// <summary>
        /// Instantiates this service class with Colleague API and credentials.
        /// </summary>
        /// <param name="colleagueApiUrl">The URL to the Colleague API instance.</param>
        /// <param name="colleagueApiUsername">The username used to connect to the Colleague API.</param>
        /// <param name="colleagueApiPassword">The password used to connect to the Colleague API.</param>
        private EthosChangeNotificationService(string colleagueApiUrl, string colleagueApiUsername, string colleagueApiPassword)
            : this( new EthosClientBuilder(colleagueApiUrl, colleagueApiUsername, colleagueApiPassword))
        {

        }

        /// <summary>
        /// Instantiates this service class with <see cref="EthosClientBuilder"/>.
        /// This constructor is only called from the inner Builder class.
        /// </summary>
        /// <param name="ethosClientBuilder"></param>
        private EthosChangeNotificationService( EthosClientBuilder ethosClientBuilder ) : base( ethosClientBuilder )
        {
            ResourceVersionOverrideMap ??= new Dictionary<string, string>();
        }

        #endregion

        #region Fields

        /// <summary>
        /// A map containing the requested resource(s) and version for each resource.
        /// </summary>
        private Dictionary<string, string> ResourceVersionOverrideMap;

        /// <summary>
        /// <b>Used internally by the SDK.</b> Constant value used to determine if a ChangeNotification is for a deleted operation.
        /// </summary>
        private const string OPERATION_DELETED = "deleted";

        /// <summary>
        /// <b>Used internally by the SDK.</b>
        /// Default constant value used to replace the contentType in the ChangeNotification if there is no x-content-restricted header when overriding a resource version.
        /// </summary>
        private const string RESOURCE_CONTENT_TYPE = "resource-representation";

        #endregion

        #region Properties

        /// <summary>
        /// The EthosMessagesClient used for retrieving change notification messages from Ethos.
        /// </summary>
        protected EthosMessagesClient EthosMessagesClient;

        /// <summary>
        /// The EthosProxyClient used for making Ethos API calls through Ethos.
        /// </summary>
        protected EthosProxyClient EthosProxyClient;

        #endregion

        #region Methods        

        /// <summary>
        /// Enables client application code to add resource version overrides after this service has been built.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="version">The desired version of the resource to override change notification of the same resource with, e.g. application/vnd.hedtech.integration.v12.3.0+json</param>
        public void AddResourceVersionOverride( string resourceName, string version )
        {
            AddToDictionary( resourceName, version );
        }

        /// <summary>
        /// Same as <see cref="AddResourceVersionOverride"/> except the version can be abbreviated as just the version value, e.g: 16, or 16.0.0.
        /// Also accepts the version value prefixed with the char 'v', e.g: v16, or v16.0.0.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="version"></param>
        public void AddResourceAbbreviatedVersionOverride( string resourceName, string version )
        {
            if ( version.StartsWith( "v" ) )
            {
                version = version.Substring( 1 );
            }
            AddToDictionary( resourceName, version );
        }

        /// <summary>
        /// Enables client application code to remove resource version overrides after this service has been built.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        public void RemoveResourceVersionOverride( string resourceName )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "ERROR: Cannot remove a resource version override for the EthosChangeNotificationService with a null or blank resource name value." );
            }
            ResourceVersionOverrideMap.Remove( resourceName );
        }

        /// <summary>
        /// Gets a list of resources to be overridden in change notifications. These are the resources added to the resource version override
        /// capability of this class.
        /// </summary>
        /// <returns>A list of resource names configured to override change notification content for the specified version. 
        /// Will be an empty list if no overrides have been specified.</returns>
        public IEnumerable<string> GetOverriddenResources()
        {
            return ResourceVersionOverrideMap.Keys.ToList();
        }

        /// <summary>
        /// Gets the overridden version for the given resource name specified when adding to the resource version override.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get the version override for.</param>
        /// <returns>The version overriding the version of any change notification with the given resource name, or null if the resourceName
        /// is null or blank, or if there is no version mapping for the given resourceName.</returns>
        public string GetOverriddenResourcesVersion( string resourceName )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) ) return null;

            if ( ResourceVersionOverrideMap.TryGetValue( resourceName, out string resValue ) )
            {
                return resValue;
            }
            return null;
        }

        /// <summary>
        /// Retrieves change notifications using the given limit for message retrieval.
        /// </summary>
        /// <param name="limit">The number of messages to retrieve at once.</param>
        /// <returns>A list of ChangeNotifications, overriding any notifications with the desired version of the resource if
        /// notifications in the returned list match those added to this class (by resource name) to be overridden.</returns>
        /// <exception cref="ArgumentNullException">If resource name and versions are null. </exception>
        public async Task<IEnumerable<ChangeNotification>> GetChangeNotificationsAsync( int? limit = null )
        {
            EthosMessagesClient ??= EthosClientBuilder.BuildEthosMessagesClient();

            //Check if the ResourceVersionOverrideMap has any items in it. If it's empty then return all the change notifications as is.
            //If ResourceVersionOverrideMap has items with resource name & version then need to check 
            var changeNotificationList = await EthosMessagesClient.ConsumeAsync( limit );

            //If token was just created then don't need to create again.
            if ( EthosMessagesClient.Token != null )
            {
                EthosProxyClient.Token = EthosMessagesClient.Token;
            }

            //If ResourceVersionOverrideMap is null or does not have any items in it then just return change notifications otherwise process them.
            if ( ResourceVersionOverrideMap == null || !ResourceVersionOverrideMap.Any() )
            {
                return changeNotificationList;
            }
            return await ProcessChangeNotificationOverridesAsync( changeNotificationList );
        }

        /// <summary>
        /// Retrieves change notifications using the default limit of 20 messages for message retrieval.
        /// </summary>
        /// <param name="changeNotificationList"></param>
        /// <returns>A list of ChangeNotifications, overriding any notifications with the desired version of the resource if
        /// notifications in the returned list match those added to this class (by resource name) to be overridden.</returns>
        private async Task<IEnumerable<ChangeNotification>> ProcessChangeNotificationOverridesAsync( IEnumerable<ChangeNotification> changeNotificationList )
        {
            List<ChangeNotification> responseList = new List<ChangeNotification>();
            foreach ( var cn in changeNotificationList )
            {

                if ( cn.Operation.Equals( OPERATION_DELETED, StringComparison.InvariantCultureIgnoreCase ) )
                {
                    responseList.Add( cn );
                    continue;
                }
                ChangeNotification cnModified = await ProcessChangeNotificationOverrideAsync( cn );
                responseList.Add( cnModified );
            }
            return responseList;
        }

        /// <summary>
        /// Retrieves change notification using the default limit of 20 messages for message retrieval.
        /// </summary>
        /// <param name="cn"><see cref="ChangeNotification"/>.</param>
        /// <returns>A ChangeNotification, overriding any notifications with the desired version of the resource if
        /// notifications in the returned list match those added to this class (by resource name) to be overridden.</returns>
        private async Task<ChangeNotification> ProcessChangeNotificationOverrideAsync( ChangeNotification cn )
        {
            if ( ResourceVersionOverrideMap == null || ( ResourceVersionOverrideMap != null && !ResourceVersionOverrideMap.Any() ) ) return cn;

            foreach ( var keyValuePair in ResourceVersionOverrideMap )
            {
                string resName = keyValuePair.Key;
                if ( resName.Equals( cn.Resource.Name, StringComparison.InvariantCultureIgnoreCase ) )
                {
                    string versionValue = keyValuePair.Value;
                    if ( !versionValue.Equals( cn.Resource.Version ) )
                    {
                        EthosResponse ethosResponse = await EthosProxyClient.GetByIdAsync( cn.Resource.Name, cn.Resource.Id, versionValue );
                        cn = OverrideChangeNotification( cn, ethosResponse, versionValue );
                    }
                    break;
                }
            }
            return cn;
        }

        /// <summary>
        /// <b>Used internally by the SDK.</b> 
        /// <p>
        /// Updates the given changeNotification with the data from the EthosResponse. Specifically, updates the contentType,
        /// content, and version of the changeNotification.</p>
        /// </summary>
        /// <param name="cn">The ChangeNotification to update/override.</param>
        /// <param name="ethosResponse">The EthosResponse containing the data from the proxy call for overriding the changeNotification.</param>
        /// <param name="version">The full version header of the resource overridden.</param>
        /// <returns>The updated ChangeNotification.</returns>
        private ChangeNotification OverrideChangeNotification( ChangeNotification cn, EthosResponse ethosResponse, string version )
        {
            ChangeNotification changeNotification;
            string contentRestrictedHeader = ethosResponse.GetHeader( EthosProxyClient.HDR_X_CONTENT_RESTRICTED );
            string contentType = !string.IsNullOrWhiteSpace( contentRestrictedHeader ) ? contentRestrictedHeader : RESOURCE_CONTENT_TYPE;
            string versionHeader = ethosResponse.GetHeader( EthosProxyClient.HDR_X_MEDIA_TYPE );
            version = !string.IsNullOrWhiteSpace( versionHeader ) ? versionHeader : version;

            Resource resource = new Resource( cn.Resource.Id, cn.Resource.Name, version, cn.Resource.Domain );
            JObject content = JObject.Parse( ethosResponse.Content );
            changeNotification = new ChangeNotification( cn.Id, cn.Published, cn.Operation, cn.Publisher, resource, contentType, content );
            return changeNotification;
        }

        /// <summary>
        /// Instantiates this EthosChangeNotificationService with the given <see cref="EthosClientBuilder"/>.
        /// </summary>
        /// <param name="ethosClientBuilder"><see cref="EthosClientBuilder"/>.</param>
        /// <returns>EthosChangeNotificationService, for fluent API usage.</returns>
        public EthosChangeNotificationService WithEthosClientBuilder( EthosClientBuilder ethosClientBuilder )
        {
            EthosClientBuilder = ethosClientBuilder;
            BuildService( ethosClientBuilder );
            return this;
        }

        /// <summary>
        /// Sets the connection timeout value for the ethosClientBuilder.
        /// </summary>
        /// <param name="connectionTimeout">The timeout <b>in seconds</b> for a connection to be established, as used by the EthosClientBuilder.</param>
        /// <returns>EthosChangeNotificationService, for fluent API usage.</returns>
        public EthosChangeNotificationService WithConnectionTimeout( int connectionTimeout )
        {
            EthosClientBuilder.WithConnectionTimeout( connectionTimeout );
            return this;
        }

        /// <summary>
        /// Adds the resource name and version to the resourceVersionOverrideMap. This enables this service to override
        /// the content of change notifications that match the resource names and versions listed in this map with the content
        /// of the given resource for the version specified in the map.For example, if a change notification is for persons, v8
        /// and persons, v12 is added to the resourceVersionOverrideMap, then for the persons change notifications that are NOT
        /// v12, this service will retrieve persons v12 (using the GUID from the change notification) and replace the content
        /// of the persons change notification with the persons v12 content.This overrides the version in the change notification
        /// with the desired version listed in the resourceVersionOverrideMap.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="version">The desired version of the resource to override change notification of the same resource with.</param>
        /// <returns>EthosChangeNotificationService, for fluent API usage.</returns>
        public EthosChangeNotificationService WithResourceVersionOverride( string resourceName, string version )
        {
            AddToDictionary( resourceName, version );
            return this;
        }

        /// <summary>
        /// Same as see{@link com.ellucian.ethos.integration.service.EthosChangeNotificationService.
        /// EthosChangeNotificationService <see cref="WithResourceVersionOverride"/> except the version can be abbreviated as just the version value, e.g: 16, or 16.0.0. Also accepts the version value
        /// prefixed with the char 'v', e.g: v16, or v16.0.0.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="version">The desired version of the resource to override change notification of the same resource with.</param>
        /// <returns>EthosChangeNotificationService, for fluent API usage.</returns>
        public EthosChangeNotificationService WithResourceAbbreviatedVersionOverride( string resourceName, string version )
        {
            if ( version.ToUpper().StartsWith( "V" ) )
            {
                version = version.Substring( 1 );
            }
            string fullVersion = EthosConfigurationClient.FULL_VERSION.Replace( EthosConfigurationClient.FULL_VERSION_TAG, version );
            return WithResourceVersionOverride( resourceName, fullVersion );
        }

        /// <summary>
        /// Used internally by SDK to build message client, proxy client and configuration client.
        /// </summary>
        /// <param name="ethosClientBuilder"><see cref="EthosClientBuilder"/> builds clients used by <see cref="EthosChangeNotificationService"/>.</param>
        private void BuildService( EthosClientBuilder ethosClientBuilder )
        {
            EthosMessagesClient ??= ethosClientBuilder.BuildEthosMessagesClient();
            EthosProxyClient ??= ethosClientBuilder.BuildEthosProxyClient();
            ResourceVersionOverrideMap ??= new Dictionary<string, string>();
        }

        /// <summary>
        /// Builds an instance of the EthosChangeNotificationService with the given ethosClientBuilder and any resource version overrides.
        /// </summary>
        /// <param name="action">Actions delegate.</param>
        /// <param name="apiKey">A <see cref="Guid"/> api key.</param>
        /// <returns>An instance of the EthosChangeNotificationService.</returns>
        public static EthosChangeNotificationService Build( Action<EthosChangeNotificationService> action, string apiKey)
        {
            EthosChangeNotificationService ethosChangeNotificationService = new EthosChangeNotificationService(apiKey);
            action( ethosChangeNotificationService );
            return ethosChangeNotificationService;
        }
        /// <summary>
        /// Builds an instance of the EthosChangeNotificationService with the given ethosClientBuilder and any resource version overrides.
        /// </summary>
        /// <param name="action">Actions delegate.</param>
        /// <param name="colleagueApiUrl">The URL to the Colleague API instance.</param>
        /// <param name="colleagueApiUsername">The username used to connect to the Colleague API.</param>
        /// <param name="colleagueApiPassword">The password used to connect to the Colleague API.</param>
        public static EthosChangeNotificationService Build(Action<EthosChangeNotificationService> action, string colleagueApiUrl, string colleagueApiUsername, string colleagueApiPassword)
        {
            EthosChangeNotificationService ethosChangeNotificationService = new EthosChangeNotificationService(colleagueApiUrl, colleagueApiUsername, colleagueApiPassword);
            action(ethosChangeNotificationService);
            return ethosChangeNotificationService;
        }

        /// <summary>
        /// Adds resource name and version to a dictionary. If the key (resourceName) already exists with version value, then it replaces same resource with the version provided.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="version">The desired version of the resource to override change notification of the same resource with.</param>
        private void AddToDictionary( string resourceName, string version )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( "ERROR: Cannot add a resource version override for the EthosChangeNotificationService with a null or blank resource name value." );
            }

            if ( string.IsNullOrWhiteSpace( version ) )
            {
                throw new ArgumentNullException( "ERROR: Cannot add a resource version override for the EthosChangeNotificationService with a null or blank version header value." );
            }

            if ( ResourceVersionOverrideMap.ContainsKey( resourceName ) )
            {
                ResourceVersionOverrideMap.Remove( resourceName );
            }
            ResourceVersionOverrideMap.Add( resourceName, version );
        }

        #endregion
    }
}
