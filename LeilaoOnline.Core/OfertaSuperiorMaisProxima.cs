using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeilaoOnline.Core
{
    public class OfertaSuperiorMaisProxima : IModalidadeAvaliacao
    {
        public double _valorDestino { get; }
        public OfertaSuperiorMaisProxima(double valorDestino)
        {
            _valorDestino = valorDestino;
        }

        public Lance Avalia(Leilao leilao)
        {
            //maior oferta mais proxima do valor de destino
            return leilao.Lances
                .DefaultIfEmpty(new Lance(null, 0))
                .Where(lance => lance.Valor > _valorDestino)
                .OrderBy(lance => lance.Valor)
                .FirstOrDefault();  //primeiro valor maior que o _valorDestino
        }
    }
}
