using System;
using System.Collections.Generic;

#nullable disable

namespace NeroStore.Models
{
    public partial class TilausRivi
    {
        public int TilausriviId { get; set; }
        public int Lkm { get; set; }
        public int? TilausId { get; set; }
        public int? TuoteId { get; set; }

        public virtual Tilau Tilaus { get; set; }
        public virtual Tuote Tuote { get; set; }
    }

}
