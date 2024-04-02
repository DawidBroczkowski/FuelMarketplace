namespace FuelMarketplace.Application.Interfaces
{
    public interface IValidationService
    {
        /// <summary>
        /// Validates the E-mail address.
        /// </summary>
        /// <param name="email">E-mail address</param>
        /// <returns>True if the E-mail is vaild, false if not.</returns>
        bool ValidateEmail(string email);
        /// <summary>
        /// Checks if the phone number contains digits only.
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>True if the phone number is valid, false if not.</returns>
        bool ValidatePhoneNumber(string phoneNumber);
    }
}