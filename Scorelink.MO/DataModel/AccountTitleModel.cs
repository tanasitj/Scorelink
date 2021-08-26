using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class AccountTitleModel
    {
        public int AccTitleId { get; set; }
        public int AccGroupId { get; set; }
        public string AccTitleName { get; set; }
        public string AccTitleDesc { get; set; }
        public string AccTitleLanguage { get; set; }
        public string Active { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
