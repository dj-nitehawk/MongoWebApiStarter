using MongoDB.Entities;

namespace App.Biz.Base
{
    public abstract class ModelBase<TRepo> where TRepo : new()
    {
        public TRepo Repo { get; set; } = new TRepo();

        abstract public void Save();

        abstract public void Load();

        public One<TEntity> One<TEntity>(string id) where TEntity : Entity
        {
            return new One<TEntity>() { ID = id };
        }
    }
}
