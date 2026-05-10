// File: BookStoreApplication.Common/Exceptions/UnauthorizedException.cs

namespace BookStoreApplication.Web.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(message, 401)
        {
        }
    }
}