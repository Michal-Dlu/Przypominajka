using Microsoft.Data.Sqlite;
using Przypominajka.Models;
namespace Przypominajka.Services
{
    public class DatabaseService
    {
        private readonly string _dbPath;

        // Constructor to initialize the database path

        public DatabaseService(string dbPath)
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, dbPath);
            Console.WriteLine("Ścieżka do bazy danych: " + _dbPath);
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Leki (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        nazwa TEXT NOT NULL, dawkowanie TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();

                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS GodzinyPodania (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        godzina INTEGER NOT NULL,
                        LekId INTEGER NOT NULL,
                        FOREIGN KEY (LekId) REFERENCES Leki(Id)
                    );";
                command.ExecuteNonQuery();
            }
        }
        public async Task<List<Lek>> PobierzLek()
        {
            Console.WriteLine("Pobieram dane z bazy...");
            var leki = new List<Lek>();
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id,nazwa,dawkowanie FROM Leki";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var lek = new Lek
                        {
                            Id = reader.GetInt32(0),
                            nazwa = reader.GetString(1),
                            dawkowanie = reader.GetString(2)
                        };
                        leki.Add(lek);
                    }
                }
            }
            return leki;
        }
        public void DodajLek(Lek lek)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Leki (nazwa, dawkowanie) VALUES (@nazwa, @dawkowanie)";
                command.Parameters.AddWithValue("@nazwa", lek.nazwa);
                command.Parameters.AddWithValue("@dawkowanie", lek.dawkowanie);
                command.ExecuteNonQuery();
                // command.CommandText = "SELECT Id from Leki order by Id desc limit 1";
                command.CommandText = "SELECT last_insert_rowid()";
                lek.Id = Convert.ToInt32((long)command.ExecuteScalar());
            }
        }

        public void DodajGodzinePodania(GodzinyPodania godzinyPodania, Lek lek)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // Wstawianie godziny podania dla danego leku

                command.CommandText = "INSERT INTO GodzinyPodania (godzina, LekId) VALUES (@godzina, @LekId)";
                command.Parameters.AddWithValue("@godzina", godzinyPodania.godzina);
                command.Parameters.AddWithValue("@LekId", lek.Id);
                command.ExecuteNonQuery();
            }
        }

        public async Task<List<GodzinyPodania>> PobierzGodzinyPodania()
        {
            var godzinyPodania = new List<GodzinyPodania>();
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, godzina, LekId FROM GodzinyPodania";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        godzinyPodania.Add(new GodzinyPodania
                        {
                            Id = reader.GetInt32(0),
                            godzina = reader.GetInt32(1),
                            LekId = reader.GetInt32(2)
                        });
                    }
                }
            }
            return godzinyPodania;
        }

        public void UsunLek_i_GodzinePodania(int LekId)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Usuwanie godzin podania dla danego leku
                        var command = connection.CreateCommand();
                        command.CommandText = "DELETE FROM GodzinyPodania WHERE LekId = @LekId";
                        command.Parameters.AddWithValue("@LekId", LekId);
                        command.ExecuteNonQuery();
                        // Usuwanie leku
                        command.CommandText = "DELETE FROM Leki WHERE Id = @LekId";
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
