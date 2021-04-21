using System;
using System.Collections.Generic;

#nullable disable

namespace Partida.Models
{
    public partial class Jogadore
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? TimesId { get; set; }

        public virtual Time Times { get; set; }
    }
}
