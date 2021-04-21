using System;
using System.Collections.Generic;

#nullable disable

namespace Partida.Models
{
    public partial class Jogo
    {
        public int Id { get; set; }
        public DateTime CriadoEm { get; set; }
        public int? Time1Id { get; set; }
        public int? Time2Id { get; set; }
        public int? Time1Gol { get; set; }
        public int? Time2Gol { get; set; }

        public virtual Time Time1 { get; set; }
        public virtual Time Time2 { get; set; }
    }
}
