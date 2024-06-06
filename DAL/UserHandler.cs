using COM;
using ENT.Dto;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace DAL {
    public static class UserHandler {

        public const int SALT_LENGTH = 12;

        private static string GenerateSalt(int length = SALT_LENGTH) {
            string salt = "";
            Random random = new();
            for (int i = 0; i < length; i++) {
                salt += (char) (random.Next(33, 126));
            }
            return salt;
        }

        private static string Hash(string data) {
            byte[] encodedData = Encoding.UTF8.GetBytes(data);
            byte[] hashedData = SHA256.HashData(encodedData);
            return Encoding.UTF8.GetString(hashedData);
        }

        private static string HashPassword(string password, string salt) {
            return Hash(password + salt);
        }

        private static string? ValidateUser(string username, string email) {
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

        public static void SendEmail(string toEmail, string subject, string body) {
            MailMessage mailMessage = new();
            mailMessage.From = new MailAddress(ClsConnection.EMAIL);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            // Set up the SMTP client
            SmtpClient smtpClient = new("smtp.gmail.com", 587) {
                Credentials = new NetworkCredential(ClsConnection.EMAIL, ClsConnection.EMAIL_PASSWORD),
                EnableSsl = true
            };

            try {
                // Send the email
                smtpClient.Send(mailMessage);
            } catch (Exception ex) {
                // Handle any errors that occur while sending the email
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public static int RegisterUser(DtoRegister dto) {
            if (dto.Validate() != null) {
                throw new Exception("Invalid User Information");
            } else if (ValidateUser(dto.Username, dto.Email) != null) {
                throw new Exception("User already exists");
            } else {
                int affectedRows = 0;
                string body = File.ReadAllText("wwwroot/templates/EmailTemplate.html");

                body = body.Replace("@Model.Name", dto.Username);

                string salt = GenerateSalt();
                dto.Password = HashPassword(dto.Password, salt);
                string activationCode = $"{Commons.VENDOR}-{new Random().Next(100000, 999999)}";
                body = body.Replace("@Model.ActivationCode", activationCode);

                activationCode = Hash(activationCode);

                SqlConnection conn = ClsConnection.GetConnection(true);
                SqlCommand cmd = new("INSERT INTO Users (Username, Password, Salt, Email, ActivationCode) VALUES (@Username, @Password, @Salt, @Email, @ActivationCode)", conn);
                cmd.Parameters.AddWithValue("@Username", dto.Username);
                cmd.Parameters.AddWithValue("@Password", dto.Password);
                cmd.Parameters.AddWithValue("@Salt", salt);
                cmd.Parameters.AddWithValue("@Email", dto.Email);
                cmd.Parameters.AddWithValue("@ActivationCode", activationCode);

                try {
                    SendEmail(dto.Email, "Account Activation", body);
                    affectedRows = cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                } finally {
                    conn.Close();
                }

                return affectedRows;
            }
        }

        public static HttpStatusCode ActivateUser(string user, string code) {
            if (user == "") {
                return HttpStatusCode.BadRequest;
            } else if (code == "") {
                return HttpStatusCode.BadRequest;
            } else {
                SqlConnection conn = ClsConnection.GetConnection(true);
                SqlCommand cmd = new("SELECT * FROM Users WHERE Username = @Username AND ActivationCode = @ActivationCode", conn);
                cmd.Parameters.AddWithValue("@Username", user);
                cmd.Parameters.AddWithValue("@ActivationCode", Hash(code));

                SqlDataReader? reader = null;

                try {
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows) {
                        reader.Close();
                        cmd.CommandText = "UPDATE Users SET Active = 1 WHERE Username = @Username";
                        cmd.ExecuteNonQuery();
                        return HttpStatusCode.OK;
                    } else {
                        return HttpStatusCode.NotFound;
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                } finally {
                    reader?.Close();
                    conn.Close();
                }

                return HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="dto">User Information</param>
        /// <returns>A newly Ticket</returns>
        public static string? LoginUser(DtoRegister dto) {
            string? token = null;

            SqlConnection conn = ClsConnection.GetConnection(true);
            SqlCommand cmd;
            if (dto.Username != "") {
                cmd = new("SELECT * FROM Users WHERE Username = @Username", conn);
            } else {
                cmd = new("SELECT * FROM Users WHERE Email = @Email", conn);
            }
            cmd.Parameters.AddWithValue("@Username", dto.Username);
            cmd.Parameters.AddWithValue("@Email", dto.Email);

            SqlDataReader? reader = null;

            try {
                reader = cmd.ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    string? salt = reader["Salt"].ToString();
                    string? password = reader["Password"].ToString();
                    string? username = reader["Username"].ToString();
                    string hashedPassword = HashPassword(dto.Password, salt!);

                    if (password == hashedPassword) {
                        token = TokenUtils.GenerateToken(dto.Username);

                        // INSERT Token INTO Tokens, Exopiration of 7 days
                        reader.Close();
                        cmd.CommandText = "INSERT INTO Tokens (Token, Username, Expiration) VALUES (@Token, @Username, @Expiration)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Token", token);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Expiration", DateTime.Now.AddDays(7));
                        cmd.ExecuteNonQuery();
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            } finally {
                reader?.Close();
                conn.Close();
            }

            return token;   
        }

        public static bool CheckToken(string? token) {
            SqlConnection conn = ClsConnection.GetConnection(true);
            SqlCommand cmd = new("SELECT * FROM Tokens WHERE Token = @Token", conn);
            cmd.Parameters.AddWithValue("@Token", token);

            SqlDataReader? reader = null;

            try {
                reader = cmd.ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    DateTime expiration = (DateTime) reader["Expiration"];
                    if (expiration > DateTime.Now) {
                        expiration = DateTime.Now.AddDays(7); // Renew Token
                        return true;
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            } finally {
                reader?.Close();
                conn.Close();
            }

            return false;
        }
    }
}
