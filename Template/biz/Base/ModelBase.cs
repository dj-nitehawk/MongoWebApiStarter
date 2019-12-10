using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq.Expressions;

namespace MongoWebApiStarter.Biz.Base
{
    /// <summary>
    /// Base class for models
    /// </summary>
    /// <typeparam name="TRepo">The type of repo for the model</typeparam>
    public abstract class ModelBase<TRepo> where TRepo : new()
    {
        protected TRepo Repo { get; set; } = new TRepo();

        private ModelStateDictionary modelState = new ModelStateDictionary();

        public abstract void Save();

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

        public bool HasErrors() => !modelState.IsValid;

        public ModelStateDictionary Errors() => modelState;
    }
}
