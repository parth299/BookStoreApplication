using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookStoreApplication.Web.Validators.Author
{
    /// <summary>
    /// Custom Validation Attribute for ISBN-10 or ISBN-13 format validation
    /// Validates that the ISBN follows standard format (10 or 13 digits with optional hyphens)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidIsbnAttribute : ValidationAttribute
    {
        // ISBN-10: 0-306-40615-2 or 0306406152
        // ISBN-13: 978-0-306-40615-7 or 9780306406157
        private static readonly Regex Isbn10Pattern = new Regex(@"^(?:\d{9}[\dX]|(?:\d-?){9}[\dX])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex Isbn13Pattern = new Regex(@"^(?:978|979)[-]?\d{1,5}[-]?\d{1,7}[-]?\d{1,6}[-]?\d$", RegexOptions.Compiled);

        public ValidIsbnAttribute() : base("The field {0} is not a valid ISBN format. Expected ISBN-10 or ISBN-13.")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            var isbn = value.ToString()?.Replace("-", "").Replace(" ", "").ToUpper();
            
            if (string.IsNullOrWhiteSpace(isbn))
                return true;

            // Check ISBN-10 (10 characters, last can be X)
            if (isbn.Length == 10)
            {
                return Isbn10Pattern.IsMatch(value.ToString()!);
            }
            
            // Check ISBN-13 (13 digits, starts with 978 or 979)
            if (isbn.Length == 13)
            {
                return Isbn13Pattern.IsMatch(value.ToString()!);
            }

            return false;
        }
    }
}
