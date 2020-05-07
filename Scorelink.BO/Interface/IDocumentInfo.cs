using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorelink.BO.Interface
{
    public interface IDocumentInfo<T>
    {
        IEnumerable<T> GetList(string id);
        string Add(T item);
        string Delete(string id);
    }
}
