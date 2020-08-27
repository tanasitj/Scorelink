using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class SysConfigModel
    {
        public int ConstId { get; set; }
        public string ConstName { get; set; }
        public string ConstOutputText { get; set; }
        public int ConstOutputInt { get; set; }
        public decimal ConstOutputDouble { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
