using System;

namespace SerieAConsoleApp
{
    public class Partita
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int GolCasa { get; set; }
        public int GolTrasferta { get; set; }

        public int SquadraCasaId { get; set; }
        public Squadra SquadraCasa { get; set; }

        public int SquadraTrasfertaId { get; set; }
        public Squadra SquadraTrasferta { get; set; } 
    }
}