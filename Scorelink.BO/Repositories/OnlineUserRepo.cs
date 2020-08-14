using Scorelink.BO.Helper;
using Scorelink.MO;
using Scorelink.MO.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.BO.Repositories
{
    public class OnlineUserRepo
    {
        public string Add(OnlineUserModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var online = new OnlineUser
                        {
                            UserId = item.UserId,
                            IPAddress = item.IPAddress,
                            SessionId = item.SessionId,
                            MACAddress = item.MACAddress,
                            CPUNO = item.CPUNO,
                            OnlineUpdate = DateTime.Now
                        };

                        db.OnlineUsers.Add(online);
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

        public string Update(OnlineUserModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var online = db.OnlineUsers.Where(x => x.UserId == item.UserId).First();
                        online.UserId = item.UserId;
                        online.IPAddress = item.IPAddress;
                        online.SessionId = item.SessionId;
                        online.MACAddress = item.MACAddress;
                        online.CPUNO = item.CPUNO;
                        online.OnlineUpdate = DateTime.Now;

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

        public string Delete(int userid)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var online = db.OnlineUsers.Where(x => x.UserId == userid).First();
                        db.OnlineUsers.Remove(online);

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

        public OnlineUserModel Get(int userid)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from online in db.OnlineUsers
                            where online.UserId == userid
                            select new OnlineUserModel
                            {
                                UserId = online.UserId ?? default(int),
                                IPAddress = online.IPAddress,
                                SessionId = online.SessionId,
                                MACAddress = online.MACAddress,
                                CPUNO = online.CPUNO,
                                OnlineUpdate = online.OnlineUpdate.ToString()
                            }).FirstOrDefault();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckTimeOut(OnlineUserModel item)
        {
            try
            {
                bool chk = false;
                int iTimeout = Common.getConstNum("TimeOut");

                using (ScorelinkEntities db = new ScorelinkEntities())
                    //Check existing Online User
                    if(db.OnlineUsers.Where(x => x.UserId == item.UserId).Any())
                    {
                        //Check User online time.
                        if(db.OnlineUsers.Where(x => x.UserId == item.UserId && DbFunctions.DiffMinutes(x.OnlineUpdate, DateTime.Now) < iTimeout).Any())
                        {
                            chk = true;
                        }
                        else
                        {
                            Update(item);
                        }
                    }
                    else
                    {
                        Add(item);
                    }

                return chk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getTimeRemain(OnlineUserModel item)
        {
            try
            {
                string mins = "0";
                int iNum = Common.getConstNum("TimeOut");

                using (ScorelinkEntities db = new ScorelinkEntities())
                    //Check existing Online User
                    if (db.OnlineUsers.Where(x => x.UserId == item.UserId).Any())
                    {
                        //Get Time remianing.
                        var data = db.OnlineUsers.Where(x => x.UserId == item.UserId).First();
                        mins = DbFunctions.DiffMinutes(data.OnlineUpdate, DateTime.Now).ToString();
                    }

                return mins;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckSession(OnlineUserModel item)
        {
            try
            {
                bool chk = false;
                int iNum = Common.getConstNum("TimeOut");

                using (ScorelinkEntities db = new ScorelinkEntities())
                    //Check existing Online User
                    if (db.OnlineUsers.Where(x => x.UserId == item.UserId).Any())
                    {
                        //Check Session ID.
                        if (db.OnlineUsers.Where(x => x.UserId == item.UserId && x.SessionId == item.SessionId).Any())
                        {
                            chk = true;
                        }
                    }

                return chk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
