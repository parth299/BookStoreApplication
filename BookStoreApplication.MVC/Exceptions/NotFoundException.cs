namespace BookStoreApplication.MVC.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, 404)
        {
        }
    }
}
