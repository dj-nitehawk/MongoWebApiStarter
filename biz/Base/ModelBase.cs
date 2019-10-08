namespace App.Biz.Base
{
    public class ModelBase<TManager> where TManager : new()
    {
        public TManager Manager { get; set; } = new TManager();
    }
}
