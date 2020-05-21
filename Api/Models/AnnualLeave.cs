using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class AnnualLeave : BaseEntity
    {
        public int PersonnelId { get; set; }
        public DateTime LeaveStart { get; set; }
        public DateTime LeaveEnd { get; set; }

    }
}
