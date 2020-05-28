using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.BO.Interface
{
    public interface IDocumentDetail<T>
    {
        IEnumerable<T> GetList(int id);
        string Add(T item);
        string Update(T item);
        string Delete(string id);
    }
}
