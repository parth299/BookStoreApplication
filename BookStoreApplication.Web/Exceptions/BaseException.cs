// File: BookStoreApplication.Common/Exceptions/BaseException.cs

namespace BookStoreApplication.Web.Exceptions
{
    public abstract class BaseException : Exception
    {
        public int StatusCode { get; set; }

        protected BaseException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
