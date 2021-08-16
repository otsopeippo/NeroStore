using System;
using System.Collections.Generic;

#nullable disable

namespace NeroStore.Models
{
    public partial class Tilau
    {
        public Tilau()
        {
            TilausRivis = new HashSet<TilausRivi>();
        }

        public int TilausId { get; set; }
        public string Email { get; set; }
        public DateTime? Tilauspvm { get; set; }
        public DateTime? ToimitusPvm { get; set; }
        public decimal? Tilaussumma { get; set; }
        public int? KayttajaId { get; set; }

        public virtual ICollection<TilausRivi> TilausRivis { get; set; }
    }
}
