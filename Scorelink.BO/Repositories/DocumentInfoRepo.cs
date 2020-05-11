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
        public IEnumerable<DocumentInfoModel> GetList(string id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from doc in db.DocumentInfoes
                             where doc.CreateBy.Contains(id)
                             select new DocumentInfoModel
                             {
                                 DocId = doc.DocId,
                                 FileUID = doc.FileUID,
                                 FileName = doc.FileName,
                                 FilePath = doc.FilePath,
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
                            CreateBy = item.CreateBy,
                            CreateDate = DateTime.Parse(item.CreateDate)
                        };

                        db.DocumentInfoes.Add(docInfo);
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
                        var user = db.DocumentInfoes.Where(x => x.DocId.ToString() == id).First();
                        db.DocumentInfoes.Remove(user);

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
