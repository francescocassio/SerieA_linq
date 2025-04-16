//using System;
//using System.Collections.Generic;
//using Microsoft.Data.SqlClient;
//using System.Linq;

//namespace SerieAConsoleAppRaw 
//{
//    public class Squadra
//    {
//        public int Id { get; set; }
//        public string Nome { get; set; }
//        public string Città { get; set; }
//    }

//    public class Giocatore
//    {
//        public int Id { get; set; }
//        public string Nome { get; set; }
//        public int Età { get; set; }
//        public string Ruolo { get; set; }
//        public int SquadraId { get; set; }
//    }

//    class Program
//    {
//        static string connString = "Server=localhost;Database=SerieA;Trusted_Connection=True;";

//        static void Main()
//        {
//            while (true)
//            {
//                Console.Clear();
//                Console.WriteLine("📊 MENU SERIE A");
//                Console.WriteLine("1. Tutte le squadre");
//                Console.WriteLine("2. Squadre ordinate per nome");
//                Console.WriteLine("3. Squadre che iniziano con 'M'");
//                Console.WriteLine("4. Giocatori over 30");
//                Console.WriteLine("5. Giocatori ordinati per età decrescente");
//                Console.WriteLine("6. Conteggio giocatori per ruolo");
//                Console.WriteLine("7. Squadre con più di 5 giocatori");
//                Console.WriteLine("8. Età media giocatori per squadra");
//                Console.WriteLine("9. Giocatori con nome più lungo di 10 caratteri");
//                Console.WriteLine("10. Squadre senza giocatori");
//                Console.WriteLine("0. Esci");
//                Console.Write("Scelta: ");
//                var scelta = Console.ReadLine();
//                Console.WriteLine();

//                var squadre = GetSquadre();
//                var giocatori = GetGiocatori();

//                switch (scelta)
//                {
//                    case "1":
//                        // Stampa tutte le squadre
//                        foreach (var s in squadre)
//                            Console.WriteLine($"{s.Nome} - {s.Città}");
//                        break;

//                    case "2":
//                        // LINQ: ordina le squadre per nome
//                        var ord = squadre
//                            .OrderBy(s => s.Nome);

//                        foreach (var s in ord)
//                            Console.WriteLine(s.Nome);
//                        break;

//                    case "3":
//                        // LINQ: filtra squadre che iniziano con 'M'
//                        var filtro = squadre
//                            .Where(s => s.Nome.StartsWith("M"));

//                        foreach (var s in filtro)
//                            Console.WriteLine(s.Nome);
//                        break;

//                    case "4":
//                        // LINQ: giocatori con età > 30
//                        var senior = giocatori
//                            .Where(g => g.Età > 30);

//                        foreach (var g in senior)
//                            Console.WriteLine($"{g.Nome} - {g.Età} anni");
//                        break;

//                    case "5":
//                        // LINQ: giocatori ordinati per età decrescente
//                        var ordinati = giocatori
//                            .OrderByDescending(g => g.Età);

//                        foreach (var g in ordinati)
//                            Console.WriteLine($"{g.Nome} - {g.Età} anni");
//                        break;

//                    case "6":
//                        // LINQ: conta giocatori per ruolo
//                        var perRuolo = giocatori
//                            .GroupBy(g => g.Ruolo)
//                            .Select(g => new {
//                                Ruolo = g.Key,
//                                Totale = g.Count()
//                            });

//                        foreach (var r in perRuolo)
//                            Console.WriteLine($"{r.Ruolo}: {r.Totale} giocatori");
//                        break;

//                    case "7":
//                        // LINQ: squadre con più di 5 giocatori
//                        var conMolti = squadre
//                            .Where(s => giocatori.Count(g => g.SquadraId == s.Id) > 5);

//                        foreach (var s in conMolti)
//                            Console.WriteLine($"{s.Nome}");
//                        break;

//                    case "8":
//                        // LINQ: età media per squadra
//                        var media = squadre
//                            .Select(s => new {
//                                s.Nome,
//                                Media = giocatori.Where(g => g.SquadraId == s.Id).Any()
//                                    ? giocatori.Where(g => g.SquadraId == s.Id).Average(g => g.Età)
//                                    : 0
//                            });

//                        foreach (var s in media)
//                            Console.WriteLine($"{s.Nome}: età media {s.Media:F1}");
//                        break;

