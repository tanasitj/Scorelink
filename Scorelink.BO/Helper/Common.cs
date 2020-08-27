using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Leadtools;
using Leadtools.Pdf;
using System.Management;
using Scorelink.BO.Repositories;

namespace Scorelink.BO.Helper
{
    public class Common
    {
        //For get License Leadtools in Server.
        public static void GetLicenseLeadTool()
        {
            //string sPath = Consts.LeadtoolsLIC;
            string sPath = Common.getConstTxt("LeadtoolsLIC");
            try
            {
                string licenseFilePath = sPath + @"LEADTOOLS.LIC";
                string developerkeyPath = sPath + @"LEADTOOLS.LIC.key";
                developerkeyPath = Path.Combine(developerkeyPath);
                string developerkey = System.IO.File.ReadAllText(developerkeyPath);
                Leadtools.RasterSupport.SetLicense(licenseFilePath, developerkey);
            }
            catch (Exception ex)
            {

            }
        }

        //Create Folder
        //Parameter : Path
        public static void CreateDocFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }

        //Delete Before Create Folder
        //Parameter : Path
        public static void InitailDocFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }

        //Delete Folder
        //Parameter : Path
        public static void DeleteFolder(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (folderExists)
            {
                Directory.Delete(path, true);
            }
        }

        //Delete File
        //Parameter : Path
        public static void DeleteFile(string path)
        {
            bool fileExists = File.Exists(path);
            if (fileExists)
            {
                File.Delete(path);
            }
        }

        //Example   : Value 1 , Digit 3 = "001" | Value 10 , Digit 3 = "010"
        //Parameter : Value String , Digit Integer.
        //Return    : String.
        public static string GenZero(string value, int iDigit)
        {
            int iLen = value.Length;
            int iLoop = iDigit - iLen;
            string sZero = "";

            for (int i = 0; i < iLoop; i++)
            {
                sZero += "0";
            }

            string sOutput = sZero + value;

            return sOutput;
        }

        public static void PDFFileDeletePages(string sFrom, string sTo, int iPage)
        {
            GetLicenseLeadTool();

            string sourceFileName = sFrom; //Path.Combine(LEAD_VARS.ImagesDir, @"Leadtools.pdf");
            string destinationFileName = sTo; //Path.Combine(LEAD_VARS.ImagesDir, @"LEAD_DeletePages.pdf");

            // Get the number of pages in the source file 
            PDFFile file = new PDFFile(sourceFileName);
            int pageCount = file.GetPageCount();
            Console.WriteLine("Pages in source file : {0}", pageCount);

            // If the file has more than 1 page, delete all except the first page 
            if (pageCount > 1)
            {
                // -1 is (up to and including last page) 
                file.DeletePages(2, -1, destinationFileName);
            }
        }

        public static FREngine.ITextLanguage GetLanguageDB(FREngine.IEngine engine, string LanguageWord, string CustomDictionaryPass)
        {

            // Create new TextLanguage object
            FREngine.ILanguageDatabase LanguageDatabase;
            LanguageDatabase = engine.CreateLanguageDatabase();
            FREngine.ITextLanguage TextLanguage;
            TextLanguage = LanguageDatabase.CreateTextLanguage();

            // Copy all attributes from predefined Target language
            FREngine.ITextLanguage TargetLanguage;
            TargetLanguage = engine.PredefinedLanguages.Find(LanguageWord).TextLanguage;
            // ChinesePRC English  Japanese
            TextLanguage.CopyFrom(TargetLanguage);
            TextLanguage.InternalName = "FSFTextLanguage";

            // Bind new dictionary to first (and single) BaseLanguage object within TextLanguage
            FREngine.IBaseLanguage BaseLanguage;
            BaseLanguage = TextLanguage.BaseLanguages[0];

            // Change internal dictionary name to user-defined
            BaseLanguage.InternalName = "FSFBaseLanguage";

            // set custom dictinary for base language
            SetDictionary(BaseLanguage, engine, CustomDictionaryPass);

            return TextLanguage;
        }

        private static void SetDictionary(FREngine.IBaseLanguage BaseLanguage, FREngine.IEngine engine, string CustomDictionaryPass)
        {

            // カスタム辞書がない場合は何もしない
            if (System.IO.File.Exists(CustomDictionaryPass) == false)
                return;

            FREngine.IDictionaryDescription DictionaryDescription;

            // Create dictionary file
            MakeDictionary(engine, CustomDictionaryPass);    // カスタムの辞書ファイル作成するときはここのコメント部分を参考にする

            // ' Get collection of dictionary descriptions and remove all items
            FREngine.IDictionaryDescriptions DictionaryDescriptions;
            DictionaryDescriptions = BaseLanguage.DictionaryDescriptions;
            // DictionaryDescriptions.DeleteAll()						’サンプルではPredefine辞書を削除している

            // Create user dictionary description and add it to the collection
            DictionaryDescription = DictionaryDescriptions.AddNew(FREngine.DictionaryTypeEnum.DT_UserDictionary);

            FREngine.UserDictionaryDescription UserDictionaryDescription;
            UserDictionaryDescription = DictionaryDescription.GetAsUserDictionaryDescription();

            UserDictionaryDescription.FileName = CustomDictionaryPass;
        }

        private static void MakeDictionary(FREngine.IEngine engine, string CustomDictionaryPass)
        {
            // Create new dictionary
            FREngine.ILanguageDatabase LanguageDatabase;
            LanguageDatabase = engine.CreateLanguageDatabase();
            FREngine.IDictionary Dictionary;
            // すでに出来上がっている辞書ファイルでDictionaryを作成
            Dictionary = LanguageDatabase.OpenExistingDictionary(CustomDictionaryPass, FREngine.LanguageIdEnum.LI_EnglishUnitedStates);    // LI_Thaiに変えるとエラー出る
            Dictionary.Name = "FSFUserDictionary";
        }

        public struct OCRResult
        {
            public string Value;
            public int Top;
            public int Bottom;
        }

        public static FREngine.ITextLanguage GetTextLanguage(int flg, FREngine.IEngine engine, string LanguageWord, string CustomDictionaryPass)
        {
            FREngine.ILanguageDatabase FRLanguageDatabase = engine.CreateLanguageDatabase();
            FREngine.ITextLanguage FRTextLanguage = FRLanguageDatabase.CreateTextLanguage();
            switch (flg)
            {
                case 1:
                    {
                        FRTextLanguage = GetLanguageDB(engine, LanguageWord, CustomDictionaryPass);
                        FRTextLanguage.LetterSet[FREngine.TextLanguageLetterSetEnum.TLLS_ProhibitedLetters] = "^©¬®°±—‘’‛“”•′™■□▲△►▻▼▽◄◅◊◎◦★☆♦✓❖";
                        break;
                    }

                case 2:
                    {
                        FREngine.IBaseLanguages FRBaseLanguages = FRTextLanguage.BaseLanguages;
                        FREngine.IBaseLanguage FRBaseLanguage = FRBaseLanguages.AddNew();
                        //FRBaseLanguage.LetterSet[FREngine.BaseLanguageLetterSetEnum.BLLS_Alphabet] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz()-0123456789-+,.:()/%$'&";
                        FRBaseLanguage.LetterSet[FREngine.BaseLanguageLetterSetEnum.BLLS_Alphabet] = "()-.,0123456789";

                        //FRTextLanguage = GetLanguageDB(engine, LanguageWord, CustomDictionaryPass);
                        //FRTextLanguage.LetterSet[FREngine.TextLanguageLetterSetEnum.TLLS_ProhibitedLetters] = "^©¬®°±—‘’‛“”•′™■□▲△►▻▼▽◄◅◊◎◦★☆♦✓❖";
                        break;
                    }

                case 3:
                    {
                        FREngine.IBaseLanguages FRBaseLanguages = FRTextLanguage.BaseLanguages;
                        FREngine.IBaseLanguage FRBaseLanguage = FRBaseLanguages.AddNew();
                        FRBaseLanguage.LetterSet[FREngine.BaseLanguageLetterSetEnum.BLLS_Alphabet] = "()-.,0123456789";

                        //FRTextLanguage = GetLanguageDB(engine, LanguageWord, CustomDictionaryPass);
                        //FRTextLanguage.LetterSet[FREngine.TextLanguageLetterSetEnum.TLLS_ProhibitedLetters] = "^©¬®°±—‘’‛“”•′™■□▲△►▻▼▽◄◅◊◎◦★☆♦✓❖";
                        break;
                    }
            }

            return FRTextLanguage;
        }

        public static List<List<OCRResult>> GetOCRResult(FREngine.IEngine Engine, FREngine.FRDocument FRDocument)
        {
            var dicResult = new List<List<OCRResult>>();

            // ************************************************************************
            // Get OCR result
            // ************************************************************************
            for (int cnt = 0, loopTo = FRDocument.Pages[0].Layout.Blocks.Count - 1; cnt <= loopTo; cnt++)
            {
                var dicInfo = new List<OCRResult>();
                FREngine.ITextBlock FRTextBlock = FRDocument.Pages[0].Layout.Blocks[cnt].GetAsTextBlock();
                if (FRTextBlock is null)
                    continue;
                FREngine.IParagraphs Paragraphs = FRTextBlock.Text.Paragraphs;
                for (int cnt2 = 0, loopTo1 = Paragraphs.Count - 1; cnt2 <= loopTo1; cnt2++)
                {
                    var result = new OCRResult();
                    long wTop = 0;
                    long wBottom = 0;
                    int charCnt = 0;
                    FREngine.ICharParams FRCharparams = Engine.CreateCharParams();
                    var loopTo2 = Paragraphs[cnt2].Length - 1;
                    for (int ichar = 0; ichar <= loopTo2; ichar++)
                    {
                        Paragraphs[cnt2].GetCharParams(ichar, FRCharparams);
                        string character = "";
                        character = Paragraphs[cnt2].Text.Substring(ichar, 1);
                        result.Value += character;
                        if (FRCharparams.Top != 0)
                        {
                            wTop += FRCharparams.Top;
                            wBottom += FRCharparams.Bottom;
                            charCnt += 1;
                        }

                        // Convert to hexadecimal
                        var data = Encoding.UTF8.GetBytes(result.Value);
                        string hexText = BitConverter.ToString(data);
                        if (hexText.IndexOf("-E2-80-A8") >= 0)
                        {
                            // 改行コード(L SEP)が含まれている場合、単語を区切る
                            hexText = hexText.Replace("-E2-80-A8", "");
                            var hexChars = hexText.Split('-');
                            byte[] decData;
                            decData = new byte[(hexChars.Count())];
                            for (int i = 0, loopTo3 = hexChars.Count() - 1; i <= loopTo3; i++)
                                decData[i] = Convert.ToByte(hexChars[i], 16);
                            result.Value = System.Text.Encoding.UTF8.GetString(decData);  // UTF8のバイト列からstringに変換
                            //result.Value = result.Value.Replace(Constants.vbLf, "");
                            result.Value = result.Value.Replace("\n", "");
                            //result.Value = result.Value.Replace(Constants.vbTab, "");
                            result.Value = result.Value.Replace("\t", "");
                            result.Top = unchecked((int)wTop / charCnt);
                            result.Bottom = unchecked((int)wBottom / charCnt);
                            dicInfo.Add(result);

                            // 初期化
                            result = new OCRResult();
                            wTop = 0;
                            wBottom = 0;
                            charCnt = 0;
                        }
                    }

                    if (charCnt > 0)
                    {
                        result.Value = result.Value.Replace("\n", "");
                        result.Value = result.Value.Replace("\t", "");
                        //result.Top = Conversions.ToInteger(wTop / (double)charCnt);
                        //result.Bottom = Conversions.ToInteger(wBottom / (double)charCnt);
                        result.Top = unchecked((int)wTop / charCnt);
                        result.Bottom = unchecked((int)wBottom / charCnt);
                        dicInfo.Add(result);
                    }
                }

                dicResult.Add(dicInfo);
            }

            return dicResult;
        }

        public static string EditOCRResult(List<List<OCRResult>> dicResult)
        {
            string ret = "";

            // ************************************************************************
            // OCR結果を編集
            // ************************************************************************
            // 各列の行数の合計を取得
            int sumRow = 0;
            for (int colIdx = 0, loopTo = dicResult.Count - 1; colIdx <= loopTo; colIdx++)
            {
                {
                    var withBlock = dicResult[colIdx];
                    sumRow += withBlock.Count;
                }
            }
            // 全ての列の行がなくなるまで繰り返す
            while (sumRow != 0)
            {
                int minTop = 0;
                int minBottom = 0;

                // 各列の一番上の行の一番高いTopとBottomを取得
                for (int colIdx = 0, loopTo1 = dicResult.Count - 1; colIdx <= loopTo1; colIdx++)
                {
                    if (dicResult[colIdx].Count == 0)
                        continue;
                    minTop = dicResult[colIdx][0].Top;
                    minBottom = dicResult[colIdx][0].Bottom;
                    break;
                }

                for (int colIdx = 0, loopTo2 = dicResult.Count - 1; colIdx <= loopTo2; colIdx++)
                {
                    if (dicResult[colIdx].Count == 0)
                        continue;
                    if (minTop > dicResult[colIdx][0].Top)
                    {
                        minTop = dicResult[colIdx][0].Top;
                        minBottom = dicResult[colIdx][0].Bottom;
                    }
                }

                // 一番高いTopとBottomに近い行を出力
                for (int colIdx = 0, loopTo3 = dicResult.Count - 1; colIdx <= loopTo3; colIdx++)
                {
                    if (dicResult[colIdx].Count == 0)
                    {
                        if (colIdx != dicResult.Count - 1)
                            ret = ret + "\"" + "\"" + ",";
                        continue;
                    }

                    {
                        var withBlock1 = dicResult[colIdx];

                        /// For rowidx As Integer = 0 To .Count - 1
                        int rowidx = 0;
                        // todo:暫定
                        // Dim rat As Double = System.Math.Abs(100 - (System.Math.Abs(100 - (.Item(rowidx).Top / minTop) * 100) + System.Math.Abs(100 - (.Item(rowidx).Bottom / minBottom) * 100)))
                        /// Dim rat As Double = 100 - (System.Math.Abs(100 - (.Item(rowidx).Top / minTop) * 100) + System.Math.Abs(100 - (.Item(rowidx).Bottom / minBottom) * 100))

                        double rat = 0;
                        if (Math.Abs(Math.Max(withBlock1[rowidx].Bottom, minBottom) - Math.Min(withBlock1[rowidx].Top, minTop)) != 0)
                        {
                            rat = (Math.Min(withBlock1[rowidx].Bottom, minBottom) - Math.Max(withBlock1[rowidx].Top, minTop)) / (double)(Math.Max(withBlock1[rowidx].Bottom, minBottom) - Math.Min(withBlock1[rowidx].Top, minTop)) * 100;
                        }

                        rat = ToRoundDown(rat, 0);
                        /// If rat >= 95 And rat <= 100 Then
                        if (rat >= 10)
                        {
                            ret = ret + "\"" + withBlock1[rowidx].Value + "\"";
                            minTop = (minTop + withBlock1[rowidx].Top) / 2;
                            minBottom = (minBottom + withBlock1[rowidx].Bottom) / 2;

                            // 出力した行は削除する
                            withBlock1.Remove(withBlock1[rowidx]);
                            /// Exit For
                        }
                        /// Next

                        if (colIdx != dicResult.Count - 1)
                            ret = ret + ",";
                    }
                }

                ret = ret + "\r\n";

                // 各列の行数の合計を再取得
                sumRow = 0;
                for (int colIdx = 0, loopTo4 = dicResult.Count - 1; colIdx <= loopTo4; colIdx++)
                {
                    {
                        var withBlock2 = dicResult[colIdx];
                        sumRow += withBlock2.Count;
                    }
                }
            }

            return ret;
        }

        public static double ToRoundDown(double dValue, int iDigits)
        {
            double dCoef = Math.Pow(10, iDigits);
            if (dValue > 0)
            {
                return Math.Floor(dValue * dCoef) / dCoef;
            }
            else
            {
                return Math.Ceiling(dValue * dCoef) / dCoef;
            }
        }

        //Delete all file in folder expect name pattern.
        //Parameter : folder String ex. C:\\Temp\\ , name string ex. test*.txt .
        //Return    : Boolean.
        public static bool DeleteAllFile(string folder, string namePattearn)
        {
            try
            {
                var dir = new DirectoryInfo(folder);

                foreach (var file in dir.EnumerateFiles(namePattearn))
                {
                    using (var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Write, FileShare.Read))
                    {
                        stream.Close();
                        stream.Dispose();
                        file.Delete();
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Get IP Address
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        //Get MAC Address
        public static string GetMACAddress()
        {
            string sMac = "";

            using (var mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    Console.WriteLine(mo["MacAddress"].ToString());
                    sMac = mo["MacAddress"].ToString();
                }

                return sMac;
            }
        }

        public static string GetCPUID()
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }

            return id;
        }

        public static string EncryptText(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public string DecryptText(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

        public static string getConstTxt(string constname)
        {
            SysConfigRepo sys = new SysConfigRepo();
            return sys.GetConstTxt(constname);
        }

        public static int getConstNum(string constname)
        {
            SysConfigRepo sys = new SysConfigRepo();
            return sys.GetConstNum(constname);
        }

    }
}
