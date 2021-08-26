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
    public class AccountTitleRepo
    {

        public AccountTitleModel GetAccountTitleId(string txt)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from a in db.AccountTitles
                            where a.Active == "Y" && a.AccTitleName == txt
                            select new AccountTitleModel
                            {
                                AccGroupId = a.AccGroupId ?? 0
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
