using Microsoft.Data.SqlClient;
using Dapper;
using Spectre.Console;
using FlashCards;
using FlashCards.Models;

namespace FlashCards.Controllers
{
    static class StackController
    {

        public static async Task InsertStack(string nameInput)
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
            var checkSql = $"SELECT * FROM Stacks WHERE Name = '{nameInput}'";
            var checkStack = await connection.QueryFirstOrDefaultAsync<Stacks>(checkSql);
            if (checkStack != null)
            {
                AnsiConsole.MarkupLine("[red]Stack already exists! Press enter to continue...[/]");
                Console.ReadKey();
                return;
            }

            var sql = $"INSERT INTO Stacks (Name) VALUES ('{nameInput}')";
            await connection.ExecuteAsync(sql);
        }

        public static async Task EditStack()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
            var sql = "SELECT * FROM Stacks";
            var stacks = await connection.QueryAsync<Stacks>(sql);

            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks to edit. Press enter to continue...[/]");
                Console.ReadKey();
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

        public static async Task ViewStack()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
            var sql = "SELECT * FROM Stacks";
            await connection.ExecuteAsync(sql);
            var stacks = await connection.QueryAsync<Stacks>(sql);
            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks to view.[/]");
                return;
            }

            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");

            foreach (var stack in stacks)
            {
                table.AddRow(stack.Id.ToString(), stack.Name);
            }
            AnsiConsole.Render(table);


            AnsiConsole.MarkupLine("[green]Press any key to return to the main menu.[/]");
            Console.ReadKey();
        }   

        public static async Task DeleteStack()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
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
            Console.ReadKey();
        }

    }
}
