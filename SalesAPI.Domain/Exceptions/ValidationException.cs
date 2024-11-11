namespace SalesAPI.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("One or more validation failures have occured.")
        {
            Errors = [];
        }
        public List<string> Errors { get; }
        public ValidationException(IEnumerable<string> errors)
            : this()
        {
           Errors.AddRange(errors);
        }

    }
}
