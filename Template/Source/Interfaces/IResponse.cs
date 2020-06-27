using ServiceStack;

namespace MongoWebApiStarter
{
    /// <summary>
    /// A response DTO to use when there's nothing to return from a service method
    /// </summary>
    public class Nothing : IResponse, IReturnVoid
    {
        public string ResultStatus { get; } = "Successful!";
    }

    /// <summary>
    /// Marker interface for response DTOs
    /// </summary>
    public interface IResponse { }
}
