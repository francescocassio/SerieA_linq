using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using SerieAConsoleApp;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace SerieAConsoleAppRaw 
{
    public class Squadra
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Città { get; set; }
    }

    public class Giocatore
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Età { get; set; }
        public string Ruolo { get; set; }
        public int SquadraId { get; set; }
    }

    class Program
    {
        static string connString = "Server=LAPTOP-V3K3MMCB\\SQLEXPRESS;Database=SerieA;Trusted_Connection=True;TrustServerCertificate=True;";



        static void Main()
        {
            var continua = 1;
            while (continua == 1)
            {
                Console.Clear();
                Console.WriteLine("📊 MENU SERIE A");
                Console.WriteLine("1. Tutte le squadre");
                Console.WriteLine("2. Squadre ordinate per nome");
                Console.WriteLine("3. Squadre che iniziano con 'M'");
                Console.WriteLine("4. Giocatori over 30");
                Console.WriteLine("5. Giocatori ordinati per età decrescente");
                Console.WriteLine("6. Conteggio giocatori per ruolo");
                Console.WriteLine("7. Squadre con più di 5 giocatori");
                Console.WriteLine("8. Età media giocatori per squadra");
                Console.WriteLine("9. Giocatori con nome più lungo di 10 caratteri");
                Console.WriteLine("10. Squadre senza giocatori");
                Console.WriteLine("11. Giocatori con nome della squadra (JOIN)");
                Console.WriteLine("12. Giocatori di squadre di Milano (JOIN)");
                Console.WriteLine("13. Giocatori ordinati per città della squadra (JOIN)");
                Console.WriteLine("14. Numero di giocatori per squadra (JOIN + GROUP)");
                Console.WriteLine("15. Età media dei giocatori per squadra (JOIN + GROUP)");
                Console.WriteLine("0. Esci");
                Console.Write("Scelta: ");
                var scelta = Console.ReadLine();
                Console.WriteLine();

                var squadre = GetSquadre();
                var giocatori = GetGiocatori();
                var partite = GetPartite();

                switch (scelta)
                {
                    case "1":
                        // Query: seleziona tutte le squadre
                        //var tutte =
                        //    from s in squadre
                        //    select s;
                        var tutte = squadre.Select(s => s);

                        // Stampa nome e città di ciascuna squadra
                        foreach (var s in tutte)
                            Console.WriteLine($"{s.Nome} - {s.Città}");
                        break;

                    case "2":
                        // Query: ordina le squadre per nome in ordine alfabetico
                        var ord = squadre.OrderBy(s => s.Nome);
                        //from s in squadre
                        //orderby s.Nome
                        //select s;

                        // Stampa i nomi delle squadre ordinate
                        foreach (var s in ord)
                            Console.WriteLine(s.Nome);
                        break;

                    case "3":
                        // Query: seleziona squadre che iniziano con la lettera "M"
                        var filtro = squadre
                            .OrderBy(s => s.Nome)
                            .Where(s => s.Nome.StartsWith("M"));
                        //from s in squadre
                        //where s.Nome.StartsWith("M")
                        //select s;

                        // Stampa le squadre filtrate
                        foreach (var s in filtro)
                            Console.WriteLine(s.Nome);
                        break;

                    case "4":
                        // Query: seleziona i giocatori con età superiore a 30 anni
                        var senior = giocatori
                            .Where(g => g.Età > 30);
                            //from g in giocatori
                            //where g.Età > 30
                            //select g;

                        // Stampa nome ed età dei giocatori trovati
                        foreach (var g in senior)
                            Console.WriteLine($"{g.Nome} - {g.Età} anni");
                        break;

                    case "5":
                        // Query: ordina i giocatori per età in ordine decrescente
                        var ordinati = giocatori
                            .OrderByDescending(g => g.Età);
                            //from g in giocatori
                            //orderby g.Età descending
                            //select g;

                        // Stampa i giocatori ordinati
                        foreach (var g in ordinati)
                            Console.WriteLine($"{g.Nome} - {g.Età} anni");
                        break;

                    case "6":
                        // Query: raggruppa i giocatori per ruolo e conta quanti ce ne sono
                        var perRuolo = giocatori
                            .GroupBy(g => g.Ruolo);
                            //.Select(g => new
                            //{
                            //    Ruolo = g.Key,
                            //    Totale = g.Count()
                            //});
                        //from g in giocatori
                        //group g by g.Ruolo into gruppo
                        //select new
                        //{
                        //    Ruolo = gruppo.Key,          // Chiave del gruppo = nome del ruolo
                        //    Totale = gruppo.Count()      // Numero di elementi nel gruppo
                        //};

                        // Stampa il numero di giocatori per ogni ruolo
                        foreach (var r in perRuolo)
                        {
                            Console.WriteLine($"{r.Key}: {r.Count()} giocatori");
                            foreach (var g in r)
                            {
                                Console.WriteLine($"{g.Nome}");
                            }
                            Console.WriteLine("--------------------");
                        }
                        break;

                    case "7":
                        // Query: seleziona le squadre con più di 5 giocatori
                        var conMolti = giocatori
                            .GroupBy(g => g.SquadraId)
                            .Where(g => g.Count() > 5);
                            
                        //var squadre = squadre
                        //    .Where(s => s.Id = g.Key);
                            //from g in giocatori
                            //from s in squadre
                            //where
                            //    (from g in giocatori
                            //     where g.SquadraId == s.Id
                            //     select g).Count() > 5
                            //select s;

                        // Stampa i nomi delle squadre trovate
                        foreach (var s in conMolti)
                            Console.WriteLine(s.Nome);
                        break;

                    case "8":
                        // Definiamo una query LINQ che calcola l'età media dei giocatori per ogni squadra
                        var media =
                            from s in squadre                     // Ciclo su ogni squadra nella lista `squadre`

                                // LET: creo una sottoquery che recupera i giocatori appartenenti alla squadra corrente
                            let giocatoriSquadra = (
                                from g in giocatori              // Ciclo su ogni giocatore
                                where g.SquadraId == s.Id        // Condizione: solo i giocatori della squadra corrente
                                select g                         // Seleziona il giocatore
                            )

                            // Proiezione: restituisce un oggetto anonimo con il nome della squadra e l'età media
                            select new
                            {
                                s.Nome,                          // Nome della squadra

                                // Calcolo della media solo se ci sono giocatori, altrimenti mettiamo 0
                                Media = giocatoriSquadra.Any()  // Se la lista ha almeno un giocatore
                                    ? giocatoriSquadra.Average(g => g.Età) // allora calcola la media dell'età
                                    : 0                          // altrimenti restituisci 0 come età media
                            };


                        // Stampa il nome della squadra e la media età
                        foreach (var s in media)
                            Console.WriteLine($"{s.Nome}: età media {s.Media:F1}");
                        break;

                    case "9":
                        // Query: seleziona i giocatori con nome più lungo di 10 caratteri
                        var lunghi =
                            from g in giocatori
                            where g.Nome.Length > 10
                            select g;

                        // Stampa i nomi trovati
                        foreach (var g in lunghi)
                            Console.WriteLine(g.Nome);
                        break;

                    case "10":
                        // Query: seleziona le squadre che non hanno alcun giocatore associato
                        var senza =                                  // Salviamo il risultato della query in una variabile chiamata "senza"
                            from s in squadre                        // Cicliamo su ogni squadra presente nella lista "squadre"

                            where                                    // Applichiamo un filtro

                                !(                                   // Negazione del risultato: vogliamo le squadre che NON soddisfano la condizione seguente

                                    from g in giocatori             // Cicliamo su tutti i giocatori
                                    where g.SquadraId == s.Id       // Selezioniamo solo quelli che appartengono alla squadra corrente (s)
                                    select g                        // Selezioniamo il giocatore

                                ).Any()                             // Verifica se almeno un giocatore è stato trovato (true/false)

                            select s;                               // Se il risultato è false (cioè non ha giocatori), includiamo la squadra nei risultati


                        // Stampa le squadre senza giocatori
                        foreach (var s in senza)
                            Console.WriteLine($"{s.Nome} - Nessun giocatore");
                        break;

                    case "11":
                        // JOIN tra giocatori e squadre: accoppia ogni giocatore alla sua squadra
                        var joinBase =
                            from g in giocatori                  // ciclo sui giocatori
                            join s in squadre                   // fai join con le squadre
                            on g.SquadraId equals s.Id          // condizione di join: ID squadra
                            select new                          // proietta il risultato
                            {
                                g.Nome,                         // nome del giocatore
                                g.Ruolo,                        // ruolo del giocatore
                                Squadra = s.Nome                // nome della squadra
                            };

                        // stampa i risultati
                        foreach (var x in joinBase)
                            Console.WriteLine($"{x.Nome} ({x.Ruolo}) - {x.Squadra}");
                        break;

                    case "12":
                        // JOIN filtrata: mostra solo i giocatori di squadre di Milano
                        var joinMilano =
                            from g in giocatori
                            join s in squadre
                            on g.SquadraId equals s.Id
                            where s.Città == "Milano"           // filtro sulla città della squadra
                            select new
                            {
                                g.Nome,                         // nome del giocatore
                                Squadra = s.Nome                // nome della squadra
                            };

                        foreach (var x in joinMilano)
                            Console.WriteLine($"{x.Nome} - {x.Squadra}");
                        break;

                    case "13":
                        // JOIN + ORDERBY: ordina i giocatori in base alla città della squadra
                        var joinOrdinati =
                            from g in giocatori
                            join s in squadre
                            on g.SquadraId equals s.Id
                            orderby s.Città                     // ordinamento per città
                            select new
                            {
                                g.Nome,
                                s.Città
                            };

                        foreach (var x in joinOrdinati)
                            Console.WriteLine($"{x.Nome} - {x.Città}");
                        break;

                    case "14":
                        // JOIN + GROUP BY: raggruppa per nome squadra e conta i giocatori
                        var gruppoJoin =
                            from g in giocatori
                            join s in squadre
                            on g.SquadraId equals s.Id
                            group g by s.Nome into gruppo       // raggruppa i giocatori per nome squadra
                            select new
                            {
                                Squadra = gruppo.Key,           // Key = nome della squadra
                                Totale = gruppo.Count()         // numero totale di giocatori
                            };

                        foreach (var x in gruppoJoin)
                            Console.WriteLine($"{x.Squadra}: {x.Totale} giocatori");
                        break;

                    case "15":
                        // JOIN + GROUP BY + AVERAGE: età media per squadra
                        var mediaJoin =
                            from g in giocatori
                            join s in squadre
                            on g.SquadraId equals s.Id
                            group g by s.Nome into gruppo       // raggruppa i giocatori per squadra
                            select new
                            {
                                Squadra = gruppo.Key,
                                Media = gruppo.Average(g => g.Età)  // calcola media dell’età
                            };

                        foreach (var x in mediaJoin)
                            Console.WriteLine($"{x.Squadra}: Età media {x.Media:F1}");
                        break;

                    

                    case "0":
                        continua = 0;
                        break;
                }

                Console.WriteLine("\nPremi un tasto per continuare...");
                Console.ReadKey();
            }
        }

        // Metodo che restituisce una lista di oggetti Squadra letti dal database
        static List<Squadra> GetSquadre()
        {
            // Crea una lista vuota per contenere le squadre da restituire
            var result = new List<Squadra>();

            // Crea una nuova connessione SQL usando la stringa di connessione globale (connString)
            using var conn = new SqlConnection(connString);

            // Apre la connessione al database
            conn.Open();

            // Prepara un comando SQL per selezionare le colonne dalla tabella Squadre
            using var cmd = new SqlCommand("SELECT Id, Nome, Città FROM Squadre", conn);

            // Esegue il comando e ottiene un lettore per leggere i risultati riga per riga
            using var reader = cmd.ExecuteReader();

            // Cicla su ogni riga restituita dal lettore
            while (reader.Read())
            {
                // Crea un nuovo oggetto Squadra e lo riempie con i dati letti
                result.Add(new Squadra
                {
                    // Legge la colonna 0 (Id) come intero
                    Id = reader.GetInt32(0),

                    // Legge la colonna 1 (Nome) come stringa
                    Nome = reader.GetString(1),

                    // Legge la colonna 2 (Città) come stringa
                    Città = reader.GetString(2)
                });
            }

            // Restituisce la lista completa delle squadre lette dal database
            return result;
        }


        // Metodo che restituisce una lista di oggetti Giocatore letti dal database
        static List<Giocatore> GetGiocatori()
        {
            // Crea una lista vuota che conterrà tutti i giocatori recuperati
            var result = new List<Giocatore>();

            // Crea una nuova connessione SQL usando la stringa di connessione definita (connString)
            using var conn = new SqlConnection(connString);

            // Apre effettivamente la connessione al database
            conn.Open();

            // Prepara il comando SQL da eseguire: seleziona i campi dalla tabella Giocatori
            using var cmd = new SqlCommand("SELECT Id, Nome, Età, Ruolo, SquadraId FROM Giocatori", conn);

            // Esegue il comando e ottiene un oggetto SqlDataReader per leggere riga per riga
            using var reader = cmd.ExecuteReader();

            // Scorre tutte le righe del risultato
            while (reader.Read())
            {
                // Per ogni riga, crea un nuovo oggetto Giocatore e lo aggiunge alla lista
                result.Add(new Giocatore
                {
                    // Legge la colonna 0 (Id) come intero
                    Id = reader.GetInt32(0),

                    // Legge la colonna 1 (Nome) come stringa
                    Nome = reader.GetString(1),

                    // Legge la colonna 2 (Età) come intero
                    Età = reader.GetInt32(2),

                    // Legge la colonna 3 (Ruolo) come stringa
                    Ruolo = reader.GetString(3),

                    // Legge la colonna 4 (SquadraId) come intero
                    SquadraId = reader.GetInt32(4)
                });
            }

            // Restituisce l'elenco completo dei giocatori
            return result;
        }

        // Metodo che restituisce una lista di partite dal database
        static List<Partita> GetPartite()
        {
            // Crea una lista vuota che conterrà le partite
            var result = new List<Partita>();

            // Crea una connessione SQL usando la stringa di connessione globale
            using var conn = new SqlConnection(connString);

            // Apre la connessione al database
            conn.Open();

            // Prepara il comando SQL per leggere i dati dalla tabella Partite
            //@"SELECT Id, Data FROM Partite"
            //verbatim string, cioè una stringa verbatim in C#. Questo significa:

            //Puoi andare a capo senza usare \n

            //I caratteri speciali(come \) non vengono interpretati

            //È molto utile per query SQL multilinea o percorsi file Windows
            using var cmd = new SqlCommand(@"
        SELECT Id, Data, GolCasa, GolTrasferta, SquadraCasaId, SquadraTrasfertaId 
        FROM Partite", conn);

            // Esegue il comando e ottiene un lettore per leggere i dati riga per riga
            using var reader = cmd.ExecuteReader();

            // Cicla su ogni riga restituita
            while (reader.Read())
            {
                // Aggiunge una nuova partita alla lista leggendo i dati dalla riga corrente
                result.Add(new Partita
                {
                    Id = reader.GetInt32(0),                           // Colonna 0: Id
                    Data = reader.GetDateTime(1),                      // Colonna 1: Data (tipo DateTime)
                    GolCasa = reader.GetInt32(2),                      // Colonna 2: GolCasa
                    GolTrasferta = reader.GetInt32(3),                 // Colonna 3: GolTrasferta
                    SquadraCasaId = reader.GetInt32(4),                // Colonna 4: SquadraCasaId
                    SquadraTrasfertaId = reader.GetInt32(5)            // Colonna 5: SquadraTrasfertaId
                });
            }

            // Restituisce la lista di partite
            return result;
        }


    }
}