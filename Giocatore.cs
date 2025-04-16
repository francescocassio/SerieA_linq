namespace SerieAConsoleApp
{
    public class Giocatore
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Età { get; set; }
        public string Ruolo { get; set; }

        public int SquadraId { get; set; } 
        public Squadra Squadra { get; set; }
    }
}