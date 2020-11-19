using System;

namespace MongoWebApiStarter.Auth
{
    /// <summary>
    /// Use this attribute to automatically bind Claim Values to properties of DTO classes from the JWT token.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FromAttribute : Attribute
    {
        /// <summary>
        /// The claim type to get the value for from the JWT token
        /// </summary>
        public string ClaimType { get; }

        /// <summary>
        /// Specify the claim type to bind the value of to the property 
        /// </summary>
        /// <param name="claimType">The claim type to get the value of from the JWT token</param>
        public FromAttribute(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
