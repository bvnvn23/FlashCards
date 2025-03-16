using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    class Stacks
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Stacks(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
