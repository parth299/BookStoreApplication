namespace BookStoreApplication.Web.Exceptions.RatingsAndReviewers
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message, StatusCodes.Status404NotFound)
        {
        }
    }
}
