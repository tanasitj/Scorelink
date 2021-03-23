using System.Collections.Generic;
using System.Dynamic;

namespace DictionaryServices.Models
{
   
   public class Acctitle
    {
        public string pagetype { get; set; }
        public string acctitle { get; set; }
        public string accStandard { get; set; }
        public string CLCTCD { get; set; }
    }
    public class retData
    {
        public List<string> stdValue { get; set; }
        public List<List<string>> RecoverData { get; set; }
        public List<List<string>> CustomData { get; set; }
        public List<string> RowHighLight { get; set; }

        public retData()
        {
            stdValue = new List<string>();
            RecoverData = new List<List<string>>();
            CustomData = new List<List<string>>();
            RowHighLight = new List<string>();
        }
        
      

    }
   
}