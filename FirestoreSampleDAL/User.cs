using Google.Cloud.Firestore;

namespace FirestoreSampleDAL
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string? Nome { get; set; }

        [FirestoreProperty]
        public string? Email { get; set; }

        [FirestoreProperty]
        public int Idade { get; set; }

        [FirestoreProperty]
        public Timestamp DataCriacao { get; set; }
    }
}
