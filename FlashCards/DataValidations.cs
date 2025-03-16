using Spectre.Console;

namespace FlashCards
{
    static class DataValidations
    { 
        static public string GetUserInput(string message)
        {
            var userInput = AnsiConsole.Ask<string>(message);

            while (string.IsNullOrWhiteSpace(userInput))
            {
                AnsiConsole.MarkupLine("[red]Please enter a valid input![/]");
                userInput = AnsiConsole.Ask<string>(message);
            }

            return userInput;
        }
    }
}
