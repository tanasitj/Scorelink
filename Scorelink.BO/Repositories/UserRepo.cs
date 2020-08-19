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
    public class UserRepo
    {
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
                            Status = "A",
                            Admin = "N",
                            RegisterDate = DateTime.Now,
                            ExpireDate = DateTime.Now,
                            UpdateBy = "System",
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
                                RegisterDate = user.RegisterDate.ToString(),
                                ExpireDate = user.ExpireDate.ToString(),
                                UpdateBy = user.UpdateBy,
                                UpdateDate = user.UpdateDate.ToString()
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
                                RegisterDate = user.RegisterDate.ToString(),
                                ExpireDate = user.ExpireDate.ToString(),
                                UpdateBy = user.UpdateBy,
                                UpdateDate = user.UpdateDate.ToString()
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
    }
}
