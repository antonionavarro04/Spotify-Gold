namespace ENT {
    public class ClsUser {
        #region Fields and Properties

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public int? Gender { get; set; } // 0 - Male. 1 - Female, 2 - Other
        public DateTime? BirthDate { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public bool Active { get; set; }

        #endregion
        #region Constructors

        public ClsUser() {
            this.Id = 0;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.Salt = string.Empty;
            this.Email = string.Empty;
        }

        #endregion
        #region Methods

        public override string ToString() {
            return $"User: {this.Username} - Email: {this.Email}";
        }

        #endregion
    }
}
