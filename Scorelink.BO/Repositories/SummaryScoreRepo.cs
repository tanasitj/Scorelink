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
    public class SummaryScoreRepo
    {
        public string Add(SummaryScoreModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var sum = new SummaryScore
                        {
                            CreateBy = item.CreateBy,
                            AccGroupId = item.AccGroupId,
                            SumAmount1 = decimal.Parse(item.SumAmount1, System.Globalization.NumberStyles.Currency),
                            SumAmount2 = decimal.Parse(item.SumAmount2, System.Globalization.NumberStyles.Currency),
                            SumAmount3 = decimal.Parse(item.SumAmount3, System.Globalization.NumberStyles.Currency),
                            UpdateDate = DateTime.Now
                    };

                        db.SummaryScores.Add(sum);
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
        public string Update(SummaryScoreModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var sum = db.SummaryScores.Where(x => x.CreateBy == item.CreateBy && x.AccGroupId == item.AccGroupId).First();
                        
                        sum.SumAmount1 = sum.SumAmount1 + decimal.Parse(item.SumAmount1, System.Globalization.NumberStyles.Currency) ?? 0;
                        sum.SumAmount2 = sum.SumAmount2 + decimal.Parse(item.SumAmount2, System.Globalization.NumberStyles.Currency) ?? 0;
                        sum.SumAmount3 = sum.SumAmount3 + decimal.Parse(item.SumAmount3, System.Globalization.NumberStyles.Currency) ?? 0;
                        sum.UpdateDate = DateTime.Now;

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
        public string Delete(string userId)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
            {
                using (System.Data.Entity.DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var score = db.SummaryScores.Where(x => x.CreateBy == userId);
                        db.SummaryScores.RemoveRange(score);

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
        public bool CheckExist(SummaryScoreModel item)
        {
            using (ScorelinkEntities db = new ScorelinkEntities())
                return db.SummaryScores.Where(x => x.CreateBy == item.CreateBy && x.AccGroupId == item.AccGroupId).Any();

        }
    }
}
