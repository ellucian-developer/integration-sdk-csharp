/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Authentication;

using System.Collections.Generic;
using System.Text;

namespace Ellucian.Ethos.Integration
{
    /// <summary>
    /// Utility class used for building Ethos Integration URLs with various criteria.
    /// </summary>
    public static class EthosIntegrationUrls
    {
        /// <summary>
        /// A Dictionary&lt;SupportedRegions,string&gt; of supported regions where each region is assigned the
        /// appropriate country code top-level domain and/or second-level domain.
        /// Supported regions include:
        /// <list type="bullet">
        /// <item>
        /// <description>US: .com</description>
        /// </item>
        /// <item>
        /// <description>CANADA: .ca</description>
        /// </item>
        /// <item>
        /// <description>EUROPE: .ie</description>
        /// </item>
        /// <item>
        /// <description>AUSTRALIA: .com.au</description>
        /// </item>
        /// <item>
        /// <description>SELF-HOSTED</description>
        /// </item>
        /// </list>
        /// </summary>
        private static readonly Dictionary<dynamic, string> RegionUrlPostFix = new Dictionary<dynamic, string>
        {
            [ SupportedRegions.US ] = ".com",
            [ SupportedRegions.Canada ] = ".ca",
            [ SupportedRegions.Europe ] = ".ie",
            [ SupportedRegions.Australia ] = ".com.au",
            [ SupportedRegions.SelfHosted ] = ""
        };


#pragma warning disable S1075
        const string MAIN_ETHOS_BASE_URL = "https://integrate.elluciancloud";
#pragma warning restore S1075

        /// <summary>The override for self-hosted ERP clients.</summary>
        public static string SelfHostBaseUrl { get; set; } = "";

