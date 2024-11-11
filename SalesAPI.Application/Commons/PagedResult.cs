namespace SalesAPI.Application.Commons
{
    //public class PagedResult<T> 
    //{
    //    public int PageNumber { get; set; }
    //    public int PageSize { get; set; }
    //    public int Total { get; set; }
    //    public bool HasPrevious => PageNumber > 1;
    //    public bool HasNext => PageSize < Total;

    //    public PagedResult(T data, int pageNumber, int pageSize, int total, string message = null)
    //    {
    //        PageNumber = pageNumber;
    //        PageSize = pageSize;
    //        Total = total;
    //        Data = data;
    //        Message = message;
    //        Succeeded = true;
    //        Errors = null;

    //    }
    //    public PagedResult(string message, List<string> errors = null)
    //    {
    //        Message = message;
    //        Succeeded = false;
    //        Errors = errors;
    //    }

    //    public static PagedResult<T> Success(T data, int pageNumber, int pageSize, int total, string message = null)
    //    {
    //        message = message ?? $"{total} record(s) found.";
    //        return new PagedResult<T>(data, pageNumber, pageSize, total, message);
    //    }
    //    public static PagedResult<T> Failure(string message, List<string> errors = null)
    //    {
    //        return new PagedResult<T>(message, errors);
    //    }
    //}
}
