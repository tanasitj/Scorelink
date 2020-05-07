using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class DocumentInfoModel
    {
        public int DocId { get; set; }
        public string FileUID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
    }
}
