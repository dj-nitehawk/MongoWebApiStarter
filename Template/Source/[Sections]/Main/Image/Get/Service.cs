using Data;
using SCVault;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace Main.Image.Get
{
    [Authenticate(ApplyTo.None)]
    public class Service : Service<Request, Nothing, Data.Image>
    {
        public new async Task Get(Request r)
        {
            HttpResponse.ContentType = "image/jpeg";

            try
            {
                HttpResponse.StatusCode = 200;
                await RepoImage.DownloadAsync(r.ID, HttpResponse.OutputStream);
            }
            catch (Exception)
            {
                HttpResponse.StatusCode = 404;
            }
            finally
            {
                HttpResponse.EndRequest();
            }
        }
    }
}
