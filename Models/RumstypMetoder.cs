using System.Data.SqlClient;

namespace Projekt.Models
{
    public class RumstypMetoder
    {
        public RumstypMetoder() { }

        public List<Rumstyp> GetRumstypLista(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";
            String sqlstring = "SELECT Rumstyp_Id, Namn, Pris FROM Tbl_Rumstyp";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            SqlDataReader reader = null;

            List<Rumstyp> RumstypLista = new List<Rumstyp>();
            errormsg = "";

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();
                while (reader.Read())
                {
                    Rumstyp r = new Rumstyp();
                    r.RumstypId = Convert.ToInt16(reader["Rumstyp_Id"]);
                    r.Namn = reader["Namn"].ToString();
                    r.PrisPerNatt = Convert.ToInt32(reader["Pris"]);

                    RumstypLista.Add(r);

                }
                reader.Close();
                return RumstypLista;


            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int GetRumstypID(string Namn)
        {
            int rumstypID = -1; // Använd -1 som standardvärde om användaren/gästen inte hittas.

            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True"))
            {
                dbConnection.Open();

                string query = "SELECT Rumstyp_Id FROM Tbl_Rumstyp WHERE Namn = @Namn";
                SqlCommand command = new SqlCommand(query, dbConnection);
                command.Parameters.AddWithValue("@Namn", Namn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Om en rad hittades
                    {
                        rumstypID = reader.GetInt32(0); // Hämta användarens/gästens ID från den första kolumnen (GuestID)
                    }
                }
            }

            return rumstypID;
        }
        public int GetRumstypID(ViewModelBR br)
        {
            int rumstypID = -1; // Använd -1 som standardvärde om rumstypen inte hittas.

            // Använd br.ValtRumstypId för att hämta det valda rumstyp-id:t från ViewModelBR-objektet.
            int valtRumstypId = br.ValtRumstypId;

            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True"))
            {
                dbConnection.Open();

                string query = "SELECT Rumstyp_Id FROM Tbl_Rumstyp WHERE Rumstyp_Id = @RumstypId";
                SqlCommand command = new SqlCommand(query, dbConnection);
                command.Parameters.AddWithValue("@RumstypId", valtRumstypId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Om en rad hittades
                    {
                        rumstypID = reader.GetInt32(0); // Hämta rumstyp-id från den första kolumnen (Rumstyp_Id)
                    }
                }
            }

            return rumstypID;
        }

        public int GetPris(int rumstypID)
        {
            int Pris = -1; // Använd -1 som standardvärde om användaren/gästen inte hittas.

            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True"))
            {
                dbConnection.Open();

                string query = "SELECT Pris FROM Tbl_Rumstyp WHERE Rumstyp_Id = @rumstypID";
                SqlCommand command = new SqlCommand(query, dbConnection);
                command.Parameters.AddWithValue("@rumstypID", rumstypID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Om en rad hittades
                    {
                        Pris = reader.GetInt32(0); // Hämta användarens/gästens ID från den första kolumnen (GuestID)
                    }
                }
            }

            return Pris;
        }


    }
}