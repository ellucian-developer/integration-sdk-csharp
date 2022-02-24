/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Client.Proxy;

using Xunit;

namespace Ellucian.Ethos.Integration.Test
{

    public class PagerTests
    {
        [Fact]
        public void Pager_Check_Default_Value_NullCheck()
        {
            Pager pager = Pager.Build( p => { } );
            Assert.NotNull( pager );
        }

        [Fact]
        public void Pager_Check_Resource_NullCheck()
        {
            Pager pager = Pager.Build( p =>
            {
                p.ForResource( "student-cohorts" );
            } );
            Assert.NotNull( pager );
            Assert.NotNull( pager.ResourceName );
            Assert.Equal( "student-cohorts", pager.ResourceName );
        }

        [Fact]
        public void Pager_Check_PageSize_NullCheck()
        {
            Pager pager = Pager.Build( p =>
            {
                p.ForResource( "student-cohorts" )
                .WithPageSize( 15 );
            } );
            Assert.NotNull( pager );
            Assert.Equal( "student-cohorts", pager.ResourceName );
            Assert.Equal( 15, pager.PageSize );
        }

        [Fact]
        public void Pager_Check_Version_NullCheck()
        {
            Pager pager = Pager.Build( p =>
            {
                p.ForResource( "student-cohorts" )
                .WithPageSize( 15 )
                .ForVersion( "10" );
            } );
            Assert.NotNull( pager );
            Assert.Equal( "student-cohorts", pager.ResourceName );
            Assert.Equal( 15, pager.PageSize );
            Assert.Equal( "10", pager.Version );
        }

        [Fact]
        public void Pager_Check_NumPages_NullCheck()
        {
            Pager pager = Pager.Build( p =>
            {
                p.ForResource( "student-cohorts" )
                .WithPageSize( 15 )
                .ForVersion( "10" )
                .ForNumPages( 4 );
            } );
            Assert.NotNull( pager );
            Assert.Equal( "student-cohorts", pager.ResourceName );
            Assert.Equal( 15, pager.PageSize );
            Assert.Equal( "10", pager.Version );
            Assert.Equal( 4, pager.NumPages );
        }

        [Fact]
        public void Pager_Check_NumRows_NullCheck()
        {
            Pager pager = Pager.Build( p =>
            {
                p.ForResource( "student-cohorts" )
                .WithPageSize( 15 )
                .ForVersion( "10" )
                .ForNumPages( 4 )
                .ForNumRows( 2 );
            } );
            Assert.NotNull( pager );
            Assert.Equal( "student-cohorts", pager.ResourceName );
            Assert.Equal( 15, pager.PageSize );
            Assert.Equal( "10", pager.Version );
            Assert.Equal( 4, pager.NumPages );
            Assert.Equal( 2, pager.NumRows );
        }

        [Fact]
        public void Pager_Check_Offset_NullCheck()
        {
            Pager pager = Pager.Build( p =>
            {
                p.ForResource( "student-cohorts" )
                .WithPageSize( 15 )
                .ForVersion( "10" )
                .ForNumPages( 4 )
                .ForNumRows( 2 )
                .FromOffSet( 100 );
            } );
            Assert.NotNull( pager );
            Assert.Equal( "student-cohorts", pager.ResourceName );
            Assert.Equal( 15, pager.PageSize );
            Assert.Equal( "10", pager.Version );
            Assert.Equal( 4, pager.NumPages );
            Assert.Equal( 2, pager.NumRows );
            Assert.Equal( 100, pager.Offset );
        }

        [Fact]
        public void Pager_Check_PagerType_NullCheck()
        {
            Pager pager = Pager.Build( p =>
            {
                p.ForResource( "student-cohorts" )
                .WithPageSize( 15 )
                .ForVersion( "10" )
                .ForNumPages( 4 )
                .ForNumRows( 2 )
                .FromOffSet( 100 )
                .ForPagerType( Pager.PagingType.PageFromOffsetForNumPages );
            } );
            Assert.NotNull( pager );
            Assert.Equal( "student-cohorts", pager.ResourceName );
            Assert.Equal( 15, pager.PageSize );
            Assert.Equal( "10", pager.Version );
            Assert.Equal( 4, pager.NumPages );
            Assert.Equal( 2, pager.NumRows );
            Assert.Equal( 100, pager.Offset );
            Assert.Equal( Pager.PagingType.PageFromOffsetForNumPages, pager.HowToPage );
        }
    }
}
