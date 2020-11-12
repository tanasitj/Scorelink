using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class F_DocumentDetailModel
    {
        public int StatementId { get; set; }
        public int DocId { get; set; }
        public string StatementName { get; set; }
        public string FootnoteNo { get; set; }
        public string NoScan { get; set; }
        public string PageNo { get; set; }
        public string PageType { get; set; }
        public string PageTypeName { get; set; }
        public string DocPageNo { get; set; }
        public string Commited { get; set; }
    }
}
