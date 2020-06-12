using System;
using System.Collections.Generic;
using System.Text;

namespace KlinikosInformacineSistema.Model
{
    public class Daktaras
    {
        public Daktaras()
        {
        }
        public Daktaras(int id, SkyriausKodas skyriausKodas)
        {
            Id = id;
            SkyriausKodas = skyriausKodas;
        }

        public int Id { get; set; }
        public SkyriausKodas SkyriausKodas { get; set; }


    }
}
