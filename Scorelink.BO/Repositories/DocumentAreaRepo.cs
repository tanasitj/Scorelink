using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scorelink.MO;
using Scorelink.MO.DataModel;

namespace Scorelink.BO.Repositories
{
    public class DocumentAreaRepo
    {
        public IEnumerable<DocumentAreaModel> GetList(string id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from doc in db.DocumentAreas
                             where doc.CreateBy.Contains(id)
                             select new DocumentAreaModel
                             {
                                 AreaNo = doc.AreaNo,
                                 DocId = doc.DocId,
                                 DocDetId = doc.DocDetId,
                                 AreaX = doc.AreaX,
                                 AreaY = doc.AreaY,
                                 AreaH = doc.AreaH,
                                 AreaW = doc.AreaW,
                                 AreaPath = doc.AreaPath,
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

        public string Update(DocumentAreaModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var docDet = db.DocumentAreas.Where(x => x.DocDetId == item.DocDetId).First();
                        docDet.AreaNo = item.AreaNo;
                        docDet.DocId = item.DocId;
                        docDet.DocDetId = item.DocDetId;
                        docDet.AreaX = item.AreaX;
                        docDet.AreaY = item.AreaY;
                        docDet.AreaH = item.AreaH;
                        docDet.AreaW = item.AreaW;
                        docDet.AreaPath = item.AreaPath;
                        docDet.CreateBy = item.CreateBy;
                        docDet.CreateDate = DateTime.Parse(item.CreateDate);
                        docDet.UpdateDate = DateTime.Parse(item.UpdateDate);

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

        public string Add(DocumentAreaModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var doc = new DocumentArea
                        {
                            AreaNo = item.AreaNo,
                            DocId = item.DocId,
                            DocDetId = item.DocDetId,
                            AreaX = item.AreaX,
                            AreaY = item.AreaY,
                            AreaH = item.AreaH,
                            AreaW = item.AreaW,
                            AreaPath = item.AreaPath,
                            CreateBy = item.CreateBy,
                            CreateDate = DateTime.Parse(item.CreateDate),
                            UpdateDate = DateTime.Parse(item.CreateDate)
                        };

                        db.DocumentAreas.Add(doc);
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

        public string Delete(string id)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var doc = db.DocumentAreas.Where(x => x.AreaNo.ToString() == id).First();
                        db.DocumentAreas.Remove(doc);

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
