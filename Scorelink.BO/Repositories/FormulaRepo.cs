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
    public class FormulaRepo
    {
        public IEnumerable<FormulaModel> GetFormulaList(string userId,FormulaModel item)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var formula = (from f in db.Formula
                             where f.Active == "Y" && f.FormulaLanguage == item.FormulaLanguage
                             select new 
                             {
                                 FormulaId = f.FormulaId,
                                 FormulaName = f.FormulaName,
                                 FormulaDesc = f.FormulaDesc,
                                 FormulaQuery = f.FormulaQuery,
                                 FormulaLanguage = f.FormulaLanguage,
                                 StatementTypeId = f.StatementTypeId ?? 0,
                                 Active = f.Active
                             }).AsEnumerable().Select(x => new FormulaModel
                             {
                                 FormulaId = x.FormulaId,
                                 FormulaName = x.FormulaName,
                                 FormulaDesc = x.FormulaDesc,
                                 FormulaQuery = x.FormulaQuery,
                                 FormulaResult = GetFormulaResult(userId, x.FormulaQuery) ?? "",
                                 FormulaLanguage = x.FormulaLanguage,
                                 Active = x.Active
                             });
                return formula;
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        public string GetFormulaResult(string userId, string sSQL)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                string data = db.Database.SqlQuery<String>(string.Format(sSQL, userId)).FirstOrDefault();

                return data;
            }
            catch
            {
                return "";
            }
        }

    }
}
