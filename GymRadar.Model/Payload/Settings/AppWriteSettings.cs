using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymRadar.Model.Payload.Settings
{
    public class AppWriteSettings
    {
        public string EndPoint { get; set; }
        public string ProjectId { get; set; }
        public string APIKey { get; set; }
        public string Bucket { get; set; }
    }
}
