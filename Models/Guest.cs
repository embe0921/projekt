namespace Projekt.Models
{
    public class Guest
    {
        public Guest() { }
        public int GuestId { get; set; }    
        public string Fornamn { get; set; }
        public string Efternamn { get; set; }
        public string Epost { get; set; }
        public int Telefon { get; set; }
        public string Losenord { get; set; }
    }
}
