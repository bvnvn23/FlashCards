using Microsoft.Data.SqlClient;
using Dapper;
using Spectre.Console;
using FlashCards;
using FlashCards.Models;

namespace FlashCards.Controllers
{
    static class StackController
    {
        const string databaseConnection = "Server=localhost;Database=FlashCards;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True";

        public static async Task InsertStack(string nameInput)
        {
            await using var connection = new SqlConnection(databaseConnection);
            var sql = $"INSERT INTO Stacks (Name) VALUES ('{nameInput}')";
            await connection.ExecuteAsync(sql);
        }

        public static async Task EditStack()
        {
            await using var connection = new SqlConnection(databaseConnection);
            var sql = "SELECT * FROM Stacks";
            var stacks = await connection.QueryAsync<Stacks>(sql);

            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks to edit.[/]");
                return;
            }

            var selectionPrompt = new SelectionPrompt<Stacks>()
                .Title("[cyan3]Select a Stack to Edit[/]")
                .HighlightStyle("cyan3")
                .PageSize(10)
                .UseConverter(stack => stack.Name)
                .AddChoices(stacks);

            var selectedStack = AnsiConsole.Prompt(selectionPrompt);
            var newName = DataValidations.GetUserInput("Enter the new name for the stack: ");
            var updateSql = $"UPDATE Stacks SET Name = '{newName}' WHERE Id = {selectedStack.Id}";
            await connection.ExecuteAsync(updateSql);
            AnsiConsole.MarkupLine("[green]Stack updated successfully![/]");
            AnsiConsole.MarkupLine("[green]Press any key to return to the main menu.[/]");
        }


        public static async Task DeleteStack()
        {
            await using var connection = new SqlConnection(databaseConnection);
            var sql = "SELECT * FROM Stacks";
            var stacks = await connection.QueryAsync<Stacks>(sql);
            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks to delete.[/]");
                return;
            }
            var selectionPrompt = new SelectionPrompt<Stacks>()
                .Title("[cyan3]Select a Stack to Delete[/]")
                .HighlightStyle("cyan3")
                .PageSize(10)
                .UseConverter(stack => stack.Name)
                .AddChoices(stacks);
            var selectedStack = AnsiConsole.Prompt(selectionPrompt);
            var deleteSql = $"DELETE FROM Stacks WHERE Id = {selectedStack.Id}";
            await connection.ExecuteAsync(deleteSql);
            AnsiConsole.MarkupLine("[green]Stack deleted successfully![/]");
            AnsiConsole.MarkupLine("[green]Press any key to return to the main menu.[/]");
        }

    }
}
