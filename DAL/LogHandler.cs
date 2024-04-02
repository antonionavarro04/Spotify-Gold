using ENT;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL {
    public static class LogHandler {
        /// <summary>
        /// Function that writes a message to the DDBB
        /// This data will persist
        /// </summary>
        /// <param name="logPath">Path of the file containing the logs</param>
        /// <param name="message">Thing to write</param>
        /// <returns></returns>
        public static int WriteToDDBB(ClsLog message) {
            int affectedRows = 0;
            SqlConnection conn = ClsConnection.getConnection(false);
            SqlCommand query = new SqlCommand();

            try {
                conn.Open();

                query.Connection = conn;
                query.CommandText = "INSERT INTO Log (Date, Receiver, Message) VALUES (@Date, @Receiver, @Message)";
                query.Parameters.AddWithValue("@Date", message.Date);
                query.Parameters.AddWithValue("@Receiver", message.Receiver);
                query.Parameters.AddWithValue("@Message", message.Message);

                affectedRows = query.ExecuteNonQuery();
            } catch (SqlException ex) {
                Console.WriteLine($"Error writing to log: {ex.Message}");
            } catch (InvalidOperationException ex) {
                Console.WriteLine($"Error writing to log: {ex.Message}");
            } catch (Exception ex) {
                Console.WriteLine($"Error writing to log: {ex.Message}");
            } finally {
                conn.Close();
            }

            return affectedRows;
        }
    }
}
