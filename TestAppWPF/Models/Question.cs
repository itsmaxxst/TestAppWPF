using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppWPF.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string QuestionText { get; set; }

        //Relationships
        public virtual Test Test { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
