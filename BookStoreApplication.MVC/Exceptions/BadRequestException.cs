// File: BookStoreApplication.Common/Exceptions/BadRequestException.cs
using BookStoreApplication.MVC.Exceptions;
namespace BookStoreApplication.MVC.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }
    }
}
