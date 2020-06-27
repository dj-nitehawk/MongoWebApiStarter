using Data;
using MongoWebApiStarter;

namespace Main.Image.Delete
{
    public class Service : Service<Request, Nothing, Data.Image>
    {
        public override Nothing Any(Request r)
        {
            _ = RepoImage.DeleteAsync(r.ID);

            return Nothing;
        }
    }
}
