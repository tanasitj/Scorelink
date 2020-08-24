using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.MO.DataModel
{
    public class ResultModel
    {
        public List<DataResult> ScanEdit { get; set; }
       // public string get_value { get; set; }
       public List<DataDict> GetDict { get; set; }
    }
    public class DataResult
    {
        //public string Name { get; set; }
        //public string Address { get; set; }
        public string Footnote_No { get; set; }
        //public string Divisions { get; set; }
        public SelectList Divisions { get; set; }
        public string Digitized_Account_Title { get; set; }
        //public SelectList Recovered { get; set; }
        public string Recovered { get; set; }
        public string Standard_Title {get; set;}
        public string Amount { get; set; }
        public string Modified { get; set; }
        public string CLCTCD { get; set; }      
    }
    public class DataDict
    {
        public string Recovered { get; set; }
    }
    public class Status

    {
        public int ID { get; set; }
        public string StatusName { get; set; }
    }
}