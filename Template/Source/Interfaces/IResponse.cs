using ServiceStack;

namespace SCVault
{
    /// <summary>
    /// A response DTO to use when there's nothing to return from a service method
    /// </summary>
    public class Nothing : IResponse, IReturnVoid { }

    /// <summary>
    /// Marker interface for response DTOs
    /// </summary>
    public interface IResponse { }
}
