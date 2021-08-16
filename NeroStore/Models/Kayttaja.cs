using System;
using System.Collections.Generic;

#nullable disable

namespace NeroStore.Models
{
    public partial class Kayttaja
    {
        public int KayttajaId { get; set; }
        public string Email { get; set; }
        public string Salasana { get; set; }
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Osoite { get; set; }
        public string Postinumero { get; set; }
        public DateTime? Syntymäaika { get; set; }
    }
}
