namespace BookStoreApplication.MVC.Exceptions.RatingsAndReviewers
{
    public abstract class ApiException : Exception
    {
        public int StatusCode { get; }

        protected ApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
