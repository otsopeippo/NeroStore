using NeroStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace NeroStore
{
    public class Apumetodit
    {
        private readonly NeroStoreDBContext _context;

        public Apumetodit(NeroStoreDBContext context)
        {
            _context = context;
        }

        public List<Tuote> HaeTuotteet()
        {
            var tuotteet = new List<Tuote> { };
            tuotteet = _context.Tuotes.Select(tuote => tuote).ToList();
            return tuotteet;
        }

        public Tuote HaeTuote(int id)
        {
            var tuote = new Tuote();
            tuote = _context.Tuotes.Find(id);
            return tuote;
        }

        public bool LisääTuote(string nimi, decimal hinta, int lkm, string kuvaus, string tyyppi, string tuoteryhma)
        {
            var uusiTuote = new Tuote();
            uusiTuote.Nimi = nimi;
            uusiTuote.Hinta = hinta;
            uusiTuote.Lkm = lkm;
            uusiTuote.Kuvaus = kuvaus;
            uusiTuote.Tyyppi = tyyppi;
            uusiTuote.Tuoteryhma = tuoteryhma;

            try
            {
                _context.Tuotes.Add(uusiTuote);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool MuutaTuotteenSaldoa(int id, int muutos)
        {
            try
            {
                var tuote = new Tuote();
                tuote = _context.Tuotes.Find(id);
                tuote.Lkm = tuote.Lkm + muutos;
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void PoistaTuote()
        {
            // koodi
        }

        public List<Tuote> HaeOstoskori(ISession sessio)
        {
            var ostoskori = new List<Tuote> { };
            string ostoskoriSerialized = sessio.GetString("tuotteet");
            if (!string.IsNullOrEmpty(ostoskoriSerialized))
            {
                ostoskori = JsonConvert.DeserializeObject<List<Tuote>>(ostoskoriSerialized);
            }
            return ostoskori;
        }

        public void LisääOstoskoriin(ISession sessio, int id)
        {
            var ostoskori = HaeOstoskori(sessio);
            var tuote = HaeTuote(id);
            ostoskori.Add(tuote);
            string ostoskoriSerialized = JsonConvert.SerializeObject(ostoskori);
            sessio.SetString("tuotteet", ostoskoriSerialized);
        }

        public void PoistaOstoskorista()
        {
            // koodi
        }

        public bool KäyttäjäOnOlemassa(int id)
        {
            var q = (from k in _context.Kayttajas
                     where k.KayttajaId == id
                     select k).FirstOrDefault().KayttajaId;
            return (q == id) ? true : false;
        }

        public bool KäyttäjäOnAdmin(int id)
        {
            var onAdmin = (from k in _context.Kayttajas
                           where k.KayttajaId == id
                           select k).FirstOrDefault().OnAdmin;
            return onAdmin;
        }
        public bool LisääTilaus(string email, decimal tilaussumma)
        {
            DateTime dt = new();
            Tilau uusiTilaus = new()
            {
                Email = email,
                Tilauspvm = DateTime.Now,
                ToimitusPvm = dt.AddDays(2),
                Tilaussumma = tilaussumma,
            };            
            try
            {
                _context.Tilaus.Add(uusiTilaus);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void LisaaTilausrivi(int lkm, int? tilaus_id, int? tuote_id)
        {
            if (tilaus_id != null && tuote_id != null)
            {
                TilausRivi tr = new TilausRivi()
                {
                    Lkm = lkm,
                    TilausId = tilaus_id,
                    TuoteId = tuote_id
                };
                _context.TilausRivis.Add(tr);
                _context.SaveChanges();
            }
            else { return; }
        }
        public string HashPassword(string salasana)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] salasana_bytes = Encoding.ASCII.GetBytes(salasana);
            byte[] encrypted_bytes = sha1.ComputeHash(salasana_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }

        public int? HaeAdmin(ISession sessio)
        {
            string käyttäjäserialized = sessio.GetString("käyttäjä");
            if (!string.IsNullOrEmpty(käyttäjäserialized))
            {
                int käyttäjä = Convert.ToInt32(käyttäjäserialized);
                return käyttäjä;
            }
            return null;
        }
        public void LisääAdminSessioon(ISession sessio, int? id)
        {
            sessio.SetString("käyttäjä", id.ToString());
        }
        public bool OnkoSessiossa(ISession sessio)
        {
            int? id = null;
            NeroStore.Apumetodit am = new Apumetodit(_context);
            var käyttäjä = from k in _context.Kayttajas
                           select k;
            foreach (var k in käyttäjä)
            {
                id = k.KayttajaId;
            }
            if (am.HaeAdmin(sessio) == id)
            {
                return true;
            }
            return false;
        }
    }
}
