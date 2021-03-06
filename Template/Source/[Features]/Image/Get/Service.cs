﻿using MongoWebApiStarter;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace Image.Get
{
    [Authenticate(ApplyTo.None)]
    public class Service : Service<Request, Nothing>
    {
        public async Task GetAsync(Request r)
        {
            HttpResponse.ContentType = "image/jpeg";

            try
            {
                HttpResponse.StatusCode = 200;
                await Data.DownloadAsync(r.ID, HttpResponse.OutputStream);
            }
            catch (Exception)
            {
                HttpResponse.StatusCode = 404;
            }
            finally
            {
                await HttpResponse.EndRequestAsync();
            }
        }
    }
}
