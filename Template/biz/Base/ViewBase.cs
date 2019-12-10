namespace MongoWebApiStarter.Biz.Base
{
    /// <summary>
    /// Base class for views
    /// </summary>
    /// <typeparam name="TRepo">The type of repo for the view</typeparam>
    public abstract class ViewBase<TRepo> where TRepo : new()
    {
        protected TRepo Repo { get; set; } = new TRepo();
        public abstract void Load();
    }
}
