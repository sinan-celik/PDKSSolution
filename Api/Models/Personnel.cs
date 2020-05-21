using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Personnel : BaseEntity
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfStart { get; set; }

    }
}
