using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookStoreApplication.MVC.Validators.Author
{
    /// <summary>
    /// Custom Validation Attribute that prevents special characters in string fields
    /// Used for validating names, descriptions that should only contain alphanumeric chars and spaces
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NoSpecialCharactersAttribute : ValidationAttribute
    {
        private static readonly Regex AllowedPattern = new Regex(@"^[a-zA-Z0-9\s]+$", RegexOptions.Compiled);

        public NoSpecialCharactersAttribute() : base("The field {0} contains invalid characters. Only letters, numbers, and spaces are allowed.")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true; // Null values are handled by [Required] if needed

            var stringValue = value.ToString();
            
            if (string.IsNullOrWhiteSpace(stringValue))
                return true; // Empty values are handled by other validators

            return AllowedPattern.IsMatch(stringValue);
        }
    }
}
