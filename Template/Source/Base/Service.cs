using MongoDB.Entities.Core;
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
    /// <typeparam name="TEntity">The type of main data entity of the service</typeparam>
    [Authenticate]
    public abstract class Service<TRequest, TResponse, TEntity> : Service
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse, new()
        where TEntity : Entity
    {
        /// <summary>
        /// The app settings object
        /// </summary>
        public Settings Settings { get; set; }

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

        ///// <summary>
        ///// The handler logic for any type of http verb
        ///// </summary>
        ///// <param name="r">The request DTO</param>
        public virtual TResponse Any(TRequest r) => throw new NotImplementedException();

        ///// <summary>
        ///// Handler for GET requests
        ///// </summary>
        ///// <param name="r">The request DTO</param>
        //public virtual TResponse Get(TRequest r) => throw new NotImplementedException();

        ///// <summary>
        ///// Handler for POST requests
        ///// </summary>
        ///// <param name="r">The request DTO</param>
        //public virtual TResponse Post(TRequest r) => throw new NotImplementedException();

        ///// <summary>
        ///// Handler for PUT requests
        ///// </summary>
        ///// <param name="r">The request DTO</param>
        //public virtual TResponse Put(TRequest r) => throw new NotImplementedException();

        ///// <summary>
        ///// Handler for PATCH requests
        ///// </summary>
        ///// <param name="r">The request DTO</param>
        //public virtual TResponse Patch(TRequest r) => throw new NotImplementedException();

        ///// <summary>
        ///// Handler for DELETE requests
        ///// </summary>
        ///// <param name="r">The request DTO</param>
        //public virtual TResponse Delete(TRequest r) => throw new NotImplementedException();

        /// <summary>
        /// Convert a request DTO to a data entity
        /// </summary>
        /// <param name="r">The request DTO</param>
        protected virtual TEntity ToEntity(TRequest r) => throw new NotImplementedException();

        /// <summary>
        /// Convert a data entity to a response DTO
        /// </summary>
        /// <param name="e">The request DTO</param>
        protected virtual TResponse ToResponse(TEntity e) => throw new NotImplementedException();

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
