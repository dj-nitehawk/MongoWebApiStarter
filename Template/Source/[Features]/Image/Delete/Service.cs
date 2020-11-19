using MongoWebApiStarter;
using System.Threading.Tasks;

namespace Image.Delete
{
    public class Service : Service<Request, Nothing>
    {
        public async Task<Nothing> DeleteAsync(Request r)
        {
            await Logic.Image.DeleteAsync(r.ID);
            return Nothing;
        }
    }
}
