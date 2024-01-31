using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppWPF.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
        //Relationships
        public virtual List<Question> Questions { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual List<Result> Results { get; set; }
    }
}
