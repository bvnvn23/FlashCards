using FlashCards;
using FlashCards.Controllers;

await DatabaseController.CreateDatabase();
await DatabaseController.CreateTables();

var ui = new UserInterface();

while (true)
{
    await ui.MainMenu();
}
