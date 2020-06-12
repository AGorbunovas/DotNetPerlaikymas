using KlinikosInformacineSistema.Model;
using System;
using System.Collections.Generic;

namespace KlinikosInformacineSistema.Tests
{
    public interface IDaktaroSaugykla
    {
        List<Daktaras> GautiPrieinamusDaktarus(DateTime data);
    }
}