        ///<summary>The main domain for Ethos Integration.</summary>
        public static string MAIN_BASE_URL
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SelfHostBaseUrl))
                    return MAIN_ETHOS_BASE_URL;

                return SelfHostBaseUrl;
            }
        }

        /// <summary>
        /// The base URL for getting Api result(s) in Ethos Integration.
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <param name="resource">The Ethos resource the URL should contain.</param>
        /// <param name="id">The (optional) ID for the given resource to build URLs for "get by ID" requests.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Proxy APIs.</returns>
        public static string Api( SupportedRegions region, string resource, string id = "" )
        {
            string url = BuildUrl(region, "/api");
            if (!string.IsNullOrWhiteSpace(resource))
            {
                url += ("/" + resource);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    url += ("/" + id);
                }
            }
            return url;
        }

        /// <summary>
        /// Builds a URL for interacting with the proxy APIs through Ethos Integration supporting filters.
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <param name="resource">The Ethos resource the URL should contain.</param>
        /// <param name="filter">The resource filter the URL should contain.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Proxy APIs, supporting filters.</returns>
        public static string ApiFilter( SupportedRegions region, string resource, string filter )
        {
            string url = Api( region, resource, null );
            StringBuilder sb = new StringBuilder();
            sb.Append( url );
            if ( !string.IsNullOrWhiteSpace( filter ) )
            {
                sb.Append( filter );
            }
            return sb.ToString();
        }

        /// <summary>
        /// The base URL for getting QAPI result(s) in Ethos Integration.
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <param name="resource">The Ethos resource the URL should contain.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Proxy APIs.</returns>
        public static string Qapi( SupportedRegions region, string resource )
        {
            string url = BuildUrl( region, "/qapi" );
            if ( !string.IsNullOrWhiteSpace( resource ) )
            {
                url = ( $"{url}/{resource}" );
            }
            return url;
        }

        /// <summary>
        /// Builds a URL for interacting with the proxy APIs through Ethos Integration supporting paging for QAPI POST requests
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <param name="resource">The Ethos resource the URL should contain.</param>
        /// <param name="offset">The row index from which to begin paging for data for the given resource.</param>
        /// <param name="pageSize">The number of rows each response can contain.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Proxy APIs.</returns>
        public static string QapiPaging( SupportedRegions region, string resource, int offset, int pageSize )
        {
            string url = Qapi( region, resource );
            return AddPaging( url, offset, pageSize );
        }


        /// <summary>
        /// Builds a URL for interacting with the proxy APIs through Ethos Integration supporting paging with filters.
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <param name="resource">The Ethos resource the URL should contain.</param>
        /// <param name="filter">The resource filter the URL should contain.</param>
        /// <param name="offset">The row index from which to begin paging for data for the given resource.</param>
        /// <param name="pageSize">The number of rows each response can contain.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Proxy APIs, supporting filters.</returns>
        public static string ApiFilterPaging( SupportedRegions region, string resource, string filter, int offset, int pageSize )
        {
            string url = ApiFilter( region, resource, filter );
            return AddPaging( url, offset, pageSize );
        }

        /// <summary>
        /// Builds a URL for interacting with the Proxy APIs through Ethos Integration, in support of paging.
        /// </summary>
        /// <param name="region">The appropriate supported region to build the URL with.</param>
        /// <param name="resource">The Ethos resource the URL should contain.</param>
        /// <param name="offset">The row index from which to begin paging for data for the given resource.</param>
        /// <param name="pageSize">The number of rows each response can contain.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Proxy APIs, in support of paging.</returns>
        public static string ApiPaging( SupportedRegions region, string resource, int offset, int pageSize )
        {
            string urlStr = Api( region, resource, null );
            return AddPaging( urlStr, offset, pageSize );
        }

        /// <summary>
        /// Builds an Ethos Integration URL supporting the Errors API.
        /// </summary>
        /// <param name="region">The appropriate supported region to build the URL with.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Errors APIs.</returns>
        public static string Errors( SupportedRegions region )
        {
            return BuildUrl( region, "/errors" );
        }

        /// <summary>
        /// Builds a URL for interacting with the Errors API through Ethos Integration, in support of paging.
        /// </summary>
        /// <param name="region">The appropriate supported region to build the URL with.</param>
        /// <param name="offset">The row index from which to begin paging for errors.</param>
        /// <param name="pageSize">The number of errors (limit) each response can contain.</param>
        /// <returns>A string value containing the URL to use for interacting with EthosIntegration Errors API, in support of paging.</returns>
        public static string ErrorsPaging( SupportedRegions region, int offset, int pageSize )
        {
            string url = Errors( region );
            return AddPaging( url, offset, pageSize );
        }

        /// <summary>
        /// Builds an Ethos Integration URL supporting the Token API.
        /// </summary>
        /// <param name="region">The appropriate supported region to build the URL with.</param>
        /// <returns>A string value containing the URL to use for interacting with Ethos Integration Token API.</returns>
        public static string Auth( SupportedRegions region )
        {
            return BuildUrl( region, "/auth" );
        }

        /// <summary>
        /// The URL for getting an application's configuration in Ethos Integration.
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <returns>The region specific /appconfig URL</returns>
        public static string AppConfig( SupportedRegions region )
        {
            return BuildUrl( region, "/appconfig" );
        }

        /// <summary>
        /// The URL for getting information about all the available resources in your tenant in Ethos Integration.
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <returns>The region specific /available-resources URL</returns>
        public static string AvailableResources( SupportedRegions region )
        {
            return BuildUrl( region, "/admin/available-resources" );
        }

        /// <summary>
        /// Builds an Ethos Integration URL supporting the consume API.
        /// </summary>
        /// <param name="region">The appropriate supported region to build the URL with.</param>
        /// <param name="lastProcessedID">A value to use for the 'lastProcessedID' query parameter. Any value of zero or greater will
        /// be added to the URL as a query parameter. If the value is less than zero, it will not be added
        /// to the URL.</param>
        /// <param name="limit">A value to use for the 'limit' query parameter. Any value greater than zero will be added to the URL as
        /// a query parameter. If the value is zero or less, it will not be added to the URL.</param>
        /// <returns>The URL to use for calling the Ethos Integration consume endpoint.</returns>
        public static string Consume( SupportedRegions region, long? lastProcessedID, int? limit )
        {
            StringBuilder builder = new StringBuilder();

            if ( lastProcessedID.HasValue && lastProcessedID >= 0 )
            {
                _ = builder.Append( $"lastProcessedID={lastProcessedID}" );
            }

            if ( limit.HasValue && limit > 0 )
            {
                if ( builder.Length > 0 )
                {
                    _ = builder.Append( "&" );
                }
                _ = builder.Append( $"limit={limit}" );
            }

            string url = builder.Length > 0 ? $"/consume?{ builder }" : "/consume";
            return BuildUrl( region, url );
        }

        /// <summary>
        /// The URL for getting authentication tokens in Ethos Integration.
        /// </summary>
        /// <param name="region">A supported region.</param>
        /// <returns></returns>
        public static string BaseUrl( SupportedRegions region )
        {
            return $"{MAIN_BASE_URL}{RegionUrlPostFix [ region ]}";
        }

        /// <summary>
        ///  Builds the URL with the mainBaseUrl, the supported region, and the correct path.
        /// </summary>
        /// <param name="region">The appropriate supported region to build the URL with.</param>
        /// <param name="urlEnd">The correct path for the type of API the URL will be used with (/api for Proxy API URL,</param>
        /// <see cref="Auth(SupportedRegions)"> for Token API URL, etc.).</see>
        /// <returns></returns>
        private static string BuildUrl( SupportedRegions region, string urlEnd)
        {
            return region == SupportedRegions.SelfHosted
                ? $"{MAIN_BASE_URL}"
                : $"{MAIN_BASE_URL}{RegionUrlPostFix[region]}{urlEnd}";
        }

        /// <summary>
        /// Adds paging filter criteria to the given URL string.
        /// </summary>
        /// <param name="urlStr">The URL string to add paging criteria to.</param>
        /// <param name="offset">The offset param to page from.</param>
        /// <param name="pageSize">The limit param to page with.</param>
        /// <returns>A URL string containing the offset and limit params for paging.</returns>
        private static string AddPaging( string urlStr, int offset, int pageSize )
        {
            StringBuilder sb = new StringBuilder( urlStr );
            if ( offset >= 0 && pageSize > 0 )
            {
                // If offset is >= 0 and pageSize is > 0 then use them to build the URL.
                if ( urlStr.Contains( "?" ) )
                {
                    sb.Append( "&offset=" );
                }
                else
                {
                    sb.Append( "?offset=" );
                }
                sb.Append( offset );
                sb.Append( "&limit=" );
                sb.Append( pageSize );
            }
            else if ( offset >= 0 )
            {
                // Offset >= 0, so pageSize must be negative.  Do not include pageSize.
                if ( urlStr.Contains( "?" ) )
                {
                    sb.Append( "&offset=" );
                }
                else
                {
                    sb.Append( "?offset=" );
                }
                sb.Append( offset );
            }
            else if ( pageSize > 0 )
            {
                // pageSize > 0, so offset must be negative.  Do not include offset.
                if ( urlStr.Contains( "?" ) )
                {
                    sb.Append( "&limit=" );
                }
                else
                {
                    sb.Append( "?limit=" );
                }
                sb.Append( pageSize );
            }
            // By default, if both offset and pageSize are negative, they will not be included in the URL.
            return sb.ToString();
        }
    }
}
