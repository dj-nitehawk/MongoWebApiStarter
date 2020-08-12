using MongoWebApiStarter;

namespace Main.Image.Delete
{
    public class Service : Service<Request, Nothing, Database>
    {
        public Nothing Delete(Request r)
        {
            Dom.Image.Delete(r.ID);
            return Nothing;
        }
    }
}
