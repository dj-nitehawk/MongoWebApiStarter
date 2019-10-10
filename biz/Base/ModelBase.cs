namespace MongoWebApiStarter.Biz.Base
{
    public abstract class ModelBase<TRepo> where TRepo : new()
    {
        public TRepo Repo { get; set; } = new TRepo();
        abstract public void Save();
        abstract public void Load();
    }
}
