using System;
using System.Collections.Generic;

#nullable disable

namespace NeroStore.Models
{
    public partial class Tuote
    {
        public Tuote()
        {
            TilausRivis = new HashSet<TilausRivi>();
        }

        public int TuoteId { get; set; }
        public string Nimi { get; set; }
        public decimal Hinta { get; set; }
        public int Lkm { get; set; }
        public string Kuvaus { get; set; }
        public string Tyyppi { get; set; }
        public string Tuoteryhma { get; set; }

        public virtual Nayttokerrat Nayttokerrat { get; set; }
        public virtual ICollection<TilausRivi> TilausRivis { get; set; }
    }
}
