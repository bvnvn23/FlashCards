using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    class StudySession
    {
        public int StackId { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }


        public StudySession (int stackId, int score, DateTime date)
        {
            StackId = stackId;
            Score = score;
            Date = date;
        } 
    }
}
