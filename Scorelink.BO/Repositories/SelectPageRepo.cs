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
    public class SelectPageRepo
    {
        public string UpdatePathFile(DocumentDetailModel item)
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
                            det.ScanStatus = item.ScanStatus;
                            det.PagePath = item.PagePath;
                            det.PageUrl = item.PageUrl;
                            det.UpdateDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                        }

                        db.SaveChanges();
                        dbTran.Commit();

                        return "OK";
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        Logger Err = new Logger();
                        Err.ErrorLog(ex.ToString());
                        return ex.ToString();
                    }
                }
            }
        }

        public IEnumerable<F_DocumentDetailModel> GetListView(int id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from doc in db.F_DocumentDetail(id)
                             where doc.DocId == id
                             select new F_DocumentDetailModel
                             {
                                 DocId = doc.DocId,
                                 DocPageNo = doc.PageNo,
                                 FootnoteNo = doc.FootnoteNo,
                                 PageType = doc.StatementId.ToString(),
                                 PageTypeName = doc.StatementName,
                                 NoScan = doc.NoScan.ToString(),
                                 Commited = doc.Commited
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
        public bool CheckDocPage(DocumentDetailModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.DocumentDetails.Where(x => x.DocId == item.DocId && x.DocPageNo == item.DocPageNo).Any();

        }
    }
}
