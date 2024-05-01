using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT {
    public class ClsUser {
        #region Fields and Properties

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Gender { get; set; } // 0 - Male. 1 - Female, 2 - Other
        public DateTime? BirthDate { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public int ProfilePicture { get; set; } // FK of the Profile Picture 0 is a default stored image

        #endregion
        #region Constructors

        public ClsUser() {
            this.Id = 0;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.Email = string.Empty;
        }

        public ClsUser(string username, string password, string email) {
            this.Id = 0;
            this.Username = username;
            this.Password = password;
            this.Email = email;
        }

        public ClsUser(ClsUser user) {
            this.Id = user.Id;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Gender = user.Gender;
            this.BirthDate = user.BirthDate;
            this.Country = user.Country;
            this.City = user.City;
            this.ProfilePicture = user.ProfilePicture;
        }

        #endregion
        #region Methods

        public override string ToString() {
            return $"User: {this.Username} - Email: {this.Email}";
        }

        #endregion
    }
}
