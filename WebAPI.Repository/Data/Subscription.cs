using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Repository.Data
{
    public class Subscription
    {
        public string PlanName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive => !EndDate.HasValue || EndDate > DateTime.UtcNow;
    }
}
