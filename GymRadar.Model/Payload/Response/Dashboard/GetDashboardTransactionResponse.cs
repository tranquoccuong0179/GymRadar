using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.Dashboard
{
    public class GetDashboardTransactionResponse
    {
        public DateOnly Date {  get; set; }
        public double SubscriptionIncome { get; set; }
        public double TransactionIncome { get; set; }
        public double TotalRevenue { get; set; }
        public double Profit { get; set; }
    }
}
