using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppWPF.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public string AnswerStatus { get; set; }

        //Relationships
        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }
    }
}
