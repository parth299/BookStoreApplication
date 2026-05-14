// File: BookStoreApplication.Common/Exceptions/UnauthorizedException.cs

namespace BookStoreApplication.MVC.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(message, 401)
        {
        }
    }
}
