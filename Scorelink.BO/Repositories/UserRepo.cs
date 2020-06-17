using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                            Password = item.Password,
                            Email = item.Email,
                            Company = item.Company,
                            Telephone = item.Telephone,
                            Status = item.Status,
                            Admin = item.Admin,
                            RegisterDate = DateTime.Parse(item.RegisterDate),
                            ExpireDate = DateTime.Parse(item.ExpireDate),
                            UpdateBy = item.UpdateBy,
                            UpdateDate = DateTime.Parse(item.UpdateDate)
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
                        return ex.ToString();
                    }
                }
            }
        }
    }
}
