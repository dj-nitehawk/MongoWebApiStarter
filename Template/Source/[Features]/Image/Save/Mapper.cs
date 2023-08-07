namespace Image.Save;

internal sealed class Mapper : Mapper<Request, object, Dom.Image>
{
    public override Dom.Image ToEntity(Request r) => new()
    {
        Height = r.Height,
        Width = r.Width,
        ID = r.ID
    };
}