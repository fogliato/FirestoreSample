using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;

namespace FirestoreSampleDAL
{
    public class FirestoreDAL
    {
        private readonly FirestoreDb _db;

        public FirestoreDAL(string projectId, string databaseId)
        {
            // Add debug log
            Console.WriteLine($"Starting connection with project: {projectId}");

            // Path for the JSON file with the Firebase credentials. Replace it with your own path.
            string jsonFilePath =
                @"C:\Projects\PESSOAIS\FirestoreSample\FirestoreSampleConsole\testedofogliato.json";
            // reading the JSON file
            string credentialsJson = File.ReadAllText(jsonFilePath);

            // Add log to verify if credentials were loaded
            Console.WriteLine("Credentials loaded successfully");

            /*
            // get from environment variable
            string credentialsJson =
                Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_JSON") ?? string.Empty;
            */
            if (string.IsNullOrEmpty(credentialsJson))
                throw new Exception(
                    "The environment variable FIREBASE_CREDENTIALS_JSON was not found or is empty."
                );
            GoogleCredential credential = GoogleCredential.FromJson(credentialsJson);
            _db = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                DatabaseId = databaseId,
                // Credenciais podem ser definidas aqui ou via ambiente
                Credential = credential
            }.Build();

            // Add final log
            Console.WriteLine("Connection established successfully");
        }

        public async Task<string?> AddUserAsync(User user)
        {
            try
            {
                Console.WriteLine($"Adding user: {user.Email}");
                CollectionReference usersRef = _db.Collection("usuarios");
                Console.WriteLine("Reference to the collection obtained");
                DocumentReference docRef = await usersRef.AddAsync(user);
                Console.WriteLine("User added successfully");
                return docRef.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            try
            {
                DocumentReference docRef = _db.Collection("usuarios").Document(userId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                    return snapshot.ConvertTo<User>();

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return null;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                QuerySnapshot querySnapshot = await _db.Collection("usuarios").GetSnapshotAsync();
                List<User> users = new List<User>();

                foreach (DocumentSnapshot doc in querySnapshot.Documents)
                {
                    users.Add(doc.ConvertTo<User>());
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<bool> UpdateUserAsync(string userId, User user)
        {
            try
            {
                DocumentReference docRef = _db.Collection("usuarios").Document(userId);
                await docRef.SetAsync(user, SetOptions.MergeAll);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                DocumentReference docRef = _db.Collection("usuarios").Document(userId);
                await docRef.DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckIfUserExistsAsync(string email)
        {
            Query query = _db.Collection("usuarios").WhereEqualTo("Email", email);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            return snapshot.Documents.Count > 0;
        }
    }
}
