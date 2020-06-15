using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoWebApiStarter.Data.Base;
using System;
using System.Linq.Expressions;

namespace MongoWebApiStarter.Biz.Base
{
    /// <summary>
    /// Base class for models
    /// </summary>
    /// <typeparam name="TRepo">The type of repo for the model</typeparam>
    public abstract class ModelBase<TRepo> where TRepo : IRepo, new()
    {
        protected TRepo Repo { get; set; } = new TRepo();

        private readonly ModelStateDictionary modelState = new ModelStateDictionary();

        /// <summary>
        /// The logic for saving the model
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// The logic for loading the model
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// Add a model state error for a specific property
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <param name="targetProperty">Lambda expression for the property. x => x.PropName</param>
        /// <param name="errorMessage">The error description</param>
        protected void AddError<TModel>(Expression<Func<TModel, object>> targetProperty, string errorMessage)
        {
            modelState.AddModelError(targetProperty, errorMessage);
        }

        /// <summary>
        /// Add a common model state error
        /// </summary>
        /// <param name="errorMessage">The error description</param>
        protected void AddError(string errorMessage)
        {
            modelState.AddModelError("GeneralErrors", errorMessage);
        }

        /// <summary>
        /// Returns true if modelstate is not valid
        /// </summary>
        /// <returns></returns>
        public bool HasErrors() => !modelState.IsValid;

        /// <summary>
        /// Returns the model state dictionary
        /// </summary>
        /// <returns></returns>
        public ModelStateDictionary Errors() => modelState;
    }
}
