using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scorelink.MO;
using Scorelink.MO.DataModel;
using Scorelink.BO.Helper;

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
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
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
                        var docArea = db.DocumentAreas.Where(x => x.AreaNo == item.AreaNo && x.DocDetId == item.DocDetId && x.DocPageNo == item.DocPageNo).First();
                        //docArea.AreaNo = item.AreaNo;
                        //docArea.DocId = item.DocId;
                        //docArea.DocDetId = item.DocDetId;
                        //docArea.DocPageNo = item.DocPageNo;
                        docArea.PageType = item.PageType;
                        docArea.AreaX = item.AreaX;
                        docArea.AreaY = item.AreaY;
                        docArea.AreaH = item.AreaH;
                        docArea.AreaW = item.AreaW;
                        docArea.AreaPath = item.AreaPath;
                        docArea.AreaUrl = item.AreaUrl;
                        docArea.CreateBy = item.CreateBy;
                        docArea.CreateDate = DateTime.Parse(item.CreateDate);
                        docArea.UpdateDate = DateTime.Parse(item.UpdateDate);

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
                            DocPageNo = item.DocPageNo,
                            PageType = item.PageType,
                            AreaX = item.AreaX,
                            AreaY = item.AreaY,
                            AreaH = item.AreaH,
                            AreaW = item.AreaW,
                            AreaPath = item.AreaPath,
                            AreaUrl = item.AreaUrl,
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
                        Logger Err = new Logger();
                        Err.ErrorLog(ex.ToString());
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
                        Logger Err = new Logger();
                        Err.ErrorLog(ex.ToString());
                        return ex.ToString();
                    }
                }
            }
        }

        public bool CheckDocumentArea(int AreaNo, int docDetId, string DocPageNo)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.DocumentAreas.Where(x => x.AreaNo == AreaNo && x.DocDetId == docDetId && x.DocPageNo == DocPageNo).Any();
        }
    }
}
