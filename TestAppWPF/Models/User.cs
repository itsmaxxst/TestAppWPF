using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppWPF.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int RoleId { get; set; }

        //Navigation
        public virtual Role Role { get; set; }

        //Relationships
        public virtual List<Result> Results { get; set; }
        public virtual List<Role> Roles { get; set; }
    }
}
