using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Dtos
{
    class FlashCardsDto
    {
        public int DisplayId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }

    }

    class StudySessionDto
    {
        public int DisplayId { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}
