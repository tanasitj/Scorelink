using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class OnlineUserModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public string CPUNO { get; set; }
        public string OnlineUpdate { get; set; }
    }
}
