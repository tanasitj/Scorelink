using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class AccountTitleGroupModel
    {
        public int AccGroupId { get; set; }
        public string AccGroupName { get; set; }
        public string AccGroupDesc { get; set; }
        public string AccGroupLanguage { get; set; }
        public string Active { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
