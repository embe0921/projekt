using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class BokningHotell
    {
        public BokningHotell() { }

        public string HotellNamn { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckInDatum { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckUtDatum { get; set; }
    }
}
