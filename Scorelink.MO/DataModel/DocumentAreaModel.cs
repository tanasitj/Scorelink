using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class DocumentAreaModel
    {
        public int AreaNo { get; set; }
        public int DocId { get; set; }
        public int DocDetId { get; set; }
        public string AreaX { get; set; }
        public string AreaY { get; set; }
        public string AreaH { get; set; }
        public string AreaW { get; set; }
        public string AreaPath { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
    }
}
