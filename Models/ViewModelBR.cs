namespace Projekt.Models
{
    public class ViewModelBR
    {
        public IEnumerable<Rumstyp> RumstypLista { get; set; }

        public DateTime CheckInDatum { get; set; }
        public DateTime CheckUtDatum { get; set; }

        //public string Namn { get; set; }

        public int ValtRumstypId { get; set; }


        //public IEnumerable<BokningRumstyp> RumstypLista { get; set; }

        //public BokningRumstyp BokningRumstyp { get; set; }


    }
}
