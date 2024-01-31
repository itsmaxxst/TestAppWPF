using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppWPF.Models
{
    public class Result
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

        //Relationships
        public virtual User User { get; set; }
        public virtual Test Test { get; set; }
    }
}
