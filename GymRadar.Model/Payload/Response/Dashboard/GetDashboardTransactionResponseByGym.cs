using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.Dashboard
{
    public class GetDashboardTransactionResponseByGym
    {
        public DateOnly Date {  get; set; }
        public double TotalRevenue { get; set; }
        public double AppCommission { get; set; }
        public double PaybackToGym { get; set; }
    }
}
