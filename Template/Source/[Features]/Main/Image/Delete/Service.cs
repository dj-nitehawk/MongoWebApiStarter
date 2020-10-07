using MongoWebApiStarter;
using System.Threading.Tasks;

namespace Main.Image.Delete
{
    public class Service : Service<Request, Nothing, Database>
    {
        public async Task<Nothing> Delete(Request r)
        {
            await Logic.Image.DeleteAsync(r.ID);
            return Nothing;
        }
    }
}
