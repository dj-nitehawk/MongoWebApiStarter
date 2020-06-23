using ServiceStack;

namespace SCVault
{
    /// <summary>
    /// Marker interface for request DTOs
    /// </summary>
    /// <typeparam name="TResponse">The type of the response DTO associated with the reqeust</typeparam>
    public interface IRequest<TResponse> : IReturn<TResponse> { }
}
