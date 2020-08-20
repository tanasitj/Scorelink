using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Scorelink.BO.Repositories;
using System.Linq;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Leadtools.Document;
using System.IO;
using ABBYEngine;
using FREngine;
using NLog.Internal;

namespace Scorelink.web.Controllers
{
    public class SelectAreaController : Controller
    {
        SelectAreaRepo doc = new SelectAreaRepo();

        // GET: SelectArea
        public ActionResult Index(DocumentDetailModel item)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewBag.UserId = Session["UserId"].ToString();
                int iUserId = 0;
                Int32.TryParse(Session["UserId"].ToString(), out iUserId);

                //Get User Info.
                UserRepo userRepo = new UserRepo();
                var userDB = userRepo.Get(iUserId);
                ViewBag.Name = userDB.Name;
                ViewBag.Surname = userDB.Surname;

                //Check and Update online date time.
                OnlineUserRepo onlineRepo = new OnlineUserRepo();
                var online = onlineRepo.Get(iUserId);
                onlineRepo.CheckTimeOut(online);

                ViewBag.Id = item.DocId;
                ViewBag.PatternNo = item.PatternNo;
                //Get Document Info data.
                var docInfo = doc.GetDocInfo(item.DocId);
                //Get Document Detail data.
                var docDet = doc.GetDocDet(item.DocId, item.PageType);
                //Set Path for Image.
                string sPagePath = Common.getConstTxt("sUrl") + "/FileUploads/" + Common.GenZero(docInfo.CreateBy, 8) + "/" + docInfo.FileUID + "/" + "PG" + Common.GenZero(docDet.PageType, 5) + Common.GenZero(docDet.DocPageNo, 4) + ".jpg";

