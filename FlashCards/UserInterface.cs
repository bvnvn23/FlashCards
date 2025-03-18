using FlashCards.Controllers;
using Microsoft.Identity.Client;
using Spectre.Console;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace FlashCards
{
    class UserInterface
    {

        string message = "Something...";

        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        public async Task MainMenu()
        {

            AnsiConsole.Clear();

            AnsiConsole.Write(new FigletText("Flash Cards")
             .Color(Color.Cyan3));
            var choices = Enum.GetValues(typeof(Enums.MenuChoices)).Cast<Enums.MenuChoices>().ToList();
            var menu = AnsiConsole.Prompt(new SelectionPrompt<Enums.MenuChoices>()
                .Title("[cyan3]Main Menu[/]")
                .HighlightStyle("cyan3")
                .PageSize(10)
                .UseConverter(choice => GetEnumDescription(choice))
                .AddChoices(choices));

            switch (menu)
            {
                case Enums.MenuChoices.ManageStacks:
                    await ManageStacksMenu();
                    break;
                case Enums.MenuChoices.ManageFlashCards:
                    await ManageFlashCardsMenu();
                    break;
                case Enums.MenuChoices.Study:
                    await StudyController.StartStudy();
                    break;
                case Enums.MenuChoices.ViewStudySessionHistory:
                    await StudyController.ViewStudyHiostry();
                    break;
                case Enums.MenuChoices.Exit:
                    Environment.Exit(0);
                    break;
            }

        }


        public async Task ManageStacksMenu()
        {

            var choices = Enum.GetValues(typeof(Enums.ManageStacksChoices)).Cast<Enums.ManageStacksChoices>().ToList();
            var menu = AnsiConsole.Prompt(new SelectionPrompt<Enums.ManageStacksChoices>()
                .Title("[cyan3]Manage Stacks[/]")
                .HighlightStyle("cyan3")
                .PageSize(10)
                .UseConverter(choice => GetEnumDescription(choice))
                .AddChoices(choices));

            switch (menu)
            {
                case Enums.ManageStacksChoices.CreateStack:
                    message = "Enter the name of the stack you would like to create: ";
                    await StackController.InsertStack(DataValidations.GetUserInput(message));
                    break;
                case Enums.ManageStacksChoices.EditStack:
                    await StackController.EditStack();
                    break;
                case Enums.ManageStacksChoices.ViewStacks:
                    await StackController.ViewStack();
                    break;
                case Enums.ManageStacksChoices.DeleteStack:
                    await StackController.DeleteStack();
                    break;
                case Enums.ManageStacksChoices.ReturnToMainMenu:
                    await MainMenu();
                    break;
            }
        }


        public async Task ManageFlashCardsMenu()
        {

            var choices = Enum.GetValues(typeof(Enums.ManageFlashCardsChoices)).Cast<Enums.ManageFlashCardsChoices>().ToList();
            var menu = AnsiConsole.Prompt(new SelectionPrompt<Enums.ManageFlashCardsChoices>()
                .Title("[cyan3]Manage Flash Cards[/]")
                .HighlightStyle("cyan3")
                .PageSize(10)
                .UseConverter(choice => GetEnumDescription(choice))
                .AddChoices(choices));

            switch (menu)
            {
                case Enums.ManageFlashCardsChoices.CreateFlashCard:
                    await FlashCardsController.AddFlashCard();
                    break;
                case Enums.ManageFlashCardsChoices.EditFlashCard:
                    break;
                case Enums.ManageFlashCardsChoices.ViewFlashCards:
                    await FlashCardsController.ViewFlashCard();
                    break;
                case Enums.ManageFlashCardsChoices.DeleteFlashCard:
                    break;
                case Enums.ManageFlashCardsChoices.ReturnToMainMenu:
                    await MainMenu();
                    break;

            }
        }
    }
}
