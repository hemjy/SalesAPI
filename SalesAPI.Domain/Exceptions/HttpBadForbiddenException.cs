namespace SalesAPI.Domain.Exceptions
{
    public class HttpBadForbiddenException(string message) : Exception(message)
    {
    }
}
