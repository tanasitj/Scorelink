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
    public class SysConfigRepo
    {
        public SysConfigModel Get(string consts)
        {
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from sys in db.SysConfigs
                            where sys.ConstName == consts
                            select new SysConfigModel
                            {
                                ConstId = sys.ConstId,
                                ConstName = sys.ConstName,
                                ConstOutputText = sys.ConstOutputText,
                                ConstOutputInt = sys.ConstOutputInt ?? default(int),
                                ConstOutputDouble = sys.ConstOutputDouble ?? default(decimal),
                                CreateBy = sys.CreateBy,
                                CreateDate = sys.CreateDate.ToString(),
                                UpdateBy = sys.UpdateBy,
                                UpdateDate = sys.UpdateDate.ToString()
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

        public string GetConstTxt(string consts)
        {
            string rst = "";
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from sys in db.SysConfigs
                            where sys.ConstName == consts
                            select new SysConfigModel
                            {
                                ConstOutputText = sys.ConstOutputText
                            }).FirstOrDefault();

                rst = data.ConstOutputText;
                return rst;
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                throw ex;
            }
        }

        public int GetConstNum(string consts)
        {
            int rst = 0;
            ScorelinkEntities db = new ScorelinkEntities();
            try
            {
                var data = (from sys in db.SysConfigs
                            where sys.ConstName == consts
                            select new SysConfigModel
                            {
                                ConstOutputInt = sys.ConstOutputInt ?? default(int)
                            }).FirstOrDefault();

                rst = data.ConstOutputInt;
                return rst;
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
