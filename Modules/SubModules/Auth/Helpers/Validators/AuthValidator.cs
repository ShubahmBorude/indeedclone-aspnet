using IndeedClone.Modules.Shared.Error;
using System.Text.RegularExpressions;


namespace IndeedClone.Modules.SubModules.Auth.Helpers.Validators
{
    /*
     * Central Validation Class
     * --------------------------
     * - Pushes validation errors to ErrorError
     * - Does NOT throw exceptions
     * - Does NOT return params
     * - ErrorError.SetError() sets erroras in list<>
     * - ErrorError.CheckError() check error and returns bool
     * - Field-wise validation (UI friendly)
     */

    public static class AuthValidator
    {
     // # Custom Format Patterns
        private const string userNamePattern = @"^[a-zA-Z\s]+$";
        private const string userEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string userPasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,18}$";

        /* Name validation */
        public static void NameValidator(string param, string group = "name")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Name is required.");
                return;
            }

            if (!Regex.IsMatch(param, userNamePattern))
                ErrorError.SetError("Name can contain only letters and spaces.");
        }

        /* Email validation */
        public static void EmailValidator(string param, string group = "email")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Email is required.");
                return;
            }

            if (!Regex.IsMatch(param, userEmailPattern))
                ErrorError.SetError("Invalid email format.");
        }

        /* Password validation */
        public static void PasswordValidator(string param, string group = "password")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Password is required.");
                return;
            }

            if (param.Length < 8 || param.Length > 18)
                ErrorError.SetError("Password must be between 8 and 18 characters.");

            if (!Regex.IsMatch(param, userPasswordPattern))
                ErrorError.SetError("Password must include uppercase, lowercase, digit, and special character.");
        }

        /* Confirm password */
        public static void ConfirmPassword(string password, string confirmPassword, string group = "confirmPassword")
        {
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                ErrorError.SetError("Confirm password is required.");
                return;
            }

            if (!Regex.IsMatch(confirmPassword, userPasswordPattern))
                ErrorError.SetError("Password must include uppercase, lowercase, digit, and special character.");

            if (password != confirmPassword)
                ErrorError.SetError("Passwords do not match.");
        }

        /* Checkbox validation */
        public static void Checkbox(bool isChecked, string label = "This field", string group = "checkbox")
        {
            if (!isChecked)
                ErrorError.SetError($"{label} must be accepted.");
        }

    }
}
