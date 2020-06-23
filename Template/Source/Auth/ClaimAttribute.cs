using ServiceStack;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Auth
{
    /// <summary>
    /// Use this attribute to only allow users who has all the given ClaimTypes
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NeedAttribute : AuthenticateAttribute
    {
        private readonly string[] claims;

        /// <summary>
        /// Restrict users by ClaimType
        /// </summary>
        /// <param name="applyTo">Specify which http verbs this applies to</param>
        /// <param name="claimTypes">The claim types the uses must have in order to be allowed access</param>
        public NeedAttribute(ApplyTo applyTo, params string[] claimTypes)
        {
            claims = claimTypes;
            ApplyTo = applyTo;
            Priority = (int)RequestFilterPriority.RequiredRole;
        }

        /// <summary>
        /// Restrict users by ClaimType
        /// </summary>
        /// <param name="claimTypes">The claim types the uses must have in order to be allowed access</param>
        public NeedAttribute(params string[] claimTypes)
            : this(ApplyTo.All, claimTypes) { }

        public override async Task ExecuteAsync(ServiceStack.Web.IRequest req, ServiceStack.Web.IResponse res, object requestDto)
        {
            if (HostContext.AppHost.HasValidAuthSecret(req))
                return;

            await base.ExecuteAsync(req, res, requestDto); //first check if session is authenticated
            if (res.IsClosed)
                return; //AuthenticateAttribute already closed the request (ie auth failed)

            if (claims.Except(req.GetClaims().Select(c => c.Type)).Any())
                return;

            if (DoHtmlRedirectAccessDeniedIfConfigured(req, res))
                return;

            res.StatusCode = (int)HttpStatusCode.Forbidden;
            res.StatusDescription = ErrorMessages.ClaimDoesNotExistFmt.Fmt(claims).Localize(req);
            await HostContext.AppHost.HandleShortCircuitedErrors(req, res, requestDto);
        }

        protected bool Equals(NeedAttribute other)
        {
            return base.Equals(other) && string.Equals(claims, other.claims);
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
