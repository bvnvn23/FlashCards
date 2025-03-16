using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Models
{
    class StudySession
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }


        public StudySession (int id, int score, DateTime date)
        {
            Id = id;
            Score = score;
            Date = date;
        } 
    }
}
