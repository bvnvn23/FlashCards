using Microsoft.Data.SqlClient;
using Dapper;
using Spectre.Console;

namespace FlashCards.Controllers
{
    static class DatabaseController
    {

        const string connectionString = "Server=localhost;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True";

        public static async Task CreateDatabase()
        {
            await using var connection = new SqlConnection(connectionString);

            var sql = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FlashCards') CREATE DATABASE FlashCards";

            await connection.ExecuteAsync(sql);
        }

        public static async Task CreateTables()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
            var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
            CREATE TABLE Stacks
    (
        Id INT PRIMARY KEY IDENTITY,
        Name NVARCHAR(100) NOT NULL
    )
    
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FlashCards') AND EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
    CREATE TABLE FlashCards
    (
        Id INT PRIMARY KEY IDENTITY,
        StackId INT NOT NULL,
        Front NVARCHAR(1000) NOT NULL,
        Back NVARCHAR(1000) NOT NULL,
        FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
    )

    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StudySessionHistory')
    CREATE TABLE StudySessionHistory
    (
        Id INT PRIMARY KEY IDENTITY,
        StackId INT NOT NULL,
        Score INT NOT NULL,
        Date DATETIME NOT NULL,
        FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE
    )
";
            await connection.ExecuteAsync(sql);
        }

    }
}
