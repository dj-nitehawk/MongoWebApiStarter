using MongoWebApiStarter.Auth;
using ServiceStack;
using ServiceStack.Validation;
using System;
using System.Linq.Expressions;

namespace MongoWebApiStarter
{
    /// <summary>
    /// Abstract class for services
    /// </summary>
    /// <typeparam name="TRequest">The type of the request DTO</typeparam>
    /// <typeparam name="TResponse">The type of the response DTO</typeparam>
    [Authenticate]
    public abstract class Service<TRequest, TResponse, TDatabase> : Service
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse, new()
        where TDatabase : IDatabase, new()
    {
        /// <summary>
        /// The app settings object
        /// </summary>
        public Settings Settings { get; set; }

        protected TDatabase Data { get; set; } = new TDatabase();

        /// <summary>
        /// The response DTO for the current service
        /// </summary>
        protected new TResponse Response { get; set; } = new TResponse();

        /// <summary>
        /// The underlying http response object
        /// </summary>
        protected ServiceStack.Web.IResponse HttpResponse => base.Response;

        /// <summary>
        /// A void respoinse DTO object
        /// </summary>
        protected Nothing Nothing => new Nothing();

        /// <summary>
        /// The user session of the current request
        /// </summary>
        protected UserSession User => Request.SessionAs<UserSession>();

        /// <summary>
        /// The base url of the current request
        /// </summary>
        protected string BaseURL => Request.GetBaseUrl() + "/";

        private readonly ValidationError error = new ValidationError("Validation Error", "There were some problems!");

        /// <summary>
        /// Check if teh current user has a given permission
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        protected bool HasPermission(Allow permission) =>
            Request.SessionAs<UserSession>()
                   .Permissions
                   .Contains(permission.ToString("D"));

        /// <summary>
        /// Add a validation error to the collection
        /// </summary>
        /// <param name="message">A general error message to add</param>
        protected void AddError(string message)
        {
            error.Violations.Add(new ValidationErrorField("ERROR", "GeneralErrors", message));
        }

        /// <summary>
        /// Add a validation error to the collection
        /// </summary>
        /// <param name="property">x => x.PropName</param>
        /// <param name="message">An error message to add</param>
        protected void AddError(Expression<Func<TRequest, object>> property, string message)
        {
            var exp = (MemberExpression)property.Body;

            if (exp == null) throw new ArgumentException("Please supply a valid member expression!");

            error.Violations.Add(new ValidationErrorField("ERROR", exp.Member.Name, message));
        }

        /// <summary>
        /// Check if there are any errors in the collection and throw an exception if any exists
        /// </summary>
        protected void ThrowIfAnyErrors()
        {
            if (error.Violations.Count > 0)
                throw error;
        }

        /// <summary>
        /// Throw an exception with the given error message
        /// </summary>
        /// <param name="message">A general error message to add</param>
        protected void ThrowError(string message)
        {
            AddError(message);
            ThrowIfAnyErrors();
        }

        /// <summary>
        /// Throw an exception with the given error message
        /// </summary>
        /// <param name="property">x => x.PropName</param>
        /// <param name="message">A general error message to add</param>
        protected void ThrowError(Expression<Func<TRequest, object>> property, string message)
        {
            AddError(property, message);
            ThrowIfAnyErrors();
        }
    }
}
