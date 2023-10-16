namespace Projekt.Models
{
    public class BokningRumstyp
    {
        public BokningRumstyp() { }

        public int BokningId { get; set; }
        public int GuestId { get; set; }
        public int RumId { get; set; }
        public int TotalKostnad { get; set; }

        public DateTime CheckInDatum { get; set; }
        public DateTime CheckUtDatum { get; set;}
        public string Namn { get; set;}
        public int RumstypId { get; set; }
        public int PrisPerNatt { get; set; }


    }
}
