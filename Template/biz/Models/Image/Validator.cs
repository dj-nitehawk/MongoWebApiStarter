using FluentValidation;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class ImageModel
    {
        public class Validator : AbstractValidator<ImageModel>
        {
            public Validator()
            {
                RuleFor(i => i.Width)
                    .GreaterThan(10).WithMessage("Image width too small")
                    .LessThan(2000).WithMessage("Image width is too large");

                RuleFor(i => i.Height)
                    .GreaterThan(10).WithMessage("Image height too small")
                    .LessThan(2000).WithMessage("Image width is too large"); ;

                RuleFor(i => i.File)
                    .NotEmpty().WithMessage("Image is required")
                    .Must((e, i) => e.IsAllowedType()).WithMessage("File type not allowed")
                    .Must((e, i) => e.IsAllowedSize()).WithMessage("File size not allowed");
            }
        }
    }
}
