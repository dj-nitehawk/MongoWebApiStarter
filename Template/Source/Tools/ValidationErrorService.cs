using ServiceStack;
using ServiceStack.Validation;
using System.Linq;

namespace SCVault.Tools
{
    public static class ValidationErrorService
    {
        public static HttpError GetErrorResponse(ValidationError ex)
        {
            var errors = ex.Violations
                .GroupBy(f => f.FieldName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

            var valErrors = new
            {
                errors,
                status = 400,
                title = "One or more validation errors occurred."
            }.SerializeToString();

            string genErrors = default;

            if (errors.ContainsKey("GeneralErrors"))
                genErrors = errors.SerializeToString();

            return new HttpError(
                genErrors == default ? valErrors : genErrors, //todo: this is only for SCV. remove in template project
                400,
                "Validation Error");
        }
    }
}
