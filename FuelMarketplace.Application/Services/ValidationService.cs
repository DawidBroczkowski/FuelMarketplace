using System.Net.Mail;
using System.Text.RegularExpressions;
using FuelMarketplace.Application.Interfaces;

namespace FuelMarketplace.Application.Services
{
    public class ValidationService : IValidationService
    {
        public bool ValidateEmail(string email)
        {
            email = email.Trim();

            string pattern = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";
            if (email.Contains(" "))
            {
                return false;
            }
            if (email.Contains(".."))
            {
                return false;
            }
            if (email.StartsWith(".") || email.EndsWith("."))
            {
                return false;
            }
            if (email.Contains("@") && email.IndexOf("@") != email.LastIndexOf("@"))
            {
                return false;
            }

            return Regex.IsMatch(email, pattern);
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            // Check if phone number is digits only
            if (phoneNumber.All(char.IsDigit) == false)
            {
                return false;
            }
            return true;
        }
    }
}
