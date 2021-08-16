using NeroStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeroStore
{
    public class Apumetodit
    {
        public void HaeTuotteet()
        {
            // koodi
        }

        public void LisääTuote()
        {
            // koodi
        }

        public void MuutaTuotetta()
        {
            // koodi
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

        public void KäyttäjäOnAdmin(int id)
        {
            NeroStoreDBContext db = new();
            var q = (from k in db.Kayttajas
                     where k.KayttajaId == id
                     select k).FirstOrDefault();
        }
    }
}
