using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Dtos
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int CantidadMaximaPorPagina = 50;

        public int RecordsPorPagina
        {
            get { return recordsPorPagina; }
            set { recordsPorPagina = (value > CantidadMaximaPorPagina) ? CantidadMaximaPorPagina : value; }
        }


    }
}
