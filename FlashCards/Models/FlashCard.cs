using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    class FlashCard
    {
        public int Id { get; set; }
        public int StackID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

        public string Name { get; set; }
        
        public FlashCard(int id, int stackID, string front, string back, string name)
        {
            Id = id;
            StackID = stackID;
            Front = front;
            Back = back;
            Name = name;
        }
    }
}
