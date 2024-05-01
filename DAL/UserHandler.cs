using Microsoft.Data.SqlClient;

namespace DAL {
    public static class UserHandler {
        public static string? ValidateUser(string username, string email) {
            string? reason = null;

            SqlConnection conn = ClsConnection.GetConnection(true);

            SqlCommand allDBUsernames = new("SELECT * FROM Users WHERE Username = @Username", conn);
            allDBUsernames.Parameters.AddWithValue("@Username", username);

            SqlCommand allDBEmails = new("SELECT * FROM Users WHERE Email = @Email", conn);
            allDBEmails.Parameters.AddWithValue("@Email", email);

            SqlDataReader? reader = null;

            try {
                reader = allDBUsernames.ExecuteReader();
                if (reader.HasRows) {
                    reason = "Username already taken";
                }

                reader = allDBEmails.ExecuteReader();
                if (reader.HasRows) {
                    reason = "Email already taken";
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            } finally {
                reader?.Close();
                conn.Close();
            }

            return reason;
        }
    }
}
