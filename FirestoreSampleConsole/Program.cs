// See https://aka.ms/new-console-template for more information
using FirestoreSampleDAL;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Executing migrations...");
        FirestoreDAL firestoreDAL = new FirestoreDAL("testedofogliato", "testebanco3");
        FirestoreMigrator migrator = new FirestoreMigrator(firestoreDAL);

        await migrator.RunMigrationsAsync();
    }
}
