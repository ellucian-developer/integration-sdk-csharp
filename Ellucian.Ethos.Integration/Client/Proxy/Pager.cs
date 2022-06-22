/*
 * ******************************************************************************
 *   Copyright 2022 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;

namespace Ellucian.Ethos.Integration.Client.Proxy
{
    /// <summary>
    /// Data transfer object (DTO) used primarily within the SDK to easily specify various criteria supporting paging operations. 
    /// This class follows the builder pattern and contains an inner static Builder class used to build this object with the various attributes/properties.
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// Various paging types to determine what kind of paging to be done.  This class is built with different attributes 
        /// depending on the pagingType( howToPage attribute ).
        /// </summary>
        public enum PagingType
        {
            ///<summary>Page for all of the pages for a given resource, starting at the beginning (offset 0).</summary>
            PageAllPages,
            ///<summary>Page up to the specified number of pages, starting at the beginning (offset 0). 
            ///e.g. Page some resource for 47 pages.</summary>
            PageToNumPages,
            ///<summary>Page beginning from the given offset (row num) for all of the data. 
            ///e.g. Page for some resource from offset (row) 33 for all of the resource data.</summary>
            PageFromOffset,
            ///<summary>Page from the given offset for some number of pages. 
            ///e.g.Page for some resource from offset( row) 33 for 7 pages.</summary>
            PageFromOffsetForNumPages,
            ///<summary>Page up to some number of rows. 
            ///e.g.Page for some resource from the beginning( offset 0) up to row 77.</summary>
            PageToNumRows,
            ///<summary>Page from the given offset for some number of rows.
            ///e.g.Page for some resource from offset( row) 33 up to row 77.</summary>
            PageFromOffsetForNumRows
        }

        /// <summary>
        /// The resource name used when paging.
        /// </summary>
        public string ResourceName { get; private set; }
        /// <summary>
        /// The version specified when paging.
        /// </summary>
        public string Version { get; internal set; }
        /// <summary>
        /// The pagingType for how to page.
        /// </summary>
        public PagingType HowToPage { get; set; }
        /// <summary>
        /// The pageSize specified when paging.
        /// </summary>
        public int PageSize { get; internal set; }
        /// <summary>
        /// The number of pages to page for.
        /// </summary>
        public int NumPages { get; internal set; }
        /// <summary>
        /// The number of rows to page for.  Data is still returned in pages up to the number of rows when this is specified.
        /// </summary>
        public int NumRows { get; internal set; }
        /// <summary>
        /// The total count (of rows) for a given resource.
        /// </summary>
        public int TotalCount { get; internal set; }
        /// <summary>
        /// The offset to begin paging from.
        /// </summary>
        public int Offset { get; internal set; }
        /// <summary>
        /// Indicator of whether paging should be done or not for a given request.
        /// </summary>
        public bool ShouldDoPaging { get; internal set; }
        /// <summary>
        /// Ethos response.
        /// </summary>
        public EthosResponse EthosResponse { get; internal set; }

        /// <summary>
        /// The optional request URL criteria used when paging.
        /// </summary>
        public string CriteriaFilter { get; internal set; }

        /// <summary>
        /// The optional request URL named query used when paging.
        /// </summary>
        public string NamedQueryFilter { get; internal set; }

        /// <summary>
        /// The optional request URL filter map used when paging.
        /// </summary>
        public string FilterMap { get; internal set; }

        /// <summary>
        /// The optional QAPI request body used when making filter QAPI POST request.
        /// </summary>
        public string QapiRequestBody { get; internal set; }

        /// <summary>
        /// Assigns the given resourceName to the resourceName of Pager.
        /// </summary>
        /// <param name="resourceName">Name of the resource e.g. student-cohorts</param>
        /// <returns>Pager object.</returns>
        public Pager ForResource( string resourceName ) { ResourceName = resourceName; return this; }

        /// <summary>
        /// Assigns the specified version and returns this builder for fluent API functionality.
        /// </summary>
        /// <param name="version">The version of the Ethos resource (A.K.A media type).</param>
        /// <returns>Pager object.</returns>
        public Pager ForVersion( string version ) { Version = version; return this; }

        /// <summary>
        ///  Assigns the specified request URL criteria filter and returns this builder for fluent API functionality. 
        ///  Nulls out the namedQueryFilter and filterMap because there can only be one filter approach used at a time.
        ///  Nulls out the namedQueryFilter, filterMap, and qapiRequestBody because there can only be one filter approach used at a time.
        /// </summary>
        /// <param name="criteriaFilter">The request URL criteria-based filter which can also be used when paging.</param>
        /// <returns>The Builder with the criteria filter assigned.</returns>
        public Pager WithCriteriaFilter( string criteriaFilter )
        {
            NamedQueryFilter = null;
            FilterMap = null;
            CriteriaFilter = criteriaFilter;
            QapiRequestBody = null;
            return this;
        }

        /// <summary>
        /// Assigns the specified request URL named query filter and returns this builder for fluent API functionality. 
        /// Nulls out the criteriaFilter, filterMap, and qapiRequestBody because there can only be one filter approach used at a time.
        /// </summary>
        /// <param name="namedQuery">The request URL Named Query filter which can also be used when paging.</param>
        /// <returns>The Builder with the criteria filter assigned.</returns>
        public Pager WithNamedQuery( string namedQuery )
        {
            FilterMap = null;
            CriteriaFilter = null;
            NamedQueryFilter = namedQuery;
            QapiRequestBody = null;
            return this;
        }

        /// <summary>
        /// Assigns the specified request URL filter map and returns this builder for fluent API functionality.
        /// Nulls out the criteriaFilter, namedQueryFilter, and qapiRequestBody because there can only be one filter approach used at a time.
        /// </summary>
        /// <param name="filterMap">The request URL filter map which can also be used when paging.</param>
        /// <returns>The Builder with the filter map assigned.</returns>
        public Pager WithFilterMap( string filterMap )
        {
            CriteriaFilter = null;
            NamedQueryFilter = null;
            FilterMap = filterMap;
            QapiRequestBody = null;
            return this;
        }

        /// <summary>
        /// Assigns the specified pageSize and returns this builder for fluent API functionality.
        /// </summary>
        /// <param name="pageSize">The pageSize (number of rows in each response) to page with.</param>
        /// <returns>Pager object.</returns>
        public Pager WithPageSize( int pageSize ) { PageSize = pageSize; return this; }

        /// <summary>
        /// Assigns the specified QAPI request request body and returns this builder for fluent API functionality. 
        /// Nulls out the criteriaFilter, namedQueryFilter and filterMap because there can only be one filter approach used at a time.
        /// </summary>
        /// <param name="qapiRequestBody">The QAPI request body which can also be used when paging.</param>
        public void WithQAPIRequestBodyFilter( string qapiRequestBody )
        {
            CriteriaFilter = null;
            NamedQueryFilter = null;
            FilterMap = null;
            QapiRequestBody = qapiRequestBody;
        }

        /// <summary>
        /// Sets number of pages.
        /// </summary>
        /// <param name="forNumPages"></param>
        /// <returns></returns>
        public Pager ForNumPages( int forNumPages ) { NumPages = forNumPages; return this; }

        /// <summary>
        /// Assigns the specified numRows and returns this builder for fluent API functionality.
        /// </summary>
        /// <param name="forNumRows">The number of rows to page for.  Data is returned in pages up to the specified number of rows.</param>
        /// <returns>Pager object.</returns>
        public Pager ForNumRows( int forNumRows ) { NumRows = forNumRows; return this; }

        /// <summary>
        /// Assigns the specified offset and returns this builder for fluent API functionality.
        /// </summary>
        /// <param name="forOffSet">The offset (row number) to start paging from.</param>
        /// <returns>Pager object.</returns>
        public Pager FromOffSet( int forOffSet ) { Offset = forOffSet; return this; }

        /// <summary>
        /// Assigns the specified PagingType for how to page and returns this builder for fluent API functionality.
        /// </summary>
        /// <param name="pagingType">Must be of the PagingType enumeration defined in the Pager class, and used to determine how to page.</param>
        /// <returns>Pager object.</returns>
        public Pager ForPagerType( PagingType pagingType ) { HowToPage = pagingType; return this; }

        /// <summary>
        /// An inner static Builder class used for building the Pager object with various criteria.  This uses the builder 
        /// fluent API pattern. 
        /// All of the attributes in this Builder class correspond to the attributes in the containing Pager class.
        /// </summary>
        /// <param name="action">Takes an Action delegate.</param>
        /// <example>
        /// <code>
        /// Pager pager = Pager.Build(p =>
        ///	{
        ///		p.ForResource("student-cohorts")
        ///		.WithPageSize(5)
        ///		.ForVersion("12")
        ///		.ForNumPages(10)
        ///		.ForNumRows(5);
        ///	});
        /// </code>
        /// </example>
        /// <returns></returns>
        public static Pager Build( Action<Pager> action )
        {
            Pager pager = new Pager();
            action( pager );
            return pager;
        }
    }
}
