namespace BookStoreApplication.Web.Exceptions.RatingsAndReviewers
{
    public class RequestValidationException : ApiException
    {
        public Dictionary<string, string[]> Errors { get; }

        public RequestValidationException(Dictionary<string, string[]> errors) : base("Validation failed.", StatusCodes.Status400BadRequest)
        {
            Errors = errors;
        }
    }
}
