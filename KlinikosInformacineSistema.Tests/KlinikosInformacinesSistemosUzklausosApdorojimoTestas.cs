using KlinikosInformacineSistema.Model;
using KlinikosInformacineSistema.Processor;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace KlinikosInformacineSistema.Tests
{
    public class KlinikosInformacinesSistemosUzklausosApdorojimoTestas
    {
        private UzklausosForma _uzklausa;
        private List<Daktaras> _laisviDaktarai;
        private KlinikosInformacineSistemaProcessor _processor;
        private Mock<IFormosSaugykla> _formosSaugyklaMock;
        private Mock<IDaktaroSaugykla> _daktaroSaugyklaMock;

        public KlinikosInformacinesSistemosUzklausosApdorojimoTestas()
        {
            _uzklausa = new UzklausosForma
            {
                PacientoId = 0001,
                LigosAprasymas = "Luzo koja",
                Data = new DateTime(2020-06-10)
            };

            _laisviDaktarai = new List<Daktaras>() { new Daktaras(001, SkyriausKodas.Traumotologinis)};
            _formosSaugyklaMock = new Mock<IFormosSaugykla>();
            _daktaroSaugyklaMock = new Mock<IDaktaroSaugykla>();
            _daktaroSaugyklaMock.Setup(x => x.GautiPrieinamusDaktarus(_uzklausa.Data))
                .Returns(_laisviDaktarai);
            _processor = new KlinikosInformacineSistemaProcessor(_formosSaugyklaMock.Object, _daktaroSaugyklaMock.Object);
        }



        [Fact]
        public void TuriGrazintiUzklausosRezultataSuReiksmem()
        {
            //Arrange
            
            //Act
            UzklausosFormosRezultatas rezultatas = _processor.FormosPatvirtinimas(_uzklausa);

            //Assert
            Assert.NotNull(rezultatas);
            Assert.Equal(_uzklausa.PacientoId, rezultatas.PacientoId);
            Assert.Equal(_uzklausa.LigosAprasymas, rezultatas.LigosAprasymas);            
            Assert.Equal(_uzklausa.Data, rezultatas.Data);
        }

        [Fact]
        public void TuriIssaugotiUzklausosForma()
        {
            //arrange
            DaktaroForma issaugotaUzklausosForma = null;

            _formosSaugyklaMock.Setup(x => x.Issaugoti(It.IsAny<DaktaroForma>()))
                .Callback<DaktaroForma>(forma =>
                {
                    issaugotaUzklausosForma = forma;
                });
            //act
            _processor.FormosPatvirtinimas(_uzklausa);
            //assert
            _formosSaugyklaMock.Verify(x => x.Issaugoti(It.IsAny<DaktaroForma>()), Times.Once);

            Assert.NotNull(issaugotaUzklausosForma);
            Assert.Equal(_uzklausa.PacientoId, issaugotaUzklausosForma.PacientoId);
            Assert.Equal(_uzklausa.LigosAprasymas, issaugotaUzklausosForma.LigosAprasymas);
            Assert.Equal(_uzklausa.Data, issaugotaUzklausosForma.Data);            

            Assert.Equal(_laisviDaktarai.First().Id, issaugotaUzklausosForma.DaktarasId);
        }

        [Theory]
        [InlineData(DaktaroPasirinkimoKodas.Sekmingas, true)]
        [InlineData(DaktaroPasirinkimoKodas.NeraLaisvoLaiko, false)]
        public void TuriGrazintiTiketinoRezultatoKoda(DaktaroPasirinkimoKodas tiketinasRezultatoKodas, bool arPrieinamasDaktaras)
        {
            //arrange
            if (!arPrieinamasDaktaras)
            {
                _laisviDaktarai.Clear();
            }
            //act
            var rezultatas = _processor.FormosPatvirtinimas(_uzklausa);
            //assert
            Assert.Equal(tiketinasRezultatoKodas, rezultatas.DaktaroPasirinkimoKodas);
        }
    }
}
