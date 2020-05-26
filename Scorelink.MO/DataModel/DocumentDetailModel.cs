using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class DocumentDetailModel
    {
        public int DocDetId { get; set; }
        public int DocId { get; set; }
        public string DocPageNo { get; set; }
        public string FootnoteNo { get; set; }
        public string PageType { get; set; }
        public string ScanStatus { get; set; }
        public string PageFileName { get; set; }
        public string PagePath { get; set; }
        public string Selected { get; set; }
        public string PatternNo { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
    }
}
