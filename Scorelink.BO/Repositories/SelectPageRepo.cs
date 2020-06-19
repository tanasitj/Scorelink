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
                        return ex.ToString();
                    }
                }
            }
        }
    }
}
