using System.Collections.Generic;
using System.Linq;

namespace LeilaoOnline.Core
{
    public enum EstadoLeilao
    {
        LeilaoEmAndamento,
        LeilaoFinalizado,
        LeilaoAntesDoPregao
    }

    public class Leilao
    {
        //privates prop
        private Interessada _ultimoCliente;
        private IList<Lance> _lances;
        private IModalidadeAvaliacao _avaliador;
        //publics prop
        public IEnumerable<Lance> Lances => _lances;
        public string Peca { get; }
        public Lance Ganhador { get; private set; }
        public EstadoLeilao Estado { get; private set; }


        public Leilao(string peca)
        {
            Peca = peca;
            _ultimoCliente = null;
            _lances = new List<Lance>();
            _avaliador = new MaiorValor(); //modalidade padrão
            Estado = EstadoLeilao.LeilaoAntesDoPregao;
        }

        public Leilao(string peca, IModalidadeAvaliacao avaliador)
        {
            Peca = peca;
            _ultimoCliente = null;
            _lances = new List<Lance>();
            _avaliador = avaliador;
            Estado = EstadoLeilao.LeilaoAntesDoPregao;
        }

        private bool VerificacaoLanceAceito(Interessada cliente, double valor)
        {
            if ((Estado == EstadoLeilao.LeilaoEmAndamento)
                && (_ultimoCliente != cliente))
            {
                return true;
            }

            return false;
        }

        public void RecebeLance(Interessada cliente, double valor)
        {
            if (VerificacaoLanceAceito(cliente, valor))
            {
                _lances.Add(new Lance(cliente, valor));
                _ultimoCliente = cliente;
            }
        }

        public void IniciaPregao()
        {
            Estado = EstadoLeilao.LeilaoEmAndamento;
        }

        public void TerminaPregao()
        {
            if (Estado != EstadoLeilao.LeilaoEmAndamento)
            {
                throw new System.InvalidOperationException("Não é possive fechar pregão sem abri-lo posteriormente [IniciaPregao()]");
            }

            Ganhador = _avaliador.Avalia(this); //this = LeilaoOnline.Core.Leilao

            Estado = EstadoLeilao.LeilaoFinalizado;
        }
    }
}
