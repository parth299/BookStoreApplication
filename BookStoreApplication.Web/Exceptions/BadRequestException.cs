// File: BookStoreApplication.Common/Exceptions/BadRequestException.cs
using BookStoreApplication.Web.Exceptions;
namespace BookStoreApplication.Web.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }
    }
}
