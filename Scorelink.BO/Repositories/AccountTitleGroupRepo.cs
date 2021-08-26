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
    public class AccountTitleGroupRepo
    {
        public IEnumerable<AccountTitleGroupModel> GetAccountTitleGroupDD(string sLanguage)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var query = (from a in db.AccountTitleGroups
                             where a.Active == "Y" && a.AccGroupLanguage == sLanguage
                             select new AccountTitleGroupModel
                             {
                                 AccGroupId = a.AccGroupId,
                                 AccGroupName = a.AccGroupName
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


    }
}
