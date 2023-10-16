using System.Data.SqlClient;

namespace Projekt.Models
{
    public class RumMetoder
    {
        public RumMetoder() { }

        public int GetRumID(int hotellID, int rumstypID)
        {
            int rumID = -1; // Använd -1 som standardvärde om användaren/gästen inte hittas.

            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True"))
            {
                dbConnection.Open();

                string query = "SELECT Rum_Id FROM Tbl_Rum WHERE Hotell_Id = @hotellID AND Rumstyp_Id = @rumstypID";
                SqlCommand command = new SqlCommand(query, dbConnection);
                command.Parameters.AddWithValue("@hotellID", hotellID);
                command.Parameters.AddWithValue("@rumstypID", rumstypID);


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Om en rad hittades
                    {
                        rumID = reader.GetInt32(0); 
                    }
                }
            }

            return rumID;
        }
    }
}
