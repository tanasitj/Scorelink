using Scorelink.BO.Helper;
using Scorelink.MO;
using Scorelink.MO.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.BO.Repositories
{
    public class StatementTypeRepo
    {
        public StatementTypeModel Get(int id)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from stm in db.StatementTypes
                            where stm.StatementId == id
                            select new StatementTypeModel
                            {
                                StatementId = stm.StatementId,
                                StatementName = stm.StatementName,
                                Active = stm.Active,
                                CreateBy = stm.CreateBy,
                                CreateDate = stm.CreateDate.ToString(),
                                UpdateBy = stm.UpdateBy,
                                UpdateDate = stm.UpdateDate.ToString()
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
