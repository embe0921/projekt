using System.Data;
using System.Data.SqlClient;

namespace Projekt.Models
{
    public class GuestMetoder
    {
        public GuestMetoder() { }
        //public bool GuestExist(string epost, string losenord)
        //{
        //    SqlConnection dbConncetion = new SqlConnection();
        //    dbConncetion.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

        //    using (dbConncetion)
        //    {
        //        dbConncetion.Open();

        //        string query = "SELECT COUNT(*) FROM Tbl_Guest WHERE Epost = @epost AND Losenord = @losenord";
        //        SqlCommand command = new SqlCommand(query, dbConncetion);
        //        command.Parameters.AddWithValue("@epost", epost);
        //        command.Parameters.AddWithValue("@losenord", losenord);

        //        int count = (int)command.ExecuteScalar();

        //        return count > 0;
        //    }
        //}

        public int GetGuestId(string epost, string losenord)
        {
            int guestId = -1; // Använd -1 som standardvärde om användaren/gästen inte hittas.

            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True"))
            {
                dbConnection.Open();

                string query = "SELECT Guest_Id FROM Tbl_Guest WHERE Epost = @epost AND Losenord = @losenord";
                SqlCommand command = new SqlCommand(query, dbConnection);
                command.Parameters.AddWithValue("@epost", epost);
                command.Parameters.AddWithValue("@losenord", losenord);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Om en rad hittades
                    {
                        guestId = reader.GetInt32(0); // Hämta användarens/gästens ID från den första kolumnen (GuestID)
                    }
                }
            }

            return guestId;
        }




        //public int InsertGuest(Guest g, out string errormsg)
        //{
        //    SqlConnection dbConnection = new SqlConnection();

        //    dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

        //    String sqlstring = "INSERT INTO Tbl_Guest (Fornamn, Efternamn, Epost, Telefon, Losenord) VALUES (@Fornamn, @Efternamn, @Epost, @Telefon, @Losenord)";
        //    SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

        //    dbCommand.Parameters.Add("Fornamn", SqlDbType.NVarChar, 50).Value = g.Fornamn;
        //    dbCommand.Parameters.Add("Efternamn", SqlDbType.NVarChar, 50).Value = g.Efternamn;
        //    dbCommand.Parameters.Add("Epost", SqlDbType.NVarChar, 50).Value = g.Epost;
        //    dbCommand.Parameters.Add("Telefon", SqlDbType.Int).Value = g.Telefon;
        //    dbCommand.Parameters.Add("Losenord", SqlDbType.NVarChar, 50).Value = g.Losenord;

        //    try
        //    {
        //        dbConnection.Open();
        //        int i = 0;
        //        i = dbCommand.ExecuteNonQuery();
        //        if (i == 1) { errormsg = ""; }
        //        else { errormsg = "Det skapas inte en användare i databasen"; }
        //        return (i);

        //    }
        //    catch (Exception e)
        //    {
        //        errormsg = e.Message;
        //        return 0;
        //    }
        //    finally
        //    {
        //        dbConnection.Close();

        //    }
        //}



        public int InsertGuest(Guest g, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

            String sqlstring = "INSERT INTO Tbl_Guest (Fornamn, Efternamn, Epost, Telefon, Losenord) VALUES (@Fornamn, @Efternamn, @Epost, @Telefon, @Losenord)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("Fornamn", SqlDbType.NVarChar, 50).Value = g.Fornamn;
            dbCommand.Parameters.Add("Efternamn", SqlDbType.NVarChar, 50).Value = g.Efternamn;
            dbCommand.Parameters.Add("Epost", SqlDbType.NVarChar, 50).Value = g.Epost;
            dbCommand.Parameters.Add("Telefon", SqlDbType.Int).Value = g.Telefon;
            dbCommand.Parameters.Add("Losenord", SqlDbType.NVarChar, 50).Value = g.Losenord;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();

                if (i == 1)
                {
                    // Hämta användar-ID med hjälp av e-post
                    int guestID = GetGuestIDByEmail(g.Epost);

                    if (guestID > 0)
                    {
                        errormsg = ""; // Allt gick bra
                        return guestID; // Returnera användar-ID
                    }
                    else
                    {
                        errormsg = "Användaren skapades, men användar-ID kunde inte hämtas.";
                        return 0;
                    }
                }
                else
                {
                    errormsg = "Det skapades inte en användare i databasen";
                    return 0;
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int GetGuestIDByEmail(string epost)
        {
            int guestID = -1; // Använd -1 som standardvärde om ingen användare hittades

            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

            string query = "SELECT Guest_Id FROM Tbl_Guest WHERE Epost = @epost";

            SqlCommand command = new SqlCommand(query, dbConnection);
            command.Parameters.AddWithValue("@epost", epost);

            try
            {
                dbConnection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    guestID = Convert.ToInt32(result);
                }
            }
            catch (Exception e)
            {
                // Hantera eventuella fel här
            }
            finally
            {
                dbConnection.Close();
            }

            return guestID;
        }

        public Guest GetGuest(int guestID, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

            String sqlstring = "SELECT * FROM Tbl_Guest WHERE Guest_Id = @guestID";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("guestID", SqlDbType.Int).Value = guestID;

            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();

                myAdapter.Fill(myDS, "myGuest");

                int count = 0;
                int i = 0;
                count = myDS.Tables["myGuest"].Rows.Count;

                if (count > 0)
                {
                    Guest g = new Guest();
                    g.GuestId = Convert.ToInt32(myDS.Tables["myGuest"].Rows[i]["Guest_Id"]);
                    g.Epost = myDS.Tables["myGuest"].Rows[i]["Epost"].ToString();
                    g.Telefon = Convert.ToInt32(myDS.Tables["myGuest"].Rows[i]["Telefon"]);
                
                    errormsg = "";
                    return g;
                }
                else
                {
                    errormsg = "Det hämtas ingen gäst.";
                    return null;


                }
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
        public int EditGuest(Guest g, out string errormsg)
        {

            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

            String sqlstring = "UPDATE Tbl_Guest SET Epost = @epost,Telefon = @telefon WHERE Guest_Id = @guestID;";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("guestID", SqlDbType.Int).Value = g.GuestId;
            dbCommand.Parameters.Add("epost", SqlDbType.NVarChar, 30).Value = g.Epost;
            dbCommand.Parameters.Add("telefon", SqlDbType.NVarChar, 30).Value = g.Telefon;

         

            try
            {
                dbConnection.Open();

                int i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Det uppdateras inget i databasen."; }
                return i;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }



        }
}