                ViewBag.DocDetId = docDet.DocDetId;
                ViewBag.DocPageNo = docDet.DocPageNo;
                ViewBag.PageType = docDet.PageType;
                ViewBag.PageFileName = docDet.PageFileName;
                ViewBag.PagePath = sPagePath;
            }
            return View("SelectAreaMain");
        }

        public JsonResult SaveArea(List<String> values, int docId, int docDetId)
        {
            //Get File UID for Create Folder to Save Area.
            DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
            var docInfo = docInfoRepo.Get(docId);

            //Get Document Detail data.
            var docDet = doc.GetDocDet(docDetId);
            //Set Path
            String sFrom = Common.getConstTxt("SLUserFlie") + Common.GenZero(docInfo.CreateBy, 8) + "\\" + docInfo.FileUID + "\\" + "PG" + Common.GenZero(docDet.PageType,5) + Common.GenZero(docDet.DocPageNo, 4) + ".jpg";
            String sFromTif = Common.getConstTxt("SLUserFlie") + Common.GenZero(docInfo.CreateBy, 8) + "\\" + docInfo.FileUID + "\\" + "PG" + Common.GenZero(docDet.PageType, 5) + Common.GenZero(docDet.DocPageNo, 4) + ".tif";
            String sSaveFolder = Server.MapPath("..\\FileUploads\\" + Common.GenZero(docInfo.CreateBy, 8) + "\\" + docInfo.FileUID + "\\");
            String sUrlPath = Common.getConstTxt("sUrl") + "/FileUploads/" + Common.GenZero(docInfo.CreateBy, 8) + "/" + docInfo.FileUID + "/";

            //ABBY Load Engine
            EngineLoader engineLoader = new EngineLoader();
            IEngine engine = default;

            String sLanguageFolder = Server.MapPath("..\\Language\\");
            String sLanguageFile = "th_ABBYY.dic";
            String CustomDictionaryPass = sLanguageFolder + sLanguageFile;

            engineLoader.LoadEngine();
            engine = engineLoader.Engine;

            try
            {
                PrepareImageMode FRPrepareImageMode = engine.CreatePrepareImageMode();
                FRPrepareImageMode.AutoOverwriteResolution = true;
                FRDocument FRDocument = engine.CreateFRDocument();
                //String sOCRFileName = "OCR" + Common.GenZero(docDet.PageType, 5) + Common.GenZero(docDet.DocPageNo, 4) + ".txt";
                String sOCRFileName = "OCR" + Common.GenZero(docDet.PageType, 5) + Common.GenZero(docDet.DocPageNo, 4) + ".csv";
                String sOCRAllFileName = "RST" + Common.GenZero(docDet.PageType, 5) + ".csv";
                //Save OCR per page name
                String sSaveOCR = sSaveFolder + sOCRFileName;
                //Save OCR all page name
                String sSaveOCRAll = sSaveFolder + sOCRAllFileName;

                for (int i = 0; i < values.Count; i++)
                {
                    int iRunNo = i + 1;
                    String sFileName = "AR" + Common.GenZero(docDet.PageType, 5) + Common.GenZero(docDet.DocPageNo, 4) + Common.GenZero(iRunNo.ToString(), 4) + ".tif";
                    String sSave = sSaveFolder + sFileName;
                    String sSaveUrl = sUrlPath + sFileName;
                    String sCrop = values[i];
                    String[] aArea = sCrop.Split('|');
                    int x, y, w, h;


                    //Check for Delete File for Initail.
                    if (System.IO.File.Exists(sSave))
                    {
                        System.IO.File.Delete(sSave);
                    }
                    //Check Convert String Before Crop Image.
                    if (Int32.TryParse(aArea[1], out x) && Int32.TryParse(aArea[2], out y) && Int32.TryParse(aArea[3], out w) && Int32.TryParse(aArea[4], out h))
                    {
                        cropImage(Image.FromFile(sFrom), new Rectangle(x, y, w, h)).Save(sSave, ImageFormat.Tiff);

                        //Insert or Update table DocumentArea
                        DocumentAreaModel docArea = new DocumentAreaModel();
                        docArea.AreaNo = iRunNo;
                        docArea.DocId = docId;
                        docArea.DocDetId = docDetId;
                        docArea.DocPageNo = docDet.DocPageNo;
                        docArea.PageType = docDet.PageType;
                        docArea.AreaX = x.ToString();
                        docArea.AreaY = y.ToString();
                        docArea.AreaH = h.ToString();
                        docArea.AreaW = w.ToString();
                        docArea.AreaPath = sSave;
                        docArea.AreaUrl = sSaveUrl;
                        docArea.CreateBy = docInfo.CreateBy;
                        docArea.CreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        docArea.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                        //OCR process by ABBY
                        //processImageABBY(sSave, sSaveOCR, h, w);

                        //Check Existing Data before Update or Insert data.
                        DocumentAreaRepo docAreaRepo = new DocumentAreaRepo();
                        if (docAreaRepo.CheckDocumentArea(iRunNo, docDetId, docDet.DocPageNo))
                        {
                            docAreaRepo.Update(docArea);
                        }
                        else
                        {
                            docAreaRepo.Add(docArea);
                        }

                        //Update ScanStatus table DocumentDetail
                        DocumentDetailModel objDocDet = new DocumentDetailModel();
                        objDocDet.DocDetId = docDetId;
                        objDocDet.ScanStatus = "Y";
                        objDocDet.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();
                        docDetRepo.UpdateScanStatus(objDocDet);

                        //-- ABBY OCR Process --//
                        // Add image file to document
                        //FRDocument.AddImageFile(sSave, FRPrepareImageMode);
                        FRDocument.AddImageFile(sFrom, FRPrepareImageMode);
                        IRegion FRRegion = engine.CreateRegion();
                        int left = x;
                        int top = y;
                        int right = x+w;
                        int bottom = y+h;

                        FRRegion.AddRect(left, top, right, bottom);
                        IBlock newBlock = FRDocument.Pages[0].Layout.Blocks.AddNew(BlockTypeEnum.BT_Text, FRRegion);
                        // Set Dictionary
                        newBlock.GetAsTextBlock().RecognizerParams.TextLanguage = Common.GetTextLanguage(iRunNo, engine, "Thai", CustomDictionaryPass); //Common.GetLanguageDB(engine, "Thai", CustomDictionaryPass);//
                        // Specify horizontal writing
                        ITextOrientation wTextOrientation = engine.CreateTextOrientation();
                        wTextOrientation.ReadingType = ReadingTypeEnum.TRT_LinesBased;
                        newBlock.GetAsTextBlock().TextOrientation = wTextOrientation;
                        //----------------------//
                    }
                }
                // Recognize document
                FRDocument.Recognize();
                // Get OCR result
                var dicResult = new List<List<Common.OCRResult>>();
                string csvData;
                dicResult = Common.GetOCRResult(engine, FRDocument);
                csvData = Common.EditOCRResult(dicResult);

                // Close document
                FRDocument.Close();
                FRDocument = default;

                //Save per page
                string csvFileName = sSaveOCR;
                using (var sw = new StreamWriter(csvFileName, false, System.Text.Encoding.GetEncoding("UTF-16")))
                {
                    sw.Write(csvData);
                    sw.Close();
                }


                //Save all page
                const int chunkSize = 2 * 1024; // 2KB
                string[] inputFiles = Directory.GetFiles(sSaveFolder, "OCR" + Common.GenZero(docDet.PageType, 5) + "?????.csv");

                if (System.IO.File.Exists(sSaveOCRAll))
                {
                    using (var output = System.IO.File.Create(sSaveOCRAll))
                    {
                        foreach (var file in inputFiles)
                        {
                            using (var input = System.IO.File.OpenRead(file))
                            {
                                var buffer = new byte[chunkSize];
                                int bytesRead;
                                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    output.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (var output = System.IO.File.Create(sSaveOCRAll))
                    {
                        foreach (var file in inputFiles)
                        {
                            using (var input = System.IO.File.OpenRead(file))
                            {
                                var buffer = new byte[chunkSize];
                                int bytesRead;
                                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    output.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            finally
            {
                GC.Collect(0);
                engineLoader.Dispose();
            }

            //Return Next Page Data
            var data = doc.GetDocDet(docId,docDet.PageType);
            if (data == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                data.PagePath = Common.getConstTxt("sUrl") + "/FileUploads/" + Common.GenZero(docInfo.CreateBy, 8) + "/" + docInfo.FileUID + "/" + "PG" + Common.GenZero(data.PageType, 5) + Common.GenZero(data.DocPageNo, 4) + ".jpg";
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            //bmpCrop.Dispose();
            bmpImage.Dispose();
            return (Image)(bmpCrop);
        }

        private void processImageABBY(string imagePath, string resultPath, int H, int W)
        {

            //ABBY Load Engine
            EngineLoader engineLoader = new EngineLoader();
            IEngine engine = default;

            String sLanguageFolder = Server.MapPath("..\\Language\\");
            String sLanguageFile = "th_ABBYY.dic";

            // Load ABBY Engine.
            engineLoader.LoadEngine();
            engine = engineLoader.Engine;

            PrepareImageMode FRPrepareImageMode = engine.CreatePrepareImageMode();
            FRPrepareImageMode.AutoOverwriteResolution = true;
            FRDocument FRDocument = engine.CreateFRDocument();

            try
            {
                String CustomDictionaryPass = sLanguageFolder + sLanguageFile;

                // Add image file to document
                FRDocument.AddImageFile(imagePath, FRPrepareImageMode);

                IRegion FRRegion = engine.CreateRegion();
                FRRegion.AddRect(0, 0, W, H);
                IBlock newBlock = FRDocument.Pages[0].Layout.Blocks.AddNew(FREngine.BlockTypeEnum.BT_Text, FRRegion);

                newBlock.GetAsTextBlock().RecognizerParams.TextLanguage = Common.GetLanguageDB(engine, "Thai", CustomDictionaryPass);

                ITextOrientation wTextOrientation = engine.CreateTextOrientation();
                wTextOrientation.ReadingType = ReadingTypeEnum.TRT_LinesBased;
                newBlock.GetAsTextBlock().TextOrientation = wTextOrientation;

                // Recognize document
                FRDocument.Recognize();

                // Get OCR result
                var dicResult = new List<List<Common.OCRResult>>();
                string csvData;

                dicResult = Common.GetOCRResult(engine, FRDocument);
                csvData = Common.EditOCRResult(dicResult);

                // Close document
                FRDocument.Close();
                FRDocument = default;

                string csvFileName = resultPath;
                using (var sw = new StreamWriter(csvFileName, false, System.Text.Encoding.GetEncoding("UTF-16")))
                {
                    sw.Write(csvData);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            finally
            {
                engineLoader.Dispose();
            }
        }
    }
}