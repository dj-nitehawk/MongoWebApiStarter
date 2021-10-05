using System.Net;

namespace MongoWebApiStarter.Middleware
{
    public class MaintenanceMode
    {
        private bool maintenanceMode;
        private readonly string content;
        private readonly RequestDelegate next;

        public MaintenanceMode(RequestDelegate next, IWebHostEnvironment environment)
        {
            this.next = next;
            content = File.ReadAllText(Path.Combine(environment.WebRootPath, "maintenance.htm"));
        }

        public async Task Invoke(HttpContext context)
        {
            switch (context.Request.Path)
            {
                case "/turn-site-off": //todo: protect this route with nginx or disable in production
                    maintenanceMode = true;
                    break;
                case "/turn-site-on": //todo: protect this route with nginx or disable in production
                    maintenanceMode = false;
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<h1>maintenance off...</h1>");
                    return;
            }

            if (maintenanceMode)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(content);
                return;
            }

            await next.Invoke(context);
        }
    }

    public static class MaintenanceModeExtensions
    {
        public static IApplicationBuilder UseMaintenanceModeMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MaintenanceMode>();
        }
    }
}