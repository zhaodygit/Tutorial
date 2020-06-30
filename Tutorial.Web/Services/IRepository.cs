using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Web.Services
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        T GeById(int id);
        T Add(T newModel);
    }
}
