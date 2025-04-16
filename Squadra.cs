using System.Collections.Generic;

namespace SerieAConsoleApp
{
    public class Squadra
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public string Città { get; set; }
        public List<Giocatore> Giocatori { get; set; }
        public List<Partita> PartiteInCasa { get; set; }
        public List<Partita> PartiteInTrasferta { get; set; }
    }
}