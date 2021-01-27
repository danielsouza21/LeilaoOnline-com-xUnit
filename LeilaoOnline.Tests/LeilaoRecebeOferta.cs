using Xunit;
using LeilaoOnline.Core;
using System.Linq;

namespace LeilaoOnline.Tests
{
    public class LeilaoRecebeOferta
    {
        [Theory]
        [InlineData(4, new double[] { 1000, 1200, 1400, 1300 })]
        [InlineData(2, new double[] { 800, 900 })]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado(
            int qtdeEsperada, double[] ofertas)
        {
            //Arranje - cenário
            var leilao = new Leilao("Van Gogh"); //modalidade padrão maiorValor()
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Count(); i++)
            {
                if((i%2)==0)
                    leilao.RecebeLance(fulano, ofertas[i]);
                else
                    leilao.RecebeLance(maria, ofertas[i]);
            }

            leilao.TerminaPregao();

            //Act - método sob teste
            leilao.RecebeLance(fulano, 1000);

            //Assert
            var qtdeObtida = leilao.Lances.Count();
            Assert.Equal(qtdeEsperada, qtdeObtida);
        }

        [Fact]
        public void NaoAceitaProximoLanceDadoMesmoClienteRealizouUltimoLance()
        {
            //Arranje - cenário
            var leilao = new Leilao("Van Gogh"); //modalidade padrão maiorValor()
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            leilao.RecebeLance(maria, 700);

            leilao.RecebeLance(fulano, 800);

            //Act - método sob teste
            leilao.RecebeLance(fulano, 1000);

            leilao.TerminaPregao();

            //Assert
            var qtdeEsperada = 2;
            var qtdeObtida = leilao.Lances.Count();
            Assert.Equal(qtdeEsperada, qtdeObtida);
        }
    }
}
