/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using Ellucian.Ethos.Integration.Config;

using Xunit;

namespace Ellucian.Ethos.Integration.Test.Client.Config
{
    public class SemVerTests
    {
        [Fact]
        public void SemVerTest()
        {
            SemVer ver = new SemVer.Builder( "7.1.5" ).Build();
            Assert.NotNull( ver );
            Assert.Equal( 7, ver.Major );
            Assert.Equal( 1, ver.Minor );
            Assert.Equal( 5, ver.Patch );
        }

        [Fact]
        public void WithMajorTest()
        {
            SemVer ver = new SemVer.Builder().WithMajor( 7 ).Build();
            Assert.NotNull( ver );
            Assert.Equal( 7, ver.Major );
            Assert.Equal( 0, ver.Minor );
            Assert.Equal( 0, ver.Patch );
        }

        [Fact]
        public void WithMinorTest()
        {
            SemVer ver = new SemVer.Builder().WithMinor( 5 ).Build();
            Assert.NotNull( ver );
            Assert.Equal( 0, ver.Major );
            Assert.Equal( 5, ver.Minor );
            Assert.Equal( 0, ver.Patch );
        }

        [Fact]
        public void WithPatchTest()
        {
            SemVer ver = new SemVer.Builder().WithPatch( 6 ).Build();
            Assert.NotNull( ver );
            Assert.Equal( 0, ver.Major );
            Assert.Equal( 0, ver.Minor );
            Assert.Equal( 6, ver.Patch );
        }

        [Fact]
        public void CompareTo()
        {
            SemVer ver = new SemVer.Builder( "7.1.5" ).Build();
            SemVer ver2 = new SemVer.Builder( "7.0.5" ).Build();

            var result = ver.CompareTo( ver2 );
            Assert.True( result >= 1 );
        }

        [Fact]
        public void CompareTo_Null()
        {
            SemVer ver = new SemVer.Builder( "7.1.5" ).Build();

            var result = ver.CompareTo( null );
            Assert.True( result >= 1 );
        }

        [Fact]
        public void Equals_True()
        {
            SemVer ver = new SemVer.Builder( "7.1.5" ).Build();
            SemVer ver2 = new SemVer.Builder( "7.1.5" ).Build();

            var result = ver.Equals( ver2 );
            Assert.True( result );
        }

        [Fact]
        public void Equals_False()
        {
            SemVer ver = new SemVer.Builder( "7.1.5" ).Build();
            SemVer ver2 = new SemVer.Builder( "7" ).Build();

            var result = ver.Equals( ver2 );
            Assert.False( result );
        }

        [Fact]
        public void HashCode()
        {
            var ver = new SemVer.Builder( "7.1.5" ).Build();
            var hashCode = ver.GetHashCode();
            Assert.NotEqual<int>( 0, hashCode );
        }

        [Fact]
        public void ToString_Test()
        {
            var ver = new SemVer.Builder( "7.1.5" ).Build();
            var verString = ver.ToString();
            Assert.Equal( "7.1.5", verString );
        }
    }
}
