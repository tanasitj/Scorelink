using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scorelink.MO;
using Scorelink.MO.DataModel;

namespace Scorelink.BO.Repositories
{
    public class DocumentInfoRepo : Interface.IDocumentInfo<DocumentInfoModel>
    {
        public DocumentInfoModel Get(int id)
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
                                CreateBy = doc.CreateBy,
                                CreateDate = doc.CreateDate.ToString()
                            }).FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<DocumentInfoModel> GetList(string id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from doc in db.DocumentInfo
                             where doc.CreateBy.Contains(id)
                             select new DocumentInfoModel
                             {
                                 DocId = doc.DocId,
                                 FileUID = doc.FileUID,
                                 FileName = doc.FileName,
                                 FilePath = doc.FilePath,
                                 FileUrl = doc.FileUrl,
                                 CreateBy = doc.CreateBy,
                                 CreateDate = doc.CreateDate.ToString()
                             });
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Add(DocumentInfoModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var docInfo = new DocumentInfo
                        {
                            FileUID = item.FileUID,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            FileUrl = item.FileUrl,
                            CreateBy = item.CreateBy,
                            CreateDate = DateTime.Parse(item.CreateDate)
                        };

                        db.DocumentInfo.Add(docInfo);
                        db.SaveChanges();

                        //var lastuser = db.Users.Select(x => x.UserId).Max();

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
                        //Delete Document Area.
                        var area = db.DocumentAreas.Where(x => x.DocId.ToString() == id);
                        db.DocumentAreas.RemoveRange(area);
                        //Delete Document Detail.
                        var docDet = db.DocumentDetails.Where(x => x.DocId.ToString() == id);
                        db.DocumentDetails.RemoveRange(docDet);
                        //Delete Document Info.
                        var docInfo = db.DocumentInfo.Where(x => x.DocId.ToString() == id).First();
                        db.DocumentInfo.Remove(docInfo);

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
