using Microsoft.Data.SqlClient;

namespace DAL {
    public static class ImageHandler {
        public static int SaveImage(byte[] image) {

            SqlConnection conn = ClsConnection.GetConnection(true);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Images (Image) VALUES (@Image)";
            cmd.Parameters.AddWithValue("@Image", image);

            int rowsAffected = cmd.ExecuteNonQuery();

            conn.Close();

            return rowsAffected;
        }
    }
}
