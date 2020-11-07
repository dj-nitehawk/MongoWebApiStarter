using ServiceStack;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Auth
{
    /// <summary>
    /// Use this attribute to only allow users who has the given ClaimTypes
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NeedAttribute : AuthenticateAttribute
    {
        private readonly string[] claims;
        private readonly bool allowAny;

        /// <summary>
        /// Restrict users by ClaimType
        /// </summary>
        /// <param name="applyTo">Specify which http verbs this applies to</param>
        /// <param name="claimTypes">All the claim types the user must have in order to be allowed access</param>
        public NeedAttribute(ApplyTo applyTo, params string[] claimTypes)
        {
            claims = claimTypes;
            ApplyTo = applyTo;
            Priority = (int)RequestFilterPriority.RequiredRole;
        }

        /// <summary>
        /// Restrict users by ClaimType
        /// </summary>
        /// <param name="claimTypes">All the claim types the user must have in order to be allowed access</param>
        public NeedAttribute(params string[] claimTypes)
        {
            claims = claimTypes;
            ApplyTo = ApplyTo.All;
            Priority = (int)RequestFilterPriority.RequiredRole;
        }

        /// <summary>
        /// Restrict users by ClaimType
        /// </summary>
        /// <param name="anyClaim">Specify if having at least one of the claim types allows access</param>
        /// <param name="claimTypes">The claim types</param>
        public NeedAttribute(bool anyClaim, params string[] claimTypes)
        {
            claims = claimTypes;
            ApplyTo = ApplyTo.All;
            Priority = (int)RequestFilterPriority.RequiredRole;
            allowAny = anyClaim;
        }

        /// <summary>
        /// Restrict users by ClaimType
        /// </summary>
        /// <param name="applyTo">Specify which http verbs this applies to</param>
        /// <param name="anyClaim">Specify if having at least one of the claim types allows access</param>
        /// <param name="claimTypes">The claim types</param>
        public NeedAttribute(ApplyTo applyTo, bool anyClaim, params string[] claimTypes)
        {
            claims = claimTypes;
            ApplyTo = applyTo;
            Priority = (int)RequestFilterPriority.RequiredRole;
            allowAny = anyClaim;
        }

        public override async Task ExecuteAsync(ServiceStack.Web.IRequest req, ServiceStack.Web.IResponse res, object requestDto)
        {
            if (HostContext.AppHost.HasValidAuthSecret(req))
                return;

            await base.ExecuteAsync(req, res, requestDto); //first check if session is authenticated

            if (res.IsClosed)
                return; //AuthenticateAttribute already closed the request (ie auth failed)

            var userClaims = req.SessionAs<UserSession>().Claims;

            if ((allowAny && claims.Intersect(userClaims.Select(c => c.Key)).Any()) || //user has at least one required claim when allowAny is true or
                (!allowAny && !claims.Except(userClaims.Select(c => c.Key)).Any())) //user has all required claims when allowAny is false
            {
                foreach (var claimType in claims) // populate values of public fields if field name matches a ClaimType
                {
                    var field = requestDto.GetType().GetField(
                        claimType,
                        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                    if (field != null && userClaims.TryGetValue(claimType, out string claimValue))
                    {
                        field.SetValue(requestDto, claimValue);
                    }
                }

                return;
            }

            if (DoHtmlRedirectAccessDeniedIfConfigured(req, res))
                return;

            res.StatusCode = (int)HttpStatusCode.Forbidden;
            res.StatusDescription = "The user doesn't have one or more required claims to access this resource!";
            await HostContext.AppHost.HandleShortCircuitedErrors(req, res, requestDto);
        }

        protected bool Equals(NeedAttribute other)
        {
            return base.Equals(other) && Equals(claims, other.claims);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NeedAttribute)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), claims);
        }
    }
}
