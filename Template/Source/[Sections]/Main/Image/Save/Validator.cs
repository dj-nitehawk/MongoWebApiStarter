using ServiceStack.FluentValidation;

namespace Main.Image.Save
{
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(i => i.Width)
                .GreaterThan(10).WithMessage("Image width too small")
                .LessThan(2000).WithMessage("Image width is too large");

            RuleFor(i => i.Height)
                .GreaterThan(10).WithMessage("Image height too small")
                .LessThan(2000).WithMessage("Image width is too large");
        }
    }
}
