using Microsoft.Data.SqlClient;
using Dapper;
using Spectre.Console;
using FlashCards.Dtos;
using FlashCards.Models;

namespace FlashCards.Controllers
{
    static class StudyController
    {
        public static async Task StartStudy()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);

            var sql = "SELECT * FROM Stacks";
            await connection.ExecuteAsync(sql);

            var stacks = await connection.QueryAsync<Stacks>(sql);

            if (!stacks.Any())
            {
                AnsiConsole.MarkupLine("[red]No stacks to study. Press enter to continue...[/]");
                Console.ReadKey();
                return;
            }

            var selectionPrompt = new SelectionPrompt<Stacks>()
                   .Title("[cyan3]Select a Stack to View Flashcards from[/]")
                   .HighlightStyle("cyan3")
                   .PageSize(10)
                   .UseConverter(stack => stack.Name)
                   .AddChoices(stacks);


            var selectedStack = AnsiConsole.Prompt(selectionPrompt);

            sql = $@"
                SELECT Front, Back
                FROM FlashCards
                WHERE StackId = {selectedStack.Id}";

            var flashCards = await connection.QueryAsync<FlashCardsDto>(sql);

            if (!flashCards.Any())
            {
                AnsiConsole.MarkupLine("[red]No flashcards to study. Press enter to continue...[/]");
                Console.ReadKey();
                return;
            }

            var score = 0;

            foreach (var flashCard in flashCards)
            {
                AnsiConsole.MarkupLine($"[cyan3]{flashCard.Front}[/]");
                var userInput = DataValidations.GetUserInput("Press enter to reveal the answer: ").ToLower();

                if (flashCard.Back.ToLower() == userInput)
                {
                    score++;
                    AnsiConsole.MarkupLine("[green]Correct![/]");
                    AnsiConsole.MarkupLine($"[green]Score: {score}[/]");
                    AnsiConsole.MarkupLine("[green]Press enter to continue...[/]");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Incorrect![/]");
                    AnsiConsole.MarkupLine($"[red]Score: {score}[/]");
                    AnsiConsole.MarkupLine("[red]Press enter to continue...[/]");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

                sql = $@"
                    INSERT INTO StudySessionHistory (StackId, Date, Score)
                    VALUES ({selectedStack.Id}, GETDATE(), {score})";

                await connection.ExecuteAsync(sql);

                AnsiConsole.MarkupLine("[green]Study session saved to history. Press enter to continue...[/]");
            Console.ReadKey();
        }

        public static async Task ViewStudyHiostry()
        {
            await using var connection = new SqlConnection(Consts.databaseConnection);
            var sql = "SELECT ROW_NUMBER() OVER (ORDER BY Id) AS DisplayId, Score, Date FROM StudySessionHistory";

            var studyHistory = await connection.QueryAsync<StudySessionDto>(sql);

            if (!studyHistory.Any())
            {
                AnsiConsole.MarkupLine("[red]No study history to display. Press enter to continue...[/]");
                Console.ReadKey();
                return;
            }

            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Score");
            table.AddColumn("Date");

            foreach (var study in studyHistory)
            {
                table.AddRow(study.DisplayId.ToString(), study.Score.ToString(), study.Date.ToString());
            }

            AnsiConsole.Render(table);
            AnsiConsole.MarkupLine("[green]Press enter to continue...[/]");
            Console.ReadKey();

        }
    }

}