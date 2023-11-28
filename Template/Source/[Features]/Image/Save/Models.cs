namespace Image.Save;

sealed class Request
{
    public string? ID { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public IFormFile File { get; set; }
}

sealed class Validator : Validator<Request>
{
    static readonly string[] _mimeTypeWhitelist =
    {
        "image/jpeg",
        "image/png"
    };

    public Validator()
    {
        RuleFor(x => x.Width)
            .GreaterThan(10).WithMessage("Image width too small")
            .LessThan(2000).WithMessage("Image width is too large");

        RuleFor(x => x.Height)
            .GreaterThan(10).WithMessage("Image height too small")
            .LessThan(2000).WithMessage("Image width is too large");

        RuleFor(x => x.File)
            .NotEmpty().WithMessage("File is required!");

        RuleFor(x => x.File)
            .Must(f => IsAllowedType(f.ContentType)).WithMessage("Invalid image type!")
            .Must(f => IsAllowedSize(f.Length)).WithMessage("File is too small!");
    }

    static bool IsAllowedType(string contentType)
        => _mimeTypeWhitelist.Contains(contentType.ToLower());

    static bool IsAllowedSize(long fileLength)
        => fileLength is >= 100 and <= 10485760;
}