using ServiceStack;
using ServiceStack.Validation;
using System.Linq;

namespace MongoWebApiStarter
{
    public static class Validation
    {
        public static void AddExceptionHandler(AppHost appHost)
        {
            appHost.ServiceExceptionHandlers.Add((_, __, x) =>
            {
                if (x is ValidationError ex)
                {
                    return new HttpError(
                        new
                        {
                            errors = ex.Violations
                                       .GroupBy(f => f.FieldName)
                                       .ToDictionary(x => x.Key,
                                                     x => x.Select(e => e.ErrorMessage)),
                            status = 400,
                            title = "One or more validation errors occurred."
                        },
                        400,
                        "Validation Error");
                }

                return null;
            });
        }
    }
}
