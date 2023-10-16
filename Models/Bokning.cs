using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class Bokning
    {
        public Bokning() { }
        public int BokningId { get; set; }
        public int GuestId { get; set; }
        public int RumId { get; set; }

        [DataType(DataType.Date)] 
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckInDatum { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckUtDatum { get; set;}
        public int TotalKostnad { get; set; }

    }
}
