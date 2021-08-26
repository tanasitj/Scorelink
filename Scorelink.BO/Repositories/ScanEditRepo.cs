using Scorelink.BO.Helper;
using Scorelink.MO;
using Scorelink.MO.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.BO.Repositories
{
    public class ScanEditRepo
    {
        public DocumentDetailModel GetDetails(int docId, string pagetype)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from doc in db.DocumentDetails
                            where doc.DocId == docId && doc.PageType == pagetype
                            select new DocumentDetailModel
                            {
                                DocDetId = doc.DocDetId,
                                DocId = doc.DocId,
                                DocPageNo = doc.DocPageNo,
                                FootnoteNo = doc.FootnoteNo,
                                PageType = doc.PageType,
                                ScanStatus = doc.ScanStatus,
                                PageFileName = doc.PageFileName,
                                PagePath = doc.PagePath,
                                PageUrl = doc.PageUrl,
                                Selected = doc.Selected,
                                PatternNo = doc.PatternNo,
                                CreateBy = doc.CreateBy,
                                CreateDate = doc.CreateDate.ToString(),
                                UpdateDate = doc.UpdateDate.ToString()
                            }).FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                throw ex;
            }
        }
        public DocumentInfoModel GetInfo(int id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from doc in db.DocumentInfo
                            where doc.DocId == id
                            select new DocumentInfoModel
                            {
                                DocId = doc.DocId,
                                FileUID = doc.FileUID,
                                FileName = doc.FileName,
                                FilePath = doc.FilePath,
                                FileUrl = doc.FileUrl,
                                Language = doc.Language,
                                CreateBy = doc.CreateBy,
                                CreateDate = doc.CreateDate.ToString()
                            }).FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                throw ex;
            }
        }
        public IEnumerable<DocumentDetailModel> GetList(int id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from doc in db.DocumentDetails
                             where doc.DocId == id
                             select new DocumentDetailModel
                             {
                                 DocDetId = doc.DocDetId,
                                 DocId = doc.DocId,
                                 DocPageNo = doc.DocPageNo,
                                 FootnoteNo = doc.FootnoteNo,
                                 PageType = doc.PageType,
                                 ScanStatus = doc.ScanStatus,
                                 PageFileName = doc.PageFileName,
                                 PagePath = doc.PagePath,
                                 PageUrl = doc.PageUrl,
                                 Selected = doc.Selected,
                                 PatternNo = doc.PatternNo,
                                 CreateBy = doc.CreateBy,
                                 CreateDate = doc.CreateDate.ToString(),
                                 UpdateDate = doc.UpdateDate.ToString()
                             });
                return query;
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                throw ex;
            }
        }
       
    }
}