//                    case "9":
//                        // LINQ: giocatori con nome lungo
//                        var lunghi = giocatori
//                            .Where(g => g.Nome.Length > 10);

//                        foreach (var g in lunghi)
//                            Console.WriteLine(g.Nome);
//                        break;

//                    case "10":
//                        // LINQ: squadre senza giocatori
//                        // Usiamo .Where() per filtrare solo le squadre che NON hanno alcun giocatore associato
//                        var senza = squadre
//                            .Where(s =>                     // Where: filtro su ogni squadra "s"
//                                !giocatori.Any(g =>        // Any: verifica se ESISTE almeno un giocatore "g"...
//                                    g.SquadraId == s.Id    // ...il cui SquadraId corrisponde all'Id della squadra corrente
//                                )
//                            );
//                        foreach (var s in senza)
//                            Console.WriteLine($"{s.Nome} - Nessun giocatore");
//                        break;

//                    case "0":
//                        return;

//                    default:
//                        Console.WriteLine("❌ Scelta non valida.");
//                        break;
//                }

//                Console.WriteLine("\nPremi un tasto per continuare...");
//                Console.ReadKey();
//            }
//        }

//        // Metodo che restituisce una lista di oggetti Squadra letti dal database
//        static List<Squadra> GetSquadre()
//        {
//            // Crea una lista vuota per contenere le squadre da restituire
//            var result = new List<Squadra>();

//            // Crea una nuova connessione SQL usando la stringa di connessione globale (connString)
//            using var conn = new SqlConnection(connString);

//            // Apre la connessione al database
//            conn.Open();

//            // Prepara un comando SQL per selezionare le colonne dalla tabella Squadre
//            using var cmd = new SqlCommand("SELECT Id, Nome, Città FROM Squadre", conn);

//            // Esegue il comando e ottiene un lettore per leggere i risultati riga per riga
//            using var reader = cmd.ExecuteReader();

//            // Cicla su ogni riga restituita dal lettore
//            while (reader.Read())
//            {
//                // Crea un nuovo oggetto Squadra e lo riempie con i dati letti
//                result.Add(new Squadra
//                {
//                    // Legge la colonna 0 (Id) come intero
//                    Id = reader.GetInt32(0),

//                    // Legge la colonna 1 (Nome) come stringa
//                    Nome = reader.GetString(1),

//                    // Legge la colonna 2 (Città) come stringa
//                    Città = reader.GetString(2)
//                });
//            }

//            // Restituisce la lista completa delle squadre lette dal database
//            return result;
//        }


//        // Metodo che restituisce una lista di oggetti Giocatore letti dal database
//        static List<Giocatore> GetGiocatori()
//        {
//            // Crea una lista vuota che conterrà tutti i giocatori recuperati
//            var result = new List<Giocatore>();

//            // Crea una nuova connessione SQL usando la stringa di connessione definita (connString)
//            using var conn = new SqlConnection(connString);

//            // Apre effettivamente la connessione al database
//            conn.Open();

//            // Prepara il comando SQL da eseguire: seleziona i campi dalla tabella Giocatori
//            using var cmd = new SqlCommand("SELECT Id, Nome, Età, Ruolo, SquadraId FROM Giocatori", conn);

//            // Esegue il comando e ottiene un oggetto SqlDataReader per leggere riga per riga
//            using var reader = cmd.ExecuteReader();

//            // Scorre tutte le righe del risultato
//            while (reader.Read())
//            {
//                // Per ogni riga, crea un nuovo oggetto Giocatore e lo aggiunge alla lista
//                result.Add(new Giocatore
//                {
//                    // Legge la colonna 0 (Id) come intero
//                    Id = reader.GetInt32(0),

//                    // Legge la colonna 1 (Nome) come stringa
//                    Nome = reader.GetString(1),

//                    // Legge la colonna 2 (Età) come intero
//                    Età = reader.GetInt32(2),

//                    // Legge la colonna 3 (Ruolo) come stringa
//                    Ruolo = reader.GetString(3),

//                    // Legge la colonna 4 (SquadraId) come intero
//                    SquadraId = reader.GetInt32(4)
//                });
//            }

//            // Restituisce l'elenco completo dei giocatori
//            return result;
//        }

//    }
//}