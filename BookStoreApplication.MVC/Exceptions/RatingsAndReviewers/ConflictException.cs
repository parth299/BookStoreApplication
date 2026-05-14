namespace BookStoreApplication.MVC.Exceptions.RatingsAndReviewers
{
    public class ConflictExceptionReview : ApiException
    {
        public ConflictExceptionReview(string message) : base(message, StatusCodes.Status409Conflict)
        {
        }
    }
}
