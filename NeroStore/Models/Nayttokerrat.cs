using System;
using System.Collections.Generic;

#nullable disable

namespace NeroStore.Models
{
    public partial class Nayttokerrat
    {
        public int TuoteId { get; set; }
        public int Lkm { get; set; }

        public virtual Tuote Tuote { get; set; }
    }
}
