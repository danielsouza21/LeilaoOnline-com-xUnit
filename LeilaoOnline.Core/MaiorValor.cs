using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeilaoOnline.Core
{
    public class MaiorValor : IModalidadeAvaliacao
    {
        public Lance Avalia(Leilao leilao)
        {
            //modalidade padrão => vence quem tem o maior valor
            return leilao.Lances
                .DefaultIfEmpty(new Lance(null, 0))
                .OrderBy(l => l.Valor)
                .LastOrDefault();
        }
    }
}
