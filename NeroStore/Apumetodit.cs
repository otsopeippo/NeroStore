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
            //tuote = _context.Tuotes.Find(id);
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
                using (NeroStoreDBContext db = new NeroStoreDBContext())
                {
                    db.Tuotes.Add(uusiTuote);
                    db.SaveChanges();
                }
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
                using (NeroStoreDBContext db = new NeroStoreDBContext())
                {
                    tuote = db.Tuotes.Find(id);
                    tuote.Lkm = tuote.Lkm + muutos;
                    db.SaveChanges();
                }
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

        public void HaeOstoskori()
        {
            // koodi
            // ostoskorissa on vain id :itä, joten tämä metodi hakee iideet,
            // hakee niillä tuotteet ja palauttaa tuotteet




        }

        public void LisääOstoskoriin(ISession sessio, int id)
        {
            // koodi
            //var tuote = HaeTuote(id);
            //var tuoteJson = JsonConvert.SerializeObject(tuote);

            sessio.SetString("foo", "foobar");


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
    }
}
