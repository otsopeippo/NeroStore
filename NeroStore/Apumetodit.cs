using NeroStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
            using NeroStoreDBContext db = new();
            var q = (from k in db.Kayttajas
                     where k.KayttajaId == id
                     select k).FirstOrDefault().KayttajaId;
            return (q == id) ? true : false;
        }

        public bool KäyttäjäOnAdmin(int id)
        {
            using NeroStoreDBContext db = new();
            var onAdmin = (from k in db.Kayttajas
                     where k.KayttajaId == id
                     select k).FirstOrDefault().OnAdmin;
            return onAdmin;
        }

        public void LisaaTilausrivi(int lkm, int tilaus_id, int tuote_id )
        {
            TilausRivi tr = new TilausRivi() { 
                Lkm = lkm,
                TilausId = tilaus_id,
                TuoteId = tuote_id
            };
            using NeroStoreDBContext db = new();
            db.TilausRivis.Add(tr);
            db.SaveChanges();
        }
    }
}
