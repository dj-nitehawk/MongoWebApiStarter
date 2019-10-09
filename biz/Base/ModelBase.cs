namespace App.Biz.Base
{
    public class ModelBase<TRepo> where TRepo : new()
    {
        public TRepo Repo { get; set; } = new TRepo();
    }
}
