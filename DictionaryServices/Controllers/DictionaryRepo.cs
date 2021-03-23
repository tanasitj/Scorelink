
using System.Collections.Generic;
using System.Linq;
using SIO = System.IO;
using Microsoft.VisualBasic;
using System;
using OXml = OpenXML;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections;
using Microsoft.VisualBasic.CompilerServices;
using DictionaryServices.Models;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Configuration;

namespace DictionaryServices.Controllers
{
    public class WordInfo
    {
        private string _FullWord = "";
        // 単語(文章)全体
        internal string FullWord
        {
            get
            {
                return _FullWord;
            }
        }

        private int _WordsCount = 0;
        // 単語数
        internal int WordsCount
        {
            get
            {
                return _WordsCount;
            }
        }

        private int _CharsCount = 0;
        // 文字数
        internal int CharsCount
        {
            get
            {
                return _CharsCount;
            }
        }

        private int _Distance = 0;
        // 訂正時の点数
        internal int Distance
        {
            get
            {
                return _Distance;
            }

            set
            {
                _Distance = value;
            }
        }

        // コンストラクタ
        public WordInfo(string pFullWord)
        {
            _FullWord = pFullWord;
            _CharsCount = pFullWord.Length;
            _WordsCount = pFullWord.Split(' ').Count();
        }
    }


    public struct standard
    {
        public string SBJName;
        public string CLCTCD;
    }




    public class DictionaryRepo
    {
       // private int intDistance = 0;
        private static string statementName_PL = "INCOME STATEMENT";

        private static string statementName_BS = "BALANCE SHEET";


        private static string statementName_CF = "CASH FLOW STATEMENT";

