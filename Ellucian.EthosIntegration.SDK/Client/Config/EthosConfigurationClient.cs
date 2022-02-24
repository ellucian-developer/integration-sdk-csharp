/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client;
using Ellucian.Ethos.Integration.Client.Config;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ellucian.Ethos.Integration.Config
{
    /// <summary>
    /// A client that can be used to call configuration endpoints in the Ethos Integration API.
    /// </summary>
    public class EthosConfigurationClient: EthosClient
    {
        /// <summary>
        /// EthosResponseConverter used to convert from EthosResponses to string or Json formats.
        /// </summary>
        private EthosResponseConverter EthosResponseConverter { get; }

        #region Constants        

        /// <summary>
        /// Constant used when accessing a name property from a json.
        /// </summary>
        public readonly static string JSON_NAME = "name";

        /// <summary>
        /// Constant used when accessing an id property from a json.
        /// </summary>
        public readonly static string JSON_ID = "id";

        /// <summary>
        /// Constant used when accessing an appId property from a json.
        /// </summary>
        public readonly static string JSON_APPID = "appId";

        /// <summary>
        /// Constant used when accessing an applicationId property from a json.
        /// </summary>
        public readonly static string JSON_APPICATIONID = "applicationId";

        /// <summary>
        /// Constant used when accessing an appName property from a json.
        /// </summary>
        public static string JSON_APPNAME = "appName";

        /// <summary>
        /// Constant used when accessing a resources property from a json.
        /// </summary>
        public readonly static string JSON_RESOURCES = "resources";

        /// <summary>
        /// Constant used when accessing a resource.name property path from a json.
        /// </summary>
        public readonly static string JSON_RESOURCE_NAME = "resource.name";

        /// <summary>
        /// Constant used when accessing a resourceName property on a json.
        /// </summary>
        public readonly static string JSON_RESOURCENAME = "resourceName";

        /// <summary>
        /// Constant used when accessing a resource property from a json.
        /// </summary>
        public readonly static string JSON_RESOURCE = "resource";

        /// <summary>
        /// Constant used when accessing a representations property path from a json.
        /// </summary>
        public static string JSON_REPRESENTATIONS = "representations";

        /// <summary>
        /// Constant used when accessing an X-Media-Type property from a json.
        /// </summary>
        public readonly static string JSON_XMEDIATYPE = "X-Media-Type";

        /// <summary>
        /// Constant used when accessing versions property from a json.
        /// </summary>
        public readonly static string JSON_VERSIONS = "versions";

        /// <summary>
        /// Constant used when accessing a version property from a json.
        /// </summary>
        public readonly static string JSON_VERSION = "version";

        /// <summary>
        /// The tag used as a placeholder for the version value in the FULL_VERSION string.
        /// </summary>
        public readonly static string FULL_VERSION_TAG = "<VERSION>";

        /// <summary>
        /// Pattern used to provide the full version of a given resource.
        /// </summary>
        public readonly static string FULL_VERSION = $"application/vnd.hedtech.integration.v{ FULL_VERSION_TAG }+json";

        /// <summary>
        /// Constant used when accessing Filters.
        /// </summary>
        public readonly static string JSON_FILTERS = "filters";

        /// <summary>
        /// Constant used when accessing Named Queries.
        /// </summary>
        public readonly static string JSON_NAMED_QUERIES = "namedQueries";

        /// <summary>
        /// Constant used when accessing application/json property from a json.
        /// </summary>
        public readonly static string JSON_APPLICATION_JSON = "application/json";

        /// <summary>
        /// Constant used when accessing list of semantic version name.
        /// </summary>
        private readonly static string JSON_SEMANTIC_LIST = "semanticList";

        /// <summary>
        /// Constant used when accessing non semantic version list name.
        /// </summary>
        private readonly static string JSON_NON_SEMANTIC_LIST = "nonSemanticList";

        #endregion

        /// <summary>
        /// Public constructor taking a api key and http client parameter.
        /// <p>Note that the preferred way to instantiate this class is through the <see cref="EthosClientBuilder"/></p>
        /// </summary>
        /// <param name="apiKey">A valid API key from Ethos Integration. This is required to be a valid 36 character GUID string.
        /// If it is null or empty, an <see cref="ArgumentNullException"/> will be thrown or not in a valid GUID format, then a <see cref="FormatException"/> will be thrown.</param>
        /// <param name="httpClient">The http client required to make calls through the Ethos Proxy API.</param>
        public EthosConfigurationClient( string apiKey, HttpClient httpClient ) : base( apiKey, httpClient )
        {
            this.EthosResponseConverter = new EthosResponseConverter();
        }

        /// <summary>
        /// Get application's configuration info from Ethos Integration. This returns the config
        /// data for the application that this client's access token belongs to.
        /// </summary>
        /// <returns>The config info for your app in string format</returns>
        public async Task<string> GetAppConfigAsync()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            return await base.GetStringAsync( headers, EthosIntegrationUrls.AppConfig( Region ) );
        }

        /// <summary>
        /// Get application's configuration as a JSON object from Ethos Integration. This returns the config
        /// data for the application that this client's access token belongs to.
        /// </summary>
        /// <returns>The config info for your app in a <see cref="JObject"/>.</returns>
        public async Task<JObject> GetAppConfigJsonAsync()
        {
            return JObject.Parse( await GetAppConfigAsync() );
        }

        /// <summary>
        /// Get the details about all the available resources in the tenant associated with this client's access token.
        /// </summary>
        /// <returns>The available resources details in string format</returns>
        public async Task<string> GetAllAvailableResourcesAsync()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            return await base.GetStringAsync( headers, EthosIntegrationUrls.AvailableResources( Region ) );
        }

        /// <summary>
        /// Get the details about all the available resources in the tenant associate with this client's access token as a
        /// JSON array.
        /// </summary>
        /// <returns>The available resources details in a <see cref="JArray"/>.</returns>
        public async Task<JArray> GetAllAvailableResourcesAsJsonAsync()
        {
            return JArray.Parse( await GetAllAvailableResourcesAsync() );
        }

        /// <summary>
        /// Get the details about the available resources your application points to, from the tenant associated with this
        /// client's session token. The results will be filtered based on the 'ownerOverrides' array from the access token application's
        /// config data.
        /// </summary>
        /// <returns>The available resources details in a <see cref="JArray"/>.</returns>
        public async Task<JArray> GetAvailableResourcesForAppAsJsonAsync()
        {
            JObject appConfig = await GetAppConfigJsonAsync();
            JArray resourceList = FilterAvailableResources( await GetAllAvailableResourcesAsJsonAsync(),
                ( JArray ) appConfig [ "ownerOverrides" ] );
            return resourceList;
        }

        /// <summary>
        /// Get the details about the available resources your application points to, from the tenant associated with this
        /// client's access token. The results will be filtered based on the 'ownerOverrides' array from the access token application's
        /// config data.
        /// </summary>
        /// <returns>The available resources details in a string format</returns>
        public async Task<string> GetAvailableResourcesForAppAsync()
        {
            return ( await GetAvailableResourcesForAppAsJsonAsync() ).ToString();
        }

        /// <summary>
        /// Filter the given list of available resources by the list of desired resources. The expected formats are:
        /// <list type="bullet">
        /// <item><description>availableResources - JSON response from calling the /admin/available-resources endpoint in Ethos Integration</description></item>
        /// <item><description>desiredResources - JSON array containing objects with 'applicationId' and 'resourceName' properties. This is the format of
        /// the 'ownerOverrides' array from calling the /appConfig endpoint in Ethos Integration.
        /// </description></item>
        /// </list>
        /// </summary>
        /// <param name="availableResources">The entire list of available resources from Ethos Integration</param>
        /// <param name="desiredResources">The target list of specific app resources that you want returned</param>
        /// <returns>A filtered list of available resources</returns>
        public JArray FilterAvailableResources( JArray availableResources, JArray desiredResources )
        {
            JArray resourceList = new JArray();
            foreach ( var item in desiredResources )
            {
                string appId = ( string ) item [ JSON_APPICATIONID ];
                string resourceName = ( string ) item [ JSON_RESOURCENAME ];
                // get list of resources for target app
                var appResources = availableResources.Where( a => ( string ) a [ JSON_ID ] == appId ).FirstOrDefault();
                if ( appResources == null ) continue;
                // get the specific resource matching the name from the target app's list
                var resourceDetails = ( from r in appResources [ JSON_RESOURCES ] where r != null && ( string ) r [ JSON_NAME ] == resourceName select r ).ToList();
                if ( resourceDetails != null && resourceDetails.Count() > 0 )
                {
                    resourceList.Add( resourceDetails );
                }
            }
            return resourceList;
        }

        /// <summary>
        /// Get the details of a single resource from the tenant associated with this client's access token. The results will include
        /// details from each application in the tenant that owns the resource.
        /// <para>
        /// The format of the response will be a JSON array with each object containing the following properties:
        /// <list type="bullet">
        /// <item><description>appId - the ID of an owning application</description></item>
        /// <item><description>appName - the name of an owning application</description></item>
        /// <item><description>resource - the details of the resource</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="resourceName">The name of the resource for which you want details.</param>
        /// <returns>The resource details in string format</returns>
        public async Task<string> GetResourceDetailsAsync( string resourceName )
        {
            return ( await GetResourceDetailsAsJsonAsync( resourceName ) ).ToString();
        }

        /// <summary>
        /// Get the details of a single resource from the tenant associated with this client's access token. The results will include
        /// details from each application in the tenant that owns the resource.
        /// <para>
        /// The format of the response will be a JSON array with each object containing the following properties:
        /// <list type="bullet">
        /// <item><description>appId - the ID of an owning application</description></item>
        /// <item><description>appName - the name of an owning application</description></item>
        /// <item><description>resource - the details of the resource</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="resourceName">The name of the resource for which you want details.</param>
        /// <returns>The resource details in a JSON array</returns>
        public async Task<JArray> GetResourceDetailsAsJsonAsync( string resourceName )
        {
            JArray allResources = await GetAllAvailableResourcesAsJsonAsync();
            var filteredArrayNode = FilterAvailableResources( allResources, resourceName );
            if ( filteredArrayNode == null )
            {
                throw new EthosResourceNotFoundException( $"Ethos resource name { resourceName } not found in the available resources response.", resourceName );
            }

            return filteredArrayNode;
        }

        /// <summary>
        /// Filter the given list of available resources by the name of the resource. The expected format of the available resources list is
        /// the JSON response from calling the /admin/available-resources endpoint in Ethos Integration. This returns an array containing
        /// the details of the given resource from each owning application in the available resources list.
        /// </summary>
        /// <param name="availableResources">The entire list of available resources from Ethos Integration</param>
        /// <param name="resourceName">The name of the resource for which you want details</param>
        /// <returns>A filtered list of available resources</returns>
        public JArray FilterAvailableResources( JArray availableResources, string resourceName )
        {
            var response = availableResources
                .SelectMany( a =>
                    from r in a [ JSON_RESOURCES ]
                    where r != null && r.HasValues && ( string ) r [ JSON_NAME ] == resourceName
                    select new
                    {
                        appId = a [ JSON_ID ],
                        appName = a [ JSON_NAME ],
                        resource = r
                    } ).ToList();
            return JArray.FromObject( response );
        }

        /// <summary>
        /// Returns a list of version headers for the given resourceName filtered by the ownerOverrides property from appConfig.
        /// Returns a simple list of version headers because each resource is only listed once under ownerOverrides.
        /// The results will be filtered based on the resources available in the 'ownerOverrides' array from the session token application's
        /// configuration data.
        /// If resourceName not found in ownerOverrides, returns an empty List.
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <returns>A list of version headers according to whether the given resource is found in the ownerOverrides property of appConfig,
        /// or an empty list if not found.</returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<IEnumerable<string>> GetVersionHeadersForAppAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );
            JArray resourcesArrayNode = await GetAvailableResourcesForAppAsJsonAsync();
            return GetVersionListForOwnerOverrides( resourcesArrayNode, JSON_XMEDIATYPE );
        }

        /// <summary>
        /// Returns a list of versions for the given resourceName filtered by the ownerOverrides property from appConfig.
        /// Returns a simple list of versions because each resource is only listed once under ownerOverrides.
        /// The results will be filtered based on the resources available in the 'ownerOverrides' array from the session token application's
        /// configuration data.
        /// If resourceName not found in ownerOverrides, returns an empty List.
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <returns>A list of versions according to whether the given resource is found in the ownerOverrides property of appConfig,
        /// or an empty list if not found.Each version in the list will be prefixed with the 'v' char, e.g. "v4.5".</returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<IEnumerable<string>> GetVersionsForAppAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );
            JArray resourcesArrayNode = await GetAvailableResourcesForAppAsJsonAsync();

            return GetVersionListForOwnerOverrides( resourcesArrayNode, JSON_VERSION );
        }

        /// <summary>
        /// <b>Intended to be used internally by the SDK.</b>
        /// <p>
        /// Extracts a list of versions as either a list of version headers or a list of version values depending on the
        /// representationType.</p>
        /// </summary>
        /// <param name="resourcesArrayNode">The ArrayNode containing a list of resources per the ownerOverrides configuration from appConfig.</param>
        /// <param name="representationType">The type of version representation to extract, either "/version" for the version value,
        /// or "/X-Media-Type" for the version header value.</param>
        /// <returns>A list of either version values or version headers per the representationType.</returns>
        protected IEnumerable<string> GetVersionListForOwnerOverrides( JArray resourcesArrayNode, string representationType )
        {
            List<string> versionList = new List<string>();

            if ( resourcesArrayNode != null && resourcesArrayNode.Children().Any() )
            {
                foreach ( JObject item in resourcesArrayNode )
                {
                    var representationsNode = item.GetValue( JSON_REPRESENTATIONS );
                    if ( representationsNode != null && representationsNode.Children().Any() )
                    {
                        foreach ( JObject representation in representationsNode )
                        {
                            if ( representation.TryGetValue( representationType, StringComparison.InvariantCultureIgnoreCase, out JToken version ) )
                            {
                                versionList.Add( version.ToString() );
                            }
                        }
                    }
                }
            }
            return versionList;
        }

        /// <summary>
        /// Gets only the major version headers as a List of strings for the given resource name.
        /// Gets the entire version header string of the resource also found in the Accept Header
        /// of the request containing only the major versioning notation: e.g.application/vnd.hedtech.integration.v12+json
        /// <p>
        /// 
        /// <list type="bullet">
        ///     <listheader>
        ///         <term>The following table shows examples given various supported versions. Supported resource versions and versions returned by the SDK:</term>                 
        ///     </listheader>
        ///     <item>
        ///         <description>Resource Supports Versions SDK Returns Version</description><description/>
        ///     </item>
        ///     <item>
        ///         <description>v12.2.1, v12, v12.0.0, v13.1.0</description><description/>
        ///     </item>
        ///     <item>
        ///         <description>application/vnd.hedtech.integration.v12+json, application/vnd.hedtech.integration.v13+json</description><description/>
        ///     </item>
        ///     <item>
        ///         <description>v3, v4.5.0, v5.0.1</description>
        ///     </item>
        ///     <item>
        ///         <description>application/vnd.hedtech.integration.v3+json, application/vnd.hedtech.integration.v4+json, application/vnd.hedtech.integration.v5+json</description>
        ///     </item>
        ///     <item>
        ///         <description>v2.0.1, v3.2.0, v3.0.0</description>
        ///     </item>
        ///     <item>
        ///         <description>application/vnd.hedtech.integration.v2+json, application/vnd.hedtech.integration.v3+json, application/vnd.hedtech.integration.v10+json, application/vnd.hedtech.integration.v11+json</description>
        ///     </item>
        ///     <item><description>v10.0.0, v11.12.0, v11.12.3</description></item>
        /// </list>
        /// </p>
        /// <p>
        /// Also throws an ArgumentNullException (Runtime) if the given resourceName is null or empty.</p>
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <returns>A List of Strings where each item in the list is a major version supported by the given resource.</returns>
        public async Task<IEnumerable<string>> GetMajorVersionsOfResourceAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );

            var versionList = await GetVersionsOfResourceAsStringsAsync( resourceName );
            var filteredVersionList = FilterMajorVersions( versionList );
            List<string> versionHeaderList = new List<string>();
            foreach ( string filteredVersion in filteredVersionList )
            {
                versionHeaderList.Add( FULL_VERSION.Replace( FULL_VERSION_TAG, filteredVersion ) );
            }
            return versionHeaderList;
        }

        /// <summary>
        /// <b>Intended to be used internally within the SDK.</b>
        /// <p>
        /// Filters the given versionList to remove duplicate versions and ensure each version in the returned list
        /// a major whole number version only.</p>
        /// </summary>
        /// <param name="versionList">A list of versions from a given resource.</param>
        /// <returns>A list of filtered versions containing no duplicates where each version in the list is a major whole version only.</returns>
        protected List<string> FilterMajorVersions( IEnumerable<string> versionList )
        {
            List<string> filteredList = new List<string>();
            foreach ( string verStr in versionList )
            {
                string tempStr = string.Empty;

                if ( string.IsNullOrWhiteSpace( verStr ) )
                {
                    continue;
                }
                if ( verStr.StartsWith( "v" ) )
                {
                    tempStr = verStr.Substring( 1 ); // Remove the 'v' if it starts with it.
                }
                if ( verStr.Contains( "." ) )
                {
                    tempStr = verStr.Substring( 0, verStr.IndexOf( '.' ) );
                }
                if ( filteredList.Contains( verStr ) == false )
                {
                    filteredList.Add( tempStr );
                }
            }
            return filteredList;
        }

        /// <summary>
        /// Indicates if the given resource supports the given major version. For example, if the resource supports this version:
        /// <br><i>application/vnd.hedtech.integration.v12+json</i></br>
        /// <br>this method will return true if the major version is 12. However, if the resource only supports these versions:</br>
        /// <br><i>application/vnd.hedtech.integration.v12.0+json</i> OR<i>application/vnd.hedtech.integration.v12.0.0+json</i></br>
        /// <br>this method will return false.</br>
        /// <p>
        /// Also throws an ArgumentNullException (Runtime) if the given resourceName is null or empty.</p>
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <param name="majorVersion">The major version of the resource to check.</param>
        /// <param name="minorVersion">The minor version of the resource to check.</param>
        /// <param name="patchVersion">The patch version of the resource to check.</param>
        /// <returns>True if the given resource supports the given major version, false otherwise.</returns>        
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<bool> IsResourceVersionSupportedAsync( string resourceName, int? majorVersion = null, int? minorVersion = null, int? patchVersion = null )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );

            bool versionSupported = false;
            var versionList = await GetVersionsOfResourceAsStringsAsync( resourceName );
            if ( versionList != null && versionList.Any() )
            {
                foreach ( var version in versionList )
                {
                    string requestedVersion = string.Empty;
                    requestedVersion = majorVersion != null ? majorVersion.ToString() : string.Empty;
                    requestedVersion += minorVersion != null ? $".{ minorVersion.ToString() }" : string.Empty;
                    requestedVersion += patchVersion != null ? $".{ patchVersion.ToString() }" : string.Empty;
                    
                    // Substring to remove the beginning 'v' char.
                    if ( requestedVersion.Equals( version.Substring( 1 ) ) )
                    {
                        versionSupported = true;
                        break;
                    }
                }
            }
            return versionSupported;
        }

        /// <summary>
        /// Indicates if the given resource supports the given full version header. The full version header should be in the following
        /// format:  application/vnd.hedtech.integration.vSEMVER+json, where SEMVER is the semantic version of the requested resource.
        /// <p>
        /// Also throws an ArgumentNullException( Runtime) if the given resourceName or fullVersionHeader is null or empty.</p>
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <param name="fullVersionHeader">The full version header of the Ethos resource as described above.</param>
        /// <returns>True if the given resource supports the specified full version header, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<bool> IsResourceVersionSupportedAsync( string resourceName, string fullVersionHeader )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );

            if ( string.IsNullOrWhiteSpace( fullVersionHeader ) )
            {
                throw new ArgumentNullException( $"Error: cannot check if resource version is supported for resource '{ resourceName }' for the given full version header due to a null or empty full version header param." );
            }

            bool versionSupported = false;
            var versionHeaderList = await GetVersionHeadersOfResourceAsStringsAsync( resourceName );
            if ( versionHeaderList != null && versionHeaderList.Any() )
            {
                foreach ( var versionHeader in versionHeaderList )
                {
                    if ( fullVersionHeader.Equals( versionHeader, StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        versionSupported = true;
                        break;
                    }
                }
            }
            return versionSupported;
        }

        /// <summary>
        /// Indicates if the requested Ethos resource supports a version represented by the given SemVer object.
        /// SemVer objects are to be used where a version is truly a semantic version, and not when any of the major, minor,
        /// or patch versions of the semantic version notation are missing or not known.For example, this method should
        /// not be used when trying to determine if version 12.2 is supported because the given SemVer will translate 12.2
        /// into 12.2.0 which is different.
        /// <p>
        /// Also throws an ArgumentNullException( Runtime) if the given resourceName is null or empty or the given semVer is null.</p>
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <param name="semVer">The SemVer object containing the full semantic version to check for the resource.</param>
        /// <returns>True if the given resource supports the specified full version header, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<bool> IsResourceVersionSupportedAsync( string resourceName, SemVer semVer )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );
            
            if ( semVer == null )
            {
                throw new ArgumentNullException( $"Error: cannot check if resource version is supported for resource '{ resourceName }' for the given SemVer due to a null semVer param." );
            }

            bool versionSupported = false;
            var versionList = await GetVersionsOfResourceAsStringsAsync( resourceName );

            if ( versionList != null && versionList.Any() )
            {
                foreach ( var version in versionList )
                {
                    SemVer ver = new SemVer.Builder( version ).Build();
                    if ( semVer.Equals( ver ) )
                    {
                        versionSupported = true;
                        break;
                    }
                }
            }
            return versionSupported;
        }

        /// <summary>
        /// Gets a list of version header string values from the /versions json property of the ArrayNode 
        /// returned from {@link #getVersionHeadersOfResource(string) getVersionHeadersOfResource()}.
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <returns>A list of version header strings.</returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<IEnumerable<string>> GetVersionHeadersOfResourceAsStringsAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );

            var arrayNode = await GetVersionHeadersOfResourceAsync( resourceName );
            var versionList = GetVersionList( arrayNode, JSON_VERSIONS );
            return versionList.Distinct();
        }

        /// <summary>
        /// Gets a list of full version headers of the given resource name from the available-resources API.
        /// Each version in the list is from the /resource/representations/X-Media-Type property of the available-resources response
        /// for the given resource across all Ethos applications for the tenant in the access token.An example of a full version
        /// header returned in the list is "application/vnd.hedtech.integration.v8+json".
        /// The returned list may contain duplicate version header values.
        /// <p>
        /// Each element in the returned ArrayNode contains the following properties:
        /// <ul>
        ///     <li>appId - The GUID applicationId.</li>
        ///     <li>appName - The name of the application in Ethos Integration.</li>
        ///     <li>resourceName - The name of the Ethos resource.</li>
        ///     <li>versions - An array of version header values: e.g. [ "application/vnd.hedtech.integration.v8+json", "application/vnd.hedtech.integration.v12.1.0+json" ]</li>
        /// </ul></p>
        /// <p>
        /// The following is an example of the JSON data structure contained within the returned ArrayNode:
        /// <code>
        /// [
        ///     {
        ///        "appId" : "11111111-1111-1111-1111-111111111111",
        ///        "appName" : "Banner Integration API",
        ///        "resourceName" : "general-ledger-transactions",
        ///        "versions" : [
        ///        "application/vnd.hedtech.integration.v6+json",
        ///        "application/vnd.hedtech.integration.v8+json",
        ///        "application/vnd.hedtech.integration.v12.1.0+json",
        ///        "application/vnd.hedtech.integration.v12.0.0+json",
        ///        "application/vnd.hedtech.integration.v12+json",
        ///        "application/json"
        ///         ]
        ///     }
        /// ]
        /// </code></p>
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <returns>A list of supported version headers for the given resource.</returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<JArray> GetVersionHeadersOfResourceAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );
            return await GetResourceVersionsByRepresentationTypeAsync( resourceName, JSON_XMEDIATYPE );
        }

        /// <summary>
        /// Gets the full version header string for the given resource, major, minor, and patch version if the resource supports the given
        /// major.minor.patch version.If the resource does not support the major.minor.patch version, an UnsupportedVersionException is thrown,
        /// which is a RuntimeException.
        /// <p>
        /// Also throws an IllegalArgumentException (Runtime) if the given resourceName is null or empty.</p>
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <param name="majorVersion">The major version of the resource to check.</param>
        /// <param name="minorVersion">The minor version of the resource to check.</param>
        /// <param name="patchVersion">The patch version of the resource to check.</param>
        /// <returns>The full version string for the given resource and major.minor.patch version: e.g. application/vnd.hedtech.integration.v12.1.3+json</returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<string> GetVersionHeaderAsync( string resourceName, int? majorVersion = null, int? minorVersion = null, int? patchVersion = null )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );
            string requestedVersion = string.Empty;
            requestedVersion = majorVersion != null ? majorVersion.ToString() : string.Empty;
            requestedVersion += minorVersion != null ? $".{ minorVersion }" : string.Empty;
            requestedVersion += patchVersion != null ? $".{ patchVersion }" : string.Empty;

            if ( !await IsResourceVersionSupportedAsync( resourceName, majorVersion, minorVersion, patchVersion ) )
            {
                throw new UnsupportedVersionException( "The given major.minor.patch version is unsupported for the requested resource.", resourceName, requestedVersion );
            }
            return FULL_VERSION.Replace( FULL_VERSION_TAG, requestedVersion );
        }

        /// <summary>
        /// Gets the full version header string for the given resource and SemVer if the resource supports the given
        /// version contained within the SemVer.If the resource does not support the SemVer version, an UnsupportedVersionException is thrown,
        /// which is a RuntimeException.
        /// <p>
        /// Also throws an IllegalArgumentException (Runtime) if the given resourceName is null or empty or the given semVer is null.</p>
        /// <p>
        /// NOTE: This method should only be used when the complete semVer notation of a version is known.For example, using the SemVer version
        /// when only the major.minor version is provided will result in a patch version of 0, which may or may not be desired.</p>
        /// </summary>
        /// <param name="resourceName">The resource name for which to get a list of supported version headers.</param>
        /// <param name="semVer">The SemVer containing the requested version of the resource.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</exception>
        public async Task<string> GetVersionHeaderAsync( string resourceName, SemVer semVer )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );

            if ( semVer == null )
            {
                throw new ArgumentNullException( "Error: Cannot get full resource version for the given SemVer due to a null semVer param." );
            }

            if ( !await IsResourceVersionSupportedAsync( resourceName, semVer ) )
            {
                string unsupportedVersion = semVer.ToString();
                throw new UnsupportedVersionException( "The given major.minor.patch version is unsupported for the requested resource.", resourceName, unsupportedVersion );
            }
            return FULL_VERSION.Replace( FULL_VERSION_TAG, semVer.ToString() );
        }

        /// <summary>
        /// Gets a JObject containing the supported filters and named queries for the given resource and version header.
        /// If the given versionHeader is null or empty, the latest version will be used.
        /// <br></br>
        /// The following is an example of the JSON data structure within the returned JObject:
        /// <pre>
        /// {
        ///   "resourceName" : "persons",
        ///   "version" : "application/vnd.hedtech.integration.v12.3.0+json",
        ///   "namedQueries" : [
        ///       {
        ///           "filters" : [ "personFilter" ],
        ///           "name" : "personFilter"
        ///       }
        ///   ],
        ///   "filters" : [
        ///       "names.title",
        ///       "names.firstName",
        ///       "names.middleName",
        ///       "names.lastNamePrefix",
        ///       "names.lastName",
        ///       "names.pedigree",
        ///       "roles.role",
        ///       "credentials.type",
        ///       "credentials.value",
        ///       "alternativeCredentials.type.id",
        ///       "alternativeCredentials.value",
        ///       "emails.address"
        ///   ]
        /// }
        /// </pre>
        /// Throws an EthosResourceNotFoundException (Runtime) if the given resourceName is not found in the available-resource response.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get available filters and named queries for.</param>
        /// <param name="versionHeader">
        /// The version header value used to retrieve available filters and/or named queries for the resource.
        /// If null or empty, the latest version header value will be used.
        /// </param>
        /// <returns>
        /// A JObject containing the name of the resource and version header, with filters if they exist for the
        /// given resource and version, and/or named queries if they exist for the given resource and version.
        /// </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        public async Task<JObject> GetFiltersAndNamedQueriesAsync( string resourceName, string versionHeader = "" )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get filters and named queries for resource due to a null or empty resourceName param." );
            JArray appResourceArrayNode = await GetResourceDetailsAsJsonAsync( resourceName );
            if ( string.IsNullOrWhiteSpace( versionHeader ) )
            {
                versionHeader = await GetLatestVersionHeaderAsync( resourceName );
            }
            JObject filteredNode = new JObject();
            foreach ( JObject appResource in appResourceArrayNode )
            {
                var resourceNode = appResource.GetValue( JSON_RESOURCE );
                var representationsNode = resourceNode [ JSON_REPRESENTATIONS ];
                bool isFound = false;
                foreach ( JObject repNode in representationsNode )
                {
                    string repHeader = repNode [ JSON_XMEDIATYPE ].ToString();
                    if ( versionHeader == repHeader )
                    {
                        // found the version now get the filters and named queries.
                        filteredNode.Add( JSON_RESOURCENAME, resourceName );
                        filteredNode.Add( JSON_VERSION, versionHeader );
                        var namedQueryNode = repNode [ JSON_NAMED_QUERIES ];
                        if ( namedQueryNode != null && namedQueryNode.HasValues )
                        {
                            filteredNode [ JSON_NAMED_QUERIES ] = namedQueryNode;
                        }
                        var filtersNode = repNode [ JSON_FILTERS ];
                        if ( filtersNode != null && filtersNode.HasValues )
                        {
                            filteredNode [ JSON_FILTERS ] = filtersNode;
                        }
                        isFound = true;
                        break;
                    }
                    if ( isFound )
                    {
                        break;
                    }
                }
            }
            return filteredNode;
        }

        /// <summary>
        /// Gets a list of filters for the given resource name. The latest version of the given resource is used to obtain
        /// the list of filters.
        /// </summary>
        /// <param name="resourceName">The name of the Ethos resource for which to get a list of filters.</param>
        /// <param name="versionHeader">The full version header value for which to get a list of filters for the given resource.</param>
        /// <returns>A list of filter values (strings) which the given resource supports, or an empty list if none found.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        public async Task<IEnumerable<string>> GetFiltersAsync( string resourceName, string versionHeader = "" )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get latest version header of resource due to a null or empty resourceName param." );

            if ( string.IsNullOrWhiteSpace( versionHeader ) )
            {
                versionHeader = await GetLatestVersionHeaderAsync( resourceName );
            }

            JObject resourceFiltersNode = await GetFiltersAndNamedQueriesAsync( resourceName, versionHeader );
            List<string> filterList = new List<string>();
            if ( resourceFiltersNode.TryGetValue( JSON_FILTERS, out JToken filters ) )
            {
                foreach ( var item in filters.Children() )
                {
                    filterList.Add( item.ToString() );
                }
            }

            return filterList;
        }

        /// <summary>
        /// Gets named queries for the given resource name and version header value. If the given version header is
        /// null or empty, the latest version of the given resource will be used.
        /// </summary>
        /// <param name="resourceName">The name of the Ethos resource for which to get a map of named queries.</param>
        /// <param name="versionHeader">The full version header value for which to get a map of named queries for the given resource.</param>
        /// <returns>A map of named query values (strings) which the given resource supports, or an empty map if none found.
        /// Each key in the map is the name of a named query, with a corresponding value which is a string list of filter values.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        public async Task<Dictionary<string, IEnumerable<string>>> GetNamedQueriesAsync( string resourceName, string versionHeader = "" )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get latest version header of resource due to a null or empty resourceName param." );

            if ( string.IsNullOrWhiteSpace( versionHeader ) )
            {
                versionHeader = await GetLatestVersionHeaderAsync( resourceName );
            }

            JObject resourceFiltersNode = await GetFiltersAndNamedQueriesAsync( resourceName, versionHeader );
            List<string> nmQueriesList = new List<string>();
            Dictionary<string, IEnumerable<string>> namedQueriesDict = new Dictionary<string, IEnumerable<string>>();
            if ( resourceFiltersNode.TryGetValue( JSON_NAMED_QUERIES, out JToken nmQueries ) )
            {
                string namedQueryName = string.Empty;
                foreach ( var item in nmQueries.Children() )
                {
                    namedQueryName = item.SelectToken( JSON_NAME ).ToString();
                    nmQueriesList.Add( item.SelectToken( JSON_FILTERS ).ToString() );
                    namedQueriesDict.Add( namedQueryName, nmQueriesList );
                }

            }
            return namedQueriesDict;
        }

        /// <summary>
        /// Gets the latest version header of the given resource.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get the latest version header for.</param>
        /// <returns>The latest full version header value of the given resource.</returns>        
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        public async Task<string> GetLatestVersionHeaderAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get latest version header of resource due to a null or empty resourceName param." );
            string latestVersion = await GetLatestVersionAsync( resourceName );
            return FULL_VERSION.Replace( FULL_VERSION_TAG, latestVersion );
        }

        /// <summary>
        /// Gets the latest version of the given resource. Could return either a Semantic version (e.g. 12.0.0), or a
        /// non-semantic version( e.g. 12), as a string value.
        /// </summary>
        /// <param name="resourceName">The name of the resource to get the latest version of.</param>
        /// <returns>The latest version of the given resource, either a semantic version, or a non-semantic whole number value, as a string.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        private async Task<string> GetLatestVersionAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get latest version of resource due to a null or empty resourceName param." );

            var versionList = await GetVersionsOfResourceAsStringsAsync( resourceName );
            // the version list should contain both non-semantic, and semantic versions.
            // split it into both lists
            Dictionary<string, IEnumerable<object>> versionSplitMap = SplitVersionList( versionList );
            List<SemVer> semanticList = ( List<SemVer> ) versionSplitMap [ JSON_SEMANTIC_LIST ];
            List<string> nonSemanticList = ( List<string> ) versionSplitMap [ JSON_NON_SEMANTIC_LIST ];
            if ( !semanticList.Any() && !nonSemanticList.Any() )
            {
                // Both lists are empty, so no version was found for this resource, and the null/empty version value is
                // removed from the versionList for application/json versions. So return application/json as the version.
                return JSON_APPLICATION_JSON;
            }
            if ( !semanticList.Any() )
            {
                // The semantic list is empty, so get the first element from the non-semantic list, convert it to a string and return it.
                string latestVersion = nonSemanticList [ 0 ];
                return latestVersion.ToString();
            }
            if ( !nonSemanticList.Any() )
            {
                // The non-semantic list is empty, so get the first element from the semantic list, convert it to a string and return it.
                string latestVersion = nonSemanticList [ 0 ];
                return latestVersion.ToString();
            }
            string nonSemanticVer = nonSemanticList [ 0 ];
            SemVer semanticVer = semanticList [ 0 ];
            if ( int.TryParse( nonSemanticVer, out int tempInt ) )
            {
                if ( semanticVer.Major >= tempInt )
                {
                    // If the semantic major version value is >= the non-semantic version, then return the semantic version.
                    // This includes being equal because there is a semantic version.
                    return semanticVer.ToString();
                }
            }
            // The non-semantic version is > semantic, so return it.
            return nonSemanticVer.ToString();
        }

        /// <summary>
        /// Gets a list of version string values from the /versions json property of the JArray returned from {@link #getVersionsOfResource(string) getVersionsOfResource()}.
        /// Version values are gathered from across Ethos Integration applications for the given tenant(access token) and resource name,
        /// and should not contain duplicate values.
        /// </summary>
        /// <param name="resourceName">The name of the Ethos resource to get a list of versions for.</param>
        /// <returns>A list of version strings. Each element in the list is prefixed with the 'v' char, e.g. "v4.5".</returns>
        public async Task<IEnumerable<string>> GetVersionsOfResourceAsStringsAsync( string resourceName )
        {
            JArray arrayNode = await GetVersionsOfResourceAsync( resourceName );
            var versionList = GetVersionList( arrayNode, JSON_VERSIONS );
            return versionList.Distinct().ToList();
        }

        /// <summary>
        /// <b>Intended to be used internally by the SDK.</b>
        /// <p/>
        /// Reads the given application/resource arrayNode and returns a simple list of version strings from that node.
        /// The given arrayNode is expected to contain an array of the following properties:
        /// <ul>
        ///     <li>appId - the application ID of the app in Ethos Integration</li>
        ///     <li>appName - the name of the application in Ethos Integration</li>
        ///     <li>resourceName - the name of the Ethos resource</li>
        ///     <li>versions - a list of versions based on the representationType</li>
        /// </ul>
        /// </summary>
        /// <param name="arrayNode">The arrayNode containing application/resource info with a list of versions per resource.</param>
        /// <param name="representationType">The JSON property to use, expected to be "/versions" when the array node contains both version
        /// values (v2, v3.0.4, etc. ) and version header strings.
        /// </param>
        /// <returns>A list of the versions as strings.</returns>
        protected IEnumerable<string> GetVersionList( JArray arrayNode, string representationType )
        {
            List<string> versionList = new List<string>();

            foreach ( JObject node in arrayNode )
            {
                if ( node.TryGetValue( representationType, out JToken versions ) )
                {
                    if ( versions != null && versions.Any() )
                    {
                        versions.ToList().ForEach( i =>
                        {
                            versionList.Add( i.ToString() );
                        } );
                    }
                }
            }
            return versionList;
        }

        /// <summary>
        ///  Gets a list of versions of the given resource name from the available-resources API.
        ///  Each version in the list is from the /resource/representations/version property of the available-resources response
        ///  for the given resource across all Ethos applications for the tenant in the access token.Each version value in the
        ///  returned list is prefixed with the 'v' char, e.g. 'v4.5.1'. The returned list may contain duplicate version values.
        ///  <p>
        ///  Each element in the returned ArrayNode contains the following properties:
        ///  <ul>
        ///      <li>appId - The GUID applicationId.</li>
        ///      <li>appName - The name of the application in Ethos Integration.</li>
        ///      <li>resourceName - The name of the Ethos resource.</li>
        ///      <li>versions - An array of version values, each prefixed with the 'v' char: e.g. [ 'v6', 'v8', 'v12.1.0' ]</li>
        ///  </ul></p>
        ///  <p>
        ///  The following is an example of the JSON data structure contained within the returned ArrayNode:
        ///  <code>
        ///  [
        ///     {
        ///        &quot;appId&quot; : &quot;11111111-1111-1111-1111-111111111111&quot;,
        ///        &quot;appName&quot; : &quot;Banner Integration API&quot;,
        ///        &quot;resourceName&quot; : &quot;general-ledger-transactions&quot;,
        ///        &quot;versions&quot; : [ &quot;v6&quot;, &quot;v8&quot;, &quot;v12.1.0&quot;, &quot;v12.0.0&quot;, &quot;v12&quot; ]
        ///     }
        ///  ]
        ///  </code></p>
        /// </summary>
        /// <param name="resourceName">The resource for which to get a list of versions.</param>
        /// <returns>A list of supported version values for the given resource.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        private async Task<JArray> GetVersionsOfResourceAsync( string resourceName )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get versions of resource due to a null or empty resourceName param." );
            // Should return an arrayNode of objects containing appId, appName, resourceName, and list of versions across all apps in the tenant.
            return await GetResourceVersionsByRepresentationTypeAsync( resourceName, JSON_VERSION );
        }

        /// <summary>
        /// <b>Intended to be used internally within the SDK.</b>
        /// <p/>
        /// Gets an ArrayNode containing a list of objects with each element in the list having this structure:
        /// <ul>
        ///     <li>appId - the application ID of the app in Ethos Integration</li>
        ///     <li>appName - the name of the application in Ethos Integration</li>
        ///     <li>resourceName - the name of the Ethos resource</li>
        ///     <li>versions - a list of versions based on the representationType</li>
        /// </ul>
        /// <p/>
        /// Gets a list of versions of the given resource name from the available-resources API.
        /// Uses the representationType param to return either a list of version headers, or a list of version values.
        /// Version values are prefixed with the char 'v', e.g.v4.5.0.
        /// To return a list of version headers, the representationType must be '/X-Media-Type'. To return a list of version
        /// values, the representationType must be '/version'.
        /// A runtime ArgumentNullException will be thrown if the resourceName param is null or empty, or if the representationType
        /// param is not one of those two supported values.
        /// Each version in the list is from the /resource/representations/X-Media-Type or the /resource/representations/version
        /// property of the available-resources response for the given resource across all Ethos applications for the tenant in the access token.
        /// The returned list may contain duplicate version values.
        /// </summary>
        /// <param name="resourceName">The resource for which to get a list of versions.</param>
        /// <param name="representationType">
        /// The jsonLabel in the available-resources response used to access the version or version header value.
        /// Expected to only be 'version' or 'X-Media-Type', otherwise an ArgumentException will be thrown.
        /// </param>
        /// <returns>
        /// A list of supported version values for the given resource according to the given representationType. If the
        /// representationType is 'X-Media-Type' a list of full version headers will be returned. If the representationType
        /// is '/version' a list of version values where each version value is prefixed with the 'v' char will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">If resource name is not provided.</exception>
        /// <exception cref="ArgumentNullException">If representationType name is not provided.</exception>        
        /// <exception cref="ArgumentException">If representationType does not match 'version' or 'X-Media-Type'.</exception>
        protected async Task<JArray> GetResourceVersionsByRepresentationTypeAsync( string resourceName, string representationType )
        {
            ValidateResourceNameNotNull( resourceName, "Error: Cannot get resource versions by representation type due to a null or empty resourceName param." );

            if ( representationType == null )
            {
                throw new ArgumentNullException( "Error: Cannot get resource versions by representation type due to a null representation type." );
            }

            if ( representationType.Equals( JSON_VERSION ) == false &&
                representationType.Equals( JSON_XMEDIATYPE ) == false )
            {
                throw new ArgumentException( "Error: Cannot get resource versions by representation type due to an invalid representation type value.  " +
                                                    "This value must be either \"/version\" or \"/X-Media-Type\"." );
            }

            JArray resultArrayNode = new JArray();
            JArray arrayNode = await GetResourceDetailsJsonAsync( resourceName );

            if ( arrayNode != null && arrayNode.Any() )
            {
                foreach ( JObject appResourceNode in arrayNode.ToList() )
                {
                    string appId = appResourceNode.GetValue( JSON_APPID ).ToString();
                    string appName = appResourceNode.GetValue( JSON_APPNAME ).ToString();
                    string rscName = ( string ) appResourceNode.SelectToken( JSON_RESOURCE_NAME );
                    JObject filteredNode = new JObject();
                    JArray versionArrayNode = new JArray();
                    filteredNode.Add( JSON_APPID, appId );
                    filteredNode.Add( JSON_APPNAME, appName );
                    filteredNode.Add( JSON_RESOURCENAME, rscName );

                    var representations = appResourceNode [ JSON_RESOURCE ] [ JSON_REPRESENTATIONS ];
                    if ( representations != null && representations.Children().Any() )
                    {
                        foreach ( JObject reprepresentation in representations.ToList() )
                        {
                            var reprType = reprepresentation [ representationType ];
                            if ( reprType != null )
                            {
                                versionArrayNode.Add( reprType );
                            }
                        }
                        filteredNode.Add( JSON_VERSIONS, versionArrayNode );
                        resultArrayNode.Add( filteredNode );
                    }
                }
            }
            return resultArrayNode;
        }

        /// <summary>
        /// Get the details of a single resource from the tenant associated with this client's access token. The results will include
        /// details from each application in the tenant that owns the resource.
        /// <p>
        /// The format of the response will be a JSON array with each object containing the following properties:
        /// <list type="bullet">
        /// <item><description>appId - the ID of an owning application</description></item>
        /// <item><description>appName - the name of an owning application</description></item>
        /// <item><description>resource - the details of the resource</description></item>
        /// </list>
        /// </p>
        /// </summary>
        /// <param name="resourceName">The name of the resource for which you want details.</param>
        /// <returns>The resource details in a JSON array</returns>
        public async Task<JArray> GetResourceDetailsJsonAsync( string resourceName )
        {
            JArray allResources = await GetAllAvailableResourcesAsJsonAsync();
            return FilterAvailableResources( allResources, resourceName );
        }

        /// <summary>
        /// <b>Used internally by the SDK.</b>
        /// <p>
        /// The given versionList could contain both semantic and non-semantic version values.Splits the versionList
        /// into 2 lists:  1 with SemVer values, and 1 with non-semantic Integer version values.</p>
        /// <p>
        /// Any null or blank version values in the versionList will be filtered out and not processed.</p>
        /// </summary>
        /// <param name="versionList"></param>
        /// <returns></returns>
        private Dictionary<string, IEnumerable<object>> SplitVersionList( IEnumerable<string> versionList )
        {
            Dictionary<string, IEnumerable<object>> resultMap = new Dictionary<string, IEnumerable<object>>();
            List<SemVer> semanticList = new List<SemVer>();
            List<string> nonSemanticList = new List<string>();

            foreach ( string version in versionList )
            {
                string ver = version;
                if ( string.IsNullOrWhiteSpace( ver ) )
                {
                    continue;
                }
                if ( ver.StartsWith( "v" ) )  // strip off the "v" character if it is there.
                {
                    ver = ver.Substring( 1 );
                }
                if ( ver.Contains( "." ) ) // If the ver value contains a dot. it's semantic.
                {
                    semanticList.Add( new SemVer.Builder( ver ).Build() );
                }
                else
                {
                    nonSemanticList.Add( ver );
                }
            }

            semanticList = semanticList.OrderByDescending( mj => mj.Major ).ThenByDescending( m => m.Minor ).ThenByDescending( p => p.Patch ).ToList();
            nonSemanticList = nonSemanticList.OrderByDescending( i => i ).ToList();

            resultMap.Add( JSON_SEMANTIC_LIST, semanticList );
            resultMap.Add( JSON_NON_SEMANTIC_LIST, ( IEnumerable<object> ) nonSemanticList );

            return resultMap;
        }

        /// <summary>
        /// <b>Used internally by the SDK.</b>
        /// <p>
        /// Validates the given resourceName to ensure it is not null or empty( blank).
        /// Throws an ArgumentNullException with the given errorMessage if the resourceName is null or empty.</p>
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="errorMessage"></param>
        /// <exception cref="ArgumentNullException">When <paramref name="resourceName"/> is passed as null or empty or white space.</exception>
        private static void ValidateResourceNameNotNull( string resourceName, string errorMessage )
        {
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                throw new ArgumentNullException( nameof( resourceName ), errorMessage );
            }
        }
    }
}
