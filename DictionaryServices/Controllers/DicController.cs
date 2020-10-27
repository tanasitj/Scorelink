using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DictionaryServices.Models;
using System.Web.Mvc;
using System.Web.Http.Results;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using System.Collections;
using System.Data;

namespace DictionaryServices.Controllers
{
    public class DicController : ApiController
    {
        private static string statementName_PL = "INCOME STATEMENT";

        private static string statementName_BS = "BALANCE SHEET";


        private static string statementName_CF = "CASH FLOW STATEMENT";
        [Route("api/dic/GetRecoveryDic")]
        [HttpPost]
        public List<List<string>> GetRecoveryDic([FromBody] List<Acctitle> Acctitles) // Return only value from dictionary  as list
        {
            var repLst = new ArrayList();
            repLst.Add(statementName_PL);
            repLst.Add(statementName_BS);
            repLst.Add(statementName_CF);
            string strStatementName;
            string KeyCLCTCD="";
            string strwCLCTCD="0000";
            string strCLCTCD="";
            int distance = 0;
            List<List<string>> recoverList = new List<List<string>>();
            recoverList.Clear();
            DictionaryRepo dic = new DictionaryRepo();
            //Dictionary<string, List<string>> d = dic.ReadyRecoverDictionary();
            foreach (Acctitle acc in Acctitles) {
                strStatementName = repLst[Int32.Parse(acc.pagetype) - 1].ToString();
                dic.ConvertOCRResult(strStatementName, acc.acctitle, KeyCLCTCD, distance);
                dic.ConvertStandard(strStatementName, acc.acctitle, strwCLCTCD,ref strCLCTCD);
                acc.CLCTCD = strCLCTCD;
                //List<string> recoverListItem = dic.initComboItemCheckSPS(acc.pagetype, acc.acctitle);
                //recoverList.Add(recoverListItem);
                if (strCLCTCD.Length >= 2)
                {
                    strwCLCTCD = strCLCTCD.Substring(2);
                    if (strCLCTCD.Substring(0, 2) == "01")
                    {
                        KeyCLCTCD = strCLCTCD;
                    }
                }

            }
            recoverList = dic.initComboItemCheckSPS(Acctitles);
           
            return recoverList;
        }

       

       
    }

}
