using KlinikosInformacineSistema.Model;
using KlinikosInformacineSistema.Tests;
using System;
using System.Linq;

namespace KlinikosInformacineSistema.Processor
{
    public class KlinikosInformacineSistemaProcessor
    {
        private IFormosSaugykla _formosSaugykla;
        private IDaktaroSaugykla _daktaroSaugykla;

        public KlinikosInformacineSistemaProcessor()
        {
        }

        public KlinikosInformacineSistemaProcessor(IFormosSaugykla formosSaugykla, IDaktaroSaugykla daktaroSaugykla)
        {
            _formosSaugykla = formosSaugykla;
            _daktaroSaugykla = daktaroSaugykla;
        }

        public UzklausosFormosRezultatas FormosPatvirtinimas(UzklausosForma uzklausa)
        {
            if (uzklausa == null)
            {
                throw new ArgumentNullException(nameof(uzklausa));
            }
            var rezultatas = Create<UzklausosFormosRezultatas>(uzklausa);
            var laisviDaktarai = _daktaroSaugykla.GautiPrieinamusDaktarus(uzklausa.Data);
            if (laisviDaktarai.FirstOrDefault() is Daktaras laisvasDaktaras)
            {
                var daktaroForma = Create<DaktaroForma>(uzklausa);
                daktaroForma.DaktarasId = laisvasDaktaras.Id;

                _formosSaugykla.Issaugoti(daktaroForma);

                rezultatas.PasirinktasDaktaras = daktaroForma.Id;

                rezultatas.DaktaroPasirinkimoKodas = DaktaroPasirinkimoKodas.Sekmingas;
            }
            else
            {
                rezultatas.DaktaroPasirinkimoKodas = DaktaroPasirinkimoKodas.NeraLaisvoLaiko;
            }
            return rezultatas;
        }

        private T Create<T>(UzklausosForma uzklausa) where T : UzklausosFormaPagrindine, new()
        {
            return new T()
            {
                PacientoId = uzklausa.PacientoId,
                LigosAprasymas = uzklausa.LigosAprasymas,
                Data = uzklausa.Data
            };
        }
    }
}