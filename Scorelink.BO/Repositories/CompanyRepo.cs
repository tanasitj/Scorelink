using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scorelink.BO.Helper;
using Scorelink.MO;
using Scorelink.MO.DataModel;

namespace Scorelink.BO.Repositories
{
    public class CompanyRepo
    {
        public string GetCompanyId(string compName)
        {
            string rst = "";
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from cmp in db.Company
                            where cmp.CompanyName == compName
                            select new CompanyModel
                            {
                                CompanyId = cmp.CompanyId
                            }).FirstOrDefault();

                rst = data.CompanyId.ToString();
                return rst;
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        public IEnumerable<CompanyModel> GetCompanyDDList()
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from u in db.Company
                             where u.Status == "Y"
                             orderby u.CompanyName 
                             select new CompanyModel
                             {
                                 CompanyId = u.CompanyId,
                                 CompanyName = u.CompanyName
                             });
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CompanyModel> GetList()
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from c in db.Company
                             select new CompanyModel
                             {
                                 CompanyId = c.CompanyId,
                                 CompanyName = c.CompanyName,
                                 Domain = c.Domain,
                                 Address = c.Address,
                                 Telephone = c.Telephone,
                                 Status = c.Status,
                                 CreateBy = c.CreateBy,
                                 CreateDate = c.CreateDate.ToString()
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

        public string Add(CompanyModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var company = new Company
                        {
                            CompanyName = item.CompanyName,
                            Domain = item.Domain,
                            Address = item.Address,
                            Telephone = item.Telephone,
                            Status = item.Status,
                            CreateBy = item.CreateBy,
                            CreateDate = DateTime.Now,
                            UpdateBy = item.UpdateBy,
                            UpdateDate = DateTime.Now,
                        };

                        db.Company.Add(company);
                        db.SaveChanges();

                        //var lastuser = db.Users.Select(x => x.UserId).Max();

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

        public bool CheckCompanyDup(string comp)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.Company.Where(x => x.CompanyName == comp).Any();

        }

        public bool CheckDomainDup(string domain)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.Company.Where(x => x.Domain == domain).Any();

        }

        public CompanyModel GetCompanyByName(string compname)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from comp in db.Company
                            where comp.CompanyName == compname
                            select new CompanyModel
                            {
                                CompanyId = comp.CompanyId,
                                CompanyName = comp.CompanyName,
                                Domain = comp.Domain,
                                Address = comp.Address,
                                Telephone = comp.Telephone,
                                Status = comp.Status,
                                CreateBy = comp.CreateBy,
                                CreateDate = DateTime.Now.ToString(),
                                UpdateBy = comp.UpdateBy,
                                UpdateDate = DateTime.Now.ToString()
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

        public CompanyModel GetCompanyById(int compId)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from comp in db.Company
                            where comp.CompanyId == compId
                            select new CompanyModel
                            {
                                CompanyId = comp.CompanyId,
                                CompanyName = comp.CompanyName,
                                Domain = comp.Domain,
                                Address = comp.Address,
                                Telephone = comp.Telephone,
                                Status = comp.Status,
                                CreateBy = comp.CreateBy,
                                CreateDate = DateTime.Now.ToString(),
                                UpdateBy = comp.UpdateBy,
                                UpdateDate = DateTime.Now.ToString()
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

        public string Update(CompanyModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var comp = db.Company.Where(x => x.CompanyId == item.CompanyId).First();
                        comp.CompanyName = item.CompanyName;
                        comp.Domain = item.Domain;
                        comp.Address = item.Address;
                        comp.Telephone = item.Telephone;
                        comp.Status = item.Status;
                        comp.UpdateBy = item.UpdateBy;
                        comp.UpdateDate = DateTime.Now;

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

    }
}
