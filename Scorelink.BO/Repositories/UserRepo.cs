using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scorelink.BO.Helper;
using Scorelink.MO;
using Scorelink.MO.DataModel;

namespace Scorelink.BO.Repositories
{
    public class UserRepo
    {
        CultureInfo provider = CultureInfo.InvariantCulture;
        public string Add(UserModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new User
                        {
                            UserName = item.UserName,
                            Name = item.Name,
                            Surname = item.Surname,
                            Password = Common.EncryptText(item.Password),
                            Email = item.Email,
                            Company = item.Company,
                            Address = item.Address,
                            Telephone = item.Telephone,
                            Status = item.Status,
                            Admin = item.Admin,
                            RegisterDate = DateTime.Now,
                            ExpireDate = DateTime.Now,
                            UpdateBy = item.UpdateBy,
                            UpdateDate = DateTime.Now,
                        };

                        db.Users.Add(user);
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
        public bool CheckUserDup(string username)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.Users.Where(x => x.UserName == username).Any();

        }
        public bool CheckLogIn(string username, string password)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.Users.Where(x => x.UserName == username && x.Password == password).Any();
            
        }
        public UserModel Get(string username)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from user in db.Users
                            where user.UserName == username
                            select new UserModel
                            {
                                UserId = user.UserId,
                                UserName = user.UserName,
                                Name = user.Name,
                                Surname = user.Surname,
                                Password = user.Password,
                                Email = user.Email,
                                Company = user.Company,
                                Address = user.Address,
                                Telephone = user.Telephone,
                                Status = user.Status,
                                Admin = user.Admin,
                                RegisterDate = user.RegisterDate,
                                ExpireDate = user.ExpireDate,
                                UpdateBy = user.UpdateBy,
                                UpdateDate = user.UpdateDate
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
        public UserModel Get(int userid)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from user in db.Users
                            where user.UserId == userid
                            select new UserModel
                            {
                                UserId = user.UserId,
                                UserName = user.UserName,
                                Name = user.Name,
                                Surname = user.Surname,
                                Password = user.Password,
                                Email = user.Email,
                                Company = user.Company,
                                Address = user.Address,
                                Telephone = user.Telephone,
                                Status = user.Status,
                                Admin = user.Admin,
                                RegisterDate = user.RegisterDate,
                                ExpireDate = user.ExpireDate,
                                UpdateBy = user.UpdateBy,
                                UpdateDate = user.UpdateDate
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
        public IEnumerable<UserModel> GetUserList()
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from t1 in db.Users
                             join t2 in db.Company on t1.Company equals t2.CompanyId into t
                             from rt in t.DefaultIfEmpty()
                             select new UserModel
                             {
                                 UserId = t1.UserId,
                                 UserName = t1.UserName,
                                 Name = t1.Name,
                                 Surname = t1.Surname,
                                 FullName = t1.Name + " " + t1.Surname,
                                 //Password = t1.Password,
                                 Email = t1.Email,
                                 Company = t1.Company,
                                 CompanyName = rt.CompanyName,
                                 Address = t1.Address,
                                 Telephone = t1.Telephone,
                                 Status = t1.Status,
                                 Admin = t1.Admin,
                                 RegisterDate = t1.RegisterDate,
                                 ExpireDate = t1.ExpireDate,
                                 UpdateBy = t1.UpdateBy,
                                 UpdateDate = t1.UpdateDate
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
        public IEnumerable<CompanyModel> GetCompanyDD()
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from c in db.Company
                             where c.Status == "Y"
                             select new CompanyModel
                             {
                                 CompanyId = c.CompanyId,
                                 CompanyName = c.CompanyName
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
        public UserModel GetUserById(int id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from t1 in db.Users
                            join t2 in db.Company on t1.Company equals t2.CompanyId into t
                            from rt in t.DefaultIfEmpty()
                            where t1.UserId == id
                            select new UserModel
                            {
                                UserId = t1.UserId,
                                UserName = t1.UserName,
                                Name = t1.Name,
                                Surname = t1.Surname,
                                FullName = t1.Name + " " + t1.Surname,
                                Password = t1.Password,
                                Email = t1.Email,
                                Company = t1.Company,
                                CompanyName = rt.CompanyName,
                                Address = t1.Address,
                                Telephone = t1.Telephone,
                                Status = t1.Status,
                                Admin = t1.Admin,
                                RegisterDate = t1.RegisterDate,
                                ExpireDate = t1.ExpireDate,
                                UpdateBy = t1.UpdateBy,
                                UpdateDate = t1.UpdateDate
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
        public string Update(UserModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var data = db.Users.Where(x => x.UserId == item.UserId).First();
                        //data.UserName = item.UserName;
                        data.Name = item.Name;
                        data.Surname = item.Surname;
                        //data.Password = Common.EncryptText(item.Password);
                        data.Email = item.Email;
                        data.Company = item.Company;
                        data.Address = item.Address;
                        data.Telephone = item.Telephone;
                        data.Status = item.Status;
                        data.Admin = item.Admin;
                        data.RegisterDate = DateTime.ParseExact(item.RegisterDateStr, "dd/MM/yyyy", provider);
                        //data.RegisterDate = item.RegisterDate;
                        data.ExpireDate = DateTime.ParseExact(item.ExpireDateStr, "dd/MM/yyyy", provider);
                        //data.ExpireDate = item.ExpireDate;
                        data.UpdateBy = item.UpdateBy;
                        data.UpdateDate = DateTime.Now;

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
        public bool CheckExpireDate(int userid)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.Users.Where(x => x.UserId == userid && x.ExpireDate >= DateTime.Now).Any();

        }
        public string UpdateExpireUser()
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var userDB = db.Users.Where(x => x.Admin == "N" && x.Status == "Y" && x.ExpireDate < DateTime.Now).ToList();

                        foreach (User u in userDB)
                        {
                            u.Status = "N";
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
    }
}
