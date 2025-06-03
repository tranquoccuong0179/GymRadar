using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Response.Transaction
{
    public class GetTransactionResponse
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public double? Price { get; set; }
        public DateTime? CreateAt { get; set; }
        public GymResponse? Gym { get; set; }
    }

    public class GymResponse
    {
        public Guid Id { get; set; }
        public string? GymName {  get; set; }
        public CourseResponse? Course { get; set; }
        public PTResponse? PT { get; set; }
    }

    public class CourseResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public class PTResponse
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
    }
}
