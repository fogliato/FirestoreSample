using Google.Cloud.Firestore;

namespace FirestoreSampleDAL
{
    public class FirestoreMigrator
    {
        private readonly FirestoreDAL _firestoreDAL;

        public FirestoreMigrator(FirestoreDAL firestoreDAL)
        {
            _firestoreDAL = firestoreDAL;
        }

        public async Task RunMigrationsAsync()
        {
            Console.WriteLine("🚀 Starting migration...");
            List<User> users = GetUsers();
            await LoadInFirestoreDataBase(users);
            Console.WriteLine("✅ Migration Done!");
        }

        private static List<User> GetUsers()
        {
            return new List<User>
            {
                new User
                {
                    Nome = "Luiz Fogliato Junior",
                    Email = "fogliato.jr@gmail.com",
                    Idade = 28,
                    DataCriacao = Timestamp.GetCurrentTimestamp()
                },
                new User
                {
                    Nome = "Carlos Mendes",
                    Email = "carlos@email.com",
                    Idade = 35,
                    DataCriacao = Timestamp.GetCurrentTimestamp()
                },
                new User
                {
                    Nome = "João Silva",
                    Email = "joao@email.com",
                    Idade = 40,
                    DataCriacao = Timestamp.GetCurrentTimestamp()
                }
            };
        }

        private async Task LoadInFirestoreDataBase(List<User> users)
        {
            foreach (var user in users)
            {
                if (user.Email == null)
                {
                    Console.WriteLine($"⚠️ User {user.Nome} has no email, skipping...");
                    continue;
                }
                bool exists = await _firestoreDAL.CheckIfUserExistsAsync(user.Email);
                if (!exists)
                {
                    await _firestoreDAL.AddUserAsync(user);
                    Console.WriteLine($"✅ User {user.Nome} created.");
                }
                else
                {
                    Console.WriteLine($"⚠️ User {user.Nome} exists, skipping...");
                }
            }
        }
    }
}
