using System.Linq;
using LeilaoOnline.Core;
using Xunit;

namespace LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1200, new double[] { 800, 900, 1000, 1200 })]
        [InlineData(1000, new double[] { 800, 900, 1000, 990 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaiorValorDadoLeilaoComPeloMenosUmLance(
            double valorEsperado, 
            double[] ofertas)
        {
            //Arranje - cenário
            var leilao = new Leilao("Van Gogh");    //modalidade padrão maiorValor()
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Count(); i++) 
            {
                if ((i % 2) == 0) //garante não input de usuarios consecutivos
                    leilao.RecebeLance(fulano, ofertas[i]);
                else
                    leilao.RecebeLance(maria, ofertas[i]);
            }

            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLances()
        {
            //Arranje - cenário
            var leilao = new Leilao("Van Gogh");    //modalidade padrão maiorValor()

            leilao.IniciaPregao();
            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        public void LancaExcecaoQuandoFinalizaPregaoSemAbertura()
        {
            //Testa se "TerminaPregao()" lança uma exceção quando não instanciado previamente "IniciaPregao()"
            //Arranje - cenário
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);

            var excecaoObtida = Assert.Throws<System.InvalidOperationException>(() => 
                //Act - método sob teste
                leilao.TerminaPregao()
            );

            var msgEsperada = "Não é possive fechar pregão sem abri-lo posteriormente [IniciaPregao()]";
            Assert.Equal(msgEsperada, excecaoObtida.Message);
        }

        [Theory]
        [InlineData(1200, 1250, new double[] { 800,1150,1400,1250})]
        public void RetornaValorSuperiorMaisProximoDestino(double valorDestino, double valorEsperado, double[] ofertas)
        {
            //Arranje - cenário
            //Valor aguardado seria 1250 quando valor destino de 1200
            var modalidade = new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Count(); i++)
            {
                if ((i % 2) == 0) //garante input de clientes alternados
                    leilao.RecebeLance(fulano, ofertas[i]);
                else
                    leilao.RecebeLance(maria, ofertas[i]);
            }

            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
