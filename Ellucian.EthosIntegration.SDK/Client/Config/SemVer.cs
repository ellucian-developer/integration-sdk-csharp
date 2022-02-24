/*
 * ******************************************************************************
 *   Copyright  2020 Ellucian Company L.P. and its affiliates.
 * ******************************************************************************
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace Ellucian.Ethos.Integration.Config
{
    /// <summary>
    /// The SemVer class holds the semantic version for an Ethos resource. Implements the IComparable interface to enable
    /// easy sorting of this class by major/minor/patch values.
    /// <p/>
    /// NOTE: Not all versions of Ethos resources are Semantic versions. Therefore, this class should only be used in support of
    /// those versions of resources following Semantic version notation.
    /// </summary>
    public class SemVer: IComparable<SemVer>
    {
        /// <summary>
        /// The major version.
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// The minor version.
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// The patch version.
        /// </summary>
        public int Patch { get; set; }

        /// <summary>
        /// An inner static Builder class used for building the SemVer object with various criteria. This uses the builder
        /// fluent API pattern.
        /// All of the attributes in this Builder class correspond to the attributes in the containing SemVer class.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// The major version.
            /// </summary>
            private int major;

            /// <summary>
            /// The minor version.
            /// </summary>
            private int minor;

            /// <summary>
            /// The patch version.
            /// </summary>
            private int patch;

            /// <summary>
            /// No-arg constructor for the Builder. Sets all version values to 0.
            /// </summary>
            public Builder()
            {
                this.major = 0;
                this.minor = 0;
                this.patch = 0;
            }

            /// <summary>
            /// A Builder constructor that takes a version string which should have a value in SemVer notation,
            /// e.g. 12.0.1, or 11, or 9.2. Attempts to parse the version into the appropriate major, minor, and/or patch values.
            /// </summary>
            /// <param name="version">The version value in SemVer notation to parse into the major, minor, and/or patch values.</param>
            public Builder( string version ) : base()
            {
                if ( string.IsNullOrWhiteSpace( version ) )
                {
                    return;
                }
                if ( version.StartsWith( "v" ) )
                {
                    version = version.Substring( 1 );
                }
                if ( version.Contains( "." ) == false )
                {
                    this.major = int.Parse( version );
                }
                else
                {
                    this.major = int.Parse( version.Substring( 0, version.IndexOf( "." ) ) );
                    version = version.Substring( version.IndexOf( "." ) + 1 );
                    if ( !version.Contains( "." ) )
                    {
                        this.minor = int.Parse( version );
                    }
                    else
                    {
                        this.minor = int.Parse( version.Substring( 0, version.IndexOf( "." ) ) );
                        version = version.Substring( version.IndexOf( "." ) + 1 );
                        if ( !string.IsNullOrWhiteSpace( version ) )
                        {
                            this.patch = int.Parse( version );
                        }
                    }
                }
            }

            /// <summary>
            /// Sets the given major value for the version in the builder.
            /// </summary>
            /// <param name="major">The major value to set.</param>
            /// <returns>This builder instance with the major value set.</returns>
            public Builder WithMajor( int major )
            {
                this.major = major;
                return this;
            }

            /// <summary>
            /// Sets the given minor value for the version in the builder.
            /// </summary>
            /// <param name="minor">The minor value to set.</param>
            /// <returns>This builder instance with the minor value set.</returns>
            public Builder WithMinor( int minor )
            {
                this.minor = minor;
                return this;
            }

            /// <summary>
            /// Sets the given patch value for the version in the builder.
            /// </summary>
            /// <param name="patch">The patch value to set.</param>
            /// <returns>This builder instance with the patch value set.</returns>
            public Builder WithPatch( int patch )
            {
                this.patch = patch;
                return this;
            }

            /// <summary>
            /// Builds a SemVer object with the given major, minor, and patch values that have been set in the builder.
            /// </summary>
            /// <returns>A SemVer object containing the major, minor, and patch values from the builder.</returns>
            public SemVer Build()
            {
                SemVer semVer = new SemVer();
                semVer.Major = this.major;
                semVer.Minor = this.minor;
                semVer.Patch = this.patch;
                return semVer;
            }
        }

        /// <summary>
        /// Compares this object to the given object, which should also be an instance of this class.
        /// Compares by the major, then minor, then patch values.
        /// Returns the difference between the major, minor, and patch values.If the difference of the major values is 0,
        /// then takes the difference of the minor values.If the difference of the minor values is also 0, then it returns
        /// the difference of the patch values.
        /// </summary>
        /// <param name="sv">Another SemVer instance to compare against this one.</param>
        /// <returns>
        /// A negative value if this SemVer is less than the given SemVer object, a positive value if this SemVer is greater
        /// than the given SemVer object, or 0 if they are equal.
        /// </returns>
        public int CompareTo( [AllowNull] SemVer sv = null )
        {
            if ( sv == null )
            {
                return 1;
            }

            int result = this.Major - sv.Major;
            if ( result == 0 )
            {
                result = this.Minor - sv.Minor;
                if ( result == 0 )
                {
                    result = this.Patch - sv.Patch;
                }
            }
            return result;
        }

        /// <summary>
        /// Overrides the equals method to compare on the major, minor, and patch values.
        /// </summary>
        /// <param name="o">The object to compare equality against, expected to be another instance of this class.</param>
        /// <returns>true if the given SemVer object has the same major, minor, and patch values. False otherwise.</returns>
        public override bool Equals( Object o )
        {
            if ( !( o is SemVer ) )
            {
                return false;
            }
            if ( this == o )
            {
                return true;
            }
            SemVer otherSemVer = ( SemVer ) o;
            if ( this.Major == otherSemVer.Major &&
                this.Minor == otherSemVer.Minor &&
                this.Patch == otherSemVer.Patch )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Overrides the hashCode to compute and return the hashCode value comprised of the major, minor, and patch values
        /// of this class.
        /// </summary>
        /// <returns>A hash code computed from the major, minor, and patch values of this class.</returns>
        public override int GetHashCode()
        {
            // Use a value tuple to combine the three fields, then get that
            // hash-code. There are other ways to do this but dotnetcore has one way
            // and .NET Framework has another. In order to stay compatible we go this way, which
            // will work with both.
            return (Major, Minor, Patch).GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of this object containing the SemVer notation of the major.minor.patch values.
        /// </summary>
        /// <returns>This object as a semantic version string such as 5.24.12 or 6.23.14.</returns>
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }
    }
}