using FastEndpoints.Validation;

namespace Image.Save;

public class Request : IRequest<Dom.Image>
{
    public string? ID { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Dom.Image ToEntity()
    {
        return new Dom.Image
        {
            Height = Height,
            Width = Width,
            ID = ID
        };
    }
}

public class Validator : Validator<Request>
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

