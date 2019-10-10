namespace MongoWebApiStarter.Biz.Base
{
    public abstract class ViewBase<TRepo> : ModelBase<TRepo> where TRepo : new() {
        public override void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
