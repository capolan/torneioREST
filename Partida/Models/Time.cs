using System;
using System.Collections.Generic;

#nullable disable

namespace Partida.Models
{
    public partial class Time
    {
        public Time()
        {
            Jogadoress = new HashSet<Jogadore>();
            JogoTime1s = new HashSet<Jogo>();
            JogoTime2s = new HashSet<Jogo>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime CriadoEm { get; set; }
        public int? Jogadores { get; set; }

        public virtual ICollection<Jogadore> Jogadoress { get; set; }
        public virtual ICollection<Jogo> JogoTime1s { get; set; }
        public virtual ICollection<Jogo> JogoTime2s { get; set; }
    }
}
