using Microsoft.Data.SqlClient;
using Dapper;
using FlashCards.Models;
using FlashCards.Dtos;
using Spectre.Console;

namespace FlashCards.Controllers
{
    static class FlashCardsController
    {
       
        public static async Task AddFlashCard()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
            var sql = "SELECT * FROM Stacks";
            var stacks = await connection.QueryAsync<Stacks>(sql);

            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks to add flashcards to. Press enter to continue...[/]");
                Console.ReadLine();
                return;
            }

            var selectionPrompt = new SelectionPrompt<Stacks>()
                .Title("[cyan3]Select a Stack to Add Flashcards to[/]")
                .HighlightStyle("cyan3")
                .PageSize(10)
                .UseConverter(stack => stack.Name)
                .AddChoices(stacks);

            var selectedStack = AnsiConsole.Prompt(selectionPrompt);

            var front = DataValidations.GetUserInput("Enter the front of the flashcard: ");
            var back = DataValidations.GetUserInput("Enter the back of the flashcard: ");

            var insertSql = $"INSERT INTO FlashCards (StackId, Front, Back) VALUES ({selectedStack.Id}, '{front}', '{back}')";

            await connection.ExecuteAsync(insertSql);

            AnsiConsole.MarkupLine("[green]Flashcard added successfully![/]");
            AnsiConsole.MarkupLine("[green]Press any key to return to the main menu.[/]");
        }

        public static async Task ViewFlashCard()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
            var sql = "SELECT * FROM Stacks";
            await connection.ExecuteAsync(sql);
            var stacks = await connection.QueryAsync<Stacks>(sql);

            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks to view flashcards from. Press enter to continue...[/]");
                return;
            }

            var selectionPrompt = new SelectionPrompt<Stacks>()
                .Title("[cyan3]Select a Stack to View Flashcards from[/]")
                .HighlightStyle("cyan3")
                .PageSize(10)
                .UseConverter(stack => stack.Name)
                .AddChoices(stacks);

            var selectedStack = AnsiConsole.Prompt(selectionPrompt);

            var flashCardsSql = $@"
         SELECT ROW_NUMBER() OVER (ORDER BY Id) AS DisplayId, Front, Back
         FROM FlashCards
         WHERE StackId = {selectedStack.Id}";
            var flashCards = await connection.QueryAsync<FlashCardsDto>(flashCardsSql);

            if (!flashCards.Any())
            {
                AnsiConsole.MarkupLine("[red]No flashcards to view. Press enter to continue...[/]");
                Console.ReadKey();
                return;
            }

            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");

            foreach (var flashCard in flashCards)
            {
                var front = flashCard.Front ?? string.Empty;
                var back = flashCard.Back ?? string.Empty;
                table.AddRow(flashCard.DisplayId.ToString(), front, back);
            }

            AnsiConsole.Render(table);
            AnsiConsole.MarkupLine("[green]Press any key to return to the main menu.[/]");
            Console.ReadKey();
        }
    }
}
