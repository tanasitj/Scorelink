using Scorelink.MO;
using Scorelink.MO.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.BO.Repositories
{
    public class SelectPatternRepo
    {
        public DocumentDetailModel Get(int docId, string sPageType)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from doc in db.DocumentDetails
                            where doc.DocId == docId && doc.PageType == sPageType
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
                throw ex;
            }
        }
        public string Update(DocumentDetailModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var docDet = db.DocumentDetails.Where(x => x.DocId == item.DocId && x.PageType == item.PageType).ToList();

                        foreach (DocumentDetail det in docDet)
                        {
                            det.PatternNo = item.PatternNo;
                            det.ScanStatus = item.ScanStatus;
                            det.UpdateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        }

                        db.SaveChanges();
                        dbTran.Commit();

                        return "OK";
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        return ex.ToString();
                    }
                }
            }
        }
    }
}
