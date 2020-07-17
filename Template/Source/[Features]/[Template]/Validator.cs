using ServiceStack.FluentValidation;

namespace NAMESPACE
{
    public class Validator : AbstractValidator<Request>
    {
        private readonly Database Data = new Database();

        public Validator()
        {

        }
    }
}