        private static Dictionary<string, List<string>> pRecoverDictionary = null;
        private static Dictionary<string, Dictionary<string, standard>> pStandardKeyDictionary;
        private static Dictionary<string, Dictionary<string, standard>> pStandardDictionary;
        private static Dictionary<string, string> pConvertWordDictionary= null;
        private static Dictionary<string, Dictionary<string, string>> pForcedConvertDictionary;
        private string convertOCRPath = "";
        private string convertStand = "";
        private string convertCustom = "";
        public DictionaryRepo(string uid,string lng)
        {
            string dicLang="";


            switch (lng) 
            {
                case "Thai":
                    dicLang = "th";
                    break;
                case "English":
                    dicLang = "en";
                    break;
            }

            string DicPath=ConfigurationManager.AppSettings["DictPath"];
            //convertOCRPath =  HttpContext.Current.Server.MapPath("~/Dict/")+ uid+ "/"+ dicPath + "/ConvertOCRResult.xlsx"  ;
            convertOCRPath = DicPath+uid + "\\" + dicLang + "\\ConvertOCRResult.xlsx";
            //convertStand = HttpContext.Current.Server.MapPath("~/Dict/") + uid + "/" + dicPath + "/ConvertStandard.xlsx";
            convertStand = DicPath+uid + "\\" + dicLang + "\\ConvertStandard.xlsx";
           // convertCustom = HttpContext.Current.Server.MapPath("~/Dict/") + uid + "/" + dicPath +  "/TCCS_"+dicPath+".xlsx";

            // convertCustom = HttpContext.Current.Server.MapPath("~/Dict/") + "TCCS.xlsx";
            ReadyRecoverDictionary();
            ReadConvertStandard();
           
        }
        public List<string> GetCustomDictionary(string statement)
        {

          
            string DictPath = convertCustom;
            using (var openXML = new OXml.SpreadSheets())
            {

                Dictionary<string, List<string>> pCustomDictionary = new Dictionary<string, List<string>>();
                pCustomDictionary.Add("INCOME STATEMENT", new List<string>());
                pCustomDictionary.Add("BALANCE SHEET", new List<string>());
                pCustomDictionary.Add("CASH FLOW STATEMENT", new List<string>());
                string shName = statement;
                using (var document = openXML.OpenSpreadsheetDocument(DictPath, false))
                {
                    var wbPart = document.WorkbookPart;
                    var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    //foreach (string shName in new[] { "INCOME STATEMENT", "BALANCE SHEET", "CASH FLOW STATEMENT" })
                    //{

                   
                        var theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == shName).FirstOrDefault();
                        WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);
                        var rows = wsPart.Worksheet.Descendants<Row>();
                        for (int rowIdx = 0, loopTo = rows.Count() - 1; rowIdx <= loopTo; rowIdx++)
                        {
                            if (rows.ElementAtOrDefault(rowIdx) is null)
                                continue;

                            var cells = rows.ElementAtOrDefault(rowIdx).Elements<Cell>();


                            string sbjName = GetCellValue(cells.ElementAtOrDefault(0), stringTable);


                            if (string.IsNullOrEmpty(sbjName))
                                break;
                            if (pCustomDictionary.ContainsKey(shName) == true)
                                pCustomDictionary[shName].Add(sbjName.ToLower());
                       }
                    //} End for each
                 }
                openXML.Dispose();
                List<string> customDic =pCustomDictionary[shName].ToList();
                return customDic;
            }// End Using
        }

        //private static void ReadyRecoverDictionary(string LangPath, string FileName)
        public Dictionary<string, List<string>> ReadyRecoverDictionary()
        {

        
            string DictPath = convertOCRPath;
            using (var openXML = new OXml.SpreadSheets())
            {

                pRecoverDictionary = new Dictionary<string, List<string>>();
                pRecoverDictionary.Add("INCOME STATEMENT", new List<string>());
                pRecoverDictionary.Add("BALANCE SHEET", new List<string>());
                pRecoverDictionary.Add("CASH FLOW STATEMENT", new List<string>());
                using (var document = openXML.OpenSpreadsheetDocument(DictPath, false))
                {
                    var wbPart = document.WorkbookPart;
                    var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    foreach (string shName in new[] { "INCOME STATEMENT", "BALANCE SHEET", "CASH FLOW STATEMENT" })
                    {


                        var theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == shName).FirstOrDefault();
                        WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);
                        var rows = wsPart.Worksheet.Descendants<Row>();
                        for (int rowIdx = 0, loopTo = rows.Count() - 1; rowIdx <= loopTo; rowIdx++)
                        {
                            if (rows.ElementAtOrDefault(rowIdx) is null)
                                continue;

                            var cells = rows.ElementAtOrDefault(rowIdx).Elements<Cell>();


                            string sbjName = GetCellValue(cells.ElementAtOrDefault(0), stringTable);


                            if (string.IsNullOrEmpty(sbjName))
                                break;
                            if (pRecoverDictionary.ContainsKey(shName) == true)
                                pRecoverDictionary[shName].Add(sbjName.ToLower()); 
                        }
                    }
                }
                openXML.Dispose();
                return pRecoverDictionary;
            }// End Using
        }
        //private static void ReadConvertStandard(string LangPath, string FileName)
        public Dictionary<string, Dictionary<string, standard>> ReadConvertStandard()
        {
           
           // string DictPath = "C:\\Users\\cyber\\source\\repos\\DictionaryWeb\\DictionaryWeb\\Dicts\\th\\ConvertStandard.xlsx";
            string DictPath = convertStand;
            using (var openXML = new OXml.SpreadSheets())
            {

                // 初期化
                pStandardKeyDictionary = new Dictionary<string, Dictionary<string, standard>>();
                pStandardKeyDictionary.Add("INCOME STATEMENT", new Dictionary<string, standard>());
                pStandardKeyDictionary.Add("BALANCE SHEET", new Dictionary<string, standard>());
                pStandardKeyDictionary.Add("CASH FLOW STATEMENT", new Dictionary<string, standard>());
                pStandardDictionary = new Dictionary<string, Dictionary<string, standard>>();
                using (var document = openXML.OpenSpreadsheetDocument(DictPath, false))
                {
                    var wbPart = document.WorkbookPart;
                    var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    foreach (string shName in new[] { "INCOME STATEMENT", "BALANCE SHEET", "CASH FLOW STATEMENT" })
                    {

                        var theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == shName).FirstOrDefault();
                        WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);
                        var rows = wsPart.Worksheet.Descendants<Row>();
                        for (int rowIdx = 0, loopTo = rows.Count() - 1; rowIdx <= loopTo; rowIdx++)
                        {
                            if (rows.ElementAtOrDefault(rowIdx) is null)
                                continue;

                            
                            var cells = rows.ElementAtOrDefault(rowIdx).Elements<Cell>();

                           
                            int idx = rowIdx + 1;
                            string from = "";
                            string sbjName = "";
                            string kbn = "00"; 
                            string sbjCd = StrPadLeft(GetCellValue(rows.ElementAtOrDefault(rowIdx).Descendants<Cell>().Where(c => c.CellReference == "D" + idx.ToString()).FirstOrDefault(), stringTable), '0', 2);
                            string sbjCdSub = StrPadLeft(Strings.Right(sbjCd, 1), '0', 2);

                            
                            foreach (Cell cell in cells)
                            {
                                if (cell.CellReference is null)
                                    continue;
                                string cellReference = cell.CellReference.ToString();
                                switch (cellReference.Substring(0, cellReference.Length - idx.ToString().Length) ?? "")
                                {
                                    case "A":
                                        {
                                            // 変換元科目名
                                            from = GetCellValue(cell, stringTable).ToLower();
                                            break;
                                        }

                                    case "B":
                                        {
                                            // 変換後科目名
                                            sbjName = GetCellValue(cell, stringTable).ToLower();
                                            break;
                                        }

                                    case "C":
                                        {
                                            // 大区分
                                            kbn = StrPadLeft(GetCellValue(cell, stringTable), '0', 2);
                                            break;
                                        }

                                    case "D":
                                        {
                                            // 科目コード
                                            sbjCd = StrPadLeft(GetCellValue(cell, stringTable), '0', 2);
                                            break;
                                        }

                                    case "E":
                                        {
                                            // 科目コード(サブ)
                                            string wSbj = GetCellValue(rows.ElementAtOrDefault(rowIdx).Descendants<Cell>().Where(c => c.CellReference == "E" + idx.ToString()).FirstOrDefault(), stringTable);
                                            if (!string.IsNullOrEmpty(wSbj))
                                                sbjCdSub = StrPadLeft(wSbj, '0', 2);
                                            break;
                                        }

                                    default:
                                        {
                                            break;
                                        }
                                        // なにもしない
                                }
                            }

                            
                            if (string.IsNullOrEmpty(sbjCdSub))
                                sbjCdSub = StrPadLeft(Strings.Right(sbjCd, 1), '0', 2);
                            if (kbn != "00")
                                {
                                    
                                    if (pStandardKeyDictionary[shName].ContainsKey(from) == false)
                                {
                                    var s = new standard();
                                    s.SBJName = sbjName;
                                    s.CLCTCD = kbn + sbjCdSub + sbjCd;
                                    pStandardKeyDictionary[shName].Add(from, s);
                                }
                            }
                            else
                            {
                            
                                if (pStandardDictionary.ContainsKey(sbjCd) == false)
                                    pStandardDictionary.Add(sbjCd, new Dictionary<string, standard>());
                                if (pStandardDictionary[sbjCd].ContainsKey(from) == false)
                                {
                                    var s = new standard();
                                    s.SBJName = sbjName;
                                    s.CLCTCD = kbn + sbjCdSub + sbjCd;
                                    pStandardDictionary[sbjCd].Add(from, s);
                                }
                            }
                    }
                  }
                }
                openXML.Dispose();
                return pStandardDictionary;
            }
        }
        private static string GetCellValue(SpreadsheetDocument document, string sheetNm, string cellName)
        {
            var wbPart = document.WorkbookPart;
            var theSheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetNm).FirstOrDefault();
            WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);
            var theCell = wsPart.Worksheet.Descendants<Cell>().Where(c => c.CellReference == cellName).FirstOrDefault();
            string ret = "";
            if (theCell is object)
            {
                ret = theCell.InnerText;
                if (theCell.DataType is object)
                {
                    switch (theCell.DataType.Value)
                    {
                        case CellValues.SharedString:
                            {
                                var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                                if (stringTable is object)
                                {
                                    ret = stringTable.SharedStringTable.ElementAt(int.Parse(ret)).InnerText;
                                }

                                break;
                            }

                        case CellValues.Boolean:
                            {
                                switch (ret ?? "")
                                {
                                    case "0":
                                        {
                                            ret = "FALSE";
                                            break;
                                        }

                                    default:
                                        {
                                            ret = "TRUE";
                                            break;
                                        }
                                }

                                break;
                            }
                    }
                }
            }

            return ret;
        }

        private static string GetCellValue(Cell theCell, SharedStringTablePart stringTable)
        {
            string ret = "";
            if (theCell is null)
                return "";
            if (theCell.InnerText is null)
                return "";
            ret = theCell.InnerText;
            if (theCell.DataType is object)
            {
                switch (theCell.DataType.Value)
                {
                    case CellValues.SharedString:
                        {
                            if (stringTable is object)
                            {
                                ret = stringTable.SharedStringTable.ElementAt(int.Parse(ret)).InnerText;
                            }

                            break;
                        }

                    case CellValues.Boolean:
                        {
                            switch (ret ?? "")
                            {
                                case "0":
                                    {
                                        ret = "FALSE";
                                        break;
                                    }

                                default:
                                    {
                                        ret = "TRUE";
                                        break;
                                    }
                            }

                            break;
                        }
                }
            }

            return ret;
        }
        public static int ByteCount(string stTarget)
        {
            var hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            return hEncoding.GetByteCount(stTarget);
        }

        public static string StrPadLeft(string target, char paddingChar, int totalBytes)
        {
        string s;
        int baseLength;
        baseLength = ByteCount(target);
        try
        {
            s = new string(paddingChar, totalBytes - baseLength) + target;
        }
        catch (Exception ex)
        {
            return target;
        }

        return s;
    }

        public static string ConvertWord(string strString)
        {
            foreach (string strFrom in pConvertWordDictionary.Keys)
            {
                if (!string.IsNullOrEmpty(strFrom))
                {
                    if (strString.IndexOf(strFrom) > -1)
                    {
                        strString = strString.Replace(strFrom, pConvertWordDictionary[strFrom]);
                    }
                }
            }

            return strString;
        }

        public string ConvertOCRResult(string strStatementName, string title, string KeyCLCTCD, int distance)

        {
            pConvertWordDictionary = new Dictionary<string, string>() ;
            

            string convertTitle = ConvertWord(title);
            if (string.IsNullOrEmpty(convertTitle))
                return "";
            if (pRecoverDictionary.ContainsKey(strStatementName) == false)
                return "";
            var myDictLst = new List<string>();
            if (KeyCLCTCD.Length < 6)
            {
                myDictLst = pRecoverDictionary[strStatementName];
            }
            else
            {
                string sbjCd = KeyCLCTCD.Substring(4, 2);
           
                string keyStr;
                myDictLst.Add("total(auto)");
                foreach (var currentKeyStr in pStandardKeyDictionary[strStatementName].Keys)
                {
                    keyStr = currentKeyStr;
                    string tmpCLCTCD = pStandardKeyDictionary[strStatementName][keyStr].CLCTCD;
                    if (myDictLst.Contains(keyStr) == false & (tmpCLCTCD.Substring(0, 2) == "01" | tmpCLCTCD.Substring(0, 2) == "99" | (tmpCLCTCD.Substring(4, 2) ?? "") == (sbjCd ?? "")))
                    {
                        myDictLst.Add(keyStr);
                    }
                }
               
                if (myDictLst.Contains(convertTitle) == true)
                    return convertTitle;
                    myDictLst.Clear();
                if (pStandardDictionary.ContainsKey(sbjCd) == true)
                {
                    foreach (var currentKeyStr1 in pStandardDictionary[sbjCd].Keys)
                    {
                        keyStr = currentKeyStr1;
                        if (myDictLst.Contains(keyStr) == false)
                        {
                            myDictLst.Add(keyStr);
                        }
                    }
                }

                foreach (var currentKeyStr2 in pStandardKeyDictionary[strStatementName].Keys)
                {
                    keyStr = currentKeyStr2;
                    string tmpCLCTCD = pStandardKeyDictionary[strStatementName][keyStr].CLCTCD;
                    if (myDictLst.Contains(keyStr) == false && tmpCLCTCD.Substring(0, 2) == "99" | (tmpCLCTCD.Substring(4, 2) ?? "") == (sbjCd ?? ""))
                    {
                        myDictLst.Add(keyStr);
                    }
                }
            }


            if (myDictLst.Contains(convertTitle) == true)
                return convertTitle;
            var dic = new Dictionary<string, int>();
            foreach (string key in myDictLst)
            {
                if (dic.ContainsKey(key) == false)
                {
                    dic.Add(key, ComputeLevenshteinDistance(convertTitle, key));
                }
            }

            string ret = "";
            int len = 0;
            bool maxFlg = false;
            int maxLen = 0;
            string MAX_DISTANCE = "";
            //if (!string.IsNullOrEmpty(GetClientConfigFile("MAX_DISTANCE")) && Information.IsNumeric(GetClientConfigFile("MAX_DISTANCE")) == true)
            if (!string.IsNullOrEmpty("MAX_DISTANCE") && Information.IsNumeric("MAX_DISTANCE") == true)
            {
                maxFlg = true;
                maxLen = Int32.Parse("MAX_DISTANCE");
            }

            foreach (string key in dic.Keys)
            {
                if (string.IsNullOrEmpty(ret))
                {
                    ret = key;
                    len = dic[key];
                }

                if (len > dic[key])
                {
                    ret = key;
                    len = dic[key];
                }
            }


            if (maxFlg == true && len >= maxLen)
                ret = "**********";

          
            distance = len;
            return ret;
        }
        public  string ConvertStandard(string strStatementName, string title, string strwCLCTCD, ref string strCLCTCD)
        {
            string ConvertStandardRet = default;
            ConvertStandardRet = "";
            if (pStandardKeyDictionary.ContainsKey(strStatementName) == false)
                return "";
            if (pStandardKeyDictionary[strStatementName].ContainsKey(title) == true)
            {
                ConvertStandardRet = pStandardKeyDictionary[strStatementName][title].SBJName;
                strCLCTCD = pStandardKeyDictionary[strStatementName][title].CLCTCD;
            }
            else
            {
               strCLCTCD = "00" + strwCLCTCD;
                // --nnmm
               string sbjCd = strwCLCTCD.Substring(2, 2);
                if (pStandardDictionary.ContainsKey(sbjCd) == true && pStandardDictionary[sbjCd].ContainsKey(title) == true)
                {
                    ConvertStandardRet = pStandardDictionary[sbjCd][title].SBJName;
                }
                else //if (title.ToLower() == "total(auto)") //change for thai
                {
                    ConvertStandardRet = "N/A"; //"total";
                    strCLCTCD = "02" + strwCLCTCD;
                }
                /// For Thai;
                //else
                //{
                  
                //    strCLCTCD = "10" + strwCLCTCD;
                //    ConvertStandardRet = GetForcedConvertStandard(strStatementName, strCLCTCD);
                //}
            }

            return ConvertStandardRet;
        }
        private static string GetForcedConvertStandard(string sheetName, string clctcd)
        {
            pForcedConvertDictionary = new Dictionary<string, Dictionary<string, string>>();
            if (pForcedConvertDictionary.ContainsKey(sheetName) == false)
                return "";
            if (pForcedConvertDictionary[sheetName].ContainsKey(clctcd) == false)
                return "";
            return pForcedConvertDictionary[sheetName][clctcd];
        }
        public static int ComputeLevenshteinDistance(string strX, string strY)
        {
            if (strX is null)
                throw new ArgumentNullException("strX");
            if (strY is null)
                throw new ArgumentNullException("strY");
            if (strX.Length == 0)
                return strY.Length;
            if (strY.Length == 0)
                return strX.Length;
            var d = new int[strX.Length + 1 + 1, strY.Length + 1 + 1];
            for (int i = 0, loopTo = strX.Length; i <= loopTo; i++)
                d[i, 0] = i;
            for (int j = 0, loopTo1 = strY.Length; j <= loopTo1; j++)
                d[0, j] = j;
            for (int j = 1, loopTo2 = strY.Length; j <= loopTo2; j++)
            {
                for (int i = 1, loopTo3 = strX.Length; i <= loopTo3; i++)
                {
                    if (strX[i - 1] == strY[j - 1])
                    {
                        d[i, j] = d[i - 1, j - 1];
                    }
                    else
                    {
                        d[i, j] = Min(d[i - 1, j] + 1, d[i, j - 1] + 1, d[i - 1, j - 1] + 1);
                    }
                }
            }

            return d[strX.Length, strY.Length];
        }
        private static int Min(int x, int y, int z)
        {
            return Math.Min(Math.Min(x, y), z);
        }
        //public List<string> initComboItemCheckSPS(string pagetype, string subjectStr)
        public List<List<string>>  initComboItemCheckSPS(List<Acctitle> acctitles)
        {
           
            var repLst = new ArrayList();
            repLst.Add(statementName_PL);
            repLst.Add(statementName_BS);
            repLst.Add(statementName_CF);
           
           
           // ReadyRecoverDictionary();
            //string ReSubject = ConvertOCRResult(repName, subjectStr, "", intDistance);
            //ReadConvertStandard();
            //string StdSubject=ConvertStandard(repName, ReSubject, "0000", "");
           //string[,] SbjRow = new string[1,2] { { subjectStr, "000212" } };
            
           
            List<string> ArrReCovItems= new List<string>(); ;
            List<string> ArrReCovItemsKey = new List<string>(); ;
            List<string> comboList=new List<string>();
            List<List<string>> recoverList = new List<List<string>>();
            recoverList.Clear();
            var ArrDivs = new List<string>();
            ArrDivs.Add("");
            //ArrDivs.Add(pDivsionMajorStr);
            string keyClctcd = "";
            for (int rowIdx = 0, loopTo = acctitles.Count-1; rowIdx <= loopTo; rowIdx++)
            {
                string repName = repLst[Int32.Parse(acctitles[rowIdx].pagetype) - 1].ToString(); 
                string title = acctitles[rowIdx].acctitle;

               
                string clctcd = acctitles[rowIdx].CLCTCD;
                string kbn = "";
                string sbjCd = "";
                if (clctcd is null || clctcd.Length < 6)
                 {
                    kbn = "01";
                 }
                else
                 {
                    kbn = acctitles[rowIdx].CLCTCD.Substring(0, 2);
                    sbjCd = acctitles[rowIdx].CLCTCD.Substring(4, 2);

                    if (kbn == "01")
                    {
                        keyClctcd = clctcd;
                    }
                 }

                if (kbn == "01" || string.IsNullOrEmpty(keyClctcd))
                {
                  
                    if (clctcd is null || clctcd.Length < 6 || string.IsNullOrEmpty(keyClctcd))
                    {
                        /// ZaimuSheet.Cells[rowIdx, (int)FFBStuct.spdCol_C.Division].Text = "";
                    }
                    else
                    {
                        /// ZaimuSheet.Cells[rowIdx, (int)FFBStuct.spdCol_C.Division].Text = pDivsionMajorStr;
                    }

                    if (kbn == "01")
                    {
                        comboList = getReCoveryItemKeyRowList(repName, title);
                        //ArrReCovItemsKey = getReCoveryItemKeyRowList(repName, title);
                        
                       // ComboItemSetOne(ZaimuSheet, rowIdx, (int)FFBStuct.spdCol_C.ReSubject, ArrReCovItemsKey);
                    }
                    else
                    {
                        comboList  = getReCoveryItemList(repName, "", title);
                       //ArrReCovItems = getReCoveryItemList(repName, "", title);
                        // this.ComboItemSetOne(ZaimuSheet, rowIdx, (int)FFBStuct.spdCol_C.ReSubject, ArrReCovItems);
                    }
                }
                else
                {

                    //ZaimuSheet.Cells[rowIdx, (int)FFBStuct.spdCol_C.Division].Text = "";
                        comboList  = getReCoveryItemList(repName, sbjCd, title);
                    //ArrReCovItems = getReCoveryItemList(repName, sbjCd, title);
                   
                    //this.ComboItemSetOne(ZaimuSheet, rowIdx, (int)FFBStuct.spdCol_C.ReSubject, ArrReCovItems);
                }


                //this.ComboItemSetOne(ZaimuSheet, rowIdx, (int)FFBStuct.spdCol_C.Division, ArrDivs);
                recoverList.Add(comboList);
            }
            return recoverList;
        }  /// <summary>
           ///  End Initial Combobox


        public class DistanceComparer : IComparer<string>
        {
            private string _title;

            public DistanceComparer(string title)
            {
                _title = title;
            }

            public int Compare(string valA, string valB)
            {
                int distanceA = computeDistance(_title, valA);
                int distanceB = computeDistance(_title, valB);
                if (distanceA == distanceB)
                {
                    if ((valA ?? "") == (valB ?? ""))
                    {
                        return 0;
                    }
                    else if (Operators.CompareString(valA, valB, false) > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return distanceA - distanceB;
                }
            }
        }

        public static int computeDistance(string title, string recover)
        {
            int computeDistanceRet = default;
            // 不要記号の削除
            string convertTitle = ConvertWord(title.ToLower());
            computeDistanceRet = ComputeLevenshteinDistance(convertTitle, recover);
            return computeDistanceRet;
        }
        public static List<string> getReCoveryItemKeyRowList(string strStatementName, string title)
        {
            var retList = new List<string>();
            if (!pStandardKeyDictionary.ContainsKey(strStatementName))
            {
                return retList;
            }

            foreach (KeyValuePair<string, standard> kvp in pStandardKeyDictionary[strStatementName])
            {
                if (kvp.Value.CLCTCD.Substring(0, 2) == "01")
                {
                    retList.Add(kvp.Key.ToString());
                }
            }

            IComparer<string> comparer = new DistanceComparer(title);
            retList.Sort(comparer);
            return retList;
        }
        public static List<string> getReCoveryItemList(string strStatementName, string sbjCd, string title)
        {
            var retList = new List<string>();
            if (string.IsNullOrEmpty(sbjCd))
            {
                // Ver3.2 IT不具合対応No.32
                retList.AddRange(pRecoverDictionary[strStatementName]);
            }
            else if (pStandardDictionary.ContainsKey(sbjCd) == true)
            {
                foreach (KeyValuePair<string, standard> kvp in pStandardDictionary[sbjCd])
                {
                    if (retList.Contains(kvp.Key.ToString()) == false)
                    {
                        retList.Add(kvp.Key.ToString());
                    }
                }
            }
            // 合計項目を取得する。
            if (pStandardKeyDictionary.ContainsKey(strStatementName))
            {
                foreach (KeyValuePair<string, standard> kvp in pStandardKeyDictionary[strStatementName])
                {
                    if (kvp.Value.CLCTCD.Substring(0, 2) == "02" && (kvp.Value.CLCTCD.Substring(4, 2) ?? "") == (sbjCd ?? ""))
                    {
                        if (retList.Contains(kvp.Key.ToString()) == false)
                        {
                            retList.Add(kvp.Key.ToString());
                        }
                    }
                }
            }

            // V3.2 不具合改修
            // 除外項目を取得する。
            if (pStandardKeyDictionary.ContainsKey(strStatementName))
            {
                foreach (KeyValuePair<string, standard> kvp in pStandardKeyDictionary[strStatementName])
                {
                    if (kvp.Value.CLCTCD.Substring(0, 2) == "99" && (kvp.Value.CLCTCD.Substring(4, 2) ?? "") == (sbjCd ?? ""))
                    {
                        if (retList.Contains(kvp.Key.ToString()) == false)
                        {
                            retList.Add(kvp.Key.ToString());
                        }
                    }
                }
            }
            // V3.2 不具合改修

            // ソート順
            IComparer<string> comparer = new DistanceComparer(title);
            retList.Sort(comparer);
            return retList;
        }
        //private void ComboItemSetOne(FPWS.SheetView sps, int row, int col, List<string> ArrL)
        //{
        //    // copy list
        //    var setLst = new ArrayList();

        //    // Ver3.2 IT不具合対応No.32
        //    // Dim reVal As String = sps.Cells(row, col).Text
        //    // If Not ArrL.Contains(reVal) Then
        //    // setLst.Add(reVal)
        //    // End If

        //    foreach (object item in ArrL)
        //        setLst.Add(item);


        //    var nm = new FPWS.CellType.ComboBoxCellType();
        //    nm.Items = (string[])setLst.ToArray(typeof(string));
        //    // Ver3.2 IT不具合対応No.29
        //    if ((int)FFBStuct.spdCol_C.Division == col)
        //    {
        //        nm.Editable = false;
        //    }
        //    else
        //    {
        //        nm.Editable = true;
        //    }


        //    // If editflg = True Then
        //    // nm.CharacterSet = FarPoint.Win.ComboCharacterSet.KanjiOnlyIME
        //    // End If
        //    sps.Cells[row, col].CellType = nm;
        //}

        

    }/// End Class
    //public class FinancialCollect
    //{

    //    // 定数宣言（Spread）
    //    /// <summary>
    //    /// 一覧表スプレッドカラム列番号
    //    /// </summary>
    //    /// <remarks>一覧表スプレッドカラム列番号</remarks>
    //    public enum spdCol : int
    //    {
    //        /// <summary>注記</summary>
    //        Note = 0,
    //        /// <summary>勘定科目</summary>
    //        Subject = 1,
    //        /// <summary>金額</summary>
    //        Kingaku = 2,
    //        /// <summary>編集</summary>
    //        Modified = 3
    //    }

        /// <summary>
        /// チェック用一覧表スプレッドカラム列番号
        /// </summary>
        /// <remarks>チェック用一覧表スプレッドカラム列番号</remarks>
        //public enum spdCol_C : int
        //{
        //    /// <summary>注記</summary>
        //    Note = 0,
        //    /// <summary>大区分科目</summary>
        //    Division = 1,
        //    /// <summary>勘定科目</summary>
        //    Subject = 2,
        //    /// <summary>リカバリー科目</summary>
        //    ReSubject = 3,
        //    /// <summary>中間集約科目</summary>
        //    Standard = 4,
        //    /// <summary>金額</summary>
        //    Kingaku = 5,
        //    /// <summary>編集</summary>
        //    Modified = 6,
        //    /// <summary>文字数</summary>
        //    Length = 7,
        //    /// <summary>距離</summary>
        //    Distance = 8,
        //    /// <summary>割合</summary>
        //    Rate = 9,
        //    /// <summary>科目コード</summary>
        //    CLCTCD = 10
        //}

        /// <summary>科目置き換えハイライト処理</summary>
        /// <param name="ZaimuSheet">スプレッドシート</param>
        /// <param name="isExcel">Excel処理のフラグ</param>
        /// <param name="strStatementName">決算書名</param>
        //public static void SubjectChangeHighLight(FPWS.SheetView ZaimuSheet, bool isExcel, string strStatementName)
        //{

        //    // 共通関数
        //    // 距離により背景色を色付け

        //    // 科目ハイライト項目出力/非出力
        //    string showFlag = FCC.GetClientConfigFile("HIGH_LIGHT_SHOW_FLAG");
        //    int CLCTCDIdx = (int)spdCol_C.CLCTCD;
        //    if (showFlag != "1" && isExcel == true)
        //    {
        //        // 科目ハイライト項目が非表示の場合、Modifyの後ろがCLCTCD
        //        CLCTCDIdx = (int)spdCol_C.Modified + 1;
        //    }

        //    string workStatementName = strStatementName;
        //    int wMinHldis = 0;
        //    int wMinHlrate = 0;
        //    string wKeyMinHldis = "MIN_HLDIS" + "_" + Strings.Replace(workStatementName, " ", "_");
        //    string wKeyMinHlrate = "MIN_HLRATE" + "_" + Strings.Replace(workStatementName, " ", "_");
        //    if (!string.IsNullOrEmpty(FCC.GetClientConfigFile(wKeyMinHldis)))
        //        wMinHldis = Conversions.ToInteger(FCC.GetClientConfigFile(wKeyMinHldis));
        //    if (!string.IsNullOrEmpty(FCC.GetClientConfigFile(wKeyMinHlrate)))
        //        wMinHlrate = Conversions.ToInteger(FCC.GetClientConfigFile(wKeyMinHlrate));
        //    for (int rowIdx = 0, loopTo = ZaimuSheet.Rows.Count - 1; rowIdx <= loopTo; rowIdx++)
        //    {
        //        // 帳表名取得
        //        if (!string.IsNullOrEmpty(ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].Text) && string.IsNullOrEmpty(ZaimuSheet.Cells[rowIdx, CLCTCDIdx].Text))
        //        {
        //            // See All Resultの場合、帳表名が変更する必要がある
        //            workStatementName = ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].Text;
        //            wKeyMinHldis = "MIN_HLDIS" + "_" + Strings.Replace(workStatementName, " ", "_");
        //            wKeyMinHlrate = "MIN_HLRATE" + "_" + Strings.Replace(workStatementName, " ", "_");
        //            if (!string.IsNullOrEmpty(FCC.GetClientConfigFile(wKeyMinHldis)))
        //                wMinHldis = Conversions.ToInteger(FCC.GetClientConfigFile(wKeyMinHldis));
        //            if (!string.IsNullOrEmpty(FCC.GetClientConfigFile(wKeyMinHlrate)))
        //                wMinHlrate = Conversions.ToInteger(FCC.GetClientConfigFile(wKeyMinHlrate));
        //        }

        //        if (!string.IsNullOrEmpty(ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Distance].Text))
        //        {
        //            if (Conversions.ToInteger(ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Distance].Value) > wMinHldis || Conversions.ToInteger(ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Rate].Text.Replace("%", "")) > wMinHlrate)
        //            {
        //                ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].BackColor = System.Drawing.Color.Gold;
        //                ZaimuSheet.Cells[rowIdx, (int)spdCol_C.ReSubject].BackColor = System.Drawing.Color.Gold;
        //            }
        //        }
        //    }
        //}

        /// <summary>キー科目・合計科目・除外科目ハイライト、HighLight.txt参照科目ハイライト処理</summary>
        /// <param name="ZaimuSheet">スプレッドシート</param>
        /// <param name="isExcel">Excel処理のフラグ</param>
        /// <param name="strStatementName">決算書名</param>
        //public static void EmphasisHighlight(FPWS.SheetView ZaimuSheet, bool isExcel, string strStatementName)
        //{

        //    // 科目ハイライト項目出力/非出力
        //    string showFlag = FCC.GetClientConfigFile("HIGH_LIGHT_SHOW_FLAG");
        //    int CLCTCDIdx = (int)spdCol_C.CLCTCD;
        //    if (showFlag != "1" && isExcel == true)
        //    {
        //        // 科目ハイライト項目が非表示の場合、Modifyの後ろがCLCTCD
        //        CLCTCDIdx = (int)spdCol_C.Modified + 1;
        //    }

        //    string workStatementName = strStatementName;
        //    for (int rowIdx = 0, loopTo = ZaimuSheet.Rows.Count - 1; rowIdx <= loopTo; rowIdx++)
        //    {
        //        string CLCTCDStr = ZaimuSheet.Cells[rowIdx, CLCTCDIdx].Text;
        //        if (!string.IsNullOrEmpty(ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].Text) && string.IsNullOrEmpty(CLCTCDStr))
        //        {
        //            // See All Resultの場合、帳表名が変更する必要がある
        //            workStatementName = ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].Text;
        //        }

        //        // ver3.2 IT不具合No.33
        //        var color = System.Drawing.Color.White;
        //        var font = new System.Drawing.Font("Segoe UI", 11.5f, System.Drawing.FontStyle.Regular);
        //        if (CLCTCDStr.Length == 0)
        //            continue;
        //        switch (CLCTCDStr.Substring(0, 2) ?? "")
        //        {
        //            case "01":
        //                {
        //                    font = new System.Drawing.Font("Segoe UI", 11.5f, System.Drawing.FontStyle.Bold);
        //                    break;
        //                }

        //            case "02":
        //                {
        //                    color = System.Drawing.Color.PaleGreen;
        //                    font = new System.Drawing.Font("Segoe UI", 11.5f, System.Drawing.FontStyle.Italic);
        //                    break;
        //                }

        //            case "99":
        //                {
        //                    color = System.Drawing.Color.DarkGray;
        //                    break;
        //                }

        //            default:
        //                {
        //                    break;
        //                }
        //        }
        //        // フォント
        //        ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Note].Font = font;
        //        ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Division].Font = font;
        //        ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].Font = font;
        //        ZaimuSheet.Cells[rowIdx, (int)spdCol_C.ReSubject].Font = font;
        //        ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Standard].Font = font;
        //        ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Kingaku].Font = font;
        //        // 背景色
        //        if (color != default)
        //        {
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Note].BackColor = color;
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Division].BackColor = color;
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].BackColor = color;
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.ReSubject].BackColor = color;
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Standard].BackColor = color;
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Kingaku].BackColor = color;
        //        }

        //        // OCR科目とリカバリー科目に色付けをする項目を判断
        //        if (TinyDictionary.RecoveryHighLight(workStatementName, ZaimuSheet.Cells[rowIdx, (int)spdCol_C.ReSubject].Text.ToString()) == true)
        //        {
        //            // Digitized Account Title
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.Subject].BackColor = System.Drawing.Color.FromArgb(255, 250, 191, 143);
        //            // Recovered Account Title
        //            ZaimuSheet.Cells[rowIdx, (int)spdCol_C.ReSubject].BackColor = System.Drawing.Color.FromArgb(255, 250, 191, 143);
        //        }
        //    }
        //}

    }