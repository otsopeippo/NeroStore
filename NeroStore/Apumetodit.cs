﻿using NeroStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeroStore
{
    public class Apumetodit
    {
        public List<Tuote> HaeTuotteet()
        {
            var tuotteet = new List<Tuote> { };
            using (NeroStoreDBContext db = new NeroStoreDBContext())
            {
                tuotteet = db.Tuotes.Select(tuote => tuote).ToList();
            }
            return tuotteet;
        }

        public Tuote HaeTuote(int id)
        {
            var tuote = new Tuote();
            using (NeroStoreDBContext db = new NeroStoreDBContext())
            {
                tuote = db.Tuotes.Find(id);
            }
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
        }

        public void LisääOstoskoriin()
        {
            // koodi
        }

        public void PoistaOstoskorista()
        {
            // koodi
        }

        public void KäyttäjäOnOlemassa()
        {
            // koodi
        }

        public void KäyttäjäOnAdmin()
        {
            // koodi
        }
    }
}
