using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Projekt.Models
{
    public class HotellMetoder
    {
        public List<Hotell> GetHotellWithDataSet(out string errormsg)
        {
            SqlConnection dbConncetion = new SqlConnection();
            dbConncetion.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";
            String sqlstring = "SELECT Namn, Hotell_Id, Bild FROM Tbl_Hotell";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConncetion);
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            List<Hotell> HotellList = new List<Hotell>();

            try
            {
                dbConncetion.Open();

                myAdapter.Fill(myDS, "myHotell");
                int count = 0;
                int i = 0;
                count = myDS.Tables["myHotell"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        Hotell h = new Hotell();
                        h.Namn = myDS.Tables["myHotell"].Rows[i]["Namn"].ToString();
                        h.HotellId = Convert.ToInt16(myDS.Tables["myHotell"].Rows[i]["Hotell_Id"]);
                        h.Bild = myDS.Tables["myHotell"].Rows[i]["Bild"].ToString();

                        i++;
                        HotellList.Add(h);
                    }
                    errormsg = "";
                    return HotellList;
                } 
                else
                {
                    errormsg = "Det hämtas inget hotell.";
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
                dbConncetion.Close();
            }
           
        }

        public Hotell GetHotell(int id, out string errormsg)
        {
            SqlConnection dbConncetion = new SqlConnection();
            dbConncetion.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Hotellbokning;Integrated Security=True";
            String sqlstring = "SELECT * FROM Tbl_Hotell WHERE Hotell_Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConncetion);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConncetion.Open();
                myAdapter.Fill(myDS, "myHotell");

                int count = 0;
                int i = 0;
                count = myDS.Tables["myHotell"].Rows.Count;

                if (count > 0)
                {
                    Hotell h = new Hotell();
                    h.HotellId = Convert.ToInt32(myDS.Tables["myHotell"].Rows[i]["Hotell_Id"]);
                    h.Namn = myDS.Tables["myHotell"].Rows[i]["Namn"].ToString();
                    h.Adress = myDS.Tables["myHotell"].Rows[i]["Adress"].ToString();
                    h.Stad = myDS.Tables["myHotell"].Rows[i]["Stad"].ToString();
                    h.Bild = myDS.Tables["myHotell"].Rows[i]["Bild"].ToString();

                    errormsg = "";
                    return h;
                }
                else
                {
                    errormsg = "Det hämtas inget hotell.";
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
                dbConncetion.Close();
            }
        }

    }
}
