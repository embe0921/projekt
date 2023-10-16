using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Projekt.Models
{
    public class BokningMetoder
    {
        public BokningMetoder() { }

        //public int InsertBokning(Bokning b, out string errormsg)
        //{

        //    SqlConnection dbConnection = new SqlConnection();
        //    dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

        //    String sqlstring = "INSERT INTO Tbl_Bokning (Guest_Id, Rum_Id, Check_In_Datum, Check_Ut_Datum, Total_Kostnad) VALUES (@guestID, @rumID, @checkIN, @checkUT, @totalKostnad)";
        //    SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);


        //    dbCommand.Parameters.Add("guestID", SqlDbType.Int).Value = b.GuestId;
        //    dbCommand.Parameters.Add("rumID", SqlDbType.Int).Value = b.RumId;
        //    dbCommand.Parameters.Add("checkIN", SqlDbType.DateTime).Value = b.CheckInDatum;
        //    dbCommand.Parameters.Add("checkUT", SqlDbType.DateTime).Value = b.CheckUtDatum;
        //    dbCommand.Parameters.Add("totalKostnad", SqlDbType.Int).Value = b.TotalKostnad;


        //    try
        //    {
        //        dbConnection.Open();
        //        int i = 0;
        //        i = dbCommand.ExecuteNonQuery();
        //        if (i == 1) { errormsg = ""; }
        //        else { errormsg = "Det skapas inte en bokning i databasen"; }
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

        public int InsertBokning(Bokning b, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

            // Kontrollera om det redan finns en överlappande bokning för samma rum och datum
            string checkOverlapQuery = "SELECT COUNT(*) FROM Tbl_Bokning WHERE Rum_Id = @rumID AND " +
                "((@checkIN >= Check_In_Datum AND @checkIN < Check_Ut_Datum) OR " +
                "(@checkUT > Check_In_Datum AND @checkUT <= Check_Ut_Datum))";
            SqlCommand checkOverlapCommand = new SqlCommand(checkOverlapQuery, dbConnection);
            checkOverlapCommand.Parameters.Add("rumID", SqlDbType.Int).Value = b.RumId;
            checkOverlapCommand.Parameters.Add("checkIN", SqlDbType.DateTime).Value = b.CheckInDatum;
            checkOverlapCommand.Parameters.Add("checkUT", SqlDbType.DateTime).Value = b.CheckUtDatum;


            try
            {
                dbConnection.Open();

                // Utför frågan för att kontrollera överlappande bokningar
                int overlappandeBokingsCount = (int)checkOverlapCommand.ExecuteScalar();

                if (overlappandeBokingsCount > 0)
                {
                    errormsg = "Det finns inga lediga rum under dessa datum. Vänligen välj nya datum eller ändra rumstyp.";
                    return 0;
                }
                else
                {
                    // Ingen överlappande bokning, så lägg till den nya bokningen
                    string insertQuery = "INSERT INTO Tbl_Bokning (Guest_Id, Rum_Id, Check_In_Datum, Check_Ut_Datum, Total_Kostnad) " +
                        "VALUES (@guestID, @rumID, @checkIN, @checkUT, @totalKostnad)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, dbConnection);
                    insertCommand.Parameters.Add("guestID", SqlDbType.Int).Value = b.GuestId;
                    insertCommand.Parameters.Add("rumID", SqlDbType.Int).Value = b.RumId;
                    insertCommand.Parameters.Add("checkIN", SqlDbType.DateTime).Value = b.CheckInDatum;
                    insertCommand.Parameters.Add("checkUT", SqlDbType.DateTime).Value = b.CheckUtDatum;
                    insertCommand.Parameters.Add("totalKostnad", SqlDbType.Int).Value = b.TotalKostnad;

                    int result = insertCommand.ExecuteNonQuery();
                    if (result == 1)
                    {
                        errormsg = "";
                        return 1; // Bokningen lades till
                    }
                    else
                    {
                        errormsg = "Det uppstod ett fel när bokningen skulle läggas till.";
                        return 0;
                    }
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

        


        public int GetBokningID(Bokning b)
        {
            int bokningID = -1; // Använd -1 som standardvärde om användaren/gästen inte hittas.

            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True"))
            {
                dbConnection.Open();

                string query = "SELECT Bokning_Id FROM Tbl_Bokning WHERE Check_In_Datum = @CheckInDatum AND Check_Ut_Datum = @CheckUtDatum AND Rum_Id = @RumId";
                SqlCommand command = new SqlCommand(query, dbConnection);
                command.Parameters.AddWithValue("@CheckInDatum", b.CheckInDatum);
                command.Parameters.AddWithValue("@CheckUtDatum", b.CheckUtDatum);
                command.Parameters.AddWithValue("@RumId", b.RumId);


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Om en rad hittades
                    {
                        bokningID = reader.GetInt32(0);
                    }
                }
            }

            return bokningID;
        }
        public Bokning GetBokning(int bokningID)
        {
            Bokning bokning = null;

            using (SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True"))
            {
                dbConnection.Open();

                string query = "SELECT * FROM Tbl_Bokning WHERE Bokning_Id = @bokningID";
                SqlCommand command = new SqlCommand(query, dbConnection);
                command.Parameters.AddWithValue("@bokningID", bokningID);


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Om en rad hittades
                    {
                        // Skapa en ny instans av bokningsobjektet och fyll det med data från databasen
                        bokning = new Bokning
                        {
                            BokningId = reader.GetInt32(reader.GetOrdinal("Bokning_Id")),
                            GuestId = reader.GetInt32(reader.GetOrdinal("Guest_Id")),
                            RumId = reader.GetInt32(reader.GetOrdinal("Rum_Id")),
                            CheckInDatum = reader.GetDateTime(reader.GetOrdinal("Check_In_Datum")),
                            CheckUtDatum = reader.GetDateTime(reader.GetOrdinal("Check_Ut_Datum")),
                            TotalKostnad = reader.GetInt32(reader.GetOrdinal("Total_Kostnad")),
                            // Lägg till fler egenskaper här baserat på din databasstruktur
                        };
                    }
                }

                return bokning;
            }

        }

       
        public List<BokningHotell> GetBokningHotell(int guestID, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

            String sqlstring = "SELECT Tbl_Hotell.Namn, Tbl_Bokning.Check_In_Datum, Tbl_Bokning.Check_Ut_Datum FROM Tbl_Bokning, Tbl_Rum, Tbl_Hotell WHERE Tbl_Bokning.Guest_Id = @guestID AND Tbl_Bokning.Rum_Id = Tbl_Rum.Rum_Id AND Tbl_Rum.Hotell_Id = Tbl_Hotell.Hotell_Id;";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("guestID", SqlDbType.Int).Value = guestID;

            SqlDataReader reader = null;

            List<BokningHotell> BokningHotellList = new List<BokningHotell>();

            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    BokningHotell bh = new BokningHotell();
                    bh.HotellNamn = reader["Namn"].ToString();
                    bh.CheckInDatum = (DateTime)reader["Check_In_Datum"];
                    bh.CheckUtDatum = (DateTime)reader["Check_Ut_Datum"];

                    BokningHotellList.Add(bh);

                }
                reader.Close();
                return BokningHotellList;
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

        public List<Guest> GetGuest(int guestID, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";

            String sqlstring = "SELECT Guest_Id, Fornamn, Efternamn, Epost, Telefon FROM Tbl_Guest WHERE Guest_Id = @guestID";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("guestID", SqlDbType.Int).Value = guestID;
            SqlDataReader reader = null;

            List<Guest> GuestLista = new List<Guest>();
            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Guest g = new Guest();
                    g.GuestId = Convert.ToInt32(reader["Guest_Id"]);
                    g.Fornamn = reader["Fornamn"].ToString();
                    g.Efternamn = reader["Efternamn"].ToString();
                    g.Epost = reader["Epost"].ToString();
                    g.Telefon = Convert.ToInt32(reader["Telefon"]);

                    GuestLista.Add(g);

                }
                reader.Close();
                return GuestLista;
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



    }

}