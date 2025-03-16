using System.ComponentModel;

namespace FlashCards
{
    class Enums
    {
        public enum  MenuChoices
        {
            [Description("Manage Stacks")]
            ManageStacks,
            [Description("Manage Flash Cards")]
            ManageFlashCards,
            [Description("Study")]
            Study,
            [Description("View Study Session History")]
            ViewStudySessionHistory,
            [Description("Exit")]
            Exit
        }

        public enum ManageStacksChoices
        {
            [Description("Create Stack")]
            CreateStack,
            [Description("Edit Stack")]
            EditStack,
            [Description("Delete Stack")]
            DeleteStack,
            [Description("Return to Main Menu")]
            ReturnToMainMenu
        }

        public enum ManageFlashCardsChoices
        {
            [Description("Create Flash Card")]
            CreateFlashCard,
            [Description("Edit Flash Card")]
            EditFlashCard,
            [Description("Delete Flash Card")]
            DeleteFlashCard,
            [Description("Return to Main Menu")]
            ReturnToMainMenu
        }
    }
}
