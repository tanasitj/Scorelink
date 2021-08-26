using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.MO.DataModel
{
    public class FormulaModel
    {
        public int FormulaId { get; set; }
        public string FormulaName { get; set; }
        public string FormulaDesc { get; set; }
        public string FormulaQuery { get; set; }
        public string FormulaLanguage { get; set; }
        public int StatementTypeId { get; set; }
        public string Active { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public string FormulaResult { get; set; }
    }
}
