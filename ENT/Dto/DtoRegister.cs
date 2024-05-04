using COM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT.Dto {
    /// <summary>
    /// Registration/Login of an User
    /// </summary>
    public class DtoRegister {
        #region Constants

        private static readonly List<char> USER_ALLOWED_SPECIAL_CHARACTERS = new() { '_', '-', '.' };

        public static readonly int MIN_USERNAME_LENGTH = 3;
        public static readonly int MAX_USERNAME_LENGTH = 30;
        public static readonly int MIN_PASSWORD_LENGTH = 8;
        public static readonly int MAX_PASSWORD_LENGTH = 64;
        public static readonly int MIN_EMAIL_LENGTH = 5;
        public static readonly int MAX_EMAIL_LENGTH = 320;

        #endregion
        #region Fields & Properties

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        #endregion
        #region Constructors

        public DtoRegister() {
            Username = "";
            Password = "";
            Email = "";
        }

        public DtoRegister(string username, string password, string email) {
            Username = username;
            Password = password;
            Email = email;
        }

        public DtoRegister(DtoRegister userInfo) {
            Username = userInfo.Username;
            Password = userInfo.Password;
            Email = userInfo.Email;
        }

        #endregion
        #region Methods

        /// <summary>
        /// Validates the User Information
        /// </summary>
        /// <returns>
        /// If the User Information is inValid it will return the reaseon, otherwise it will return null
        /// </returns>
        public string? Validate() {
            string? reason = null;

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email)) {
                reason = "Username, Password and Email are required";
            } else if (Username.Length < MIN_USERNAME_LENGTH) {
                reason = $"Username must be at least {MIN_USERNAME_LENGTH} characters long";
            } else if (Username.Length > MAX_USERNAME_LENGTH) {
                reason = $"Username must be at most {MAX_USERNAME_LENGTH} characters long";
            } else if (Password.Length < MIN_PASSWORD_LENGTH) {
                reason = $"Password must be at least {MIN_PASSWORD_LENGTH} characters long";
            } else if (Password.Length > MAX_PASSWORD_LENGTH) {
                reason = $"Password must be at most {MAX_PASSWORD_LENGTH} characters long";
            } else if (Username.Contains(' ') || Password.Contains(' ') || Email.Contains(' ')) {
                reason = "Username, Password and Email cannot contain spaces";
            } else if (Username.Any(c => !char.IsLetterOrDigit(c) && !USER_ALLOWED_SPECIAL_CHARACTERS.Contains(c))) {
                reason = "Username can only contain letters, numbers, underscores, hyphens and dots";
            } else if (!Email.Contains('@') || !Email.Contains('.') || Email.Length < MIN_EMAIL_LENGTH || Email.Length > MAX_EMAIL_LENGTH) {
                reason = "Email is not valid";
            } else if (Password.Any(c => (c > 127 || c < 33))) {
                reason = "Password cannot contain special characters";
            } else { // Password specific validations
                bool hasDigit = false;
                bool hasUpperCase = false;
                bool hasLowerCase = false;
                bool hasSpecial = false;

                foreach (char c in Password) {
                    if (char.IsDigit(c)) {
                        hasDigit = true;
                    } else if (char.IsUpper(c)) {
                        hasUpperCase = true;
                    } else if (char.IsLower(c)) {
                        hasLowerCase = true;
                    } else if (!char.IsLetterOrDigit(c)) {
                        hasSpecial = true;
                    }

                    if (hasDigit && hasUpperCase && hasLowerCase && hasSpecial) {
                        break;
                    }
                }

                if (!hasDigit) {
                    reason = "Password must contain at least one digit";
                } else if (!hasUpperCase) {
                    reason = "Password must contain at least one uppercase letter";
                } else if (!hasLowerCase) {
                    reason = "Password must contain at least one lowercase letter";
                } else if (!hasSpecial) {
                    reason = "Password must contain at least one special character";
                }
            }

            return reason;
        }

        public void Trim() {
            Username = Username.Trim();
            Password = Password.Trim();
            Email = Email.Trim();
        }

        #endregion
    }
}
